using System;
using UnityEngine;
using UnityEngine.Events;

namespace ImaginaryTown.Core
{
    public abstract class BaseAction : MonoBehaviour
    {
        protected bool isActive = false;
        protected Action onActionComplete;

        
        public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

        public void ActionStart(Action onActionComplete)
        {
            isActive = true;
            this.onActionComplete = onActionComplete;
        }

        public void ActionComplete()
        {
            isActive = false;
            onActionComplete();
        }

        public abstract string GetActionName();
    }
}
