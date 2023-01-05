using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;
using TransformHandle;
using UnityEngine.Assertions;

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
        private RuntimeTransformHandle _transformHandle = null;

        public override void OnUpdate()
        {
            // TODO
        }

        public override void OnStateEnter()
        {
            // TODO: support grouped editing
            Assert.AreEqual(1, _buildManager.blocksBeingEdited.Count);

            _transformHandle =
                RuntimeTransformHandle.Create(
                    _buildManager.blocksBeingEdited[0].transform,
                    _buildManager.currentCamera,
                    HandleType.POSITION,
                    LayerMask.NameToLayer("Gizmos")
                );
            _transformHandle.space = HandleSpace.WORLD;
        }

        public override void OnStateExit()
        {
            GameObject.Destroy(_transformHandle.gameObject);
            _transformHandle = null;
        }
    }
}