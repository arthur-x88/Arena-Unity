using Bolt;
using JetBrains.Annotations;
using UdpKit;
using UnityEngine;

namespace Core
{
    [UsedImplicitly]
    public class PhotonBoltSharedListener : PhotonBoltBaseListener
    {
        [SerializeField, UsedImplicitly] PhotonBoltReference photon;

        public override void SceneLoadLocalDone(string map, IProtocolToken token)
        {
            base.SceneLoadLocalDone(map, token);

            // Proceed only for gameplay scenes (have MapSettings in hierarchy)
            var settings = Object.FindObjectOfType<MapSettings>();
            if (settings == null)
                return;

            if (BoltNetwork.IsConnected && BoltNetwork.IsClient)
                World.MapManager.InitializeLoadedMap(settings.Definition.Id);
        }
    }
}
