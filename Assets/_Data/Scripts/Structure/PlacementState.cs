using UnityEngine;

namespace ImaginaryTown.Core
{
    public class PlacementState : IBuildingState
    {
        private int selectedObjectIndex = -1;
        int ID;
        Grid grid;
        PreviewSystem previewSystem;
        ObjectsDatabaseSO database;
        GridData groundData;
        GridData furnitureData;
        ObjectPlacer objectPlacer;

        public PlacementState( int iD, Grid grid, PreviewSystem previewSystem, ObjectsDatabaseSO database, GridData groundData, GridData furnitureData, ObjectPlacer objectPlacer)
        {
            ID = iD;
            this.grid = grid;
            this.previewSystem = previewSystem;
            this.database = database;
            this.groundData = groundData;
            this.furnitureData = furnitureData;
            this.objectPlacer = objectPlacer;


            selectedObjectIndex = database.objectDatas.FindIndex(data => data.ID == ID);
            if (selectedObjectIndex > -1)
            {
                previewSystem.StartShowingPlacementPreview(database.objectDatas[selectedObjectIndex].Prefabs,
                database.objectDatas[selectedObjectIndex].Size);
            }
            else
            {
                Debug.Log("No Id");
            }

        }

        public void EndState()
        {
            previewSystem.StopShowingPreview();
        }

        public void OnAction(Vector3Int gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

            if (!placementValidity) return;

            int index = objectPlacer.PlaceObject(database.objectDatas[selectedObjectIndex].Prefabs, grid.CellToWorld(gridPosition));
            GridData selectedData = database.objectDatas[selectedObjectIndex].ID == 0 ? groundData : furnitureData;
            selectedData.AddObjectAt(gridPosition, database.objectDatas[selectedObjectIndex].Size, database.objectDatas[selectedObjectIndex].ID, index);

            previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
        }

        private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
        {
            GridData selectedData = database.objectDatas[selectedObjectIndex].ID == 0 ? groundData : furnitureData;

            return selectedData.CanPlaceObjectAt(gridPosition, database.objectDatas[selectedObjectIndex].Size);
        }

        public void UpdateState(Vector3Int gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
            previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
        }
    }
}

