using LibraryTPSWebsite.Data;
using LibraryTPSWebsite.Extensions;
using LibraryTPSWebsite.Models;
using LibraryTPSWebsite.Repositories.IRepo;
using LibraryTPSWebsite.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LibraryTPSWebsite.Controllers
    {
    public class HomeController : Controller
        {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookRepository bookRepository;
        private readonly IShelfRepository shelfRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public HomeController( ILogger<HomeController> logger, IBookRepository bookRepository, IShelfRepository shelfRepository, IWebHostEnvironment webHostEnvironment )
            {
            _logger = logger;
            this.bookRepository = bookRepository;
            this.shelfRepository = shelfRepository;
            this.webHostEnvironment = webHostEnvironment;
            }

        public IActionResult Index()
            {
            return View();
            }
        public async Task<IActionResult> shelf()
            {
            var shelves = await shelfRepository.GetAllShelfAsync();
            if (shelves == null || !shelves.Any())
                {
                return View();
                }
            return View(shelves);
            }
        [HttpGet]
        public async Task<IActionResult> AddShelf()
            {
            return View();
            }
        [HttpPost]
        public async Task<IActionResult> AddShelf( ShelfViewModel addShelf )
            {
            if (ModelState.IsValid)
                {
                var shelfCheck = await shelfRepository.GetShelfNameAsync(addShelf.Name);
                if (shelfCheck == null)
                    {
                    shelf shelf = new shelf
                        {
                        Name = addShelf.Name
                        };
                    await shelfRepository.AddShelfAsync(shelf);
                    return RedirectToAction("shelf");
                    }
                else
                    {
                    TempData["AlertMessage"] = "shelf already exists.";
                    return View();
                    }
                }
            return View();
            }
        [HttpGet("home/UpdateShelf/{id}")]
        public async Task<IActionResult> UpdateShelf( int id )
            {
            var shelf = await shelfRepository.GetShelfByIdAsync(id);
            var editShelf = new UpdateShelfModel
                {
                Name = shelf.Name
                };
            return View(editShelf);
            }
        [HttpPost("home/UpdateShelf/{id}")]
        public async Task<IActionResult> UpdateShelf( int id, UpdateShelfModel updateShelf )
            {
            if (ModelState.IsValid)
                {
                var shelf = await shelfRepository.GetShelfNameAsync(updateShelf.Name);
                var currentShelf = await shelfRepository.GetShelfByIdAsync(id);
                if (shelf == null)
                    {
                    currentShelf.Name = updateShelf.Name;
                    await shelfRepository.UpdateShelfAsync(currentShelf);
                    return RedirectToAction("shelf");
                    }
                else
                    {
                    TempData["AlertMessage"] = "shelf Name is exists.";
                    return View();
                    }
                }
            return View();
            }
        public async Task<IActionResult> SoftDelete( int id )
            {
            var currentShelf = await shelfRepository.GetShelfByIdAsync(id);
            if (currentShelf.IsDeleted == true)
                {
                TempData["AlertMessage"] = "shelf is not found.";
                }
            else
                {
                var resultShelf = await shelfRepository.GetAllShelfWithBookAsync(id);
                if (resultShelf.Count() == 0)
                    {
                    currentShelf.IsDeleted = true;
                    await shelfRepository.UpdateShelfAsync(currentShelf);          
                    TempData["AlertMessage"] = "Shelve has going to Recycle.";
                    }
                else
                    {
                    TempData["AlertMessage"] = "you must delete all books.";
                    }
                }
            return RedirectToAction("shelf");
            }
        [HttpGet("home/AddBook/{id}")]
        public async Task<IActionResult> AddBook( int id )
            {
            var shelve = await shelfRepository.GetShelfByIdAsync(id);
            ViewData["ShelveName"] = shelve.Name;
            return View();
            }
        [HttpPost("home/AddBook/{id}")]
        public async Task<IActionResult> AddBook( int id, AddBookViewModel addBookViewModel )
            {
            var shelve = await shelfRepository.GetShelfByIdAsync(id);
            if (ModelState.IsValid)
                {
                var Book = await bookRepository.GetBookByNameAsync(addBookViewModel.BookName);
                if (Book == null)
                    {
                    book book = new book()
                        {
                        BookName = addBookViewModel.BookName,
                        Title = addBookViewModel.Title,
                        shelfID = shelve.shelfID,
                        PhotoUrl = PhotoExtensions.ConvertPhotoToString(addBookViewModel.image, webHostEnvironment)
                        };
                    await bookRepository.AddBookAsync(book);
                    }
                else
                    {
                    TempData["AlertMessage"] = "Book is already exists.";
                    return View();
                    }
                return RedirectToAction("shelf");
                }
            return View();
            }

        public async Task<IActionResult> SoftDeleteBook( int id )
            {
            var currentSBook = await bookRepository.GetBookByIdAsync(id);
            if (currentSBook.IsDeleted == true)
                {
                TempData["AlertMessage"] = "Book is not found.";
                return View();
                }
            else
                {
                    currentSBook.IsDeleted = true;
                    await bookRepository.UpdateBookAsync(currentSBook);
                }
            TempData["AlertMessage"] = "Book has going to Recycle.";
            return RedirectToAction("shelf");
                }

        [HttpGet("home/UpdateBook/{id}")]
        public async Task<IActionResult> UpdateBook(int id)
            {
            var book = await bookRepository.GetBookByIdAsync(id);
     
            var updateBookViewModel = new UpdateBookViewModel
                {
                BookName=book.BookName,
                Title =book.Title,
                };      
            var shelve = await shelfRepository.GetShelfByIdAsync(book.shelfID);
            ViewData["ShelveName"] = shelve.Name;
            return View(updateBookViewModel);
            }
        public async Task<IActionResult> Recycle()
            {
            var shelves = await shelfRepository.GetAllShelfAsync();
            var listOfViewModel = new ListOfViewModel();
            if (shelves == null || !shelves.Any())
                {
                return View(listOfViewModel);
                }
            listOfViewModel.Shelf= shelves;
            return View(listOfViewModel);
            }
        [HttpPost("home/UpdateBook/{id}")]
        public async Task<IActionResult> UpdateBook( int id, UpdateBookViewModel updateBookViewModel )
            {
            if (ModelState.IsValid)
                {
                var book = await bookRepository.GetBookByIdAsync(id);
                    book.BookName = updateBookViewModel.BookName;
                    book.Title = updateBookViewModel.Title;
                   if(updateBookViewModel.formFile!=null)
                    {
                    PhotoExtensions.DeletePhotoFromImageFolder(book.PhotoUrl, webHostEnvironment);
                    book.PhotoUrl = PhotoExtensions.ConvertPhotoToString(updateBookViewModel.formFile, webHostEnvironment);
                    }     
                    await bookRepository.UpdateBookAsync(book);
                    return RedirectToAction("shelf");
                 }
                   return View();
             }       
        public async Task<IActionResult> HardDeletedBook(int id)
            {
            var book = await bookRepository.GetBookByIdAsync(id);
            if(book == null)
                TempData["AlertMessage"] = "Book is not found.";
            await bookRepository.DeleteBookAsync(id);
            TempData["AlertMessage"] = "Book is Deleted from Database.";

            return RedirectToAction("Recycle");
            }
        public async Task<IActionResult> RestoreBook( int id )
            {
            var book = await bookRepository.GetBookByIdAsync(id);
            if (book == null)
                TempData["AlertMessage"] = "Book is not found.";
            if (book.IsDeleted == true)
                {
                book.IsDeleted = false;
                await bookRepository.UpdateBookAsync(book);
                TempData["AlertMessage"] = book.BookName+" is Restored.";
                }
            return RedirectToAction("Recycle");
            }
        public async Task<IActionResult> SwapShelve( int id,int shelveId )
            {
            var book = await bookRepository.GetBookByIdAsync(id);
            if (book == null)
                TempData["AlertMessage"] = "Book is not found.";
            if (book.IsDeleted == true)
                {
                var shelve = await shelfRepository.GetShelfByIdAsync(shelveId); 
                if(shelve!=null)
                    {
                    book.IsDeleted = false;
                    book.shelfID = shelveId;
                    await bookRepository.UpdateBookAsync(book);
                    TempData["AlertMessage"] = book.BookName + "is Swapped and is Restored.";
                    }
                }
            return RedirectToAction("Recycle");
            }
        public async Task<IActionResult> HardDeletedShelve( int id )
            {
            var shelve = await shelfRepository.GetShelfByIdAsync(id);
            if (shelve == null)
                TempData["AlertMessage"] = "shelve is not found.";
            if(shelve.books.Count == 0)
                {
                await shelfRepository.DeleteShelfAsync(id);
                TempData["AlertMessage"] = "shelve is Deleted from Database.";
                }
            else
                {
                TempData["AlertMessage"] = "Shelve have Book, You must Delete book from Database.";
                }
            return RedirectToAction("Recycle");
            }
        public async Task<IActionResult> RestoreShelve( int id )
            {
            var shelve = await shelfRepository.GetShelfByIdAsync(id);
            if (shelve == null)
                TempData["AlertMessage"] = "shelve is not found.";
            if (shelve.IsDeleted == true)
                {
                shelve.IsDeleted = false;
                await shelfRepository.UpdateShelfAsync(shelve);
                TempData["AlertMessage"] = shelve.Name + " is Restored.";
                }
            return RedirectToAction("Recycle");
            }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
