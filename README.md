# Seeke - Sales API

Projeto de exemplo para apresentar persistência de dados com uma API de solicitação de vendas.

## ✨ Funcionalidade Principal

Simula a criação de uma **venda** com múltiplos itens e retorno detalhado da transação.


### 📌 Regras de Negócio

As regras de negócio abaixo definem níveis de desconto e restrições com base na quantidade de itens idênticos vendidos:

#### ✅ Níveis de Desconto

- **4 a 9 itens**: 10% de desconto
- **10 a 20 itens**: 20% de desconto

#### ❌ Restrições

- **Menos de 4 itens**: não é permitido aplicar desconto
- **Mais de 20 itens**: venda não permitida

---

### 🔧 Configuração da Connection String

ATENÇÃO:  **antes de rodar a migrations, valide as configurações abaixo**

No arquivo `appsettings.json`, substitua os valores em **CAIXA ALTA** pelas informações reais do seu banco de dados:

    "ConnectionStrings": {
      "DefaultConnection": "Host=HOST;Port=PORT;Database=DB_NAME;Username=USER_DB;Password=PASS_DB"
    }

### 📌 Substituições

    HOST → endereço do servidor do banco de dados (ex: localhost)
    
    PORT → porta de conexão (ex: 5432 para PostgreSQL)
    
    DB_NAME → nome do banco de dados
    
    USER_DB → nome de usuário com permissão de acesso
    
    PASS_DB → senha do usuário

---

### 🧩 Aplicar Migrations (EF Core)

Ao baixar o projeto, utilize os comandos abaixo no terminal dentro da pasta raiz do projeto para gerar e aplicar as migrations:

* Gerar a migration inicial
  dotnet ef migrations add AlterFieldsSales --project ./src/Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj --startup-project ./src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj

* Aplicar as migrations ao banco
  dotnet ef database update --project ./src/Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj --startup-project ./src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj

* (Opcional) Remover migration caso necessário
  dotnet ef migrations remove --force --project ./src/Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj --startup-project ./src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj

---

## 📦 Tecnologias Utilizadas

- [.NET 8](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
- ASP.NET Core Web API
- Swagger / OpenAPI (Swashbuckle)
- Arquitetura aplicando DDD
- xUnit e NSubstitute (para testes)
- FluentValidation (para validação de dados)
- AutoMapper (para mapeamento entre entidade de entrada e retorno)

---

## ➕ Criar venda

**POST** `/api/sales`

Endpoint responsável para criar uma venda.

**Body (JSON):**

    {
      "customerId": "962ec098-903a-4769-8d6c-dac13fa10175",
      "items": [
        {
          "productId": "2746e8a4-409b-488a-9d40-83eeb4d88358",
          "quantity": 2,
          "unitPrice": 100.00
        },
        {
          "productId": "2746e8a4-409b-488a-9d40-83eeb4d88358",
          "quantity": 3,
          "unitPrice": 100.00
        }
      ]

**Resposta esperada:**

    {
      "data": {
        "saleId": "f974099d-81fb-40b7-82d4-ddce9ba9bb9b",
        "saleNumber": "BDDF2658",
        "saleDate": "2025-07-30T03:43:41.618709Z",
        "customerName": "Cliente1",
        "branchName": "São Paulo - Unidade Central",
        "items": [
          {
            "saleId": "f974099d-81fb-40b7-82d4-ddce9ba9bb9b",
            "productId": "2746e8a4-409b-488a-9d40-83eeb4d88358",
            "productName": "Product1",
            "quantity": 5,
            "unitPrice": 100.00,
            "subTotal": 500.00,
            "discount": 50.00,
            "total": 450.00
          }
        ],
        "totalAmount": 450.00,
        "isCanceled": false
      },
      "success": true,
      "message": "Sale created successfully",
      "errors": []
    }
---

## ➕ Atualizar uma venda

**PUT** `/api/sales`

Endpoint responsável para atualizar uma venda.

**Body (JSON):**

    {
      "saleNumber": "BDDF2658",
      "customerId": "962ec098-903a-4769-8d6c-dac13fa10175",
      "items": [    
        {
          "productId": "2746e8a4-409b-488a-9d40-83eeb4d88358",
          "quantity": 2,
          "unitPrice": 100.00
        },
        {
          "productId": "ac60dc36-5ad4-410f-8d45-3b2a635bbfda",
          "quantity": 8,
          "unitPrice": 100.00    
        }
      ]
    }

**Resposta esperada:**

    {
        "data": {
            "data": {
                "saleId": "f974099d-81fb-40b7-82d4-ddce9ba9bb9b",
                "saleNumber": "BDDF2658",
                "saleDate": "2025-07-30T03:43:41.618709Z",
                "customerName": "Cliente1",
                "branchName": "São Paulo - Unidade Central",
                "items": [
                    {
                        "saleId": "f974099d-81fb-40b7-82d4-ddce9ba9bb9b",
                        "productId": "2746e8a4-409b-488a-9d40-83eeb4d88358",
                        "productName": "Product1",
                        "quantity": 2,
                        "unitPrice": 100.00,
                        "subTotal": 200.00,
                        "discount": 0,
                        "total": 200.00
                    },
                    {
                        "saleId": "f974099d-81fb-40b7-82d4-ddce9ba9bb9b",
                        "productId": "ac60dc36-5ad4-410f-8d45-3b2a635bbfda",
                        "productName": "Product2",
                        "quantity": 8,
                        "unitPrice": 100.00,
                        "subTotal": 800.00,
                        "discount": 80.0000,
                        "total": 720.0000
                    }
                ],
                "totalAmount": 920.0000,
                "isCanceled": false
            },
            "success": true,
            "message": "Sale updated successfully",
            "errors": []
        },
        "success": true,
        "message": "",
        "errors": []
    }
---

## ➕ Cancelar uma venda

**DElETE** `/api/sales`

Endpoint responsável para cancelar uma venda.

**Body (JSON):**

     {
        "saleNumber": "BDDF2658",
        "customerId": "962ec098-903a-4769-8d6c-dac13fa10175"
      }

**Resposta esperada:**

    {
        "data": {
            "data": {
                "saleId": "f974099d-81fb-40b7-82d4-ddce9ba9bb9b",
                "saleNumber": "BDDF2658",
                "saleDate": "2025-07-30T03:43:41.618709Z",
                "customerName": "Cliente1",
                "branchName": "São Paulo - Unidade Central",
                "items": [
                    {
                        "saleId": "f974099d-81fb-40b7-82d4-ddce9ba9bb9b",
                        "productId": "ac60dc36-5ad4-410f-8d45-3b2a635bbfda",
                        "productName": "Product2",
                        "quantity": 8,
                        "unitPrice": 100.00,
                        "subTotal": 800.00,
                        "discount": 80.00,
                        "total": 720.00
                    },
                    {
                        "saleId": "f974099d-81fb-40b7-82d4-ddce9ba9bb9b",
                        "productId": "2746e8a4-409b-488a-9d40-83eeb4d88358",
                        "productName": "Product1",
                        "quantity": 2,
                        "unitPrice": 100.00,
                        "subTotal": 200.00,
                        "discount": 0.00,
                        "total": 200.00
                    }
                ],
                "totalAmount": 920.00,
                "isCanceled": true
            },
            "success": true,
            "message": "Sale cancel successfully",
            "errors": []
        },
        "success": true,
        "message": "",
        "errors": []
    }

---

