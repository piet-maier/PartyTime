using UnityEngine;
using UnityEngine.Tilemaps;

namespace AStar
{
    public class Grid : MonoBehaviour
    {
        // Unity Grid Size (Multiple of 2 * Cell Size)
        public Vector3 worldSize;

        // Unity Grid -> Cell Size / 2
        public float nodeRadius;

        // Obstacle Tile Maps
        public Tilemap[] obstacles;

        private Node[,] _nodes;

        // This method is called once at the start of the game.
        public void Start()
        {
            var sizeX = Mathf.RoundToInt(worldSize.x / (2 * nodeRadius));
            var sizeY = Mathf.RoundToInt(worldSize.y / (2 * nodeRadius));

            CreateGrid(sizeX, sizeY);
        }

        private void CreateGrid(int x, int y)
        {
            _nodes = new Node[x, y];

            // Bottom Left Grid Corner
            var corner = transform.position + Vector3.left * worldSize.x / 2 + Vector3.down * worldSize.y / 2;

            for (var i = 0; i < x; i++)
            {
                for (var j = 0; j < y; j++)
                {
                    var right = Vector3.right * ((2 * i + 1F) * nodeRadius);
                    var up = Vector3.up * ((2 * j + 1F) * nodeRadius);

                    // Initialize Node
                    _nodes[i, j] = new Node(corner + right + up, false);
                    
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

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            // Outline Grid in Scene View
            Gizmos.DrawWireCube(transform.position, worldSize);

            // Draw Nodes in Scene View
            if (_nodes == null) return;
            foreach (var node in _nodes)
            {
                Gizmos.color = node.IsObstacle ? Color.red : Color.white;
                Gizmos.DrawCube(node.WorldPosition, Vector3.one * nodeRadius);
            }
        }
    }
}