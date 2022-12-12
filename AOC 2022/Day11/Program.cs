using System.Numerics;

var monkeys = new Dictionary<int, Monkey>();

var lines = (await File.ReadAllLinesAsync("Input.txt"));

for (int i = 0; i < lines.Length; i += 7)
{
    var monkey = new Monkey();
    monkeys.Add(i / 7, monkey);
    lines[i + 1].Split(":")[1].Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).Select(BigInteger.Parse).ToList().ForEach(monkey.Items.Enqueue);
    monkey.WorryIncrease = HandleOperation(lines[i + 2].Split("=")[1].Split(' ', StringSplitOptions.RemoveEmptyEntries));
    monkey.TestValue = int.Parse(lines[i + 3].Split(' ').Last());
    monkey.TrueMonkey = int.Parse(lines[i + 4].Split(' ').Last());
    monkey.FalseMonkey = int.Parse(lines[i + 5].Split(' ').Last());
}

var numberOfRounds = 10000;
for (int i = 0; i < numberOfRounds; i++)
{
    PerformRound(i + 1);

    if (i + 1 == 1 || i + 1 == 20 || ((i + 1) % 1000) == 0)
    {
        Console.WriteLine($"== After Round {i + 1} ==");
        foreach (var monkey in monkeys)
        {
            Console.WriteLine($"Monkey {monkey.Key} inspected items {monkey.Value.ItemsInspected} times. {monkey.Value}");
        }
    }
}

Func<BigInteger, BigInteger> HandleOperation(string[] strings)
{
    var isNumber = int.TryParse(strings[2], out var value);
    return strings[1] switch
    {
        "*" => (x) => x * (isNumber ? value : x),
        "+" => (x) => x + (isNumber ? value : x),
        _ => throw new NotImplementedException()
    };
}

void PerformRound(int round)
{
    foreach (var kvp in monkeys)
    {
        var monkey = kvp.Value;
        while (monkey.Items.TryDequeue(out var value))
        {
            var prevValue = value;
            value = monkey.WorryIncrease(value);

            //value = value / 3;

            value = ModifyValue(value);
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

BigInteger ModifyValue(BigInteger value)
{
    var x = monkeys.Aggregate(1, (agg, m) => m.Value.TestValue * agg);

    if (value % x > 0)
    {
        return value % x;
    }
    else
    {
        return x;
    }
}

var top2 = monkeys.Values.OrderByDescending(x => x.ItemsInspected).Take(2).ToArray();

Console.WriteLine($"Part 2 Output: {top2[0].ItemsInspected * top2[1].ItemsInspected}");


class Monkey
{
    public Queue<BigInteger> Items { get; set; } = new Queue<BigInteger>();

    public Func<BigInteger, BigInteger> WorryIncrease { get; set; }

    public int TestValue { get; set; }

    public int TrueMonkey { get; set; }

    public int FalseMonkey { get; set; }

    public BigInteger ItemsInspected { get; set; }

    public bool Test(BigInteger value)
    {
        return value % TestValue == 0;
    }

    public override string ToString()
    {
        return $"{ItemsInspected}\t{string.Join(", ", Items)}";
    }
}