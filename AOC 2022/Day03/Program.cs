var total = (await File.ReadAllLinesAsync("Input.txt"))
    .Aggregate(0, (acc, line) =>
    {
        var part1 = line[..(line.Length / 2)];
        var part2 = line[(line.Length / 2)..];

        var duplicated = part1.Distinct().Concat(part2.Distinct()).GroupBy(x => x).Where(g => g.Count() > 1).First().Key;

        return acc + (duplicated > 96 ? (duplicated - 96) : (duplicated - 38));
    });

Console.WriteLine($"Part 1 Total: {total}");



var badgePriority = (await File.ReadAllLinesAsync("Input.txt"))
    .Aggregate(new Accumulator(), (acc, line) =>
    {
        acc.CurrentGroup.Add(line);

        if (acc.CurrentGroup.Count < 3)
        {
            return acc;
        }

        var duplicated = acc.CurrentGroup[0]
            .Join(acc.CurrentGroup[1], x => x, y => y, (x, y) => x)
            .Join(acc.CurrentGroup[2], x => x, y => y, (x, y) => x)
            .First();

        acc.Priority += duplicated > 96 ? (duplicated - 96) : (duplicated - 38);

        acc.CurrentGroup.Clear();

        return acc;
    })
    .Priority;

Console.WriteLine($"Part 1 Total: {badgePriority}");

class Accumulator
{
    public int Priority { get; set; }

    public List<string> CurrentGroup { get; } = new List<string>();
}