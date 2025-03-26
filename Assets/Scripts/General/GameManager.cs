using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("<color=#12CA11>References")]
    [SerializeField] private List<Transform> _t1BoidsList;
    [SerializeField] private List<Transform> _t2BoidsList;

    //public void SetNodeList(List<Pf_Node> list) => _nodeList = list;
    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);

        Time.timeScale = 1;
    }
    public void Eliminate(TypeEvent team, Transform transform)
    {
        switch (team)
        {
            case TypeEvent.Team1: 
                if (_t1BoidsList.Contains(transform)) 
                    _t1BoidsList.Remove(transform);
                break;
            case TypeEvent.Team2:
                if (_t2BoidsList.Contains(transform))
                    _t2BoidsList.Remove(transform);
                break;
            default: Debug.LogError("Ask for a valid team!"); break;
        }
    }
    public List<Transform> GetTeam(TypeEvent team)
    {
        switch (team)
        {
            case TypeEvent.Team1: return _t1BoidsList;
            case TypeEvent.Team2: return _t2BoidsList;
            default: Debug.LogError("Ask for a valid team!"); return null;
        }
    }

    public List<Transform> GetEnemyTeam(TypeEvent team)
    {
        switch (team)
        {
            case TypeEvent.Team1: return _t2BoidsList;
            case TypeEvent.Team2: return _t1BoidsList;
            default: Debug.LogError("Ask for a valid team!"); return null;
        }
    }
}