
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

struct Point3D
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }
}