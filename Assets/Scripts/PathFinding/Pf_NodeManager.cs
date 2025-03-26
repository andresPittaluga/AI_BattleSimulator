using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pf_NodeManager : MonoBehaviour
{
    public static Pf_NodeManager Instance;

    [SerializeField] private LayerMask _nodeMask;
    [SerializeField] private float _initSeparation = 6;
    private List<Pf_Node> _nodeList;
    public List<Pf_Node> NodeList { get => _nodeList; }

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);
    }
    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        Pf_Node[] nodeArray = GetComponentsInChildren<Pf_Node>();
        _nodeList = nodeArray.ToList();
    }
    public Pf_Node GetClosestNode(Vector3 position)
    {
        Collider[] closeNodes = Physics.OverlapSphere(position, _initSeparation, _nodeMask);

        return closeNodes[0].GetComponent<Pf_Node>();
    }
    public Pf_Node GetExactClosestNode(Vector3 position)
    {
        Pf_Node closestNode = null;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < _nodeList.Count; i++)
        {
            if (_nodeList[i] == null) continue;

            float distance = (_nodeList[i].transform.position - position).sqrMagnitude; 

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestNode = _nodeList[i];
            }
        }
        return closestNode;
    }
    public Pf_Node GetExactFarthestNode(Vector3 position)
    {
        Pf_Node farthestNode = null;
        float farthestDistance = -Mathf.Infinity;

        for (int i = 0; i < _nodeList.Count; i++)
        {
            if (_nodeList[i] == null) continue;

            float distance = (_nodeList[i].transform.position - position).sqrMagnitude;

            if (distance > farthestDistance)
            {
                farthestDistance = distance;
                farthestNode = _nodeList[i];
            }
        }
        return farthestNode;
    }

    public void SelectedNode(Pf_Node selected, bool T1)
    {
        if (T1)
            EventManager.Trigger(TypeEvent.Team1, selected);
        else
            EventManager.Trigger(TypeEvent.Team2, selected);
    }
}
