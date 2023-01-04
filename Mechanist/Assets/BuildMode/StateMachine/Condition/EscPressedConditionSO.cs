using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

namespace BuildMode.SM
{
    [CreateAssetMenu(fileName = "EscPressedCondition",
        menuName = "State Machines/Conditions/EscPressedCondition")]
    public class EscPressedConditionSO : StateConditionSO<EscPressedCondition>
    {
        protected override Condition CreateCondition() => new EscPressedCondition();
    }

    public class EscPressedCondition : BuildModeBaseCondition
    {
        protected new EscPressedConditionSO OriginSO => (EscPressedConditionSO)base.OriginSO;

        protected override bool Statement()
        {
            return _buildManager.escPressed;
        }
    }
}