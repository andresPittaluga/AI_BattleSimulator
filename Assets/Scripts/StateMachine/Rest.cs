using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rest : State
{
    Boid _boid;
    Coroutine _healthRoutine;
    protected override void OnEnter()
    {
        _boid.StopMovement();

        if(_healthRoutine == null)
            _healthRoutine = _boid.StartCoroutine(HealRoutine());
    }

    protected override void OnExit()
    {
        if (_healthRoutine != null)
        {
            _boid.StopCoroutine(_healthRoutine);
            _healthRoutine = null;
        }
    }

    protected override void OnUpdate(float deltaTime)
    {
        
    }
    
    public Rest(Boid lead)
    {
        _boid = lead;
    }

    private IEnumerator HealRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_boid.healTime);
            _boid.Heal(5);
        }
    }
}
