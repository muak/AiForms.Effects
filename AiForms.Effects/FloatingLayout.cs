using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;

namespace AiForms.Effects
{
    /// <summary>
    /// Floating layout.
    /// </summary>
    public class FloatingLayout :View, IList<FloatingView>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Effects.FloatingLayout"/> class.
        /// </summary>
        public FloatingLayout()
        {
        }

        /// <summary>
        /// Ons the parent set.
        /// </summary>
        protected override void OnParentSet()
        {
            base.OnParentSet();

            foreach(var child in _children)
            {
                child.Parent = Parent;
            }
        }

        /// <summary>
        /// Layouts the children.
        /// </summary>
        public void LayoutChildren()
        {
            foreach(var child in _children)
            {
                LayoutChild(child);
            }
        }

        /// <summary>
        /// Layouts the child.
        /// </summary>
        /// <param name="child">Child.</param>
        public void LayoutChild(FloatingView child)
        {
            var sizeRequest = child.Measure(Width, Height);

            var finalW = child.HorizontalLayoutAlignment == LayoutAlignment.Fill ? Width : sizeRequest.Request.Width;
            var finalH = child.VerticalLayoutAlignment == LayoutAlignment.Fill ? Height : sizeRequest.Request.Height;

            child.Layout(new Rectangle(0, 0, finalW,finalH));
        }

        ObservableCollection<FloatingView> _children = new ObservableCollection<FloatingView>();

        /// <summary>
        /// Gets or sets the <see cref="T:AiForms.Effects.FloatingLayout"/> at the specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        public FloatingView this[int index] {
            get { return _children[index]; }
            set { _children[index] = value; }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count => _children.Count;

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:AiForms.Effects.FloatingLayout"/> is read only.
        /// </summary>
        /// <value><c>true</c> if is read only; otherwise, <c>false</c>.</value>
        public bool IsReadOnly => false;

        /// <summary>
        /// Add the specified item.
        /// </summary>
        /// <param name="item">Item.</param>
        public void Add(FloatingView item)
        {
            item.Parent = Parent;
            _children.Add(item);
        }

        /// <summary>
        /// Clear this instance.
        /// </summary>
        public void Clear()
        {
            _children.Clear();
        }

        /// <summary>
        /// Contains the specified item.
        /// </summary>
        /// <returns>The contains.</returns>
        /// <param name="item">Item.</param>
        public bool Contains(FloatingView item)
        {
            return _children.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">Array.</param>
        /// <param name="arrayIndex">Array index.</param>
        public void CopyTo(FloatingView[] array, int arrayIndex)
        {
            _children.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<FloatingView> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        /// <summary>
        /// Indexs the of.
        /// </summary>
        /// <returns>The of.</returns>
        /// <param name="item">Item.</param>
        public int IndexOf(FloatingView item)
        {
            return _children.IndexOf(item);
        }

        /// <summary>
        /// Insert the specified index and item.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="item">Item.</param>
        public void Insert(int index, FloatingView item)
        {
            item.Parent = Parent;
            _children.Insert(index, item);
        }

        /// <summary>
        /// Remove the specified item.
        /// </summary>
        /// <returns>The remove.</returns>
        /// <param name="item">Item.</param>
        public bool Remove(FloatingView item)
        {
            item.Parent = null;
            return _children.Remove(item);
        }

        /// <summary>
        /// Removes at index.
        /// </summary>
        /// <param name="index">Index.</param>
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
