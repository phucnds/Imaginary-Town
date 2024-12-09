using System;
using System.Collections.Generic;
using UnityEngine;

namespace ImaginaryTown.Core
{
    public class GridData
    {
        Dictionary<Vector3Int, PlacementData> placedObjects = new Dictionary<Vector3Int, PlacementData>();

        public void AddObjectAt(Vector3Int gridPostion, Vector2Int objectSize, int ID, int placedObjectIndex)
        {
            List<Vector3Int> positionToOccupy = CalculatePositions(gridPostion, objectSize);
            PlacementData placement = new PlacementData(positionToOccupy, ID, placedObjectIndex);

            foreach (var pos in positionToOccupy)
            {
                if (placedObjects.ContainsKey(pos))
                {
                    Debug.Log("Contain");
                    return;
                }

                placedObjects[pos] = placement;

                Debug.Log(pos);
            }
        }

        private List<Vector3Int> CalculatePositions(Vector3Int gridPostion, Vector2Int objectSize)
        {
            List<Vector3Int> returnVal = new List<Vector3Int>();

            for (int x = 0; x < objectSize.x; x++)
            {
                for (int y = 0; y < objectSize.y; y++)
                {
                    Vector3Int a = gridPostion + new Vector3Int(x, 0, y);
                    returnVal.Add(a);
                }
            }

            return returnVal;
        }

        public bool CanPlaceObjectAt(Vector3Int gridPostion, Vector2Int objectSize)
        {
            List<Vector3Int> positionToOccupy = CalculatePositions(gridPostion, objectSize);
            foreach (var pos in positionToOccupy)
            {
                if (placedObjects.ContainsKey(pos))
                {
                    return false;
                }
            }

            return true;
        }

        public int GetPresentationIndex(Vector3Int gridPosition)
        {
            if(!placedObjects.ContainsKey(gridPosition))
            {
                return -1;
            }

            return placedObjects[gridPosition].PlacedObjectIndex;
        }

        public void RemoveObjectAt(Vector3Int gridPosition)
        {
            foreach (var pos in placedObjects[gridPosition].occupiedPositions)
            {
                placedObjects.Remove(pos);
            }
        }
    }

    public class PlacementData
    {
        public List<Vector3Int> occupiedPositions;
        public int ID { get; private set; }
        public int PlacedObjectIndex { get; private set; }

        public PlacementData(List<Vector3Int> occupiedPositions, int ID, int PlacedObjectIndex)
        {
            this.occupiedPositions = occupiedPositions;
            this.ID = ID;
            this.PlacedObjectIndex = PlacedObjectIndex;
        }
    }

}