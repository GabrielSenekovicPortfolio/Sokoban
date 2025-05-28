using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PriorityQueue<T>
{
    private List<(T item, int priority)> elements = new();
    public int Count => elements.Count;
    public void Enqueue(T item, int priority)
    {
        elements.Add((item, priority)); int ci = elements.Count - 1;
        while (ci > 0)
        {
            int pi = (ci - 1) / 2;
            if (elements[ci].priority >= elements[pi].priority) break;
            (elements[ci], elements[pi]) = (elements[pi], elements[ci]);
            ci = pi;
        }
    }
    public T Dequeue()
    {
        var ret = elements[0].item;
        var last = elements[elements.Count - 1]; elements.RemoveAt(elements.Count - 1);
        if (elements.Count > 0)
        {
            elements[0] = last; int pi = 0;
            while (true)
            {
                int l = 2 * pi + 1, r = 2 * pi + 2, smallest = pi;
                if (l < elements.Count && elements[l].priority < elements[smallest].priority) smallest = l;
                if (r < elements.Count && elements[r].priority < elements[smallest].priority) smallest = r;
                if (smallest == pi) break;
                (elements[pi], elements[smallest]) = (elements[smallest], elements[pi]);
                pi = smallest;
            }
        }
        return ret;
    }
}