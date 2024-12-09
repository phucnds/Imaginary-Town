using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ImaginaryTown.Core
{
    public class InputManager : Singleton<InputManager>
    {
        [SerializeField] private Camera sceneCamera;
        [SerializeField] private LayerMask layerMask;

        private Vector3 lastPosition;

        public Action Onclicked, OnExit;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Onclicked?.Invoke();
            }

            if (Input.GetMouseButtonDown(1))
            {
                OnExit?.Invoke();
            }
        }

        public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

        #region Camera Movement

        public Vector3 GetSelectedMapPosition()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = sceneCamera.nearClipPlane;

            Ray ray = sceneCamera.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 999, layerMask))
            {
                lastPosition = hit.point;
            }

            return lastPosition;

        }

        public Vector2 GetMouseScreenPoint()
        {
            return Input.mousePosition;
        }

        public bool IsMouseButtonDown()
        {
            return Input.GetMouseButtonDown(0);
        }

        public Vector2 GetCameraMoveVector()
        {
            Vector3 inputMoveDir = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
            {
                inputMoveDir.y = 1f;
            }

            if (Input.GetKey(KeyCode.S))
            {
                inputMoveDir.y = -1f;
            }

            if (Input.GetKey(KeyCode.A))
            {
                inputMoveDir.x = -1f;
            }

            if (Input.GetKey(KeyCode.D))
            {
                inputMoveDir.x = 1f;
            }

            return inputMoveDir;
        }

        public float GetCameraRotateAmount()
        {
            float rotateAmount = 0;

            if (Input.GetKey(KeyCode.Q))
            {
                rotateAmount += 1f;
            }

            if (Input.GetKey(KeyCode.E))
            {
                rotateAmount -= 1f;
            }

            return rotateAmount;
        }

        public float GetCameraZoomAmout()
        {
            float zoomAmout = 0f;

            if (Input.mouseScrollDelta.y > 0)
            {
                zoomAmout = -1f;
            }

            if (Input.mouseScrollDelta.y < 0)
            {
                zoomAmout = 1f;
            }

            return zoomAmout;
        }

        #endregion
    }

}