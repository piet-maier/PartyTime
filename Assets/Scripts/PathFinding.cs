using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    private AStar.Grid _grid;
    private Transform _player;
    private List<AStar.Node> _path;

    public void Start()
    {
        _grid = GameObject.Find("A*").GetComponent<AStar.Grid>();
        _player = GameObject.Find("Player").GetComponent<Transform>();
    }

    public void Update()
    {
        _path = AStar.AStar.FindPath(_grid, transform.position, _player.position);
    }

    public void OnDrawGizmos()
    {
        if (_path == null) return;
        Gizmos.color = Color.gray;
        foreach (var node in _path)
        {
            Gizmos.DrawCube(node.worldPosition, Vector3.one * _grid.nodeRadius);
        }
    }
}