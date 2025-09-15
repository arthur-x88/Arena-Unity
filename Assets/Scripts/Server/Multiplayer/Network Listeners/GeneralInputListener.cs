using Bolt;
using Core;
using System.Text.RegularExpressions;

namespace Server
{
    public partial class PhotonBoltServerListener
    {
        private const int MaxChatLength = 256;
        public override void OnEvent(TargetSelectionRequestEvent targetingRequest)
        {
            base.OnEvent(targetingRequest);

            if (targetingRequest.FromSelf)
                return;

            World.FindPlayer(targetingRequest.RaisedBy)?.Attributes.UpdateTarget(targetingRequest.TargetId.PackedValue, updateState: true);
        }

        public override void OnEvent(PlayerEmoteRequestEvent emoteRequest)
        {
            base.OnEvent(emoteRequest);

            var emoteType = (EmoteType)emoteRequest.EmoteType;
            if (!emoteType.IsDefined())
                return;

            World.FindPlayer(emoteRequest.RaisedBy)?.ModifyEmoteState(emoteType);
        }

        public override void OnEvent(PlayerChatRequestEvent chatRequest)
        {
            base.OnEvent(chatRequest);

            Player player = World.FindPlayer(chatRequest.RaisedBy);
            if (player == null)
                return;

            if (!player.IsAlive)
                return;

            // sanitize message: collapse whitespace, trim, clamp length
            string message = chatRequest.Message ?? string.Empty;
            message = Regex.Replace(message, "\\s+", " ").Trim();
            if (string.IsNullOrEmpty(message))
                return;
            if (message.Length > MaxChatLength)
                message = message.Substring(0, MaxChatLength);

            UnitChatMessageEvent unitChatMessageEvent = UnitChatMessageEvent.Create(GlobalTargets.Everyone);
            unitChatMessageEvent.SenderId = player.BoltEntity.NetworkId;
            unitChatMessageEvent.SenderName = player.Name;
            unitChatMessageEvent.Message = message;
            unitChatMessageEvent.Send();
        }

        public override void OnEvent(PlayerClassChangeRequestEvent classRequest)
        {
            base.OnEvent(classRequest);

            Player player = World.FindPlayer(classRequest.RaisedBy);
            if (player == null)
                return;

            var classType = (ClassType)classRequest.ClassType;
            if (!classType.IsDefined())
                return;

            player.SwitchClass(classType);
        }
    }
}
