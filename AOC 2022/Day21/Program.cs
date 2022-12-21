
Monkey rootMonkey = null;

var monkeys = (await File.ReadAllLinesAsync("Input.txt"))
    .Select(l =>
    {
        var parts = l.Split(' ');
        decimal? value = parts.Length == 2 ? decimal.Parse(parts[1]) : null;
        string? leftMonkey = parts.Length > 2 ? parts[1] : null;
        string? rightMonkey = parts.Length > 2 ? parts[3] : null;
        string? operation = parts.Length > 2 ? parts[2] : null;

        var monkey = new Monkey
        {
            Id = parts[0].TrimEnd(':'),
            Value = value,
            LeftMonkey = leftMonkey,
            RightMonkey = rightMonkey,
            Operation = operation
        };

        if (monkey.Id == "root")
        {
            rootMonkey = monkey;
        }

        return monkey;
    })
    .ToDictionary(m => m.Id);

Console.WriteLine(rootMonkey.Calculate(monkeys));

public class Monkey
{
    public string Id { get; set; }

    public decimal? Value { get; set; }

    public string? LeftMonkey { get; set; }

    public string? Operation { get; set; }

    public string? RightMonkey { get; set; }

    public decimal Calculate(Dictionary<string, Monkey> monkeys)
    {
        if (Value.HasValue)
        {
            return Value.Value;
        }

        var l = monkeys[LeftMonkey!];
        var r = monkeys[RightMonkey!];

        Value = Operation switch
        {
            "+" => l.Calculate(monkeys) + r.Calculate(monkeys),
            "-" => l.Calculate(monkeys) - r.Calculate(monkeys),
            "*" => l.Calculate(monkeys) * r.Calculate(monkeys),
            "/" => l.Calculate(monkeys) / r.Calculate(monkeys),
            _ => throw new NotImplementedException()
        };

        return Value.Value;
    }
}