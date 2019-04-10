namespace do9Rename.Core
{
    public interface IAppendExtCommand : IRenameCommand
    {
        string Extension { get; set; }
    }
}
