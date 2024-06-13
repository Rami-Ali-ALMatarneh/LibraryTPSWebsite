using LibraryTPSWebsite.Models;

namespace LibraryTPSWebsite.ViewModel
    {
    public class ListOfViewModel
        {
        public IEnumerable<shelf> Shelf { get; set; }
        public IEnumerable<book> Books { get; set; }
        }
    }
