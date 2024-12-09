using System;
using System.Collections.Generic;
using ImaginaryTown.Ultis;
using UnityEngine;

namespace ImaginaryTown.Core
{
    public class PlacementManager : MonoBehaviour
    {

        [SerializeField] private GameObject gridVisualiztion;
        [SerializeField] private ObjectsDatabaseSO databaseSO;
        [SerializeField] private Grid grid;
        [SerializeField] private PreviewSystem previewSystem;
        [SerializeField] private ObjectPlacer objectPlacer;
        private GridData groundData, furnitureData;

        private Vector3Int lastDetectedPosition = Vector3Int.zero;
        private IBuildingState buildingState;

        private void Start()
        {
            StopPlacement();
            groundData = new GridData();
            furnitureData = new GridData();
        }

        private void Update()
        {
            if (buildingState == null) return;
            Vector3 mousePosition = InputManager.Instance.GetSelectedMapPosition();
            Vector3Int gridPosition = grid.WorldToCell(mousePosition);

            if (lastDetectedPosition != gridPosition)
            {
                buildingState.UpdateState(gridPosition);
                lastDetectedPosition = gridPosition;
            }
        }

        public void StartPlacement(int ID)
        {
            StopPlacement();
            gridVisualiztion.SetActive(true);

            buildingState = new PlacementState(ID, grid, previewSystem, databaseSO, groundData, furnitureData, objectPlacer);

            InputManager.Instance.Onclicked += PlaceStructure;
            InputManager.Instance.OnExit += StopPlacement;
        }

        public void StartRemoving()
        {
            StopPlacement();
            gridVisualiztion.SetActive(true);
            buildingState = new RemovingState(grid, previewSystem, groundData, furnitureData, objectPlacer);

            InputManager.Instance.Onclicked += PlaceStructure;
            InputManager.Instance.OnExit += StopPlacement;
        }

        private void PlaceStructure()
        {
            if (InputManager.Instance.IsPointerOverUI()) return;

            Vector3 mousePosition = InputManager.Instance.GetSelectedMapPosition();
            Vector3Int gridPosition = grid.WorldToCell(mousePosition);

            buildingState.OnAction(gridPosition);
        }

        // private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
        // {
        //     GridData selectedData = databaseSO.objectDatas[selectedObjectIndex].ID == 0 ? groundData : furnitureData;

        //     return selectedData.CanPlaceObjectAt(gridPosition, databaseSO.objectDatas[selectedObjectIndex].Size);
        // }

        private void StopPlacement()
        {
            if (buildingState == null) return;

            gridVisualiztion.SetActive(false);
            buildingState.EndState();

            InputManager.Instance.Onclicked -= PlaceStructure;
            InputManager.Instance.OnExit -= StopPlacement;

            lastDetectedPosition = Vector3Int.zero;
            buildingState = null;
        }


    }
}

