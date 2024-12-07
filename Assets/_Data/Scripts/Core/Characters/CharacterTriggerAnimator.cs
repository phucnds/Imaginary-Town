using UnityEngine;

namespace ImaginaryTown.Core
{
    public class CharacterTriggerAnimator : MonoBehaviour
    {
        [SerializeField] private Character character;

        private void TriggerAnimation()
        {
            if (character.transform.TryGetComponent<VictoryAction>(out VictoryAction victoryAction))
            {
                victoryAction.ActionComplete();
            }
        }

        private void TriggerAnimationGather()
        {
            if (character.transform.TryGetComponent<GatherAction>(out GatherAction gatherAction))
            {
                gatherAction.ActionComplete();
            }
        }
    }
}
