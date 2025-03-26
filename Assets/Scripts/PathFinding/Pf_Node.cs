using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pf_Node : MonoBehaviour
{
    private List<Pf_Node> _neighbors = new();
    private PF_Grid _grid = default;
    private int _x = 0, _y = 0;
    [Header("<color=yellow>Values")]
    public float cost = 0;
    public bool isBlocked = false;

    [Header("<color=orange>Grid Options")]
    [SerializeField] private float _obsCheckRadius;
    [SerializeField] private LayerMask _obstacleMask;

    [Header("<color=red>HandPlaced Options")]
    public bool handPlaced = false;
    [SerializeField] private float _neighbourDistance = 3;
    [SerializeField] private LayerMask _nodeMask;

    [Header("<color=red>VisualFeedback")]
    [SerializeField] Material _t1Mat;
    [SerializeField] Material _t2Mat;
    private void Start()
    {
        if (!handPlaced)
        {
            if (Physics.OverlapSphere(transform.position, _obsCheckRadius, _obstacleMask).Length > 0)
                isBlocked = true;
        }
        else
            AddHandPutNeighbors();
    }
    public List<Pf_Node> GetNeighbors()
    {
        if (_neighbors.Count != 0) return _neighbors;

        if (!handPlaced)
        {
            Pf_Node neighbor = _grid.GetNode(_x, _y - 1); //Up
            if (neighbor) _neighbors.Add(neighbor);
            neighbor = _grid.GetNode(_x + 1, _y); //Right
            if (neighbor) _neighbors.Add(neighbor);
            neighbor = _grid.GetNode(_x, _y + 1); //Down
            if (neighbor) _neighbors.Add(neighbor);
            neighbor = _grid.GetNode(_x - 1, _y); //Left
            if (neighbor) _neighbors.Add(neighbor);
        }


        return _neighbors;
    }
    private void AddHandPutNeighbors(float multiplier = 1)
    {
        Collider[] nodes = Physics.OverlapSphere(transform.position, _neighbourDistance * multiplier, _nodeMask);

        foreach (Collider node in nodes)
        {
            Pf_Node actualNode = node.GetComponent<Pf_Node>();

            if (actualNode == null || actualNode == this) continue;

            if (Observe.LineOfSight(transform.position, actualNode.transform.position, _obstacleMask))
                _neighbors.Add(actualNode);
        }
    }

    public void Initialize(int x, int y, Vector3 pos, PF_Grid grid)
    {
        _x = x;
        _y = y;
        transform.position = pos;
        _grid = grid;
        cost = 0; //Random.Range(1, 11);
    }
    private void OnDrawGizmosSelected()
    {
        if (_neighbors != null)
        {
            foreach (var node in _neighbors)
            {
                Gizmos.DrawLine(transform.position, node.transform.position);
            }
        }
    }
    public void AddExternalNeighbour(Pf_Node node) => _neighbors.Add(node);
    public void DeleteExternalNeighbour(Pf_Node node) => _neighbors.Remove(node);
    public bool CheckForNeighbours()
    {
        AddHandPutNeighbors(2);

        if (_neighbors.Count > 0)
        {
            foreach (var node in _neighbors)
                node.AddExternalNeighbour(this);
        }

        return _neighbors.Count > 0;
    }
    public void HasBeenSummoned(TypeEvent type)
    {
        if (type == TypeEvent.Team1)
        {
            GetComponent<MeshRenderer>().material = _t1Mat;

            Pf_NodeManager.Instance.SelectedNode(this, true);
        }
        else if (type == TypeEvent.Team2)
        {
            GetComponent<MeshRenderer>().material = _t2Mat;

            Pf_NodeManager.Instance.SelectedNode(this, false);
        }
    }

    public void HasBeenDestroyed()
    {
        foreach (var node in _neighbors)
            node.DeleteExternalNeighbour(this);

        Destroy(gameObject);
    }
}