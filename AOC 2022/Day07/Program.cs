
var lines = await File.ReadAllLinesAsync("Input.txt");

var currentFolder = new Folder();
currentFolder.Folders.Add("/", new Folder
{
    ParentFolder = currentFolder
});

foreach (var line in lines)
{
    if (line.StartsWith("$"))
    {
        var parts = line.Split(' ');
        if (parts[1] == "cd")
        {
            if (parts[2] == "..")
            {
                currentFolder = currentFolder?.ParentFolder;
            }
            else
            {
                currentFolder = currentFolder.Folders[parts[2]];
            }
        }
    }
    else if (line.StartsWith("dir"))
    {
        currentFolder.Folders.TryAdd(line.Split(' ')[1], new Folder
        {
            ParentFolder = currentFolder
        });
    }
    else
    {
        var parts = line.Split(' ');

        currentFolder.Files.TryAdd(parts[1], new Folder.File
        {
            Size = long.Parse(parts[0])
        });
    }
}

while (currentFolder?.ParentFolder != null)
{
    currentFolder = currentFolder.ParentFolder;
}

long DoWork(Folder folder)
{
    long total = 0;
    foreach (var f in folder.Folders)
    {
        total += DoWork(f.Value);
    }

    if (folder.Size <= 100000)
    {
        total += folder.Size;
    }

    return total;
}

var totalOfSmallDirectories = DoWork(currentFolder);

Console.WriteLine($"Part 1 Total: {totalOfSmallDirectories}");

var freeSpace = 70000000 - currentFolder.Size;
var moreSpaceNeeded = 30000000 - freeSpace;

Folder? DoWork2(Folder folder)
{
    Folder? fff = null;
    foreach (var f in folder.Folders)
    {
        var newFFF = DoWork2(f.Value);
        if (newFFF != null)
        {
            if (fff == null || newFFF.Size < fff.Size)
            {
                fff = newFFF;
            }
        }
    }

    if (folder.Size >= moreSpaceNeeded && fff == null)
    {
        return folder;
    }

    return fff;
}

var folderToDelete = DoWork2(currentFolder);

Console.WriteLine($"Part 2 Size: {folderToDelete?.Size}");

public class Folder
{
    public Folder? ParentFolder { get; set; }
    public Dictionary<string, File> Files { get; } = new Dictionary<string, File>();
    public Dictionary<string, Folder> Folders { get; } = new Dictionary<string, Folder>();
    public long Size => Files.Sum(f => f.Value.Size) + Folders.Sum(f => f.Value.Size);

    public class File
    {
        public long Size { get; set; }
    }
}
