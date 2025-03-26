using System.Collections.Generic;
using UnityEngine;

public class Leader : Boid
{
    [Header("<color=##B1CC1>MoreRefs")]
    public GameObject speedTrail;

    private LeadTheWay _leadTheWay;
    Pf_Node lastNode;
    protected override void Start()
    {
        base.Start();

        EventManager.Subscribe(_team, MoveCommand);

        _leadTheWay = new LeadTheWay(this);
        _attack = new TriAttack(this);
        _retreat = new SpeededRetreat(this);

        //To lead Transitions
        _rest.AddTransition(MOVE_COMMAND, _leadTheWay);
        _attack.AddTransition(MOVE_COMMAND, _leadTheWay);

        //To Rest Transitions
        _leadTheWay.AddTransition(PATH_ACHIEVED, _rest);
        _retreat.AddTransition(PATH_ACHIEVED, _rest);
        _attack.AddTransition(ENEMY_LOST, _rest);

        //To Retreat Transitions
        _leadTheWay.AddTransition(LOW_HEALTH, _retreat);
        _attack.AddTransition(LOW_HEALTH, _retreat);
        _rest.AddTransition(LOW_HEALTH, _retreat);

        //Spotted Transition
        _leadTheWay.AddTransition(ENEMY_SPOTTED, _attack);
        _rest.AddTransition(ENEMY_SPOTTED, _attack);

    }

    protected override void Update()
    {
        base.Update();

        _fsm.Update(Time.deltaTime);
    }
    public override void MoveAgain(Pf_Node node) 
    { 
        if(lastNode != null)
            MoveCommand(lastNode); 
    }
    public void MoveCommand(Pf_Node node)
    {
        if (_fsm.GetState() == _attack)
            _attack.Stop();

        lastNode = node;

        _fsm.SendInput(MOVE_COMMAND);
        _leadTheWay.ConstructPath(node);
    }
    protected override void Die()
    {
        Time.timeScale = 0;
        base.Die();
        CanvasManager.Instance.TeamWin(_team);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();

        EventManager.UnSubscribe(_team, MoveCommand);
    }

    public override void LostTrack() => _fsm.SendInput(MOVE_COMMAND);
}
