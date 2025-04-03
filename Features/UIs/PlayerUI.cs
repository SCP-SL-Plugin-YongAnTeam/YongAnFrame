using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Models.Hints;
using HintServiceMeow.Core.Utilities;
using MEC;
using System.Collections.Generic;
using UnityEngine;
using YongAnFrame.Components;
using YongAnFrame.Features.Players;
using YongAnFrame.Features.UIs.Texts;

namespace YongAnFrame.Features.UIs
{
    public class PlayerUI
    {

        public FramePlayer FPlayer { get; }
        /// <summary>
        /// 获取或设置<seealso cref="PlayerUI"/>的HintServiceMeow核心
        /// </summary>
        public PlayerDisplay PlayerDisplay { get; private set; }

        public CapacityList<MessageText> MessageList { get; }
        public CapacityList<ChatText> ChatList { get; }

        private readonly CoroutineHandle coroutine;
        #region Hint
        private readonly Hint versionHint = new()
        {
            Text = "YongAnFrame 1.0.0-Beta6",
            FontSize = 20,
            Alignment = HintAlignment.Center,
            YCoordinateAlign = HintVerticalAlign.Top
        };
        private readonly Hint customRoleHint = new()
        {
            FontSize = 20,
            Alignment = HintAlignment.Left,
            YCoordinateAlign = HintVerticalAlign.Bottom
        };
        private readonly Hint chatHint = new()
        {
            FontSize = 20,
            Alignment = HintAlignment.Right,
            YCoordinate = 700
        };
        private readonly Hint messageHint = new()
        {
            FontSize = 20,
            Alignment = HintAlignment.Left,
            YCoordinate = 700
        };
        #endregion

        private IEnumerator<float> Timer()
        {
            bool isUpdate = false;

            for (int i = 0; i < MessageList.Count; i++)
            {
                MessageText message = MessageList[i];
                if (message.Duration <= 0)
                {
                    MessageList.Remove(message);
                    i--;
                    isUpdate = true;
                }
            }

            if (isUpdate) UpdateMessageUI();
            
            isUpdate = false;
            for (int i = 0; i < ChatList.Count; i++)
            {
                ChatText chat = ChatList[i];
                if (chat.Duration <= 0)
                {
                    ChatList.Remove(chat);
                    i--;
                    isUpdate = true;
                }
            }
            if (isUpdate) UpdateChatUI();

            yield return Timing.WaitForSeconds(1f);
        }

        public void UpdateUI()
        {
            UpdateCustomRoleUI();
            UpdateMessageUI();
            UpdateChatUI();
        }

        public void UpdateCustomRoleUI()
        {
            if (FPlayer.CustomRolePlus == null)
            {
                customRoleHint.Text = null;
                return;
            }
            customRoleHint.Text = $"<size=26><color=\"{FPlayer.CustomRolePlus.NameColor}\">{FPlayer.CustomRolePlus.Name}</color></size>\n\r{FPlayer.CustomRolePlus.Description}";
        }

        public void UpdateMessageUI()
        {
            messageHint.Text = string.Join("\n\r", MessageList);
        }
        public void UpdateChatUI()
        {
            chatHint.Text = string.Join("\n\r", ChatList);
        }
        public void Clean()
        {
            Timing.KillCoroutines(coroutine);
            PlayerDisplay.ClearHint();
        }
        public PlayerUI(FramePlayer fPlayer)
        {
            FPlayer = fPlayer;
            MessageList = new(7, UpdateMessageUI);
            ChatList = new(7, UpdateChatUI);
            coroutine = Timing.RunCoroutine(Timer());
            PlayerDisplay = PlayerDisplay.Get(fPlayer);
            PlayerDisplay.AddHint(customRoleHint);
            PlayerDisplay.AddHint(chatHint);
            PlayerDisplay.AddHint(messageHint);
            PlayerDisplay.AddHint(versionHint);
        }
        ~PlayerUI()
        {
            Clean();
        }
    }
}
