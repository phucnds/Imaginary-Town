using System;
using ImaginaryTown.Core;
using UnityEngine;


namespace ImaginaryTown.TaskSystem
{
    public class WorkerTaskAI : MonoBehaviour
    {
        private enum State
        {
            WaitingForNextTask,
            ExecutingTask,
        }

        private MoveAction moveAction;
        private VictoryAction victoryAction;
        private GatherAction gatherAction;
        private TaskSystem<TaskManager.Task> taskSystem;

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

        public void Setup(TaskSystem<TaskManager.Task> taskSystem)
        {
            state = State.WaitingForNextTask;
            this.taskSystem = taskSystem;
        }

        private void RequestNextTask()
        {
            Debug.Log("RequestNextTask");
            TaskManager.Task task = taskSystem.RequestNextTask();
            if (task == null)
            {
                state = State.WaitingForNextTask;
            }
            else
            {
                state = State.ExecutingTask;

                if (task is TaskManager.Task.MoveToPostition)
                {
                    ExecuteTask_MoveToPostition(task as TaskManager.Task.MoveToPostition);
                    return;
                }

                if (task is TaskManager.Task.Victory)
                {
                    ExecuteTask_Victory(task as TaskManager.Task.Victory);
                    return;
                }

                if (task is TaskManager.Task.Gathering)
                {
                    ExecuteTask_Gathering(task as TaskManager.Task.Gathering);
                    return;
                }

                if (task is TaskManager.Task.TakeCubeToCubeSlot)
                {
                    ExecuteTask_TakeCubeToCubeSlot(task as TaskManager.Task.TakeCubeToCubeSlot);
                    return;
                }
            }
        }

        private void ExecuteTask_TakeCubeToCubeSlot(TaskManager.Task.TakeCubeToCubeSlot takeCubeToCubeSlot)
        {
            Debug.Log("ExecuteTask_TakeCubeToCubeSlot");

            GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(takeCubeToCubeSlot.cubePosition);
            moveAction.TakeAction(gridPosition, () =>
            {
                takeCubeToCubeSlot.carryCube(this);
                GridPosition gridPositionSlot = LevelGrid.Instance.GetGridPosition(takeCubeToCubeSlot.cubeSlotPosition);
                moveAction.TakeAction(gridPositionSlot, () =>
                {
                    takeCubeToCubeSlot.dropCube();
                    state = State.WaitingForNextTask;
                });
            });
        }

        private void ExecuteTask_Victory(TaskManager.Task.Victory victoryTask)
        {
            Debug.Log("ExecuteTask_Victory");

            victoryAction.TakeAction(null, () =>
            {
                state = State.WaitingForNextTask;
            });
        }

        private void ExecuteTask_MoveToPostition(TaskManager.Task.MoveToPostition moveToPostitionTask)
        {
            Debug.Log("ExecuteTask_MoveToPostition");

            GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(moveToPostitionTask.targetPosition);
            moveAction.TakeAction(gridPosition, () =>
            {
                state = State.WaitingForNextTask;
            });
        }

        private void ExecuteTask_Gathering(TaskManager.Task.Gathering gatherTask)
        {
            Debug.Log("ExecuteTask_Gathering");

            GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(gatherTask.targetPosition);
            moveAction.TakeAction(gridPosition, () =>
            {
                gatherAction.TakeAction(null, () =>
                {
                    gatherTask.gather();
                    state = State.WaitingForNextTask;
                });
            });
        }
    }
}

