import React from 'react';
import { useAuth } from '../context/AuthContext';
import AdminDashboard from '../components/Dashboard/AdminDashboard';
import DoctorDashboard from '../components/Dashboard/DoctorDashboard';
import AssistantDashboard from '../components/Dashboard/AssistantDashboard';

const Dashboard: React.FC = () => {
  const { user } = useAuth();

  if (!user) return null;

  switch (user.role) {
    case 'Admin':
      return <AdminDashboard />;
    case 'Doctor':
      return <DoctorDashboard />;
    case 'Assistant':
      return <AssistantDashboard />;
    default:
      return <div>Invalid user role</div>;
  }
};

export default Dashboard;