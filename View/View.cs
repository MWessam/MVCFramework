using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public interface IView
{
    string ViewName { get; }
    void Render(object viewModel);
    void Render();
    void Close();
    void Initialize();
    void RenderPartial(string partialViewName, object viewModel = null);
}

/// <summary>
/// This should be the current open menu for example.
/// It will act as some sort of controller controller.
/// Where it will get the data it needs from multiple controllers and interact with them all inside this controller that
/// will feed the corresponding views.
/// </summary>
public abstract class View<T> : MonoBehaviour, IView
{
    protected T ViewModel;
    [Tooltip("Leave empty if you want the view name to be same as class without a view suffix if found")]
    [SerializeField] private string _viewName;
    private Dictionary<string, IView> _partialViewsMap;
    private List<IView> _openPartialViews = new();
    protected bool IsInitialized;
    public string ViewName
    {
        get
        {
            if (!string.IsNullOrEmpty(_viewName)) return _viewName;
            
            var viewTypeName = GetType().Name;
            if (viewTypeName.EndsWith("View"))
            {
                _viewName = viewTypeName.Substring(0, viewTypeName.Length - "View".Length);
            }
            else
            {
                _viewName = viewTypeName;
            }
            return _viewName;
        }
    }
    public void Render(object viewModel)
    {
        OnTransitionIn();
        if (viewModel == null)
        {
            Render();
        }
        else if (viewModel is T castedViewModel)
        {
            Bind(castedViewModel);
            OnRender();
        }
        else
        {
            Debug.LogError("View model is of wrong type. Couldn't route to specified view.");
            OnRender();
        }
    }
    public void Render()
    {
        OnTransitionIn();
        OnRenderStatic();
    }
    public void RenderPartial(string partialViewName, object viewModel = null)
    {
        if (_partialViewsMap.TryGetValue(partialViewName, out var view))
        {
            view.Render(viewModel);
            _openPartialViews.Add(view);
        }
        else
        {
            Debug.LogError($"Couldn't find Partial View: {partialViewName} in View: {ViewName}");
        }
    }
    public void Close()
    {
        OnTransitionOut();
        if (ViewModel != null)
        {
            OnUnbind(ViewModel);
        }

        foreach (var partialView in _openPartialViews)
        {
            partialView.Close();
        }
        _openPartialViews.Clear();
        
        if (!IsInitialized)
        {
            IsInitialized = true;
        }
    }
    private void Bind(T viewModel)
    {
        ViewModel = viewModel;
        OnBind(viewModel);
    }
    public void Initialize()
    {
        OnInitialize();
    }

    private void Awake()
    {
        var partialViews = GetComponentsInChildren<IView>();
        _partialViewsMap = partialViews.ToDictionary(x => x.ViewName, x => x);
    }

    protected abstract void OnInitialize();
    protected virtual void OnRender() {}
    protected virtual void OnRenderStatic() {}
    protected virtual void OnTransitionIn() {}
    protected virtual void OnTransitionOut() {}
    protected abstract void OnBind(T viewModel);
    protected abstract void OnUnbind(T viewModel);
    protected void ButtonAction(string action, string controller = "")
    {
        EventBus<OnButtonAction>.Raise(new OnButtonAction(action, controller));
    }
}

internal struct OnButtonAction : IEvent
{
    internal string Action { get; private set; }
    internal string Controller { get; private set; }
    internal OnButtonAction(string action, string controller)
    {
        Action = action;
        Controller = controller;
    }
}

internal struct OnViewGroupInitialized : IEvent
{
    internal ViewGroup ViewGroup { get; private set; }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="viewGroup"></param>
    internal OnViewGroupInitialized(ViewGroup viewGroup)
    {
        ViewGroup = viewGroup;
    }

}

public abstract class UIToolkitView<T> : View<T>
{
    [SerializeField] protected UIDocument UiDocument;
    [SerializeField] private string _viewContainerName;
    protected VisualElement RootElement;
    protected VisualElement ViewContainer;

    private void Awake()
    {
        RootElement = UiDocument.rootVisualElement;
        ViewContainer = RootElement.Q(_viewContainerName);
        if (ViewContainer == null)
        {
            Debug.LogError($"View Container of name {_viewContainerName} does not exist...");
        }

    }

    protected override void OnTransitionOut()
    {
        ViewContainer.AddToClassList("main-panel--disabled");
        if (IsInitialized)
        {
            ViewContainer.schedule
                .Execute(() => ViewContainer.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None))
                .StartingIn(1000);
        }
    }

    protected override void OnTransitionIn()
    {
        ViewContainer.RemoveFromClassList("main-panel--disabled");
        ViewContainer.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);

    }
}

public abstract class UIToolkitStaticView : UIToolkitView<object>
{
    protected override void OnBind(object viewModel)
    {
        
    }

    protected override void OnUnbind(object viewModel)
    {
        
    }
}

public abstract class UguiView<T> : View<T>
{
    [SerializeField] protected GameObject Root;
    protected override void OnTransitionOut()
    {
        Root.SetActive(false);
    }

    protected override void OnTransitionIn()
    {
        Root.SetActive(true);
    }
}