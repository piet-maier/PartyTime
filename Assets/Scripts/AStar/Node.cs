using UnityEngine;

namespace AStar
{
    public class Node
    {
        public Vector3 WorldPosition;
        public bool IsObstacle;

        public Node(Vector3 worldPosition, bool isObstacle)
        {
            WorldPosition = worldPosition;
            IsObstacle = isObstacle;
        }
    }
}