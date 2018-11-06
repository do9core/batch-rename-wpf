namespace do9Rename.Core
{
    class DoNothingOpreation : RenameOperation
    {
        public override string Execute(string input)
        {
            return input;
        }
    }
}
