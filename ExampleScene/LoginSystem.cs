using System;
using UI.UISystems;

internal class LoginSystem : BaseMenuSystem<LoginController>
{
    private void Awake()
    {
        Controller = new();
    }
}