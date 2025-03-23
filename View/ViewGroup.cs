using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class ViewGroup : MonoBehaviour
{
    [Tooltip("Make sure the name of the view group matches the same name of the controller it needs.")]
    [SerializeField] private string _controllerName;
    public IView[] Views;
    public string ControllerName
    {
        get
        {
            if (string.IsNullOrEmpty(_controllerName))
            {
                _controllerName = GetType().Name.Replace("Controller", "");
            }
            return _controllerName;
        }
    }
    private void Awake()
    {
        Views = GetComponentsInChildren<IView>();
    }
    private void Start()
    {
        foreach (var view in Views)
        {
            view.Initialize();
            view.Close();
        }
        EventBus<OnViewGroupInitialized>.Raise(new(this));
    }
}