<!---
# 🔥 Attention!!

**Currently, .NET MAUI is in preview, and setting up the environment for the .NET MAUI is quite tricky and challenging. So if you would like to ignore/skip .NET MAUI for now then, simply remove the `MauiBlazorApp` and `MauiBlazorApp.WinUI` projects from the solution and build the solution. Now everything should work fine!**
-->

# 🏃‍♂️ How to Run the Project
  1. First make sure that you have **.NET 6.0** and **Visual Studio 2022** are installed.
  2. Now open the solution with VS 2022 and build the solution to make sure that there is no error.
  3. Now make **`Identity.Api`, `EmployeeManagement.Api` and `BlazorWasmApp`** projects as startup projects and then run it. On startup necessary databases will be created in **MSSQLLocalDB**

# Clean Architecture in ASP.NET Core
This repository contains the implementation of Domain Driven Design and Clear Architecture in ASP.NET Core.

# ⚙️ Fetures
1. Domain Driven Design
2. CQRS
3. REST API
4. API Versioning
5. Blazor Client (Web Assembly)
6. Caching with InMemory and Redis
7. Logging with Serilog
8. EF Core Repository and Cache Repository
9. Microsoft SQL Server
10. Simple and clean admin template for starter

# 📁 Folder Structures:
![image](https://user-images.githubusercontent.com/14342773/188265638-ea1ebeb6-b6ee-4913-bfa0-39e71f200c8b.png)


## 📂 src/ServerApp:
  Will contain all the projects of the server side app and will look like as follows:
  ![tempsnip](https://user-images.githubusercontent.com/14342773/188265763-6f5416d9-e2a4-407e-9315-39d2e2688576.png)

### 📂 src/ServerApp/Core:
  Core folder contains the projects related to the application core funcationalities like Domain Logic and Application Logic. This folder is the heart of the server app.
  
  ![tempsnip](https://user-images.githubusercontent.com/14342773/188265917-93853b16-e284-4901-acf3-ee0c87c3dd55.png)

  
#### 📝 EmployeeManagement.Domain Project: 
  This is application's **Domain Layer** which will contain:
   1. Domain entities which will be mapped to database table
   2. Domain logic,
   3. Domain repositories
   4. Value objects.
   5. Domain Exceptions

This will not depend on any other project. This is fully independent.

![tempsnip](https://user-images.githubusercontent.com/14342773/188266015-427309e8-5eef-453a-9a4f-593dabdf155a.png)


#### 📝 EmployeeManagement.Application Project:
  This is application's **Application Layer** which will contain:
   1. Appplication Logic
   2. Infrastructure repositories' interfaces i.e Cache Repository interfaces.
   3. Infrastructure services' interfaces i.e IEmailSender, ISmsSender, IExceptionLogger etc.
   4. Data Transfer Objects (Dtos)
   5. Command and Queries
  
  It will only depend on Domain project aka **Domain Layer.**
  
 ![tempsnip](https://user-images.githubusercontent.com/14342773/188266095-717a6adc-30f8-4901-b42a-a3abbe829cf7.png)


  
### 📂 src/ServerApp/Infrastructure:
  This folder will contains all the project related to project's infrastuctures like Data access code, persistance and application's cross cutting concerns' intefaces implementation like IEmailSender, ISmsSender etc.
  
  ![Infrastructure](https://user-images.githubusercontent.com/14342773/123589564-37f56100-d80b-11eb-8f94-c79ea589adf8.PNG)

  
### 📂 src/ServerApp/Presentation:
  This folder will contain all the REST API projects which is the **PresentationLayer** of the project.
  
  ![tempsnip](https://user-images.githubusercontent.com/14342773/188266148-712f2608-ae3b-461e-af0c-470ebb57a5f3.png)

