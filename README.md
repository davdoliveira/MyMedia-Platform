

https://github.com/user-attachments/assets/44ed1fe4-4f82-43fd-a091-ce2813fccad4

<img width="724" height="905" alt="musicpage" src="https://github.com/user-attachments/assets/10fb7cb5-eae3-4ecf-9470-ce9d05bdf082" />
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
## 🗄️ Base de Dados
A base de dados foi modelada em SQL Server. Abaixo o diagrama Entidade-Relacionamento:
![Diagrama MER](Docs/ModeloER.png)

## Aplicação
![Página Inicial](Docs/homescreen.png)
![Menus] (Docs/musicpage.png)
![Dashboard do Admin](Docs/admin_edit.png)
![Versão Mobile](Docs/mobile_test.mp4)

## Como correr o projeto localmente

1. **Clonar o repositório:**
   ```bash
   git clone [https://github.com/O-Teu-User/O-Teu-Repositorio.git](https://github.com/O-Teu-User/O-Teu-Repositorio.git)
