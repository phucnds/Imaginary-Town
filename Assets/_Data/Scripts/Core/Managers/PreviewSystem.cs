using System;
using UnityEngine;

namespace ImaginaryTown.Core
{
    public class PreviewSystem : MonoBehaviour
    {
        [SerializeField] private float previewYOffset = .06f;
        [SerializeField] private GameObject cellIndicator;
        [SerializeField] private Material previewMaterialsPrefab;

        private GameObject previewObject;
        private Material previewMaterialsInstance;
        private Renderer cellIndicatorRederer;

        private void Start()
        {
            previewMaterialsInstance = new Material(previewMaterialsPrefab);
            cellIndicator.SetActive(false);
            cellIndicatorRederer = cellIndicator.GetComponentInChildren<Renderer>();
        }

        public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
        {
            previewObject = Instantiate(prefab);
            PreparePreview(previewObject);
            PrepareCurrsor(size);
            cellIndicator.SetActive(true);
        }

        private void PrepareCurrsor(Vector2Int size)
        {
            if (size.x > 0 || size.y > 0)
            {
                cellIndicator.transform.localScale = new Vector3Int(size.x, 1, size.y);
                cellIndicatorRederer.material.mainTextureScale = size;
            }
        }

        private void PreparePreview(GameObject previewObject)
        {
            Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.materials;

                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = previewMaterialsInstance;
                }

                renderer.materials = materials;
            }
        }

        public void StopShowingPreview()
        {
            cellIndicator.SetActive(false);
            if (previewObject != null)
            {
                Destroy(previewObject);
            }

        }

        public void UpdatePosition(Vector3 position, bool validity)
        {

            if (previewObject != null)
            {
                MovePreview(position);
                ApplyFeedbackToPreview(validity);
            }

            MoveCursor(position);
            ApplyFeedbackCursor(validity);
        }

        private void ApplyFeedbackToPreview(bool validity)
        {
            Color c = validity ? Color.green : Color.red;
            c.a = .5f;
            previewMaterialsInstance.color = c;
        }

        private void ApplyFeedbackCursor(bool validity)
        {
            Color c = validity ? Color.green : Color.red;
            c.a = .5f;
            cellIndicatorRederer.material.color = c;
        }

        private void MoveCursor(Vector3 position)
        {
            cellIndicator.transform.position = position;
        }

        private void MovePreview(Vector3 position)
        {
            previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
        }

        public void StartShowingRemovePreview()
        {
            cellIndicator.SetActive(true);
            PrepareCurrsor(Vector2Int.one);
            ApplyFeedbackCursor(false);
        }
    }

}
