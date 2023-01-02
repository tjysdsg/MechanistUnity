using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

namespace BuildMode.SM
{
    [CreateAssetMenu(fileName = "TwoClickBuild2Condition",
        menuName = "State Machines/Conditions/TwoClickBuild2Condition")]
    public class TwoClickBuild2ConditionSO : StateConditionSO<TwoClickBuild2Condition>
    {
        protected override Condition CreateCondition() => new TwoClickBuild2Condition();
    }

    public class TwoClickBuild2Condition : BuildModeBaseCondition
    {
        protected new TwoClickBuild2ConditionSO OriginSO => (TwoClickBuild2ConditionSO)base.OriginSO;

        protected override bool Statement()
        {
            return _buildManager.twoClickBuilding && _buildManager.twoClickBuildFirstBlock != null;
        }
    }
}