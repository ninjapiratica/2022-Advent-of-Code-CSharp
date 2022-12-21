
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
            monkey.Operation = "=";
        }

        if (monkey.Id == "humn")
        {
            monkey.Value = null;
        }

        return monkey;
    })
    .ToDictionary(m => m.Id);

rootMonkey.Calculate(monkeys);

Console.WriteLine(rootMonkey.Solve(monkeys, null));

public class Monkey
{
    public string Id { get; set; }

    public decimal? Value { get; set; }

    public string? LeftMonkey { get; set; }

    public string? Operation { get; set; }

    public string? RightMonkey { get; set; }

    public decimal? Calculate(Dictionary<string, Monkey> monkeys)
    {
        if (string.IsNullOrWhiteSpace(LeftMonkey))
        {
            return Value;
        }

        var l = monkeys[LeftMonkey!];
        var r = monkeys[RightMonkey!];

        Value = Operation switch
        {
            "+" => l.Calculate(monkeys) + r.Calculate(monkeys),
            "-" => l.Calculate(monkeys) - r.Calculate(monkeys),
            "*" => l.Calculate(monkeys) * r.Calculate(monkeys),
            "/" => l.Calculate(monkeys) / r.Calculate(monkeys),
            "=" => (l.Calculate(monkeys) + r.Calculate(monkeys)) * null,
            _ => throw new NotImplementedException()
        };

        return Value;
    }

    public decimal? Solve(Dictionary<string, Monkey> monkeys, decimal? value)
    {
        if (Value.HasValue)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(LeftMonkey))
        {
            return value;
        }

        var l = monkeys[LeftMonkey!];
        var r = monkeys[RightMonkey!];

        switch (Operation)
        {
            case "+":
                if (!l.Value.HasValue)
                {
                    return l.Solve(monkeys, value - r.Value);
                }
                else if (!r.Value.HasValue)
                {
                    return r.Solve(monkeys, value - l.Value);
                }
                break;
            case "-":
                if (!l.Value.HasValue)
                {
                    return l.Solve(monkeys, value + r.Value);
                }
                else if (!r.Value.HasValue)
                {
                    return r.Solve(monkeys, l.Value - value);
                }
                break;
            case "*":
                if (!l.Value.HasValue)
                {
                    return l.Solve(monkeys, value / r.Value);
                }
                else if (!r.Value.HasValue)
                {
                    return r.Solve(monkeys, value / l.Value);
                }
                break;
            case "/":
                if (!l.Value.HasValue)
                {
                    return l.Solve(monkeys, value * r.Value);
                }
                else if (!r.Value.HasValue)
                {
                    return r.Solve(monkeys, l.Value / value);
                }
                break;
            case "=":
                if (!l.Value.HasValue)
                {
                    return l.Solve(monkeys, r.Value);
                }
                else if (!r.Value.HasValue)
                {
                    return r.Solve(monkeys, l.Value);
                }
                break;
            default:
                return null;
        }

        return null;
    }
}