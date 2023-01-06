using System.Linq;
using Core;
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
        }

        public override void OnStateEnter()
        {
            // TODO: support grouped editing
            Assert.AreEqual(1, _buildManager.blocksBeingEdited.Count);

            _buildManager.HighlightCurrentlySelectedBlock();

            _transformHandle =
                RuntimeTransformHandle.Create(
                    _buildManager.blocksBeingEdited.ElementAt(0).transform,
                    _buildManager.currentCamera,
                    HandleType.POSITION,
                    ObjectLayer.GetGizmosLayerIndex()
                );
            _transformHandle.space = HandleSpace.WORLD;

            _buildManager.usePositionTransformHandleEventChannel.OnEventRaised += SetTransformHandleTypeAsPosition;
            _buildManager.useRotationTransformHandleEventChannel.OnEventRaised += SetTransformHandleTypeAsRotation;
        }

        public override void OnStateExit()
        {
            _buildManager.ResetHighlight();

            _buildManager.usePositionTransformHandleEventChannel.OnEventRaised -= SetTransformHandleTypeAsPosition;
            _buildManager.useRotationTransformHandleEventChannel.OnEventRaised -= SetTransformHandleTypeAsRotation;

            GameObject.Destroy(_transformHandle.gameObject);
            _transformHandle = null;
        }

        private void SetTransformHandleTypeAsPosition()
        {
            _transformHandle.type = HandleType.POSITION;
        }

        private void SetTransformHandleTypeAsRotation()
        {
            _transformHandle.type = HandleType.ROTATION;
        }
    }
}