
var lines = await File.ReadAllLinesAsync("Input.txt");

var graph = new Tree[lines.Length, lines[0].Length];

for (int i = 0; i < lines.Length; i++)
{
    var maxHeight = 0;
    for (int j = 0; j < lines[i].Length; j++)
    {
        var height = lines[i][j] - 48;

        graph[i, j] = new Tree
        {
            Height = height,
            IsVisible = (
                i == 0 ||
                j == 0 ||
                i == (lines[0].Length - 1) ||
                j == (lines.Length - 1) ||
                height > maxHeight
                ) ? true : null
        };

        if (maxHeight < height)
        {
            maxHeight = height;
        }
    }

    maxHeight = 0;
    for (int j = lines[i].Length - 1; j >= 0; j--)
    {
        if (maxHeight < graph[i, j].Height)
        {
            maxHeight = graph[i, j].Height;
            graph[i, j].IsVisible = true;
        }
    }
}

for (int j = 0; j < lines[0].Length; j++)
{
    var maxHeight = 0;
    for (int i = 0; i < lines.Length; i++)
    {
        if (maxHeight < graph[i, j].Height)
        {
            maxHeight = graph[i, j].Height;
            graph[i, j].IsVisible = true;
        }
    }

    maxHeight = 0;
    for (int i = lines.Length - 1; i >= 0; i--)
    {
        if (maxHeight < graph[i, j].Height)
        {
            maxHeight = graph[i, j].Height;
            graph[i, j].IsVisible = true;
        }
    }
}

var totalVisible = 0;
for (int i = 0; i < lines.Length; i++)
{
    for (int j = 0; j < lines[0].Length; j++)
    {
        if (graph[i, j].IsVisible == true)
        {
            totalVisible++;
            Console.Write(1);
        }
        else
        {
            Console.Write(0);
        }

    }
    Console.WriteLine();
}

Console.WriteLine($"Part 1 Total Visible: {totalVisible}");


class Tree
{
    public int Height { get; set; }

    public bool? IsVisible { get; set; }
}