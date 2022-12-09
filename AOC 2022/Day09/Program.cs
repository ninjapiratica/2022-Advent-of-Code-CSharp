
using System.Drawing;

var visited = new HashSet<Point> { new Point(0, 0) };

var lines = await File.ReadAllLinesAsync("Input.txt");

var headPosition = new Point(0, 0);
var tailPosition = new Point(0, 0);

foreach (var line in lines)
{
    var parts = line.Split(' ');

    var distance = int.Parse(parts[1]);
    switch (parts[0])
    {
        case "U":
            for (int i = 0; i < distance; i++)
            {
                headPosition = new Point(headPosition.X, headPosition.Y + 1);
                if (headPosition.Y - tailPosition.Y > 1)
                {
                    tailPosition.Y++;
                    tailPosition.X = headPosition.X;
                    visited.Add(new Point(tailPosition.X, tailPosition.Y));
                }
            }
            break;
        case "D":
            for (int i = 0; i < distance; i++)
            {
                headPosition = new Point(headPosition.X, headPosition.Y - 1);
                if (headPosition.Y - tailPosition.Y < -1)
                {
                    tailPosition.Y--;
                    tailPosition.X = headPosition.X;
                    visited.Add(new Point(tailPosition.X, tailPosition.Y));
                }
            }
            break;
        case "R":
            for (int i = 0; i < distance; i++)
            {
                headPosition = new Point(headPosition.X + 1, headPosition.Y);
                if (headPosition.X - tailPosition.X > 1)
                {
                    tailPosition.X++;
                    tailPosition.Y = headPosition.Y;
                    visited.Add(new Point(tailPosition.X, tailPosition.Y));
                }
            }
            break;
        case "L":
            for (int i = 0; i < distance; i++)
            {
                headPosition = new Point(headPosition.X - 1, headPosition.Y);
                if (headPosition.X - tailPosition.X < -1)
                {
                    tailPosition.X--;
                    tailPosition.Y = headPosition.Y;
                    visited.Add(new Point(tailPosition.X, tailPosition.Y));
                }
            }
            break;
    }
}

Console.WriteLine($"Part 1 Total Visited: {visited.Count}");