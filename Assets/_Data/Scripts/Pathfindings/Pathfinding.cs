using System.Collections.Generic;
using UnityEngine;

namespace ImaginaryTown.Core
{
    public class Pathfinding : Singleton<Pathfinding>
    {
        // [SerializeField] private Transform gridDebugPrefab;
        [SerializeField] private LayerMask obstaclesLayerMark;

        private int width;
        private int height;
        private float cellSize;

        public GridSystem<PathNode> GridSystem { private set; get; }

        public void Setup(int width, int height, float cellSize)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;

            GridSystem = new GridSystem<PathNode>(width, height, cellSize, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
            // gridSystem.CreateDebugObjects(gridDebugPrefab);

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);

                    float raycastOffsetDistance = 5f;
                    if (Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance, Vector3.up, raycastOffsetDistance * 2, obstaclesLayerMark))
                    {
                        GetNode(x, z).SetIsWalkable(false);
                    }


                }
            }
        }

        private PathNode GetNode(int x, int z)
        {
            return GridSystem.GetGridObject(new GridPosition(x, z));
        }

        public void SetWalkableGridPosition(GridPosition GridPosition, bool isWalkable)
        {
            GridSystem.GetGridObject(GridPosition).SetIsWalkable(isWalkable);
        }

        public bool IsWalkableGridPosition(GridPosition GridPosition)
        {
            return GridSystem.GetGridObject(GridPosition).IsWalkable();
        }
    }

}