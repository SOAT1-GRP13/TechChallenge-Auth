# Tech Challenge - SOAT1 - Grupo 13 - Microserviço de autenticação </h1>

![GitHub](https://img.shields.io/github/license/dropbox/dropbox-sdk-java)

# Resumo do projeto

Este projeto é desenvolvido em C# com .NET 6, seguindo os princípios da arquitetura hexagonal. Seu objetivo principal é permitir que os usuários se autentiquem e recebam um token JWT, que pode ser utilizado em nossas APIs que requerem autenticação. Atualmente, os usuários têm a opção de se autenticar como administradores, usando o CPF ou o nome.

Para garantir a segurança das informações de acesso ao banco de dados PostgreSQL, o projeto faz uso do Secret Manager. Isso permite que as credenciais do banco de dados sejam armazenadas de forma segura e acessadas apenas por autorizações apropriadas. Essa abordagem fortalece a segurança dos dados sensíveis.

Após a geração do token JWT, as informações do cliente são salvas em um banco de cache. Nesse caso, o projeto utiliza o serviço de banco de dados NoSQL da AWS, o DynamoDB, para armazenar e gerenciar as informações do cliente de forma eficiente. Isso contribui para um desempenho ágil e escalabilidade do sistema.

Ao longo do desenvolvimento, estaremos fazendo entregas incrementais e criando releases no GIT para acompanhar o progresso do projeto. Esperamos que este trabalho demonstre nosso conhecimento teórico e prático adquirido durante a pós-graduação, além de servir como um exemplo de aplicação das melhores práticas de arquitetura em projetos de software.

Sinta-se à vontade para entrar em contato conosco se tiver alguma dúvida ou sugestão. Agradecemos pelo interesse em nosso projeto!


> :construction: Projeto em construção :construction:

License: [MIT](License.txt)

# Bando de Dados

Inicialmente, nosso projeto foi concebido como um monolito, e, naquela época, tínhamos a necessidade de um banco de dados com alta integridade dos dados e um bom relacionamento entre as tabelas. Portanto, escolhemos o banco de dados PostgreSQL no início do projeto devido à familiaridade do grupo com ele.

Com o desenvolvimento do projeto, surgiu a necessidade de separar o processo de autenticação em um microsserviço. Compreendemos que, em algum momento, nosso monolito será dividido em vários microsserviços, e acreditamos que é uma prática recomendada separar também a base de dados. No entanto, neste momento, consideramos que a melhor estratégia é migrar o banco de dados PostgreSQL conforme está configurado para um serviço gerenciável na nuvem, que, no nosso caso, será o AWS RDS. Isso ocorre porque a maior parte da aplicação continua como um monolito.

Nossos critérios de decisão incluem a compatibilidade e a redução da complexidade, uma vez que não desejamos fazer alterações no código neste momento, concentrando-nos principalmente na migração para a nuvem. Acreditamos que uma migração em fases é a estratégia mais apropriada, seguindo o paradigma dos "5 R's" da AWS, que incluem Rehost, Refactor, Replatform, Rebuild e Replace.

Quando ocorrer a divisão do monolito em microsserviços, nossa equipe realizará uma reavaliação e redefinição da solução de base de dados a ser utilizada por cada microsserviço.

# ⌨️ Testando a API

Você pode baixar o projeto e executá-lo em seu ambiente local com o Visual Studio. Embora o projeto esteja hospedado em nossa infraestrutura na AWS, também o apresentamos aos professores em um vídeo demonstrando seu funcionamento.

Isso permite que você experimente a funcionalidade da API em seu próprio ambiente e explore seu comportamento. Se tiver alguma dúvida ou precisar de assistência, sinta-se à vontade para entrar em contato conosco.

# ✔️ Tecnologias utilizadas

- ``.Net 6``
- ``Postgres``
- ``DynamoDB``
- ``Secrets Manager``


# Autores

| [<img src="https://avatars.githubusercontent.com/u/28829303?s=400&v=4" width=115><br><sub>Christian Melo</sub>](https://github.com/christiandmelo) |  [<img src="https://avatars.githubusercontent.com/u/89987201?v=4" width=115><br><sub>Luiz Soh</sub>](https://github.com/luiz-soh) |  [<img src="https://avatars.githubusercontent.com/u/21027037?v=4" width=115><br><sub>Wagner Neves</sub>](https://github.com/nevesw) |  [<img src="https://avatars.githubusercontent.com/u/34692183?v=4" width=115><br><sub>Mateus Bernardi Marcato</sub>](https://github.com/xXMateus97Xx) |
| :---: | :---: | :---: | :---: |
