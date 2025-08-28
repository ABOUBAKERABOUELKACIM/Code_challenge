import React from 'react';
import { NavLink } from 'react-router-dom';
import { 
  Home, 
  Calendar, 
  Users, 
  Building, 
  UserCog,
  ClipboardList,
  Settings,
  Stethoscope
} from 'lucide-react';
import { useAuth } from '../../context/AuthContext';

const Sidebar: React.FC = () => {
  const { user } = useAuth();

  const getMenuItems = () => {
    const baseItems = [
      { path: '/dashboard', icon: Home, label: 'Dashboard' },
      { path: '/appointments', icon: Calendar, label: 'Appointments' },
    ];

    if (user?.role === 'Admin') {
      return [
        ...baseItems,
        { path: '/offices', icon: Building, label: 'Offices' },
        { path: '/doctors', icon: Stethoscope, label: 'Doctors' },
        { path: '/assistants', icon: Users, label: 'Assistants' },
        { path: '/settings', icon: Settings, label: 'Settings' },
      ];
    }

    if (user?.role === 'Doctor') {
      return [
        ...baseItems,
        { path: '/assistants', icon: Users, label: 'My Assistants' },
        { path: '/schedule', icon: ClipboardList, label: 'Schedule' },
        { path: '/settings', icon: Settings, label: 'Settings' },
      ];
    }

    // Assistant menu
    return [
      ...baseItems,
      { path: '/schedule', icon: ClipboardList, label: 'Schedule' },
      { path: '/settings', icon: Settings, label: 'Settings' },
    ];
  };

  const menuItems = getMenuItems();

  return (
    <aside className="bg-white shadow-lg w-64 min-h-screen border-r border-gray-200">
      <div className="p-6 border-b border-gray-200">
        <div className="flex items-center space-x-3">
          <div className="w-10 h-10 bg-medical-blue rounded-lg flex items-center justify-center">
            <Stethoscope className="w-6 h-6 text-white" />
          </div>
          <div>
            <h1 className="text-xl font-bold text-gray-900">MedOffice</h1>
            <p className="text-sm text-gray-500">Management System</p>
          </div>
        </div>
      </div>

      <nav className="p-4">
        <ul className="space-y-2">
          {menuItems.map((item) => (
            <li key={item.path}>
              <NavLink
                to={item.path}
                className={({ isActive }) =>
                  `flex items-center space-x-3 px-4 py-3 rounded-lg transition-colors duration-200 ${
                    isActive
                      ? 'bg-medical-blue text-white shadow-md'
                      : 'text-gray-700 hover:bg-medical-lightBlue hover:text-medical-blue'
                  }`
                }
              >
                <item.icon className="w-5 h-5" />
                <span className="font-medium">{item.label}</span>
              </NavLink>
            </li>
          ))}
        </ul>
      </nav>

      {user && (
        <div className="absolute bottom-0 w-64 p-4 border-t border-gray-200 bg-white">
          <div className="flex items-center space-x-3">
            <div className="w-10 h-10 bg-medical-green rounded-full flex items-center justify-center">
              <UserCog className="w-5 h-5 text-white" />
            </div>
            <div>
              <p className="font-medium text-gray-900">
                {user.firstName} {user.lastName}
              </p>
              <p className="text-sm text-gray-500">{user.role}</p>
            </div>
          </div>
        </div>
      )}
    </aside>
  );
};

export default Sidebar;