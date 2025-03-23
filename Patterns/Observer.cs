using System;

public class Observer<T>
{
    public event Action<T> OnValueChanged; 
    private T _value;

    public T Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            OnValueChanged?.Invoke(_value);
        }
    }
    public static implicit operator T(Observer<T> observer) => observer.Value;
}