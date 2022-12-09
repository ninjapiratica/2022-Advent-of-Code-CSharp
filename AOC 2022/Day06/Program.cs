
var packetQueue = new Queue<char>();
var distinctSOPCount = 4;
var startOfPacket = (await File.ReadAllTextAsync("Input.txt"))
    .TakeWhile(value =>
    {
        packetQueue.Enqueue(value);

        if (packetQueue.Count > distinctSOPCount)
        {
            packetQueue.Dequeue();
        }

        if (packetQueue.GroupBy(x => x).Count() == distinctSOPCount)
        {
            return false;
        }

        return true;
    })
    .Count() + 1;

Console.WriteLine($"Part 1 Start of Packet: {startOfPacket}");



var messageQueue = new Queue<char>();
var distinctSOMCount = 14;
var startOfMessage = (await File.ReadAllTextAsync("Input.txt"))
    .TakeWhile(value =>
    {
        messageQueue.Enqueue(value);

        if (messageQueue.Count > distinctSOMCount)
        {
            messageQueue.Dequeue();
        }

        if (messageQueue.GroupBy(x => x).Count() == distinctSOMCount)
        {
            return false;
        }

        return true;
    })
    .Count() + 1;

Console.WriteLine($"Part 2 Start of Message: {startOfMessage}");