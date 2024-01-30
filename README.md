# Simple Library CRUD API Documentation

Welcome to the Simple Library CRUD API documentation application. This API allows restful operations on Books, Authors & Publisher Information.

## Table of Contents

- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [API Endpoints](#api-endpoints)
- [Request and Response Formats](#request-and-response-formats)
- [Sample Usage](#sample-usage)
- [License](#license)

## Getting Started

### Prerequisites

Before you start, make sure you have the following prerequisites installed on your system:

- Dotnet Sdk (>= 8.0)
- RabbitMQ
- Redis
- Database (e.g., MSSQL) or Cloud MSSQL. In this project, Cloud SQL Server database was used

### Installation

1. Clone this repository:
    Open a terminal and run 
    
   ```bash
   git clone https://github.com/7j4n1/BookApi.git
   cd BookApi 
   ```

2. Install the dotnet packages for the project in the project by running:

    ```bash
    dotnet build
    ```
3. Configure your application settings in the appsettings.json:
    ```bash
    cp appsettings.Development.json appsettings.json
    ```
    Configure the database, redis and rabbitmq config variables as follows:
    ```bash
    "AllowedHosts": "*",
    "RedisCloudUrl": "redis-hostname:port,password=your-password",
    "RabbitMqUrl": "your-rabbitmqsetting-endpoint-goes-here",
    "ConnectionStrings": {
        "DefaultConnection" : "Data Source=mssql-servername;Initial Catalog=database-name;User ID=username;Password=password;TrustServerCertificate=true;"
    }
    ```
4. Ensure that you've started them 3 servers (Redis, SQL Server, RabbitMQ)
5. Run the migrations:
    ```bash
    dotnet build

    dotnet-ef migrations add "Migration Name"

    dotnet-ef database update  - to update the database with the Api Models
    ```
6. Start the development server:
    ```bash
    dotnet run | dotnet run --launch-profile https (To force run on https scheme)

    ```
    BookApi API is now up and running on {https://localhost:7057} which is the base url!

7. Open a browser and navigate to https://localhost:7057/swagger to see an API documentation

### API Endpoints

### Request and Response Formats
The API uses JSON for both requests and responses. The following table describes the JSON format for the requests and responses:

<table>
    <thead>
        <th> Requests </th>
        <th> Response </th>
    </thead>
    <tbody>
        <tr>
            <td>POST /api</td>
            <td>201 Created with the newly created person in the response body</td>
        </tr>
        <tr>
            <td>GET /api</td>
            <td>200 OK with an array of people in the response body.</td>
        </tr>
        <tr>
            <td>GET /api/{id}</td>
            <td>200 OK with the person with the specified id in the response body.</td>
        </tr>
        <tr>
            <td>PUT /api/{id}</td>
            <td>200 OK with the updated person in the response body.</td>
        </tr>
        <tr>
            <td>PATCH /api/{id}</td>
            <td>200 OK with the updated person in the response body.</td>
        </tr>
        <tr>
            <td>DELETE /api/{id}</td>
            <td>204 No Content</td>
        </tr>
    </tbody>
</table>

### Sample Usage

## Adding a new person (201 Created)

<img src="samples/create.png" alt="Create new person" />

## Fetch the details of a person (200 OK)

<img src="samples/read.png" alt="fetch the details of a person" />

## Modify the details of an existing person (200 OK)

<img src="samples/update.png" alt="modify the details of an existing person" />

## Remove a person (204 No Content)

<img src="samples/delete.png" alt="remove a person" />

### License

The MIT License (MIT)