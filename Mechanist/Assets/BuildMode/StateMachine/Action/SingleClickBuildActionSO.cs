using Block;
using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine.Assertions;

namespace BuildMode.SM
{
    [CreateAssetMenu(fileName = "SingleClickBuildAction", menuName = "State Machines/Actions/SingleClickBuildAction")]
    public class SingleClickBuildActionSO : StateActionSO<SingleClickBuildAction>
    {
        protected override StateAction CreateAction() => new SingleClickBuildAction();
    }

    public class SingleClickBuildAction : BuildModeBaseAction
    {
        protected new SingleClickBuildActionSO OriginSO => (SingleClickBuildActionSO)base.OriginSO;

        public override void OnUpdate()
        {
            if (!_buildManager.isFired) return;

            Vector3 targetPos = Vector3.zero;

            // find a point on the sphere surface where the camera pivot is on
            Camera camera = _buildManager.currentCamera.camera;
            float distance = camera.GetComponent<BuildModeCamera>().distance;
            Vector3 camPosition = camera.transform.position;
            Vector3 displacement = _buildManager.selectionRay.direction.normalized * distance;
            targetPos = camPosition + displacement;

            // find an empty spot if something blocks us
            if (_buildManager.selectionHitInfo != null)
            {
                var selectionHitInfo = _buildManager.selectionHitInfo.Value;
                if (selectionHitInfo.distance <= distance)
                    targetPos = selectionHitInfo.transform.position; // TODO: avoid overlap
            }

            // instantiate brace prefab
            var go = GameObject.Instantiate(_buildManager.blockConfig.GetPrefab(_buildManager.CurrentBlockType),
                targetPos, Quaternion.identity);
            var b = go.GetComponent<SingleClickBuildBlock>();
            b.Initialize();
            b.EnterBuildMode();

            _buildManager.AddCreatedBlock(b);

            _buildManager.ResetStateMachine(true);
        }
    }
}