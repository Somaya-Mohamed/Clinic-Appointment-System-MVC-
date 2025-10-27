Clinic Assistant
A simple Clinic Management System (MVP) built with ASP.NET Core MVC, Entity Framework Core, SQL Server, and Bootstrap 5.
Features

CRUD operations for Doctors, Patients, and Appointments.
View patient appointment history.
Filter appointments by doctor or patient.
Prevent scheduling conflicting appointments for the same doctor.
Dashboard with counts of doctors, patients, and upcoming appointments.

Prerequisites

.NET 8.0 SDK
SQL Server (LocalDB or full SQL Server)
Visual Studio 2022 or later (or any IDE that supports .NET)

Setup Instructions

Clone the repository:
git clone [https://github.com/your-repo/ClinicAssistant.git](https://github.com/Somaya-Mohamed/Clinic-Appointment-System)
cd ClinicAssistant


Update the connection string:

Open appsettings.json and update the DefaultConnection string to match your SQL Server setup.

"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=ClinicDb;Trusted_Connection=True;TrustServerCertificate=True;"
}


Apply migrations:
dotnet ef migrations add InitialCreate
dotnet ef database update


Run the application:
dotnet run

Or open the solution in Visual Studio and press F5.

Access the application:

Open your browser and navigate to https://localhost:5001 (or the port assigned by your setup).
The dashboard will display counts of doctors, patients, and upcoming appointments.



Seed Data

3 Doctors
5 Patients
3 Appointments

Testing Checklist

Create a new doctor → Appears in the Doctors list.
Create a new patient → Appears in the Patients list.
Create an appointment → Appears in the Appointments list.
Try to create a conflicting appointment → Error message displayed.
View patient history → Shows all appointments in chronological order.

Notes

To enable DataTables for tables, uncomment the DataTables initialization in site.js and add the DataTables CDN in _Layout.cshtml.
Bootstrap 5 is included via CDN for simplicity.

ERD
The Entity-Relationship Diagram (ERD) is available in the project documentation or can be generated using tools like dbdiagram.io.
