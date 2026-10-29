# MyMedia - E-Commerce Platform

Uma plataforma completa de comércio eletrónico com arquitetura de micro-serviços (Frontend, Backoffice e API RESTful), desenvolvida como projeto final em .NET 8.

## Tecnologias Utilizadas
* **Backend:** C#, .NET 8 Web API, Entity Framework Core (Code-First), LINQ
* **Frontend / Backoffice:** Blazor (WebAssembly/Server), Bootstrap 5
* **Base de Dados:** SQL Server
* **Segurança:** ASP.NET Core Identity, JWT (JSON Web Tokens), Autenticação baseada em Roles

## Funcionalidades Principais
O sistema suporta três perfis de utilizador distintos:
* **Administrador:** Gestão de utilizadores, aprovação de produtos, definição de margens de lucro, gestão de categorias e expedição de encomendas (lógica de redução de stock).
* **Fornecedor:** Submissão de produtos para aprovação (estado Pendente -> Ativo), definição de preço base e visualização do catálogo próprio.
* **Cliente:** Navegação no catálogo, pesquisa hierárquica por categorias, gestão de carrinho de compras e checkout.

## Base de Dados
A base de dados foi modelada em SQL Server.

## Aplicação

<div align="center">

### Página Inicial
<img src="https://github.com/user-attachments/assets/86f96b2a-0276-4a8d-a37e-2f61b67d9641" width="800" alt="homescreen" />
<br><br>

### Catálogo e Navegação
<img src="https://github.com/user-attachments/assets/10fb7cb5-eae3-4ecf-9470-ce9d05bdf082" width="350" alt="musicpage" />
<br><br>

### Dashboard do Admin
<img src="https://github.com/user-attachments/assets/b34c19d9-ee0b-44e5-87c0-205561d9bfc4" width="800" alt="admin_edit" />

</div>

## Como correr o projeto localmente

1. **Clonar o repositório:**
   ```bash
   git clone [https://github.com/O-Teu-User/O-Teu-Repositorio.git](https://github.com/O-Teu-User/O-Teu-Repositorio.git)
