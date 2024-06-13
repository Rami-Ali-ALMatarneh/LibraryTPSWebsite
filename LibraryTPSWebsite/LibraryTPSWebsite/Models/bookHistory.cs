namespace LibraryTPSWebsite.Models
    {
    public class bookHistory
        {
        public int Id { get; set; }
        public int shelfID { get; set; }
        public int BookID { get; set; }
        public string BookName { get; set; }
        public DateTime ChangeDate { get; set; } = DateTime.Now;
        public string ChangeType { get; set; }
        }
    }
