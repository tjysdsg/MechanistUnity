using UnityEngine;
using Block;

namespace BuildMode
{
    public class BuildModeManager : MonoBehaviour
    {
        [Tooltip("The event channel used to tell the build mode camera to move to a certain object")] [SerializeField]
        private Vector3EventChannelSO moveToEventChannel;

        [SerializeField] private BuildModeStateSO state;

        [SerializeField] private InputManager inputManager;

        // TODO: use state machine
        private bool _buildingBrace = false;
        private Brace _currBrace;
        private AttachableBlock _attachingBlock; // the first block to attach the brace to
        [SerializeField] private GameObject bracePrefab;

        public void OnEnable()
        {
            inputManager.BuildingModeEnterPlacementEvent += OnEnterPlacementMode;
        }

        public void OnDisable()
        {
            inputManager.BuildingModeEnterPlacementEvent -= OnEnterPlacementMode;
        }

        /// <summary>
        /// User clicked left mouse button and the BuildingModeCamera dispatch the event to us
        /// </summary>
        public void OnCameraFire(Ray ray)
        {
            if (state.state == BuildModeState.Placement && Physics.Raycast(ray, out RaycastHit info))
            {
                if (!_buildingBrace)
                {
                    AttachableBlock selectedBlock = info.transform.GetComponent<AttachableBlock>();
                    if (selectedBlock != null)
                    {
                        _buildingBrace = true;
                        _attachingBlock = selectedBlock;

                        // don't create the brace until the two selections are confirmed
                    }
                }
                else
                {
                    AttachableBlock selectedBlock = info.transform.GetComponent<AttachableBlock>();
                    if (selectedBlock != null)
                    {
                        _buildingBrace = false;

                        // instantiate brace prefab
                        var go = Instantiate(bracePrefab);
                        _currBrace = go.GetComponent<Brace>();
                        _currBrace.block1 = _attachingBlock.transform;
                        _currBrace.block2 = info.transform;
                        _currBrace.Initialize();

                        // notify two attached blocks
                        var attachment = new BlockAttachment
                        {
                            obj = _currBrace, point = info.point
                        };
                        _attachingBlock.OnAttach(attachment);
                        selectedBlock.OnAttach(attachment);
                    }
                }
            }
        }

        /// <summary>
        /// User double clicked left mouse button and the BuildingModeCamera dispatch the event to us
        /// </summary>
        public void OnCameraDoubleFire(Ray ray)
        {
            if (state.state != BuildModeState.Placement && Physics.Raycast(ray, out RaycastHit info))
            {
                moveToEventChannel.RaiseEvent(info.point);
            }
        }

        public void OnEnterPlacementMode()
        {
            // TODO: UI for display current build mode state
            if (state.state == BuildModeState.Placement)
                state.state = BuildModeState.None;
            else
                state.state = BuildModeState.Placement;
        }
    }
}