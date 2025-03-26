using System.Collections.Generic;
using UnityEngine;

public delegate void EmptyEvents(Pf_Node position);

public class EventManager
{
    private static Dictionary<TypeEvent, EmptyEvents> _currentEvents = new();

    public static void Subscribe(TypeEvent type, EmptyEvents method)
    {
        if (_currentEvents.ContainsKey(type))
            _currentEvents[type] += method;
        else
            _currentEvents.Add(type, method);
    }

    public static void UnSubscribe(TypeEvent type, EmptyEvents method)
    {
        if (_currentEvents.ContainsKey(type))
        {
            _currentEvents[type] -= method;

            if (_currentEvents[type] == null)
                _currentEvents.Remove(type);
        }
    }

    public static void Trigger(TypeEvent type, Pf_Node position)
    {
        if (_currentEvents.ContainsKey(type))
            _currentEvents[type]?.Invoke(position);
    }
}
public enum TypeEvent
{
    PlayerFound,
    Team1,
    Team2
}
