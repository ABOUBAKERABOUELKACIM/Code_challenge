import { 
  Office, 
  Doctor, 
  Assistant, 
  Appointment, 
  ApiResponse, 
} from '../types';

const API_BASE_URL = 'http://localhost:32769/api';

class ApiService {
  private getAuthHeaders(): HeadersInit {
    const token = localStorage.getItem('token');
    return {
      'Content-Type': 'application/json',
      ...(token && { Authorization: `Bearer ${token}` }),
    };
  }

  private async request<T>(endpoint: string, options: RequestInit = {}): Promise<T> {
    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      headers: this.getAuthHeaders(),
      ...options,
    });

    if (!response.ok) {
      if (response.status === 401) {
        localStorage.removeItem('token');
        localStorage.removeItem('role');
        window.location.href = '/login';
      }
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    return response.json();
  }

  // Authentication
  async login(email: string, password: string): Promise<{ token: string; role: string; fullName: string; userId: number }> {
    const response = await fetch(`${API_BASE_URL}/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password }),
    });

    if (!response.ok) throw new Error('Login failed');
    
    const data = await response.json();
    localStorage.setItem('token', data.token);
    localStorage.setItem('role', data.role);
    return data;
  }

  // Offices
  async getOffices(): Promise<ApiResponse<Office[]>> {
    try {
      const data = await this.request<Office[]>('/office');
      return { data, success: true };
    } catch (error) {
      return { data: [], success: false, message: (error as Error).message };
    }
  }

  async createOffice(office: { name: string; address: string; phone: string }): Promise<ApiResponse<Office>> {
  try {
    const data = await this.request<Office>('/office', {
      method: 'POST',
      body: JSON.stringify({
        name: office.name,
        address: office.address,
        phone: office.phone,
      }),
    });
    return { data, success: true };
  } catch (error) {
    return { data: {} as Office, success: false, message: (error as Error).message };
  }
}

  // Doctors
  async getDoctors(): Promise<ApiResponse<Doctor[]>> {
    try {
      const data = await this.request<Doctor[]>('/doctor');
      return { data, success: true };
    } catch (error) {
      return { data: [], success: false, message: (error as Error).message };
    }
  }

  async createDoctor(doctor: Omit<Doctor, 'id' | 'createdAt' | 'officeName'>): Promise<ApiResponse<Doctor>> {
    try {
      const data = await this.request<Doctor>('/doctor', {
        method: 'POST',
        body: JSON.stringify({
          firstName: doctor.firstName,
          lastName: doctor.lastName,
          specialization: doctor.specialization,
          email: doctor.email,
          password: 'TempPassword123!', // You should get this from a form
          officeId: doctor.officeId,
        }),
      });
      return { data, success: true };
    } catch (error) {
      return { data: {} as Doctor, success: false, message: (error as Error).message };
    }
  }

  // Assistants
  async getAssistants(): Promise<ApiResponse<Assistant[]>> {
    try {
      const data = await this.request<Assistant[]>('/assistant');
      return { data, success: true };
    } catch (error) {
      return { data: [], success: false, message: (error as Error).message };
    }
  }

  async createAssistant(assistant: Omit<Assistant, 'id' | 'createdAt' | 'doctorName' | 'officeName'>): Promise<ApiResponse<Assistant>> {
    try {
      const data = await this.request<Assistant>('/assistant', {
        method: 'POST',
        body: JSON.stringify({
          firstName: assistant.firstName,
          lastName: assistant.lastName,
          email: assistant.email,
          password: 'TempPassword123!', // You should get this from a form
          doctorId: assistant.doctorId,
        }),
      });
      return { data, success: true };
    } catch (error) {
      return { data: {} as Assistant, success: false, message: (error as Error).message };
    }
  }

  // Patients
  async getPatients(): Promise<ApiResponse<any[]>> {
    try {
      const data = await this.request<any[]>('/patient');
      return { data, success: true };
    } catch (error) {
      return { data: [], success: false, message: (error as Error).message };
    }
  }

  async createPatient(patient: any): Promise<ApiResponse<any>> {
    try {
      const data = await this.request<any>('/patient', {
        method: 'POST',
        body: JSON.stringify({
          firstName: patient.firstName,
          lastName: patient.lastName,
          email: patient.email,
          phone: patient.phone,
        }),
      });
      return { data, success: true };
    } catch (error) {
      return { data: {}, success: false, message: (error as Error).message };
    }
  }

  // Appointments
  async getAppointments(): Promise<ApiResponse<Appointment[]>> {
    try {
      const data = await this.request<any[]>('/appointment');
      // Map .NET DTO to frontend format
      const appointments = data.map(apt => ({
        id: apt.id,
        patientName: apt.patientName,
        patientEmail: apt.patientEmail || '',
        patientPhone: apt.patientPhone || '',
        dateTime: apt.appointmentDateTime,
        duration: 30, // Default duration
        status: apt.status,
        doctorId: apt.doctorId,
        doctorName: apt.doctorName,
        notes: apt.notes,
        createdBy: apt.createdByAssistantId,
        createdAt: apt.createdAt,
      }));
      return { data: appointments, success: true };
    } catch (error) {
      return { data: [], success: false, message: (error as Error).message };
    }
  }

  async createAppointment(appointment: Omit<Appointment, 'id' | 'createdAt' | 'doctorName'>): Promise<ApiResponse<Appointment>> {
    try {
      const data = await this.request<any>('/appointment', {
        method: 'POST',
        body: JSON.stringify({
          appointmentDateTime: appointment.dateTime,
          notes: appointment.notes,
          doctorId: appointment.doctorId,
          patientId: 1, // You'll need to get actual patient ID
          createdByAssistantId: appointment.createdBy,
        }),
      });

      const mappedAppointment: Appointment = {
        id: data.id,
        patientName: data.patientName,
        patientEmail: data.patientEmail || '',
        patientPhone: data.patientPhone || '',
        dateTime: data.appointmentDateTime,
        duration: 30,
        status: data.status,
        doctorId: data.doctorId,
        doctorName: data.doctorName,
        notes: data.notes,
        createdBy: data.createdByAssistantId,
        createdAt: data.createdAt,
      };

      return { data: mappedAppointment, success: true };
    } catch (error) {
      return { data: {} as Appointment, success: false, message: (error as Error).message };
    }
  }

  async updateAppointmentStatus(id: number, status: Appointment['status']): Promise<ApiResponse<Appointment>> {
    try {
      const data = await this.request<any>(`/appointment/${id}/status`, {
        method: 'PUT',
        body: JSON.stringify({ status }),
      });

      const mappedAppointment: Appointment = {
        id: data.id,
        patientName: data.patientName,
        patientEmail: data.patientEmail || '',
        patientPhone: data.patientPhone || '',
        dateTime: data.appointmentDateTime,
        duration: 30,
        status: data.status,
        doctorId: data.doctorId,
        doctorName: data.doctorName,
        notes: data.notes,
        createdBy: data.createdByAssistantId,
        createdAt: data.createdAt,
      };

      return { data: mappedAppointment, success: true };
    } catch (error) {
      return { data: {} as Appointment, success: false, message: (error as Error).message };
    }
  }
}

export const apiService = new ApiService();