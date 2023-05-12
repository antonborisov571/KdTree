using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskKdTree;

public interface IKdTree<T> where T : IComparable<T>, IConvertible
{
    Node<T> RootNode { get; set; }
    bool Add(Point<T> point);
    bool Remove(Point<T> point);
    bool Contains(Point<T> point);
    Point<T> NearestNeighbour(Point<T> point);
    Node<T> Find(Point<T> point);
}
