using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

namespace BuildMode.SM
{
    [CreateAssetMenu(fileName = "BallEditAction", menuName = "State Machines/Actions/BallEditAction")]
    public class BallEditActionSO : StateActionSO<BallEditAction>
    {
        protected override StateAction CreateAction() => new BallEditAction();
    }

    public class BallEditAction : BuildModeBaseAction
    {
        protected new BallEditActionSO OriginSO => (BallEditActionSO)base.OriginSO;

        public override void OnUpdate()
        {
            // TODO
        }
    }
}