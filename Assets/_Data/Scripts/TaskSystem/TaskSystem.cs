using System;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;


namespace ImaginaryTown.TaskSystem
{
    public class QueuedTask<TTask> where TTask : BaseTask
    {
        private Func<TTask> tryGetTaskFunc;

        public QueuedTask(Func<TTask> tryGetTaskFunc)
        {
            this.tryGetTaskFunc = tryGetTaskFunc;
        }

        public TTask TryDequeueTask()
        {
            return tryGetTaskFunc();
        }
    }

    public abstract class BaseTask { }

    public class TaskSystem<TTask> where TTask : BaseTask
    {
        private List<TTask> taskList;
        private List<QueuedTask<TTask>> queuedTaskList;

        public TaskSystem()
        {
            taskList = new List<TTask>();
            queuedTaskList = new List<QueuedTask<TTask>>();
            FunctionPeriodic.Create(DequeueTasks, .2f);
        }

        public TTask RequestNextTask()
        {
            if (taskList.Count > 0)
            {
                TTask task = taskList[0];
                taskList.RemoveAt(0);
                return task;
            }
            else
            {
                return null;
            }
        }

        public void AddTask(TTask task)
        {
            taskList.Add(task);
        }

        public void EnqueueTask(QueuedTask<TTask> queuedTask)
        {
            queuedTaskList.Add(queuedTask);
        }

        public void EnqueueTask(Func<TTask> tryGetTaskFunc)
        {
            QueuedTask<TTask> queuedTask = new QueuedTask<TTask>(tryGetTaskFunc);
            queuedTaskList.Add(queuedTask);
        }

        private void DequeueTasks()
        {
            for (int i = 0; i < queuedTaskList.Count; i++)
            {
                QueuedTask<TTask> queuedTask = queuedTaskList[i];
                TTask task = queuedTask.TryDequeueTask();
                if (task != null)
                {
                    AddTask(task);
                    queuedTaskList.RemoveAt(i);
                    i--;
                }
                else
                {

                }
            }
        }
    }
}

