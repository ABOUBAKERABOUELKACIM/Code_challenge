import React, { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { Button } from '../Common/Button';
import { apiService } from '../../services/api';
import { Doctor, Appointment } from '../../types';
import { useAuth } from '../../context/AuthContext';

interface AppointmentFormData {
  patientName: string;
  patientEmail: string;
  patientPhone: string;
  dateTime: string;
  duration: number;
  doctorId: number;
  notes?: string;
}

interface AppointmentFormProps {
  onSubmit: (appointment: Omit<Appointment, 'id' | 'createdAt' | 'doctorName'>) => void;
  onCancel: () => void;
  isLoading?: boolean;
}

export const AppointmentForm: React.FC<AppointmentFormProps> = ({
  onSubmit,
  onCancel,
  isLoading = false,
}) => {
  const { user } = useAuth();
  const [doctors, setDoctors] = useState<Doctor[]>([]);
  const [loadingDoctors, setLoadingDoctors] = useState(true);

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<AppointmentFormData>();

  useEffect(() => {
    loadDoctors();
  }, []);

  const loadDoctors = async () => {
    try {
      const response = await apiService.getDoctors();
      setDoctors(response.data);
    } catch (error) {
      console.error('Error loading doctors:', error);
    } finally {
      setLoadingDoctors(false);
    }
  };

  const handleFormSubmit = (data: AppointmentFormData) => {
    onSubmit({
      ...data,
      status: 'Scheduled',
      createdBy: user?.id || 1,
    });
    reset();
  };

  if (loadingDoctors) {
    return (
      <div className="flex items-center justify-center py-8">
        <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-medical-blue"></div>
      </div>
    );
  }

  return (
    <form onSubmit={handleSubmit(handleFormSubmit)} className="space-y-6">
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div>
          <label htmlFor="patientName" className="block text-sm font-medium text-gray-700 mb-2">
            Patient Name *
          </label>
          <input
            id="patientName"
            type="text"
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-medical-blue focus:border-medical-blue"
            placeholder="Enter patient name"
            {...register('patientName', {
              required: 'Patient name is required',
              minLength: {
                value: 2,
                message: 'Name must be at least 2 characters',
              },
            })}
          />
          {errors.patientName && (
            <p className="mt-1 text-sm text-red-600">{errors.patientName.message}</p>
          )}
        </div>

        <div>
          <label htmlFor="patientEmail" className="block text-sm font-medium text-gray-700 mb-2">
            Patient Email *
          </label>
          <input
            id="patientEmail"
            type="email"
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-medical-blue focus:border-medical-blue"
            placeholder="Enter patient email"
            {...register('patientEmail', {
              required: 'Email is required',
              pattern: {
                value: /^\S+@\S+$/i,
                message: 'Invalid email address',
              },
            })}
          />
          {errors.patientEmail && (
            <p className="mt-1 text-sm text-red-600">{errors.patientEmail.message}</p>
          )}
        </div>

        <div>
          <label htmlFor="patientPhone" className="block text-sm font-medium text-gray-700 mb-2">
            Patient Phone *
          </label>
          <input
            id="patientPhone"
            type="tel"
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-medical-blue focus:border-medical-blue"
            placeholder="(555) 123-4567"
            {...register('patientPhone', {
              required: 'Phone number is required',
              pattern: {
                value: /^\(\d{3}\) \d{3}-\d{4}$/,
                message: 'Phone must be in format (555) 123-4567',
              },
            })}
          />
          {errors.patientPhone && (
            <p className="mt-1 text-sm text-red-600">{errors.patientPhone.message}</p>
          )}
        </div>

        <div>
          <label htmlFor="doctorId" className="block text-sm font-medium text-gray-700 mb-2">
            Doctor *
          </label>
          <select
            id="doctorId"
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-medical-blue focus:border-medical-blue"
            {...register('doctorId', {
              required: 'Please select a doctor',
              valueAsNumber: true,
            })}
          >
            <option value="">Select a doctor</option>
            {doctors.map((doctor) => (
              <option key={doctor.id} value={doctor.id}>
                Dr. {doctor.firstName} {doctor.lastName} - {doctor.specialization}
              </option>
            ))}
          </select>
          {errors.doctorId && (
            <p className="mt-1 text-sm text-red-600">{errors.doctorId.message}</p>
          )}
        </div>

        <div>
          <label htmlFor="dateTime" className="block text-sm font-medium text-gray-700 mb-2">
            Date & Time *
          </label>
          <input
            id="dateTime"
            type="datetime-local"
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-medical-blue focus:border-medical-blue"
            {...register('dateTime', {
              required: 'Date and time is required',
            })}
          />
          {errors.dateTime && (
            <p className="mt-1 text-sm text-red-600">{errors.dateTime.message}</p>
          )}
        </div>

        <div>
          <label htmlFor="duration" className="block text-sm font-medium text-gray-700 mb-2">
            Duration (minutes) *
          </label>
          <select
            id="duration"
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-medical-blue focus:border-medical-blue"
            {...register('duration', {
              required: 'Duration is required',
              valueAsNumber: true,
            })}
          >
            <option value="">Select duration</option>
            <option value={15}>15 minutes</option>
            <option value={30}>30 minutes</option>
            <option value={45}>45 minutes</option>
            <option value={60}>60 minutes</option>
            <option value={90}>90 minutes</option>
          </select>
          {errors.duration && (
            <p className="mt-1 text-sm text-red-600">{errors.duration.message}</p>
          )}
        </div>
      </div>

      <div>
        <label htmlFor="notes" className="block text-sm font-medium text-gray-700 mb-2">
          Notes
        </label>
        <textarea
          id="notes"
          rows={3}
          className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-medical-blue focus:border-medical-blue"
          placeholder="Additional notes about the appointment..."
          {...register('notes')}
        />
      </div>

      <div className="flex justify-end space-x-4 pt-6 border-t border-gray-200">
        <Button
          type="button"
          variant="outline"
          onClick={onCancel}
          disabled={isLoading}
        >
          Cancel
        </Button>
        <Button
          type="submit"
          isLoading={isLoading}
        >
          Create Appointment
        </Button>
      </div>
    </form>
  );
};