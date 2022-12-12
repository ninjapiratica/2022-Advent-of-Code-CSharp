var monkeys = new Dictionary<int, Monkey>();

var lines = (await File.ReadAllLinesAsync("Input.txt"));

for (int i = 0; i < lines.Length; i += 7)
{
    var monkey = new Monkey();
    monkeys.Add(i / 7, monkey);
    lines[i + 1].Split(":")[1].Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList().ForEach(monkey.Items.Enqueue);
    monkey.WorryIncrease = HandleOperation(lines[i + 2].Split("=")[1].Split(' ', StringSplitOptions.RemoveEmptyEntries));
    monkey.Test = HandleTest(lines[i + 3].Split(' ').Last());
    monkey.TrueMonkey = int.Parse(lines[i + 4].Split(' ').Last());
    monkey.FalseMonkey = int.Parse(lines[i + 5].Split(' ').Last());
}

var numberOfRounds = 20;
for (int i = 0; i < numberOfRounds; i++)
{
    foreach (var monkey in monkeys)
    {
        Console.WriteLine($"Monkey {monkey.Key}: {monkey.Value}");
    }
    Console.WriteLine();

    PerformRound();
}

Func<long, bool> HandleTest(string v)
{
    var y = long.Parse(v);
    return x => x % y == 0;
}

Func<long, long> HandleOperation(string[] strings)
{
    var isNumber = int.TryParse(strings[2], out var value);
    return strings[1] switch
    {
        "*" => (x) => x * (isNumber ? value : x),
        "+" => (x) => x + (isNumber ? value : x),
        _ => throw new NotImplementedException()
    };
}

void PerformRound()
{
    foreach (var kvp in monkeys)
    {
        var monkey = kvp.Value;
        while (monkey.Items.TryDequeue(out var value))
        {
            value = monkey.WorryIncrease(value);
            value = value / 3;
            if (monkey.Test(value))
            {
                monkeys[monkey.TrueMonkey].Items.Enqueue(value);
            }
            else
            {
                monkeys[monkey.FalseMonkey].Items.Enqueue(value);
            }

            monkey.ItemsInspected++;
        }
    }
}

var top2 = monkeys.Values.OrderByDescending(x => x.ItemsInspected).Take(2).ToArray();

Console.WriteLine($"Part 1 Output: {top2[0].ItemsInspected * top2[1].ItemsInspected}");

class Monkey
{
    public Queue<long> Items { get; set; } = new Queue<long>();

    public Func<long, long> WorryIncrease { get; set; }

    public Func<long, bool> Test { get; set; }

    public int TrueMonkey { get; set; }

    public int FalseMonkey { get; set; }

    public long ItemsInspected { get; set; }

    public override string ToString()
    {
        return $"{ItemsInspected}\t{string.Join(", ", Items)}";
    }
}