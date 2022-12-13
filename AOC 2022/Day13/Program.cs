
var lines = await File.ReadAllLinesAsync("Input.txt");

var index = 0;
var sum = 0;
for (int i = 0; i < lines.Length; i += 3)
{
    index++;
    var x = 1;
    var y = 1;
    var line1 = Parse(lines[i]);
    var line2 = Parse(lines[i + 1]);
    sum += Compare(line1, line2, index) ?? 0;
}

Console.WriteLine(sum);

int? Compare(Context ctx1, Context ctx2, int index)
{
    for (int j = 0; j < ctx1.Count; j++)
    {
        if (ctx2.Count == j)
        {
            return 0;
        }

        if (ctx1[j] is int && ctx2[j] is int)
        {
            if ((int)ctx1[j] > (int)ctx2[j])
            {
                return 0;
            }
            else if ((int)ctx1[j] < (int)ctx2[j])
            {
                return index;
            }
            else
            {
                continue;
            }
        }

        if (ctx1[j] is Context && ctx2[j] is Context)
        {
            var result = Compare((Context)ctx1[j], (Context)ctx2[j], index);

            if (result.HasValue)
            {
                return result;
            }
            else
            {
                continue;
            }
        }

        if (ctx1[j] is Context)
        {
            var ctx2Context = new Context { ctx2[j] };

            var result = Compare((Context)ctx1[j], ctx2Context, index);
            if (result.HasValue)
            {
                return result;
            }
            else
            {
                continue;
            }
        }
        else
        {
            var ctx1Context = new Context { ctx1[j] };

            var result = Compare(ctx1Context, (Context)ctx2[j], index);
            if (result.HasValue)
            {
                return result;
            }
            else
            {
                continue;
            }
        }
    }

    if (ctx2.Count > ctx1.Count)
    {
        return index;
    }

    return null;
}

Context Parse(string v)
{
    var context = new Context();

    for (int i = 1; i < v.Length - 1; i++)
    {
        if (v[i] == '[')
        {
            var newContext = new Context { Parent = context };
            context.Add(newContext);
            context = newContext;
        }
        else if (v[i] == ']')
        {
            context = context.Parent;
        }
        else if (v[i] == ',')
        {
            continue;
        }
        else
        {
            var beginningI = i;
            while (int.TryParse(v[i + 1].ToString(), out var intV))
            {
                i++;
            }

            context.Add(int.Parse(v.Substring(beginningI, i - beginningI + 1)));
        }
    }

    return context;
}

class Context : List<object>
{
    public Context? Parent { get; set; }
}