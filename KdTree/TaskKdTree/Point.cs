using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskKdTree;

public class Point<T> where T : IComparable<T>, IConvertible
{
    /// <summary>
    /// Количество измерений.
    /// </summary>
    public int NumDims { get; }
    
    /// <summary>
    /// Координаты точки.
    /// </summary>
    public List<T> Values { get; set; }

    /// <summary>
    /// Конструктор для создании точки.
    /// </summary>
    /// <param name="values">
    /// Координаты точки.
    /// </param>
    public Point(params T[] values)
    {
        Values = values.ToList();
        NumDims = Values.Count;
    }

    /// <summary>
    /// Возможность индексации по измерениям точки, для того чтобы получить
    /// координату k-го измерения.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public T this[int index]
    {
        get => Values[index];
        set => Values[index] = value;
    }

    public override string ToString()
    {
        return $"{string.Join(" ", Values)}";
    }
}
