using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MythWiki.Models;
using MythWikiBusiness.Models;
using MythWikiBusiness.Services;
using MythWikiBusiness.DTO;
using MythWikiBusiness.ErrorHandling;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace MythWiki.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _userService;
        private readonly SubjectService _subjectService;

        public HomeController(UserService userService, SubjectService subjectService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _subjectService = subjectService ?? throw new ArgumentNullException(nameof(subjectService));
        }

        public IActionResult Index()
        {
            var subjectViewModel = new SubjectViewModel
            {
                subjectlist = _subjectService.GetAllSubjects()
            };
            return View(subjectViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Subject(int id)
        {
            try
            {
                var subject = _subjectService.GetSubjectById(id);

                if (subject == null)
                {
                    return NotFound();
                }

                return View(subject);
            }
            catch (DatabaseError dbex)
            {
                TempData["ErrorMessage"] = dbex.Message;
                return RedirectToAction("Index");
            }
            catch (SubjectError sex)
            {
                TempData["ErrorMessage"] = sex.Message;
                return RedirectToAction("Index");
            }
        }

        public IActionResult DeleteSubject()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DeleteSubject(int subjectID)
        {
            try
            {
                var isDeleted = _subjectService.DeleteSubject(subjectID);
                if (!isDeleted)
                {
                    TempData["ErrorMessage"] = "Failed to delete the subject.";
                }
                return RedirectToAction("Index");
            }
            catch (DatabaseError dbex)
            {
                TempData["ErrorMessage"] = dbex.Message;
                return RedirectToAction("RemoveSubject");
            }
            catch (SubjectError sex)
            {
                TempData["ErrorMessage"] = sex.Message;
                return RedirectToAction("RemoveSubject");
            }
        }

        [HttpPost]
        public IActionResult AddSubject(string title, string text, int editorID, string imageLink, string authorName)
        {
            try
            {
                _subjectService.CreateSubject(title, text, editorID, imageLink, authorName);
                return RedirectToAction("Index");
            }
            catch (DatabaseError dbex)
            {
                TempData["ErrorMessage"] = dbex.Message;
                return RedirectToAction("AddSubject");
            }
            catch (SubjectError sex)
            {
                TempData["ErrorMessage"] = sex.Message;
                return RedirectToAction("AddSubject");
            }
        }

        [HttpGet]
        public IActionResult EditSubject(int id)
        {
            var subject = _subjectService.GetSubjectById(id);

            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        [HttpPost]
        public IActionResult EditSubject(int subjectID, string title, string text, int editorID, string imageLink, string authorName)
        {
            try
            {
                var subjectDTO = new SubjectDTO
                {
                    SubjectID = subjectID,
                    Title = title,
                    Text = text,
                    EditorID = editorID,
                    Image = imageLink,
                    Author = authorName,
                };

                var updatedSubject = _subjectService.EditSubject(subjectDTO);

                return RedirectToAction("Subject", new { id = updatedSubject.SubjectID });
            }
            catch (DatabaseError dbex)
            {
                TempData["ErrorMessage"] = dbex.Message;
                return RedirectToAction("Index");
            }
            catch (SubjectError sex)
            {
                TempData["ErrorMessage"] = sex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult AddSubject()
        {
            return View();
        }

        public IActionResult RemoveSubject()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _userService.Authenticate(model.Username, model.Password);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index");
                }
                catch (UnauthorizedAccessException)
                {
                    TempData["ErrorMessage"] = "Invalid username or password.";
                }
                catch (DatabaseError dbex)
                {
                    TempData["ErrorMessage"] = dbex.Message;
                    return RedirectToAction("Login");
                }
                catch (UserError uex)
                {
                    TempData["ErrorMessage"] = uex.Message;
                    return RedirectToAction("Login");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _userService.Register(model.Username, model.Password, model.Email);
                    TempData["SuccessMessage"] = "Registration successful! Please log in.";
                    return RedirectToAction("Login");
                }
                catch (DatabaseError dbex)
                {
                    TempData["ErrorMessage"] = dbex.Message;
                }
                catch (UserError uex)
                {
                    TempData["ErrorMessage"] = uex.Message;
                }
            }
            return View(model);
        }
    }
}