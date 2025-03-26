using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeededRetreat : Retreat
{
    // Para mostrar feedback
    Leader _leader;
    float _sprintDuration = 2f, _sprintSpeed = 10;

    float _ogSpeed;

    public SpeededRetreat(Leader lead)
    {
        _boid = lead;
        _leader = lead;

        _ogSpeed = _leader.GetSpeed();
    }
    public void SetSprintDuration(float dur) => _sprintDuration = dur;
    public void SetSprintSpeed(float s) => _sprintSpeed = s;
    protected override void SpecialAbility()
    {
        _leader.StartCoroutine(SprintRoutine());
    }
    protected override void OnExit()
    {
        base.OnExit();

        _leader.SetSpeed(_ogSpeed);
        _leader.speedTrail.SetActive(false);
    }
    private IEnumerator SprintRoutine()
    {
        _leader.speedTrail.SetActive(true);
        _leader.SetSpeed(_sprintSpeed);

        yield return new WaitForSeconds(_sprintDuration);

        _leader.SetSpeed(_ogSpeed);
        _leader.speedTrail.SetActive(false);
    }
}
