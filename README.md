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

**Observações técnicas**

Class Library:
- App_Dominio: utilizada em todos os sistemas. Descreve a arquitetura usando *generics* para as camadass de controller e model. Contém as interfaces, controle de sessão e demais bibliotecas necessárias para os demais sistemas.

Banco de dados:
- SQL Server

Arquitetura:
- MVC






