using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

namespace BuildMode.SM
{
    [CreateAssetMenu(fileName = "TwoClickBuildCondition",
        menuName = "State Machines/Conditions/TwoClickBuildCondition")]
    public class TwoClickBuildConditionSO : StateConditionSO<TwoClickBuildCondition>
    {
        protected override Condition CreateCondition() => new TwoClickBuildCondition();
    }

    public class TwoClickBuildCondition : BuildModeBaseCondition
    {
        protected new TwoClickBuildConditionSO OriginSO => (TwoClickBuildConditionSO)base.OriginSO;

        protected override bool Statement()
        {
            return _buildManager.isBuilding && _buildManager.currentBlockType.IsTwoClickBuild();
        }
    }
}