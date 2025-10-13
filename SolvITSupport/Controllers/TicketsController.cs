using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; // <-- ADICIONE ESTA LINHA
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SolvITSupport.Data;
using SolvITSupport.Models;
using SolvITSupport.Services;
using System.Linq;
[Authorize]
public class TicketsController : Controller
{
    // Adicione os campos para todos os serviços que o controller usa
    private readonly ITicketService _ticketService;
    private readonly ICategoryService _categoryService;
    private readonly IPriorityService _priorityService;
    private readonly IStatusService _statusService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context; // <-- ADICIONE ESTA LINHA

    // Altere o construtor para receber também o ApplicationDbContext
    public TicketsController(
        ITicketService ticketService,
        ICategoryService categoryService,
        IPriorityService priorityService,
        IStatusService statusService,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context) // <-- ADICIONE ESTE PARÂMETRO
    {
        _ticketService = ticketService;
        _categoryService = categoryService;
        _priorityService = priorityService;
        _statusService = statusService;
        _userManager = userManager;
        _context = context; // <-- ADICIONE ESTA LINHA
    }


    // Substitua o seu método Index() por este
    public async Task<IActionResult> Index(int? categoryIdFilter, int? priorityIdFilter, int? statusIdFilter, string searchString)
    {
        // Vai buscar todos os chamados que o utilizador tem permissão para ver
        IQueryable<Ticket> ticketsQuery;
        var currentUserId = _userManager.GetUserId(User);

        if (User.IsInRole("Atendente") || User.IsInRole("Administrador"))
        {
            ticketsQuery = _context.Tickets.AsQueryable();
        }
        else
        {
            ticketsQuery = _context.Tickets.Where(t => t.SolicitanteId == currentUserId);
        }

        // --- INÍCIO DA CORREÇÃO ---
        // Aplica o filtro de pesquisa de texto, se existir
        if (!string.IsNullOrEmpty(searchString))
        {
            // Pesquisa no Título E também no Código (convertendo o ID para string no lado do C# primeiro)
            ticketsQuery = ticketsQuery.Where(t => t.Titulo.Contains(searchString) || ("TK-" + t.Id.ToString()).Contains(searchString));
        }
        // --- FIM DA CORREÇÃO ---


        // Aplica os filtros dos dropdowns, se existirem
        if (categoryIdFilter.HasValue)
        {
            ticketsQuery = ticketsQuery.Where(t => t.CategoriaId == categoryIdFilter.Value);
        }
        if (priorityIdFilter.HasValue)
        {
            ticketsQuery = ticketsQuery.Where(t => t.PrioridadeId == priorityIdFilter.Value);
        }
        if (statusIdFilter.HasValue)
        {
            ticketsQuery = ticketsQuery.Where(t => t.StatusId == statusIdFilter.Value);
        }

        // Executa a consulta na base de dados
        var filteredTickets = await ticketsQuery
            .Include(t => t.Categoria)
            .Include(t => t.Prioridade)
            .Include(t => t.Status)
            .OrderByDescending(t => t.DataCriacao)
            .ToListAsync();

        // (O resto do seu código para criar o ViewModel continua igual...)
        var viewModel = new TicketIndexViewModel
        {
            Tickets = filteredTickets,
            Categories = new SelectList(await _categoryService.GetAllAsync(), "Id", "Nome", categoryIdFilter),
            Priorities = new SelectList(await _priorityService.GetAllAsync(), "Id", "Nome", priorityIdFilter),
            Statuses = new SelectList(await _statusService.GetAllAsync(), "Id", "Nome", statusIdFilter),
            SearchString = searchString
        };

        return View(viewModel);
    }
    // Adicione estes dois métodos dentro da classe TicketsController

    // GET: /Tickets/Edit/5
    // Este método busca os dados do ticket e mostra o formulário de edição preenchido.
    public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _ticketService.GetTicketByIdAsync(id.Value);
            if (ticket == null)
            {
                return NotFound();
            }

            // Criamos o ViewModel a partir dos dados do ticket que veio do banco
            var viewModel = new EditTicketViewModel
            {
                Id = ticket.Id,
                Titulo = ticket.Titulo,
                Descricao = ticket.Descricao,
                CategoriaId = ticket.CategoriaId,
                PrioridadeId = ticket.PrioridadeId,
                StatusId = ticket.StatusId
            };

            // Preparamos as listas para as dropdowns
            ViewBag.CategoriaId = new SelectList(await _categoryService.GetAllAsync(), "Id", "Nome", ticket.CategoriaId);
            ViewBag.PrioridadeId = new SelectList(await _priorityService.GetAllAsync(), "Id", "Nome", ticket.PrioridadeId);
            ViewBag.StatusId = new SelectList(await _statusService.GetAllAsync(), "Id", "Nome", ticket.StatusId);

