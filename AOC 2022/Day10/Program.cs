
var importantCycles = new int[] { 20, 60, 100, 140, 180, 220 };

var aggFinal = (await File.ReadAllLinesAsync("Input.txt"))
    .Aggregate(new Agg { Total = 1, CycleNumber = 1 }, (agg, line) =>
    {
        var cycles = new List<int> { agg.CycleNumber };
        var change = 0;
        if (line != "noop")
        {
            cycles.Add(agg.CycleNumber + 1);
            change = int.Parse(line.Split(' ')[1]);
        }

        Console.WriteLine($"{line}\t\t{agg.Total}\t{string.Join(", ", cycles)}");

        agg.ImportantTotal += importantCycles.Intersect(cycles).DefaultIfEmpty().Sum(c => c * agg.Total);
        agg.CycleNumber = cycles.Max() + 1;
        agg.Total += change;

        return agg;
    });

Console.WriteLine($"Part 1 Output: {aggFinal.ImportantTotal}");


(await File.ReadAllLinesAsync("Input.txt"))
    .Aggregate(new Agg { Total = 1, CycleNumber = 1 }, (agg, line) =>
    {
        var cycles = new List<int> { agg.CycleNumber };
        var change = 0;
        if (line != "noop")
        {
            cycles.Add(agg.CycleNumber + 1);
            change = int.Parse(line.Split(' ')[1]);
        }

        //Console.WriteLine($"{line}\t{agg.ImportantTotal}\t{agg.Total}\t{string.Join(", ", cycles)}");
        foreach (var cycle in cycles)
        {
            var position = (cycle % 40) - 1;

            if (position == 0)
            {
                Console.WriteLine();
            }

            Console.Write(agg.Total - 1 <= position && position <= agg.Total + 1 ? '#' : '.');
        }

        agg.ImportantTotal += importantCycles.Intersect(cycles).DefaultIfEmpty().Sum(c => c * agg.Total);
        agg.CycleNumber = cycles.Max() + 1;
        agg.Total += change;

        return agg;
    });


class Agg
{
    public int ImportantTotal { get; set; }

    public int Total { get; set; }

    public int CycleNumber { get; set; }
}
