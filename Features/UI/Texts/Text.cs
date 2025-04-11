namespace YongAnFrame.Features.UI.Texts
{
    /// <summary>
    /// 给<seealso cref="PlayerUI"/>准备的基础文本
    /// </summary>
    /// <param name="text">内容</param>
    /// <param name="duration">时效</param>
    public class Text(string text, float duration)
    {
        /// <summary>
        /// 获取内容
        /// </summary>
        public string Content { get; } = text;
        /// <summary>
        /// 获取时效
        /// </summary>
        public float Duration { get; internal set; } = duration;
        /// <inheritdoc/>
        public override string ToString() => Content;
        /// <summary>
        /// 隐性转换
        /// </summary>
        /// <param name="text">准备转换的对象</param>
        public static implicit operator string(Text text) => text.ToString();
        /// <summary>
        /// 隐性转换
        /// </summary>
        /// <param name="text">准备转换的对象</param>
        public static implicit operator Text(string text) => new(text, 60);
    }
}
