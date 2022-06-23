using UnityEngine;

namespace AStar
{
    public class Node
    {
        public Vector3 WorldPosition;

        // Grid Index
        public readonly int X, Y;

        public bool IsObstacle;

        public int GCost, HCost;

        public Node Previous;

        public Node(Vector3 worldPosition, int x, int y, bool isObstacle)
        {
            WorldPosition = worldPosition;
            X = x;
            Y = y;
            IsObstacle = isObstacle;
        }

        public int FCost => GCost + HCost;
    }
}