using UnityEngine;
using System.Collections.Generic;

public class PF_Grid : MonoBehaviour
{
    private GameObject[,] _grid = default;
    [Header("<color=black>Grid References")]
    [SerializeField] private int _width = 0, _height = 0;
    [SerializeField] private float _offset = 0;
    [SerializeField] private GameObject _prefab = default;

    private List<Pf_Node> _nodeList = new();
    private void Start()
    {
        _grid = new GameObject[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                GameObject prefab = Instantiate(_prefab, transform);
                _grid[x, y] = prefab;

                Pf_Node currNode = prefab.GetComponent<Pf_Node>();

                currNode.Initialize(x, y, new Vector3((x * prefab.transform.localScale.x) * _offset, 0, (y * prefab.transform.localScale.z) * _offset), this);
                _nodeList.Add(currNode);
            }
        }
    }
    public Pf_Node GetNode(int x, int y)
    {
        if (x < 0 || x > _width - 1 || y < 0 || y > _height - 1) return null;

        return _grid[x, y].GetComponent<Pf_Node>();
    }
}