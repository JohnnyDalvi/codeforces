using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Threading;

public class Program
{
    static void Solve(FastReader reader, StreamWriter writer)
    {

        //YOUR SOLUTION LOGIC GOES HERE

        int numOfLevels = reader.NextInt();
        int numOfStars = reader.NextInt();
        // Stopwatch stopWatch = new Stopwatch();

        List<Level> levels = new List<Level>();
        for (int i = 0; i < numOfLevels; i++)
        {
            int star1 = reader.NextInt();
            int star2 = reader.NextInt();
            Level level = new Level(star1, star2);
            level.levelNumber = i + 1; // Level numbers start from 1
            levels.Add(level);
        }

        // Console.WriteLine("Number of Levels: " + numOfLevels);
        // Console.WriteLine("Number of Stars: " + numOfStars);
        // foreach (Level level in levels)
        // {
        //     level.Debug();
        // }
        int count = 0;

        List<TimePerStar> sortedLevels = new List<TimePerStar>();
        List<TimePerStar> availableStars = new List<TimePerStar>();

        foreach (Level level in levels)
        {
            availableStars.AddRange(level.selfTimerPerStars);
        }
        availableStars.Sort();
        // foreach (TimePerStar star in availableStars)
        // {
        //     star.Debug();
        // }
        // fazer a comparação com a quantidade de estrelas necessárias somente na hora de remover da lista

        int totalInteractions = 0;
        int index = 0;
        while (numOfStars > 2)
        {
            for (int i = 0; i < availableStars.Count; i++)
            {
                totalInteractions++;

                if (availableStars[i].isAvailable)
                {

                    TimePerStar starToRemove = availableStars[i];
                    //starToRemove.Debug();
                    numOfStars -= availableStars[i].stars.Length;
                    //Console.WriteLine("number of stars: " + numOfStars);
                    starToRemove.myLevel.RemoveStar(starToRemove, availableStars);

                    sortedLevels.Add(starToRemove);

                    //Console.WriteLine("Count " + count + ":");
                    //availableStars[i].myLevel.Debug();         
                    index = i;
                    break;

                }

            }

        }
        if (numOfStars == 2)
        {
            // Console.WriteLine("Here1");
            for (int i = index; i < availableStars.Count; i++)
            {

                totalInteractions++;
                TimePerStar starToRemove = availableStars[i];

                TimePerStar[] starToRemoveDouble1 = new TimePerStar[2];
                TimePerStar starToRemoveSingle2 = new TimePerStar();
                starToRemoveDouble1[0] = starToRemove;
                int totalCostForDouble1 = starToRemove.timeToComplete;
                int totalCostForSingle2 = 999999;

                if (starToRemove.isAvailable && starToRemove.stars.Length == 2)
                {
                    //Console.WriteLine("Here2");
                    numOfStars -= starToRemove.stars.Length;
                    starToRemove.myLevel.RemoveStar(starToRemove, availableStars);
                    sortedLevels.Add(starToRemove);
                }
                else
                {
                    // Console.WriteLine("Here3");


                    for (int j = i + 1; j < availableStars.Count; j++)
                    {

                        if (starToRemoveDouble1[1] == null && availableStars[j].isAvailable && availableStars[j].stars.Length == 1 && totalCostForSingle2 > 0)
                        {
                            //Console.WriteLine("Here4");
                            starToRemoveDouble1[1] = availableStars[j];
                            totalCostForDouble1 = totalCostForDouble1 + availableStars[j].timeToComplete;
                            if (totalCostForSingle2 < 999999)
                                break;
                        }
                        else if (starToRemoveSingle2 == null && availableStars[j].isAvailable && totalCostForDouble1 > starToRemove.timeToComplete)
                        {
                            //Console.WriteLine("Here5");
                            starToRemoveSingle2 = availableStars[j];
                            totalCostForSingle2 = availableStars[j].timeToComplete;
                            if (totalCostForDouble1 > starToRemove.timeToComplete)
                                break;
                        }
                    }
                    if (totalCostForDouble1 > starToRemove.timeToComplete && totalCostForSingle2 < 999999)
                        break;

                }
                // Console.WriteLine("Total cost for double1: " + totalCostForDouble1 + " Total cost for single2: " + totalCostForSingle2);
                if (totalCostForDouble1 < totalCostForSingle2)
                {
                    //Console.WriteLine("Here6");
                    numOfStars -= starToRemoveDouble1[0].stars.Length + starToRemoveDouble1[1].stars.Length;
                    //Console.WriteLine("Star To Remove Double 1: ");
                    //starToRemoveDouble1[0].Debug();
                    //Console.WriteLine("Star To Remove Double 2: ");
                    //starToRemoveDouble1[1].Debug();
                    starToRemoveDouble1[0].myLevel.RemoveStar(starToRemoveDouble1[0], availableStars);
                    starToRemoveDouble1[1].myLevel.RemoveStar(starToRemoveDouble1[1], availableStars);
                    sortedLevels.Add(starToRemoveDouble1[0]);
                    sortedLevels.Add(starToRemoveDouble1[1]);
                }
                else
                {
                    //Console.WriteLine("Here7");
                    numOfStars -= starToRemoveSingle2.stars.Length;
                    starToRemove.myLevel.RemoveStar(starToRemoveSingle2, availableStars);
                    sortedLevels.Add(starToRemoveSingle2);
                }
                break;
            }
        }
        else
        {
            for (int i = 0; i < availableStars.Count; i++)
            {
                totalInteractions++;

                if (availableStars[i].isAvailable && availableStars[i].stars.Length == 1)
                {

                    TimePerStar starToRemove = availableStars[i];
                    //starToRemove.Debug();
                    numOfStars -= availableStars[i].stars.Length;
                    //.WriteLine("number of stars: " + numOfStars);
                    starToRemove.myLevel.RemoveStar(starToRemove, availableStars);
                    sortedLevels.Add(starToRemove);
                    break;

                }

            }
        }
        //Console.WriteLine("Total Interactions: " + totalInteractions);


        string levelConfig = "";
        //Console.WriteLine("Final:");
        foreach (Level level in levels)
        {
            //level.Debug();
            levelConfig += $"{2 - level.availableStars}";
        }
        int timeSpent = 0;
        foreach (TimePerStar star in sortedLevels)
        {
            // star.Debug();
            timeSpent += star.timeToComplete;
        }
        // Console.WriteLine("Total Time Spent: " + timeSpent);
        // Console.WriteLine("Level Configuration: " + levelConfig);
        // Console.WriteLine("Output: ");
        // Console.WriteLine("\n" + timeSpent + "\n" + levelConfig);
        writer.WriteLine(timeSpent + "\n" + levelConfig);

        // Get the elapsed time as a TimeSpan value.
        // TimeSpan ts = stopWatch.Elapsed;

        // Format and display the TimeSpan value.
        // string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
        //     ts.Hours, ts.Minutes, ts.Seconds,
        //     ts.Milliseconds / 10);
        // Console.WriteLine("RunTime " + elapsedTime);

    }


