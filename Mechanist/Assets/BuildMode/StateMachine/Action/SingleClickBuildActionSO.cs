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
            if (_buildManager.selectionHitInfo == null) return;
            var selectionHitInfo = _buildManager.selectionHitInfo.Value;

            var selectionTransform = selectionHitInfo.transform;
            AttachableBlock selectedBlock = selectionTransform.GetComponent<AttachableBlock>();

            if (selectedBlock == null) return;

            // instantiate brace prefab
            var go = GameObject.Instantiate(_buildManager.currentBlockType.GetPrefab());
            var b = go.GetComponent<SingleClickBuildBlock>();
            b.Initialize();

            _buildManager.AddCreatedBlock(b);

            // reset build manager
            _buildManager.isBuilding = false;
            _buildManager.selectionHitInfo = null;
        }
    }
}