using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NotesWebApp.Models;
using Newtonsoft.Json;

namespace NotesWebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _httpClientNotesAPI;

    public HomeController(ILogger<HomeController> logger, HttpClient httpClientNotesAPI)
    {
        _logger = logger;
        _httpClientNotesAPI = httpClientNotesAPI;
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
        var response = _httpClientNotesAPI.GetAsync($"https://localhost:7276/api/Notes/{noteID}");
        HttpResponseMessage responseMessage = response.Result;
        var result = responseMessage.Content.ReadAsStringAsync().Result;
        dynamic notes = JsonConvert.DeserializeObject(result);
        var models = new List<Model>();
        foreach (var item in notes){
            models.Add(new Model{title = item.title, content=item.content});
        }
        return View(models);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
