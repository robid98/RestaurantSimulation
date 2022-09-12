# RestaurantSimulation

<h2>Deploy</h2>
- Azure or AWS

<h2>Authentication/Authorization</h2>
- Auth0 : https://auth0.com/

<h2>RestaurantSimulation.Backend</h2>

- CLEAN Arhitecture
- CQRS
- Entity Framework Core 6.0
- SQL Server
- FluentValidation
- Auth0
- ErrorOr Library
- WebAPI
- LINQ
- xUnit
- moq
- shouldly

<h2>RestaurantSimulation.Frontend</h2>

- React
- Angular

<h2>Branches conventions</h2>

- <h4>backend/feature/branch-name</h4>
- <h4>backend/bug/bug-name</h4>
- <h4>frontend/feature/branch-name</h4>
- <h4>frontend/bug/bug-name</h4>

<h2>EntityFramework Migrations Commands</h2>

- add-migration <migration-name> -project RestaurantSimulation.Infrastructure -o Persistence/Migrations ( adding a new migration )
- update-database ( update the database with the new migrations )

<h2>Branch protection rules</h2>
- Git guardian with passed status
- RestaurantSimulation.Backend Azure Pipeline with passed status
