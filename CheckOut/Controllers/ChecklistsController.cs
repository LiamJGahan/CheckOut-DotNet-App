using CheckOut.Data;
using CheckOut.Sessions;
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
        [UserSession]
        [HttpGet("ReadCurrent")]
        public async Task<IActionResult> ReadCurrent()
        {   
            int userId = HttpContext.Session.GetInt32("UserId")!.Value;

            List<Checklist> checklists = await _context.Checklists
                .Include(checklist => checklist.ToDos)
                .Where(checklist => !checklist.IsArchived && checklist.UserId == userId)
                .ToListAsync();

            return View(checklists);
        }

        // Return the archived checklists
        [UserSession]
        [HttpGet("ReadArchived")]
        public async Task<IActionResult> ReadArchived()
        {   
            int userId = HttpContext.Session.GetInt32("UserId")!.Value;

            List<Checklist> checklists = await _context.Checklists
                .Include(checklist => checklist.ToDos)
                .Where(checklist => checklist.IsArchived && checklist.UserId == userId)
                .ToListAsync();

            return View(checklists);
        }

        [UserSession]
        [HttpGet("Create")]
        public IActionResult Create()
        {   
            return View(new ChecklistViewModel());
        }

        [UserSession]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ChecklistViewModel model)
        {
            int userId = HttpContext.Session.GetInt32("UserId")!.Value;

            Checklist checklist = new Checklist
            {
                Title = model.Title,
                DateCreated = DateTime.Now,
                ToDos = model.ToDoDescriptions
                    .Where(description => !string.IsNullOrWhiteSpace(description))
                    .Select(description => new ToDo { Description = description })
                    .ToList(),
                UserId = userId
            };

            _context.Checklists.Add(checklist);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [UserSession]
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromForm] int checklistId)
        {
            int userId = HttpContext.Session.GetInt32("UserId")!.Value;

            bool isArchived = false;

            var checklist = await _context.Checklists
                .Include(c => c.ToDos)
                .FirstOrDefaultAsync(c => c.ChecklistId == checklistId && c.UserId == userId);

            if (checklist != null)
            {
                if (checklist.IsArchived){isArchived = true;}

                _context.ToDos.RemoveRange(checklist.ToDos); 
                _context.Checklists.Remove(checklist);  
                await _context.SaveChangesAsync();
            }
            else
            {
                // Add a problem response here
                return RedirectToAction("Index", "Home");
            }

            if (isArchived)
            {
                return RedirectToAction("ReadArchived", "Checklists");
            }
            else
            {
                return RedirectToAction("ReadCurrent", "Checklists");
            }
        }

        // Details is for veiwing a single Checklist by ID
        [UserSession]
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            int userId = HttpContext.Session.GetInt32("UserId")!.Value;

            var selectedChecklist = await _context.Checklists
                .Include(checklist => checklist.ToDos)
                .FirstOrDefaultAsync(checklist => checklist.ChecklistId == id && checklist.UserId == userId);

            if (selectedChecklist == null)
            {
                return NotFound();
            }

            return View(selectedChecklist);
        }

        [UserSession]
        [HttpPost("SetArchived")]
        public async Task<IActionResult> SetArchived([FromForm] int checklistId)
        {
            int userId = HttpContext.Session.GetInt32("UserId")!.Value;

            var checklist = await _context.Checklists
                .FirstOrDefaultAsync(checklist => checklist.ChecklistId == checklistId && checklist.UserId == userId);

            if (checklist != null)
            {
                checklist.IsArchived = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ReadCurrent", "Checklists");
        }
    }
}