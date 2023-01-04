using Block;
using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

namespace BuildMode.SM
{
    [CreateAssetMenu(fileName = "NormalStateAction", menuName = "State Machines/Actions/NormalStateAction")]
    public class NormalStateActionSO : StateActionSO<NormalStateAction>
    {
        protected override StateAction CreateAction() => new NormalStateAction();
    }

    public class NormalStateAction : BuildModeBaseAction
    {
        protected new NormalStateActionSO OriginSO => (NormalStateActionSO)base.OriginSO;

        public override void OnUpdate()
        {
            if (!_buildManager.isFired) return;

            // select an existing block to edit
            if (_buildManager.selectionHitInfo != null)
            {
                RaycastHit info = _buildManager.selectionHitInfo.Value;
                _buildManager.SelectBlockToEdit(info.transform.GetComponent<BaseBlock>());
            }
            else
            {
                _buildManager.ResetStateMachine(true);
            }
        }

        public override void OnStateEnter()
        {
            _buildManager.ResetStateMachine(true);
        }
    }
}