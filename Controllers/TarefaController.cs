using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        
        public IActionResult ObterPorId(int id)
        {
            // DONE: Buscar o Id no banco utilizando o EF
            var tarefa = _context.Tarefas.Find(id);
            // DONE: Validar o tipo de retorno. Se não encontrar a tarefa, retornar NotFound,
            if (tarefa == null)
                return NotFound();
            // DONE: caso contrário retornar OK com a tarefa encontrada
            return Ok(tarefa);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            // DONE: Buscar todas as tarefas no banco utilizando o EF
            var tarefas = _context.Tarefas.ToList();
            if (tarefas.Count == 0)
            {
                return NotFound("Nenhuma tarefa encontrada.");
            }
            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // DONE: Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
            var tarefas = _context.Tarefas.Where(x => x.Titulo.Contains(titulo)).ToList();
            return Ok(tarefas);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            // DONE: Buscar  as tarefas no banco utilizando o EF, que contenha o status recebido por parâmetro
            var tarefas = _context.Tarefas.Where(x => x.Status == status).ToList();
            return Ok(tarefas);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // DONE: Adicionar a tarefa recebida no EF e salvar as mudanças (save changes)
            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();

            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // DONE: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;
            // DONE: Atualizar a variável tarefaBanco no EF 
            _context.Tarefas.Update(tarefaBanco);
            //DONE: salvar as mudanças (save changes)
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            // DONE: Remover a tarefa encontrada através do EF 
            _context.Tarefas.Remove(tarefaBanco);
            // e salvar as mudanças (save changes)
            _context.SaveChanges();

            return NoContent();
        }
    }
}
