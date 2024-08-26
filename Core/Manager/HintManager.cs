using MEC;
using System.Collections.Generic;
using System.Reflection;

namespace YongAnFrame.Core.Manager
{
    /// <summary>
    /// 提示系统管理器
    /// </summary>
    public class HintManager
    {
        /// <summary>
        /// 拥有该实例的框架玩家
        /// </summary>
        private readonly FramePlayer fPlayer;

        private readonly CoroutineHandle coroutine;

        /// <summary>
        /// 存在一些问题，暂不开放
        /// </summary>
        private readonly Text[] customText = new Text[20];
        public List<Text> RoleText { get; } = [];
        public List<Text> MessageTexts { get; } = [];
        public List<Text> ChatTexts { get; } = [];
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

                int usedMex = text.Length - 1;
                int used = 0;
                text[used] += "YongAnFrame Beta v0.0.1";
                //used++;

                used = 20;

                text[used] = "<align=left>";

                if (ChatTexts.Count > 28 - used)
                {
                    for (int i = 0; i < ChatTexts.Count - (28 - used); i++)
                    {
                        ChatTexts.Remove(ChatTexts[i]);
                    }
                }

                for (int i = 0; i < ChatTexts.Count; i++)
                {
                    Text textData = ChatTexts[i];

                    text[used] += textData;
                    used++;

                    textData.Duration--;
                    if (textData.Duration <= 0)
                    {
                        ChatTexts.Remove(textData);
                        i--;
                    }
                }

                used = 29;

                if (usedMex - RoleText.Count < used + MessageTexts.Count + 1)
                {
                    for (int i = 0; i < usedMex - RoleText.Count - (used + MessageTexts.Count + 1); i++)
                    {
                        MessageTexts.Remove(MessageTexts[i]);
                    }
                }

                for (int i = 0; i < MessageTexts.Count; i++)
                {
                    Text messageText = MessageTexts[i];
                    text[used] = $"[{messageText.Duration}]{messageText}";

                    messageText.Duration--;
                    if (messageText.Duration <= 0)
                    {
                        MessageTexts.Remove(messageText);
                        i--;
                    }
                }

                used = usedMex - RoleText.Count;
                text[used] += "</align>";

                foreach (Text roleText in RoleText)
                {
                    text[used] += roleText;
                    used++;
                }

                for (int i = 0; i < usedMex; i++)
                {
                    if (string.IsNullOrEmpty(text[i]))
                    {
                        text[i] = "<color=#00000000>占</color>";
                    }
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
        public struct Registry
        {
            public Assembly Assembly { get; }
            public int StartLineNum { get; }
            public int EndLineNum { get; }
            public int LineNum => EndLineNum - StartLineNum + 1;

            public Registry(Assembly assembly, int startLineNum, int endLineNum)
            {
                Assembly = assembly;
                StartLineNum = startLineNum;
                EndLineNum = endLineNum;
            }
        }
    }
}
