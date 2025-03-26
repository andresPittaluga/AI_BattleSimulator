using System.Collections.Generic;
using UnityEngine;
public abstract class State
{
    private Dictionary<string, State> _transitions = new Dictionary<string, State>();

    public void AddTransition(string _input, State _state)
    {
        if (!_transitions.ContainsKey(_input))
        {
            _transitions.Add(_input, _state);
        }
        else Debug.LogWarning("<color=yellow>Ya tengo ese estado!</color>");
    }
    public bool ContainsInput(string input)
    {
        return _transitions.ContainsKey(input);
    }
    public State GetState(string input)
    {
        return _transitions[input];
    }
    public void Enter()
    {
        OnEnter();
    }
    public void Exit()
    {
        OnExit();
    }
    public void Update(float deltaTime)
    {
        OnUpdate(deltaTime);
    }
    protected abstract void OnEnter();
    protected abstract void OnExit();
    protected abstract void OnUpdate(float deltaTime);

    /*protected Action<string> _InputSender;
    public State GetTransitioner(Action<string> transitioner)
    {
        _InputSender = transitioner;
        return this;
    }*/
} 
           