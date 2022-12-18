var blowDirections = await File.ReadAllTextAsync("Input.txt");

var blowIndex = -1;
var shapeIndex = -1;
var maxHeight = 0L;

var numberOfRocks = 1_000_000_000_000;

var columns = new bool[7, 500_000_000];

for (int i = 0; i < columns.GetLength(0); i++)
{
    columns[i, 0] = true;
}

var resets = new Dictionary<long, (long Height, long RockNumber)>() { };
var heights = new Dictionary<long, long>();

for (long i = 0; i < numberOfRocks; i++)
{
    var shape = GetNextShape();

    var dir = GetNextBlowDirection();

    if (i > 100000 && IsReset(shape))
    {
        if (resets.ContainsKey(blowIndex))
        {
            var heightPerRepeat = maxHeight - resets[blowIndex].Height;
            var rocksInRepeat = i - resets[blowIndex].RockNumber;
            var rocksBeforeRepeat = resets[blowIndex].RockNumber;
            var remainingRocks = (numberOfRocks - rocksBeforeRepeat) % rocksInRepeat;

            maxHeight = heightPerRepeat * ((numberOfRocks - rocksBeforeRepeat) / rocksInRepeat);

            maxHeight = maxHeight + heights[rocksBeforeRepeat + remainingRocks];

            break;
        }
        else
        {
            resets.Add(blowIndex, (maxHeight, i));
        }
    }

    shape.BlowAndTryFall(dir, columns);

    while (shape.BlowAndTryFall(GetNextBlowDirection(), columns)) ;

    maxHeight = Math.Max(maxHeight, shape.AppendToStack(columns));
    heights.Add(i + 1, maxHeight);

    //Print(columns);
}


Console.WriteLine(maxHeight);
//1514285714288
//1514285714288

//1532183908048
//1535057471261
//1535057471263 // too high

bool IsReset(Shape shape)
{
    if (shapeIndex == 0)
    {
        foreach (var lockedIndex in shape.LockedIndices())
        {
            if (columns[lockedIndex, maxHeight])
            {
                return true;
            }
        }
    }

    return false;
}

void Print(bool[,] columns)
{
    Console.Clear();
    for (int y = 0; y < columns.GetLength(1); y++)
    {
        var hasTrue = false;
        for (int x = 0; x < columns.GetLength(0); x++)
        {
            hasTrue |= columns[x, y];
            Console.Write(columns[x, y] ? "#" : ".");
        }

        Console.WriteLine();

        if (!hasTrue)
        {
            break;
        }
    }
}

Shape? GetNextShape()
{
    shapeIndex = (shapeIndex + 1) % 5;

    var bottomOfShapePosition = maxHeight + 4;

    return shapeIndex switch
    {
        0 => new Horizontal(bottomOfShapePosition),
        1 => new Cross(bottomOfShapePosition),
        2 => new Ellll(bottomOfShapePosition),
        3 => new Vertical(bottomOfShapePosition),
        4 => new Square(bottomOfShapePosition),
        _ => null
    };
}

char GetNextBlowDirection()
{
    blowIndex = (blowIndex + 1) % blowDirections.Length;
    return blowDirections[blowIndex];
}

class Vertical : Shape
{
    private int column = 2;

    public Vertical(long bottomOfShapePosition) : base(bottomOfShapePosition) { }

    public override long AppendToStack(bool[,] columns)
    {
        columns[column, bottomOfShapePosition] = true;
        columns[column, bottomOfShapePosition + 1] = true;
        columns[column, bottomOfShapePosition + 2] = true;
        columns[column, bottomOfShapePosition + 3] = true;

        return bottomOfShapePosition + 3;
    }

    public override bool BlowAndTryFall(char blowDirection, bool[,] columns)
    {
        var newColumn = column;
        if (blowDirection == '>')
        {
            newColumn = Math.Min(newColumn + 1, 6);
        }
        else
        {
            newColumn = Math.Max(newColumn - 1, 0);
        }

        if (!columns[newColumn, bottomOfShapePosition] &&
            !columns[newColumn, bottomOfShapePosition + 1] &&
            !columns[newColumn, bottomOfShapePosition + 2] &&
            !columns[newColumn, bottomOfShapePosition + 3])
        {
            column = newColumn;
        }

        if (columns[column, bottomOfShapePosition - 1])
        {
            return false;
        }
        else
        {
            bottomOfShapePosition--;
            return true;
        }
    }
}

class Ellll : Shape
{
    private int column = 4;

    public Ellll(long bottomOfShapePosition) : base(bottomOfShapePosition) { }

    public override long AppendToStack(bool[,] columns)
    {
        columns[column, bottomOfShapePosition] = true;
        columns[column, bottomOfShapePosition + 1] = true;
        columns[column, bottomOfShapePosition + 2] = true;
        columns[column - 1, bottomOfShapePosition] = true;
        columns[column - 2, bottomOfShapePosition] = true;

        return bottomOfShapePosition + 2;
    }

