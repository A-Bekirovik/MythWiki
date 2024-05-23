using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MythWiki.Models;
using MythWikiBusiness.Models;
using MythWikiBusiness.Services;
using MythWikiBusiness.IRepository;
using MythWikiBusiness.DTO;
using MythWikiData.Repository;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        var subject = subjectservice.GetSubjectById(id);
        if (subject == null)
        {
            return NotFound();
        }
        return View(subject);
    }

    public IActionResult DeleteSubject()
    {
        return View();
    }

    [HttpPost]
    public IActionResult DeleteSubject(int subjectID)
    {
        var response = subjectservice.DeleteSubject(subjectID);

        if (!response.Succes)
        {
            TempData["ErrorMessage"] = response.ErrorMessage;
            return RedirectToAction("RemoveSubject");
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult AddSubject(string title, string text, int editorid, string imagelink, string authorname)
    {
        var response = subjectservice.CreateSubject(title, text, editorid, imagelink, authorname);

        if(!response.Succes) 
	    {
            TempData["Errormessage"] = response.ErrorMessage;
            return RedirectToAction("AddSubject"); 
	    }
        return RedirectToAction("Index");
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
    public IActionResult EditSubject(int subjectID, string title, string text, int editorid, string imagelink, string authorname, DateTime date)
    {
        var subjectDTO = new SubjectDTO
        {
            SubjectID = subjectID,
            Title = title,
            Text = text,
            EditorID = editorid,
            Image = imagelink,
            Author = authorname,
            Date = DateTime.Now
        };

        var response = subjectservice.EditSubject(subjectDTO);
        if (!response.Succes)
        {
            TempData["ErrorMessage"] = response.ErrorMessage;
            return RedirectToAction("EditSubject", new { id = subjectID });
        }
        return RedirectToAction("Index");
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
}

