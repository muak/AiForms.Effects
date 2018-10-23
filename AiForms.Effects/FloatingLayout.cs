using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;

namespace AiForms.Effects
{
    public class FloatingLayout :View, IList<FloatingView>
    {
        public FloatingLayout()
        {
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();

            foreach(var child in _children)
            {
                child.Parent = Parent;
            }
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
            var sizeRequest = child.Measure(Width, Height);

            var finalW = child.HorizontalLayoutAlignment == LayoutAlignment.Fill ? Width : sizeRequest.Request.Width;
            var finalH = child.VerticalLayoutAlignment == LayoutAlignment.Fill ? Height : sizeRequest.Request.Height;

            child.Layout(new Rectangle(0, 0, finalW,finalH));
        }

        ObservableCollection<FloatingView> _children = new ObservableCollection<FloatingView>();

        public FloatingView this[int index] {
            get { return _children[index]; }
            set { _children[index] = value; }
        }

        public int Count => _children.Count;

        public bool IsReadOnly => false;

        public void Add(FloatingView item)
        {
            item.Parent = Parent;
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
            item.Parent = Parent;
            _children.Insert(index, item);
        }

        public bool Remove(FloatingView item)
        {
            item.Parent = null;
            return _children.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _children[index].Parent = null;
            _children.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _children.GetEnumerator();
        }


    }
}
