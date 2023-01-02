using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine.Assertions;

namespace BuildMode.SM
{
    [CreateAssetMenu(fileName = "MoveCameraToAction", menuName = "State Machines/Actions/MoveCameraToAction")]
    public class MoveCameraToActionSO : StateActionSO<MoveCameraToAction>
    {
        protected override StateAction CreateAction() => new MoveCameraToAction();
    }

    public class MoveCameraToAction : BuildModeBaseAction
    {
        protected new MoveCameraToActionSO OriginSO => (MoveCameraToActionSO)base.OriginSO;

        public override void OnUpdate()
        {
            Assert.IsTrue(_buildManager.cameraPivotPos.HasValue);
            _buildManager.moveToEventChannel.RaiseEvent(_buildManager.cameraPivotPos.Value);
            _buildManager.cameraPivotPos = null;
        }
    }
}