using Block;
using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

namespace BuildMode.SM
{
    [CreateAssetMenu(fileName = "TwoClickBuildAction2", menuName = "State Machines/Actions/Two Click Build Action2")]
    public class TwoClickBuildAction2SO : StateActionSO<TwoClickBuildAction2>
    {
        protected override StateAction CreateAction() => new TwoClickBuildAction2();
    }

    public class TwoClickBuildAction2 : BuildModeBaseAction
    {
        protected new TwoClickBuildAction2SO OriginSO => (TwoClickBuildAction2SO)base.OriginSO;

        /// <summary>
        /// The second step of a two click build. The second object is selected and we should create the block right now.
        /// </summary>
        public override void OnUpdate()
        {
            if (_buildManager.selectionHitInfo == null) return;

            var selectionHitInfo = _buildManager.selectionHitInfo.Value;
            var selectionTransform = selectionHitInfo.transform;

            AttachableBlock selectedBlock = selectionTransform.GetComponent<AttachableBlock>();
            if (selectedBlock != null)
            {
                // instantiate brace prefab
                var go = GameObject.Instantiate(_buildManager.currBlockPrefab);
                var brace = go.GetComponent<Brace>();
                brace.block1 = _buildManager.twoClickBuildFirstBlock.transform;
                brace.block2 = selectionTransform;
                brace.Initialize();

                // notify two attached blocks
                var attachment = new BlockAttachment
                {
                    obj = brace, point = selectionHitInfo.point
                };
                _buildManager.twoClickBuildFirstBlock.OnAttach(attachment);
                selectedBlock.OnAttach(attachment);

                // reset build manager
                _buildManager.twoClickBuilding = false;
                _buildManager.twoClickBuildFirstBlock = null;
                _buildManager.selectionHitInfo = null;
            }
        }
    }
}