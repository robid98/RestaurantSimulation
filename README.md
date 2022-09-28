<h1 align="center">Restaurant Simulation - A real world restaurant application</h1>

<br>

<img src="https://user-images.githubusercontent.com/89996135/192704213-81735e23-98ed-4373-a7d7-89dce6c9b575.png" alt="angular" width="150" height="150"/> &nbsp;&nbsp;
<img src="https://uploads-ssl.webflow.com/61566192da988c377f1ac06c/616dfac0a533fe024d89e327_60dbd7237742ba750d49cf35_icon-auth0-marketplace.svg" alt="auth0"  width="150" height="140"/> &nbsp;&nbsp;
<img src="https://neosmart.net/blog/wp-content/uploads/2019/06/dot-NET-Core.png" alt="auth0" width="150" height="140"/> &nbsp;&nbsp;
<img src="https://seeklogo.com/images/M/microsoft-sql-server-logo-96AF49E2B3-seeklogo.com.png" alt="auth0" width="150" height="140"/> &nbsp;&nbsp;
<img src="https://upload.wikimedia.org/wikipedia/commons/thumb/a/a7/React-icon.svg/2300px-React-icon.svg.png" alt="auth0" width="150" height="140"/> &nbsp;&nbsp;

<br>

<div align="center">

## Description
The aim of this project is to make a functional Restaurant web application.
On the backend side the project will be build as a Web API and the frontend side will be made in Angular or React.

</div>

<br>

## Deploy
- Azure or AWS

## Authentication
- Auth0 : https://auth0.com/ <br>
- RestaurantSimulation.Backend is protected with Auth0. You need a valid access token in order to use the Api. Also there are two roles in the RestaurantSimulation:<br>
&nbsp;&nbsp;&nbsp;  1.  <b>restaurant-simulation-admin</b>  (You have full access to all features of the application) <br>
&nbsp;&nbsp;&nbsp;  2.  <b>restaurant-simulation-client</b> (You have access only to some features of the application)

## RestaurantSimulation.Backend

- <b>CLEAN Arhitecture</br>
- <b>CQRS - MediatR</br>
- <b>Entity Framework Core 6.0</br>
- <b>SQL Server</br>
- <b>FluentValidation</br>
- <b>Auth0</br>
- <b>ErrorOr Library</br>
- <b>WebAPI</br>
- <b>LINQ</br>
- <b>xUnit</br>
- <b>moq</br>
- <b>shouldly</br>

## RestaurantSimulation.Frontend

- <b>React</br>
- <b>Angular</br>

## Branches conventions

- <h4>backend/feature/branch-name</h4>
- <h4>backend/bug/bug-name</h4>
- <h4>frontend/feature/branch-name</h4>
- <h4>frontend/bug/bug-name</h4>

## EntityFramework Migrations Commands

- EntityFrameworkCore\Add-Migration 'migration-name' -project RestaurantSimulation.Infrastructure -o Persistence/Migrations
- EntityFrameworkCore\Update-database

## Branch protection rules

- Git guardian with passed status
- RestaurantSimulation.Backend Azure Pipeline with passed status

<hr>

## Contributing

- Feel free to contribute to this project if you like it or if you have any suggestions for new Features.
- Also feel free to report any bug`s you find into the application.
- This project is also good for begginers to learn how to structure code with CLEAN Architecture, learn CQRS with the help of MediatR Library, Entity Framework etc.
- This project can help you also if you are only a Frontend Developer. You will have a free Web API to use, for building an Frontend, with a technology by your choice. The only restriction is, authentication need to be made with <b>Auth0</b>.

## 

- https://github.com/robid98/RestaurantSimulation/issues - For suggestions, bugs. Here you will find also what needs to be implemented / current stories.
- https://github.com/users/robid98/projects/1 - RestaurantSimulation Board
