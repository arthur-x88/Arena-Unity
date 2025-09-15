using System.Collections.Generic;
using Bolt;
using Bolt.Utils;
using Common;
using Core;
using JetBrains.Annotations;
using UdpKit;
using UnityEngine;

namespace Server
{
    [UsedImplicitly]
    public partial class PhotonBoltServerListener : PhotonBoltBaseListener
    {
        [SerializeField, UsedImplicitly] private BalanceReference balance;
        [SerializeField, UsedImplicitly] private PhotonBoltReference photon;

        private new WorldServer World { get; set; }
        private ServerLaunchState LaunchState { get; set; }
        private ServerRoomToken ServerToken { get; set; }
        private readonly List<BoltConnection> pendingConnections = new List<BoltConnection>();

        public override void Initialize(World world)
        {
            base.Initialize(world);

            World = (WorldServer)world;

            EventHandler.RegisterEvent<ServerRoomToken>(photon, GameEvents.ServerMapLoaded, OnMapLoaded);
        }

        public override void Deinitialize()
        {
            EventHandler.UnregisterEvent<ServerRoomToken>(photon, GameEvents.ServerMapLoaded, OnMapLoaded);

            World = null;

            ServerToken = null;
            LaunchState = 0;

            base.Deinitialize();
        }

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

            if (BoltNetwork.IsConnected)
            {
                World.MapManager.InitializeLoadedMap(1);

                EventHandler.ExecuteEvent(photon, GameEvents.ServerMapLoaded, (ServerRoomToken)token);
            }
        }

        // Note: SceneLoadRemoteDone is obsolete in Bolt 1.2.15+
        // The player creation logic has been moved to SceneLoadLocalDone
        // where it's called after the scene loading is complete

        public override void SessionCreatedOrUpdated(UdpSession session)
        {
            base.SessionCreatedOrUpdated(session);

            HandleRoomCreation((ServerRoomToken)session.GetProtocolToken());
        }

        public override void ConnectRequest(UdpEndPoint endpoint, IProtocolToken token)
        {
            base.ConnectRequest(endpoint, token);

            if (!(token is ClientConnectionToken clientToken) || !clientToken.IsValid)
            {
                BoltNetwork.Refuse(endpoint, new ClientRefuseToken(ConnectRefusedReason.InvalidToken));
                return;
            }

            if (clientToken.UnityId == SystemInfo.unsupportedIdentifier)
            {
                BoltNetwork.Refuse(endpoint, new ClientRefuseToken(ConnectRefusedReason.UnsupportedDevice));
                return;
            }

            if (clientToken.Version != ServerToken.Version)
            {
                BoltNetwork.Refuse(endpoint, new ClientRefuseToken(ConnectRefusedReason.InvalidVersion));
                return;
            }

            BoltNetwork.Accept(endpoint);
        }

        public override void Connected(BoltConnection boltConnection)
        {
            base.Connected(boltConnection);

            World.SetDefaultScope(boltConnection);

            // If the server is fully launched, create player immediately
            if (LaunchState == ServerLaunchState.Complete)
            {
                World.CreatePlayer(boltConnection);
            }
            else
            {
                // Otherwise, queue the connection for when the server is ready
                pendingConnections.Add(boltConnection);
            }
        }

        public override void Disconnected(BoltConnection boltConnection)
        {
            base.Disconnected(boltConnection);

            // Remove from pending connections if it was there
            pendingConnections.Remove(boltConnection);

            // Only set network state if a player was created for this connection
            if (World.HasPlayerForConnection(boltConnection))
            {
                World.SetNetworkState(boltConnection, PlayerNetworkState.Disconnected);
            }
        }

        public override void EntityAttached(BoltEntity entity)
        {
            base.EntityAttached(entity);

            World.EntityAttached(entity);
        }

        public override void EntityDetached(BoltEntity entity)
        {
            base.EntityDetached(entity);

            World.EntityDetached(entity);
        }

        private void OnMapLoaded(ServerRoomToken roomToken)
        {
            ProcessServerLaunchState(ServerLaunchState.MapLoaded);

            if (BoltNetwork.IsSinglePlayer)
                HandleRoomCreation(roomToken);
        }

        private void HandleRoomCreation(ServerRoomToken roomToken)
        {
            ServerToken = roomToken;

            ProcessServerLaunchState(ServerLaunchState.SessionCreated);
        }

        private void ProcessServerLaunchState(ServerLaunchState state)
        {
            LaunchState |= state;

            if (LaunchState == ServerLaunchState.Complete)
            {
                World.ServerLaunched(ServerToken);

                // Create players for any connections that were waiting for the server to be ready
                foreach (BoltConnection connection in pendingConnections)
                {
                    World.CreatePlayer(connection);
                }
                pendingConnections.Clear();
            }
        }
    }
}
