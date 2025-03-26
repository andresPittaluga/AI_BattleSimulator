using System.Collections;
using UnityEngine;

public class Pf_NodeGetter : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        Pf_Node[] nodeArray = GetComponentsInChildren<Pf_Node>();

        //GameManager.Instance.SetNodeList(nodeArray.ToList());
    }
}
