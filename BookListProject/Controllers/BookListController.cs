using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookListProject.Controllers
{
    public class BookListController : Controller
    {
        private readonly ILogger<BookListController> _logger;
        private readonly Models.ApplicationDbContext _db;
        public IEnumerable<Models.Book> Books { get; set; }
        public BookListController(ILogger<BookListController> logger, Models.ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            Books = await _db.Book.ToListAsync();
            return View(Books);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Models.Book createdBook)
        {
            if (ModelState.IsValid)
            {
                await _db.AddAsync(createdBook);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return View();
        }

        public async Task<IActionResult> Detail(int id)
        {
            var book = await _db.Book.FindAsync(id);
            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _db.Book.FindAsync(id);
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Models.Book editedBook)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(editedBook).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var book = await _db.Book.FindAsync(id);
            if(book == null)
            {
                return NotFound();
            }
            _db.Book.Remove(book);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
