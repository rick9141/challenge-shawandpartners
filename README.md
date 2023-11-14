
# Challenge Shaw And Partners 🏆

- Challenge objective: Build a .NET web application that allows users to upload a CSV file with pre-formatted data and display the data as json through the API. The application should also include a search parameter that allows users to search data in the uploaded CSV file.

## Prerequisites to run the project  ❗
-   Visual Studio 2022, Visual Studio Code
-   .NET 7.0

## How to run the project - Step by Step  🔨
- Clone the project using the command "git clone https://github.com/rick9141/challenge-shawandpartners.git"
-  Open the .sln file "Challenge-ShawAndPartners.sln" in Visual Studio 2022 or Visual Code
 ### Running the solution:
- In Visual Studio, open the terminal in the default solution folder "challenge-shawandpartners"
 - Execute the following command line: dotnet run --project ShawAndPartners.API
### Running tests:
  - In Visual Studio, open the terminal in the default solution folder "challenge-shawandpartners"
  - Execute the following command line: dotnet tests --project ShawAndPartners.Tests
    
## Technologies & Frameworks used ⚙
-   .NET 7
-   Dapper
-   Swagger
-   SQLite
-   CsvHelper
-   xUnit

## About the Architecture & Design Practices Implemented 📐
-   Clean Architecture
-   Use of concepts from the software design methodology DDD (Domain Driven Design)
-   Dependency Injection (IoC design)
-   Repository Pattern

## Features  ✒️

-  CSV upload with a list of users.
-  Search for users by name, city, country, or favorite sport
  ### Extras:
-   Bring all users
-   Update a user's information
-   Delete a user
