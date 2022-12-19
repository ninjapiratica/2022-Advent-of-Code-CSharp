
var lines = await File.ReadAllLinesAsync("Input.txt");

var points = lines.Select(l =>
{
    var parts = l.Split(',').Select(int.Parse).ToArray();
    return new Point3D { X = parts[0], Y = parts[1], Z = parts[2] };
}).ToHashSet();

var totalNonTouching = 0;
foreach (var point in points)
{
    if (!points.Contains(new Point3D { X = point.X - 1, Y = point.Y, Z = point.Z }))
    {
        totalNonTouching++;
    }

    if (!points.Contains(new Point3D { X = point.X + 1, Y = point.Y, Z = point.Z }))
    {
        totalNonTouching++;
    }

    if (!points.Contains(new Point3D { X = point.X, Y = point.Y + 1, Z = point.Z }))
    {
        totalNonTouching++;
    }

    if (!points.Contains(new Point3D { X = point.X, Y = point.Y - 1, Z = point.Z }))
    {
        totalNonTouching++;
    }

    if (!points.Contains(new Point3D { X = point.X, Y = point.Y, Z = point.Z + 1 }))
    {
        totalNonTouching++;
    }

    if (!points.Contains(new Point3D { X = point.X, Y = point.Y, Z = point.Z - 1 }))
    {
        totalNonTouching++;
    }
}

Console.WriteLine(totalNonTouching);

var visitedPoints = new HashSet<Point3D>();

var maxX = points.Max(x => x.X) + 1;
var maxY = points.Max(x => x.Y) + 1;
var maxZ = points.Max(x => x.Z) + 1;

var pointsToVisit = new List<Point3D> { new Point3D(-1, -1, -1) };
var outsideTouchPoints = 0;

while(pointsToVisit.Count > 0)
{
    pointsToVisit = pointsToVisit
        .SelectMany(Visit)
        .ToList();
}

Console.WriteLine(outsideTouchPoints);

IEnumerable<Point3D> Visit(Point3D point)
{
    if (points.Contains(point))
    {
        outsideTouchPoints++;
        yield break;
    }

    if (visitedPoints.Contains(point))
    {
        yield break;
    }

    if (point.X < -1 || point.X > maxX ||
        point.Y < -1 || point.Y > maxY ||
        point.Z < -1 || point.Z > maxZ)
    {
        yield break;
    }

    visitedPoints.Add(point);

    yield return new Point3D(point.X - 1, point.Y, point.Z);
    yield return new Point3D(point.X + 1, point.Y, point.Z);
    yield return new Point3D(point.X, point.Y - 1, point.Z);
    yield return new Point3D(point.X, point.Y + 1, point.Z);
    yield return new Point3D(point.X, point.Y, point.Z - 1);
    yield return new Point3D(point.X, point.Y, point.Z + 1);
}


struct Point3D
{
    public Point3D(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }
}