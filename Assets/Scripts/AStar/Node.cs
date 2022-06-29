using System;
using UnityEngine;

namespace AStar
{
    /// <summary>
    /// This class represents a single node of the grid that is used to find a path.
    /// </summary>
    public class Node : IComparable<Node>
    {
        /// <summary>
        /// This variable contains the world coordinate of the center of the node.
        /// </summary>
        public Vector3 worldPosition;

        /// <summary>
        /// This variable contains the grid index of the node.
        /// </summary>
        private readonly Vector2 _gridPosition;

        /// <summary>
        /// This variable specifies whether the node is an obstacle or not.
        /// </summary>
        public bool isObstacle;

        /// <summary>
        /// The <c>fCost</c> of each node is calculated by adding the following values:
        /// The <c>gCost</c> is the cost of the path from the start to the node.
        /// The <c>hCost</c> is an estimate of the cost from the node to the goal.
        /// </summary>
        public int gCost, hCost;

        public Node(Vector3 worldPosition, Vector2 gridPosition, bool isObstacle)
        {
            this.worldPosition = worldPosition;
            _gridPosition = gridPosition;
            this.isObstacle = isObstacle;
        }

        /// <summary>
        /// This method returns the grid index on the horizontal axis.
        /// </summary>
        public int X => (int)_gridPosition.x;

        /// <summary>
        /// This method returns the grid index on the vertical axis.
        /// </summary>
        public int Y => (int)_gridPosition.y;

        /// <summary>
        /// This method returns the total value of the node by adding <see cref="gCost"/> and <see cref="hCost"/>.
        /// </summary>
        private int FCost => gCost + hCost;

        public int CompareTo(Node other)
        {
            var compare = FCost.CompareTo(other.FCost);
            return compare == 0 ? hCost.CompareTo(other.hCost) : compare;
        }
    }
}