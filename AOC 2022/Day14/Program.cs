
using System.Drawing;

var lines = await File.ReadAllLinesAsync("Input.txt");

var rocks = new List<Rock>();
foreach (var line in lines)
{
    var rock = new Rock();
    rocks.Add(rock);
    foreach (var part in line.Split("->"))
    {
        var xy = part.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        rock.AddPoint(new Point(int.Parse(xy[0]), int.Parse(xy[1])));
    }
}

var maxY = rocks.SelectMany(r => r.Points.Select(p => p.Y)).Max();

var ground = new Rock();
ground.AddPoint(new Point(500 - maxY - 5, maxY + 2));
ground.AddPoint(new Point(500 + maxY + 5, maxY + 2));
rocks.Add(ground);

var minX = rocks.SelectMany(r => r.Points.Select(p => p.X)).Min();
var maxX = rocks.SelectMany(r => r.Points.Select(p => p.X)).Max();

var grid = new char[maxX - minX + 3, maxY + 3];

foreach (var rock in rocks)
{
    foreach (var point in rock.Points)
    {
        grid[point.X - minX + 1, point.Y] = '#';
    }
}

for (int i = 0; i < grid.GetLength(0); i++)
{
    for (int j = 0; j < grid.GetLength(1); j++)
    {
        if (grid[i, j] != '#')
        {
            grid[i, j] = '.';
        }
    }
}


var sandPoint = new Point(500 - minX + 1, 0);

Point? restingPoint = null;
var totalSand = 0;
do
{
    restingPoint = DropSand(sandPoint);
    if (restingPoint != null)
    {
        grid[restingPoint.Value.X, restingPoint.Value.Y] = 'O';
        totalSand++;
    }
} while (restingPoint != null);

PrintGrid();
Console.WriteLine($"Total Sand: {totalSand}");

Point? DropSand(Point sandPosition)
{
    var isDropping = false;
    do
    {
        if (sandPosition.Y + 1 == grid.GetLength(1))
        {
            return sandPosition;
        }
        else if (grid[sandPosition.X, sandPosition.Y + 1] == '.')
        {
            sandPosition.Y = sandPosition.Y + 1;
            isDropping = true;
        }
        else if (grid[sandPosition.X - 1, sandPosition.Y + 1] == '.')
        {
            sandPosition.X = sandPosition.X - 1;
            sandPosition.Y = sandPosition.Y + 1;
            isDropping = true;
        }
        else if (grid[sandPosition.X + 1, sandPosition.Y + 1] == '.')
        {
            sandPosition.X = sandPosition.X + 1;
            sandPosition.Y = sandPosition.Y + 1;
            isDropping = true;
        }
        else if (isDropping)
        {
            return sandPosition;
        }
    } while (isDropping);

    if (grid[sandPosition.X, sandPosition.Y] == '.')
    {
        return sandPosition;
    }

    return null;
}

void PrintGrid()
{
    for (int i = 0; i < grid.GetLength(1); i++)
    {
        for (int j = 0; j < grid.GetLength(0); j++)
        {
            Console.Write(grid[j, i]);
        }
        Console.WriteLine();
    }
}

class Rock
{
    public List<Point> Points { get; set; } = new List<Point>();

    public void AddPoint(Point point)
    {
        if (!Points.Any())
        {
            Points.Add(point);
        }
        else
        {
            var tempPoints = new List<Point> { point, Points.Last() }
                .OrderBy(p => p.X)
                .ThenBy(p => p.Y)
                .ToList();

            for (int i = 1; i < tempPoints[1].X - tempPoints[0].X; i++)
            {
                Points.Add(new Point(tempPoints[0].X + i, tempPoints[0].Y));
            }

            for (int i = 1; i < tempPoints[1].Y - tempPoints[0].Y; i++)
            {
                Points.Add(new Point(tempPoints[0].X, tempPoints[0].Y + i));
            }

            Points.Add(point);
        }
    }
}