using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AStar
{
    public class Grid : MonoBehaviour
    {
        public Transform player;
        
        // Unity Grid Size (Multiple of 2 * Cell Size)
        public Vector3 worldSize;

        // Unity Grid -> Cell Size / 2
        public float nodeRadius;

        // Obstacle Tile Maps
        public Tilemap[] obstacles;

        public List<Node> Path;

        private Node[,] _nodes;

        // Cell Number
        private int _sizeX, _sizeY;

        // This method is called once at the start of the game.
        public void Start()
        {
            _sizeX = Mathf.RoundToInt(worldSize.x / (2 * nodeRadius));
            _sizeY = Mathf.RoundToInt(worldSize.y / (2 * nodeRadius));

            CreateGrid();
        }

        // Initialize Grid & Check Obstacles
        private void CreateGrid()
        {
            _nodes = new Node[_sizeX, _sizeY];

            // Bottom Left Grid Corner
            var corner = transform.position + Vector3.left * worldSize.x / 2 + Vector3.down * worldSize.y / 2;

            for (var i = 0; i < _sizeX; i++)
            {
                for (var j = 0; j < _sizeY; j++)
                {
                    var right = Vector3.right * ((2 * i + 1F) * nodeRadius);
                    var up = Vector3.up * ((2 * j + 1F) * nodeRadius);

                    // Initialize Node
                    _nodes[i, j] = new Node(corner + right + up, i, j, false);
                    
                    // Check Obstacles
                    foreach (var map in obstacles)
                    {
                        if (!map.HasTile(map.WorldToCell(_nodes[i, j].WorldPosition))) continue;
                        _nodes[i, j].IsObstacle = true;
                        break;
                    }
                }
            }
        }

        // World Coordinate -> Node
        public Node WorldToNode(Vector3 worldPosition)
        {
            var percentX = Mathf.Clamp01((worldPosition.x + worldSize.x / 2) / worldSize.x);
            var percentY = Mathf.Clamp01((worldPosition.y + worldSize.y / 2) / worldSize.y);
            var x = Mathf.RoundToInt((_sizeX - 1) * percentX);
            var y = Mathf.RoundToInt((_sizeY - 1) * percentY);
            return _nodes[x, y];
        }

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

                    if (x >= 0 && x < _sizeX && y >= 0 && y < _sizeY) neighbours.Add(_nodes[x, y]);
                }
            }

            return neighbours;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            // Outline Grid in Scene View
            Gizmos.DrawWireCube(transform.position, worldSize);
            
            if (_nodes == null) return;
            var playerPosition = WorldToNode(player.position + Vector3.down * 2 * nodeRadius);
            
            // Draw Nodes in Scene View
            foreach (var node in _nodes)
            {
                // Player Position = Gray
                if (node == playerPosition) Gizmos.color = Color.gray;
                // Path = Black
                else if (Path.Contains(node)) Gizmos.color = Color.black;
                // Obstacles = Red
                else Gizmos.color = node.IsObstacle ? Color.red : Color.white;
                
                Gizmos.DrawCube(node.WorldPosition, Vector3.one * nodeRadius);
            }
        }
    }
}