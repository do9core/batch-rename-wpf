namespace do9Rename.Core
{
    internal class RemoveExtCommand : RenameCommand
    {
        public string Extension { get; set; }

        public override string Execute(string input)
        {
            return !input.EndsWith(Extension) ? 
                input : 
                input.Remove(input.Length - Extension.Length, Extension.Length);
        }
    }
}
