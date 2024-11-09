using UnityEngine;

namespace CGT.CharacterControls
{
    [CreateAssetMenu(fileName = "NewMotionSpeeds", menuName = "CGT/TPC/MotionSpeeds")]
    public class MotionSpeeds : ScriptableObject
    {
        [field: SerializeField] public virtual float WalkSpeed { get; set; } = 1f;
        [field: SerializeField] public virtual float RunSpeed { get; set; } = 5f;
        [field: SerializeField] public virtual float SprintSpeed { get; set; } = 8f;
        [field: SerializeField] public virtual float AirSpeed { get; set; } = 3f;
        [field: SerializeField] public virtual float CrouchWalkSpeed { get; set; } = 2f;
    }
}