using Microsoft.AspNetCore.Mvc;

namespace CheckOut.Models
{
    [Route("ToDos")]
    public class ToDosController : Controller
    {     
        private static List<ToDo> ToDos = new List<ToDo>();

        [HttpGet("Read")]
        public IActionResult Read()
        {   
            return View(ToDos);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {   
            return View(new ToDo());
        }

        [HttpPost("Create")]
        public IActionResult Create(ToDo toDo)
        {   
            if (ModelState.IsValid)
            {
                ToDos.Add(toDo);
                return RedirectToAction("Index", "Home");
            }

            return View(toDo);
        }

        [HttpGet("Delete")]
        public IActionResult Delete()
        {   
            return View(ToDos);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(List<int> selectedIds)
        {
            List<ToDo> toDelete = new List<ToDo>();

            foreach (ToDo list in ToDos)
            {
                foreach (int id in selectedIds)
                {
                    if (id == list.ToDoId)
                    {
                        toDelete.Add(list);
                    }
                }
            }

            foreach (var list in toDelete)
            {
                ToDos.Remove(list);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}