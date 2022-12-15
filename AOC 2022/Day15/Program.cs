
using System.Drawing;

var lines = await File.ReadAllLinesAsync("Input.txt");

var sensors = new Dictionary<Point, int>();
var beacons = new HashSet<Point>();

var minX = int.MaxValue;
var maxX = int.MinValue;

foreach (var line in lines)
{
    var parts = line.Split(' ');
    var sX = int.Parse(parts[2].Split('=')[1].Replace(",", ""));
    var sY = int.Parse(parts[3].Split('=')[1].Replace(":", ""));

    var bX = int.Parse(parts[8].Split('=')[1].Replace(",", ""));
    var bY = int.Parse(parts[9].Split('=')[1]);

    var totalDistance = Math.Abs(bY - sY) + Math.Abs(bX - sX);

    sensors.Add(new Point(sX, sY), totalDistance);
    beacons.Add(new Point(bX, bY));

    minX = Math.Min(minX, sX - totalDistance);
    maxX = Math.Max(maxX, sX + totalDistance);
}

//var nonBeaconCount = 0;
//for (int i = minX; i <= maxX; i++)
//{
//    var testPoint = new Point(i, 2000000);

//    if (beacons.Contains(testPoint))
//    {
//        continue;
//    }

//    foreach (var kvp in sensors)
//    {
//        var sensor = kvp.Key;
//        var maxDistance = kvp.Value;

//        if (Math.Abs(sensor.X - testPoint.X) + Math.Abs(sensor.Y - testPoint.Y) <= maxDistance)
//        {
//            nonBeaconCount++;
//            break;
//        }
//    }
//}

//Console.WriteLine($"Non Beacons: {nonBeaconCount}");

var possiblePoints = new HashSet<Point>();
foreach (var kvp in sensors)
{
    var sensor = kvp.Key;
    for (int i = 0; i <= kvp.Value; i++)
    {
        possiblePoints.Add(new Point(sensor.X + i, sensor.Y + kvp.Value - i + 1));
        possiblePoints.Add(new Point(sensor.X - i, sensor.Y - kvp.Value + i - 1));
        possiblePoints.Add(new Point(sensor.X + kvp.Value - i + 1, sensor.Y - i));
        possiblePoints.Add(new Point(sensor.X - kvp.Value + i - 1, sensor.Y + i));
    }
}


var searchArea = 4000000;

foreach (var possiblePoint in possiblePoints)
{
    var found = false;
    foreach (var sensor in sensors)
    {
        if (Math.Abs(sensor.Key.X - possiblePoint.X) + Math.Abs(sensor.Key.Y - possiblePoint.Y) <= sensor.Value)
        {
            found = true;
            break;
        }
    }

    if (!found && possiblePoint.X >= 0 && possiblePoint.X <= searchArea && possiblePoint.Y >= 0 && possiblePoint.Y <= searchArea)
    {
        long total = possiblePoint.X * 4000000L + possiblePoint.Y;
        Console.WriteLine($"{possiblePoint} - {total}");
    }
}

//{X=3385922,Y=2671045} - 13543690671045