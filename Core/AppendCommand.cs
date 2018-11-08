namespace do9Rename.Core
{
    internal class AppendCommand : RenameCommand
    {
        public int Skip { get; set; }
        public string AppendText { get; set; }

        public override string Execute(string input)
        {
            var startIndex = Skip <= input.Length ? Skip : input.Length;
            return input.Insert(startIndex, AppendText);
        }
    }
}
