using CheckOut.Data;
using CheckOut.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheckOut.Models
{
    [Route("Checklists")]
    public class ChecklistsController : Controller
    {     
        private readonly CheckOutContext _context;

        public ChecklistsController(CheckOutContext context)
        {
            _context = context;
        }

        // Returns the currently active checklists
        [HttpGet("ReadCurrent")]
        public async Task<IActionResult> ReadCurrent()
        {   
            List<Checklist> checklists = await _context.Checklists
                .Include(checklist => checklist.ToDos)
                .Where(checklist => !checklist.IsArchived)
                .ToListAsync();

            return View(checklists);
        }

        // Return the archived checklists
        [HttpGet("ReadArchived")]
        public async Task<IActionResult> ReadArchived()
        {   
            List<Checklist> checklists = await _context.Checklists
                .Include(checklist => checklist.ToDos)
                .Where(checklist => checklist.IsArchived)
                .ToListAsync();

            return View(checklists);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {   
            return View(new ChecklistViewModel());
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(ChecklistViewModel model)
        {
            // Add user check here later

            Checklist checklist = new Checklist
            {
                Title = model.Title,
                DateCreated = DateTime.Now,
                ToDos = model.ToDoDescriptions
                    .Where(description => !string.IsNullOrWhiteSpace(description))
                    .Select(description => new ToDo { Description = description })
                    .ToList(),
                UserId = 1 //relate this to user
            };

            _context.Checklists.Add(checklist);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Delete")]
        public async Task<IActionResult> Delete()
        {   
            List<Checklist> checklists = await _context.Checklists.ToListAsync();

            return View(checklists);
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(List<int> selectedIds)
        {
            List<Checklist> toDelete = new List<Checklist>();
            List<Checklist> checklists = await _context.Checklists.ToListAsync();

            foreach (Checklist list in checklists)
            {
                foreach (int id in selectedIds)
                {
                    if (id == list.ChecklistId)
                    {
                        toDelete.Add(list);
                    }
                }
            }

            foreach (Checklist list in toDelete)
            {
                _context.Checklists.Remove(list);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        // Details is for veiwing a single Checklist by ID
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var selectedChecklist = await _context.Checklists
                .Include(checklist => checklist.ToDos)
                .FirstOrDefaultAsync(checklist => checklist.ChecklistId == id);

            if (selectedChecklist == null)
            {
                return NotFound();
            }

            return View(selectedChecklist);
        }

        [HttpPost("SetArchived")]
        public async Task<IActionResult> SetArchived([FromForm] int checklistId)
        {
            var checklist = await _context.Checklists
                .FirstOrDefaultAsync(checklist => checklist.ChecklistId == checklistId);

            if (checklist != null)
            {
                checklist.IsArchived = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ReadCurrent", "Checklists");
        }
    }
}