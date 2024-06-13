namespace LibraryTPSWebsite.Models
    {
    public class book
        {
    
        public int BookID { get; set; }
        public string BookName { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string PhotoUrl { get; set; }
        public int shelfID { get; set; }
        public bool IsDeleted { get; set; } = false;
        public shelf shelf { get; set; }
        }
    }