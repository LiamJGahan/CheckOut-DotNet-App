namespace CheckOut.Models
{
    public class Checklist
    {
        public int ChecklistId { get; set; }
        public string Title { get; set; } = "";
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public bool IsArchived { get; set; } = false;

        public List<ToDo> ToDos { get; set; } = new List<ToDo>();

        public int UserId { get; set; }
        public User User { get; set; }
    }
}