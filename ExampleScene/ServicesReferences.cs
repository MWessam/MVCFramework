using UnityEngine;

[DefaultExecutionOrder(-1000)]
internal class ServicesReferences : Singleton<ServicesReferences>
{
    public UserSerivce UserService { get; private set; } = new();
}