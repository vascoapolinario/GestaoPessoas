# GestaoPessoas

Uma aplicação .NET desenvolvida para fins de aprendizado sobre a arquitetura .NET.

## Sobre o Projeto

O **GestaoPessoas** é uma aplicação simples para gerenciamento de colaboradores, criada com o objetivo de estudar e aplicar conceitos fundamentais de desenvolvimento em .NET, incluindo:

- Arquitetura em camadas
- Injeção de dependência
- Padrões de design
- APIs RESTful

## Funcionalidades

A aplicação oferece um CRUD para gerenciamento de colaboradores, permitindo:

- Listar todos os colaboradores
- Obter detalhes de um colaborador específico
- Adicionar novos colaboradores
- Atualizar informações de colaboradores existentes
- Remover colaboradores

## Persistência de Dados

O projeto suporta dois tipos de persistência:

1. **PostgreSQL**: Implementação para base de dados PostgreSQL
2. **Arquivo JSON**: Implementação para persistência em arquivo JSON. Para desenvolvimento e testes locais

## Configuração Local

### Como criar e utilizar um arquivo de configuração local

Para personalizar as configurações apenas no seu ambiente de desenvolvimento, você pode criar um ficheiro de configuração local:

1. Na raiz do projeto `GestaoPessoas`, crie um arquivo chamado `appsettings.Development.LocalMachine.json`.
2. Insira as configurações que deseja sobrescrever apenas no seu ambiente, por exemplo:

```json
{
  "WorkerService": {
    "Implementation": "jsonfile"
  },
  "JsonWorkerService": {
    "FilePath": "./Workers.json"
  }
}
```

3. Ao iniciar a aplicação, as configurações do arquivo local serão carregadas automaticamente.
4. (Opcional): Em caso de uso de PostgreSQL, é necessario adicionar a string de conexão que pode ser configurada como user secrets:
   Para adicionar a string de conexão, utilize o comando:
 ```
 dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=nomedb;Username=username;Password=password"
 ```

 em que eve substituir `nomedb`, `username` e `password` pelos valores corretos do seu ambiente.

### Observações importantes sobre o arquivo local

- O arquivo `appsettings.Development.LocalMachine.json` está incluído no `.gitignore` e **não é versionado**
- Serve exclusivamente para configurações específicas do seu ambiente de desenvolvimento
- As configurações são carregadas automaticamente pelo ASP.NET Core

## Como Executar

1. Clone o repositório
2. Navegue até a pasta do projeto
3. Execute os comandos:

```
dotnet restore
dotnet build
dotnet run
```

4. Acesse a documentação da API em: `https://localhost:7011/swagger`

## Tecnologias Utilizadas

- .NET 8
- ASP.NET Core Web API
- System.Text.Json (para serialização JSON)
- Npgsql (para PostgreSQL)
- Swagger/OpenAPI

## Estrutura do Projeto

```
GestaoPessoas/
├── Controllers/          # Controladores da API
├── Services/            # Camada de serviços
├── Dtos/               # Objetos de transferência de dados
├── Program.cs          # Ponto de entrada da aplicação
└── appsettings.Development.LocalMachine.json    # Configurações da aplicação
```

## Contribuindo

Este projeto foi criado para fins educacionais e está aberto a contribuições! Sinta-se à vontade para:

- Abrir issues para reportar bugs ou sugerir melhorias
- Enviar pull requests com correções ou novas funcionalidades
- Compartilhar ideias e sugestões

## Aprendizado

Este projeto serve como um laboratório para explorar conceitos como:

- Configuração flexível de aplicações .NET
- Boas práticas de desenvolvimento de APIs
- Padrões de arquitetura de software
- Testes e validação de dados

---

**Nota**: Este é um projeto educacional. Todas as contribuições e sugestões são bem-vindas para torná-lo ainda mais útil como ferramenta de aprendizado!
