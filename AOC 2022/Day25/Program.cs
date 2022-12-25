
var lines = (await File.ReadAllLinesAsync("Input.txt"))
    .Select(ConvertFromSNAFU).ToList();

Console.WriteLine(ConvertToSNAFU(lines.Sum()));

long ConvertFromSNAFU(string snafu)
{
    var sum = 0L;
    for (int i = 0; i < snafu.Length; i++)
    {
        var posValue = (long)Math.Pow(5, snafu.Length - i - 1);
        var value = snafu[i] switch
        {
            '2' => 2L,
            '1' => 1L,
            '0' => 0L,
            '-' => -1L,
            '=' => -2L,
            _ => 0L
        };
        sum += (value * posValue);
    }
    return sum;
}

string ConvertToSNAFU(long value)
{
    var i = 0;
    while (Math.Pow(5, i) * 2.5 < value)
    {
        i++;
    }

    var snafu = "";
    for (int x = i; x >= 0; x--)
    {
        var pow = Math.Pow(5, x);
        var val = Math.Round(value / pow);
        snafu += val switch
        {
            -2 => '=',
            -1 => '-',
            0 => '0',
            1 => '1',
            2 => '2',
            _ => throw new Exception()
        };
        value -= (long)(val * pow);
    }

    return snafu;
}