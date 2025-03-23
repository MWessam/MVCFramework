using UnityEngine;

namespace Game_Mangement.Scripts.UI.MVC
{
    [DefaultExecutionOrder(-1000)]
    public class MVCReferenceHolder : Singleton<MVCReferenceHolder>
    {
        public IControllerManager ControllerManager { get; protected set; }
        public IViewManager ViewManager { get; protected set; }
        [SerializeField] private ControllerManager _controllerManager;
        [SerializeField] private ViewManager _viewManager;
        protected override void OnInitialize()
        {
            base.OnInitialize();
            ControllerManager = _controllerManager;
            ViewManager = _viewManager;
        }
    }
}