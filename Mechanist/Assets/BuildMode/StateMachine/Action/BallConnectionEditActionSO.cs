using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine.Assertions;

namespace BuildMode.SM
{
    [CreateAssetMenu(fileName = "BallConnectionEditAction",
        menuName = "State Machines/Actions/BallConnectionEditAction")]
    public class BallConnectionEditActionSO : StateActionSO<BallConnectionEditAction>
    {
        protected override StateAction CreateAction() => new BallConnectionEditAction();
    }

    public class BallConnectionEditAction : BuildModeBaseAction
    {
        protected new BallConnectionEditActionSO OriginSO => (BallConnectionEditActionSO)base.OriginSO;

        public override void OnUpdate()
        {
        }

        public override void OnStateEnter()
        {
            Assert.AreEqual(1, _buildManager.blocksBeingEdited.Count);

            _buildManager.HighlightCurrentlySelectedBlock();
        }

        public override void OnStateExit()
        {
            _buildManager.ResetHighlight();
            _buildManager.connectionIndexBeingEdited = -1; // need to set this in case Esc is pressed
        }
    }
}