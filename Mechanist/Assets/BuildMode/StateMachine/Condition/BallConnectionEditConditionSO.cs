using System.Linq;
using Core;
using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

namespace BuildMode.SM
{
    [CreateAssetMenu(fileName = "BallConnectionEditCondition",
        menuName = "State Machines/Conditions/BallConnectionEditCondition")]
    public class BallConnectionEditConditionSO : StateConditionSO<BallConnectionEditCondition>
    {
        protected override Condition CreateCondition() => new BallConnectionEditCondition();
    }

    public class BallConnectionEditCondition : BuildModeBaseCondition
    {
        protected new BallConnectionEditConditionSO OriginSO => (BallConnectionEditConditionSO)base.OriginSO;

        protected override bool Statement()
        {
            return _buildManager.blocksBeingEdited.Count == 1 &&
                   _buildManager.blocksBeingEdited.ElementAt(0).GetBlockType() == BlockType.Ball &&
                   _buildManager.connectionIndexBeingEdited != -1;
        }
    }
}