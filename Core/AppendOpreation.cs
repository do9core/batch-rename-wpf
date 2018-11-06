namespace do9Rename.Core
{
    class AppendOpreation : RenameOperation
    {
        public int Skip { get; set; }
        public string AppendText { get; set; }

        public override string Execute(string input)
        {
            int startIndex = Skip <= input.Length ? Skip : input.Length;
            return input.Insert(startIndex, AppendText);
        }
    }
}
