var lines = await File.ReadAllLinesAsync("Input.txt");

var valves = new Dictionary<string, Valve>();
foreach (var line in lines)
{
    var parts = line.Split(' ');
    var valveName = parts[1];
    var rate = int.Parse(parts[4].Split('=')[1].Replace(";", ""));
    var tunnels = parts.Skip(9).Select(x => x.Replace(",", "")).ToList();

    valves[valveName] = new Valve
    {
        Name = valveName,
        Rate = rate,
        ImmediateValves = tunnels
    };
}

foreach (var kvp in valves)
{
    var valve = kvp.Value;

    valve.ValveCosts.Add(valve, 0);

    var i = 1;
    while (valve.ValveCosts.Count < valves.Count)
    {
        var currentValveCosts = valve.ValveCosts.ToList();
        foreach (var kvpCost in currentValveCosts)
        {
            var costValve = kvpCost.Key;

            foreach (var immediateValve in costValve.ImmediateValves)
            {
                var imValve = valves[immediateValve];
                if (!valve.ValveCosts.TryGetValue(imValve, out var x))
                {
                    valve.ValveCosts.Add(imValve, i + (imValve.Rate > 0 ? 1 : 0));
                }
            }
        }

        i++;
    }
}

foreach (var kvp in valves)
{
    foreach (var kvp1 in kvp.Value.ValveCosts.ToList())
    {
        if (kvp1.Key.Rate == 0)
        {
            kvp.Value.ValveCosts.Remove(kvp1.Key);
        }
    }
}

var maxFlow = 0;
var total = 0;

Visit(valves["AA"], 26, new List<(Valve, int)>(), false);

Console.WriteLine("Done");
Console.WriteLine(total);

void Visit(Valve currentValve, int timeRemaining, IEnumerable<(Valve Valve, int TimeRunning)> flowingValves, bool isFinal)
{
    flowingValves = flowingValves.Append((currentValve, timeRemaining));

    var visitedAnother = false;
    foreach (var link in currentValve.ValveCosts.Where(c => c.Value < timeRemaining && !flowingValves.Any(v => v.Valve == c.Key)))
    {
        visitedAnother = true;
        Visit(link.Key, timeRemaining - link.Value, flowingValves, isFinal);
    }

    if (!visitedAnother)
    {
        if (isFinal)
        {
            total++;
            PrintResults(flowingValves);
        }
        else
        {
            Visit(valves["AA"], 26, flowingValves, true);
        }
    }
}

void PrintResults(IEnumerable<(Valve Valve, int TimeRunning)> flowingValves)
{
    var x = flowingValves.Sum(v => v.TimeRunning * v.Valve.Rate);
    if (x > maxFlow)
    {
        maxFlow = x;
        Console.WriteLine($"({x})  {string.Join(" => ", flowingValves.Select(v => $"{v.Valve.Name} ({v.Valve.Rate}) ({v.TimeRunning})"))}");
        Console.WriteLine(total);
    }
}

class Open : Step
{
    public override int Calculate()
    {
        return (Time - 1) * Valve.Rate;
    }

    public override string ToString()
    {
        return $"Open {Valve.Name}";
    }
}

class Move : Step
{
    public Valve MoveTo { get; set; }

    public override int Calculate()
    {
        return 0;
    }

    public override string ToString()
    {
        return $"{MoveTo.Name}";
    }
}

abstract class Step
{
    public int Time { get; set; }

    public Valve Valve { get; set; }

    public abstract int Calculate();
}

class Valve
{
    public string Name { get; set; }
    public int Rate { get; set; }

    public List<string> ImmediateValves { get; set; } = new List<string>();
    public Dictionary<Valve, int> ValveCosts { get; set; } = new Dictionary<Valve, int>();
}