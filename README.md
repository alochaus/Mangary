# Mangary

![Mangary](/README_media/Mangary.gif)

## Dependencies

- **bootstrap**
- **font-awesome-4.7.0**
- **jquery**
- **jquery-validation**
- **jquery-validation-unobtrusive**

*Make sure that the paths are pointing to existing files in Views/Shared/_Layout.cs.*

## How to install

**Step 1: clone this repo**
```bash
$ git clone https://github.com/alochaus/Mangary.git
$ cd Mangary
```

**Step 2: install dependencies**
```bash
$ dotnet add package bootstrap --version 4.3.1
$ dotnet add package font-awesome --version 4.7.0
$ dotnet add package jQuery --version 3.3.1
$ dotnet add package jQuery.Validation --version 1.17.0
$ dotnet add package Microsoft.jQuery.Unobtrusive.Validation --version 3.2.11
```

**Step 3: run it**
```bash
$ dotnet restore
$ dotnet ef database update
$ dotnet run
```

## Features

- Responsive layout
- Accounts
- Roles (admin and manager)
- Create, edit and delete products (only available for users with manager role)
- Role manager (only available for users with admin role)
- Shopping cart
- Search by name or category
- Tables designed for the app to be scalable to as many categories as you want
