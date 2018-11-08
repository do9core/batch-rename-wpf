using System.Collections.ObjectModel;
using System.Linq;

namespace do9Rename.Helpers
{
    public class ObservableOrderStack<T> : ObservableCollection<T>
    {
        public T Pop()
        {
            var last = this.Last();
            Remove(last);
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Count"));
            return last;
        }

        public void Push(T item)
        {
            Add(item);
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Count"));
        }
    }
}
