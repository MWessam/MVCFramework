using UnityEngine.UIElements;

internal class MainMenuView : UIToolkitView<MainMenuViewModel>
{
    Label _usernameLabel;
    private Button _backButton;
    protected override void OnInitialize()
    {
        _backButton = UiDocument.rootVisualElement.Q<Button>("BackButton");
        _backButton.clicked += () =>
        {
            
            ButtonAction("Back", "MainMenu");
        };
        _usernameLabel = UiDocument.rootVisualElement.Q<Label>("Username");
    }

    protected override void OnBind(MainMenuViewModel viewModel)
    {
        _usernameLabel.text = viewModel.Username;
    }

    protected override void OnUnbind(MainMenuViewModel viewModel)
    {
    }
}