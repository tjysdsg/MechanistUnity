using System.Linq;
using Block;
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
        private TheBall _ballBeingEdited = null;

        public override void OnUpdate()
        {
            // switch to ball connection editor if a connected beam is selected
            if (_buildManager.isFired && _buildManager.selectionHitInfo != null)
            {
                RaycastHit info = _buildManager.selectionHitInfo.Value;
                if (info.transform.gameObject.layer == ObjectLayer.GetBlockAttachmentLayerIndex())
                {
                    var b = info.transform.GetComponent<Beam>();
                    _buildManager.connectionIndexBeingEdited = _ballBeingEdited.FindConnectionIndexFromOther(b);
                }

                _buildManager.ResetFireEventStatus();
            }
        }

        public override void OnStateEnter()
        {
            // TODO: support grouped editing
            Assert.AreEqual(1, _buildManager.blocksBeingEdited.Count);

            _buildManager.ResetFireEventStatus();

            _ballBeingEdited = (TheBall)_buildManager.blocksBeingEdited.ElementAt(0);

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
            _ballBeingEdited = null;
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