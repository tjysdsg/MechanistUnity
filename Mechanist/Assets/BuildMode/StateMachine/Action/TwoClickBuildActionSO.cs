using Block;
using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine.Assertions;

namespace BuildMode.SM
{
    [CreateAssetMenu(fileName = "TwoClickBuildAction", menuName = "State Machines/Actions/TwoClickBuildAction")]
    public class TwoClickBuildActionSO : StateActionSO<TwoClickBuildAction>
    {
        protected override StateAction CreateAction() => new TwoClickBuildAction();
    }

    public class TwoClickBuildAction : BuildModeBaseAction
    {
        protected new TwoClickBuildActionSO OriginSO => (TwoClickBuildActionSO)base.OriginSO;

        private AttachableBlock _firstBlock; // the first block to attach the brace to
        private bool _firstBlockSelected = false;

        public override void OnUpdate()
        {
            if (!_buildManager.isFired || _buildManager.selectionHitInfo == null) return;

            AttachableBlock selectedBlock =
                _buildManager.selectionHitInfo.Value.transform.GetComponent<AttachableBlock>();

            if (selectedBlock == null) return;

            if (!_firstBlockSelected)
            {
                step1(selectedBlock);
                _buildManager.ResetStateMachine(false);
            }
            else
            {
                step2(selectedBlock);
                _firstBlockSelected = false;
                _buildManager.ResetStateMachine(true);
            }
        }

        /// <summary>
        /// The first step in a two click build, the first object is selected
        /// </summary>
        private void step1(AttachableBlock selectedBlock)
        {
            _firstBlockSelected = true;
            _firstBlock = selectedBlock;
        }

        /// <summary>
        /// The second step of a two click build. The second object is selected and we should create the block right now.
        /// </summary>
        private void step2(AttachableBlock selectedBlock)
        {
            Assert.IsTrue(_buildManager.selectionHitInfo.HasValue);
            var selectionHitInfo = _buildManager.selectionHitInfo.Value;
            var selectionTransform = selectionHitInfo.transform;

            // instantiate brace prefab
            var go = GameObject.Instantiate(_buildManager.currentBlockType.GetPrefab());
            var b = go.GetComponent<TwoClickBuildBlock>();
            b.block1 = _firstBlock.transform;
            b.block2 = selectionTransform;
            b.Initialize();

            // notify two attached blocks
            var attachment = new BlockAttachment
            {
                obj = b, point = selectionHitInfo.point // TODO: point is incorrect
            };
            _firstBlock.OnAttach(attachment);
            selectedBlock.OnAttach(attachment);

            _buildManager.AddCreatedBlock(b);
        }

        public override void OnStateExit()
        {
            _firstBlockSelected = false;
            _firstBlock = null;
        }
    }
}