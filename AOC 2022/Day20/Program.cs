
var decryptionKey = 811589153L;

var lines = (await File.ReadAllLinesAsync("Input.txt")).Select(x => new IxValue { Value = int.Parse(x) * decryptionKey }).ToList();
var copy = lines.ToList();

//Console.WriteLine(string.Join(", ", lines.Select(l => l.Value)));

IxValue zeroValue = null;

for (int xxxx = 0; xxxx < 10; xxxx++)
{
    foreach (var item in copy)
    {
        var i = lines.IndexOf(item);

        if (item.Value == 0)
        {
            zeroValue = item;
        }

        lines.RemoveAt(i);

        var position = (int)((i + item.Value) % lines.Count);
        if (position < 0)
        {
            position = lines.Count + position;
        }

        lines.Insert(position, item);
    }

    //Console.WriteLine(string.Join(", ", lines.Select(l => l.Value)));

}


//Console.WriteLine(string.Join(", ", lines.Select(l => l.Value)));

var ix = lines.IndexOf(zeroValue);
var val1 = lines[(ix + 1000) % lines.Count].Value;
var val2 = lines[(ix + 2000) % lines.Count].Value;
var val3 = lines[(ix + 3000) % lines.Count].Value;

Console.WriteLine($"{val1} + {val2} + {val3} = {val1 + val2 + val3}");


class IxValue
{
    public long Value { get; set; }
}

