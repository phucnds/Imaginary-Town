using System;
using System.Collections.Generic;
using UnityEngine;

namespace ImaginaryTown.Core
{
    public class ObjectPlacer : MonoBehaviour
    {
        [SerializeField] private List<GameObject> placedGameObjects = new List<GameObject>();

        public int PlaceObject(GameObject prefabs, Vector3 pos)
        {
            GameObject go = Instantiate(prefabs);
            go.transform.position = pos;
            placedGameObjects.Add(go);
            return placedGameObjects.Count - 1;
        }

        public void RemoveObjectAt(int gameObjectIndex)
        {
            if (placedGameObjects.Count < gameObjectIndex || placedGameObjects[gameObjectIndex] == null) return;

            Destroy(placedGameObjects[gameObjectIndex]);
            placedGameObjects[gameObjectIndex] = null;

        }
    }
}
