using UnityEngine;
using System.Collections.Generic;
using System;
public class ObjectPool <T>
{
    private Func<T> _Factory;
    private Action<T> _TurnOn, _TurnOff;
    public List<T> stockAvailable { get; private set; }

    public List<T> readableStock { get; private set; }
    public ObjectPool(Func<T> Factory, Action<T> TurnOn, Action<T> TurnOff, int initialStock = 5)
    {
        stockAvailable = new();

        _Factory = Factory;
        _TurnOff = TurnOff;
        _TurnOn = TurnOn;

        for (int i = 0; i < initialStock; i++)
        {
            var x = _Factory();

            _TurnOff(x);
            stockAvailable.Add(x);
        }
    }
    public T Get()
    {
        if (stockAvailable.Count > 0)
        {
            var x = stockAvailable[0];
            stockAvailable.Remove(x);
            _TurnOn(x);
            return x;
        }

        return _Factory();
    }

    public void Return(T value)
    {
        _TurnOff(value);
        stockAvailable.Add(value);
    }

}
