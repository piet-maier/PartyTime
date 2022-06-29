using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AStar
{
    /// <summary>
    /// This class represents the grid that is used to find a path.
    /// </summary>
    public class Grid : MonoBehaviour
    {
        /// <summary>
        /// This variable contains the radius of a single node.
        /// </summary>
        public float nodeRadius;

        /// <summary>
        /// This variable contains the size of the grid.
        /// The values should be multiples of the node size.
        /// </summary>
        public Vector3 size;

        /// <summary>
        /// This variable contains all relevant tilemaps.
        /// </summary>
        public Tilemap[] obstacles;

        /// <summary>
        /// This variable contains the nodes that make up the grid.
        /// </summary>
        private Node[,] _nodes;

        /// <summary>
        /// This variable contains the target to which a path is to be found.
        /// </summary>
        private Transform _player;

        /// <summary>
        /// This method returns the number of nodes on the horizontal axis.
        /// </summary>
        public int X => _nodes.GetLength(0);

        /// <summary>
        /// This method returns the number of nodes on the vertical axis.
        /// </summary>
        public int Y => _nodes.GetLength(1);

        // This method is called once at the start of the game.
        public void Start()
        {
            _player = GameObject.Find("Player").GetComponent<Transform>();

            CreateGrid();
        }

        /// <summary>
        /// This method initializes the grid and checks the tilemaps for obstacles.
        /// </summary>
        private void CreateGrid()
        {
            var x = Mathf.RoundToInt(size.x / (2 * nodeRadius));
            var y = Mathf.RoundToInt(size.y / (2 * nodeRadius));

            _nodes = new Node[x, y];

            var corner = transform.position + Vector3.left * size.x / 2 + Vector3.down * size.y / 2;

            for (var i = 0; i < X; i++)
            {
                for (var j = 0; j < Y; j++)
                {
                    var right = Vector3.right * ((2 * i + 1F) * nodeRadius);
                    var up = Vector3.up * ((2 * j + 1F) * nodeRadius);
                    var gridPosition = new Vector2(i, j);

                    _nodes[i, j] = new Node(corner + right + up, gridPosition, false);

                    foreach (var map in obstacles)
                    {
                        if (!map.HasTile(map.WorldToCell(_nodes[i, j].worldPosition))) continue;
                        _nodes[i, j].isObstacle = true;
                        break;
                    }
                }
            }

            foreach (var node in _nodes)
            {
                foreach (var map in obstacles)
                {
                    foreach (var neighbour in GetNeighbours(node)
                                 .Where(neighbour => map.HasTile(map.WorldToCell(neighbour.worldPosition))))
                    {
                        node.isObstacle = true;
                        break;
                    }

                    if (node.isObstacle) break;
                }
            }
        }

        /// <summary>
        /// This method returns the node that corresponds to the given coordinate.
        /// </summary>
        public Node WorldToNode(Vector3 position)
        {
            var gridCenter = transform.position;

            var percentX = Mathf.Clamp01((position.x - gridCenter.x + size.x / 2) / size.x);
            var percentY = Mathf.Clamp01((position.y - gridCenter.y + size.y / 2) / size.y);

            var x = Mathf.RoundToInt((X - 1) * percentX);
            var y = Mathf.RoundToInt((Y - 1) * percentY);

            return _nodes[x, y];
        }

        /// <summary>
        /// This method returns the neighbouring nodes of the given node.
        /// </summary>
        public List<Node> GetNeighbours(Node node)
        {
            var neighbours = new List<Node>();

            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    var x = node.X + i;
                    var y = node.Y + j;

                    if (x >= 0 && x < X && y >= 0 && y < Y) neighbours.Add(_nodes[x, y]);
                }
            }

            return neighbours;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            // Outline Grid
            Gizmos.DrawWireCube(transform.position, size);

            if (_nodes == null) return;
            var playerPosition = WorldToNode(_player.position);

            // Draw Nodes
            foreach (var node in _nodes)
            {
                if (node == playerPosition) Gizmos.color = Color.white;
                else if (node.isObstacle) Gizmos.color = Color.red;
                else continue;

                Gizmos.DrawCube(node.worldPosition, Vector3.one * nodeRadius);
            }
        }
    }
}