using System.Linq;

namespace do9Rename.Core
{
    internal class SubstractCommand : IRenameCommand
    {
        public int Skip { get; }
        public int Take { get; }
        public bool IsHeadFirst { get; }

        public SubstractCommand(int skip, int take, bool isHeadFirst)
        {
            this.Skip = skip;
            this.Take = take;
            this.IsHeadFirst = isHeadFirst;
        }

        public string Execute(string input)
        {
            return this.IsHeadFirst ?
                new string(input
                            .Skip(Skip)
                            .Take(Take)
                            .ToArray()) :
                new string(input
                            .Reverse()
                            .Skip(Skip)
                            .Take(Take)
                            .Reverse()
                            .ToArray());
        }

        public override string ToString()
        {
            return "从" + (IsHeadFirst ? "头" : "尾") + $"截取第{Skip}到第{Take}位字符";
        }

        public string ToString(bool isDisplayText = false)
        {
            return isDisplayText ? base.ToString() :
                $"SubstractCommand[Skip({Skip}), Take({Take}), IsHeadFirst({IsHeadFirst})]";
        }
    }
}
