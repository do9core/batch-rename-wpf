using System.Collections.ObjectModel;
using System.Linq;

namespace do9Rename.Core
{
    public class ObservableOrderStack<T> : ObservableCollection<T>
    {
        public T Pop()
        {
            var last = this.Last();
            Remove(last);
            return last;
        }

        public void Push(T item)
        {
            this.Add(item);
        }
    }
}
