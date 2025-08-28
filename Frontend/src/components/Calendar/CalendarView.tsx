import React, { useState, useEffect } from 'react';
import { ChevronLeft, ChevronRight, Calendar as CalendarIcon } from 'lucide-react';
import { Button } from '../Common/Button';
import { Appointment } from '../../types';
import { format, startOfMonth, endOfMonth, eachDayOfInterval, isSameDay, isToday } from 'date-fns';

interface CalendarViewProps {
  appointments: Appointment[];
  onDateSelect?: (date: Date) => void;
  selectedDate?: Date;
}

export const CalendarView: React.FC<CalendarViewProps> = ({
  appointments,
  onDateSelect,
  selectedDate,
}) => {
  const [currentDate, setCurrentDate] = useState(new Date());
  const [calendarDays, setCalendarDays] = useState<Date[]>([]);

  useEffect(() => {
    const start = startOfMonth(currentDate);
    const end = endOfMonth(currentDate);
    const days = eachDayOfInterval({ start, end });
    setCalendarDays(days);
  }, [currentDate]);

  const navigateMonth = (direction: 'prev' | 'next') => {
    setCurrentDate(prev => {
      const newDate = new Date(prev);
      if (direction === 'prev') {
        newDate.setMonth(prev.getMonth() - 1);
      } else {
        newDate.setMonth(prev.getMonth() + 1);
      }
      return newDate;
    });
  };

  const getAppointmentsForDate = (date: Date) => {
    return appointments.filter(appointment =>
      isSameDay(new Date(appointment.dateTime), date)
    );
  };

  const handleDateClick = (date: Date) => {
    if (onDateSelect) {
      onDateSelect(date);
    }
  };

  return (
    <div className="bg-white rounded-xl shadow-sm border border-gray-100">
      <div className="px-6 py-4 border-b border-gray-100">
        <div className="flex items-center justify-between">
          <h3 className="text-lg font-semibold text-gray-900">
            {format(currentDate, 'MMMM yyyy')}
          </h3>
          <div className="flex items-center space-x-2">
            <Button
              variant="outline"
              size="sm"
              onClick={() => navigateMonth('prev')}
            >
              <ChevronLeft className="w-4 h-4" />
            </Button>
            <Button
              variant="outline"
              size="sm"
              onClick={() => setCurrentDate(new Date())}
            >
              Today
            </Button>
            <Button
              variant="outline"
              size="sm"
              onClick={() => navigateMonth('next')}
            >
              <ChevronRight className="w-4 h-4" />
            </Button>
          </div>
        </div>
      </div>

      <div className="p-6">
        {/* Calendar Header */}
        <div className="grid grid-cols-7 gap-1 mb-4">
          {['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'].map(day => (
            <div
              key={day}
              className="p-2 text-center text-sm font-medium text-gray-500"
            >
              {day}
            </div>
          ))}
        </div>

        {/* Calendar Grid */}
        <div className="grid grid-cols-7 gap-1">
          {calendarDays.map(date => {
            const dayAppointments = getAppointmentsForDate(date);
            const isSelected = selectedDate && isSameDay(date, selectedDate);
            const isTodayDate = isToday(date);

            return (
              <div
                key={date.toISOString()}
                className={`
                  relative p-2 min-h-[80px] border border-gray-100 rounded-lg cursor-pointer
                  hover:bg-gray-50 transition-colors
                  ${isSelected ? 'bg-medical-lightBlue border-medical-blue' : ''}
                  ${isTodayDate ? 'bg-blue-50 border-blue-200' : ''}
                `}
                onClick={() => handleDateClick(date)}
              >
                <div className={`
                  text-sm font-medium mb-1
                  ${isTodayDate ? 'text-medical-blue' : 'text-gray-900'}
                `}>
                  {format(date, 'd')}
                </div>

                {dayAppointments.length > 0 && (
                  <div className="space-y-1">
                    {dayAppointments.slice(0, 2).map(appointment => (
                      <div
                        key={appointment.id}
                        className={`
                          text-xs px-2 py-1 rounded truncate
                          ${appointment.status === 'Scheduled'
                            ? 'bg-blue-100 text-blue-800'
                            : appointment.status === 'Completed'
                            ? 'bg-green-100 text-green-800'
                            : 'bg-red-100 text-red-800'
                          }
                        `}
                        title={`${appointment.patientName} - ${format(new Date(appointment.dateTime), 'HH:mm')}`}
                      >
                        {format(new Date(appointment.dateTime), 'HH:mm')} {appointment.patientName}
                      </div>
                    ))}
                    {dayAppointments.length > 2 && (
                      <div className="text-xs text-gray-500 px-2">
                        +{dayAppointments.length - 2} more
                      </div>
                    )}
                  </div>
                )}
              </div>
            );
          })}
        </div>
      </div>

      {/* Legend */}
      <div className="px-6 py-4 border-t border-gray-100">
        <div className="flex items-center justify-center space-x-6 text-sm">
          <div className="flex items-center space-x-2">
            <div className="w-3 h-3 bg-blue-100 rounded"></div>
            <span className="text-gray-600">Scheduled</span>
          </div>
          <div className="flex items-center space-x-2">
            <div className="w-3 h-3 bg-green-100 rounded"></div>
            <span className="text-gray-600">Completed</span>
          </div>
          <div className="flex items-center space-x-2">
            <div className="w-3 h-3 bg-red-100 rounded"></div>
            <span className="text-gray-600">Cancelled</span>
          </div>
        </div>
      </div>
    </div>
  );
};