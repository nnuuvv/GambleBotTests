//List<Task> gambleTasks = new List<Task>();

for (int i = 1; i < 2; i++)
{
    //gambleTasks.Add(Task.Run(() => GambleALot(10000000, i)));
}

//Task.WaitAll(gambleTasks.ToArray());


GambleALot(5000000, 10);

void GambleALot(int amount, int mModifier)
{
    OverallGambleData overallGambleData = new OverallGambleData()
    {
        BetPercentages = new List<int>(),
        Points = new List<long>(),
        Wins2X = new List<int>(),
        Wins3X = new List<int>(),
        Jackpots = new List<int>(),
        Loses = new List<int>()
    };

    List<Task> tasks = new List<Task>();
    List<GambleData> results = new List<GambleData>();

    for (int i = 0; i < amount; i++)
    {
        tasks.Add(Task.Run(() => Gamble(mModifier)));
    }

    Task.WaitAll(tasks.ToArray());


    //Task t = Task.WhenAll(tasks);
    //t.Wait();

    float withProfit = overallGambleData.Points.FindAll(e => e > 5000).Count;
    float total = overallGambleData.BetPercentages.Count;
    float percWithProfit = (withProfit / total) * 100;
    
    Console.WriteLine("Modifier: " + mModifier);
    Console.WriteLine("Average points: " + overallGambleData.Points.Average());
    Console.WriteLine("Average 2x Wins: " + overallGambleData.Wins2X.Average());
    Console.WriteLine("Average 3x Wins: " + overallGambleData.Wins3X.Average());
    Console.WriteLine("Average Jackpots: " + overallGambleData.Jackpots.Average());
    Console.WriteLine("Average Loses: " + overallGambleData.Loses.Average());
    Console.WriteLine("Lowest points: " + overallGambleData.Points.Min());
    Console.WriteLine("Highest points: " + overallGambleData.Points.Max());
    Console.WriteLine("Total over start budget(5000): " + withProfit);
    Console.WriteLine("Total: " + total);
    Console.WriteLine("% with profit: " + percWithProfit);
    Console.WriteLine("=================================");

    
    
    void Gamble(int modifier)
    {
        int jackpotPerc = 35;
        GambleData gambleData = new GambleData()
        {
            BetPercentages = modifier,
            Jackpots = 0,
            Losses = 0,
            Points = 5000,
            Wins2X = 0,
            Wins3X = 0,
            Cooldown = 5,
            Jackpot = 200
        };
    
        Random random = new Random();
        for (int i = 0; i < 48; i++)
        {
            int cur = random.Next(1, 101);
            var gAmount = gambleData.Points * modifier / 100;
            if (cur < 51)
            {
                gambleData.Points -= gAmount;
                gambleData.Jackpot += gAmount * jackpotPerc / 100;
                gambleData.Losses++;
            }
            else if (cur < 96)
            {
                gambleData.Points += gAmount * 2;
                gambleData.Wins2X++;
            }
            else if (cur != 100)
            {
                gambleData.Points += gAmount * 3;
                gambleData.Wins3X++;
            }
            else
            {
                gambleData.Points += gambleData.Jackpot;
                gambleData.Jackpots++;
                gambleData.Jackpot = 200;
            }
        }
        
        overallGambleData.BetPercentages.Add(gambleData.BetPercentages);
        overallGambleData.Points.Add(gambleData.Points);
        overallGambleData.Wins2X.Add(gambleData.Wins2X);
        overallGambleData.Wins3X.Add(gambleData.Wins3X);
        overallGambleData.Jackpots.Add(gambleData.Jackpots);
        overallGambleData.Loses.Add(gambleData.Losses);
        
        results.Add(gambleData); 
    }
}






class OverallGambleData
{
    public List<int> BetPercentages { get; set; }
    public List<long> Points { get; set; }
    public List<int> Wins2X { get; set; }
    public List<int> Wins3X { get; set; }
    public List<int> Jackpots { get; set; }
    public List<int> Loses { get; set; }
}

class GambleData
{
    public int BetPercentages { get; set; }
    public long Points { get; set; }
    public int Wins2X { get; set; }
    public int Wins3X { get; set; }
    public int Jackpots { get; set; }
    public int Losses { get; set; }
    public long Jackpot { get; set; }
    public int Cooldown { get; set; }
}