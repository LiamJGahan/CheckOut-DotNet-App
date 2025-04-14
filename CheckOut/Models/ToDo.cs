namespace CheckOut.Models
{
    public class ToDo
    {
        public int ToDoId { get; set; }
        public string Description { get; set; } = "";
        public bool IsComplete { get; set; } = false;

        public int ChecklistId { get; set; }
        public Checklist Checklist { get; set; }
    }
}