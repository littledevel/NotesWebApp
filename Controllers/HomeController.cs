using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NotesWebApp.Models;
using Newtonsoft.Json;

namespace NotesWebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _httpClientNotesAPI;
    private readonly string _notesAPIHost;

    public HomeController(ILogger<HomeController> logger, HttpClient httpClientNotesAPI)
    {
        _logger = logger;
        _httpClientNotesAPI = httpClientNotesAPI;
        var notesAPIHost = Environment.GetEnvironmentVariable("NOTESAPI_HOST");
        if (notesAPIHost == null){
            _notesAPIHost =  "https://localhost:7000";
        } else {
            _notesAPIHost = notesAPIHost;
        }
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Search(){
        return View();
    }

    public IActionResult NoteResults(string noteID){
        var response = _httpClientNotesAPI.GetAsync($"{_notesAPIHost}/api/Notes/{noteID}");
        Console.WriteLine($"ID sent: {noteID}");
        HttpResponseMessage responseMessage = response.Result;
        var result = responseMessage.Content.ReadAsStringAsync().Result;
        dynamic notes = JsonConvert.DeserializeObject(result);
        var models = new List<Model>();
        foreach (var item in notes){
            models.Add(new Model{title = item.title, content=item.content});
        }
        return View(models);
    }

    
    public IActionResult NoteResult(string noteID){
        var response = _httpClientNotesAPI.GetAsync($"https://localhost:7000/api/Notes/{noteID}");
        HttpResponseMessage responseMessage = response.Result;
        var result = responseMessage.Content.ReadAsStringAsync().Result;
        dynamic note = JsonConvert.DeserializeObject(result);
        ViewData["title"]  = note.title;
        ViewData["content"] = note.content;
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
