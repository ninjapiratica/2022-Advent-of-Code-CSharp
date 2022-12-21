
using System.Diagnostics.CodeAnalysis;

var lines = await File.ReadAllLinesAsync("Input.txt");

var blueprints = lines.Select((l, ix) =>
{
    var parts = l.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var bp = new BluePrint()
    {
        Id = int.Parse(parts[1].TrimEnd(':')),
        Ore = new OreRobot { Ore = int.Parse(parts[6]) },
        Clay = new ClayRobot { Ore = int.Parse(parts[12]) },
        Obsidian = new ObsidianRobot { Ore = int.Parse(parts[18]), Clay = int.Parse(parts[21]) },
        Geode = new GeodeRobot { Ore = int.Parse(parts[27]), Obsidian = int.Parse(parts[30]) },
    };

    bp.FinalizeSetup();
    return bp;
});

var bpBest = new Dictionary<BluePrint, int>();

var minutes = 32;

foreach (var bp in blueprints.Take(3))
{
    var wallets = new HashSet<Wallet>(new WalletEqualityComparer()) { new Wallet() { OreRobots = 1 } };

    for (int i = minutes; i > 0; i--)
    {
        var upcomingWallets = new HashSet<Wallet>(new WalletEqualityComparer());
        foreach (var wallet in wallets)
        {
            foreach (var item in Advance(bp, wallet).Where(w => w != null))
            {
                upcomingWallets.Add(item);
            }
        }

        var maxGeode = upcomingWallets.Max(x => x.Geode + ((i - 1) * x.GeodeRobots));
        var maxPossible = Enumerable.Range(0, i - 1).Sum();
        wallets = upcomingWallets.Where(w => w.Geode + ((i - 1) * (w.GeodeRobots + maxPossible)) >= maxGeode).ToHashSet();

        Console.WriteLine($"BP: {bp.Id} - Done Minute {minutes + 1 - i} - Possible Wallets: {wallets.Count}");
    }

    bpBest.Add(bp, wallets.Max(w => w.Geode));
}

foreach (var bp in bpBest)
{
    Console.WriteLine($"{bp.Key.Id} - {bp.Value}");
}

Console.WriteLine(bpBest.Sum(bp => bp.Key.Id * bp.Value));
Console.WriteLine(bpBest.Aggregate(1, (agg, x) => x.Value * agg));

IEnumerable<Wallet> Advance(BluePrint bluePrint, Wallet wallet)
{
    yield return TryPath(bluePrint.Clay, wallet);
    yield return TryPath(bluePrint.Geode, wallet);
    yield return TryPath(bluePrint.Obsidian, wallet);
    yield return TryPath(bluePrint.Ore, wallet);
    yield return TryPath(null, wallet);
}

Wallet TryPath(Robot testRobot, Wallet wallet)
{
    if ((testRobot?.CanPurchase(wallet)).GetValueOrDefault(true))
    {
        var clonedWallet = wallet.Clone();

        clonedWallet.Mine();
        testRobot?.Purchase(clonedWallet);

        return clonedWallet;
    }

    return null;
}

public class BluePrint
{
    public int Id { get; set; }
    public OreRobot Ore { get; set; }
    public ClayRobot Clay { get; set; }
    public ObsidianRobot Obsidian { get; set; }
    public GeodeRobot Geode { get; set; }

    public void FinalizeSetup()
    {
        Ore.MaxOfType = new[] { Ore.Ore, Clay.Ore, Obsidian.Ore, Geode.Ore }.Max();
        Clay.MaxOfType = new[] { Ore.Clay, Clay.Clay, Obsidian.Clay, Geode.Clay }.Max();
        Obsidian.MaxOfType = new[] { Ore.Obsidian, Clay.Obsidian, Obsidian.Obsidian, Geode.Obsidian }.Max();
    }
}

public abstract class Robot
{
    public int Ore { get; set; }
    public int Clay { get; set; }
    public int Obsidian { get; set; }
    public int MaxOfType { get; set; } = int.MaxValue;

    public Robot Clone()
    {
        return (Robot)MemberwiseClone();
    }

    public virtual bool CanPurchase(Wallet wallet)
    {
        return Clay <= wallet.Clay &&
            Obsidian <= wallet.Obsidian &&
            Ore <= wallet.Ore;
    }

    public virtual void Purchase(Wallet wallet)
    {
        wallet.Clay -= Clay;
        wallet.Ore -= Ore;
        wallet.Obsidian -= Obsidian;
    }
}

public class OreRobot : Robot
{
    public override bool CanPurchase(Wallet wallet)
    {
        return MaxOfType > wallet.OreRobots &&
            base.CanPurchase(wallet);
    }

    public override void Purchase(Wallet wallet)
    {
        base.Purchase(wallet);

        wallet.OreRobots++;
    }
}

public class ClayRobot : Robot
{
    public override bool CanPurchase(Wallet wallet)
    {
        return MaxOfType > wallet.ClayRobots &&
            base.CanPurchase(wallet);
    }

    public override void Purchase(Wallet wallet)
    {
        base.Purchase(wallet);

        wallet.ClayRobots++;
    }
}

public class ObsidianRobot : Robot
{
    public override bool CanPurchase(Wallet wallet)
    {
        return MaxOfType > wallet.ObsidianRobots &&
            base.CanPurchase(wallet);
    }

    public override void Purchase(Wallet wallet)
    {
        base.Purchase(wallet);

        wallet.ObsidianRobots++;
    }
}

public class GeodeRobot : Robot
{
    public override void Purchase(Wallet wallet)
    {
        base.Purchase(wallet);

        wallet.GeodeRobots++;
    }
}

public class Wallet
{
    public int Ore { get; set; }
    public int Clay { get; set; }
    public int Obsidian { get; set; }
    public int Geode { get; set; }

    public int OreRobots { get; set; }
    public int ClayRobots { get; set; }
    public int ObsidianRobots { get; set; }
    public int GeodeRobots { get; set; }

    public void Mine()
    {
        Ore += OreRobots;
        Clay += ClayRobots;
        Obsidian += ObsidianRobots;
        Geode += GeodeRobots;
    }

    public Wallet Clone()
    {
        return (Wallet)MemberwiseClone();
    }
}

public class WalletEqualityComparer : IEqualityComparer<Wallet>
{
    public bool Equals(Wallet? x, Wallet? y)
    {
        return x.Ore == y.Ore &&
            x.Clay == y.Clay &&
            x.Obsidian == y.Obsidian &&
            x.Geode == y.Geode &&

            x.OreRobots == y.OreRobots &&
            x.ClayRobots == y.ClayRobots &&
            x.ObsidianRobots == y.ObsidianRobots &&
            x.GeodeRobots == y.GeodeRobots;
    }

    public int GetHashCode([DisallowNull] Wallet obj)
    {
        return $"{obj.Ore}_{obj.Clay}_{obj.Obsidian}_{obj.Geode}_{obj.OreRobots}_{obj.ClayRobots}_{obj.ObsidianRobots}_{obj.GeodeRobots}".GetHashCode();
    }
}


