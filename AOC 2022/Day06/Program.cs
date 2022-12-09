
var queue = new Queue<char>();

var startOfPacket = (await File.ReadAllTextAsync("Input.txt"))
    .TakeWhile(value =>
    {
        queue.Enqueue(value);

        if (queue.Count > 4)
        {
            queue.Dequeue();
        }

        if (queue.GroupBy(x => x).Count() == 4)
        {
            return false;
        }

        return true;
    })
    .Count() + 1;

Console.WriteLine($"Part 1 Start of Packet: {startOfPacket}");


