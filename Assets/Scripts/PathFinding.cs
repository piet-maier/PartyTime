using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public List<AStar.Node> path;

    private AStar.Grid _grid;
    private Transform _player;

    public void Start()
    {
        _grid = GameObject.Find("A*").GetComponent<AStar.Grid>();
        _player = GameObject.Find("Player").GetComponent<Transform>();
    }

    public void Update()
    {
        path = AStar.AStar.FindPath(_grid, gameObject.GetComponentInChildren<Transform>().transform.position, _player.position);
    }

    public void OnDrawGizmos()
    {
        if (path == null) return;
        Gizmos.color = Color.gray;
        foreach (var node in path)
        {
            Gizmos.DrawCube(node.worldPosition, Vector3.one * _grid.nodeRadius);
        }
    }
}