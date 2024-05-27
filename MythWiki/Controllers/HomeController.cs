using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MythWiki.Models;
using MythWikiBusiness.Models;
using MythWikiBusiness.Services;
using MythWikiBusiness.IRepository;
using MythWikiBusiness.DTO;
using MythWikiData.Repository;
using MythWikiBusiness.ErrorHandling;

namespace MythWiki.Controllers;

public class HomeController : Controller
{
    

    private readonly ILogger<HomeController> _logger;
    private readonly UserService userservice;
    private readonly SubjectService subjectservice;

    private List<Subject> subjectlist = new List<Subject>();

    public HomeController()
    {
        userservice = new UserService(new UserRepository());
        subjectservice = new SubjectService(new SubjectRepository());
    }

    public IActionResult Index()
    {
        SubjectViewModel subjectviewmodel = new SubjectViewModel();
        List<Subject> subjects = subjectservice.GetAllSubjects();
        subjectviewmodel.subjectlist = subjects;   
        return View(subjectviewmodel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Subject(int id)
    {
        try 
	    {
            var subject = subjectservice.GetSubjectById(id);

            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }
        catch(DatabaseError dbex) 
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
            var subject = subjectservice.DeleteSubject(subjectID);
            return RedirectToAction("Index");
        }
        catch(DatabaseError dbex) 
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
    public IActionResult AddSubject(string title, string text, int editorid, string imagelink, string authorname)
    {       
        try 
	    {
            subjectservice.CreateSubject(title, text, editorid, imagelink, authorname);
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
        var subject = subjectservice.GetSubjectById(id);

        if (subject == null)
        {
            return NotFound();
        }
        return View(subject);
    }

    [HttpPost]
    public IActionResult EditSubject(int subjectID, string title, string text, int editorid, string imagelink, string authorname)
    {
        try
        {
            var subjectDTO = new SubjectDTO
            {
                SubjectID = subjectID,
                Title = title,
                Text = text,
                EditorID = editorid,
                Image = imagelink,
                Author = authorname,
            };

            var updatedSubject = subjectservice.EditSubject(subjectDTO);

            return RedirectToAction("Subject", new { id = updatedSubject.SubjectID });
        }
        catch (DatabaseError dbex)
        {
            TempData["ErrorMessage"] = dbex.Message;
            return RedirectToAction("Index");
        }
        catch (UserError sex)
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
    public IActionResult Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var user = userservice.Authenticate(model.Username, model.Password);
                // Set up session or authentication cookie here
                TempData["SuccessMessage"] = "Login successful!";
                return RedirectToAction("Index");
            }
            catch (UnauthorizedAccessException)
            {
                TempData["ErrorMessage"] = "Invalid username or password.";
            }
            catch (DatabaseError dbex)
            {
                TempData["ErrorMessage"] = dbex.Message;
                return RedirectToAction("Index");
            }
            catch (UserError uex)
            {
                TempData["ErrorMessage"] = uex.Message;
                return RedirectToAction("Index");
            }
        }
        return View(model);
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
                var user = userservice.Register(model.Username, model.Password, model.Email);
                TempData["SuccessMessage"] = "Registration successful! Please log in.";
                return RedirectToAction("Login");
            }
            catch (UserError uex)
            {
                TempData["ErrorMessage"] = uex.Message;
            }
        }
        return View(model);
    }
}

