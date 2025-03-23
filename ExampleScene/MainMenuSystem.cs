using UI.UISystems;

internal class MainMenuSystem : BaseMenuSystem<MainMenuController>
{
    private void Awake()
    {
        Controller = new();
    }
}