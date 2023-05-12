using TaskKdTree;

Random rnd = new Random();
List<Point<int>> points = new List<Point<int>>();
for (var i = 0; i < 10000; i++)
{
    points.Add(new Point<int>(rnd.Next(0, 1000000000), rnd.Next(0, 1000000000)));
}
KdTree<int> kdTree = new KdTree<int>(points);
//kdTree.Add(new Point<int>(15, 19));
Console.WriteLine(kdTree.NearestNeighbour(new Point<int>(9, 4)));