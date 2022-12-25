
using System.Drawing;

var lines = await File.ReadAllLinesAsync("Input.txt");

var blizzards = lines.SelectMany((l, lix) => l.Select((c, cix) => new Blizzard(cix, lix, c, l.Length, lines.Length)))
    .Where(b => b.Direction != Direction.None)
    .ToList();
var maxX = lines[0].Length - 2;
var maxY = lines.Length - 2;

var start = new Point(lines[0].IndexOf('.'), 0);
var end = new Point(lines.Last().IndexOf('.'), lines.Length - 1);

var paths = new HashSet<Point>() { start };
var time = 0;
while (!paths.Contains(end))
{
    time++;
    var badPoints = blizzards.Select(p => p.Points[time % p.Points.Length]).ToList();
    var possiblePaths = new HashSet<Point>();
    foreach(var path in paths)
    {
        var xyz = new[]
        {
            new Point(path.X -1, path.Y),
            new Point(path.X + 1, path.Y),
            new Point(path.X, path.Y - 1),
            new Point(path.X, path.Y + 1),
            path
        }.Where(x => (x.X >= 1 && x.Y >= 1 && x.X <= maxX && x.Y <= maxY) || x == start || x == end);

        foreach (var pt in xyz.Where(x => !badPoints.Contains(x)))
        {
            possiblePaths.Add(pt);
        }
    }

    paths = possiblePaths;
}

// 159 too low
Console.WriteLine(time);

public class Blizzard
{
    public Blizzard(int x, int y, char dir, int maxX, int maxY)
    {
        maxX -= 2;
        maxY -= 2;

        var max = 0;
        Direction = Direction.None;

        switch (dir)
        {
            case '>':
                Direction = Direction.Right;
                max = maxX;
                break;
            case '<':
                Direction = Direction.Left;
                max = maxX;
                break;
            case '^':
                Direction = Direction.Up;
                max = maxY;
                break;
            case 'v':
                Direction = Direction.Down;
                max = maxY;
                break;
            default:
                break;
        }

        Points = Enumerable.Range(0, max)
            .Select(i => Direction switch
            {
                Direction.Up => new Point(x, y - i <= 0 ? max - (i - y) : y - i),
                Direction.Down => new Point(x, y + i > max ? (i + y) - max : y + i),
                Direction.Left => new Point(x - i <= 0 ? max - (i - x) : x - i, y),
                Direction.Right => new Point(x + i > max ? (i + x) - max : x + i, y),
                _ => new Point(x, y)
            })
            .ToArray();
    }

    public Direction Direction { get; set; }

    public Point[] Points { get; set; }
}

public enum Direction
{
    Up, Down, Left, Right, None
}