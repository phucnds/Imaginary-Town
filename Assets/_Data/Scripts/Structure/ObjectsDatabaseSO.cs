using System;
using System.Collections.Generic;
using UnityEngine;

namespace ImaginaryTown.Core
{
    [CreateAssetMenu(fileName = "ObjectsDatabaseSO", menuName = "ImaginaryTown/Structure/ObjectsDatabaseSO", order = 0)]
    public class ObjectsDatabaseSO : ScriptableObject
    {
        public List<ObjectData> objectDatas;
    }

    [Serializable]
    public class ObjectData
    {
        [field: SerializeField] public string Name { private set; get; }
        [field: SerializeField] public int ID { private set; get; }
        [field: SerializeField] public Vector2Int Size { private set; get; } = Vector2Int.one;
        [field: SerializeField] public GameObject Prefabs { private set; get; }
    }
}