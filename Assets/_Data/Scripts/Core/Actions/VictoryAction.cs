using System;
using UnityEngine;
using UnityEngine.Events;

namespace ImaginaryTown.Core
{
    public class VictoryAction : BaseAction
    {
        public event UnityAction OnVictoryActionStarted;
        public event UnityAction OnVictoryActionCompleted;

        private void Update()
        {
            if (!isActive) return;
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            OnVictoryActionStarted?.Invoke();
            ActionStart(onActionComplete);
        }

        public override string GetActionName()
        {
            return "Victory";
        }
    }
}

