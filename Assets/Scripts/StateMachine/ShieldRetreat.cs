using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldRetreat : Retreat
{
    // Para mostrar feedback
    LeaderFollower _follower;
    float _shieldDuration = 2f;
    bool _canActivate = true;
    public ShieldRetreat(LeaderFollower follower)
    {
        _boid = follower;
        _follower = follower;
    }
    public void SetShieldDuration(float dur) => _shieldDuration = dur;
    protected override void SpecialAbility()
    {
        if (_canActivate)
        {
            _follower.StartCoroutine(ShieldRoutine());
            _canActivate = false;
        }
    }
    protected override void OnExit()
    {
        base.OnExit();

        _follower.shield.SetActive(false);
        _follower.GetComponent<Collider>().enabled = true;
    }

    private IEnumerator ShieldRoutine()
    {
        _follower.shield.SetActive(true);
        _follower.GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(_shieldDuration);

        _follower.shield.SetActive(false);
        _follower.GetComponent<Collider>().enabled = true;
    }

    public override void RefreshAbilityCD()
    {
        _canActivate = true;
    }
}
