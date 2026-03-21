using System;
using System.Collections.Generic;

namespace SamGame.Map.Pathfinding
{
    /// <summary>
    /// A*용 최소 힙 우선순위 큐
    /// </summary>
    public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
    {
        private readonly List<(TElement Element, TPriority Priority)> _heap = new();

        public int Count => _heap.Count;

        public void Enqueue(TElement element, TPriority priority)
        {
            _heap.Add((element, priority));
            BubbleUp(_heap.Count - 1);
        }

        public TElement Dequeue()
        {
            if (_heap.Count == 0)
                throw new InvalidOperationException("Queue is empty");

            var result = _heap[0].Element;
            int lastIndex = _heap.Count - 1;
            _heap[0] = _heap[lastIndex];
            _heap.RemoveAt(lastIndex);

            if (_heap.Count > 0)
                BubbleDown(0);

            return result;
        }

        public void Clear() => _heap.Clear();

        private void BubbleUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (_heap[index].Priority.CompareTo(_heap[parent].Priority) >= 0)
                    break;
                (_heap[index], _heap[parent]) = (_heap[parent], _heap[index]);
                index = parent;
            }
        }

        private void BubbleDown(int index)
        {
            int count = _heap.Count;
            while (true)
            {
                int smallest = index;
                int left = 2 * index + 1;
                int right = 2 * index + 2;

                if (left < count && _heap[left].Priority.CompareTo(_heap[smallest].Priority) < 0)
                    smallest = left;
                if (right < count && _heap[right].Priority.CompareTo(_heap[smallest].Priority) < 0)
                    smallest = right;

                if (smallest == index)
                    break;

                (_heap[index], _heap[smallest]) = (_heap[smallest], _heap[index]);
                index = smallest;
            }
        }
    }
}
