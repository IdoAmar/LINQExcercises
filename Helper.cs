using System;
using System.Collections.Generic;
using System.Text;

namespace LINQExercises
{
    public static class Helper
    {

        public static IEnumerable<Queue<T>> Window<T>(this IEnumerable<T> enumerable, int count)
        {
            Queue<T> queue = new Queue<T>();
            foreach (var item in enumerable)
            {
                queue.Enqueue(item);
                if (queue.Count > count)
                    queue.Dequeue();
                if(queue.Count == count)
                    yield return queue;
            }
        }
    }
}
