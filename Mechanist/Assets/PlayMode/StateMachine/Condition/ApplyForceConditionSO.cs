using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayMode.SM
{
    [CreateAssetMenu(fileName = "ApplyForceCondition",
        menuName = "State Machines/Conditions/ApplyForceCondition")]
    public class ApplyForceConditionSO : StateConditionSO<ApplyForceCondition>
    {
        protected override Condition CreateCondition() => new ApplyForceCondition();
    }

    public class ApplyForceCondition : PlayModeBaseCondition
    {
        protected new ApplyForceConditionSO OriginSO => (ApplyForceConditionSO)base.OriginSO;

        protected override bool Statement()
        {
            return _playManager.holdingFireInputPhase == InputActionPhase.Performed;
        }
    }
}