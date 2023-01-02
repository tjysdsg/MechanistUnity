using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

namespace BuildMode.SM
{
    [CreateAssetMenu(fileName = "SingleClickBuildCondition",
        menuName = "State Machines/Conditions/SingleClickBuildCondition")]
    public class SingleClickBuildConditionSO : StateConditionSO<SingleClickBuildCondition>
    {
        protected override Condition CreateCondition() => new SingleClickBuildCondition();
    }

    public class SingleClickBuildCondition : BuildModeBaseCondition
    {
        protected new SingleClickBuildConditionSO OriginSO => (SingleClickBuildConditionSO)base.OriginSO;

        protected override bool Statement()
        {
            return _buildManager.isPlacing && _buildManager.currentBlockType.IsSingleClickBuild();
        }
    }
}