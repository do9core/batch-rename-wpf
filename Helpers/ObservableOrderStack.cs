using System.Collections.ObjectModel;
using System.Linq;

namespace do9Rename.Helpers
{
    public class ObservableOrderStack<T> : ObservableCollection<T>
    {
        public T Pop()
        {
            T last = this.Last();
            this.Remove(last);
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Count"));
            return last;
        }

        public void Push(T item)
        {
            this.Add(item);
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Count"));
        }
    }
}
