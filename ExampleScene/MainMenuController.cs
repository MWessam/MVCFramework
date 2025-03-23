public class MainMenuController : Controller
{
    private MainMenuViewModel _mainMenuViewModel;
    private UserSerivce _userService;
    public MainMenuController()
    {
        _mainMenuViewModel = new MainMenuViewModel();
        _userService = ServicesReferences.Instance.UserService;
    }

    public override void Initialize()
    {
        base.Initialize();
    }
    public void MainMenu()
    {
        _mainMenuViewModel.Username = _userService.GetUsername();
        View("MainMenu", _mainMenuViewModel);
    }
}

internal class MainMenuViewModel
{
    public string Username { get; set; }
}
