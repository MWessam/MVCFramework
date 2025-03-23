using UnityEngine.UIElements;

internal class LoginView : UIToolkitView<LoginViewModel>
{
    Button _loginButton;
    private TextField _usernameInput;
    protected override void OnInitialize()
    {
        _loginButton = UiDocument.rootVisualElement.Q<Button>("LoginButton");
        _loginButton.clicked += () =>
        {
            ViewModel.Username = _usernameInput.value;
            ButtonAction("Login", "Login");
        };
        _usernameInput = UiDocument.rootVisualElement.Q<TextField>("UsernameInput");
    }

    protected override void OnBind(LoginViewModel viewModel)
    {
    }

    protected override void OnUnbind(LoginViewModel viewModel)
    {
    }
}