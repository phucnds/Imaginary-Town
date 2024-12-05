using System;
using System.Collections.Generic;
using UnityEngine;

namespace ImaginaryTown.Core
{
    public class MoveAction : MonoBehaviour
    {
        [SerializeField] private int maxMoveDistance = 2;
        [SerializeField] private CharacterPathfinding pathfinding;

        public Transform target;

        private List<Vector3> positionList = new List<Vector3>();
        private int currentPositionIndex;
        private bool isActive = false;

        private void Update()
        {

            if (!isActive) return;

            Vector3 targetPosition = positionList[currentPositionIndex];
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            float rotateSpeed = 10f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotateSpeed * Time.deltaTime);

            float stoppingDistance = .1f;
            if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
            {
                float moveSpeed = 4f;
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
            else
            {
                currentPositionIndex++;

                if (ReachedTarget())
                {
                    ActionComplete();
                }
            }
        }

        private void TakeAction(GridPosition gridPosition)
        {

            List<GridPosition> pathGridPositionList = pathfinding.FindPath(GetGridPosition(), gridPosition, out int pathLength);

            currentPositionIndex = 0;
            positionList = new List<Vector3>();

            foreach (GridPosition pathGridPosition in pathGridPositionList)
            {
                positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
            }

            ActionStart();
        }

        private GridPosition GetGridPosition()
        {
            return LevelGrid.Instance.GetGridPosition(transform.position);
        }

        protected void ActionStart(Action onActionComplete = null)
        {
            isActive = true;

        }

        protected void ActionComplete()
        {
            isActive = false;
        }

        [NaughtyAttributes.Button]
        public void Move()
        {
            GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(target.position);
            TakeAction(gridPosition);
        }

        public bool ReachedTarget()
        {
            return currentPositionIndex >= positionList.Count;
        }
    }
}