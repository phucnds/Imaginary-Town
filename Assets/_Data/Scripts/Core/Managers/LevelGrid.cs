using UnityEngine;

namespace ImaginaryTown.Core
{
    public class LevelGrid : Singleton<LevelGrid>
    {
        [Header("Setting")]
        [SerializeField] private int width = 10;
        [SerializeField] private int height = 10;
        [SerializeField] private float cellSize = 2f;

        private GridSystem<GridObject> gridSystem;

        protected override void Awake()
        {
            base.Awake();

            gridSystem = new GridSystem<GridObject>(width, height, cellSize, (GridSystem<GridObject> g, GridPosition gridPostition) => new GridObject(g, gridPostition));
        }

        private void Start()
        {
            Pathfinding.Instance.Setup(width, height, cellSize);
        }

        public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);
        public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

        /*
        private void OnDrawGizmosSelected()
        {
            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    Gizmos.DrawLine(new Vector3(x, 0, y) * cellSize, new Vector3(x + 1, 0, y) * cellSize);
                    Gizmos.DrawLine(new Vector3(x, 0, y) * cellSize, new Vector3(x, 0, y + 1) * cellSize);
                }

            }

            Gizmos.DrawLine(new Vector3(0, 0, height - 1) * cellSize, new Vector3(width - 1, 0, height - 1) * cellSize);
            Gizmos.DrawLine(new Vector3(width - 1, 0, 0) * cellSize, new Vector3(width - 1, 0, height - 1) * cellSize);
        }*/
    }
}