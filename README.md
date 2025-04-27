# Ambev Developer Evaluation – Sales API

This project is a RESTful API built with ASP.NET Core following Domain-Driven Design (DDD) principles. It allows you to manage sales with item tracking, quantity discounts, and full CRUD functionality, including sales cancellation.

---

## 🛠️ Technologies and Frameworks Used

- **ASP.NET Core Web API**
- **Entity Framework Core**
- **MediatR** – Communication between layers
- **AutoMapper** – Object mapping
- **Rebus** – Event-ready (simulated via log)
- **FluentValidation** – Validation of commands and requests
- **Bogus** – Generation of fake data for testing
- **NSubstitute** – Mocks in unit tests
- **xUnit** – Unit tests
- **Microsoft.AspNetCore.Mvc.Testing** – Integration tests

---

## 🔧 How to Run the Project

### 1. Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)

### 2. Clone the repository

```bash
git clone https://github.com/seu-usuario/seu-repo.gitAqui está o conteúdo completo para o seu `README.md`, incluindo todas as instruções de build, execução, testes e fluxo completo da API:

---

```markdown
# 🛒 Ambev Developer Evaluation – Sales API

This project is a RESTful API built with ASP.NET Core following Domain-Driven Design (DDD) principles. It allows you to manage sales with item tracking, quantity discounts, and full CRUD functionality, including sales cancellation.

---

## 🧱 Estrutura do Projeto

- `Domain`: entities and business rules
- `Application`: commands, queries, handlers and validations
- `WebApi`: controllers and main application entry
- `ORM`: mappings with Entity Framework Core
- `Tests`: unit tests
- `Integration`: integration tests with the API

---

## 🚀 Tecnologias Utilizadas

- **.NET 8**  
- **ASP.NET Core Web API**  
- **Entity Framework Core**
- **MediatR**
- **AutoMapper**
- **FluentValidation**
- **Rebus (simulado por log)**
- **Bogus** – dados falsos para testes
- **NSubstitute** – mocks
- **xUnit** – testes
- **Microsoft.AspNetCore.Mvc.Testing** – testes de integração

---

## 🔧 How to Run the Project

### 1. Clone the repository

```bash
git clone https://github.com/seu-usuario/seu-repo.git
cd seu-repo
```

### 2. Restore packages

```bash
dotnet restore
```

### 3. Create the database and apply migrations

```bash
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM
```

> The bank must be configured correctly in `appsettings.Development.json`

### 4. Execute API

```bash
dotnet run --project src/Ambev.DeveloperEvaluation.WebApi
```

The API will be available at:

```
http://localhost:5000
https://localhost:5001
```

---

## 📦 Available Endpoints

| Método | Rota                     | Descrição                   |
|--------|--------------------------|-----------------------------|
| POST   | `/api/sales`             | Create new sale             |
| GET    | `/api/sales/{id}`        | Get sale by ID              |
| PUT    | `/api/sales/{id}`        | Update sale                 |
| PUT    | `/api/sales/cancel/{id}` | Cancel sale                 |
| DELETE | `/api/sales/{id}`        | Delete sale                 |

---

## 📐 Business Rules

- Discount for **quantity of equal products**:
  - 4 to 9 units → **10%**
  - 10 to 20 units → **20%**
  - Over 20 units → **invalid**
  - Under 4 units → **no discount**

- Cancellation:
  - Sale cannot be canceled twice
  - Set `IsCancelled = true` and update `UpdatedAt`

---

## ✅ Running the Tests

### Unit Tests

```bash
dotnet test tests/Ambev.DeveloperEvaluation.Tests
```

### Integration tests

```bash
dotnet test tests/Ambev.DeveloperEvaluation.Integration
```

> Includes complete flow: creation → query → update → cancellation → deletion.

---

## 🧪 Integration tests

Tested with `WebApplicationFactory<Program>`, with scenarios of:

- Creating a sale with discounts
- Validating errors (e.g.: invalid ID, non-existent sale)
- Complete sale cycle (CRUD + cancellation)
- Verifying response content (`ApiResponseWithData<T>` model)

---

## 📂 Project structure

```
src/
 ├── Application
 ├── Domain
 ├── ORM
 ├── WebApi
tests/
 ├── Ambev.DeveloperEvaluation.Tests
 └── Ambev.DeveloperEvaluation.Integration
```

---

## 👨‍💻 Autor

Developed by Henrique Araújo  

---

## 📄 Licença

This project is free to use for educational and technical evaluation purposes only.
```

---
