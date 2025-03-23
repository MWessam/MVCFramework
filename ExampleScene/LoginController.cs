using UnityEngine;

internal class LoginController : Controller
{
    private LoginViewModel _loginViewModel;
    private UserSerivce _userService;
    public LoginController()
    {
        _loginViewModel = new LoginViewModel();
        _userService = ServicesReferences.Instance.UserService;
    }

    public override void Initialize()
    {
        base.Initialize();
        View("Login", _loginViewModel);
    }
    public void Login()
    {
        _userService.Login(_loginViewModel.Username);
        RedirectToAction("MainMenu", "MainMenu");
    }
}

internal class LoginViewModel
{
    public string Username { get; set; }
}

internal class UserSerivce
{
    private string _user;
    public void Login(string username)
    {
        _user = username;
    }
    public string GetUsername()
    {
        return _user;
    }
}