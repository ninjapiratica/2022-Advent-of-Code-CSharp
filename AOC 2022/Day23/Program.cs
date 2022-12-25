
using System.Drawing;

var lines = await File.ReadAllLinesAsync("Input.txt");

var elves = lines.SelectMany((l, lix) => l.Select((c, cix) => c == '#' ? new Point(cix, lix) : (Point?)null))
    .Where(p => p != null)
    .Select(p => p.Value)
    .ToHashSet();

var propGroups = new[]
{
    new[] { 0, 1, 2 },
    new[] { 4, 5, 6 },
    new[] { 6, 7, 0 },
    new[] { 2, 3, 4 },
};

var i = 0;

while (true)
{
    var proposals = new Dictionary<Point, Point>();
    var deadPoints = new HashSet<Point>();

    foreach (var elf in elves)
    {
        var nearbyElves = new[] {
            elves.Contains(new Point(elf.X - 1, elf.Y - 1)),
            elves.Contains(new Point(elf.X, elf.Y - 1)),
            elves.Contains(new Point(elf.X + 1, elf.Y - 1)),
            elves.Contains(new Point(elf.X + 1, elf.Y)),
            elves.Contains(new Point(elf.X + 1, elf.Y + 1)),
            elves.Contains(new Point(elf.X, elf.Y + 1)),
            elves.Contains(new Point(elf.X - 1, elf.Y + 1)),
            elves.Contains(new Point(elf.X - 1, elf.Y))
        };

        if (nearbyElves.Any(x => x))
        {
            var group = i % 4;
            for (int g = 0; g < 4; g++)
            {
                if (!propGroups[group].Any(x => nearbyElves[x]))
                {
                    var propPoint = group switch
                    {
                        0 => new Point(elf.X, elf.Y - 1),
                        1 => new Point(elf.X, elf.Y + 1),
                        2 => new Point(elf.X - 1, elf.Y),
                        3 => new Point(elf.X + 1, elf.Y),
                        _ => elf
                    };

                    if (!deadPoints.Contains(propPoint))
                    {
                        if (proposals.ContainsKey(propPoint))
                        {
                            proposals.Remove(propPoint);
                            deadPoints.Add(propPoint);
                        }
                        else
                        {
                            proposals.Add(propPoint, elf);
                        }
                    }
                    break;
                }
                else
                {
                    group = (group + 1) % 4;
                }
            }
        }
    }

    foreach (var proposal in proposals.Values)
    {
        elves.Remove(proposal);
    }

    foreach (var proposal in proposals.Keys)
    {
        elves.Add(proposal);
    }

    i++;

    if (i == 10)
    {
        var minX = elves.Min(e => e.X);
        var maxX = elves.Max(e => e.X);
        var minY = elves.Min(e => e.Y);
        var maxY = elves.Max(e => e.Y);

        Console.WriteLine(((maxX - minX + 1) * (maxY - minY + 1)) - elves.Count);
    }

    if(proposals.Count == 0)
    {
        Console.WriteLine(i);
        break;
    }

}

