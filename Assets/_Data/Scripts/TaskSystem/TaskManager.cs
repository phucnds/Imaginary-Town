using System;
using System.Collections.Generic;
using UnityEngine;


namespace ImaginaryTown.TaskSystem
{
    public class TaskManager : MonoBehaviour
    {
        [SerializeField] private Transform lstCharacter;
        [SerializeField] private Transform lstTransporter;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private GameObject cubePrefabs;
        [SerializeField] private GameObject cubeSlotPrefabs;

        TaskSystem<Task> taskSystem;
        public static TaskSystem<TransporterTask> transporterTaskSystem;

        private List<CubeSlot> cubeSlots = new List<CubeSlot>();

        private void Start()
        {
            taskSystem = new TaskSystem<Task>();
            transporterTaskSystem = new TaskSystem<TransporterTask>();

            foreach (Transform item in lstCharacter)
            {
                item.GetComponent<WorkerTaskAI>().Setup(taskSystem);
            }

            foreach (Transform item in lstTransporter)
            {
                item.GetComponent<TransporterWorkerTaskAI>().Setup(transporterTaskSystem);
            }

            GameObject slot1 = CreateStorage(new Vector3(10, 0, 5));
            CubeSlot cubeSlot1 = new CubeSlot(slot1.transform);

            GameObject slot2 = CreateStorage(new Vector3(10, 0, 15));
            CubeSlot cubeSlot2 = new CubeSlot(slot2.transform);

            GameObject slot3 = CreateStorage(new Vector3(10, 0, 25));
            CubeSlot cubeSlot3 = new CubeSlot(slot3.transform);

            cubeSlots.Add(cubeSlot1);
            cubeSlots.Add(cubeSlot2);
            cubeSlots.Add(cubeSlot3);

        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject cube = CreateCube(GetMousePosition());

                taskSystem.EnqueueTask(() =>
                {
                    foreach (CubeSlot cubeSlot in cubeSlots)
                    {
                        if (cubeSlot.IsEmpty())
                        {
                            cubeSlot.SetHasCubeIncoming(true);

                            Task task = new Task.TakeCubeToCubeSlot
                            {
                                cubePosition = cube.transform.position,
                                cubeSlotPosition = cubeSlot.GetPosition(),
                                carryCube = (WorkerTaskAI worker) =>
                                {
                                    cube.transform.SetParent(worker.transform);
                                    cube.transform.position += Vector3.up;
                                },
                                dropCube = () =>
                                {
                                    cube.transform.SetParent(null);
                                    cubeSlot.SetCubeTransform(cube.transform);
                                }
                            };

                            return task;
                        }
                    }

                    return null;
                });
            }

            if (Input.GetMouseButtonDown(1))
            {
                // GameObject slot = CreateStorage(GetMousePosition());
                Task task = new Task.MoveToPostition { targetPosition = GetMousePosition() };
                taskSystem.AddTask(task);
            }
        }

        private GameObject CreateCube(Vector3 pos)
        {
            GameObject go = Instantiate(cubePrefabs, pos, Quaternion.identity);
            return go;
        }

        private GameObject CreateStorage(Vector3 pos)
        {
            GameObject go = Instantiate(cubeSlotPrefabs, pos, Quaternion.identity);
            return go;
        }

        private Vector3 GetMousePosition()
        {
            Vector3 pos = new Vector3();
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f, groundLayer))
            {
                pos = hit.point;
            }

            return pos;
        }

        public class CubeSlot
        {
            private Transform cubeSlotTransform;
            private Transform cubeTransform;
            private bool hasCubeIncoming;

            public CubeSlot(Transform cubeSlotTransform)
            {
                this.cubeSlotTransform = cubeSlotTransform;
                SetCubeTransform(null);
            }

            public bool IsEmpty()
            {
                return cubeTransform == null && !hasCubeIncoming;

            }

            public void SetHasCubeIncoming(bool hasCubeIncoming)
            {
                this.hasCubeIncoming = hasCubeIncoming;
            }

            public void SetCubeTransform(Transform cubeTransform)
            {
                this.cubeTransform = cubeTransform;
                hasCubeIncoming = false;
                UpdateVisual();

                // LeanTween.delayedCall(4f, () =>
                // {
                //     if (cubeTransform == null) return;
                //     Destroy(cubeTransform.gameObject);
                //     SetCubeTransform(null);
                // });


                if (cubeTransform != null)
                {
                    TransporterTask.TakeCubeFromSlotToPosition task = new TransporterTask.TakeCubeFromSlotToPosition
                    {
                        cubeSlotPosition = GetPosition(),
                        targetPosition = GetPosition() + new Vector3(20, 0, 0),
                        carryCube = (TransporterWorkerTaskAI worker) =>
                            {
                                cubeTransform.SetParent(worker.transform);
                                SetCubeTransform(null);
                            },
                        dropCube = () =>
                        {
                            cubeTransform.SetParent(null);
                        }

                    };

                    transporterTaskSystem.AddTask(task);
                }
            }

            public Vector3 GetPosition()
            {
                return cubeSlotTransform.position;
            }

            private void UpdateVisual()
            {
                cubeSlotTransform.GetComponent<MeshRenderer>().material.color = IsEmpty() ? Color.red : Color.green;
            }

        }

        public class Task : BaseTask
        {
            public class MoveToPostition : Task
            {
                public Vector3 targetPosition;
            }

            public class Victory : Task { }

            public class Gathering : Task
            {
                public Vector3 targetPosition;
                public Action gather;
            }

            public class TakeCubeToCubeSlot : Task
            {
                public Vector3 cubePosition;
                public Action<WorkerTaskAI> carryCube;
                public Vector3 cubeSlotPosition;
                public Action dropCube;
            }
        }

        public class TransporterTask : BaseTask
        {
            public class TakeCubeFromSlotToPosition : TransporterTask
            {
                public Vector3 cubeSlotPosition;
                public Vector3 targetPosition;
                public Action<TransporterWorkerTaskAI> carryCube;
                public Action dropCube;
            }
        }
    }
}

