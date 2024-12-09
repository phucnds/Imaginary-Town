using UnityEngine;

namespace ImaginaryTown.Core
{
    public class GridObject
    {
        private GridSystem<GridObject> gridSystem;
        private GridPosition gridPosition;
        private Chunk chunk;

        public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
        {
            this.gridSystem = gridSystem;
            this.gridPosition = gridPosition;
        }

        public override string ToString()
        {
            return gridPosition.ToString();
        }
    }
}