
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

Console.WriteLine($"Max Calories: {maxCalories}");
