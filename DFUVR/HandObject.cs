using UnityEngine;

namespace DFUVR
{
    public class HandObject
    {
        public HandObjectLoadList asset { get; private set; }

        public GameObject gameObject { get; private set; }
        public Vector3 unsheatedPositionOffset { get; private set; }
        public Quaternion unsheatedRotationOffset { get; private set; }
        public Vector3 sheatedPositionOffset { get; private set; }
        public Quaternion sheatedRotationOffset { get; private set; }
        public bool renderSheated { get; private set; }

        public HandObject(HandObjectLoadList asset, GameObject gameObject)
        {
            this.asset = asset;
            this.gameObject = gameObject;
            unsheatedPositionOffset = asset.unsheatedPositionOffset;
            unsheatedRotationOffset = asset.unsheatedRotationOffset;
            sheatedPositionOffset = asset.sheatedPositionOffset;
            sheatedRotationOffset = asset.sheatedRotationOffset;
            renderSheated = asset.renderSheated;
        }
    }
}
