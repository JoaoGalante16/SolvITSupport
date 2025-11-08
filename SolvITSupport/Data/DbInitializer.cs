using Microsoft.AspNetCore.Identity;
using System.Linq; // Certifique-se de que esta linha existe
using SolvITSupport.Models;

namespace SolvITSupport.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            // --- CRIAÇÃO DE PAPÉIS ---
            if (!context.Roles.Any())
            {
                string[] roles = { "Administrador", "Atendente", "Usuario" };
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // --- CRIAÇÃO DE UTILIZADORES ---
            if (!context.Users.Any())
            {
                var admin = new ApplicationUser { UserName = "admin@solvit.com", Email = "admin@solvit.com", NomeCompleto = "Admin do Sistema", EmailConfirmed = true, Ativo = true };
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, "Administrador");

                var atendente = new ApplicationUser { UserName = "carlos.silva@solvit.com", Email = "carlos.silva@solvit.com", NomeCompleto = "Carlos Silva", EmailConfirmed = true, Ativo = true };
                await userManager.CreateAsync(atendente, "Atendente@123");
                await userManager.AddToRoleAsync(atendente, "Atendente");

                var usuario2 = new ApplicationUser{UserName = "maria.joao@solvit.com", Email = "maria.joao@solvit.com", NomeCompleto = "Maria João", EmailConfirmed = true, Ativo = true};
                await userManager.CreateAsync(usuario2, "Usuario@123");
                await userManager.AddToRoleAsync(usuario2, "Usuario"); ;

                var usuario = new ApplicationUser { UserName = "ana.costa@solvit.com", Email = "ana.costa@solvit.com", NomeCompleto = "Ana Costa", EmailConfirmed = true, Ativo = true };
                await userManager.CreateAsync(usuario, "Usuario@123");
                await userManager.AddToRoleAsync(usuario, "Usuario");

                var usuario3 = new ApplicationUser { UserName = "rui.pedro@solvit.com", Email = "rui.pedro@solvit.com", NomeCompleto = "Rui Pedro", EmailConfirmed = true, Ativo = true };
                await userManager.CreateAsync(usuario3, "Usuario@123");
                await userManager.AddToRoleAsync(usuario3, "Usuario");            

                var usuario4 = new ApplicationUser { UserName = "diogo.fernandes@solvit.com", Email = "diogo.fernandes@solvit.com", NomeCompleto = "Diogo Fernandes", EmailConfirmed = true, Ativo = true };
                await userManager.CreateAsync(usuario4, "Usuario@123");
                await userManager.AddToRoleAsync(usuario4, "Usuario");

                var usuario5 = new ApplicationUser { UserName = "sofia.lopes@solvit.com", Email = "sofia.lopes@solvit.com", NomeCompleto = "Sofia Lopes", EmailConfirmed = true, Ativo = true };
                await userManager.CreateAsync(usuario5, "Usuario@123");
                await userManager.AddToRoleAsync(usuario5, "Usuario");

                var atendente3 = new ApplicationUser { UserName = "miguel.costa@solvit.com", Email = "miguel.costa@solvit.com", NomeCompleto = "Miguel Costa", EmailConfirmed = true, Ativo = true };
                await userManager.CreateAsync(atendente3, "Atendente@123");
                await userManager.AddToRoleAsync(atendente3, "Atendente");

                var atendente4 = new ApplicationUser { UserName = "laura.gomes@solvit.com", Email = "laura.gomes@solvit.com", NomeCompleto = "Laura Gomes", EmailConfirmed = true, Ativo = true };
                await userManager.CreateAsync(atendente4, "Atendente@123");
                await userManager.AddToRoleAsync(atendente4, "Atendente");
            }

            // --- CRIAÇÃO DE CATEGORIAS, PRIORIDADES E STATUS (AGORA INDEPENDENTE) ---
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(new Category { Nome = "Hardware" }, new Category { Nome = "Software" }, new Category { Nome = "Rede" }, new Category { Nome = "Acesso" }, new Category { Nome = "Outro" });
            }
            if (!context.Priorities.Any())
            {
                context.Priorities.AddRange(new Priority { Nome = "Alta" }, new Priority { Nome = "Média" }, new Priority { Nome = "Baixa" });
            }
            if (!context.Statuses.Any())
            {
                context.Statuses.AddRange(new Status { Nome = "Aberto", IsFinalStatus = false }, new Status { Nome = "Em Andamento", IsFinalStatus = false }, new Status { Nome = "Resolvido", IsFinalStatus = true});
            }

            await context.SaveChangesAsync();

            // --- CRIAÇÃO DE CHAMADOS DE EXEMPLO ---
            if (!context.Tickets.Any())
            {
                // --- SECÇÃO DE DEBUG: Vamos encontrar o utilizador que está a falhar ---
                var anaUser = await userManager.FindByEmailAsync("ana.costa@solvit.com");             
                var mariaUser = await userManager.FindByEmailAsync("maria.joao@solvit.com");               
                var ruiUser = await userManager.FindByEmailAsync("rui.pedro@solvit.com");         
                var diogoUser = await userManager.FindByEmailAsync("diogo.fernandes@solvit.com");         
                var sofiaUser = await userManager.FindByEmailAsync("sofia.lopes@solvit.com");
                var carlosUser = await userManager.FindByEmailAsync("carlos.silva@solvit.com");
                var miguelUser = await userManager.FindByEmailAsync("miguel.costa@solvit.com");
                var lauraUser = await userManager.FindByEmailAsync("laura.gomes@solvit.com");

                // --- FIM DA SECÇÃO DE DEBUG ---

                // Se o código chegou até aqui, todos os 9 utilizadores existem.
                // Agora podemos adicionar os 25 tickets:

                context.Tickets.AddRange(
                    // ----- Hardware (Categoria 1) -----
                    new Ticket { Titulo = "Computador não liga", Descricao = "O PC da receção não dá sinal de vida.", CategoriaId = 1, PrioridadeId = 1, StatusId = 1, SolicitanteId = anaUser.Id },
                    new Ticket { Titulo = "Impressora encravada", Descricao = "A impressora HP do 2º piso está sempre a encravar o papel.", CategoriaId = 1, PrioridadeId = 2, StatusId = 2, SolicitanteId = mariaUser.Id, AtendenteId = carlosUser.Id },
                    new Ticket { Titulo = "Ecrã partido", Descricao = "O monitor do portátil caiu e o ecrã está partido.", CategoriaId = 1, PrioridadeId = 1, StatusId = 3, SolicitanteId = ruiUser.Id },
                    new Ticket { Titulo = "Rato sem fios não funciona", Descricao = "Já troquei as pilhas e o rato USB não liga.", CategoriaId = 1, PrioridadeId = 3, StatusId = 1, SolicitanteId = diogoUser.Id },
                    new Ticket { Titulo = "Computador muito lento", Descricao = "Demora 10 minutos a arrancar o Windows.", CategoriaId = 1, PrioridadeId = 2, StatusId = 2, SolicitanteId = sofiaUser.Id, AtendenteId = miguelUser.Id },
                    new Ticket { Titulo = "Pedido de novo teclado", Descricao = "A tecla 'A' do meu teclado partiu-se.", CategoriaId = 1, PrioridadeId = 3, StatusId = 3, SolicitanteId = anaUser.Id, AtendenteId = lauraUser.Id },
                    new Ticket { Titulo = "Dockstation não reconhece monitores", Descricao = "A dockstation Dell não projeta imagem para os monitores externos.", CategoriaId = 1, PrioridadeId = 2, StatusId = 1, SolicitanteId = mariaUser.Id },

                    // ----- Software (Categoria 2) -----
                    new Ticket { Titulo = "Erro ao abrir o Excel", Descricao = "O Excel apresenta a mensagem 'Erro de licença'.", CategoriaId = 2, PrioridadeId = 1, StatusId = 2, SolicitanteId = ruiUser.Id},
                    new Ticket { Titulo = "Instalação do Photoshop", Descricao = "Preciso do Adobe Photoshop para o departamento de marketing.", CategoriaId = 2, PrioridadeId = 2, StatusId = 1, SolicitanteId = diogoUser.Id },
                    new Ticket { Titulo = "Software CRM não atualiza", Descricao = "O nosso sistema de CRM está bloqueado na versão antiga.", CategoriaId = 2, PrioridadeId = 1, StatusId = 3, SolicitanteId = sofiaUser.Id, AtendenteId = carlosUser.Id },
                    new Ticket { Titulo = "VPN cai constantemente", Descricao = "A ligação VPN dura apenas 5 minutos e depois cai.", CategoriaId = 2, PrioridadeId = 2, StatusId = 2, SolicitanteId = anaUser.Id, AtendenteId = miguelUser.Id },
                    new Ticket { Titulo = "Antivírus bloqueia ficheiro", Descricao = "O antivírus está a bloquear um relatório importante, mas é seguro.", CategoriaId = 2, PrioridadeId = 3, StatusId = 1, SolicitanteId = mariaUser.Id },
                    new Ticket { Titulo = "Erro no sistema de faturação", Descricao = "Não consigo emitir faturas, o sistema dá erro 500.", CategoriaId = 2, PrioridadeId = 1, StatusId = 1, SolicitanteId = ruiUser.Id },
                    new Ticket { Titulo = "Outlook não envia emails", Descricao = "Os emails ficam presos na 'Caixa de Saída'.", CategoriaId = 2, PrioridadeId = 2, StatusId = 3, SolicitanteId = diogoUser.Id, AtendenteId = lauraUser.Id },

                    // ----- Rede (Categoria 3) -----
                    new Ticket { Titulo = "Internet muito lenta", Descricao = "A navegação está impossível hoje.", CategoriaId = 3, PrioridadeId = 2, StatusId = 1, SolicitanteId = sofiaUser.Id },
                    new Ticket { Titulo = "Não consigo aceder à pasta partilhada", Descricao = "A pasta //SERVER/Marketing não está acessível.", CategoriaId = 3, PrioridadeId = 2, StatusId = 2, SolicitanteId = anaUser.Id},
                    new Ticket { Titulo = "Wi-Fi não funciona na sala de reuniões", Descricao = "O sinal Wi-Fi está muito fraco ou inexistente na sala B.", CategoriaId = 3, PrioridadeId = 3, StatusId = 3, SolicitanteId = mariaUser.Id, AtendenteId = carlosUser.Id },
                    new Ticket { Titulo = "Cabo de rede danificado", Descricao = "Preciso de um cabo de rede novo para o meu posto.", CategoriaId = 3, PrioridadeId = 3, StatusId = 1, SolicitanteId = ruiUser.Id },
                    new Ticket { Titulo = "Acesso ao site interno bloqueado", Descricao = "A intranet não carrega, dá 'Página não encontrada'.", CategoriaId = 3, PrioridadeId = 2, StatusId = 2, SolicitanteId = diogoUser.Id, AtendenteId = miguelUser.Id },

                    // ----- Acesso (Categoria 4) -----
                    new Ticket { Titulo = "Palavra-passe expirada", Descricao = "Não consigo fazer login, o sistema pede para alterar a password mas falha.", CategoriaId = 4, PrioridadeId = 1, StatusId = 2, SolicitanteId = sofiaUser.Id, AtendenteId = lauraUser.Id },
                    new Ticket { Titulo = "Pedido de acesso a novo software", Descricao = "Preciso de acesso ao sistema de gestão de projetos (Jira).", CategoriaId = 4, PrioridadeId = 2, StatusId = 3, SolicitanteId = anaUser.Id},
                    new Ticket { Titulo = "Reset de password (Sistema Financeiro)", Descricao = "Esqueci-me da minha password do SAGE.", CategoriaId = 4, PrioridadeId = 1, StatusId = 1, SolicitanteId = mariaUser.Id },
                    new Ticket { Titulo = "Utilizador bloqueado", Descricao = "Tentei o login várias vezes e a minha conta foi bloqueada.", CategoriaId = 4, PrioridadeId = 1, StatusId = 2, SolicitanteId = ruiUser.Id, AtendenteId = carlosUser.Id },

                    // ----- Outro (Categoria 5) -----
                    new Ticket { Titulo = "Dúvida sobre utilização do telemóvel", Descricao = "Como configuro o email da empresa no meu telemóvel Android?", CategoriaId = 5, PrioridadeId = 3, StatusId = 3, SolicitanteId = diogoUser.Id, AtendenteId = miguelUser.Id },
                    new Ticket { Titulo = "Formação em novo software", Descricao = "Gostaria de saber se há formação planeada para o novo CRM.", CategoriaId = 5, PrioridadeId = 3, StatusId = 1, SolicitanteId = sofiaUser.Id }
                );

                if (!context.KnowledgeBaseArticles.Any())
                {
                    var articles = new KnowledgeBaseArticle[]
                    {
                    new KnowledgeBaseArticle
    {
        Titulo = "Bateria do notebook descarregando muito rápido",
        Descricao = "Passos para otimizar a duração da bateria do seu notebook.",
        Conteudo = "<h3>Passo 1: Verificar Brilho</h3><p>Reduza o brilho da tela. É um dos maiores consumidores de energia.</p><h3>Passo 2: Plano de Energia</h3><p>Clique no ícone de bateria (barra de tarefas) e mude o 'Modo de Energia' para 'Recomendado' ou 'Economia de bateria'.</p><h3>Passo 3: Fechar Aplicativos</h3><p>Verifique no Gerenciador de Tarefas (Ctrl+Shift+Esc) quais aplicativos estão usando muita CPU ou Bateria e feche-os se não forem necessários.</p>",
        Categoria = "Hardware",
        Tags = "hardware, bateria, notebook, energia, lento",
        Publicado = true,
        Destaque = true,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    },
    new KnowledgeBaseArticle
    {
        Titulo = "Mouse ou teclado Bluetooth não conecta",
        Descricao = "Solução de problemas para periféricos Bluetooth.",
        Conteudo = "<h3>Passo 1: Verificar Pilhas/Bateria</h3><p>Garanta que o dispositivo tenha carga ou pilhas novas.</p><h3>Passo 2: Reiniciar Bluetooth</h3><p>Desative e reative o Bluetooth no seu computador (Menu Iniciar > Configurações > Dispositivos > Bluetooth).</p><h3>Passo 3: Reparar (Parear)</h3><p>Remova o dispositivo da lista de dispositivos Bluetooth do seu PC e faça o processo de pareamento novamente, seguindo o manual do mouse/teclado.</p>",
        Categoria = "Hardware",
        Tags = "hardware, bluetooth, mouse, teclado, sem fio, conexão",
        Publicado = true,
        Destaque = false,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    },
    new KnowledgeBaseArticle
    {
        Titulo = "Webcam não funciona em reuniões (Teams/Zoom)",
        Descricao = "O que fazer quando sua câmera não é detectada.",
        Conteudo = "<h3>Passo 1: Verificar Permissões</h3><p>No Windows, vá em 'Configurações de Privacidade da Câmera' e garanta que 'Permitir que aplicativos acessem sua câmera' esteja ativado, assim como o app (Teams/Zoom).</p><h3>Passo 2: Trava Física</h3><p>Muitos notebooks possuem uma pequena tampa deslizante sobre a lente da webcam. Verifique se ela não está fechada.</p><h3>Passo 3: Reiniciar</h3><p>Feche o aplicativo de reunião e abra-o novamente. Se não funcionar, reinicie o computador.</p>",
        Categoria = "Hardware",
        Tags = "hardware, webcam, câmera, teams, zoom, reunião, vídeo",
        Publicado = true,
        Destaque = true,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    },
    new KnowledgeBaseArticle
    {
        Titulo = "Como configurar um novo fone de ouvido (Headset) USB",
        Descricao = "Passos básicos para fazer seu headset funcionar.",
        Conteudo = "<h3>Passo 1: Conectar</h3><p>Plugue o headset em uma porta USB livre. Evite hubs USB se possível.</p><h3>Passo 2: Aguardar Instalação</h3><p>O Windows detectará o novo hardware e instalará os drivers automaticamente. Isso pode levar alguns segundos.</p><h3>Passo 3: Definir como Padrão</h3><p>Clique no ícone de som (canto inferior direito), clique na seta para expandir e selecione o nome do seu headset como dispositivo de saída (e entrada, se tiver microfone).</p>",
        Categoria = "Hardware",
        Tags = "hardware, áudio, som, fone de ouvido, headset, usb, microfone",
        Publicado = true,
        Destaque = false,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    },
    new KnowledgeBaseArticle
    {
        Titulo = "Computador não liga (nenhum sinal)",
        Descricao = "Verificações iniciais quando o PC está totalmente 'morto'.",
        Conteudo = "<h3>Passo 1: Verificar Tomada e Cabo</h3><p>Certifique-se de que o cabo de força está bem conectado ao PC e à tomada. Tente usar uma tomada diferente.</p><h3>Passo 2: Verificar Filtro de Linha</h3><p>Se estiver usando um filtro de linha (régua), verifique se ele está ligado ou se o fusível queimou. Tente ligar o PC direto na tomada.</p><h3>Passo 3: (Desktop) Fonte</h3><p>Verifique se o interruptor na parte de trás da fonte de alimentação (I/O) está na posição 'I' (ligado).</p>",
        Categoria = "Hardware",
        Tags = "hardware, não liga, energia, morto, pc, desktop",
        Publicado = true,
        Destaque = false,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    },

    // --- Categoria: Software (5 Artigos) ---

    new KnowledgeBaseArticle
    {
        Titulo = "Outlook travando ou não abrindo",
        Descricao = "Como resolver problemas comuns do Microsoft Outlook.",
        Conteudo = "<h3>Passo 1: Modo de Segurança</h3><p>Tente abrir o Outlook em Modo de Segurança. Pressione 'Win+R', digite <code>outlook.exe /safe</code> e pressione Enter. Se funcionar, o problema é um suplemento.</p><h3>Passo 2: Desativar Suplementos</h3><p>Com o Outlook aberto em Modo de Segurança, vá em 'Arquivo' > 'Opções' > 'Suplementos'. Desative os suplementos (especialmente os que não são da Microsoft) um por um.</p><h3>Passo 3: Reparar o Office</h3><p>Vá em 'Painel de Controle' > 'Programas e Recursos', encontre o 'Microsoft Office', clique com o botão direito e escolha 'Alterar' > 'Reparo Rápido'.</p>",
        Categoria = "Software",
        Tags = "software, office, outlook, email, travando, lento",
        Publicado = true,
        Destaque = true,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    },
    new KnowledgeBaseArticle
    {
        Titulo = "Como limpar o cache do navegador (Chrome/Edge)",
        Descricao = "Limpar dados de navegação para resolver problemas em sites.",
        Conteudo = "<h3>Passo 1: Abrir Ferramenta</h3><p>Pressione o atalho <strong>Ctrl + Shift + Del</strong> (Delete) com o navegador aberto.</p><h3>Passo 2: Selecionar Itens</h3><p>Uma nova janela se abrirá. Mantenha selecionado pelo menos 'Cookies e outros dados do site' e 'Imagens e arquivos armazenados em cache'.</p><h3>Passo 3: Limpar</h3><p>Defina o intervalo de tempo (geralmente 'Todo o período') e clique em 'Limpar dados' ou 'Limpar agora'.</p>",
        Categoria = "Software",
        Tags = "software, chrome, edge, navegador, cache, internet, lento",
        Publicado = true,
        Destaque = false,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    },
    new KnowledgeBaseArticle
    {
        Titulo = "O que fazer quando um programa trava (Não Respondendo)",
        Descricao = "Como forçar o fechamento de um aplicativo travado.",
        Conteudo = "<h3>Passo 1: Gerenciador de Tarefas</h3><p>Pressione <strong>Ctrl + Shift + Esc</strong> para abrir o Gerenciador de Tarefas.</p><h3>Passo 2: Encontrar Processo</h3><p>Na aba 'Processos', encontre o nome do programa que está com o status 'Não respondendo'.</p><h3>Passo 3: Finalizar Tarefa</h3><p>Clique com o botão direito sobre o processo e selecione 'Finalizar Tarefa'.</p>",
        Categoria = "Software",
        Tags = "software, travado, não respondendo, lento, fechar, programa",
        Publicado = true,
        Destaque = true,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    },
    new KnowledgeBaseArticle
    {
        Titulo = "Como definir o leitor de PDF padrão",
        Descricao = "Faça seus PDFs abrirem sempre com o Adobe Reader (ou seu preferido).",
        Conteudo = "<h3>Passo 1: Encontrar um PDF</h3><p>Localize qualquer arquivo .pdf no seu computador.</p><h3>Passo 2: Abrir Com</h3><p>Clique com o botão direito no arquivo e vá em 'Abrir com' > 'Escolher outro aplicativo'.</p><h3>Passo 3: Definir Padrão</h3><p>Selecione o programa desejado (ex: 'Adobe Acrobat Reader') na lista e, o mais importante, marque a caixa 'Sempre usar este aplicativo para abrir arquivos .pdf'. Clique em OK.</p>",
        Categoria = "Software",
        Tags = "software, pdf, adobe, padrão, associação, arquivo",
        Publicado = true,
        Destaque = false,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    },
    new KnowledgeBaseArticle
    {
        Titulo = "Como verificar e instalar atualizações do Windows",
        Descricao = "Mantenha seu sistema seguro e atualizado com o Windows Update.",
        Conteudo = "<h3>Passo 1: Configurações</h3><p>Abra o Menu Iniciar e clique no ícone de engrenagem (Configurações).</p><h3>Passo 2: Windows Update</h3><p>Na janela de Configurações, clique em 'Atualização e Segurança'.</p><h3>Passo 3: Verificar</h3><p>Clique no botão 'Verificar se há atualizações'. O Windows buscará, baixará e instalará as atualizações necessárias. Pode ser preciso reiniciar o PC.</p>",
        Categoria = "Software",
        Tags = "software, windows, update, atualização, segurança, patch",
        Publicado = true,
        Destaque = false,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    },

    // --- Categoria: Acesso (3 Artigos) ---

    new KnowledgeBaseArticle
    {
        Titulo = "Minha senha expirou, como trocar?",
        Descricao = "O que fazer quando o Windows informa que sua senha expirou.",
        Conteudo = "<h3>Opção 1: Tela de Login</h3><p>Normalmente, o próprio Windows irá forçá-lo a trocar a senha na próxima vez que tentar fazer login. Siga as instruções na tela.</p><h3>Opção 2: Ctrl+Alt+Del</h3><p>Se você já estiver logado, pressione <strong>Ctrl + Alt + Del</strong> (Delete) e escolha a opção 'Alterar uma senha'.</p><h3>Passo 3: Requisitos</h3><p>Digite sua senha antiga, depois a nova senha e confirme. Lembre-se de seguir as regras de complexidade (ex: não repetir as últimas 3 senhas).</p>",
        Categoria = "Acesso",
        Tags = "acesso, senha, expirou, trocar, login, conta",
        Publicado = true,
        Destaque = true,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    },
    new KnowledgeBaseArticle
    {
        Titulo = "Como se conectar à VPN da empresa",
        Descricao = "Acessando a rede interna de fora do escritório (Home Office).",
        Conteudo = "<h3>Passo 1: Abrir o Cliente VPN</h3><p>Procure pelo cliente VPN instalado no seu PC (Ex: FortiClient, GlobalProtect, Cisco AnyConnect) e abra-o.</p><h3>Passo 2: Inserir Credenciais</h3><p>Digite o endereço do servidor (se solicitado), seu nome de usuário e sua senha de rede (a mesma do Windows).</p><h3>Passo 3: Conectar</h3><p>Clique em 'Conectar'. Pode ser que você precise aprovar a conexão no seu celular (MFA). Aguarde até que o status mude para 'Conectado'.</p>",
        Categoria = "Acesso",
        Tags = "acesso, vpn, home office, remoto, rede, conexão",
        Publicado = true,
        Destaque = true,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    },
    new KnowledgeBaseArticle
    {
        Titulo = "Configurando a Autenticação de Múltiplos Fatores (MFA)",
        Descricao = "Como configurar seu celular para aprovar logins.",
        Conteudo = "<h3>Passo 1: Baixar o App</h3><p>No seu celular, baixe o aplicativo 'Microsoft Authenticator' (ou o app aprovado pela empresa).</p><h3>Passo 2: Acessar Portal</h3><p>No seu computador, acesse o portal de segurança da sua conta (geralmente 'aka.ms/mfasetup' para contas Microsoft).</p><h3>Passo 3: Ler QR Code</h3><p>Siga os passos no portal. Ele mostrará um QR Code. Use o aplicativo no celular para ler o QR Code e vincular sua conta. Faça o teste de aprovação.</p>",
        Categoria = "Acesso",
        Tags = "acesso, mfa, 2fa, segurança, authenticator, senha, celular",
        Publicado = true,
        Destaque = true,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    },

    // --- Categoria: Rede (4 Artigos) ---

    new KnowledgeBaseArticle
    {
        Titulo = "Conectado no cabo de rede (Ethernet) mas sem acesso",
        Descricao = "O que fazer quando o cabo está plugado, mas a rede não funciona.",
        Conteudo = "<h3>Passo 1: Verificar Luzes</h3><p>Verifique a porta de rede no seu notebook ou PC. As luzes (geralmente verde e laranja) devem estar piscando. Se estiverem apagadas, o cabo ou a porta podem ter problema.</p><h3>Passo 2: Trocar o Cabo/Porta</h3><p>Tente usar um cabo de rede diferente que você saiba que está funcionando. Se estiver em uma docking station, tente plugar o cabo direto no notebook.</p><h3>Passo 3: Reiniciar</h3><p>Reinicie o computador. Isso resolve muitos problemas temporários de atribuição de IP.</p>",
        Categoria = "Rede",
        Tags = "rede, cabo, ethernet, sem internet, conexão, ip",
        Publicado = true,
        Destaque = false,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    },
    new KnowledgeBaseArticle
    {
        Titulo = "Wi-Fi está 'Conectado, mas sem internet'",
        Descricao = "Solução para quando o Wi-Fi conecta, mas não navega.",
        Conteudo = "<h3>Passo 1: Esquecer a Rede</h3><p>Clique no ícone de Wi-Fi, clique com o botão direito na rede problemática e selecione 'Esquecer'. Conecte-se novamente e digite a senha.</p><h3>Passo 2: (Home Office) Reiniciar Roteador</h3><p>Se você estiver em casa, desligue seu roteador Wi-Fi da tomada, aguarde 30 segundos e ligue-o novamente.</p><h3>Passo 3: Flush DNS</h3><p>Abra o 'Prompt de Comando' como administrador e digite <code>ipconfig /flushdns</code>, depois pressione Enter. Reinicie o PC.</p>",
        Categoria = "Rede",
        Tags = "rede, wifi, sem internet, conectado, dns, roteador",
        Publicado = true,
        Destaque = true,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    },
    new KnowledgeBaseArticle
    {
        Titulo = "Como mapear uma pasta da rede (Drive G:, P:, etc.)",
        Descricao = "Acessando pastas compartilhadas como se fossem um drive local.",
        Conteudo = "<h3>Passo 1: Abrir 'Este Computador'</h3><p>Abra o explorador de arquivos e vá em 'Este Computador'.</p><h3>Passo 2: Mapear Unidade</h3><p>No menu superior, clique em 'Computador' e depois em 'Mapear unidade de rede'.</p><h3>Passo 3: Definir Caminho</h3><p>Escolha uma letra de unidade (ex: 'P:'). No campo 'Pasta', digite o caminho de rede (ex: <code>\\\\servidor\\compartilhado</code>). Marque 'Reconectar na entrada' e clique em 'Concluir'.</p>",
        Categoria = "Rede",
        Tags = "rede, mapeamento, drive, pasta, servidor, compartilhado",
        Publicado = true,
        Destaque = false,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    },
    new KnowledgeBaseArticle
    {
        Titulo = "Internet ou rede interna muito lenta",
        Descricao = "Dicas para identificar a causa da lentidão.",
        Conteudo = "<h3>Passo 1: Cabo vs Wi-Fi</h3><p>Se estiver no Wi-Fi, tente conectar um cabo de rede. O Wi-Fi é mais suscetível a interferências e lentidão.</p><h3>Passo 2: Fechar Aplicativos</h3><p>Feche aplicativos que usam muita rede (ex: uploads de arquivos grandes, streaming de vídeo, torrents).</p><h3>Passo 3: Testar Velocidade</h3><p>Acesse um site de teste de velocidade (ex: 'fast.com' ou 'speedtest.net') para ver se o problema é com sua conexão de internet ou apenas com o acesso a um sistema interno.</p>",
        Categoria = "Rede",
        Tags = "rede, internet, lento, velocidade, wifi, cabo",
        Publicado = true,
        Destaque = false,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    },

    // --- Categoria: Outro (6 Artigos) ---

    new KnowledgeBaseArticle
    {
        Titulo = "Como solicitar troca de toner ou cartucho",
        Descricao = "Procedimento para pedir suprimentos para impressoras.",
        Conteudo = "<h3>Passo 1: Identificar Impressora</h3><p>Anote o modelo exato da impressora (ex: 'HP LaserJet Pro M404') e, se possível, o código de patrimônio ou localização (ex: 'Financeiro, 2º andar').</p><h3>Passo 2: Abrir Chamado</h3><p>Acesse o portal de chamados do TI ou de Facilities.</p><h3>Passo 3: Detalhar</h3><p>Abra um chamado na categoria 'Suprimentos' ou 'Impressora'. Informe o modelo e a cor do toner que acabou (ex: 'Preto/Black').</p>",
        Categoria = "Outro",
        Tags = "outro, impressora, toner, suprimentos, chamado, cartucho",
        Publicado = true,
        Destaque = false,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    },
    new KnowledgeBaseArticle
    {
        Titulo = "Como configurar a assinatura de e-mail padrão no Outlook",
        Descricao = "Adicionando sua assinatura profissional em novos e-mails.",
        Conteudo = "<h3>Passo 1: Opções</h3><p>No Outlook, vá em 'Arquivo' > 'Opções'.</p><h3>Passo 2: Assinaturas</h3><p>Na janela de Opções, vá para a aba 'Email' e clique no botão 'Assinaturas...'.</p><h3>Passo 3: Configurar</h3><p>Clique em 'Novo' para criar uma assinatura. Cole ou digite suas informações. Use os menus à direita para definir esta assinatura como padrão para 'Novas mensagens' e 'Respostas/encaminhamentos'.</p>",
        Categoria = "Outro",
        Tags = "outro, email, outlook, assinatura, configuração",
        Publicado = true,
        Destaque = false,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    },
    new KnowledgeBaseArticle
    {
        Titulo = "Perdi meu crachá de acesso, o que fazer?",
        Descricao = "Procedimento urgente em caso de perda do crachá.",
        Conteudo = "<h3>Passo 1: Comunicar Imediatamente</h3><p>Contate o setor de Segurança Patrimonial ou o RH o mais rápido possível. Este é um item de segurança.</p><h3>Passo 2: Bloqueio</h3><p>Solicite o bloqueio imediato do crachá perdido para evitar uso indevido.</p><h3>Passo 3: Solicitar 2ª Via</h3><p>Siga o procedimento interno do RH ou Segurança para solicitar a emissão de uma segunda via.</p>",
        Categoria = "Outro",
        Tags = "outro, rh, segurança, crachá, acesso, perda",
        Publicado = true,
        Destaque = true,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    },
    new KnowledgeBaseArticle
    {
        Titulo = "Como solicitar uma análise ergonômica ou troca de cadeira",
        Descricao = "Procedimento para questões de ergonomia no posto de trabalho.",
        Conteudo = "<h3>Passo 1: Contatar o Setor Correto</h3><p>Normalmente, este tipo de solicitação é tratada pelo RH, SESMT (Segurança do Trabalho) ou Facilities, e não pelo TI.</p><h3>Passo 2: Abrir Solicitação</h3><p>Use o canal correto (portal do RH, e-mail para Facilities) para descrever seu problema (ex: 'Cadeira com braço quebrado', 'Dores nas costas ao final do dia').</p><h3>Passo 3: Aguardar Análise</h3><p>Um profissional de ergonomia ou da equipe de facilities poderá avaliar seu posto de trabalho.</p>",
        Categoria = "Outro",
        Tags = "outro, rh, facilities, cadeira, ergonomia, sesmt",
        Publicado = true,
        Destaque = false,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    },
    new KnowledgeBaseArticle
    {
        Titulo = "Como configurar resposta automática de 'Férias' no Outlook",
        Descricao = "Avise seus colegas que você está fora do escritório.",
        Conteudo = "<h3>Passo 1: Arquivo</h3><p>No Outlook, clique em 'Arquivo' (canto superior esquerdo).</p><h3>Passo 2: Respostas Automáticas</h3><p>Clique no botão 'Respostas Automáticas (Fora do Escritório)'.</p><h3>Passo 3: Configurar</h3><p>Marque 'Enviar respostas automáticas'. Defina o período (data/hora de início e fim). Escreva sua mensagem para 'Dentro da minha organização' e, se desejar, uma diferente para 'Fora da minha organização'.</a_>",
        Categoria = "Outro",
        Tags = "outro, outlook, email, férias, ausente, resposta automática",
        Publicado = true,
        Destaque = false,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    },
    new KnowledgeBaseArticle
    {
        Titulo = "Dúvidas sobre Vale Refeição (VR) ou Vale Alimentação (VA)",
        Descricao = "Para quem reportar problemas com benefícios.",
        Conteudo = "<h3>Passo 1: Contatar RH</h3><p>O setor de Tecnologia da Informação (TI) não gerencia benefícios como VR ou VA.</p><h3>Passo 2: Canal Correto</h3><p>Qualquer problema (ex: 'Cartão não chegou', 'Valor incorreto') deve ser reportado diretamente ao setor de Recursos Humanos (RH) ou ao Departamento Pessoal (DP).</p><h3>Passo 3: Portal de Benefícios</h3><p>Verifique também o aplicativo ou site do seu cartão de benefício para extratos e informações.</p>",
        Categoria = "Outro",
        Tags = "outro, rh, dp, benefícios, vr, va, pagamento",
        Publicado = true,
        Destaque = false,
        DataCriacao = DateTime.Now,
        DataAtualizacao = DateTime.Now,
        Visualizacoes = 0,
        VotosUteis = 0,
        NaoUtil = 0,
        IdAutor = "sistema"
    }
};

                    context.KnowledgeBaseArticles.AddRange(articles);
                }
                await context.SaveChangesAsync();
            }
        
        }
    }
}