using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AStar
{
    public class Grid : MonoBehaviour
    {
        private static Transform _player;

        // Unity Grid Position (Center)
        public Vector3 worldPosition;

        // Unity Grid Size (Multiple of 2 * Cell Size)
        public Vector3 worldSize;

        // Unity Grid -> Cell Size / 2
        public float nodeRadius = 0.02F;

        // Obstacle Tile Maps
        public Tilemap[] obstacles;

        public List<Node> path;

        private Node[,] _nodes;

        // Cell Number
        private int _sizeX, _sizeY;

        // This method is called once at the start of the game.
        public void Start()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<Transform>();

            _sizeX = Mathf.RoundToInt(worldSize.x / (2 * nodeRadius));
            _sizeY = Mathf.RoundToInt(worldSize.y / (2 * nodeRadius));

            CreateGrid();
        }

        public void Update()
        {
            PathFinding.FindPath(this, transform.position, _player.position);
            //gameObject.GetComponentInChildren<Transform>().Find("Position").transform.position
        }

        // Initialize Grid & Check Obstacles
        private void CreateGrid()
        {
            _nodes = new Node[_sizeX, _sizeY];

            // Bottom Left Grid Corner
            var corner = worldPosition + Vector3.left * worldSize.x / 2 + Vector3.down * worldSize.y / 2;

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
                        if (!map.HasTile(map.WorldToCell(_nodes[i, j].worldPosition))) continue;
                        _nodes[i, j].isObstacle = true;
                        break;
                    }
                }
            }
        }

        // World Coordinate -> Node
        public Node WorldToNode(Vector3 position)
        {
            var percentX = Mathf.Clamp01((position.x + worldSize.x / 2) / worldSize.x);
            var percentY = Mathf.Clamp01((position.y + worldSize.y / 2) / worldSize.y);
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

                    var x = node.x + i;
                    var y = node.y + j;

                    if (x >= 0 && x < _sizeX && y >= 0 && y < _sizeY) neighbours.Add(_nodes[x, y]);
                }
            }

            return neighbours;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            // Outline Grid in Scene View
            Gizmos.DrawWireCube(worldPosition, worldSize);

            if (_nodes == null) return;
            var playerPosition = WorldToNode(_player.position + Vector3.down * 2 * nodeRadius);

            // Draw Nodes in Scene View
            foreach (var node in _nodes)
            {
                // Player Position = Gray
                if (node == playerPosition) Gizmos.color = Color.gray;
                // Path = Black
                else if (path != null && path.Contains(node)) Gizmos.color = Color.black;
                // Obstacles = Red
                else if (node.isObstacle) Gizmos.color = Color.red;
                else continue;

                Gizmos.DrawCube(node.worldPosition, Vector3.one * nodeRadius);
            }
        }
    }
}