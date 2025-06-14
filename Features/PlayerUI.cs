using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Models.Hints;
using HintServiceMeow.Core.Utilities;
using MEC;
using System.Collections.Generic;
using System.Text;
using YongAnFrame.Components;
using YongAnFrame.Features.Players;
using YongAnFrame.Features.Roles;
using YongAnFrame.Features.UI.Texts;

namespace YongAnFrame.Features
{
    /// <summary>
    /// 玩家UI
    /// </summary>
    public class PlayerUI
    {
        /// <summary>
        /// 获取拥有该实例的<seealso cref="FramePlayer"/>
        /// </summary>
        public FramePlayer FPlayer { get; }
        /// <summary>
        /// 获取<seealso cref="PlayerUI"/>的HintServiceMeow核心
        /// </summary>
        public PlayerDisplay PlayerDisplay { get; private set; }
        /// <summary>
        /// 获取消息数据列表
        /// </summary>
        public CapacityList<MessageText> MessageList { get; }
        /// <summary>
        /// 获取聊天数据列表
        /// </summary>
        public CapacityList<ChatText> ChatList { get; }

        private readonly CoroutineHandle coroutine;
        #region Hint
        private readonly Hint versionHint = new()
        {
            Text = "YongAnFrame 1.0.0-beta6+002",
            FontSize = 20,
            Alignment = HintAlignment.Center,
            YCoordinateAlign = HintVerticalAlign.Top,
            YCoordinate = 0
        };
        private readonly Hint customRoleHint = new()
        {
            FontSize = 20,
            Alignment = HintAlignment.Center,
            YCoordinateAlign = HintVerticalAlign.Bottom,
            YCoordinate = 1080
        };
        private readonly Hint chatHint = new()
        {
            FontSize = 20,
            Alignment = HintAlignment.Right,
            YCoordinate = 400
        };
        private readonly Hint messageHint = new()
        {
            FontSize = 20,
            Alignment = HintAlignment.Left,
            YCoordinate = 400
        };
        #endregion

        private IEnumerator<float> Timer()
        {
            while (true)
            {


                for (int i = 0; i < MessageList.Count; i++)
                {
                    MessageText message = MessageList[i];
                    if (message.Duration-- <= 0)
                    {
                        MessageList.Remove(message);
                        i--;
                    }
                    UpdateMessageUI();
                }

                bool isUpdate = false;

                for (int i = 0; i < ChatList.Count; i++)
                {
                    ChatText chat = ChatList[i];
                    if (chat.Duration-- <= 0)
                    {
                        ChatList.Remove(chat);
                        i--;
                        isUpdate = true;
                    }
                }
                if (isUpdate) UpdateChatUI();

                yield return Timing.WaitForSeconds(1f);
            }
        }

        /// <summary>
        /// 更新全部UI
        /// </summary>
        public void UpdateUI()
        {
            UpdateCustomRoleUI();
            UpdateMessageUI();
            UpdateChatUI();
        }

        /// <summary>
        /// 更新自定义角色UI
        /// </summary>
        public void UpdateCustomRoleUI()
        {
            if (FPlayer.CustomRolePlus is null)
            {
                customRoleHint.Text = null;
                return;
            }
            StringBuilder builder = new($"<size=26><color=\"{FPlayer.CustomRolePlus.NameColor}\">{FPlayer.CustomRolePlus.Name}</color></size>\n\r{FPlayer.CustomRolePlus.Description}");
            Skill[]? Skills = FPlayer.CustomRolePlus.BaseData[FPlayer].Skills;
            if (Skills != null)
            {
                foreach (var skill in Skills)
                {
                    builder.AppendLine($"{skill.Name}({skill.UseItem}):{skill.Description}(激活:{skill.ActiveMaxTime}|冷却:{skill.BurialMaxTime})");
                }
            }
            customRoleHint.Text = builder.ToString();
        }

        /// <summary>
        /// 更新消息UI
        /// </summary>
        public void UpdateMessageUI()
        {
            messageHint.Text = string.Join("\n\r", MessageList);
        }
        /// <summary>
        /// 更新聊天UI
        /// </summary>
        public void UpdateChatUI()
        {
            chatHint.Text = string.Join("\n\r", ChatList);
        }
        /// <summary>
        /// 构造方法，解构方法，更新聊天，，更新全部
        /// </summary>
        public void Clean()
        {
            Timing.KillCoroutines(coroutine);
            PlayerDisplay.ClearHint();
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="fPlayer"></param>
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
        /// <summary>
        /// 解构方法
        /// </summary>
        ~PlayerUI()
        {
            Clean();
        }
    }
}
