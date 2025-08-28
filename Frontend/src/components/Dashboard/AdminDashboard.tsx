import React, { useState, useEffect } from 'react';
import { Calendar, Users, Building, Stethoscope, TrendingUp, Clock, Plus, Edit, Trash2 } from 'lucide-react';
import { StatCard } from '../Common/Card';
import { Button } from '../Common/Button';
import { Modal } from '../Common/Modal';
import { apiService } from '../../services/api';
import { Office, Doctor, Assistant, Appointment } from '../../types';

interface CreateOfficeFormData {
  name: string;
  address: string;
  phone: string;
  // Remove email - it's not needed for office creation
}

const AdminDashboard: React.FC = () => {
  const [stats, setStats] = useState({
    totalOffices: 0,
    totalDoctors: 0,
    totalAssistants: 0,
    totalAppointments: 0,
    todayAppointments: 0,
    completedAppointments: 0,
  });
  const [recentAppointments, setRecentAppointments] = useState<Appointment[]>([]);
  const [offices, setOffices] = useState<Office[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [showCreateOffice, setShowCreateOffice] = useState(false);
  const [isCreating, setIsCreating] = useState(false);
  const [newOffice, setNewOffice] = useState<CreateOfficeFormData>({
    name: '',
    address: '',
    phone: '',
  });

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    setIsLoading(true);
    try {
      const [officesRes, doctorsRes, assistantsRes, appointmentsRes] = await Promise.all([
        apiService.getOffices(),
        apiService.getDoctors(),
        apiService.getAssistants(),
        apiService.getAppointments(),
      ]);

      const today = new Date().toDateString();
      const todayAppointments = appointmentsRes.data.filter(
        app => new Date(app.dateTime).toDateString() === today
      );
      const completedAppointments = appointmentsRes.data.filter(
        app => app.status === 'Completed'
      );

      setStats({
        totalOffices: officesRes.data.length,
        totalDoctors: doctorsRes.data.length,
        totalAssistants: assistantsRes.data.length,
        totalAppointments: appointmentsRes.data.length,
        todayAppointments: todayAppointments.length,
        completedAppointments: completedAppointments.length,
      });

      setOffices(officesRes.data);
      setRecentAppointments(appointmentsRes.data.slice(0, 5));
    } catch (error) {
      console.error('Error loading dashboard data:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleCreateOffice = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsCreating(true);

    try {
      const result = await apiService.createOffice(newOffice);
      if (result.success) {
        setOffices([...offices, result.data]);
        setStats(prev => ({ ...prev, totalOffices: prev.totalOffices + 1 }));
        setShowCreateOffice(false);
        setNewOffice({ name: '', address: '', phone: '' });
      }
    } catch (error) {
      console.error('Error creating office:', error);
      alert('Failed to create office. Please try again.');
    } finally {
      setIsCreating(false);
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
          <h1 className="text-2xl font-bold text-gray-900">Admin Dashboard</h1>
          <p className="text-gray-600 mt-1">Monitor and manage your healthcare system</p>
        </div>
        <Button onClick={() => setShowCreateOffice(true)}>
          <Plus className="w-4 h-4 mr-2" />
          New Office
        </Button>
      </div>

      {/* Stats Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <StatCard
          title="Total Offices"
          value={stats.totalOffices}
          icon={<Building className="w-6 h-6" />}
          color="blue"
        />
        <StatCard
          title="Total Doctors"
          value={stats.totalDoctors}
          icon={<Stethoscope className="w-6 h-6" />}
          color="green"
        />
        <StatCard
          title="Total Assistants"
          value={stats.totalAssistants}
          icon={<Users className="w-6 h-6" />}
          color="blue"
        />
        <StatCard
          title="Total Appointments"
          value={stats.totalAppointments}
          icon={<Calendar className="w-6 h-6" />}
          color="green"
        />
      </div>

      {/* Today's Overview */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <StatCard
          title="Today's Appointments"
          value={stats.todayAppointments}
          icon={<Clock className="w-6 h-6" />}
          color="yellow"
        />
        <StatCard
          title="Completed Appointments"
          value={stats.completedAppointments}
          icon={<TrendingUp className="w-6 h-6" />}
          color="green"
        />
        <StatCard
          title="Success Rate"
          value={`${Math.round((stats.completedAppointments / stats.totalAppointments) * 100) || 0}%`}
          icon={<TrendingUp className="w-6 h-6" />}
          color="green"
        />
      </div>

      {/* Office Management */}
      <div className="bg-white rounded-xl shadow-sm border border-gray-100">
        <div className="px-6 py-4 border-b border-gray-100">
          <h3 className="text-lg font-semibold text-gray-900">Office Management</h3>
          <p className="text-sm text-gray-600 mt-1">Manage your medical offices</p>
        </div>
        <div className="p-6">
          {offices.length === 0 ? (
            <div className="text-center py-8">
              <Building className="w-12 h-12 text-gray-400 mx-auto mb-4" />
              <p className="text-gray-500">No offices found</p>
              <Button 
                onClick={() => setShowCreateOffice(true)}
                className="mt-4"
              >
                Create First Office
              </Button>
            </div>
          ) : (
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
              {offices.map((office) => (
                <div
                  key={office.id}
                  className="p-4 border border-gray-200 rounded-lg hover:border-medical-blue transition-colors"
                >
                  <div className="flex items-start justify-between">
                    <div className="flex-1">
                      <h4 className="font-medium text-gray-900">{office.name}</h4>
                      <p className="text-sm text-gray-600 mt-1">{office.address}</p>
                      <p className="text-sm text-gray-600">{office.phone}</p>
                      <p className="text-xs text-gray-500 mt-2">
                        {office.doctorsCount || 0} doctors
                      </p>
                    </div>
                    <div className="flex space-x-2 ml-4">
                      <button className="p-1 text-gray-400 hover:text-medical-blue">
                        <Edit className="w-4 h-4" />
                      </button>
                      <button className="p-1 text-gray-400 hover:text-red-500">
                        <Trash2 className="w-4 h-4" />
                      </button>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>

      {/* Recent Appointments */}
      <div className="bg-white rounded-xl shadow-sm border border-gray-100">
        <div className="px-6 py-4 border-b border-gray-100">
          <h3 className="text-lg font-semibold text-gray-900">Recent Appointments</h3>
          <p className="text-sm text-gray-600 mt-1">Latest scheduled appointments</p>
        </div>
        <div className="p-6">
          {recentAppointments.length === 0 ? (
            <p className="text-gray-500 text-center py-8">No appointments found</p>
          ) : (
            <div className="space-y-4">
              {recentAppointments.map((appointment) => (
                <div
                  key={appointment.id}
                  className="flex items-center justify-between p-4 bg-gray-50 rounded-lg hover:bg-gray-100 transition-colors"
                >
                  <div>
                    <p className="font-medium text-gray-900">{appointment.patientName}</p>
                    <p className="text-sm text-gray-600">{appointment.doctorName}</p>
                  </div>
                  <div className="text-right">
                    <p className="text-sm font-medium text-gray-900">
                      {new Date(appointment.dateTime).toLocaleDateString()}
                    </p>
                    <span className={`inline-flex px-2 py-1 text-xs font-medium rounded-full ${
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

      {/* Create Office Modal */}
      {showCreateOffice && (
        <Modal
          isOpen={showCreateOffice}
          onClose={() => setShowCreateOffice(false)}
          title="Create New Office"
        >
          <form onSubmit={handleCreateOffice} className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Office Name *
              </label>
              <input
                type="text"
                required
                value={newOffice.name}
                onChange={(e) => setNewOffice({ ...newOffice, name: e.target.value })}
                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-medical-blue focus:border-transparent"
                placeholder="Enter office name"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Address *
              </label>
              <textarea
                required
                value={newOffice.address}
                onChange={(e) => setNewOffice({ ...newOffice, address: e.target.value })}
                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-medical-blue focus:border-transparent"
                rows={3}
                placeholder="Enter office address"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Phone *
              </label>
              <input
                type="tel"
                required
                value={newOffice.phone}
                onChange={(e) => setNewOffice({ ...newOffice, phone: e.target.value })}
                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-medical-blue focus:border-transparent"
                placeholder="Enter phone number"
              />
            </div>
            <div className="flex justify-end space-x-3 pt-4">
              <Button
                type="button"
                variant="outline"
                onClick={() => setShowCreateOffice(false)}
              >
                Cancel
              </Button>
              <Button
                type="submit"
                isLoading={isCreating}
              >
                Create Office
              </Button>
            </div>
          </form>
        </Modal>
      )}
    </div>
  );
};

export default AdminDashboard;