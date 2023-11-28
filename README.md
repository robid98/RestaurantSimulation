<h1 align="center">Restaurant Simulation - A real world restaurant application</h1>

<br>

<img src="https://user-images.githubusercontent.com/89996135/192704213-81735e23-98ed-4373-a7d7-89dce6c9b575.png" alt="angular" width="150" height="150"/> &nbsp;&nbsp;
<img src="https://uploads-ssl.webflow.com/61566192da988c377f1ac06c/616dfac0a533fe024d89e327_60dbd7237742ba750d49cf35_icon-auth0-marketplace.svg" alt="auth0"  width="150" height="140"/> &nbsp;&nbsp;
<img src="https://neosmart.net/blog/wp-content/uploads/2019/06/dot-NET-Core.png" alt="dotnet" width="150" height="140"/> &nbsp;&nbsp;&nbsp;
<img src="https://imagedelivery.net/5MYSbk45M80qAwecrlKzdQ/6f6d6101-68b4-4c53-d405-71f5de512f00/preview" alt="mysql" width="150" height="140"/> &nbsp;&nbsp;
<img src="https://d2nir1j4sou8ez.cloudfront.net/wp-content/uploads/2021/12/nextjs-boilerplate-logo.png" alt="nextjs" width="150" height="150"/> 

<br>

<div align="center">
  
## Description
The aim of this project is to make a functional Restaurant web application.
On the backend side the project will be build as a Web API and the frontend side will be made in Angular.
Backend architecture is a monolitch one and frontend architecture will be based on feature modules.
</div>

<br>

## Project status
[![RestaurantSimulation.Backend.CI](https://github.com/robid98/RestaurantSimulation/actions/workflows/backend-ci.yml/badge.svg)](https://github.com/robid98/RestaurantSimulation/actions/workflows/backend-ci.yml)

## Deploy
- Deploy will be done when the project is in a good state. For deploying `AWS` will be used.

## Authentication
- Auth0 free tier will be used: https://auth0.com/ <br>
- `RestaurantSimulation.Backend` is protected with `Auth0`. You need a valid access token in order to use the `RestaurantSimulation.Api`. Also there are two roles in the RestaurantSimulation:<br>

> Below will be presented all the roles inside RestaurantSimulation application

| Role name | Role description |
|--|--|
| **restaurant-simulation-admin** | You have full access to all features of the RestaurantSimulation app |
| **restaurant-simulation-client** | You have limited access only to some features of the RestaurantSimulation app |

## RestaurantSimulation libraries/technologies used:
> Please complete this list when a new technology/library will be used:

<details>
  <summary>RestaurantSimulation.Backend</summary>
  
- <b>CLEAN Arhitecture</br>
- <b>MediatR</br>
- <b>Entity Framework Core 6.0</br>
- <b>MySql</br>
- <b>FluentValidation</br>
- <b>Auth0</br>
- <b>ErrorOr Library</br>
- <b>WebAPI</br>
- <b>LINQ</br>
- <b>xUnit</br>
- <b>Moq</br>
- <b>Shouldly</br>

</details>
<br/>

- RestaurantSimulation.Frontend
> The frontend is focused to be written in Next.js and also the is a possibility to create a version in Angular too.
  -  Angular
  > To be completed
  -  Next.js
> To be completed

## Branches conventions
- Branches will follow the next conventions based on the work that is done:
  
| Branch name | Used when |
|--|--|
| **backend/feature/[branch-name]** | Used when you want to create a new feature in the backend |
| **backend/bug/[bug-name]** | Used when you try to fix a bug in the backend |
| **frontend/feature/[branch-name]** | Used when you want to create a new feature in the frontend |
| **frontend/bug/[bug-name]** | Used when you try to fix a bug in the frontend  |

## EntityFramework Migrations Commands
> Below will be described useful commands to interact with the MySql database and EntityFramework

- Adding a new migration in RestaurantSimulation.Backend
  - `EntityFrameworkCore\Add-Migration 'migration-name' -project RestaurantSimulation.Infrastructure -o Persistence/Migrations`
- Applying migrations to the MySQL instance
  - `EntityFrameworkCore\Update-database`

## Branch protection rules

- `Git guardian` with passed status
- [backend-ci](https://github.com/robid98/RestaurantSimulation/actions/workflows/backend-ci.yml) github action with passed status when modifying `RestaurantSimulation.Backend`

Github action for validating the pull requests that target `RestaurantSimulation.Backend` contains some simple steps for building the project, running unit tests and integration tests.
Integration tests will target a MySQL Docker Container that is created also in the pipeline.
<br>
![image](https://github.com/robid98/RestaurantSimulation/assets/89996135/43397099-998c-403e-93e2-9c55811e7a92)
</br>

> After the [backend-ci](https://github.com/robid98/RestaurantSimulation/actions/workflows/backend-ci.yml) GitHub Action is completed you can see the test results:
![image](https://github.com/robid98/RestaurantSimulation/assets/89996135/375eeb52-ff0f-47f7-b6e5-5c389ae5f367)


<hr>

## Run RestaurantSimulation.Backend on local
<p align="center">
  <img src="https://user-images.githubusercontent.com/89996135/193544075-9f17332b-bf94-466a-836d-ecf308cd4103.png" alt="auth0" width="150" height="140"/> &nbsp;&nbsp;
</p>

RestaurantSimulation Swagger Documentation Url: http://localhost:8080/swagger/index.html <br>
RestaurantSimulation Api Url: http://localhost:8080 <br>

<h3>Commands</h3>

- Building the project and creating the image
  - `docker-compose -f docker-compose.yml build`
- Creating the database and RestaurantSimulation.Api Container
  - `docker-compose -f docker-compose.yml up -d`
- Removing the created containers without deleting the database volume
  - `docker-compose -f docker-compose.yml down`
- Removing the created containers and deleting the database volume. Data stored will be lost
  - `docker-compose -f docker-compose.yml down -v`

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
- [Docker](https://www.docker.com/)
- [MySql](https://www.mysql.com/)
- [Next.js](https://nextjs.org/)
