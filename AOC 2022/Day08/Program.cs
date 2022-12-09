
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


for (int i = 0; i < lines.Length; i++)
{
    for (int j = 0; j < lines[i].Length; j++)
    {
        graph[i, j].Score = LookUp(graph[i, j], i, j) *
            LookRight(graph[i, j], i, j) *
            LookDown(graph[i, j], i, j) *
            LookLeft(graph[i, j], i, j);
    }
}

long highestScore = 0;
for (int i = 0; i < lines.Length; i++)
{
    for (int j = 0; j < lines[0].Length; j++)
    {
        highestScore = Math.Max(graph[i, j].Score, highestScore);
        Console.Write(graph[i, j].Score + "\t");
    }
    Console.WriteLine();
}

Console.WriteLine($"Part 2 Scenic Score: {highestScore}");

long LookUp(Tree tree, int i, int j)
{
    var count = 0;
    while (i > 0)
    {
        count++;
        if (graph[i - 1, j].Height < tree.Height)
        {
            i--;
        }
        else
        {
            break;
        }
    }
    return count;
}

long LookRight(Tree tree, int i, int j)
{
    var count = 0;
    while (j < lines[0].Length - 1)
    {
        count++;
        if (graph[i, j + 1].Height < tree.Height)
        {
            j++;
        }
        else
        {
            break;
        }
    }
    return count;
}

long LookDown(Tree tree, int i, int j)
{
    var count = 0;
    while (i < lines.Length - 1)
    {
        count++;
        if (graph[i + 1, j].Height < tree.Height)
        {
            i++;
        }
        else
        {
            break;
        }
    }
    return count;
}

long LookLeft(Tree tree, int i, int j)
{
    var count = 0;
    while (j > 0)
    {
        count++;
        if (graph[i, j - 1].Height < tree.Height)
        {
            j--;
        }
        else
        {
            break;
        }
    }
    return count;
}

class Tree
{
    public int Height { get; set; }

    public bool? IsVisible { get; set; }

    public long Score { get; set; }
}