using UnityEngine;

public class FSM
{
    private State _current;

    public FSM(State initialState)
    {
        _current = initialState;
        _current.Enter();
    }
    public void SendInput(string input)
    {
        if (_current == null) 
        {
            Debug.LogError("No configuraste la finite state Machine...");
            return;
        }

        if (_current.ContainsInput(input)) 
        {
            State next = _current.GetState(input);

            if (next == null) Debug.LogError("No hay estado, pero si input");

            _current.Exit();
            _current = next;
            _current.Enter();
        }
    }

    public void Update(float deltaTime)
    {
        if(_current == null) return; 
        
        _current.Update(deltaTime);
    }

    public State GetState() => _current;
}