    // RUN THE CODE WITH "dotnet run" within the project directory (use cd if necessary)


    public static void Main()
    {
        var reader = new FastReader();
        var writer = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = false };
        Solve(reader, writer);
        writer.Flush();
    }

}

public class Level
{
    public int levelNumber;
    public bool firstStar = true;
    public bool secondStar = true;
    public int time1Star;
    public int time2Stars;
    public int availableStars
    {
        get
        {
            if (firstStar && secondStar) return 2;
            else if (firstStar || secondStar) return 1;
            else return 0;
        }
    }

    public List<TimePerStar> selfTimerPerStars;

    public Level(int star1, int star2)
    {
        time1Star = star1;
        time2Stars = star2;
        selfTimerPerStars = new List<TimePerStar>();
        TimePerStar firstStarTime = new TimePerStar
        {
            myLevel = this,
            stars = new int[] { 1 },
            timeToComplete = time1Star,
            isAvailable = true
        };
        TimePerStar twoStarTimer = new TimePerStar
        {
            myLevel = this,
            stars = new int[] { 1, 2 },
            timeToComplete = time2Stars,
            isAvailable = true
        };
        TimePerStar firstToTwoStarTimer = new TimePerStar
        {
            myLevel = this,
            stars = new int[] { 2 },
            timeToComplete = time2Stars - time1Star
        };
        selfTimerPerStars.Add(firstStarTime);
        selfTimerPerStars.Add(twoStarTimer);
        selfTimerPerStars.Add(firstToTwoStarTimer);
    }

    public void RemoveStar(TimePerStar starToRemove, List<TimePerStar> timerPerStars)
    {
        // stopWatch.Start();

        timerPerStars.Remove(starToRemove);
        if (starToRemove.stars[0] == 2)
        {
            secondStar = false;
        }
        else if (starToRemove.stars.Length == 1 && starToRemove.stars[0] == 1)
        {
            firstStar = false;
            selfTimerPerStars[2].isAvailable = true;
            timerPerStars.Remove(selfTimerPerStars[1]);
        }
        else if (starToRemove.stars.Length == 2)
        {
            timerPerStars.Remove(selfTimerPerStars[0]);
            timerPerStars.Remove(selfTimerPerStars[2]);
            firstStar = false;
            secondStar = false;
        }
        timerPerStars.Remove(starToRemove);

    }

    public void Debug()
    {
        Console.WriteLine($"Level Number: {levelNumber}, Number of Stars: {2 - availableStars}, t1: {time1Star}, t2: {time2Stars}");
    }
}

public class TimePerStar : IComparable<TimePerStar>
{
    public int CompareTo(TimePerStar other)
    {
        if (other == null) return 1;
        return timePerStar.CompareTo(other.timePerStar);
    }
    public Level myLevel;
    public int[] stars;
    public int timeToComplete;
    public float timePerStar { get { return (float)timeToComplete / stars.Length; } }
    public bool isAvailable = false;

    public void Debug()
    {
        Console.WriteLine($"Level: {myLevel.levelNumber}, Stars: {string.Join(",", stars)}, Time: {timePerStar}");
    }
}




public class FastReader
{
    private readonly StreamReader _reader;
    private string[] _tokens = Array.Empty<string>();
    private int _index = 0;

    public FastReader()
    {
        Stream inputStream;

        // Check if standard input is redirected (Codeforces will always redirect input)
        if (!Console.IsInputRedirected)
        {
            // Local dev: no input piped in -> read from input.txt
            inputStream = new FileStream("C:\\Repos\\codeforces\\VSCommunity\\ConsoleApp1\\input.txt", FileMode.Open, FileAccess.Read);
        }
        else
        {
            // Submission or piped input: use standard input
            inputStream = Console.OpenStandardInput();
        }
        _reader = new StreamReader(inputStream, Encoding.ASCII, false, 1 << 16);

    }

    private void ReadTokens()
    {
        while (_index >= _tokens.Length)
        {
            var line = _reader.ReadLine();
            while (line == "")
                line = _reader.ReadLine();
            _tokens = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            _index = 0;
        }
    }

    public string Next()
    {
        ReadTokens();
        return _tokens[_index++];
    }

    public int NextInt() => int.Parse(Next());
    public long NextLong() => long.Parse(Next());
    public double NextDouble() => double.Parse(Next());
}