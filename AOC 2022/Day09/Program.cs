
using System.Drawing;

var visited = new HashSet<Point> { new Point(0, 0) };

var lines = await File.ReadAllLinesAsync("Input.txt");

var knots = new List<Point>();
for (int i = 0; i < 10; i++)
{
    knots.Add(new Point(0, 0));
}

foreach (var line in lines)
{
    var parts = line.Split(' ');

    var distance = int.Parse(parts[1]);
    switch (parts[0])
    {
        case "U":
            for (int i = 0; i < distance; i++)
            {
                knots[0] = new Point(knots[0].X, knots[0].Y + 1);

                DoWork();
            }
            break;
        case "D":
            for (int i = 0; i < distance; i++)
            {
                knots[0] = new Point(knots[0].X, knots[0].Y - 1);

                DoWork();
            }
            break;
        case "R":
            for (int i = 0; i < distance; i++)
            {
                knots[0] = new Point(knots[0].X + 1, knots[0].Y);

                DoWork();
            }
            break;
        case "L":
            for (int i = 0; i < distance; i++)
            {
                knots[0] = new Point(knots[0].X - 1, knots[0].Y);

                DoWork();
            }
            break;
    }
}

Console.WriteLine($"Part 1 Total Visited: {visited.Count}");

void DoWork()
{
    for (int j = 1; j < knots.Count; j++)
    {
        var previous = knots[j - 1];
        var current = knots[j];
        var yDiff = previous.Y - current.Y;
        var xDiff = previous.X - current.X;

        if (yDiff > 1)
        {
            current.Y++;

            if (current.X < previous.X)
            {
                current.X++;
            }
            else if (current.X > previous.X)
            {
                current.X--;
            }
        }
        else if (yDiff < -1)
        {
            current.Y--;

            if (current.X < previous.X)
            {
                current.X++;
            }
            else if (current.X > previous.X)
            {
                current.X--;
            }
        }
        else if (xDiff > 1)
        {
            current.X++;

            if (current.Y < previous.Y)
            {
                current.Y++;
            }
            else if (current.Y > previous.Y)
            {
                current.Y--;
            }
        }
        else if (xDiff < -1)
        {
            current.X--;

            if (current.Y < previous.Y)
            {
                current.Y++;
            }
            else if (current.Y > previous.Y)
            {
                current.Y--;
            }
        }

        knots[j] = current;

        if (j == knots.Count - 1)
        {
            visited.Add(new Point(current.X, current.Y));
        }
    }
}