    public override bool BlowAndTryFall(char blowDirection, bool[,] columns)
    {
        var newColumn = column;
        if (blowDirection == '>')
        {
            newColumn = Math.Min(newColumn + 1, 6);
        }
        else
        {
            newColumn = Math.Max(newColumn - 1, 2);
        }

        if (!columns[newColumn, bottomOfShapePosition] &&
            !columns[newColumn, bottomOfShapePosition + 1] &&
            !columns[newColumn, bottomOfShapePosition + 2] &&
            !columns[newColumn - 1, bottomOfShapePosition] &&
            !columns[newColumn - 2, bottomOfShapePosition])
        {
            column = newColumn;
        }

        if (columns[column, bottomOfShapePosition - 1] ||
            columns[column - 1, bottomOfShapePosition - 1] ||
            columns[column - 2, bottomOfShapePosition - 1])
        {
            return false;
        }
        else
        {
            bottomOfShapePosition--;
            return true;
        }
    }
}

class Cross : Shape
{
    private int column = 3;

    public Cross(long bottomOfShapePosition) : base(bottomOfShapePosition) { }

    public override long AppendToStack(bool[,] columns)
    {
        columns[column, bottomOfShapePosition] = true;
        columns[column, bottomOfShapePosition + 1] = true;
        columns[column, bottomOfShapePosition + 2] = true;
        columns[column + 1, bottomOfShapePosition + 1] = true;
        columns[column - 1, bottomOfShapePosition + 1] = true;

        return bottomOfShapePosition + 2;
    }

    public override bool BlowAndTryFall(char blowDirection, bool[,] columns)
    {
        var newColumn = column;
        if (blowDirection == '>')
        {
            newColumn = Math.Min(newColumn + 1, 5);
        }
        else
        {
            newColumn = Math.Max(newColumn - 1, 1);
        }

        if (!columns[newColumn, bottomOfShapePosition] &&
            !columns[newColumn + 1, bottomOfShapePosition + 1] &&
            !columns[newColumn - 1, bottomOfShapePosition + 1] &&
            !columns[newColumn, bottomOfShapePosition + 2])
        {
            column = newColumn;
        }

        if (columns[column, bottomOfShapePosition - 1] ||
            columns[column + 1, bottomOfShapePosition] ||
            columns[column - 1, bottomOfShapePosition])
        {
            return false;
        }
        else
        {
            bottomOfShapePosition--;
            return true;
        }
    }
}

class Horizontal : Shape
{
    private int column = 2;

    public Horizontal(long bottomOfShapePosition) : base(bottomOfShapePosition) { }

    public override long AppendToStack(bool[,] columns)
    {
        columns[column, bottomOfShapePosition] = true;
        columns[column + 1, bottomOfShapePosition] = true;
        columns[column + 2, bottomOfShapePosition] = true;
        columns[column + 3, bottomOfShapePosition] = true;

        return bottomOfShapePosition;
    }

    public override bool BlowAndTryFall(char blowDirection, bool[,] columns)
    {
        var newColumn = column;
        if (blowDirection == '>')
        {
            newColumn = Math.Min(newColumn + 1, 3);
        }
        else
        {
            newColumn = Math.Max(newColumn - 1, 0);
        }

        if (!columns[newColumn, bottomOfShapePosition] &&
            !columns[newColumn + 3, bottomOfShapePosition])
        {
            column = newColumn;
        }

        if (columns[column, bottomOfShapePosition - 1] ||
            columns[column + 1, bottomOfShapePosition - 1] ||
            columns[column + 2, bottomOfShapePosition - 1] ||
            columns[column + 3, bottomOfShapePosition - 1])
        {
            return false;
        }
        else
        {
            bottomOfShapePosition--;
            return true;
        }
    }

    public override IEnumerable<int> LockedIndices()
    {
        return new[] { 3 };
    }
}

class Square : Shape
{
    private int column = 2;

    public Square(long bottomOfShapePosition) : base(bottomOfShapePosition) { }

    public override long AppendToStack(bool[,] columns)
    {
        columns[column, bottomOfShapePosition] = true;
        columns[column + 1, bottomOfShapePosition] = true;
        columns[column, bottomOfShapePosition + 1] = true;
        columns[column + 1, bottomOfShapePosition + 1] = true;

        return bottomOfShapePosition + 1;
    }

    public override bool BlowAndTryFall(char blowDirection, bool[,] columns)
    {
        var newColumn = column;
        if (blowDirection == '>')
        {
            newColumn = Math.Min(newColumn + 1, 5);
        }
        else
        {
            newColumn = Math.Max(newColumn - 1, 0);
        }

        if (!columns[newColumn, bottomOfShapePosition] &&
            !columns[newColumn + 1, bottomOfShapePosition] &&
            !columns[newColumn, bottomOfShapePosition + 1] &&
            !columns[newColumn + 1, bottomOfShapePosition + 1])
        {
            column = newColumn;
        }

        if (columns[column, bottomOfShapePosition - 1] ||
            columns[column + 1, bottomOfShapePosition - 1])
        {
            return false;
        }
        else
        {
            bottomOfShapePosition--;
            return true;
        }
    }
}

abstract class Shape
{
    protected long bottomOfShapePosition;

    public Shape(long bottomOfShapePosition)
    {
        this.bottomOfShapePosition = bottomOfShapePosition;
    }

    public abstract bool BlowAndTryFall(char blowDirection, bool[,] columns);

    public abstract long AppendToStack(bool[,] columns);

    public virtual IEnumerable<int> LockedIndices()
    {
        return new int[] { };
    }
}