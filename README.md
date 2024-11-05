# Online Shopping Platform - ASP.NET Core Web API

Welcome to the **Online Shopping Platform** project! This repository contains an ASP.NET Core Web API implementation for an online shopping platform with a multi-layered architecture. The project utilizes Entity Framework Core with a Code First approach to interact with the database.

## Project Overview

This project is built with the following requirements:

- **Three-layer architecture**: Presentation Layer (API), Business Layer (Business Logic), and Data Access Layer (Database interaction using Entity Framework).
- **Identity & Authentication**: User authentication and authorization via JWT and custom identity service.
- **Data Protection**: Secure user password storage using ASP.NET Core Data Protection.
- **Middleware**: Custom middleware for logging requests and maintenance checks.
- **Model Validation**: Validation of user and product data.
- **Global Exception Handling**: Centralized error handling mechanism.

## Project Structure

1. **Presentation Layer** (API Layer)
   - Controllers handle HTTP requests and responses.
   - Endpoints are defined for users, products, and orders.

2. **Business Layer** (Business Logic Layer)
   - Contains the business logic related to users, products, and orders.
   - Service classes handle operations like order creation, product stock management, etc.

3. **Data Access Layer** (Repository Layer)
   - Implements Entity Framework Core to manage interactions with the database.
   - Includes repository classes for managing entities (User, Product, Order, etc.).
   - Uses a Unit of Work pattern to manage transactions across multiple repositories.

## Features

- **User Management**: 
  - Customer information storage with properties like FirstName, LastName, Email, PhoneNumber, and Password.
  - Passwords are securely encrypted using ASP.NET Core Data Protection.
  - Role-based authorization with roles like "Admin" and "Customer".

- **Product Management**:
  - Products are stored with attributes like ProductName, Price, and StockQuantity.

- **Order Management**:
  - Orders can be created by customers, with each order containing multiple products (many-to-many relationship).
  - Order data includes properties such as OrderDate, TotalAmount, and CustomerId.

- **Authentication and Authorization**:
  - Uses JWT tokens for authentication.
  - Role-based access control ensures that only authorized users can perform certain actions.

- **Logging Middleware**:
  - Logs the request URL, request time, and the customer making the request for each API call.

- **Maintenance Middleware**:
  - Includes a mechanism to put the platform into maintenance mode, which can be activated through a special flag in the database.

- **Custom Action Filter**:
  - Provides the ability to control access to specific API endpoints based on the time of day or other conditions.

- **Model Validation**:
  - Validation rules for User and Product entities (e.g., email format, required fields, stock quantity validation).

- **Global Exception Handling**:
  - Catches all exceptions globally and returns a user-friendly response.

## Technologies Used

- **ASP.NET Core**: For building the Web API.
- **Entity Framework Core**: For database interactions (Code First approach).
- **JWT**: For authentication and authorization.
- **ASP.NET Core Identity**: For user authentication and role management.
- **Dependency Injection**: For managing service lifetimes and dependencies.
- **Data Protection API**: For securely handling user passwords.
- **Custom Middleware**: For logging and maintenance mode management.

## Database Model

- **User**: Stores customer information.
  - `Id` (int, Primary Key)
  - `FirstName` (string)
  - `LastName` (string)
  - `Email` (string, Unique)
  - `PhoneNumber` (string)
  - `Password` (string, encrypted)
  - `Role` (enum: Admin or Customer)

- **Product**: Stores product details.
  - `Id` (int, Primary Key)
  - `ProductName` (string)
  - `Price` (decimal)
  - `StockQuantity` (int)

- **Order**: Stores customer orders.
  - `Id` (int, Primary Key)
  - `OrderDate` (DateTime)
  - `TotalAmount` (decimal)
  - `CustomerId` (int, Foreign Key to User)

- **OrderProduct**: Many-to-many relationship between Orders and Products.
  - `OrderId` (int, Foreign Key to Order)
  - `ProductId` (int, Foreign Key to Product)
  - `Quantity` (int)

![image](https://github.com/user-attachments/assets/6889d42c-e668-45e9-8c5b-e3f8722abf0e)
