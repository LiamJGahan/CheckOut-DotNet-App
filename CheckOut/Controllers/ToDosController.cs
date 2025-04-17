using CheckOut.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheckOut.Models
{
    [Route("ToDos")]
    public class ToDosController : Controller
    {     
        private readonly CheckOutContext _context;

        public ToDosController(CheckOutContext context)
        {
            _context = context;
        }

        [HttpPost("SetCompleted")]
        public async Task<IActionResult> SetCompleted([FromForm] int checklistId, [FromForm] List<int> completedToDoIds)
        {
            var toDos = await _context.ToDos
                .Where(todo => todo.ChecklistId == checklistId)
                .ToListAsync();

            foreach (var toDo in toDos)
            {
                if (completedToDoIds.Contains(toDo.ToDoId) && !toDo.IsComplete)
                {
                    toDo.IsComplete = true;
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Checklists", new { id = checklistId });
        }
    }
}