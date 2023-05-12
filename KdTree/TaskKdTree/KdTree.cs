using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskKdTree;

public class KdTree<T> : IKdTree<T> where T : IComparable<T>, IConvertible
{
    /// <summary>
    /// Корневой узел.
    /// </summary>
    public Node<T> RootNode { get; set; }

    /// <summary>
    /// Список со всеми точками.
    /// </summary>
    private List<Point<T>> _points;

    /// <summary>
    /// Размерность данного дерева.
    /// </summary>
    private int _numDims;

    /// <summary>
    /// Создаёт дерево по списку точек.
    /// Выполняется за O(nlog^2n).
    /// </summary>
    /// <param name="points"></param>
    public KdTree(List<Point<T>> points)
    {
        _points = points;
        _numDims = _points[0].NumDims;
        points.Sort((a, b) => a[0].CompareTo(b[0]));
        var median = points[points.Count / 2];
        RootNode = new Node<T>(median);
        var points1 = points.Where(x => median[0].CompareTo(x[0]) > 0).ToList();
        var points2 = points.Where(x => median[0].CompareTo(x[0]) < 0).ToList();
        RootNode.Left = AddRecursive(points1, 1);
        if (RootNode.Left != null)
        {
            RootNode.Left.Parent = RootNode;
        }
        RootNode.Right = AddRecursive(points2, 1);
        if (RootNode.Right != null)
        {
            RootNode.Right.Parent = RootNode;
        }
    }

    /// <summary>
    /// Метод рекурсивно создаёт поддеревья, при этом ищет медианы и возращает их,
    /// для присваяния потомков к родителям.
    /// Выполняется за O(nlogn)
    /// </summary>
    /// <param name="points"></param>
    /// <param name="i"></param>
    /// <returns>
    /// Узел с медианной точкой.
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    private Node<T> AddRecursive(List<Point<T>> points, int i) 
    {
        if (points.Count == 0)
        {
            return null;
        }
        points.Sort((a, b) => a[i].CompareTo(b[i]));
        var median = points[points.Count / 2];
        if (median.NumDims != _numDims)
        {
            throw new ArgumentException();
        }
        var node = new Node<T>(median);
        var points1 = points.Where(x => median[i].CompareTo(x[i]) > 0).ToList();
        var points2 = points.Where(x => median[i].CompareTo(x[i]) < 0).ToList();
        node.Left = AddRecursive(points1, (i + 1) % _numDims);
        if (node.Left != null)
        {
            node.Left.Parent = node;
        }
        node.Right = AddRecursive(points2, (i + 1) % _numDims);
        if (node.Right != null)
        {
            node.Right.Parent = node;
        }
        return node;
    } 

    /// <summary>
    /// Добавляет данную точку в дерево.
    /// Выполняется в среднем за O(logn).
    /// </summary>
    /// <param name="point"></param>
    /// <returns>
    /// true - точка добавлена.
    /// false - точка не добавлена.
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    public bool Add(Point<T> point)
    {
        if (point.NumDims != _numDims)
        {
            throw new ArgumentException();
        }
        var node = RootNode;
        var nodeParent = RootNode.Parent;
        var axis = 0;
        while (node != null)
        {
            for (var i = 0; i < _numDims; i++)
            {
                if (!point[i].Equals(node.Point[i]))
                {
                    break;
                }
                else if (i == _numDims - 1)
                {
                    return false;
                }
            }
            nodeParent = node;
            if (point[axis % _numDims].CompareTo(node.Point[axis % _numDims]) < 0)
            {
                node = node.Left;
            }
            else if (point[axis % _numDims].CompareTo(node.Point[axis % _numDims]) > 0)
            {
                node = node.Right;
            }
            axis++;
        }
        _points.Add(point);
        if (point[(axis - 1) % _numDims].CompareTo(nodeParent!.Point[(axis - 1) % _numDims]) < 0)
        {
            nodeParent.Left = new Node<T>(point);
            nodeParent.Left.Parent = nodeParent;
        }
        else if (point[(axis - 1) % _numDims].CompareTo(nodeParent!.Point[(axis - 1) % _numDims]) > 0)
        {
            nodeParent.Right = new Node<T>(point);
            nodeParent.Right.Parent = nodeParent;
        }
        return true;
    }

