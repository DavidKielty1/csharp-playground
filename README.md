# GeekMeet

A modern social networking application built with ASP.NET Core and Angular, designed to connect people with similar interests.

## Features

- 🔐 Secure authentication using JWT tokens
- 👤 User profiles with customizable information
- 📸 Photo upload and management with Cloudinary integration
- 💬 Real-time messaging using SignalR
- ❤️ Like system for user connections
- 📱 Responsive design with modern UI
- 🔍 Advanced user search and filtering
- 🌐 Real-time online presence tracking

## Technical Stack

### Backend
- ASP.NET Core 8.0
- Entity Framework Core with SQLite
- Identity Framework for authentication
- SignalR for real-time features
- JWT Authentication
- AutoMapper for object mapping
- Cloudinary for cloud image storage

### Frontend
- Angular 17
- Bootstrap 5 with Bootswatch themes
- ngx-bootstrap components
- SignalR client
- ngx-toastr for notifications
- ngx-spinner for loading states
- ngx-timeago for time formatting
- ng-gallery for photo galleries

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- Node.js
- Angular CLI

### Running the Application

1. Clone the repository
2. Navigate to the API directory:
   ```bash
   cd API
   dotnet run
   ```
3. Navigate to the Client directory:
   ```bash
   cd Client
   ng serve
   ```
4. Access the application at `https://localhost:4200`

## Development

This project follows modern development practices including:
- Clean Architecture principles
- Repository pattern
- Dependency Injection
- Real-time communication
- Secure authentication
- Responsive design
- Error handling middleware
- Database migrations

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- Built with best practices in mind for scalability and maintainability
