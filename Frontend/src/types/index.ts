export interface User {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  role: 'Admin' | 'Doctor' | 'Assistant';
  officeId?: number;
  doctorId?: number;
  createdAt: string;
}

export interface Office {
  id: number;
  name: string;
  address: string;
  phone: string;
  email: string;
  createdAt: string;
  doctorsCount: number; 
}

export interface Doctor {
  id: number;
  firstName: string;
  lastName: string;
  specialization: string;
  email: string;
  phone: string;
  officeId: number;
  officeName: string;
  createdAt: string;
}

export interface Assistant {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  doctorId: number;
  doctorName: string;
  officeId: number;
  officeName: string;
  createdAt: string;
}

export interface Appointment {
  id: number;
  patientName: string;
  patientEmail: string;
  patientPhone: string;
  dateTime: string;
  duration: number;
  status: 'Scheduled' | 'Completed' | 'Cancelled' | 'No Show';
  doctorId: number;
  doctorName: string;
  notes?: string;
  createdBy: number;
  createdAt: string;
}

export interface ApiResponse<T> {
  data: T;
  message?: string;
  success: boolean;
}

export interface PaginatedResponse<T> extends ApiResponse<T[]> {
  totalCount: number;
  pageSize: number;
  currentPage: number;
  totalPages: number;
}