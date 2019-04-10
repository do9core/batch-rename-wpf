namespace do9Rename.Core
{
    internal class AppendCommand : IRenameCommand
    {
        public int Skip { get; set; }
        public string AppendText { get; set; }

        public string Execute(string input)
        {
            var startIndex = Skip <= input.Length ? Skip : input.Length;
            return input.Insert(startIndex, AppendText);
        }
    }
}
