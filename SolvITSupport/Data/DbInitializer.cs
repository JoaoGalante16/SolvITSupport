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

                var usuario2 = new ApplicationUser
                {
                    UserName = "maria.joao@solvit.com", // <-- Deve ser este email
                    Email = "maria.joao@solvit.com", // <-- E este email
                    NomeCompleto = "Maria João",
                    EmailConfirmed = true,
                    Ativo = true
                };
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
                if (anaUser == null) throw new Exception("DEBUG: O utilizador 'ana.costa@solvit.com' não foi encontrado. Verifique o bloco 'if (!context.Users.Any())'.");

                var mariaUser = await userManager.FindByEmailAsync("maria.joao@solvit.com");
                if (mariaUser == null) throw new Exception("DEBUG: O utilizador 'maria.joao@solvit.com' não foi encontrado. Verifique o bloco 'if (!context.Users.Any())'.");

                var ruiUser = await userManager.FindByEmailAsync("rui.pedro@solvit.com");
                if (ruiUser == null) throw new Exception("DEBUG: O utilizador 'rui.pedro@solvit.com' não foi encontrado. Verifique o bloco 'if (!context.Users.Any())'.");

                var diogoUser = await userManager.FindByEmailAsync("diogo.fernandes@solvit.com");
                if (diogoUser == null) throw new Exception("DEBUG: O utilizador 'diogo.fernandes@solvit.com' não foi encontrado. Verifique o bloco 'if (!context.Users.Any())'.");

                var sofiaUser = await userManager.FindByEmailAsync("sofia.lopes@solvit.com");
                if (sofiaUser == null) throw new Exception("DEBUG: O utilizador 'sofia.lopes@solvit.com' não foi encontrado. Verifique o bloco 'if (!context.Users.Any())'.");

                var carlosUser = await userManager.FindByEmailAsync("carlos.silva@solvit.com");
                if (carlosUser == null) throw new Exception("DEBUG: O utilizador 'carlos.silva@solvit.com' não foi encontrado. Verifique o bloco 'if (!context.Users.Any())'.");

                var miguelUser = await userManager.FindByEmailAsync("miguel.costa@solvit.com");
                if (miguelUser == null) throw new Exception("DEBUG: O utilizador 'miguel.costa@solvit.com' não foi encontrado. Verifique o bloco 'if (!context.Users.Any())'.");

                var lauraUser = await userManager.FindByEmailAsync("laura.gomes@solvit.com");
                if (lauraUser == null) throw new Exception("DEBUG: O utilizador 'laura.gomes@solvit.com' não foi encontrado. Verifique o bloco 'if (!context.Users.Any())'.");

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
                await context.SaveChangesAsync();
            }
        
        }
    }
}