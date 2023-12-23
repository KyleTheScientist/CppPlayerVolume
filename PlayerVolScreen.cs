using ComputerPlusPlus;
using ComputerPlusPlus.Tools;
using GorillaExtensions;
using GorillaNetworking;
using Photon.Pun;
using System;
using System.Linq;
using System.Text;

namespace CppPlayerVolume
{
    public class PlayerVolScreen : IScreen
    {
        public string Title => "PlayerVol";

        public string Description => "Change player volumes. [W/S] to scroll.";

        int index = 0;

        public string GetContent()
        {
            var rigs = GorillaParent.instance.vrrigs;
            if (rigs.Count == 0) return "\n\n" + ComputerManager.Center("No players found.");
            while (index >= rigs.Count) index--;
            var content = new StringBuilder();
            int i = 0;
            foreach (var rig in rigs)
            {
                var wrapper = rig.gameObject.GetOrAddComponent<RigWrapper>();
                try
                {
                    string arrow = i == index ? ">" : " ";
                    if (wrapper.invalid)
                    {
                        content.AppendLine($" {arrow}[{wrapper.volume}] <color=Red>RIG ERROR</color>");
                        continue;
                    }
                    content.AppendLine($" {arrow}[{wrapper.volume}]{wrapper.photonView.Owner.NickName}");
                } catch (Exception e)
                {
                    Logging.Debug("    wrapper:", wrapper);
                    Logging.Debug("    .PhotonView:", wrapper.photonView);
                    Logging.Debug("    .Owner:", wrapper.photonView.Owner);
                    Logging.Debug("    .NickName:", wrapper.photonView.Owner.NickName);
                    Logging.Exception(e);
                }
                i++;
            }
            return content.ToString();
        }

        public void OnKeyPressed(GorillaKeyboardButton button)
        {
            var rigs = GorillaParent.instance.vrrigs;
            if (button.IsNumericKey())
            {
                if (rigs.Count == 0) return;
                while (index >= rigs.Count) index--;
                var wrapper = rigs[index].gameObject.GetOrAddComponent<RigWrapper>();
                int volume = int.Parse(button.characterString);
                MetadataSerializer.volumes[wrapper.userId] = volume;
                wrapper.volume = volume;
                wrapper.voiceAudio.volume = volume / 9f;
                MetadataSerializer.Serialize();
                return;
            }

            if (button.characterString == "W")
            {
                index--;
                if (index < 0) index = 0;
            }
            else if (button.characterString == "S")
            {
                index++;
                if (index >= rigs.Count) index = rigs.Count - 1;
            }
        }

        public void Start()
        {

        }
    }
}
