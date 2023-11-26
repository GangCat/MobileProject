using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// IParsable이 있어야 데이터 분류할 때 감지해서 여러 동작을 수행할 수 있음.
[Serializable]
public class IntList :IParsable, IList<int>
{
    public List<int> values;
    public void FillFromStr(string str)
    {
        values = str.Split(',').Select(l => int.Parse(l)).ToList();
    }

    public int this[int index]
    {
        get => values[index];
        set => values[index] = value;
    }

    public int Count => values.Count;

    public bool IsReadOnly => false;

    public void Add(int item)
    {
        values.Add(item);
    }

    public void Clear()
    {
        values.Clear();
    }

    public bool Contains(int item)
    {
        return values.Contains(item);
    }

    public void CopyTo(int[] array, int arrayIndex)
    {
        values.CopyTo(array, arrayIndex);
    }

   

    public IEnumerator<int> GetEnumerator()
    {
        return values.GetEnumerator();
    }

    public int IndexOf(int item)
    {
        return values.IndexOf(item);
    }

    public void Insert(int index, int item)
    {
        values.Insert(index, item);
    }

    public bool Remove(int item)
    {
        return values.Remove(item);
    }

    public void RemoveAt(int index)
    {
        values.RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}