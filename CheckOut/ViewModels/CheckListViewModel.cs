namespace CheckOut.ViewModels
{
    public class ChecklistViewModel
    {
        public string Title { get; set; } = "";

        // The user can optionally add multiple ToDos when creating the checklist
        public List<string> ToDoDescriptions { get; set; } = new List<string>();
    }
}