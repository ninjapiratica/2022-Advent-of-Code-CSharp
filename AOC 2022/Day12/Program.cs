
using System.Drawing;

var lines = (await File.ReadAllLinesAsync("Input.txt"));

var start = new Point(0, 0);
var end = new Point(0, 0);
var maxX = lines[0].Length;
var maxY = lines.Length;
var grid = new Coordinate[maxX, maxY];

for (int i = 0; i < maxY; i++)
{
    for (int j = 0; j < maxX; j++)
    {
        var value = lines[i][j];
        if (value == 'S')
        {
            value = 'a';
            start = new Point(j, i);
        }
        else if (value == 'E')
        {
            value = 'z';
            end = new Point(j, i);
        }

        grid[j, i] = new Coordinate { Height = value };
    }
}

var sToE = RunSimulation(start);

Console.WriteLine(sToE);

var shortestPath = int.MaxValue;
for (int i = 0; i < maxY; i++)
{
    for (int j = 0; j < maxX; j++)
    {
        if (grid[j, i].Height == 'a')
        {
            Restart();
            shortestPath = Math.Min(shortestPath, RunSimulation(new Point(j, i)));

        }
    }
}

Console.WriteLine(shortestPath);



int RunSimulation(Point startingPoint)
{
    var steppers = new List<Stepper?>
    {
        new Stepper(null, startingPoint)
    };

    grid[startingPoint.X, startingPoint.Y].ShortestPath = 0;

    while (steppers.Any() && !steppers.Any(s => s.Current == end))
    {
        foreach (var stepper in steppers.ToList())
        {
            stepper?.Step(steppers, grid);
        }
    }

    var xStepper = steppers.FirstOrDefault(s => s.Current == end);

    var pathLength = xStepper != null ? xStepper.Path.Count() - 1 : int.MaxValue;

    if (pathLength == 525)
    {
        for (int i = 0; i < grid.GetLength(1); i++)
        {
            for (int j = 0; j < grid.GetLength(0); j++)
            {
                Console.Write(xStepper.Path.Contains(new Point(j, i)) ? 'x' : '.');
            }

            Console.WriteLine();
        }
    }

    // steps is total path length - starting location
    return pathLength;
}



void Restart()
{
    for (int i = 0; i < grid.GetLength(1); i++)
    {
        for (int j = 0; j < grid.GetLength(0); j++)
        {
            grid[j, i].ShortestPath = null;
        }
    }
}



//TryHandlePosition(out var final);

//Console.WriteLine($"Part 1 Result: {final}");


//bool TryHandlePosition(out int steps)
//{
//    if (current == end)
//    {
//        steps = 0;
//        return true;
//    }

//    var minSteps = int.MaxValue;
//    if (TryMove(new Point(current.X - 1, current.Y), out var left))
//    {
//        minSteps = Math.Min(minSteps, left);
//    }

//    if (TryMove(new Point(current.X + 1, current.Y), out var right))
//    {
//        minSteps = Math.Min(minSteps, right);
//    }

//    if (TryMove(new Point(current.X, current.Y - 1), out var up))
//    {
//        minSteps = Math.Min(minSteps, up);
//    }

//    if (TryMove(new Point(current.X, current.Y + 1), out var down))
//    {
//        minSteps = Math.Min(minSteps, down);
//    }

//    if (minSteps < int.MaxValue)
//    {
//        steps = minSteps;
//        return true;
//    }

//    steps = 0;
//    return false;
//}

//bool TryMove(Point newPoint, out int steps)
//{
//    var oldCurrent = current;
//    if (CanMove(current, newPoint))
//    {
//        grid[newPoint.X, newPoint.Y].Visited = true;
//        current = newPoint;
//        var handled = TryHandlePosition(out steps);
//        grid[newPoint.X, newPoint.Y].Visited = false;
//        current = oldCurrent;

//        steps++;
//        return handled;
//    }

//    steps = 0;
//    return false;
//}

class Stepper
{
    private readonly Stepper? previousStepper;

    public IEnumerable<Point> Path
    {
        get
        {
            yield return Current;
            var pre = previousStepper;
            while (pre != null)
            {
                yield return pre.Current;
                pre = pre.previousStepper;
            }
        }
    }

    public Point Current { get; }

    public Stepper(Stepper? previousStepper, Point point)
    {
        this.previousStepper = previousStepper;
        this.Current = point;
    }

    public void Step(List<Stepper?> steppers, Coordinate[,] map)
    {
        var newSteppers = new List<Stepper?>
        {
            PerformMove(new Point(Current.X - 1, Current.Y), map),
            PerformMove(new Point(Current.X + 1, Current.Y), map),
            PerformMove(new Point(Current.X, Current.Y - 1), map),
            PerformMove(new Point(Current.X, Current.Y + 1), map),
        };

        steppers.Remove(this);
        steppers.AddRange(newSteppers.Where(ns => ns != null));
    }

    Stepper? PerformMove(Point newPoint, Coordinate[,] map)
    {
        if (CanMove(newPoint, map))
        {
            map[newPoint.X, newPoint.Y].ShortestPath = Path.Count();
            return new Stepper(this, newPoint);
        }

        return null;
    }

    bool CanMove(Point newPoint, Coordinate[,] map)
    {
        var maxX = map.GetLength(0);
        var maxY = map.GetLength(1);

        if (newPoint.X >= 0 &&
            newPoint.X < maxX &&
            newPoint.Y >= 0 &&
            newPoint.Y < maxY &&
            map[newPoint.X, newPoint.Y].Height <= (map[Current.X, Current.Y].Height + 1))
        {
            return !map[newPoint.X, newPoint.Y].ShortestPath.HasValue;
        }

        return false;
    }
}

public class Coordinate
{
    public int Height { get; set; }

    public int? ShortestPath { get; set; }
}