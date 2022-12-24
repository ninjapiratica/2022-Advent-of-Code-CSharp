
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
        if (c == ' ')
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
var sideLength = 50;

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

direction = Direction.Right;
currentPoint = initialPoint;
for (int i = 0; i < numbers.Length; i++)
{
    var move = numbers[i];
    while (move > 0)
    {
        var next = GetNextPointPart2(currentPoint, direction);
        if (next.Point.Value == '.')
        {
            currentPoint = next.Point;
            direction = next.Direction;
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
    
    Console.WriteLine($"{currentPoint.Point} ({direction})");

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
        return (Direction)(direction == 0 ? 3 : (int)direction - 1);
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

(MyPoint Point, Direction Direction) GetNextPointPart2(MyPoint current, Direction direction)
{
    var testPoint = new Point(
        current.Point.X + (direction == Direction.Left ? -1 : direction == Direction.Right ? 1 : 0),
        current.Point.Y + (direction == Direction.Up ? -1 : direction == Direction.Down ? 1 : 0));

    if (points.TryGetValue(testPoint, out var found))
    {
        return (found!, direction);
    }
    else
    {
        if (direction == Direction.Left)
        {
            var area = (current.Point.Y - 1) / sideLength;
            if (area == 0)
            {
                return (points[new Point(1, sideLength + 1 - current.Point.Y + (sideLength * 2))]!, Direction.Right);
            }
            else if (area == 1)
            {
                var xcoord = current.Point.Y % sideLength;
                if (xcoord == 0)
                {
                    xcoord = sideLength;
                }
                return (points[new Point(xcoord, (sideLength * 2) + 1)]!, Direction.Down);
            }
            else if (area == 2)
            {
                return (points[new Point(sideLength + 1, (sideLength * 3) + 1 - current.Point.Y)]!, Direction.Right);
            }
            else
            {
                return (points[new Point(current.Point.Y - (sideLength * 2), 1)]!, Direction.Down);
            }
        }
        else if (direction == Direction.Right)
        {
            var area = (current.Point.Y - 1) / sideLength;
            if (area == 0)
            {
                return (points[new Point(sideLength * 2, sideLength + 1 - current.Point.Y + (sideLength * 2))]!, Direction.Left);
            }
            else if (area == 1)
            {
                return (points[new Point(current.Point.Y + sideLength, sideLength)]!, Direction.Up);
            }
            else if (area == 2)
            {
                return (points[new Point(sideLength * 3, (sideLength * 3) + 1 - current.Point.Y)]!, Direction.Left);
            }
            else
            {
                return (points[new Point(current.Point.Y - (sideLength * 2), sideLength * 3)]!, Direction.Up);
            }
        }
        else if (direction == Direction.Up)
        {
            var area = (current.Point.X - 1) / sideLength;
            if (area == 0)
            {
                return (points[new Point(sideLength + 1, sideLength + current.Point.X)]!, Direction.Right);
            }
            else if (area == 1)
            {
                return (points[new Point(1, current.Point.X + (sideLength * 2))]!, Direction.Right);
            }
            else
            {
                return (points[new Point(current.Point.X % sideLength, sideLength * 4)]!, Direction.Up);
            }
        }
        else // Direction.Down
        {
            var area = (current.Point.X - 1) / sideLength;
            if (area == 0)
            {
                return (points[new Point(current.Point.X + (sideLength * 2), 1)]!, Direction.Down);
            }
            else if (area == 1)
            {
                return (points[new Point(sideLength, current.Point.X + (sideLength * 2))]!, Direction.Left);
            }
            else
            {
                return (points[new Point(sideLength * 2, current.Point.X - sideLength)]!, Direction.Left);
            }
        }
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