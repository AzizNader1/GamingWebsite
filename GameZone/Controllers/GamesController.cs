namespace GameZone.Controllers;
public class GamesController : Controller
{
    private readonly ICategoriesService _categoriesService;
    private readonly IDevicesService _devicesService;
    private readonly IGamesService _gamesService;

    public GamesController(ICategoriesService categoriesService, 
        IDevicesService devicesService, 
        IGamesService gamesService)
    {
        _categoriesService = categoriesService;
        _devicesService = devicesService;
        _gamesService = gamesService;
    }
    /// <summary>
    /// this method will invok the GetAll() from the gamesService and will return all the existing games and all the related data to them
    /// </summary>
    /// <returns></returns>
    public IActionResult Index()
    {
        var games = _gamesService.GetAll();
        return View(games);
    }
    
    /// <summary>
    /// this method will invok the GetById() from the gameService and will return all the data that are related to the wanted game
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public IActionResult Details(int id)
    {
        var game = _gamesService.GetById(id);

        if(game is null)
            return NotFound();

        return View(game);
    }

    /// <summary>
    /// this method related to the get view that only will display the form the user will use to enter the data of the new game
    /// and inside this get method we will select the existing categoires and all the existing supported devices 
    /// that will make the user able to define which category that game fill in and which devices are support that game
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Create()
    {
        CreateGameFormViewModel viewModel = new()
        {
            Categories = _categoriesService.GetSelectList(),
            Devices = _devicesService.GetSelectList()
        };

        return View(viewModel);
    }

    /// <summary>
    /// this method will invok the Create() method form the gameService and that is the actual create to the new game
    /// if the data atached match all the requirments the already defined
    /// and if there is any error happend the data of the categories and devices to the view once more time again
    /// to make the use able to re-enter the data again
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateGameFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = _categoriesService.GetSelectList();
            model.Devices = _devicesService.GetSelectList();
            return View(model);
        }

        await _gamesService.Create(model);

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// this method related to the get view that only will display the form that user will use to enter the data of the new data that he need to applied to an existing game
    /// and inside this get method we will select the existing categoires and all the existing supported devices and the old data of the wanted game
    /// that will make the user able to define which category that game fill in and which devices are support that game
    /// and make his own update on the existing data
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var game = _gamesService.GetById(id);

        if (game is null)
            return NotFound();

        EditGameFormViewModel viewModel = new()
        {
            Id = id,
            Name = game.Name,
            Description = game.Description,
            CategoryId = game.CategoryId,
            SelectedDevices = game.Devices.Select(d => d.DeviceId).ToList(),
            Categories = _categoriesService.GetSelectList(),
            Devices= _devicesService.GetSelectList(),
            CurrentCover = game.Cover
        };

        return View(viewModel);
    }

    /// <summary>
    /// this method will invoke the Update() method which exist in the gameService and that will make the actual update of the game
    /// and if the update fail it will return the same view again and also will return the old data of the game
    /// and if the update success will return the index view which will display all the games in the database and will dispaly the underworking game with it's new data
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditGameFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = _categoriesService.GetSelectList();
            model.Devices = _devicesService.GetSelectList();
            return View(model);
        }

        var game = await _gamesService.Update(model);

        if (game is null)
            return BadRequest();

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// this method will invoke the Delete() method that exist in the gameService and that method will take the id
    /// of the game that use need to delete it 
    /// and if the deletion success it will return ture and otherwise false
    /// and according to the returned data it will determine which action will take Ok() or BadRequest()
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var isDeleted = _gamesService.Delete(id);

        return isDeleted ? Ok() : BadRequest();
    }
}