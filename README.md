<h1 align="center">Restaurant Simulation - A real world restaurant application</h1>

<br>

<img src="https://user-images.githubusercontent.com/89996135/192704213-81735e23-98ed-4373-a7d7-89dce6c9b575.png" alt="angular" width="150" height="150"/> &nbsp;&nbsp;
<img src="https://uploads-ssl.webflow.com/61566192da988c377f1ac06c/616dfac0a533fe024d89e327_60dbd7237742ba750d49cf35_icon-auth0-marketplace.svg" alt="auth0"  width="150" height="140"/> &nbsp;&nbsp;
<img src="https://neosmart.net/blog/wp-content/uploads/2019/06/dot-NET-Core.png" alt="auth0" width="150" height="140"/> &nbsp;&nbsp;
<img src="https://seeklogo.com/images/M/microsoft-sql-server-logo-96AF49E2B3-seeklogo.com.png" alt="auth0" width="150" height="140"/> &nbsp;&nbsp;

<br>

<div align="center">

## Description
The aim of this project is to make a functional Restaurant web application.
On the backend side the project will be build as a Web API and the frontend side will be made in Angular.
Backend architecture is a monolitch one and frontend architecture will be based on feature modules.

</div>

<br>

## Deploy
- Deploy will be done when the project is in a good state. For deploying `AWS` will be used.

## Authentication
- Auth0 : https://auth0.com/ <br>
- `RestaurantSimulation.Backend` is protected with `Auth0`. You need a valid access token in order to use the Api. Also there are two roles in the RestaurantSimulation:<br>
&nbsp;&nbsp;&nbsp;  1.  `restaurant-simulation-admin`  (You have full access to all features of the application) <br>
&nbsp;&nbsp;&nbsp;  2.  `restaurant-simulation-client` (You have access only to some features of the application)

## RestaurantSimulation.Backend libraries/technologies used:

- <b>CLEAN Arhitecture</br>
- <b>MediatR</br>
- <b>Entity Framework Core 6.0</br>
- <b>SQL Server</br>
- <b>FluentValidation</br>
- <b>Auth0</br>
- <b>ErrorOr Library</br>
- <b>WebAPI</br>
- <b>LINQ</br>
- <b>xUnit</br>
- <b>Moq</br>
- <b>Shouldly</br>

## RestaurantSimulation.Frontend

- <b>Angular</br>

## Branches conventions
- Branches will follow the next conventions based on the work that is done:
  - <h4>backend/feature/branch-name</h4>
  - <h4>backend/bug/bug-name</h4>
  - <h4>frontend/feature/branch-name</h4>
  - <h4>frontend/bug/bug-name</h4>

## EntityFramework Migrations Commands

- Adding a new migration in RestaurantSimulation.Backend
  - `EntityFrameworkCore\Add-Migration 'migration-name' -project RestaurantSimulation.Infrastructure -o Persistence/Migrations`
- Applying migrations to the MySQL instance
  - `EntityFrameworkCore\Update-database`

## Branch protection rules

- `Git guardian` with passed status
- `RestaurantSimulation.Backend` azure pipeline with passed status

Pipeline for validating the pull requests that target `RestaurantSimulation.Backend` contains some simple tasks for building the project, running unit tests and integration tests.
Integration tests will target a MySQL Docker Container that is created also in the pipeline.
<br>
![image](https://github.com/robid98/RestaurantSimulation/assets/89996135/29b2bdf9-15e5-4fde-b381-8a902b28b8cf)

<hr>

## Run RestaurantSimulation.Backend on local
<p align="center">
  <img src="https://user-images.githubusercontent.com/89996135/193544075-9f17332b-bf94-466a-836d-ecf308cd4103.png" alt="auth0" width="150" height="140"/> &nbsp;&nbsp;
</p>

RestaurantSimulation Swagger Documentation Url: http://localhost:8080/swagger/index.html <br>
RestaurantSimulation Api Url: http://localhost:8080 <br>

<h3>Commands</h3>

- <b>docker-compose -f docker-compose.yml build</b> ( For building the project and creating the image ) <br>
- <b>docker-compose -f docker-compose.yml up -d</b> ( For creating the database and restaurant api containers) <br>
- <b>docker-compose -f docker-compose.yml down</b> ( For removing the created containers without deleting the database volume ) <br>
- <b>docker-compose -f docker-compose.yml down -v</b> ( For removing the created containers and deleting the database volume - data stored will be lost)

<br>

## Contributing

- Feel free to contribute to this project if you like it or if you have any suggestions for new Features.
- Feel free to report any bug`s you find into the application.
- Project is good for begginers to learn how to structure code with CLEAN Architecture, learn CQRS with help of MediatR Library, Entity Framework, write unit and integration tests.
- This project can help you if you are only a Frontend Developer. You will have a free Web API to use, for building an Frontend, with a technology by your choice. The only restriction is, authentication need to be made with <b>Auth0</b>.

<br>

<hr> 

<br>

## Useful links

- [.NET6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [Angular](https://angular.io/)
- [Auth0](https://auth0.com/)
- [EntityFramework6](https://learn.microsoft.com/en-us/ef/ef6/)
