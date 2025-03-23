using System;
using System.Collections.Generic;
using System.Reflection;
using Game_Mangement.Scripts.UI.MVC;
using UnityEngine;
// using VContainer;
    
public abstract class Controller : IDisposable
{
    [Tooltip("Leave empty if you want the controller name to be same as class without a controller suffix if found")]
    private string _controllerName;
    private Dictionary<string, IView> _routes = new();
    private Dictionary<string, Action> _actions = new();
    private IControllerManager _controllerManager;
    private IViewManager _viewManager;

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
    public Controller(string controllerName = "")
    {
        _viewManager = MVCReferenceHolder.Instance.ViewManager;
        _controllerManager = MVCReferenceHolder.Instance.ControllerManager;
        _controllerName = controllerName;
        _controllerManager.AddController(this);
    }

    public void View(string route, object viewModel = null)
    {
        if (!_routes.TryGetValue(route, out var view)) return;
        
        if (_viewManager.TransitionIn(view))
        {
            view.Render(viewModel);
        }
    }

    public void PartialView(string partialView, object viewModel = null)
    {
        if (_viewManager.CurrentView == null)
        {
            Debug.LogError("There is no active view. Couldn't render partial view.");
            return;
        }
        _viewManager.CurrentView.RenderPartial(partialView, viewModel);
    }

    /// <summary>
    /// Dynamically invokes a parameterless method with the same name as the action
    /// </summary>
    /// <param name="action"></param>
    /// <param name="controllerName"></param>
    public void RedirectToAction(string action, string controllerName = "")
    {
        if (controllerName == "" || controllerName == ControllerName)
        {
            var method = GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (method == null)
            {
                Debug.LogError($"Action '{action}' not found in controller '{GetType().Name}'.");
            }
            else
            {
                // Must be parameterless.
                method.Invoke(this, null);
            }
        }
        else
        {
            var controller = _controllerManager.GetController(controllerName);
            if (controller != null)
            {
                controller.RedirectToAction(action);
            }
        }
    }

    public void Back()
    {
        var backView = _viewManager.Return();
        backView?.Render();
    }
    public void AddRoute(string route, IView view)
    {
        _routes[route] = view;
    }
    public virtual void Initialize()
    {
        
    }

    public virtual void Dispose()
    {
        _controllerManager.RemoveController(this);
    }

}