            return View(viewModel);
        }

        // POST: /Tickets/Edit/5
        // Este método é executado quando o utilizador submete o formulário de edição.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditTicketViewModel viewModel)
        {
            // Verifica se o ID da URL corresponde ao ID do modelo (uma medida de segurança)
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Busca o ticket original no banco de dados
                    var ticketToUpdate = await _ticketService.GetTicketByIdAsync(id);
                    if (ticketToUpdate == null)
                    {
                        return NotFound();
                    }

                    // Atualiza as propriedades do ticket original com os novos valores do ViewModel
                    ticketToUpdate.Titulo = viewModel.Titulo;
                    ticketToUpdate.Descricao = viewModel.Descricao;
                    ticketToUpdate.CategoriaId = viewModel.CategoriaId;
                    ticketToUpdate.PrioridadeId = viewModel.PrioridadeId;
                    ticketToUpdate.StatusId = viewModel.StatusId;

                    // Manda o serviço salvar as alterações
                    await _ticketService.UpdateTicketAsync(ticketToUpdate);
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Trata erros de concorrência (se outra pessoa editou ao mesmo tempo)
                    // Para simplificar, não faremos nada aqui, mas em um sistema real seria importante
                    throw;
                }
                // Redireciona para a lista de chamados após salvar
                return RedirectToAction(nameof(Index));
            }

            // Se a validação falhar, recarrega as dropdowns e mostra o formulário novamente
            ViewBag.CategoriaId = new SelectList(await _categoryService.GetAllAsync(), "Id", "Nome", viewModel.CategoriaId);
            ViewBag.PrioridadeId = new SelectList(await _priorityService.GetAllAsync(), "Id", "Nome", viewModel.PrioridadeId);
            ViewBag.StatusId = new SelectList(await _statusService.GetAllAsync(), "Id", "Nome", viewModel.StatusId);
            return View(viewModel);
        }

        // Adicione estes dois métodos dentro da classe TicketsController

        // GET: /Tickets/Delete/5
        // Este método busca os dados do chamado e mostra a página de confirmação.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _ticketService.GetTicketByIdAsync(id.Value);
            if (ticket == null)
            {
                return NotFound();
            }

            // Envia o ticket encontrado para a View de confirmação
            return View(ticket);
        }

        // POST: /Tickets/Delete/5
        // Este método é executado quando o utilizador clica no botão "Sim, apagar" na página de confirmação.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Manda o serviço apagar o ticket da base de dados
            await _ticketService.DeleteTicketAsync(id);

            // Redireciona de volta para a lista de chamados
            return RedirectToAction(nameof(Index));
        }

        // --- MÉTODOS NOVOS ADICIONADOS ABAIXO ---

        // GET: /Tickets/Create
        // Este método continua igual, apenas mostra o formulário.
        public async Task<IActionResult> Create()
        {
            ViewBag.CategoriaId = new SelectList(await _categoryService.GetAllAsync(), "Id", "Nome");
            ViewBag.PrioridadeId = new SelectList(await _priorityService.GetAllAsync(), "Id", "Nome");
            return View();
        }

    // POST: /Tickets/Create
    // AGORA, este método recebe o nosso novo ViewModel em vez do Ticket completo.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTicketViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var ticket = new Ticket
            {
                Titulo = viewModel.Titulo,
                Descricao = viewModel.Descricao,
                CategoriaId = viewModel.CategoriaId,
                PrioridadeId = viewModel.PrioridadeId,
                StatusId = 1,
                SolicitanteId = _userManager.GetUserId(User)
            };

            // Processar o anexo, se existir
            if (viewModel.Anexo != null && viewModel.Anexo.Length > 0)
            {
                // Define o caminho onde vamos guardar o ficheiro
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Cria um nome de ficheiro único para evitar conflitos
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.Anexo.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Guarda o ficheiro no disco
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.Anexo.CopyToAsync(fileStream);
                }

                // Cria o objeto Attachment para guardar na base de dados
                var attachment = new Attachment
                {
                    NomeArquivo = uniqueFileName,
                    Caminho = "/uploads/" + uniqueFileName, // Caminho relativo para usar no HTML
                    NomeOriginal = viewModel.Anexo.FileName,
                    TipoArquivo = viewModel.Anexo.ContentType,
                    TamanhoBytes = viewModel.Anexo.Length,
                    UsuarioId = _userManager.GetUserId(User)
                };

                // Associa o anexo ao ticket
                ticket.Anexos.Add(attachment);
            }

            await _ticketService.CreateTicketAsync(ticket);
            return RedirectToAction(nameof(Index));
        }

        // Se a validação falhar, recarrega as dropdowns
        ViewBag.CategoriaId = new SelectList(await _categoryService.GetAllAsync(), "Id", "Nome", viewModel.CategoriaId);
        ViewBag.PrioridadeId = new SelectList(await _priorityService.GetAllAsync(), "Id", "Nome", viewModel.PrioridadeId);
        return View(viewModel);
    }
    // Substitua o seu método Details antigo por este
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ticket = await _ticketService.GetTicketByIdAsync(id.Value);

        if (ticket == null)
        {
            return NotFound();
        }

        // Criamos o nosso novo ViewModel
        var viewModel = new TicketDetailsViewModel
        {
            Ticket = ticket,
            Updates = ticket.Atualizacoes.OrderBy(u => u.DataCriacao) // Mostra as atualizações por ordem cronológica
        };

        return View(viewModel);
    }

    // Substitua o seu método AddComment por esta versão muito mais simples
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(int ticketId, string NewComment)
    {
        // Como já não temos o ModelState a validar, verificamos manualmente se o comentário não está vazio
        if (!string.IsNullOrEmpty(NewComment))
        {
            var userId = _userManager.GetUserId(User);
            await _ticketService.AddUpdateAsync(ticketId, userId, NewComment);
        }

        // Após adicionar o comentário, redireciona de volta para a mesma página de detalhes
        return RedirectToAction("Details", new { id = ticketId });
    }

    // Adicione estes dois métodos dentro da sua classe TicketsController

  
}
