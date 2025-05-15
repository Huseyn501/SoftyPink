using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftyPinko.DAL;
using SoftyPinko.Helper.Extensions;
using SoftyPinko.Models;
using SoftyPinko.ViewModels;
using System.Threading.Tasks;

namespace SoftyPinko.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentController : Controller
    {
        AppDbContext _context;
        private readonly IWebHostEnvironment environment;

        public CommentController(AppDbContext context,IWebHostEnvironment environment)
        {
            _context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            var comments =_context.Comments.ToList();
            return View(comments);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CommentVm commentVm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var comment = new Comment
            {
                Name = commentVm.Name,
                Position = commentVm.Position,
                Text = commentVm.Text,
            };
            if (commentVm.Image == null)
            {
                ModelState.AddModelError("Image", "Please select an image");
                return View();
            }
            if (!commentVm.Image.ContentType.Contains("image"))
            {
                ModelState.AddModelError("image", "Please select an image!");
            }
            if(commentVm.Image.Length > 2097152)
            {
                ModelState.AddModelError("image", "Image size must be less than 2MB");
            }
            comment.ImgUrl = commentVm.Image.CreatingImage(environment.WebRootPath, "upload");
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            var comment = _context.Comments.FirstOrDefault(x=>x.Id == id);
            if (comment == null)
            {
                return RedirectToAction("Index");
            }
            CommentVm commentVm = new CommentVm
            {
                Name = comment.Name,
                Position = comment.Position,
                Text = comment.Text,
            };
            return View(commentVm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,CommentVm commentVm)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (commentVm.Image == null)
            {
                ModelState.AddModelError("Image", "Please select an image");
                return View();
            }
            if (!commentVm.Image.ContentType.Contains("image"))
            {
                ModelState.AddModelError("image", "Please select an image!");
            }
            if (commentVm.Image.Length > 2097152)
            {
                ModelState.AddModelError("image", "Image size must be less than 2MB");
            }
            if (commentVm.Image != null)
            {
                if (comment.ImgUrl != null)
                {
                    comment.ImgUrl.DeletingImage(environment.WebRootPath, "Upload");
                }
            }
            comment.Name = commentVm.Name;
            comment.Position = commentVm.Position;
            comment.Text = commentVm.Text;
            comment.ImgUrl = commentVm.Image.CreatingImage(environment.WebRootPath, "upload");
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null)
            {
                return NotFound();
            }
            if (comment.ImgUrl != null)
            {
                comment.ImgUrl.DeletingImage(environment.WebRootPath, "Upload");
            }
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
