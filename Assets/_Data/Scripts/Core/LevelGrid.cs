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
    }
}