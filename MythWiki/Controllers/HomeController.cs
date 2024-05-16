using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MythWiki.Models;
using MythWikiBusiness.Models;
using MythWikiBusiness.Services;
using MythWikiBusiness.IRepository;
using MythWikiBusiness.DTO;
using MythWikiData.Repository;

namespace MythWiki.Controllers;

public class HomeController : Controller
{
    

    private readonly ILogger<HomeController> _logger;
    private readonly UserService userservice;

    private List<Subject> subjectlist = new List<Subject>();

    public HomeController()
    {
        userservice = new UserService(new UserRepository());
    }

    public IActionResult Index()
    {
        UserViewModel userviewmodel = new UserViewModel();

        List<User> users = new List<User>();
        List<UserDTO> userDTOs = userservice.GetAllUsers();
        foreach (var dto in userDTOs)
        {
            users.Add(new User(dto));
        }

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

