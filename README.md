# dwm-imovel
Sistema de gestão de vendas de imóveis (workflow de vendas)

O DWM-Imóvel tem como principal objetivo oferecer uma agilidade e qualidade do fluxo de processos da venda de imóveis novos. Outro objetivo é abastecer a alta gerência de informações que possibilitem uma melhor gestão estratégica. 
Todas as vendas estarão integradas com o sistema financeiro (DWM-Finanças). 

A integração ocorrerá no momento da aprovação da etapa de comissionamento, quando deverá gerar os registros de contas a pagar aos corretores, gerentes e coordenadores (e/ou construtora), bem como a inclusão da operação de contas a receber para a imobiliária. 

O processo de venda é realizado em etapas. Essas etapas são configuradas diretamente em banco de dados pelo administrador. Cada imobiliária poderá ter processos de vendas distintos uma da outra. Tudo começa com o cadastro do cliente e a proposta de venda do imóvel de um dado empreendimento.

Exemplo:

1. Proposta 
2. Em análise 
3. Reanálise 
4. Análise de crédito 
5. Contabilizado 
6. Comissão 
7. Aprovar comissão 

Para cada etapa exise um perfil de usuário habilitado para aprovar ou revogar a respetiva etapa da venda. Em cada etapa o corretor ou gerente poderá incluir comentários, realizar upload de documentos (RG, comprovante de renda, comprovante de residência).

Existe uma etapa especial onde é calculado a comissão da imobiliária e o rateio dos valores entre os corretores e gerentes de venda.

A gestão dos usuários e perfis é realizado pelo sistema de segurança.

**Processo de venda**

**1. Cadastrar proposta** 

Incluir, alterar, excluir e listar propostas de vendas 
Quando incluir a proposta, disponibilizar opção para incluir um novo cliente. 
Informar o empreendimento, unidade e modelo. Informar o VGV e a comissão total. 
Somente usuários com perfil definidos para esta etapa poderão incluir proposta de venda. 
Não deve ser permitido excluir propostas que já estejam em outras etapas, assim como não será permitido alterar o VGV e Comissão total das propostas que já tenham financeiro. 
Quando cadastra a proposta de venda, esta já entra automaticamente na primeira etapa do processo da venda. 

**2. Aprovar ou recusar uma etapa**

Esta funcionalidade tem como objetivo possibilitar ao usuário com perfil apropriado para a etapa dar o seu parecer, aprovar ou recusar e avançar ou retornar à etapa anterior conforme fluxo cadastrado no exemplo acima. 

**3. Aprovar ou recusar etapa de contabilização**

Esta funcionalidade tem como objetivo possibilitar ao usuário com perfil apropriado para a etapa dar sua manifestação, aprovar ou recusar e avançar ou retornar à etapa anterior conforme fluxo cadastrado no exemplo acima.

Nesta etapa o responsável se encarregará de fazer o upload dos documentos da venda para dentro do sistema. 

**4. Impressão de documentos**

Este recurso permite realizar a impressão de 5 documentos considerados essenciais da venda.  
A impressão destes documentos deverá ocorrer na etapa de contabilização. 

**5. Comissão**

Esta funcionalidade tem como objetivo possibilitar ao usuário com perfil apropriado para a etapa dar sua manifestação, aprovar ou recusar e avançar ou retornar à etapa anterior conforme fluxo cadastrado no exemplo acima. 

Nesta etapa, além de aprovar e recusar, o responsável se encarregará de realizar o desmembramento da comissão entre os corretores, gerente de equipe e coordenador. Se o comissionamento não for aprovado pelo proprietário, esta etapa volta a ser executada para que o administrativo possa rever a distribuição dos valores. 

**6. Aprovar comissão**

Esta funcionalidade tem como objetivo possibilitar ao proprietário aprovar ou recusar o comissionamento. Se aprovado, será gerado os registros de contas a pagar para os corretores, gerente de equipe e coordenador, bem como a operação de contas a receber da imobiliária. 

A gestão financeira das vendas não é objeto do sistema e será tratado pelo DWM-Finanças. 

**7. Incluir comentários**

Para cada etapa do processo de venda, os perfis envolvidos poderão incluir comentários sobre o andamento da respectiva etapa, descrevendo as pendências e ou ações que estão sendo realizadas. Se o processo estiver demorando muito, o responsável poderá incluir um manifesto justificando a demora.   

Cada comentário poderá ser enviado um e-mail aos envolvidos notificando que há mensagens novas sobre o processo da respectiva venda para serem lidas. 

**8. Transferir a venda para outro gerente**

