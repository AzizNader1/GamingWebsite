
namespace GameZone.Services;
/// <summary>
/// this class implements the IGamesService interface which has many method that need to be implemented
/// inside this class there are many method and each of them will do something
/// GetAll() >> this will return the all games from the games table and also will return any other related data with these returned games
/// GetById() >> this method will return only one game according to the wanted id and also will return any other data related to the wanted game
/// Creat() >> this method will take the data of the new game that we need to create it and according to the success of the creation proccess
/// we also will upload the related image that atached with that game
/// Update() >> this method will take the new data of an existing game and make an update over them
/// and if the updated process success we will check about if the cover of that image needed to be also updated and delete the old one or not
/// Delete() >> this method will take the id of the wanted game to be deleted and also after the success of the delete method we also will delete it's own cover
/// </summary>
public class GamesService : IGamesService
{
    private readonly ApplicationDbContext _context;
    // this IWebHostEnvironment is related to the operations that we need to do on the local device of uploading or deleting files and other operations like this 
    // and we use it to make us able to access the files and directores and other data
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly string _imagesPath;

    public GamesService(ApplicationDbContext context, 
        IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        // according to the string interpolation the value of the _imagePath will be >> /wwwroot/assets/images/games
        _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagesPath}";
    }
    /// <summary>
    /// this will return all the games and also will return all the related data to them from other tables
    /// like the Categorty the each game belong to and the devices which support each game
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Game> GetAll()
    {
        return _context.Games
            .Include(g => g.Category)
            .Include(g => g.Devices)
            .ThenInclude(d => d.Device)
            .AsNoTracking()
            .ToList(); 
    }
    /// <summary>
    /// this will return the wanted game according to the id and also will return all the related data to them from other tables
    /// like the Categorty the each game belong to and the devices which support each game
    /// </summary>
    /// <returns></returns>
    public Game? GetById(int id)
    {
        return _context.Games
            .Include(g => g.Category)
            .Include(g => g.Devices)
            .ThenInclude(d => d.Device)
            .AsNoTracking()
            .SingleOrDefault(g => g.Id == id);
    }
    /// <summary>
    /// this method will take a modle of type CreateGameFormViewModel which has all the related data of creating new game
    /// and this method will break this function in to steps on of the related file and the other step to the actual game to be add in the games table
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task Create(CreateGameFormViewModel model)
    {
        var coverName = await SaveCover(model.Cover); // this will handle the process of the upload file and will return the path of saving the file if the process is done successfully
    
        Game game = new()
        {
           Name = model.Name,
           Description = model.Description,
           CategoryId = model.CategoryId,
           Cover = coverName,
           Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList()
        };

        _context.Add(game);
        _context.SaveChanges();
    }

    public async Task<Game?> Update(EditGameFormViewModel model)
    {
        // in the following selection of the game according to the id of the game which came in the modle 
        // we didn't select the categories inside the same step because the supported devices may be change over time
        // but the category which that game fill in it will never change that is the reason why we didn't take the update of the catergory in out consideratoin
        var game = _context.Games
            .Include(g => g.Devices)
            .SingleOrDefault(g => g.Id == model.Id);

        if (game is null)
            return null;

        var hasNewCover = model.Cover is not null; // this mean the user upload a new cover to that iamge
        var oldCover = game.Cover; // this will hold the cover of the game before any update

        game.Name = model.Name;
        game.Description = model.Description;
        game.CategoryId = model.CategoryId;
        game.Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList(); // if there is any change of the supported devices

        // if the game has a new cover we will do the same step as we will create a new image to that game 
        // by calling the SaveCover() that will handle these operation
        if (hasNewCover)
        {
            game.Cover = await SaveCover(model.Cover!);
        }

        var effectedRows = _context.SaveChanges(); // this will inform us if there is any change of the effected rows in the database accoring to the update proccess on the game data

        if(effectedRows > 0)
        {
            if(hasNewCover)
            {
                var cover = Path.Combine(_imagesPath, oldCover);
                File.Delete(cover);
            }

            return game;
        }
        // this else will handle the case if the update process fail and the game has a new cover saved in our files 
        // in this case we will delete the new uploaded file because the update process fail and there is no sense to has a new cover while all the other new data not applied
        else
        {
            var cover = Path.Combine(_imagesPath, game.Cover);
            File.Delete(cover);

            return null;
        }
    }
    /// <summary>
    /// this method will take a parameter named id, according to this parameter we will selected the existing game
    /// that related to the passed id, and if the game exist the game will be deleted and after the succession of the delete process
    /// we will delete the related cover to that game 
    /// and if there is no game to that id then there is no thing will happend
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool Delete(int id)
    {
        var isDeleted = false; // this is named flag in the programming concepts

        var game = _context.Games.Find(id);

        if (game is null)
            return isDeleted;

        _context.Remove(game);
        var effectedRows = _context.SaveChanges(); // this will make us able to know if there is any row effected by the deletion or not

        if(effectedRows > 0)
        {
            isDeleted = true;

            var cover = Path.Combine(_imagesPath, game.Cover);
            File.Delete(cover);
        }

        return isDeleted;
    }
    /// <summary>
    /// this method will take a cover of type IFormFile which will represent the image of the game 
    /// and according to that we will copy the image in the predefined path and return the path of that image 
    /// to be stored as a string in the game table which will make up able to access this image by using it's own path
    /// </summary>
    /// <param name="cover"></param>
    /// <returns></returns>
    private async Task<string> SaveCover(IFormFile cover)
    {
        var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";

        var path = Path.Combine(_imagesPath, coverName);

        using var stream = File.Create(path);
        await cover.CopyToAsync(stream);

        return coverName;
    }
}