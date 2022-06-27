using UnityEngine;

namespace AStar
{
    public class Node
    {
        public Vector3 worldPosition;

        // Grid Index
        public readonly int x, y;

        public bool isObstacle;

        public int gCost, hCost;

        public Node previous;

        public Node(Vector3 worldPosition, int x, int y, bool isObstacle)
        {
            this.worldPosition = worldPosition;
            this.x = x;
            this.y = y;
            this.isObstacle = isObstacle;
        }

        public int FCost => gCost + hCost;
    }
}