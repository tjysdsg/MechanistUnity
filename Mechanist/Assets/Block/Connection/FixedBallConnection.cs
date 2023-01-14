using System;
using Core;
using SaveSystem;
using UnityEngine;

namespace Block
{
    [Serializable]
    public class FixBallBeamConnection : BallBeamConnection
    {
        private FixedJoint _joint = null;

        public override BlockConnectionType GetConnectionType() => BlockConnectionType.Fixed;

        public FixBallBeamConnection(TheBall ball, Beam beam) : base(ball, beam)
        {
        }

        public override Joint CreatePhysicalConnection()
        {
            _joint = _ball.gameObject.AddComponent<FixedJoint>();
            _joint.connectedBody = _plug;
            return _joint;
        }

        public override void DestroyPhysicalConnection()
        {
            GameObject.Destroy(_joint);
        }

        public override void Update()
        {
        }

        public override void OnDisable()
        {
        }

        #region Save and load

        internal class FixedConnectionSaveData : SaveData
        {
            public ReferenceSaveData ball;
            public ReferenceSaveData beam;

            public FixedConnectionSaveData(int id, string typename) : base(id, typename)
            {
            }
        }

        public override SaveData OnSave()
        {
            var data = new FixedConnectionSaveData(GetSaveDataId(), GetConnectionType().ToString());
            data.ball = new ReferenceSaveData(_ball.GetSaveDataId());
            data.beam = new ReferenceSaveData(_beam.GetSaveDataId());
            return data;
        }

        public override void OnLoad(SaveData data, ISaveableInstanceLoader loader)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}