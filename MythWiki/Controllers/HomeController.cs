using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MythWiki.Models;
using MythWikiLogic.Models;
using MythWikiLogic.Services;

namespace MythWiki.Controllers;

public class HomeController : Controller
{

    UserService userservice = new UserService();

    private readonly ILogger<HomeController> _logger;

    private List<Subject> subjectlist = new List<Subject>();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }



    public IActionResult Index()
    {
        UserViewModel userviewmodel = new UserViewModel();

        List<UserModel> users;
        users = userservice.GetAllUsers();
        userviewmodel.userlist = users;

        return View(userviewmodel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

        public IActionResult Subject()
    {
        return View();
    }

    [HttpPost]
    public ActionResult AddSubject(Subject newsubject)
    {        
        string title = newsubject.Title;
        string Text = newsubject.Text;
        string image = newsubject.Image;
        DateTime date = newsubject.Date;

        if (newsubject != null && !string.IsNullOrEmpty(newsubject.Title) && !string.IsNullOrEmpty(newsubject.Text) && !string.IsNullOrEmpty(newsubject.Image) && newsubject.Date != DateTime.MinValue)
        {
            subjectlist.Add(newsubject);

            return RedirectToAction("Index", "Home");
        }
        else
        {

            ModelState.AddModelError("", "Please fill in all the required fields.");
            return View();
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

    public IActionResult ShowList()
    {
        foreach (var subject in subjectlist)
        {
            Console.WriteLine(subject);
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

