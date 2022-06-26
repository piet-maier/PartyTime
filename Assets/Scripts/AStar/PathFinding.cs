using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public class PathFinding : MonoBehaviour
    {
        public Transform start, goal;

        private Grid _grid;

        public void Awake()
        {
            _grid = GetComponent<Grid>();
        }

        public void Update()
        {
            FindPath(start.position, goal.position);
        }

        // A* Implementation
        private void FindPath(Vector3 worldStart, Vector3 worldGoal)
        {
            var startNode = _grid.WorldToNode(worldStart);
            var goalNode = _grid.WorldToNode(worldGoal);

            var open = new List<Node>();
            var closed = new HashSet<Node>();

            open.Add(startNode);

            while (open.Count != 0)
            {
                var current = open[0];

                for (var i = 1; i < open.Count; i++)
                {
                    if (open[i].FCost < current.FCost) current = open[i];
                    else if (open[i].FCost == current.FCost && open[i].HCost < current.HCost) current = open[i];
                }

                open.Remove(current);
                closed.Add(current);

                if (current == goalNode)
                {
                    Retrace(startNode, goalNode);
                    return;
                }

                foreach (var neighbour in _grid.GetNeighbours(current))
                {
                    if (neighbour.IsObstacle || closed.Contains(neighbour)) continue;
                    var newCost = current.GCost + Distance(current, neighbour);

                    if (newCost >= current.GCost && open.Contains(neighbour)) continue;
                    neighbour.GCost = newCost;
                    neighbour.HCost = Distance(neighbour, goalNode);
                    neighbour.Previous = current;

                    if (!open.Contains(neighbour)) open.Add(neighbour);
                }
            }
        }

        private static int Distance(Node a, Node b)
        {
            var distanceX = Mathf.Abs(a.X - b.X);
            var distanceY = Mathf.Abs(a.Y - b.Y);
            if (distanceX < distanceY) return 14 * distanceX + 10 * (distanceY - distanceX);
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }

        private void Retrace(Node startNode, Node goalNode)
        {
            var path = new List<Node>();
            var current = goalNode;

            while (current != startNode)
            {
                path.Add(current);
                current = current.Previous;
            }

            path.Reverse();

            _grid.Path = path;
        }
    }
}