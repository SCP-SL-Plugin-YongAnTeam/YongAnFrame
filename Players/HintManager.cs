using Exiled.API.Features;
using MEC;
using System.Collections.Generic;
using System.Reflection;
using YongAnFrame.Components;

namespace YongAnFrame.Players
{
    /// <summary>
    /// 提示系统管理器
    /// </summary>
    public sealed class HintManager
    {
        /// <summary>
        /// 拥有该实例的框架玩家
        /// </summary>
        private readonly FramePlayer fPlayer;

        private readonly CoroutineHandle coroutine;

        public Text[] CustomText = new Text[20];
        public CapacityList<Text> MessageTexts { get; } = new(7);
        public CapacityList<Text> ChatTexts { get; } = new(6);
        public HintManager(FramePlayer player)
        {
            fPlayer = player;
            coroutine = Timing.RunCoroutine(Update());
        }

        public IEnumerator<float> Update()
        {
            while (true)
            {
                string[] text = new string[36];

                int used = 0;
                text[used] = $"YongAnFrame 1.0.0-alpha7";

                if (fPlayer.ExPlayer.DoNotTrack && !fPlayer.IsBDNT)
                {
                    text[used] = "[注意]已开启DoNotTrack(DNT)，游戏数据不会被保存，想保存数据请控制台输入pl BDNT查看详情";
                }
                
                used = 1;
                text[used] = "<align=left>";

                for (int i = 0; i < ChatTexts.Capacity; i++)
                {
                    Text chatText = ChatTexts[i];
                    if(chatText != null)
                    {
                        text[used] += chatText;
                        chatText.Duration--;

                        if (chatText.Duration <= 0)
                        {
                            ChatTexts.Remove(chatText);
                            i--;
                        }
                    }
                    else
                    {
                        text[used] = string.Empty;
                    }
                    used++;
                }

                foreach (Text data in CustomText)
                {
                    text[used] = data ?? string.Empty;
                    used++;
                }

                for (int i = 0; i < MessageTexts.Capacity; i++)
                {
                    Text messageText = MessageTexts[i];
                    if (messageText != null)
                    {
                        text[used] = $"[{messageText.Duration}]{messageText}";

                        messageText.Duration--;
                        if (messageText.Duration <= 0)
                        {
                            MessageTexts.Remove(messageText);
                            i--;
                        }
                    }
                    else
                    {
                        text[used] = string.Empty;
                    }
                    used++;
                }
                text[used] += "</align>";

                if (fPlayer.CustomRolePlus != null)
                {
                    text[34] = fPlayer.CustomRolePlus.Name;
                    text[35] = fPlayer.CustomRolePlus.Description;
                }

                fPlayer.ExPlayer.ShowHint($"<size=20>{string.Join("\n", text)}\n\n\n\n\n\n\n\n\n\n\n\n\n\n</size>", 2f);
                yield return Timing.WaitForSeconds(1f);
            }
        }

        public void Clean()
        {
            Timing.KillCoroutines(coroutine);
        }

        public class Text
        {
            public string Content { get; private set; }
            public float Duration { get; internal set; }

            public Text(string text, float duration, int size = 0)
            {
                Content = text;
                Duration = duration;
            }

            public override string ToString()
            {
                return Content;
            }
            public static implicit operator string(Text text)
            {
                return text.ToString();
            }
            public static implicit operator Text(string text)
            {
                return new Text(text, -1);
            }
        }
    }
}