Toda venda quando cadastrada é vinculado a ela o gerente de equipe. O coordenador se julgar necessário, poderá atribuir as etapas restantes do processo para outro gerente (e gravar uma justificativa). Ao confirmar a mudança, o sistema vai enviar um e-mail para o novo gerente notificando que a venda passou a estar sob sua responsabilidade a partir desta data. 

**9. Enviar e-mails para os gerentes e coordenadores**

O sistema possibilitará ao coordenador encaminhar mensagens de e-mail para os gerentes sobre uma determinada venda 

# Visão arquitetural

O texto a seguir tem o objetivo de apresentar a arquitetura que será utilizada na construção do sistema, identificando cada componente e suas ligações com os demais objetos que compõe a estrutura da aplicação. 

A ideia central do projeto é construir toda a aplicação usando a tecnologia MVC5 da Microsoft onde é possível isolar bem as camadas de dados, negócios e apresentação. 

O projeto será basicamente construído em camadas. A camada de apresentação (View) utilizará a tecnologia HTML com javascript/Ajax (JQuery). 

Haverão as camadas de acesso aos dados e camada com as regras de negócio que regem as ações da aplicação. 

Haverão também o consumo de componentes internos para a integração com o Sistema de Segurança e com as camadas de negócio da própria aplicação, além de outras integrações que possam ser necessárias para disponibilização de informações determinas pelos gestores de negócio. 

A seguir será feito um detalhamento arquitetural de todo sistema. Serão mostrados os principais componentes, pacotes e camadas que irão compor o sistema. 

Serão demonstrados alguns diagramas com as interligações entre as funcionalidades de cada módulo e seus respectivos componentes. 

Será descrito como o sistema fará o controle de segurança dos usuários aos recursos da aplicação, bem como o registro de auditoria (LOGs) das ações desempenhadas pelos usuários, além do tratamento dado ao comportamento da aplicação devido à ocorrência de possíveis ou eventuais erros que venham a surgir. 

Será explanado o mecanismo para manuseio dos dados que se farão através de instruções LINQ ou Stored Procedures. 

– Interfaces Internas e/ou Externas 

Tanto o Sistema de Condomínios quanto o Sistema de Segurança dependem do componente “App_Dominio”. 

O componente **App_Dominio** constitui um conjunto de objetos usados como interfaces nas camadas de controle e de acesso a dados na construção dos sistemas. Eles regem os modelos de padronização da arquitetura. 

O componente **“App_Dominio”** contém os objetos responsáveis em estabelecer a comunicação da aplicação com o ambiente de segurança, estabelecer a conexão com o banco de dados e propor de forma organizada e padronizada os modelos de classes e objetos - distribuídos em camadas - que serão desenvolvidos no Sistema. 

**Padrões de Projeto**

- Codificação e Nomenclatura 

Todo o código-fonte foi escrito sob o encoding UTF-8. Quando possível, o mesmo deve ficar explícito (exemplo: arquivos JSon); 

A Instanciação dos objetos sempre ficará a cargo da infraestrutura do framework. De forma a não violar a inversão de controle nesses casos; 

As exceções deverão ser tratadas por camada, ou seja, se uma exceção for levantada na camada Model, ela deverá ser tratada primeiro antes de ser relançada para a camada superior.  

Relatórios foram feitos em HTML, com a lógica toda interna no código fonte. Não foi utilizado nenhum gerador de relatório. 

Todo a acesso aos dados ocorre através do Entityframework, instruções LINQ ou Stored Procedures. 

- Organização de Artefatos Gerados 

A arquitetura ou Solução web do sistema está organizada conforme mostrada abaixo: 

No diretório WEB haverá uma pasta raiz chamada Sindemed que contém os objetos e a implementação das funcionalidades do Sistema de Cadastro de Associados, tomando como referência os contratos e modelos descritos no componente App_Dominio. 

Estrutura de implementação: 

