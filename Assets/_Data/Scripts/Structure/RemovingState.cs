using System;
using UnityEngine;

namespace ImaginaryTown.Core
{
    public class RemovingState : IBuildingState
    {
        private int gameObjectIndex = -1;
        Grid grid;
        PreviewSystem previewSystem;
        GridData groundData;
        GridData furnitureData;
        ObjectPlacer objectPlacer;

        public RemovingState(Grid grid, PreviewSystem previewSystem, GridData groundData, GridData furnitureData, ObjectPlacer objectPlacer)
        {
            this.grid = grid;
            this.previewSystem = previewSystem;
            this.groundData = groundData;
            this.furnitureData = furnitureData;
            this.objectPlacer = objectPlacer;

            previewSystem.StartShowingRemovePreview();
        }

        public void EndState()
        {
            previewSystem.StopShowingPreview();
        }

        public void OnAction(Vector3Int gridPosition)
        {
            GridData selectedData = null;
            if (!furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one))
            {
                selectedData = furnitureData;
            }

            else if (!groundData.CanPlaceObjectAt(gridPosition, Vector2Int.one))
            {
                selectedData = groundData;
            }

            if (selectedData == null)
            {
                //sound
            }
            else
            {
                gameObjectIndex = selectedData.GetPresentationIndex(gridPosition);
                if (gameObjectIndex == -1) return;

                selectedData.RemoveObjectAt(gridPosition);
                objectPlacer.RemoveObjectAt(gameObjectIndex);
            }

            Vector3 cellPosition = grid.CellToWorld(gridPosition);
            previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
        }

        private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
        {
            return furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) && groundData.CanPlaceObjectAt(gridPosition, Vector2Int.one);
        }

        public void UpdateState(Vector3Int gridPosition)
        {
            bool validity = CheckIfSelectionIsValid(gridPosition);
            previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
        }
    }
}


