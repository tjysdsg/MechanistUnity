using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine.InputSystem;

namespace PlayMode.SM
{
    [CreateAssetMenu(fileName = "ApplyForceAction", menuName = "State Machines/Actions/ApplyForceAction")]
    public class ApplyForceActionSO : StateActionSO<ApplyForceAction>
    {
        protected override StateAction CreateAction() => new ApplyForceAction();
    }

    public class ApplyForceAction : PlayModeBaseAction
    {
        protected new ApplyForceActionSO OriginSO => (ApplyForceActionSO)base.OriginSO;

        private Rigidbody _body = null;
        private float _distance = 0;
        private const float TimeToArrive = 0.5f;
        private const float MaxTargetVelocity = 1000;

        public override void OnUpdate()
        {
        }

        public override void OnFixedUpdate()
        {
            if (_body != null && _playManager.holdingFireInputPhase == InputActionPhase.Performed)
            {
                Camera cam = _playManager.CurrentCamera;
                Ray ray = cam.ScreenPointToRay(_playManager.InputManager.GetPointerScreenPosition());

                Vector3 targetPos = ray.origin + ray.direction.normalized * _distance;

                var position = _body.position;
                var delta = targetPos - position;

                // impulse = m deltaV
                var targetV = delta / TimeToArrive;
                if (targetV.magnitude > MaxTargetVelocity)
                    targetV = targetV.normalized * MaxTargetVelocity;

                Vector3 force = _body.mass * (targetV - _body.velocity);
                _body.AddForceAtPosition(force, position, ForceMode.Impulse);
            }
            else
            {
                _playManager.ResetStateMachine();
            }
        }

        public override void OnStateEnter()
        {
            Camera cam = _playManager.CurrentCamera;
            Ray ray = cam.ScreenPointToRay(_playManager.InputManager.GetPointerScreenPosition());
            if (Physics.Raycast(
                    ray: ray, hitInfo: out RaycastHit info, maxDistance: Mathf.Infinity,
                    layerMask: _playManager.RaycastMask
                ))
            {
                _body = info.transform.GetComponent<Rigidbody>();
            }
            else
            {
                _body = null;
            }

            if (_body == null)
            {
                // didn't hit any block, reset
                _playManager.ResetStateMachine();
                return;
            }

            _distance = (_body.transform.position - ray.origin).magnitude;
        }

        public override void OnStateExit()
        {
            _body = null;
            _distance = 0;
        }
    }
}