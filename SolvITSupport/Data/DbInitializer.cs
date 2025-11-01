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

                var usuario = new ApplicationUser { UserName = "ana.costa@solvit.com", Email = "ana.costa@solvit.com", NomeCompleto = "Ana Costa", EmailConfirmed = true, Ativo = true };
                await userManager.CreateAsync(usuario, "Usuario@123");
                await userManager.AddToRoleAsync(usuario, "Usuario");
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
                var anaUser = await userManager.FindByEmailAsync("ana.costa@solvit.com");
                var carlosUser = await userManager.FindByEmailAsync("carlos.silva@solvit.com");

                if (anaUser != null && carlosUser != null)
                {
                    context.Tickets.AddRange(
                        new Ticket { Titulo = "Computador não liga (da Ana)", Descricao = "O PC não está a ligar.", CategoriaId = 1, PrioridadeId = 1, StatusId = 1, SolicitanteId = anaUser.Id },
                        new Ticket { Titulo = "Erro no sistema (da Ana)", Descricao = "Sistema de finanças com erro.", CategoriaId = 2, PrioridadeId = 2, StatusId = 2, SolicitanteId = anaUser.Id, AtendenteId = carlosUser.Id }
                    );
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}