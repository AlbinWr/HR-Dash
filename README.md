# HR Dash
Ett enkelt HR-dashboard byggt med ASP.NET Core och Entity Framework. Applikationen hanterar anställda, skift, möten.

# Stack
- ASP.NET Core MVC
- Entity Framework Core
- Identity för autentisering/roller
- SQL Server
- Bootstrap

# Funktioner
- Registrering & inloggning (Identity)  
- Rollbaserad åtkomst – Admin / Manager / Employee  
- CRUD-hantering av anställda  
- Mötesbokning (skapa, redigera, ta bort)  
- Schemaläggning  
   – Återkommande skift  
   – Engångsskift  
- Profilsidor  
- SQL-databas via Entity Framework

# Seedade Användare
Användare seedas automatiskt vid uppstart.

Namn │ Roll │ E-post (inloggning) │ Lösenord
- Albin Admin	Admin	albin@admin.com	Admin123!
- Anna Larsson	Manager, Anställd	anna.larsson@example.com	Manager123!
- Erik Svensson	Anställd	erik.svensson@example.com	Anstalld123!
- Lisa Nyman	Anställd	lisa.nyman@example.com	Anstalld123!

Körs i terminal
- dotnet ef database update
- dotnet run
