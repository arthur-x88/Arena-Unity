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
            // TODO(TwiiK): After upgrading Bolt from 1.2.9 to 1.2.15 the "Launcher" scene would be passed in here as
            // well, which cause errors like duplicate players etc. It wasn't like this originally, but I'm not sure
            // what exactly has changed in Bolt to cause this. I'm sure this can be fixed properly, and not with a hack
            // like this, but I'm not going to investigate that at the moment. In another project I've upgraded Bolt to
            // 1.3.2 and the problem is there as well, so I assume this is due to some change in Bolt itself.
            if (map == "Launcher") {
                return;
            }

            base.SceneLoadLocalDone(map, token);

            if (BoltNetwork.IsConnected && BoltNetwork.IsClient)
                World.MapManager.InitializeLoadedMap(1);
        }
    }
}
