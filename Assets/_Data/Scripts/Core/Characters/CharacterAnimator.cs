using System;
using ImaginaryTown.Core;
using UnityEngine;

namespace ImaginaryTown.Core
{
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private Animator anim;

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            if (TryGetComponent<MoveAction>(out MoveAction moveAction))
            {
                moveAction.OnStartMoving += moveAction_OnStartMoving;
                moveAction.OnStopMoving += moveAction_OnStopMoving;
            }

            if (TryGetComponent<VictoryAction>(out VictoryAction victoryAction))
            {
                victoryAction.OnVictoryActionStarted += victoryAction_OnVictoryActionStarted;
            }

            if (TryGetComponent<GatherAction>(out GatherAction gatherAction))
            {
                gatherAction.OnGatherActionStarted += gatherAction_OnGatherActionStarted;
            }
        }

        private void gatherAction_OnGatherActionStarted()
        {
            anim.SetTrigger("gather");
        }

        private void victoryAction_OnVictoryActionStarted()
        {
            anim.SetTrigger("victory");
        }

        private void moveAction_OnStopMoving()
        {
            anim.SetBool("isMoving", false);
        }

        private void moveAction_OnStartMoving()
        {
            anim.SetBool("isMoving", true);
        }
    }


}
