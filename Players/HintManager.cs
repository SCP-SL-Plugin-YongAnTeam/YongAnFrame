using MEC;
using System.Collections.Generic;
using YongAnFrame.Components;

namespace YongAnFrame.Players
{
    /// <summary>
    /// 提示系统管理器
    /// </summary>
    public sealed class HintManager
    {
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

        private IEnumerator<float> Update()
        {
            while (true)
            {
                CustomText = new Text[20];
                Events.Handlers.FramePlayer.OnFramerHintUpdate();
                string[] text = new string[36];

                int used = 0;
                text[used] = $"YongAnFrame 1.0.0-Beta5";

                if (fPlayer.ExPlayer.DoNotTrack && !fPlayer.IsBDNT)
                {
                    text[used] = "[注意]已开启DoNotTrack(DNT)，游戏数据不会被保存，想保存数据请控制台输入pl BDNT查看详情";
                }

                used = 1;
                text[used] = "<align=left><noparse>";

                for (int i = 0; i < ChatTexts.Capacity; i++)
                {
                    Text chatText = ChatTexts[i];
                    if (chatText != null)
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
                        text[used] += Text.Empty;
                    }
                    used++;
                }
                text[used] = "</noparse>";

                foreach (Text data in CustomText)
                {
                    text[used] += data ?? Text.Empty;
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
                        text[used] += Text.Empty;
                    }
                    used++;
                }
                text[34] = "</align>";

                if (fPlayer.CustomRolePlus != null)
                {
                    text[34] += $"<color=\"{fPlayer.CustomRolePlus.NameColor}\">{fPlayer.CustomRolePlus.Name}</color>";
                    text[35] = fPlayer.CustomRolePlus.Description;
                }
                fPlayer.ExPlayer.ShowHint($"<size=20>{string.Join("\n", text)}\n\n\n\n\n\n\n\n\n\n\n\n\n\n</size>", 2f);
                yield return Timing.WaitForSeconds(1f);
            }
        }

        /// <summary>
        /// 立刻停用这个提示系统管理器
        /// </summary>
        public void Clean()
        {
            Timing.KillCoroutines(coroutine);
        }

        public class Text(string text, float duration)
        {
            public string Content { get; private set; } = text;
            public float Duration { get; internal set; } = duration;

            public static string Empty => "<color=#00000000>占</color>";

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
