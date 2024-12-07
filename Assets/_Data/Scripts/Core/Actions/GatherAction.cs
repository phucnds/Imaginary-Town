using System;
using UnityEngine;
using UnityEngine.Events;

namespace ImaginaryTown.Core
{
    public class GatherAction : BaseAction
    {
        public event UnityAction OnGatherActionStarted;
        public event UnityAction OnGatherActionCompleted;

        private void Update()
        {
            if (!isActive) return;
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            OnGatherActionStarted?.Invoke();
            ActionStart(onActionComplete);
        }

        public override string GetActionName()
        {
            return "Gather";
        }
    }
}