    /// <summary>
    /// Данный метод ищет точку.
    /// Метод выполняется в среднем за O(logn)
    /// </summary>
    /// <param name="point"></param>
    /// <returns>
    /// true - в дереве есть эта точка.
    /// false - в дереве нет этой точки.
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    public bool Contains(Point<T> point)
    {
        if (point.NumDims != _numDims)
        {
            throw new ArgumentException();
        }
        var node = RootNode;
        var axis = 0;
        while (node != null)
        {
            for (var i = 0; i < _numDims; i++)
            {
                if (!point[i].Equals(node.Point[i]))
                {
                    break;
                }
                else if (i == _numDims - 1)
                {
                    return true;
                }
            }
            if (point[axis % _numDims].CompareTo(node.Point[axis % _numDims]) < 0)
            {
                node = node.Left;
            }
            else if (point[axis % _numDims].CompareTo(node.Point[axis % _numDims]) > 0)
            {
                node = node.Right;
            }
            axis++;
        }
        return false;
    }

    /// <summary>
    /// Данный метод ищет точку.
    /// Метод выполняется в среднем за O(logn)
    /// </summary>
    /// <param name="point"></param>
    /// <returns>
    /// Узел с данной точкой.
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    public Node<T> Find(Point<T> point)
    {
        if (point.NumDims != _numDims)
        {
            throw new ArgumentException();
        }
        var node = RootNode;
        var axis = 0;
        while (node != null)
        {
            for (var i = 0; i < _numDims; i++)
            {
                if (!point[i].Equals(node.Point[i]))
                {
                    break;
                }
                else if (i == _numDims - 1)
                {
                    return node;
                }
            }
            if (point[axis % _numDims].CompareTo(node.Point[axis % _numDims]) < 0)
            {
                node = node.Left;
            }
            else if (point[axis % _numDims].CompareTo(node.Point[axis % _numDims]) > 0)
            {
                node = node.Right;
            }
            axis++;
        }
        return null;
    }


    /// <summary>
    /// Данный метод ищет ближайшую точку от данной.
    /// Метод выполняется в среднем за O(logn).
    /// </summary>
    /// <param name="point"></param>
    /// <returns>
    /// Ближайшую точку.
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    public Point<T> NearestNeighbour(Point<T> point)
    {
        if (point.NumDims != _numDims)
        {
            throw new ArgumentException();
        }
        var node = RootNode;
        var nodeParent = RootNode.Parent;
        var axis = 0;
        while (node != null)
        {
            for (var i = 0; i < _numDims; i++)
            {
                if (!point[i].Equals(node.Point[i]))
                {
                    break;
                }
                else if (i == _numDims - 1)
                {
                    return point;
                }
            }
            nodeParent = node;
            if (point[axis % _numDims].CompareTo(node.Point[axis % _numDims]) < 0)
            {
                node = node.Left;
            }
            else if (point[axis % _numDims].CompareTo(node.Point[axis % _numDims]) > 0)
            {
                node = node.Right;
            }
            axis++;
        }
        var listPoints = new List<Point<T>>();
        listPoints.Add(nodeParent.Point);
        var minDistance = GetDistance(point, nodeParent.Point);
        if (nodeParent.Left != null)
        {
            GetTreeDistance(nodeParent.Left, point, ref minDistance, listPoints);
        }
        if (nodeParent.Right != null)
        {
            GetTreeDistance(nodeParent.Right, point, ref minDistance, listPoints);
        }
        axis -= 2;
        while (nodeParent.Parent != null)
        {
            if (minDistance > Math.Pow(Subtraction(point[axis % _numDims], nodeParent.Parent.Point[axis % _numDims]), 2))
            {
                listPoints.Add(nodeParent.Parent.Point);
                if (IsLeftParent(nodeParent))
                {
                    GetTreeDistance(nodeParent.Parent.Right, point, ref minDistance, listPoints);
                }
                if (IsRightParent(nodeParent))
                {
                    GetTreeDistance(nodeParent.Parent.Left, point, ref minDistance, listPoints);
                }
            }
            nodeParent = nodeParent.Parent;
            axis--;
        }
        foreach (var potentialPoint in listPoints)
        {
            if (minDistance == GetDistance(point, potentialPoint))
            {
                return potentialPoint;
            }
        }
        return null;
    }

    /// <summary>
    /// Ищет в поддереве точку с минимальным расстоянием от данной точки.
    /// Данный метод имеет сложность O(m), где m - количество точек в поддереве.
    /// </summary>
    /// <param name="nodeChild"></param>
    /// <param name="point"></param>
    /// <param name="minDistance"></param>
    /// <param name="listPoints"></param>
    /// <returns>
    /// Минимальное расстояние от данной точки до какой-то точки в поддереве.
    /// </returns>
    private double GetTreeDistance(Node<T> nodeChild, Point<T> point,  ref double minDistance, List<Point<T>> listPoints)
    {
        if (nodeChild != null)
        {
            minDistance = Math.Min(minDistance, GetDistance(point, nodeChild.Parent.Point));
            minDistance = Math.Min(minDistance, GetDistance(point, nodeChild.Point));
            listPoints.Add(nodeChild.Point);
            if (nodeChild.Left != null)
            {
                minDistance = GetTreeDistance(nodeChild.Left, point, ref minDistance, listPoints);
            }
            if (nodeChild.Right != null)
            {
                minDistance = GetTreeDistance(nodeChild.Right, point, ref minDistance, listPoints);
            }
        }
        return minDistance;
    }

