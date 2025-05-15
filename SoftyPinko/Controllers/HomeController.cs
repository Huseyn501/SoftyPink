using Microsoft.AspNetCore.Mvc;
using SoftyPinko.DAL;

namespace SoftyPinko.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext _context;

        public HomeController(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public IActionResult Index()
        {
            var comments = _context.Comments.ToList();
            return View(comments);
        }
    }
}
