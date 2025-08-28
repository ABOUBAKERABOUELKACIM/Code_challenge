import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { User } from '../types';

interface AuthContextType {
  user: User | null;
  login: (email: string, password: string) => Promise<boolean>;
  logout: () => void;
  isLoading: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    // Check for stored auth data
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      try {
        setUser(JSON.parse(storedUser));
      } catch (error) {
        console.error('Error parsing stored user data:', error);
        localStorage.removeItem('user');
      }
    }
    setIsLoading(false);
  }, []);

  const login = async (email: string, password: string): Promise<boolean> => {
    setIsLoading(true);
    try {
      // Mock login - in real app, this would call your API
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      // Mock user data based on email for demo
      let mockUser: User;
      if (email.includes('admin')) {
        mockUser = {
          id: 1,
          firstName: 'John',
          lastName: 'Admin',
          email: email,
          role: 'Admin',
          createdAt: new Date().toISOString(),
        };
      } else if (email.includes('doctor')) {
        mockUser = {
          id: 2,
          firstName: 'Dr. Sarah',
          lastName: 'Johnson',
          email: email,
          role: 'Doctor',
          officeId: 1,
          createdAt: new Date().toISOString(),
        };
      } else {
        mockUser = {
          id: 3,
          firstName: 'Mary',
          lastName: 'Assistant',
          email: email,
          role: 'Assistant',
          officeId: 1,
          doctorId: 2,
          createdAt: new Date().toISOString(),
        };
      }

      setUser(mockUser);
      localStorage.setItem('user', JSON.stringify(mockUser));
      return true;
    } catch (error) {
      console.error('Login error:', error);
      return false;
    } finally {
      setIsLoading(false);
    }
  };

  const logout = () => {
    setUser(null);
    localStorage.removeItem('user');
  };

  return (
    <AuthContext.Provider value={{ user, login, logout, isLoading }}>
      {children}
    </AuthContext.Provider>
  );
};