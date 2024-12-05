using TMPro;
using UnityEngine;

namespace ImaginaryTown.Core
{
    public class GridDebugObject : MonoBehaviour
    {
        [SerializeField] private TextMeshPro gridPosition;
        private object gridObject;

        public virtual void SetGridObject(object gridObject)
        {
            this.gridObject = gridObject;

        }

        protected virtual void Update()
        {
            gridPosition.text = gridObject.ToString();
        }
    }
}