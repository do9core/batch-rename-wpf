namespace do9Rename.Core
{
    class AppendExtOperation : RenameOperation
    {
        public string Extension { get; set; }

        public override string Execute(string input)
        {
            return input.EndsWith(Extension) ? input : input + Extension;
        }
    }
}
