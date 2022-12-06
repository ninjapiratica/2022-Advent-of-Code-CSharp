
var maxCalories = 0;

var lastElfCalories = (await File.ReadAllLinesAsync("Input.txt"))
    .Aggregate(0, (acc, line) =>
    {
        if (!string.IsNullOrWhiteSpace(line))
        {
            return acc + int.Parse(line);
        }
        else
        {
            maxCalories = Math.Max(maxCalories, acc);
            return 0;
        }
    });

maxCalories = Math.Max(maxCalories, lastElfCalories);

Console.WriteLine($"Part 1 Max Calories: {maxCalories}");



var totalCalories = (await File.ReadAllLinesAsync("Input.txt"))
    .Aggregate(new int[1] { 0 }, (acc, line) =>
    {
        if (!string.IsNullOrWhiteSpace(line))
        {
            acc[acc.Length - 1] += int.Parse(line);
            return acc;
        }
        else
        {
            return acc.Append(0).ToArray();
        }
    })
    .OrderByDescending(x => x)
    .Take(3)
    .Sum();

Console.WriteLine($"Part 2 Total Calories: {totalCalories}");
