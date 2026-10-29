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

# MyMedia - E-Commerce Platform

Uma plataforma completa e distribuída de comércio eletrónico desenvolvida em .NET 8. Este projeto implementa uma arquitetura orientada a serviços, separando a lógica de negócio (API RESTful), o painel de administração (Backoffice) e a loja de clientes (Frontend / Mobile).

O sistema foi desenhado para suportar um modelo de negócio B2B2C, onde múltiplos fornecedores podem submeter produtos para aprovação e comercialização através da plataforma.

## Arquitetura do Sistema

O projeto está dividido em quatro camadas principais para promover a reutilização de código e a separação de responsabilidades:

- **TP.Backend.API:** Web API RESTful segura que centraliza as regras de negócio, o acesso à base de dados e a emissão de tokens JWT.
- **TP.Admin.Gestao:** Aplicação Web (Blazor) dedicada ao painel de controlo (Backoffice), exclusiva para perfis com privilégios de gestão.
- **TP.Frontend:** Aplicação cliente (Blazor Web / Hybrid) para navegação no catálogo, gestão do carrinho e checkout, preparada para comunicação via Dev Tunnels.
- **TP.Shared.RCL:** Razor Class Library que contém os DTOs (Data Transfer Objects), serviços HTTP partilhados e componentes visuais reutilizáveis.

## Tecnologias Utilizadas

- **Backend:** C#, .NET 8 Web API, Entity Framework Core (Code-First), LINQ
- **Frontend / Backoffice:** Blazor (WebAssembly/Server), Bootstrap 5
- **Base de Dados:** SQL Server
- **Segurança:** ASP.NET Core Identity, JWT (JSON Web Tokens), controlo de acesso baseado em funções (RBAC)

## Funcionalidades Principais

O sistema suporta três perfis de utilizador distintos, cada um com permissões e fluxos de trabalho específicos, controlados através de *Role-Based Access Control (RBAC)*.

- **Administrador (Backoffice):**
  - **Gestão de Utilizadores:** Aprovação de contas de fornecedores e clientes, permitindo a transição do estado *Pendente* para *Ativo*.
  - **Gestão de Margens de Lucro:** Análise dos produtos submetidos pelos fornecedores e definição de uma margem de lucro percentual. O sistema calcula automaticamente o preço final com base no preço base e na margem definida.
  - **Moderação do Catálogo:** Aprovação e inativação de produtos no catálogo geral, garantindo o controlo sobre os produtos disponibilizados aos clientes.
  - **Gestão de Expedição:** Processamento de encomendas e atualização dos respetivos estados, desencadeando a redução efetiva do stock na base de dados.

- **Fornecedor (Área Privada):**
  - **Gestão de Inventário:** Criação e submissão de novos produtos, com definição do preço base e da quantidade disponível em stock.
  - **Acompanhamento de Estado:** Consulta do estado dos produtos submetidos, permitindo distinguir entre produtos *Pendentes* e *Ativos*.
  - **Submissão para Aprovação:** Os produtos criados não ficam imediatamente disponíveis ao público, sendo necessária a aprovação do administrador antes da sua disponibilização no catálogo.

- **Cliente (Loja):**
  - **Navegação e Filtragem do Catálogo:** Exploração dos produtos através de uma hierarquia de categorias (categoria principal, subcategoria e subcategoria de terceiro nível), com recurso a consultas LINQ.
  - **Carrinho de Compras:** Gestão de um carrinho de compras persistente e reativo, utilizando `LocalStorage` para manter os dados entre sessões de navegação.
  - **Finalização de Encomendas (Checkout):** Conclusão do processo de compra com verificação preventiva da disponibilidade de stock e seleção dos métodos de entrega.

## Base de Dados

A base de dados foi modelada em SQL Server, utilizando Entity Framework Core com a abordagem Code-First para definir e gerir a estrutura dos dados.

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