    /// <summary>
    /// Данный метод ищет расстояние между точками в k-мерном пространстве.
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    /// <returns>
    /// Расстояние.
    /// </returns>
    private double GetDistance(Point<T> point1, Point<T> point2)
    {
        double distanceSquared = 0;
        for (var i = 0; i < _numDims; i++) 
        {
            distanceSquared += Subtraction(point1[i], point2[i]) * Subtraction(point1[i], point2[i]);
        }
        return distanceSquared;
    }

    /// <summary>
    /// Удаляет данную точку из дерева, также перестраивает всё дерево.
    /// Данный метод имеет сложность O(nlog^2n)
    /// </summary>
    /// <param name="point"></param>
    /// <returns>
    /// true - точка удалена.
    /// false - такой точки не было.
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    public bool Remove(Point<T> point)
    {
        if (point.NumDims != _numDims)
        {
            throw new ArgumentException();
        }
        if (_points.Remove(point))
        {
            _points.Sort((a, b) => a[0].CompareTo(b[0]));
            var median = _points[_points.Count / 2];
            RootNode = new Node<T>(median);
            var points1 = _points.Where(x => median[0].CompareTo(x[0]) > 0).ToList();
            var points2 = _points.Where(x => median[0].CompareTo(x[0]) < 0).ToList();
            RootNode.Left = AddRecursive(points1, 1);
            if (RootNode.Left != null)
            {
                RootNode.Left.Parent = RootNode;
            }
            RootNode.Right = AddRecursive(points2, 1);
            if (RootNode.Right != null)
            {
                RootNode.Right.Parent = RootNode;
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Проверка на левого потомка.
    /// </summary>
    /// <param name="node">
    /// Узел, который проверяем.
    /// </param>
    /// <returns>
    /// true - это левый потомок.
    /// false - это не левый потомок.
    /// </returns>
    private bool IsLeftParent(Node<T> node)
    {
        if (IsParent(node) && node.Parent!.Left != null && node.Parent!.Left.Equals(node))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Проверка на правого потомка.
    /// </summary>
    /// <param name="node">
    /// Узел, который проверяем.
    /// </param>
    /// <returns>
    /// true - это правый потомок.
    /// false - это не правый потомок.
    /// </returns>
    private bool IsRightParent(Node<T> node)
    {
        if (IsParent(node) && node.Parent!.Right != null && node.Parent!.Right.Equals(node))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Проверка на существование родителя.
    /// </summary>
    /// <param name="node">
    /// Узел, который проверяем.
    /// </param>
    /// <returns>
    /// true - есть родитель.
    /// false - нет родителя.
    /// </returns>
    private bool IsParent(Node<T> node)
    {
        if (node == null || node.Parent == null)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Метод, который позволяет перемножать обобщённый тип данных.
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns>
    /// Произведение.
    /// </returns>
    /// <exception cref="NotSupportedException"></exception>
    public double Multiply(T value1, T value2)
    {
        if (!typeof(T).IsPrimitive || typeof(T) == typeof(bool))
        {
            throw new NotSupportedException("Тип не поддерживает операцию умножения.");
        }

        return (double)(object)(Convert.ToDouble(value1) * Convert.ToDouble(value2));
    }

    /// <summary>
    /// Метод, который позволяет складывать обобщённый тип данных.
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns>
    /// Сумму.
    /// </returns>
    /// <exception cref="NotSupportedException"></exception>
    public double Sum(T value1, T value2)
    {
        if (!typeof(T).IsPrimitive || typeof(T) == typeof(bool))
        {
            throw new NotSupportedException("Тип не поддерживает операцию прибавления.");
        }

        return (double)(object)(Convert.ToDouble(value1) + Convert.ToDouble(value2));
    }

    /// <summary>
    /// Метод, который позволяет вычитать обобщённый тип данных.
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns>
    /// Разность.
    /// </returns>
    /// <exception cref="NotSupportedException"></exception>
    public double Subtraction(T value1, T value2)
    {
        if (!typeof(T).IsPrimitive || typeof(T) == typeof(bool))
        {
            throw new NotSupportedException("Тип не поддерживает операцию вычитания.");
        }

        return (double)(object)(Convert.ToDouble(value1) - Convert.ToDouble(value2));
    }
}
