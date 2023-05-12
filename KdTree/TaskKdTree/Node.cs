using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskKdTree;

public class Node<T> where T : IComparable<T>, IConvertible
{
    /// <summary>
    /// Конструктор, который может понадобится когда мы знаем: родителя, левый потомок, правый потомок.
    /// </summary>
    /// <param name="point">
    /// Точка.
    /// </param>
    /// <param name="parent">
    /// Родитель.
    /// </param>
    /// <param name="left">
    /// Левый потомок.
    /// </param>
    /// <param name="right">
    /// Правый потомок.
    /// </param>
    public Node(Point<T> point, Node<T>? parent, Node<T>? left, Node<T>? right)
    {
        Point = point;
        Parent = parent;
        if (IsLeftParent(this))
        {
            Parent.Left = this;
        }
        if (IsRightParent(this))
        {
            Parent.Right = this;
        }
        Left = left;
        if (left != null)
        {
            Left!.Parent = this;
        }
        Right = right;
        if (right != null)
        {
            Right!.Parent = this;
        }
    }

    /// <summary>
    /// Конструктор, который может понадобится когда мы знаем только родителя.
    /// </summary>
    /// <param name="point">
    /// Точка.
    /// </param>
    public Node(Point<T> point)
    {
        Point = point;
    }

    /// <summary>
    /// Точка в k-мерном пространстве
    /// </summary>
    public Point<T> Point { get; set; }

    /// <summary>
    /// Родитель.
    /// </summary>
    public Node<T>? Parent { get; set; }
    
    /// <summary>
    /// Левый потомок.
    /// </summary>
    public Node<T>? Left { get; set; }

    /// <summary>
    /// Правый потомок.
    /// </summary>
    public Node<T>? Right { get; set; }

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
}
