using UnityEngine;

namespace ImaginaryTown.Core
{
    public class EffectManager : Singleton<EffectManager>
    {
        public Transform EffectParent { get { return transform; } }

        public void ClearChild()
        {
            foreach (Transform tr in transform)
            {
                Destroy(tr.gameObject);
            }
        }
    }
}
