namespace do9Rename.Core
{
    internal class AppendCommand : IRenameCommand
    {
        public int Skip { get; }
        public string AppendText { get; }
        public bool IsHeadFirst { get; }

        public AppendCommand(int skip, string appendText, bool isHeadFirst)
        {
            this.Skip = skip;
            this.AppendText = appendText;
            this.IsHeadFirst = isHeadFirst;
        }

        public string Execute(string input)
        {
            var startIndex = Skip <= input.Length ? Skip : input.Length;
            return IsHeadFirst ? input.Insert(startIndex, AppendText) :
                                 input.Insert(input.Length - startIndex, AppendText);
        }

        public override string ToString()
        {
            return $"从第{Skip}位追加内容{AppendText}";
        }

        public string ToString(bool isDisplayText = false)
        {
            return isDisplayText ? base.ToString() :
                $"AppendCommand[Skip({Skip}), AppendText({AppendText}), IsHeadFirst({IsHeadFirst})]";
        }
    }
}
