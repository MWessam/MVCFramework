using System;
using System.Collections.Generic;
using UnityEngine;

public interface IControllerManager
{
    Controller GetController(string controllerName);
    void AddController(Controller controller);
    void RemoveController(Controller controller);
}
public class ControllerManager : MonoBehaviour, IControllerManager
{
    private Dictionary<string, Controller> _controllerMap = new();

    public Controller GetController(string controllerName)
    {
        return _controllerMap.GetValueOrDefault(controllerName);
    }

    public void AddController(Controller controller)
    {
        _controllerMap[controller.ControllerName] = controller;
    }

    public void RemoveController(Controller controller)
    {
        if (_controllerMap.Remove(controller.ControllerName))
        {
            
        }
    }
    private void OnEnable()
    {
        EventBus<OnViewGroupInitialized>.Register(OnViewInitialized);
        EventBus<OnButtonAction>.Register(OnViewButtonActionInput);
    }

    private void OnViewButtonActionInput(OnButtonAction action)
    {
        if (_controllerMap.TryGetValue(action.Controller, out var controller))
        {
            controller.RedirectToAction(action.Action, action.Controller);
        }
        else
        {
            Debug.LogError($"Controller '{action.Controller}' not found.");
        }
    }

    private void OnDisable()
    {
        EventBus<OnViewGroupInitialized>.Deregister(OnViewInitialized);
    }
    private void OnViewInitialized(OnViewGroupInitialized obj)
    {
        if (obj.ViewGroup.ControllerName.Contains("Controller"))
        {
            if (_controllerMap.TryGetValue(obj.ViewGroup.ControllerName, out var controllerSuffix))
            {
                foreach (var view in obj.ViewGroup.Views)
                {
                    controllerSuffix.AddRoute(view.ViewName, view);
                }
            }
            else if (_controllerMap.TryGetValue(obj.ViewGroup.ControllerName.Replace("Controller", ""),
                         out var controllerNoSuffix))
            {
                foreach (var view in obj.ViewGroup.Views)
                {
                    controllerNoSuffix.AddRoute(view.ViewName, view);
                }
            }
        }
        else
        {
            if (_controllerMap.TryGetValue(obj.ViewGroup.ControllerName, out var controller))
            {
                foreach (var view in obj.ViewGroup.Views)
                {
                    controller.AddRoute(view.ViewName, view);
                }
            }
        }
    }
}