**App_Code**: diretório que armazenará as funções “@htmlhelpers” com as “partialpages”, tais como as “ListofValues - LOV” (tela modal com a lista de valores de uma determinada entidade para pesquisa e seleção dentro de um grid.  

**App_Data**: diretório que armazenará arquivos JSon, XML, TXT e Logs de erro manipulados pela aplicação. 

**Content**: diretório que armazenará as imagens e folhas de estilo do sistema. 

**Controllers**: diretório que armazenará as classes de controle (facade) que acionarão os componentes de negócio. 

**Models**: diretório que armazenará as classes de entidade, o mapeamento para o modelo relacional, as classes de negócio e as classes “repositories” (ViewModels) utilizadas nos modelos de entrada de dados e filtros de consulta dos formulários da camada de apresentação. 

**Scripts**: diretório que contém os arquivos javascript utilizados pela aplicação, incluindo o bootstrap. Nele também ficarão armazenados os arquivos javascript da ferramenta JQuery. 
 
**Views**: diretório que armazenará os arquivos razor “cshtml” da camada de apresentação. 

**Bin**: diretório que armazenará o código compilado da aplicação. 


# APP_DOMINIO

**Component**: Este componente contém os objetos utilizados para envio de e-mails, os modelos de objetos usados nas listagens dos grids e a superclasse “Repository” que dela derivam todos os demais objetos da aplicação que serão usados para apresentação dos dados (Viewmodels). 

**Contratos**: Este componente contém todas as interfaces que regem a padronização das principais funcionalidades do sistema, tais como:  

- Exibição de conteúdo em combobox (dropdownlist); 
- CRUDS (inclusão, alteração, exclusão e consulta) de tabelas e cadastros; 
- Listagem de dados nos grids da tela; 
- Controle de paginação nos grids da tela; 
- Retorno e controle de mensagens de validação 
- Controle de segurança (autenticação de usuário, menus do sistema, etc) 

**Controllers**: Este componente contém os objetos responsáveis pela camada de controle da aplicação. Ele implementa os métodos em alto nível (Superclasse) responsáveis pelo controle das operações de exibição de mensagens, operações de inclusão, alteração, exclusão, consulta, operações de listagem entre outras. Todas os objetos controllers das aplicações herdam das superclasses deste Controller.

**Entidades**: Este componente possui as classes que modelam as entidades do banco de dados. Seus atributos correspondem de forma idêntica aos atributos das tabelas no banco de dados. 

**Repositories**: Este componente contém os objetos usados pelo Sistema de Segurança  

**Security**: Este componente implementa as classes responsáveis pela comunicação entre as demais camadas deste componente com o Sistema. Implementa também as operações de segurança para autenticação do usuário, esqueci minha senha e alteração de senha.  
 
# Armazenamento e Recuperação de dados 

SQL Server

A persistência e recuperação de dados e o controle operacional serão feitas através de instruções LINQ que mapearão diretamente as tabelas do banco de dados ou nos casos mais essenciais, através de stored procedures. Essas stored procedures estarão assinadas no objeto “App_DominioContext” constante na camada “Entidades” do componente “App_Dominio” e serão chamadas nas classes de negócio constantes na camada “Models” da aplicação. 

A aplicação realiza conexão em duas bases de dados: banco de dados do sistema e banco de dados de Segurança. 

A conexão é feita através de dois componentes, respectivamente: *ApplicationContext* e *SecurityContext*

# Auditoria e Tratamento de Erros 

As exceções serão tratadas por camada, isto é, se uma exceção for lançada na camada de serviço, a mesma deve ser primeiramente tratada nesta camada antes de ser lançada para uma camada superior. 

Na aplicação web haverá uma camada macro de tratamento de erro no qual um bloco de código possivelmente não tratado pelo desenvolvedor será capturado pela aplicação e logado para posterior avaliação. 

Existirá uma classe chamada “App_DominioException” constante no componente “App_Dominio” que vai herdar da classe “Exception”, nativa do framework. Esta classe será responsável pelo tratamento da manipulação dos erros nas 3 camadas da aplicação. Ela se encarregará de atribuir as mensagens amigáveis ao usuário e registrar em arquivo XML dinâmico (um arquivo diário contendo sequencialmente as ocorrências), o trace com a descrição resumida e detalhada do correspondente erro. A consulta a esses arquivos poderá ser tratada e disponibilizada a partir de uma funcionalidade específica de consulta de erros.  

Esta funcionalidade estará cadastrada no sistema de segurança e conterá os links para os arquivos diários de logs. 

O LOG de auditoria na camada web ou transações de sistema via comandos LINQ ou stored procedures serão armazenados em banco de dados. O registro deste LOG será identificado por um número único, sequencial, que não poderá ser excluído por questões de segurança e guardará o LOGIN do usuário, data e hora que a funcionalidade foi executada, identificação da funcionalidade, a ação realizada que pode ser de inclusão, alteração, consulta ou exclusão e uma descrição resumida/detalhada da ocorrência. Também será registrado o namespace do processo. 

O registro ocorrerá de acordo com a relevância do caso de uso, ou seja, processos simples como cadastros básicos possuirão uma ocorrência no log de auditoria, todavia processos mais complexos, que envolvam cálculos ou atualizações em várias tabelas terão mais de um registro auditado para a mesma funcionalidade. Nestes casos de alta relevância, além de registrar os atributos citados acima, também serão registrados os atributos e valores antes e após a execução da operação. 

Será possível também, dentro de um mesmo processo, registrar vários logs como se fosse uma espécie de trilha percorrida, mantendo assim um histórico mais minucioso e detalhado das operações realizadas, criando assim pontos de impacto para melhor depuração de possíveis falhas que possam vir a ocorrer. 

Assim como o log de erros, o log de auditoria também possuirá uma funcionalidade cadastrada no sistema de segurança, cujo objetivo é exibir a auditoria de acordo com os parâmetros disponibilizados ao usuário. 

Já nos processos Batch, o LOG de auditoria e erros será realizado por JOB e armazenado em arquivo texto diário. O LOG será tratado a partir da camada de controle e vai registrar o trace das operações realizadas pelo usuário em cada camada, similar a uma memória de cálculo, registrando o passo a passo das ações executadas no sistema. 

Exemplo: 

<?xml version="1.0" encoding="utf-8"?> 

<LOG> 

  <LOG ID="1086159964" Data="14/03/2014 11:24:19"> 

    <Sessao>nbro2hegv3u1hkvptls0nc5n</Sessao> 

    <IP>::1</IP> 

    <HostName>ANDRE-VAIO\André</HostName> 

    <Mensagem>O índice estava fora dos limites da matriz.</Mensagem> 

    <NameSpace>Sindemed.Models.Report.RelacaoGeralReport =&gt; report01/listparam</NameSpace> 

    <Context /> 

    <StackTrace>   em Sindemed.Models.Report.RelacaoGeralReport.Bind(Nullable`1 index, Int32       pageSize, Object[] param) na c:\Users\André\Source\Repos\SINDMED\Sindemed\Models\Report\RelacaoGeralReport.cs:linha 31 

   em App_Dominio.Component.ReportRepository`2.ListRepository(Nullable`1 index, Int32 pageSize, Object[] param) na c:\Users\André\Source\Repos\App_Dominio\App_Dominio\App_Dominio\Component\ListViewRepository.cs:linha 97 

   em App_Dominio.Component.ListViewRepository`2.getPagedList(Nullable`1 index, Int32 pageSize, Object[] param) na c:\Users\André\Source\Repos\App_Dominio\App_Dominio\App_Dominio\Component\ListViewRepository.cs:linha 30</StackTrace> 

</LOG> 

# Segurança 

Todas as funcionalidades do sistema estão cadastradas no banco de dados do sistema de segurança. Cada funcionalidade está vinculada a um ou muitos grupos de usuários. Somente o usuário que pertencer ao grupo poderá visualizar e acessar a respectiva funcionalidade. 

Quando o usuário efetua o seu LOGIN, a aplicação verifica a qual grupo ele pertence e recupera todas as funcionalidades do respectivo grupo para exibir o menu de opções do site. 

**Tempo de Sessão**

Existe um relógio interno que cronometra o tempo que o usuário permanece estático em uma dada página. Sempre que o usuário muda de tela, o sistema checa esse tempo e caso tenha atingido um limite de 15 minutos (esse tempo é parametrizado), o sistema encerra a sessão do usuário e redireciona para a tela de LOGIN para que ele faça uma nova autenticação. 

**Acesso sem autenticação**

Se algum usuário não credenciado tentar acessar qualquer formulário do sistema diretamente pela URL sem proceder a autenticação, o sistema também irá redirecioná-lo para a tela de LOGIN.  

Acesso a uma página não autorizada, mesmo estando autenticado: 

Outro cenário que pode ocorrer é o usuário estar autenticado e tentar acessar uma página que não está autorizada para o grupo a que pertence.  

Por exemplo, um determinado cliente, uma vez “logado”, pode tentar acessar o cadastro de outro cliente ou tentar acessar o relatório com a relação geral de todos os clientes.  

Esta funcionalidade só é disponibilizada para quem pertencer ao grupo “Administração”.  

O grupo “Cliente” não tem acesso a esse recurso. Porém, se o cliente tentar fazer o acesso, o sistema irá apresentar uma mensagem informando que ele não está autorizado a executar tal operação. 

# Sistema de Segurança 

O Sistema de Segurança faz parte do contexto do projeto. É um recurso apartado e tem como objetivo administrar as contas de usuários que farão acesso ao Sistema e também ao Sistema Financeiro/Contábil. 

Todas as funcionalidades de cada sistema ficarão gravadas em banco de dados. Haverá um ou mais de um usuário administrador que terá acesso ao Sistema de Segurança para executar a gestão das contas dos usuários. 

Os usuários estarão sempre vinculados a um grupo. Cada grupo terá suas funcionalidades específicas. Os usuários só conseguirão visualizar e acessar as funcionalidades atribuídas ao seu grupo. 

No Sistema de segurança também é possível consultar o Log de Auditoria de todas as operações realizadas no sistema. 




