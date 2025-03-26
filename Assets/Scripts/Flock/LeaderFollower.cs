using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderFollower : Boid
{
    [Header("<color=##B1CC1>MoreFlockStats")]
    public float leaderMaxDistance = 3;
    [SerializeField, Range(0f, 3f)] public float arriveWeight = 3f;
    //[SerializeField, Range(0f, 3f)] private float _obstacleAvoidWeight = 3f;

    private FollowLead _followLead;
    [HideInInspector] public Transform leader;
    public GameObject shield;

    protected override void Start()
    {
        base.Start();

        EventManager.Subscribe(_team, FollowLead);

        leader = GameManager.Instance.GetTeam(_team)[0];

        Vector3 dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        AddForce(dir.normalized * _maxSpeed);

        _followLead = new FollowLead(this);
        _attack = new DoubleShot(this);
        _retreat = new ShieldRetreat(this);

        //To Attack Transitions
        _rest.AddTransition(ENEMY_SPOTTED, _attack);

        //To Follow Transitions
        _rest.AddTransition(MOVE_COMMAND, _followLead);
        _attack.AddTransition(MOVE_COMMAND, _followLead);

        //To rest Transitions
        _retreat.AddTransition(PATH_ACHIEVED, _rest);
        _followLead.AddTransition(PATH_ACHIEVED, _rest);
        _attack.AddTransition(ENEMY_LOST, _rest);

        //To retreat Transition
        _followLead.AddTransition(LOW_HEALTH, _retreat);
        _attack.AddTransition(LOW_HEALTH, _retreat);
        _rest.AddTransition(LOW_HEALTH, _retreat);

        //Spotted Transition
        _followLead.AddTransition(ENEMY_SPOTTED, _attack);
    }
    protected override void Update()
    {
        base.Update();
        _fsm.Update(Time.deltaTime);
    }
    public void FollowLead(Pf_Node n) => _fsm.SendInput(MOVE_COMMAND);
    public override void MoveAgain(Pf_Node node) 
    {
        _retreat.RefreshAbilityCD();
        _fsm.SendInput(MOVE_COMMAND);
    } 
    public bool IsLeaderInArea(float area = 1)
    {
        if (leader == null) return false;

        return (leader.position - transform.position).sqrMagnitude < (_viewRadius * _viewRadius) * area;
    }
    public override void LostTrack()
    {
        _fsm.SendInput(MOVE_COMMAND);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _separationRadius);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();

        EventManager.UnSubscribe(_team, FollowLead);
    }
}
