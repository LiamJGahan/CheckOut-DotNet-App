using Microsoft.AspNetCore.Mvc;

namespace CheckOut.Models
{
    [Route("Checklists")]
    public class ChecklistsController : Controller
    {     
        private static List<Checklist> checklists = new List<Checklist>();

        [HttpGet("Read")]
        public IActionResult Read()
        {   
            return View(checklists);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {   
            return View(new Checklist());
        }

        [HttpPost("Create")]
        public IActionResult Create(Checklist checklist)
        {   
            if (ModelState.IsValid)
            {
                checklists.Add(checklist);
                return RedirectToAction("Index", "Home");
            }

            return View(checklist);
        }

        [HttpGet("Delete")]
        public IActionResult Delete()
        {   
            return View(checklists);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(List<int> selectedIds)
        {
            List<Checklist> toDelete = new List<Checklist>();

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

            foreach (var list in toDelete)
            {
                checklists.Remove(list);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}