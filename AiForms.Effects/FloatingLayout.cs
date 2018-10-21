using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;

namespace AiForms.Effects
{
    public class FloatingLayout : View, IList<FloatingView>
    {
        ObservableCollection<FloatingView> _children = new ObservableCollection<FloatingView>();

        public FloatingLayout()
        {
        }

        public void LayoutChildren()
        {
            foreach(var child in _children)
            {
                LayoutChild(child);
            }
        }

        public void LayoutChild(FloatingView child)
        {
            child.Parent = Parent;

            var fWidth = child.ProportionalWidth >= 0 ? Width * child.ProportionalWidth : Width;
            var fHeight = child.ProportionalHeight >= 0 ? Height * child.ProportionalHeight : Height;

            if (child.ProportionalWidth < 0 || child.ProportionalHeight < 0)
            {
                var sizeRequest = child.Measure(fWidth, fHeight);

                var reqWidth = child.ProportionalWidth >= 0 ? fWidth : sizeRequest.Request.Width;
                var reqHeight = child.ProportionalHeight >= 0 ? fHeight : sizeRequest.Request.Height;

                child.Layout(new Rectangle(0, 0, reqWidth, reqHeight));
                return;
            }

            // If both width and height are proportional, Measure is not called.
            child.Layout(new Rectangle(0,0,fWidth, fHeight));
        }

        public FloatingView this[int index] {
            get { return _children[index]; }
            set { _children[index] = value; }
        }

        public int Count => _children.Count;

        public bool IsReadOnly => false;

        public void Add(FloatingView item)
        {
            _children.Add(item);
        }

        public void Clear()
        {
            _children.Clear();
        }

        public bool Contains(FloatingView item)
        {
            return _children.Contains(item);
        }

        public void CopyTo(FloatingView[] array, int arrayIndex)
        {
            _children.CopyTo(array, arrayIndex);
        }

        public IEnumerator<FloatingView> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        public int IndexOf(FloatingView item)
        {
            return _children.IndexOf(item);
        }

        public void Insert(int index, FloatingView item)
        {
            _children.Insert(index, item);
        }

        public bool Remove(FloatingView item)
        {
            return _children.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _children.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _children.GetEnumerator();
        }
    }
}
