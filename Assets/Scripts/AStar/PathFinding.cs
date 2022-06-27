using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public static class PathFinding
    {
        // A* Implementation
        public static void FindPath(Grid grid, Vector3 worldStart, Vector3 worldGoal)
        {
            var startNode = grid.WorldToNode(worldStart);
            var goalNode = grid.WorldToNode(worldGoal);

            var open = new List<Node>();
            var closed = new HashSet<Node>();

            open.Add(startNode);

            while (open.Count != 0)
            {
                var current = open[0];

                for (var i = 1; i < open.Count; i++)
                {
                    if (open[i].FCost < current.FCost) current = open[i];
                    else if (open[i].FCost == current.FCost && open[i].hCost < current.hCost) current = open[i];
                }

                open.Remove(current);
                closed.Add(current);

                if (current == goalNode)
                {
                    Retrace(grid, startNode, goalNode);
                    return;
                }

                foreach (var neighbour in grid.GetNeighbours(current))
                {
                    if (neighbour.isObstacle || closed.Contains(neighbour)) continue;
                    var newCost = current.gCost + Distance(current, neighbour);

                    if (newCost >= current.gCost && open.Contains(neighbour)) continue;
                    neighbour.gCost = newCost;
                    neighbour.hCost = Distance(neighbour, goalNode);
                    neighbour.previous = current;

                    if (!open.Contains(neighbour)) open.Add(neighbour);
                }
            }
        }

        private static int Distance(Node a, Node b)
        {
            var distanceX = Mathf.Abs(a.x - b.x);
            var distanceY = Mathf.Abs(a.y - b.y);
            if (distanceX < distanceY) return 14 * distanceX + 10 * (distanceY - distanceX);
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }

        private static void Retrace(Grid grid, Node startNode, Node goalNode)
        {
            var path = new List<Node>();
            var current = goalNode;

            while (current != startNode)
            {
                path.Add(current);
                current = current.previous;
            }

            path.Reverse();

            grid.path = path;
        }
    }
}