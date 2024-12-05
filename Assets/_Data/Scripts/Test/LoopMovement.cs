using ImaginaryTown.Core;
using UnityEngine;

public class LoopMovement : MonoBehaviour
{
    [SerializeField] private MoveAction[] moveActions;
    [SerializeField] private Transform[] transforms;

    private bool isGo = true;
    private bool isActive = false;
    private int index = 0;

    private void Update()
    {
        if (!moveActions[0].ReachedTarget()) return;

        index++;
        if (index >= transforms.Length) index = 0;
        moveActions[0].target = transforms[index];
        moveActions[0].Move();
    }
}
