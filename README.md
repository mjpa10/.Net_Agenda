# APIAgenda

Este projeto é uma API para gerenciamento de contatos, que garante que os contatos não tenham e-mails ou telefones duplicados. A API permite a criação, leitura, atualização e exclusão (CRUD) de contatos, com validações específicas para garantir a integridade dos dados.

## Funcionalidades
- **CRUD de Contatos: Permite criar, ler, atualizar e excluir contatos.**
- **Validação de E-mails e Telefones: Garante que não existam contatos com e-mails ou telefones duplicados.**
- **Paginação: Suporte à paginação nas listas de contatos.**
- **Filtros: Permite filtrar contatos por nome, e-mail e telefone.**

## Tecnologias Utilizadas

- **.NET 7.0: Framework utilizado para construir a API.**
- **Entity Framework Core: Ferramenta de mapeamento objeto-relacional (ORM) para interação com o banco de dados MySQL**
- **MySQL: Banco de dados utilizado para armazenar os lembretes.**
- **AutoMapper: Biblioteca para mapeamento automático entre objetos.**
- **Swagger: Ferramenta para documentação interativa da API.**

## Pré-requisitos

- .NET SDK 7.0 ou superior
- MySQL
- Visual Studio 2022 ou outro IDE de sua preferência

## Configuração do Projeto

### 1. Clonando o Repositório

```bash
git clone https://github.com/mjpa10/.Net_Agenda.git
```

```bash
cd .NET_Agenda
```

### 2. Instalando Dependências

Use o comando abaixo para restaurar as dependências do projeto:

```bash
dotnet restore
```
```bash
cd API_Agenda
```
### 3. Configurando o Banco de Dados

- Crie as configurações de conexão no arquivo `appsettings.json` dentro da pasta `\API_Agenda`, seguindo esse modelo:
  
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=APIAgenda;Uid=seu_usuario;Password=sua_senha;"
  },
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
      }
    },
    "AllowedHosts": "*"
  }
```

- Execute as migrações para configurar o banco de dados:

```bash
dotnet ef migrations add MigracaoInicial
```
```bash
dotnet ef database update
```

### 4. Abra o Projeto na IDE:

Inicie sua IDE e abra o arquivo de solução (.sln) que corresponde ao projeto, No caso o `API_Agenda.sln`.

### 5. Compile o Projeto:

Na IDE, localize e selecione a opção para compilar o projeto. Normalmente, isso pode ser feito clicando com o botão direito no arquivo de solução ou projeto no Gerenciador de Soluções e selecionando "Compilar" ou "Build". Compile com o Http

### 6. Inicie o Projeto:

pós a compilação bem-sucedida, inicie o projeto usando a opção "Iniciar" ou "Run" na IDE. Pode ser usado o Atalaho F5 Também

## Estrutura do Projeto

- **Controllers**: Contém os controladores que gerenciam as requisições HTTP.
- **Models**: Define as classes de modelo utilizadas na aplicação.
- **Repositories**: Implementa os padrões de repositório e unidade de trabalho.
- **Services**: Contém os serviços responsáveis pela lógica de criação e atualização de contatos.
- **DTOs**: Data Transfer Objects utilizados para transferência de dados entre as camadas.
- **Pagination**: Contém as classes responsáveis pela paginação e filtros de lembretes.
  
