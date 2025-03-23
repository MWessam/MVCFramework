using System;

internal struct OnViewDataBinderAdded : IEvent
{
    internal Type Type { get; }
    internal IViewElement View { get; }
    internal OnViewDataBinderAdded(Type type, IViewElement view)
    {
        Type = type;
        View = view;
    }
}