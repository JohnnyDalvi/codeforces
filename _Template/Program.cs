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
        // fazer a comparação com a quantidade de estrelas necessárias somente na hora de remover da lista


        while (numOfStars > 0)
        {
            for (int i = 0; i < availableStars.Count; i++)
            {

                if ((numOfStars >= 2 && availableStars[i].isAvailable)|| (availableStars[i].stars.Length <= numOfStars && availableStars[i].isAvailable))
                {

                    TimePerStar starToRemove = availableStars[i];

                    numOfStars -= availableStars[i].stars.Length;

                    starToRemove.myLevel.RemoveStar(starToRemove, availableStars);

                    sortedLevels.Add(starToRemove);

                    //Console.WriteLine("Count " + count + ":");
                    //availableStars[i].myLevel.Debug();                   
                    break;

                }
                
            }            
            
        }


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
    public int availableStars { get
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
        Console.WriteLine($"Level Number: {levelNumber}, Number of Stars: {2-availableStars}, t1: {time1Star}, t2: {time2Stars}");
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
            inputStream = new FileStream("input.txt", FileMode.Open, FileAccess.Read);
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