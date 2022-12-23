
using System.Drawing;

var allLines = await File.ReadAllLinesAsync("Input.txt");

var direction = Direction.Right;
MyPoint initialPoint = null;
var blankIndex = 0;
var points = allLines
    .TakeWhile((l, ix) =>
    {
        blankIndex = ix;
        return l.Length > 0;
    })
    .SelectMany((l, lix) => l.Select((c, cix) =>
    {
        if(c == ' ')
        {
            return null;
        }

        var point = new MyPoint { Point = new Point(cix + 1, lix + 1), Value = c };
        initialPoint ??= point;
        return point;
    }))
    .Where(x => x != null)
    .ToDictionary(x => x.Point);

var maxX = points.Select(x => x.Key.X).Max();
var maxY = points.Select(x => x.Key.Y).Max();

var instructions = allLines[blankIndex + 1];

var numbers = instructions.Split('L', 'R').Select(int.Parse).ToArray();
var letters = instructions.Where(c => c == 'L' || c == 'R').ToArray();

var currentPoint = initialPoint;
for (int i = 0; i < numbers.Length; i++)
{
    var move = numbers[i];
    while (move > 0)
    {
        var nextPoint = GetNextPoint(currentPoint, direction);
        if (nextPoint.Value == '.')
        {
            currentPoint = nextPoint;
            move--;
        }
        else
        {
            break;
        }
    }

    if (letters.Length > i)
    {
        direction = ChangeDirection(direction, letters[i]);
    }
}

Console.WriteLine($"{currentPoint.Point} ({direction}) = {(currentPoint.Point.X * 4 + currentPoint.Point.Y * 1000 + (int)direction)}");

Direction ChangeDirection(Direction direction, char v)
{
    if (v == 'R')
    {
        return (Direction)(((int)direction + 1) % 4);
    }
    else
    {
        return (Direction)(direction == 0 ? 4 : (int)direction - 1);
    }
}

MyPoint GetNextPoint(MyPoint current, Direction direction)
{
    var testPoint = new Point(
        current.Point.X + (direction == Direction.Left ? -1 : direction == Direction.Right ? 1 : 0),
        current.Point.Y + (direction == Direction.Up ? -1 : direction == Direction.Down ? 1 : 0));

    if (points.TryGetValue(testPoint, out var found))
    {
        return found;
    }
    else
    {
        Func<Point, Point> moveTestPoint = default;

        if (direction == Direction.Left)
        {
            testPoint = new Point(maxX, current.Point.Y);
            moveTestPoint = pt => new Point(pt.X - 1, pt.Y);
        }
        else if (direction == Direction.Right)
        {
            testPoint = new Point(0, current.Point.Y);
            moveTestPoint = pt => new Point(pt.X + 1, pt.Y);
        }
        else if (direction == Direction.Up)
        {
            testPoint = new Point(current.Point.X, maxY);
            moveTestPoint = pt => new Point(pt.X, pt.Y - 1);
        }
        else if (direction == Direction.Down)
        {
            testPoint = new Point(current.Point.X, 0);
            moveTestPoint = pt => new Point(pt.X, pt.Y + 1);
        }

        while (!points.TryGetValue(testPoint, out found))
        {
            testPoint = moveTestPoint(testPoint);
        }

        return found;
    }
}

public class MyPoint
{
    public Point Point { get; set; }
    public char Value { get; set; }
}

public enum Direction
{
    Right = 0,
    Down = 1,
    Left = 2,
    Up = 3
}