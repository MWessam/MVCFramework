using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

internal interface IGlobalStateManager
{
    internal void UpdateGlobalObject<T>(T obj);
    internal void Initialize();
}
// Api -> DTO -> Entity
// Api <- DTO <- Entity

// We will need dto to entity mappers and vice versa.
// Singleton data manager
// For example, let's say you have user data.
// Any ui element that is hooked to that type, will automatically update and get their corresponding entity that it requires.
// So player profile will update with the players data.
// And if there is any other views that rely on it that is currently inactive, it will store it untill it transitions in.
// You can pass the data into state manager and it will automatically use reflection to get the type

internal class GlobalStateManager : IGlobalStateManager
{
    private Dictionary<Type, List<IViewElement>> _modelToViewMap = new(16);
    private Dictionary<Type, object> _globalObjects = new();

    void IGlobalStateManager.UpdateGlobalObject<T>(T obj)
    {
        var type = typeof(T);
        _globalObjects.TryAdd(type, obj);
        RefreshGlobalObject(obj, type);
    }
    private void RefreshGlobalObject(object obj, Type type)
    {
        if (_modelToViewMap.TryGetValue(type, out var views))
        {
            foreach (var view in views)
            {
                view.OnGlobalObjectStateUpdated(obj);
            }
        }
    }
    private void RefreshGlobalObject<T>(T obj)
    {
        if (_modelToViewMap.TryGetValue(typeof(T), out var views))
        {
            foreach (var view in views)
            {
                view.OnGlobalObjectStateUpdated(obj);
            }
        }
    }

    void IGlobalStateManager.Initialize()
    {
        EventBus<OnViewDataBinderAdded>.Register(OnViewDataBinderAdded);
    }
    private void OnViewDataBinderAdded(OnViewDataBinderAdded evt)
    {
        if (_modelToViewMap.TryGetValue(evt.Type, out var views))
        {
            views.Add(evt.View);
        }
        else
        {
            _modelToViewMap.Add(evt.Type, new() {evt.View});
        }
    }
}

// Example application:
// User, Items
// Profile Menu:
// Have user data populated, and some favourite items.