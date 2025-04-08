# WebMobiTask1LoginRegister

This is an ASP.NET Core MVC application that supports login and registration functionality using Entity Framework Core (Code-First).  
The project includes a full SQL script containing both the **schema and sample data**.

---

## 🔧 Project Setup Guide

You can set up the database in two ways depending on your preference:

---

## 📌 Option 1: Use Predefined SQL Script (Schema + Data)

If you want the database with complete **structure and data**:

### 📝 Steps:
1. Open **SQL Server Management Studio (SSMS)**.
2. Create a new database (or let the script do it for you).
3. Open the SQL file from:
4. Execute the script — it will:
- Create a database named `WebMobiTask1DB`
- Set up all tables and relationships
- Insert predefined sample data

### ✅ Result:
You’ll have a fully working database that mirrors the original development setup.

---

## 📌 Option 2: Use EF Core Code-First Migrations (Schema Only)

If you prefer to use **Entity Framework Core** to generate the database:

### 📝 Steps:
1. Open the solution in **Visual Studio**.
2. Open the `appsettings.json` file and update your connection string:

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=WebMobiTask1DB;Trusted_Connection=True;TrustServerCertificate=True;"
}
Open the Package Manager Console (in Visual Studio: Tools > NuGet Package Manager > Package Manager Console) and run the following commands:

To add a migration:
Add-Migration InitialCreate

To update the database:
Update-Database

##❗️NOTE:
These commands will only create the schema (tables and relationships) in your database.
They do not insert any sample data. If you need both schema and sample data, refer to Option 1 above.
```json
