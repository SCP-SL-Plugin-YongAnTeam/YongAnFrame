namespace YongAnFrame.Features.UIs.Texts
{
    public class Text(string text, float duration)
    {
        public string Content { get; } = text;
        public float Duration { get; internal set; } = duration;

        /// <inheritdoc/>
        public override string ToString() => Content;
        public static implicit operator string(Text text) => text.ToString();
        public static implicit operator Text(string text) => new(text, 60);
    }
}
