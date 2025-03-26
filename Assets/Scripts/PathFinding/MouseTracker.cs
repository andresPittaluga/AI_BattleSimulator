using UnityEngine;
using System.Collections;

public class MouseTracker : MonoBehaviour
{
    [Header("<color=#BB41C1>References")]
    [SerializeField] GameObject _nodePrefab;
    [SerializeField] LayerMask _canCreateNodes;

    Pf_Node _t1Node, _t2Node;
    Camera _mainCam;
    bool canPlaceT1 = true, canPlaceT2 = true;
    private void Start()
    {
        _mainCam = Camera.main;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (canPlaceT1)
                CreateNodeAtMousePosition(TypeEvent.Team1);
            else
                Debug.Log("On Cooldown!");
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if(canPlaceT2)
                CreateNodeAtMousePosition(TypeEvent.Team2);
            else
                Debug.Log("On Cooldown!");
        }
    }
    private void CreateNodeAtMousePosition(TypeEvent team)
    {
        Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if ((_canCreateNodes.value & (1 << hit.collider.gameObject.layer)) == 0)
                return;

            Vector3 spawnPosition = new Vector3(hit.point.x, 0, hit.point.z);

            GameObject nodeGO = Instantiate(_nodePrefab, spawnPosition, Quaternion.identity);

            Pf_Node nodeScript = nodeGO.GetComponent<Pf_Node>();

            if (nodeScript.CheckForNeighbours())
            {
                if (team == TypeEvent.Team1)
                {
                    if (_t1Node != null)
                        _t1Node.HasBeenDestroyed();

                    _t1Node = nodeScript;
                    _t1Node.HasBeenSummoned(team);

                    canPlaceT1 = false;
                    StartCoroutine(CDRoutine(team));
                }
                else
                {
                    if (_t2Node != null)
                        _t2Node.HasBeenDestroyed();

                    _t2Node = nodeScript;
                    _t2Node.HasBeenSummoned(team);

                    canPlaceT2 = false;
                    StartCoroutine(CDRoutine(team));
                }
            }
            else
            {
                Destroy(nodeGO);
                Debug.Log("Sorry, there is no neighbour in sight!");
            }
        }
    }

    private IEnumerator CDRoutine(TypeEvent team)
    {
        yield return new WaitForSeconds(0.25f);

        if(team == TypeEvent.Team1)
            canPlaceT1 = true;
        else if (team == TypeEvent.Team2)
            canPlaceT2 = true;
    }
}