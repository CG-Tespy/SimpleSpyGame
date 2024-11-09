using System;
using UnityEngine;

namespace CGT.CharacterControls
{
    public interface IMovementInputReader
    {
        Vector2 CurrentMovementInput { get; }
        // For things that want to check it each frame rather than respond to an event

        event Action JumpStart;
        event Action JumpCancel;

        event Action MoveToggleStart;
        event Action CrouchToggleStart;

    }
}