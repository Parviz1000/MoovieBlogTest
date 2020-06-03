using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoovieBlog.Data;
using MoovieBlog.Models;
using MoovieBlog.ViewModels;

namespace MoovieBlog.Controllers
{
    public class MooviesController : Controller
    {
        readonly UserManager<User> _userManager;
        readonly SignInManager<User> _signInManager;
        readonly DataContext context;
        readonly IWebHostEnvironment _appEnvironment;
        public MooviesController(UserManager<User> userManager, SignInManager<User> signInManager, DataContext context, IWebHostEnvironment appEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.context = context;
            _appEnvironment = appEnvironment;

        }

        [HttpGet]
        public async Task<IActionResult> home(int page = 1)
        {
            int pageSize = 8;   
            IQueryable<Moovie> source = context.Moovies;
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            var viewModel = new MooviesViewModel
            {
                PageViewModel = pageViewModel,
                Movies = items
            };
            return View(viewModel);
        }

   
        public string randomId(string id, int lenght)
        {
            Random ran = new Random();
            string b = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string random = id;
            for (int i = 0; i <= lenght; i++)
            {
                int a = ran.Next(26);
                random += b.ElementAt(a);
            }
            return random;
        }

        [HttpGet]
        public IActionResult addmoovie()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> addmoovie(AddMoovieViewModel model, string id, IFormFile file)
        {
            if (model.Title == "" || model.Body == "" || model.SelectedFile == "" || model.Date == "" || model.Director == "")
            {
                ModelState.AddModelError("Error", "Title is needed");
                return NoContent();
            }
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var moovie = new Moovie { UserEmail = user.UserName, Title = model.Title, Body = model.Body, Director = model.Director, Date = model.Date, VideoLink = model.VideoLink};
            if (file != null && file.Length > 0 || file == null)
            {
                moovie.MoovieId = randomId(id, 27);
                try
                {
                    var imagepath = @"\Moovies\";
                    var uploadPath = _appEnvironment.WebRootPath + imagepath;

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    var uiqFileName = file.FileName;
                    var filename = Path.GetFileName(uiqFileName + "." + file.FileName.Split(".")[1].ToLower());
                    string fullPath = uploadPath + filename;

                    imagepath += @"\";
                    var filePath = @".." + Path.Combine(imagepath, filename);

                    if (!System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                        using var fileStream = new FileStream(fullPath, FileMode.Create);
                        await file.CopyToAsync(fileStream);
                    }

                    moovie.SelectedFile = filePath;
                    context.Moovies.Add(moovie);
                    context.SaveChanges();
                    return RedirectToAction("home", "Moovies");
                }
                catch (Exception) { }
            }
            Response.Redirect(Request.Path);
            return View(model);
        }


        [HttpGet]
        public IActionResult id(string id)
        {
            var moovie = context.Moovies.Find(id);
            if (moovie == null)
            {
                return NotFound();
            }
            return View(moovie);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var moovie = context.Moovies.Find(id);
            var model = new EditMoovieViewModel() { MoovieId = moovie.MoovieId, Body = moovie.Body, Date = moovie.Date, SelectedFile = moovie.SelectedFile, Director = moovie.Director, Title = moovie.Title };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> edit(EditMoovieViewModel model, IFormFile file)
        {
            var moovie = new Moovie { MoovieId = model.MoovieId, VideoLink = model.VideoLink, UserEmail = User.Identity.Name, Title = model.Title, Body = model.Body, Date = model.Date, Director = model.Director };

              if (model.Date == null || file == null)
              {
                  moovie.Date = model.Date;
                  return NoContent();
              }
             var imagepath = @"\Moovies\";
             var uploadPath = _appEnvironment.WebRootPath + imagepath;
             
             if (!Directory.Exists(uploadPath))
             {
                 Directory.CreateDirectory(uploadPath);
             }
             
             var uiqFileName = file.FileName;
             
             var filename = Path.GetFileName(uiqFileName + "." + file.FileName.Split(".")[1].ToLower());
             string fullPath = uploadPath + filename;
             
             imagepath += @"\";
             var filePath = @".." + Path.Combine(imagepath, filename);
             
             if (!System.IO.File.Exists(fullPath))
             {
                 System.IO.File.Delete(fullPath);
                 using var fileStream = new FileStream(fullPath, FileMode.Create);
                 await file.CopyToAsync(fileStream);
             }
             
             moovie.SelectedFile = filePath;
             context.Moovies.Update(moovie);
             context.SaveChanges();
             moovie.SelectedFile = filePath;
            return Redirect($"/Moovies/id/{moovie.MoovieId}/");
        }
    }
}