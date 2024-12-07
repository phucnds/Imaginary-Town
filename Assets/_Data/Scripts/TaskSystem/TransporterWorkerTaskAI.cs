using System;
using ImaginaryTown.Core;
using UnityEngine;


namespace ImaginaryTown.TaskSystem
{
    public class TransporterWorkerTaskAI : MonoBehaviour
    {
        private enum State
        {
            WaitingForNextTask,
            ExecutingTask,
        }

        private MoveAction moveAction;
        private VictoryAction victoryAction;
        private GatherAction gatherAction;
        private TaskSystem<TaskManager.TransporterTask> taskSystem;

        private State state;
        private float waitingTimer;
        private float waitingTimerMax = .2f;

        private void Start()
        {
            moveAction = GetComponent<MoveAction>();
            victoryAction = GetComponent<VictoryAction>();
            gatherAction = GetComponent<GatherAction>();
        }

        private void Update()
        {
            switch (state)
            {
                case State.WaitingForNextTask:
                    waitingTimer -= Time.deltaTime;
                    if (waitingTimer <= 0)
                    {
                        waitingTimer = waitingTimerMax;
                        RequestNextTask();
                    }
                    break;

                case State.ExecutingTask:
                    break;
            }
        }

        public void Setup(TaskSystem<TaskManager.TransporterTask> taskSystem)
        {
            state = State.WaitingForNextTask;
            this.taskSystem = taskSystem;
        }

        private void RequestNextTask()
        {
            Debug.Log("RequestNextTask");
            TaskManager.TransporterTask task = taskSystem.RequestNextTask();
            if (task == null)
            {
                state = State.WaitingForNextTask;
            }
            else
            {
                state = State.ExecutingTask;

                if (task is TaskManager.TransporterTask.TakeCubeFromSlotToPosition)
                {
                    ExecuteTask_TakeCubeFromSlotToPosition(task as TaskManager.TransporterTask.TakeCubeFromSlotToPosition);
                    return;
                }

            }
        }

        private void ExecuteTask_TakeCubeFromSlotToPosition(TaskManager.TransporterTask.TakeCubeFromSlotToPosition takeCubeFromCubeSlot)
        {
            Debug.Log("ExecuteTask_TakeCubeFromSlotToPosition");

            GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(takeCubeFromCubeSlot.cubeSlotPosition);
            moveAction.TakeAction(gridPosition, () =>
            {
                takeCubeFromCubeSlot.carryCube(this);
                GridPosition gridPositionSlot = LevelGrid.Instance.GetGridPosition(takeCubeFromCubeSlot.targetPosition);
                moveAction.TakeAction(gridPositionSlot, () =>
                {
                    takeCubeFromCubeSlot.dropCube();
                    state = State.WaitingForNextTask;
                });
            });
        }
    }
}

