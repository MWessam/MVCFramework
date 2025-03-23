using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class GlobalStateBindAttribute : Attribute
{
}

internal class ViewElementRegistrationService
{
    private IViewElement _viewElement;
    private Dictionary<Type, Delegate> _actionMap = new();

    internal ViewElementRegistrationService(IViewElement viewElement)
    {
        _viewElement = viewElement;
    }

    internal void OnGlobalObjectStateUpdated(object obj)
    {
        if (_actionMap.TryGetValue(obj.GetType(), out var bindAction))
        {
            try
            {
                bindAction.DynamicInvoke(obj);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to bind object of type {obj.GetType()} to view element {_viewElement.GetType()}: {ex.Message}");
            }
        }
    }

    private void AddSingletonDataBinder(Type type, Delegate binder)
    {
        var parameters = binder.Method.GetParameters();
        if (parameters.Length != 1)
        {
            Debug.LogError($"Signature mismatch for singleton data binder {binder.Method.Name}. Make sure it ONLY contains one argument, and that it's the same as the passed in type.");
            return;
        }
        if (!_actionMap.TryAdd(type, binder))
        {
            _actionMap[type] = binder;
        }
        else
        {
            EventBus<OnViewDataBinderAdded>.Raise(new (type, _viewElement));
        }
    }

    internal void RegisterGlobalStateBinders()
    {
        var instanceType = _viewElement.GetType();
        var methods = instanceType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (var method in methods)
        {
            var attribute = method.GetCustomAttribute<GlobalStateBindAttribute>();
            if (attribute == null) continue;
            
            var parameters = method.GetParameters();
                
            // Validate method signature
            if (parameters.Length != 1)
            {
                Debug.LogError($"Method {method.Name} in {_viewElement.GetType()} has an invalid signature for DataBinder. " +
                               $"Expected only one parameter, but got {parameters.Length}.");
                continue;
            }
            
            // Create and register the delegate
            var binderDelegate = Delegate.CreateDelegate(
                typeof(Action<>).MakeGenericType(parameters[0].ParameterType),
                _viewElement,
                method
            );
            AddSingletonDataBinder(parameters[0].ParameterType, binderDelegate);

        }
    }
}

public abstract class UIToolkitViewElement : VisualElement, IViewElement
{
    private ViewElementRegistrationService _viewElementRegistration;

    public UIToolkitViewElement()
    {
    }
    public void Initialize()
    {
        _viewElementRegistration = new ViewElementRegistrationService(this);
        _viewElementRegistration.RegisterGlobalStateBinders();
        OnInitialize();
    }
    public void OnGlobalObjectStateUpdated(object obj)
    {
        _viewElementRegistration.OnGlobalObjectStateUpdated(obj);
    }

    protected virtual void OnInitialize()
    {
        
    }
    protected void ButtonAction(string action, string controller = "")
    {
        EventBus<OnButtonAction>.Raise(new OnButtonAction(action, controller));
    }
}
public interface IViewElement
{
    void OnGlobalObjectStateUpdated(object obj);
}