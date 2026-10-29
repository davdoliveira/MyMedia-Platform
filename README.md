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

## Screenshots
*(Coloca aqui 3 ou 4 imagens. Uma da loja, uma do carrinho, e uma do dashboard do Admin. Exemplo de código de imagem: `![Página Inicial](Docs/screenshot_home.png)`)*

## Como correr o projeto localmente

1. **Clonar o repositório:**
   ```bash
   git clone [https://github.com/O-Teu-User/O-Teu-Repositorio.git](https://github.com/O-Teu-User/O-Teu-Repositorio.git)
