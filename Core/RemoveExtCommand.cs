namespace do9Rename.Core
{
    internal class RemoveExtCommand : IRemoveExtCommand
    {
        public string Extension { get; set; }

        public string Execute(string input)
        {
            return !input.EndsWith(Extension) ? 
                input : 
                input.Remove(input.Length - Extension.Length, Extension.Length);
        }
    }
}
