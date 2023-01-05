using GameState;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace PlayMode
{
    [RequireComponent(typeof(StateMachine.StateMachine))]
    public class PlayModeManager : MonoBehaviour
    {
        [Header("Configs")] [SerializeField] private GameModeSO gameMode;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private CameraSO currentCamera;
        [SerializeField] private LayerMask raycastMask;

        /// <summary>
        /// Did user left-clicked the mouse
        /// </summary>
        [HideInInspector] public bool isFired = false;

        [HideInInspector] public bool escPressed = false;

        public InputActionPhase holdingFireInputPhase = InputActionPhase.Canceled;

        // =============================================

        public InputManager InputManager => inputManager;
        public Camera CurrentCamera => currentCamera.camera;
        public LayerMask RaycastMask => raycastMask;
        StateMachine.StateMachine _sm;

        private void Start()
        {
            _sm = GetComponent<StateMachine.StateMachine>();
        }

        private void OnEnable()
        {
            inputManager.EscPressedEvent += OnEsc;
            inputManager.HoldFireEvent += OnHoldFire;

            // we want to keep these callbacks below active even if this game object is disable
            // but we need to make sure they're not registered for multiple times

            gameMode.OnEventRaised -= OnGameModeChange;
            gameMode.OnEventRaised += OnGameModeChange;
        }

        private void OnDisable()
        {
            inputManager.EscPressedEvent -= OnEsc;
            inputManager.HoldFireEvent -= OnHoldFire;
        }

        #region APIs Used by State Actions

        public void ResetStateMachine()
        {
            escPressed = false;
            isFired = false;
            holdingFireInputPhase = InputActionPhase.Canceled;
        }

        #endregion

        #region Input Handling

        private void OnEsc()
        {
            escPressed = true;
        }

        public void OnHoldFire(InputActionPhase phase)
        {
            holdingFireInputPhase = phase;
        }

        #endregion

        public void OnGameModeChange(GameMode mode)
        {
            // we always reset no matter what game mode we entered
            ResetStateMachine();

            // enable/disable this game object
            if (mode == GameMode.PlayMode)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}