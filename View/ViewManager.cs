using System.Collections.Generic;
using UnityEngine;

public interface IViewManager
{
    bool TransitionIn(IView view);
    IView Return();
    IView CurrentView { get; }
}

internal class ViewManager : MonoBehaviour, IViewManager
{
    
    private List<IView> _viewStack = new();
    private IView _currentView;
    public IView CurrentView => _currentView;
    public bool TransitionIn(IView view)
    {
        if (_currentView == view) return true;
        
        if (_viewStack.Contains(view))
        {
            // I want to ensure that if the view is already in the stack, that it can be popped from the stack and return the top of the stack.
            // This is to ensure that the view is not added to the stack multiple times.
            _viewStack.Remove(view);
        }
        if (_currentView != null)
        {
            var previousView = _currentView;
            previousView.Close();
            _viewStack.Add(previousView);
        }
        _currentView = view;
        
        return true;
    }

    public IView Return()
    {
        if (_currentView == null) return null;
        if (_viewStack.Count == 0) return null;
        
        var previousView = _currentView;
        previousView.Close();
        _currentView = _viewStack[^1];
        _viewStack.RemoveAt(_viewStack.Count - 1);
        return _currentView;
    }
}
