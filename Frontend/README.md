# Doctor Office Management System

A modern React TypeScript application for managing doctor offices, appointments, and staff with role-based access control.

## Features

- **Role-Based Dashboards**: Different interfaces for Admin, Doctor, and Assistant roles
- **Appointment Management**: Create, view, and manage patient appointments
- **Staff Management**: Manage doctors and assistants across multiple offices
- **Office Management**: Admin can create and manage multiple medical offices
- **Professional UI**: Clean, medical-themed design with blue/green/white color palette
- **Responsive Design**: Works seamlessly on desktop and tablet devices

## Tech Stack

- React 18 with TypeScript
- Tailwind CSS for styling
- React Router for navigation
- React Hook Form for form handling
- Lucide React for icons
- Date-fns for date manipulation

## Getting Started

1. Install dependencies:
```bash
npm install
```

2. Start the development server:
```bash
npm run dev
```

3. Open your browser and navigate to the local development URL

## Demo Accounts

Use these credentials to test different user roles:

- **Admin**: admin@medoffice.com / password
- **Doctor**: doctor@medoffice.com / password  
- **Assistant**: assistant@medoffice.com / password

## Project Structure

```
src/
├── components/
│   ├── Auth/           # Authentication components
│   ├── Common/         # Reusable UI components
│   ├── Dashboard/      # Role-specific dashboards
│   └── Layout/         # Layout components
├── context/            # React context providers
├── pages/              # Page components
├── services/           # API services
├── types/              # TypeScript type definitions
└── App.tsx             # Main application component
```

## API Integration

The application is designed to work with a .NET API with the following endpoints:

- `GET/POST /api/offices` - Office management
- `GET/POST /api/doctors` - Doctor management
- `GET/POST /api/assistants` - Assistant management
- `GET/POST /api/appointments` - Appointment management
- `PUT /api/appointments/{id}/status` - Update appointment status

Currently uses mock data for development and testing.

## Color Palette

- **Primary Blue**: #2563EB
- **Primary Green**: #059669
- **Light Blue**: #DBEAFE
- **Light Green**: #D1FAE5
- **White**: #FFFFFF
- **Gray**: #F8FAFC

## License

This project is for educational and demonstration purposes.