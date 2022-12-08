var completeEncompassTotal = (await File.ReadAllLinesAsync("Input.txt"))
    .Aggregate(0, (acc, line) =>
    {
        var parts = line.Split('-', ',').Select(x => int.Parse(x)).ToList();

        if (parts[0] <= parts[2] && parts[1] >= parts[3])
        {
            return acc + 1;
        }
        else if (parts[2] <= parts[0] && parts[3] >= parts[1])
        {
            return acc + 1;
        }

        return acc;

    });

Console.WriteLine($"Part 1 Total: {completeEncompassTotal}");


var overlapTotal = (await File.ReadAllLinesAsync("Input.txt"))
    .Aggregate(0, (acc, line) =>
    {
        var parts = line.Split('-', ',').Select(x => int.Parse(x)).ToList();

        if (parts[0] <= parts[3] && parts[1] >= parts[2])
        {
            return acc + 1;
        }

        return acc;

    });

Console.WriteLine($"Part 2 Total: {overlapTotal}");