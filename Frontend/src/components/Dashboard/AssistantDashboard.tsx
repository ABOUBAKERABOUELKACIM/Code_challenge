import React, { useState, useEffect } from 'react';
import { Calendar, Clock, CheckCircle, Plus } from 'lucide-react';
import { StatCard } from '../Common/Card';
import { Button } from '../Common/Button';
import { apiService } from '../../services/api';
import { Appointment } from '../../types';
import { useAuth } from '../../context/AuthContext';

const AssistantDashboard: React.FC = () => {
  const { user } = useAuth();
  const [stats, setStats] = useState({
    totalAppointments: 0,
    todayAppointments: 0,
    completedAppointments: 0,
    upcomingAppointments: 0,
  });
  const [recentAppointments, setRecentAppointments] = useState<Appointment[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    loadDashboardData();
  }, [user]);

  const loadDashboardData = async () => {
    if (!user) return;
    
    setIsLoading(true);
    try {
      const appointmentsRes = await apiService.getAppointments();
      
      // Filter appointments for the doctor this assistant works for
      const assistantAppointments = appointmentsRes.data.filter(
        app => app.doctorId === user.doctorId
      );

      const today = new Date();
      const todayString = today.toDateString();
      
      const todayAppts = assistantAppointments.filter(
        app => new Date(app.dateTime).toDateString() === todayString
      );
      
      const completedAppts = assistantAppointments.filter(
        app => app.status === 'Completed'
      );
      
      const upcomingAppts = assistantAppointments.filter(
        app => new Date(app.dateTime) > today && app.status === 'Scheduled'
      );

      setStats({
        totalAppointments: assistantAppointments.length,
        todayAppointments: todayAppts.length,
        completedAppointments: completedAppts.length,
        upcomingAppointments: upcomingAppts.length,
      });

      setRecentAppointments(assistantAppointments.slice(0, 5));
    } catch (error) {
      console.error('Error loading dashboard data:', error);
    } finally {
      setIsLoading(false);
    }
  };

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-medical-blue"></div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Assistant Dashboard</h1>
          <p className="text-gray-600 mt-1">Manage appointments and schedules</p>
        </div>
        <Button leftIcon={<Plus className="w-4 h-4" />}>
          New Appointment
        </Button>
      </div>

      {/* Stats Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <StatCard
          title="Total Appointments"
          value={stats.totalAppointments}
          icon={<Calendar className="w-6 h-6" />}
          color="blue"
        />
        <StatCard
          title="Today's Appointments"
          value={stats.todayAppointments}
          icon={<Clock className="w-6 h-6" />}
          color="yellow"
        />
        <StatCard
          title="Completed"
          value={stats.completedAppointments}
          icon={<CheckCircle className="w-6 h-6" />}
          color="green"
        />
        <StatCard
          title="Upcoming"
          value={stats.upcomingAppointments}
          icon={<Calendar className="w-6 h-6" />}
          color="blue"
        />
      </div>

      {/* Recent Appointments */}
      <div className="bg-white rounded-xl shadow-sm border border-gray-100">
        <div className="px-6 py-4 border-b border-gray-100">
          <div className="flex items-center justify-between">
            <div>
              <h3 className="text-lg font-semibold text-gray-900">Recent Appointments</h3>
              <p className="text-sm text-gray-600 mt-1">Latest appointments you've managed</p>
            </div>
            <Button variant="outline" size="sm">View All</Button>
          </div>
        </div>
        <div className="p-6">
          {recentAppointments.length === 0 ? (
            <div className="text-center py-8">
              <Calendar className="w-12 h-12 text-gray-400 mx-auto mb-4" />
              <p className="text-gray-500">No appointments found</p>
            </div>
          ) : (
            <div className="space-y-4">
              {recentAppointments.map((appointment) => (
                <div
                  key={appointment.id}
                  className="flex items-center justify-between p-4 bg-gray-50 rounded-lg hover:bg-gray-100 transition-colors"
                >
                  <div>
                    <p className="font-medium text-gray-900">{appointment.patientName}</p>
                    <p className="text-sm text-gray-600">{appointment.patientPhone}</p>
                    {appointment.notes && (
                      <p className="text-sm text-gray-500 mt-1">{appointment.notes}</p>
                    )}
                  </div>
                  <div className="text-right">
                    <p className="text-sm font-medium text-gray-900">
                      {new Date(appointment.dateTime).toLocaleDateString()}
                    </p>
                    <p className="text-xs text-gray-500">
                      {new Date(appointment.dateTime).toLocaleTimeString([], {
                        hour: '2-digit',
                        minute: '2-digit',
                      })}
                    </p>
                    <span className={`inline-flex px-2 py-1 text-xs font-medium rounded-full mt-1 ${
                      appointment.status === 'Scheduled'
                        ? 'bg-blue-100 text-blue-800'
                        : appointment.status === 'Completed'
                        ? 'bg-green-100 text-green-800'
                        : 'bg-red-100 text-red-800'
                    }`}>
                      {appointment.status}
                    </span>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default AssistantDashboard;