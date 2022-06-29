using System;
using System.Linq;

namespace AStar
{
    /// <summary>
    /// This class represents a binary (minimal) heap.
    /// </summary>
    public class BinaryHeap<T> where T : IComparable<T>
    {
        private T[] _nodes;

        /// <summary>
        /// This variable contains the current number of nodes in the binary heap.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// This constructor creates a new binary heap that can contain a maximum of <c>capacity</c> nodes.
        /// When a node is added to a full heap, the capacity is doubled.
        /// </summary>
        public BinaryHeap(int capacity)
        {
            _nodes = new T[capacity];
        }

        /// <summary>
        /// This method adds a node to the binary heap and returns its index.
        /// </summary>
        public void Add(T node)
        {
            if (Size == _nodes.Length) DoubleCapacity();
            _nodes[Size] = node;
            UpHeap(Size++);
        }

        /// <summary>
        /// This method removes and returns the first node of the binary heap, or <c>default</c> if it is empty.
        /// </summary>
        public T RemoveFirst()
        {
            if (Size == 0) return default;
            var node = _nodes[0];
            _nodes[0] = _nodes[--Size];
            DownHeap(0);
            return node;
        }

        /// <summary>
        /// This method checks whether the specified node exists in the binary heap.
        /// </summary>
        public bool Contains(T node)
        {
            return _nodes.Contains(node);
        }

        /// <summary>
        /// This method restores the heap property when the value of the specified node has changed.
        /// </summary>
        /// <param name="node"></param>
        public void Update(T node)
        {
            UpHeap(IndexOf(node));
            DownHeap(IndexOf(node));
        }

        /// <summary>
        /// This method doubles the capacity of the binary heap.
        /// </summary>
        private void DoubleCapacity()
        {
            var tmp = new T[_nodes.Length * 2];
            for (var i = 0; i < _nodes.Length; i++)
            {
                tmp[i] = _nodes[i];
            }

            _nodes = tmp;
        }

        /// <summary>
        /// This method restores the heap property by moving the specified node to the correct position.
        /// It should be used in combination with the method <see cref="Add"/>.
        /// </summary>
        private void UpHeap(int index)
        {
            if (index == 0) return;
            var parent = (index - 1) / 2;
            while (_nodes[index].CompareTo(_nodes[parent]) < 0)
            {
                Swap(index, parent);
                index = parent;
                parent = (index - 1) / 2;
            }
        }

        /// <summary>
        /// This method restores the heap property by moving the specified node to the correct position.
        /// It should be used in combination with the method <see cref="RemoveFirst"/>.
        /// </summary>
        private void DownHeap(int index)
        {
            var one = 2 * index + 1;
            var two = 2 * index + 2;
            while (one < Size)
            {
                var swap = index;
                if (_nodes[swap].CompareTo(_nodes[one]) > 0)
                {
                    swap = one;
                }

                if (two < Size && _nodes[swap].CompareTo(_nodes[two]) > 0)
                {
                    swap = two;
                }

                if (swap != index) Swap(index, swap);
                else return;
            }
        }

        /// <summary>
        /// This method swaps the specified nodes.
        /// </summary>
        private void Swap(int a, int b)
        {
            (_nodes[a], _nodes[b]) = (_nodes[b], _nodes[a]);
        }

        /// <summary>
        /// This method returns the index of the specified node, or <c>-1</c> if it does not exist in the binary heap.
        /// </summary>
        private int IndexOf(T node)
        {
            for (var i = 0; i < Size; i++)
            {
                if (node.Equals(_nodes[i])) return i;
            }

            return -1;
        }
    }
}