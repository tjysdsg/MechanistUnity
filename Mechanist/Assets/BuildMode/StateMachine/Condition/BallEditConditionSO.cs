using System.Linq;
using Core;
using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

namespace BuildMode.SM
{
    [CreateAssetMenu(fileName = "BallEditCondition",
        menuName = "State Machines/Conditions/BallEditCondition")]
    public class BallEditConditionSO : StateConditionSO<BallEditCondition>
    {
        protected override Condition CreateCondition() => new BallEditCondition();
    }

    public class BallEditCondition : BuildModeBaseCondition
    {
        protected new BallEditConditionSO OriginSO => (BallEditConditionSO)base.OriginSO;

        protected override bool Statement()
        {
            // TODO: support multiple selection
            return _buildManager.blocksBeingEdited.Count > 0 &&
                   _buildManager.blocksBeingEdited.ElementAt(0).GetBlockType() == BlockType.Ball;
        }
    }
}