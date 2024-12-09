using UnityEngine;

namespace ImaginaryTown.Core
{
    public class RoadManager : Singleton<RoadManager>
    {
        public Transform RoadParent { get { return transform; } }

        public void ClearChild()
        {
            foreach (Transform tr in transform)
            {
                Destroy(tr.gameObject);
            }
        }
    }
}
