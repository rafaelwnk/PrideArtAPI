# PrideArt API

PrideArt é um portal desenvolvido como parte de um projeto acadêmico com o objetivo de valorizar e dar visibilidade à produção artística da comunidade LGBTQIAPN+. A plataforma oferece um espaço seguro onde artistas possam compartilhar suas obras, reconhecendo a arte como uma forma de expressão, resistência e identidade. Esta API foi desenvolvida com .NET  e se conecta a uma interface desenvolvida em Angular, disponível em: [PrideArt](https://github.com/rafaelwnk/PrideArt).

## Tecnologias Utilizadas

- .NET 8
- Entity Framework Core
- JWT Authentication

## Instalação e Execução
Siga os passos abaixo para configurar e executar o projeto localmente.

### Pré-requisitos
Antes de iniciar, certifique-se de ter os seguintes itens instalados:

- [.NET](https://dotnet.microsoft.com/en-us/download)

### 1. Clone o repositório:
```bash
git clone https://github.com/rafaelwnk/PrideArtAPI
cd PrideArtAPI
```

### 2. Restaure as dependências::
```bash
dotnet restore
```

### 3. Execute as migrações:
```bash
dotnet ef database update
```

### 4. Execute o projeto:
```bash
dotnet run
```

### 5. A API estará disponível por padrão em:
```bash
http://localhost:5131
```

## Contribuições

Se você tiver alguma sugestão de melhoria, ideia nova ou perceber algo que pode ser ajustado:

    1.Faça um fork do repositório

    2.Crie uma nova branch: git checkout -b feature

    3.Faça um commit: git commit -m 'feat: new feature'

    4.Faça o push para a branch : git push origin feature

    5.Abra um pull request