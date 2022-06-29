using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    /// <summary>
    /// This class contains the implementation of the A* algorithm.
    /// </summary>
    public static class AStar
    {
        /// <summary>
        /// This method contains the implementation of the A* algorithm.
        /// </summary>
        public static List<Node> FindPath(Grid grid, Vector3 worldStart, Vector3 worldGoal)
        {
            var startNode = grid.WorldToNode(worldStart);
            var goalNode = grid.WorldToNode(worldGoal);

            var open = new BinaryHeap<Node>(grid.X * grid.Y);
            var closed = new HashSet<Node>();

            var previous = new Node[grid.X, grid.Y];

            open.Add(startNode);

            while (open.Size != 0)
            {
                var current = open.RemoveFirst();

                closed.Add(current);

                if (current == goalNode)
                {
                    return Retrace(startNode, goalNode, previous);
                }

                foreach (var neighbour in grid.GetNeighbours(current))
                {
                    if (neighbour.isObstacle || closed.Contains(neighbour)) continue;
                    var newCost = current.gCost + Distance(current, neighbour);

                    if (newCost >= current.gCost && open.Contains(neighbour)) continue;
                    neighbour.gCost = newCost;
                    neighbour.hCost = Distance(neighbour, goalNode);
                    previous[neighbour.X, neighbour.Y] = current;

                    if (!open.Contains(neighbour)) open.Add(neighbour);
                }
            }

            return null;
        }

        /// <summary>
        /// This method returns the distance between two nodes.
        /// </summary>
        private static int Distance(Node a, Node b)
        {
            var distanceX = Mathf.Abs(a.X - b.X);
            var distanceY = Mathf.Abs(a.Y - b.Y);
            if (distanceX < distanceY) return 14 * distanceX + 10 * (distanceY - distanceX);
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }

        /// <summary>
        /// This method retraces the correct path and returns it.
        /// </summary>
        private static List<Node> Retrace(Node startNode, Node goalNode, Node[,] previous)
        {
            var path = new List<Node>();
            var current = goalNode;

            while (current != startNode)
            {
                path.Add(current);
                current = previous[current.X, current.Y];
            }

            path.Reverse();

            return path;
        }
    }
}