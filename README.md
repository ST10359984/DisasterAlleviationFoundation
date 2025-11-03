Disaster Alleviation Foundation Web App

This is a prototype ASP.NET Core Razor Pages web application designed to streamline disaster reporting and resource donation for relief efforts. It includes secure role-based access, a clean user interface, and database-backed functionality.

Features Implemented

- Role-based login system (Admin, Donor, Volunteer)
- Disaster Reporting: Submit and view disaster incidents with type, location, severity, description, and date
- Donation System: Submit donations (food, clothing, money, etc.) and view all donations with donor info and resource details
- Navigation Integration: Accessible links to Home, Disaster Reports, and Donations
- Database Integration: Entity Framework Core with SQL Server, including migrations for DisasterReports and Donations

Setup Instructions

1. Clone the repository:  
   git clone https://dev.azure.com/your-org-name/DisasterAlleviationFoundation/_git/DAF-WebApp

2. Configure the database:  
   Update appsettings.json with your local SQL Server connection string.

3. Apply migrations:  
   Open Package Manager Console and run:  
   Add-Migration InitialSetup  
   Update-Database

4. Run the app:  
   Press F5 in Visual Studio or use dotnet run from the terminal.

Project Structure

- Models: DisasterReport.cs, Donation.cs  
- Data: ApplicationDbContext.cs  
- Pages:  
  - DisasterReports: Index.cshtml and Index.cshtml.cs  
  - Donations: Create.cshtml, Index.cshtml and their code-behind files  
  - Shared: _Layout.cshtml  
- wwwroot: Static files  
- appsettings.json: Configuration  
- Program.cs: App startup  
- README.md: Project documentation

Git Repository

Azure Repos:  
https://dev.azure.com/your-org-name/DisasterAlleviationFoundation/_git/DAF-WebApp

Branching Strategy

- main: stable prototype  
- feature/disaster-reporting: disaster reporting module  
- feature/donation-system: donation submission and viewing

Notes

This is a prototype. The final version will include edit/delete functionality, filtering, dashboards, and full role-based access control. All pages are built with Razor syntax and Bootstrap styling. Commit messages follow a descriptive format for traceability.

Let me know if you want a short project summary or pitch paragraph to include with your submission.
