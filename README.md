# Ambev Developer Evaluation – Sales API

Este projeto é uma API RESTful construída com ASP.NET Core seguindo os princípios de Domain-Driven Design (DDD). Ele permite gerenciar vendas com controle de itens, descontos por quantidade e funcionalidades completas de CRUD, incluindo cancelamento de vendas.

---

## 🛠️ Tecnologias e Frameworks Utilizados

- **ASP.NET Core Web API**
- **Entity Framework Core**
- **MediatR** – Comunicação entre camadas
- **AutoMapper** – Mapeamento de objetos
- **Rebus** – Preparado para eventos (simulados via log)
- **FluentValidation** – Validação de comandos e requisições
- **Bogus** – Geração de dados falsos para testes
- **NSubstitute** – Mocks em testes unitários
- **xUnit** – Testes unitários
- **Microsoft.AspNetCore.Mvc.Testing** – Testes de integração

---

## 🔧 Como Executar o Projeto

### 1. Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)

### 2. Clonar o repositório

```bash
git clone https://github.com/seu-usuario/seu-repo.git
```
---

## 🧱 Estrutura do Projeto

- `Domain`: entidades e regras de negócio
- `Application`: comandos, consultas, handlers e validações
- `WebApi`: controladores e entrada principal da aplicação
- `ORM`: mapeamentos com Entity Framework Core
- `Tests`: testes unitários
- `Integration`: testes de integração com a API

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

## 🔧 Como Executar o Projeto

### 1. Clonar o repositório

```bash
git clone https://github.com/seu-usuario/seu-repo.git
cd seu-repo
```

### 2. Restaurar os pacotes

```bash
dotnet restore
```

### 3. Criar o banco de dados e aplicar migrations

```bash
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM
```

> É necessário que o banco esteja configurado corretamente no `appsettings.Development.json`

### 4. Executar a API

```bash
dotnet run --project src/Ambev.DeveloperEvaluation.WebApi
```

A API estará disponível em:

```
http://localhost:5000
https://localhost:5001
```

---

## 📦 Endpoints Disponíveis

| Método | Rota                     | Descrição                   |
|--------|--------------------------|-----------------------------|
| POST   | `/api/sales`             | Criar nova venda            |
| GET    | `/api/sales/{id}`        | Consultar venda por ID      |
| PUT    | `/api/sales/{id}`        | Atualizar venda             |
| PUT    | `/api/sales/cancel/{id}` | Cancelar venda              |
| DELETE | `/api/sales/{id}`        | Excluir venda               |

---

## 📐 Regras de Negócio

- Desconto por **quantidade de produtos iguais**:
  - 4 a 9 unidades → **10%**
  - 10 a 20 unidades → **20%**
  - Acima de 20 unidades → **inválido**
  - Abaixo de 4 unidades → **sem desconto**

- Cancelamento:
  - Venda não pode ser cancelada duas vezes
  - Marca `IsCancelled = true` e atualiza `UpdatedAt`

---

## ✅ Executando os Testes

### Testes Unitários

```bash
dotnet test tests/Ambev.DeveloperEvaluation.Tests
```

### Testes de Integração

```bash
dotnet test tests/Ambev.DeveloperEvaluation.Integration
```

> Inclui fluxo completo: criação → consulta → atualização → cancelamento → exclusão.

---

## 🧪 Testes de Integração

Testado com `WebApplicationFactory<Program>`, com cenários de:

- Criação de venda com descontos
- Validação de erros (ex: ID inválido, venda inexistente)
- Ciclo completo da venda (CRUD + cancelamento)
- Verificação de conteúdo de resposta (modelo `ApiResponseWithData<T>`)

---

## 📂 Como está estruturado o Projeto

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

Desenvolvido por Henrique Araújo  

---

## 📄 Licença

Este projeto é livre para uso apenas em fins educacionais e de avaliação técnica.
```

---
