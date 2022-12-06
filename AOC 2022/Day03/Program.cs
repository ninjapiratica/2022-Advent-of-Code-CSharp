
var total = (await File.ReadAllLinesAsync("Input.txt"))
    .Aggregate(0, (acc, line) =>
    {
        var part1 = line[..(line.Length / 2)];
        var part2 = line[(line.Length / 2)..];

        var duplicated = part1.Distinct().Concat(part2.Distinct()).GroupBy(x => x).Where(g => g.Count() > 1).First().Key;

        return acc + (duplicated > 96 ? (duplicated - 96) : (duplicated - 38));
    });

Console.WriteLine($"Part 1 Total: {total}");