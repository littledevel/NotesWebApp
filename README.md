
### Preparatory work

`dotnet new mvc -o NotesWebApp`

`cd NotesWebApp`

`dotnet dev-certs https --trust`

`dotnet run --launch-profile https`

### Correct launchSettings.json

### Test again it works

`dotnet run --launch-profile https`

### Inject an httpClient
In Program.cs
```
builder.Services.AddHttpClient();
```


### Change HomeController.cs for httpClient
```
private readonly HttpClient _httpClientNotesAPI;

  public HomeController(ILogger<HomeController> logger, HttpClient httpClientNotesAPI)
    {
        _logger = logger;
        _httpClientNotesAPI = httpClientNotesAPI;

```

### Test it works
`dotnet run --launch-profile https`

### Start preparing swagger actions

```
public IActionResult NoteResults(string noteID){
        var response = _httpClientNotesAPI.GetAsync($"https://localhost:7000/api/Notes/{noteID}");
        HttpResponseMessage responseMessage = response.Result;
        var result = responseMessage.Content.ReadAsStringAsync().Result;
        dynamic notes = JsonConvert.DeserializeObject(result);
        var models = new List<Model>();
        foreach (var item in notes){
            models.Add(new Model{title = item.title, content=item.content});
        }
        return View(models);
    }
```

### Explain serialize, deserialize

### Add json 
`dotnet add package Newtonsoft.Json`

### Explain new model needed to send data to vew


### Add new view for notes list
In Views/Home/NoteResults.cshtml
```
@*
    For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
*@
@{
}
@model List<Model>

@foreach (var item in Model){
    <p>Title: @item.title</p>
    <p>Content: @item.content</p>
}

```

### Add the view name in the shared _layout

### Add a Search action

```
   public IActionResult Search(){
        return View();
    }
```


### Add a Search cshtml
```
@*
    For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
*@
@{
}


<form action="/Home/NotesResult">
    Give your text <textarea name="mytext" id="mytext"></textarea><br />
    <input type="submit" value="Press to proceed"> 
</form>
```


### Add a View for single Search Results

