﻿using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

namespace BuildMode.SM
{
    [CreateAssetMenu(fileName = "MoveCameraToCondition",
        menuName = "State Machines/Conditions/MoveCameraToCondition")]
    public class MoveCameraToConditionSO : StateConditionSO<MoveCameraToCondition>
    {
        protected override Condition CreateCondition() => new MoveCameraToCondition();
    }

    public class MoveCameraToCondition : BuildModeBaseCondition
    {
        protected new MoveCameraToConditionSO OriginSO => (MoveCameraToConditionSO)base.OriginSO;

        protected override bool Statement()
        {
            return _buildManager.currentBlockConfig.IsNone() && _buildManager.cameraPivotPos.HasValue;
        }
    }
}