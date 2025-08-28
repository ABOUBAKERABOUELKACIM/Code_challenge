import React, { useState, useEffect } from 'react';
import { Plus, Filter } from 'lucide-react';
import { Button } from '../components/Common/Button';
import { Modal } from '../components/Common/Modal';
import { CalendarView } from '../components/Calendar/CalendarView';
import { AppointmentForm } from '../components/Forms/AppointmentForm';
import { apiService } from '../services/api';
import { Appointment } from '../types';
import { useAuth } from '../context/AuthContext';
import { format } from 'date-fns';

const Calendar: React.FC = () => {
  const { user } = useAuth();
  const [appointments, setAppointments] = useState<Appointment[]>([]);
  const [selectedDate, setSelectedDate] = useState<Date | undefined>();
  const [selectedDateAppointments, setSelectedDateAppointments] = useState<Appointment[]>([]);
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [isCreating, setIsCreating] = useState(false);

  useEffect(() => {
    loadAppointments();
  }, [user]);

  useEffect(() => {
    if (selectedDate) {
      const dateAppointments = appointments.filter(appointment =>
        new Date(appointment.dateTime).toDateString() === selectedDate.toDateString()
      );
      setSelectedDateAppointments(dateAppointments);
    }
  }, [selectedDate, appointments]);

  const loadAppointments = async () => {
    setIsLoading(true);
    try {
      const response = await apiService.getAppointments();
      let filteredData = response.data;

      // Filter based on user role
      if (user?.role === 'Doctor') {
        filteredData = response.data.filter(app => app.doctorId === user.id);
      } else if (user?.role === 'Assistant') {
        filteredData = response.data.filter(app => app.doctorId === user.doctorId);
      }

      setAppointments(filteredData);
    } catch (error) {
      console.error('Error loading appointments:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleCreateAppointment = async (appointmentData: Omit<Appointment, 'id' | 'createdAt' | 'doctorName'>) => {
    setIsCreating(true);
    try {
      await apiService.createAppointment(appointmentData);
      await loadAppointments();
      setIsCreateModalOpen(false);
    } catch (error) {
      console.error('Error creating appointment:', error);
    } finally {
      setIsCreating(false);
    }
  };

  const handleStatusUpdate = async (id: number, status: Appointment['status']) => {
    try {
      await apiService.updateAppointmentStatus(id, status);
      await loadAppointments();
    } catch (error) {
      console.error('Error updating appointment status:', error);
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
          <h1 className="text-2xl font-bold text-gray-900">Calendar</h1>
          <p className="text-gray-600 mt-1">View and manage appointments</p>
        </div>
        {(user?.role === 'Assistant' || user?.role === 'Admin') && (
          <Button
            leftIcon={<Plus className="w-4 h-4" />}
            onClick={() => setIsCreateModalOpen(true)}
          >
            New Appointment
          </Button>
        )}
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Calendar */}
        <div className="lg:col-span-2">
          <CalendarView
            appointments={appointments}
            onDateSelect={setSelectedDate}
            selectedDate={selectedDate}
          />
        </div>

        {/* Selected Date Details */}
        <div className="space-y-6">
          {selectedDate && (
            <div className="bg-white rounded-xl shadow-sm border border-gray-100">
              <div className="px-6 py-4 border-b border-gray-100">
                <h3 className="text-lg font-semibold text-gray-900">
                  {format(selectedDate, 'EEEE, MMMM d, yyyy')}
                </h3>
                <p className="text-sm text-gray-600 mt-1">
                  {selectedDateAppointments.length} appointment{selectedDateAppointments.length !== 1 ? 's' : ''}
                </p>
              </div>
              <div className="p-6">
                {selectedDateAppointments.length === 0 ? (
                  <p className="text-gray-500 text-center py-8">No appointments scheduled</p>
                ) : (
                  <div className="space-y-4">
                    {selectedDateAppointments.map((appointment) => (
                      <div
                        key={appointment.id}
                        className="p-4 bg-gray-50 rounded-lg"
                      >
                        <div className="flex items-start justify-between">
                          <div>
                            <p className="font-medium text-gray-900">
                              {appointment.patientName}
                            </p>
                            <p className="text-sm text-gray-600">
                              {format(new Date(appointment.dateTime), 'HH:mm')} - {appointment.duration} min
                            </p>
                            <p className="text-sm text-gray-600">
                              {appointment.doctorName}
                            </p>
                            {appointment.notes && (
                              <p className="text-sm text-gray-500 mt-2">
                                {appointment.notes}
                              </p>
                            )}
                          </div>
                          <div className="flex flex-col space-y-2">
                            <span className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full ${
                              appointment.status === 'Scheduled'
                                ? 'bg-blue-100 text-blue-800'
                                : appointment.status === 'Completed'
                                ? 'bg-green-100 text-green-800'
                                : 'bg-red-100 text-red-800'
                            }`}>
                              {appointment.status}
                            </span>
                            {appointment.status === 'Scheduled' && (
                              <div className="flex flex-col space-y-1">
                                <Button
                                  size="sm"
                                  variant="secondary"
                                  onClick={() => handleStatusUpdate(appointment.id, 'Completed')}
                                >
                                  Complete
                                </Button>
                                <Button
                                  size="sm"
                                  variant="outline"
                                  onClick={() => handleStatusUpdate(appointment.id, 'Cancelled')}
                                >
                                  Cancel
                                </Button>
                              </div>
                            )}
                          </div>
                        </div>
                      </div>
                    ))}
                  </div>
                )}
              </div>
            </div>
          )}

          {/* Quick Stats */}
          <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-6">
            <h3 className="text-lg font-semibold text-gray-900 mb-4">Quick Stats</h3>
            <div className="space-y-3">
              <div className="flex justify-between">
                <span className="text-gray-600">Total Appointments</span>
                <span className="font-medium">{appointments.length}</span>
              </div>
              <div className="flex justify-between">
                <span className="text-gray-600">Scheduled</span>
                <span className="font-medium text-blue-600">
                  {appointments.filter(a => a.status === 'Scheduled').length}
                </span>
              </div>
              <div className="flex justify-between">
                <span className="text-gray-600">Completed</span>
                <span className="font-medium text-green-600">
                  {appointments.filter(a => a.status === 'Completed').length}
                </span>
              </div>
              <div className="flex justify-between">
                <span className="text-gray-600">Cancelled</span>
                <span className="font-medium text-red-600">
                  {appointments.filter(a => a.status === 'Cancelled').length}
                </span>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Create Appointment Modal */}
      <Modal
        isOpen={isCreateModalOpen}
        onClose={() => setIsCreateModalOpen(false)}
        title="Create New Appointment"
        size="lg"
      >
        <AppointmentForm
          onSubmit={handleCreateAppointment}
          onCancel={() => setIsCreateModalOpen(false)}
          isLoading={isCreating}
        />
      </Modal>
    </div>
  );
};

export default Calendar;