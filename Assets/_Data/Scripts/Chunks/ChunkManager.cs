using UnityEngine;

namespace ImaginaryTown.Core
{
    public class ChunkManager : Singleton<ChunkManager>
    {
        public Transform ChunkParent { get { return transform; } }

        public void ClearChild()
        {
            foreach (Transform tr in transform)
            {
                Destroy(tr.gameObject);
            }
        }

        public void DestroyDuplicate(Vector3 pos)
        {
            foreach (Transform tr in transform)
            {
                if (tr.position == pos)
                {
                    Destroy(tr.gameObject);
                }
            }
        }
    }
}


