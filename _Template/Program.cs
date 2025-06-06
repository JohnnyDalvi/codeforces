using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class Program
{
    static void Solve(FastReader reader, StreamWriter writer)
    {

        //YOUR SOLUTION LOGIC GOES HERE

        int numOfLevels = reader.NextInt();
        int numOfStars = reader.NextInt();
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
        while (numOfStars > 0 || count < 1000000)
        {
            count++;
            List<TimePerStar> availableStars = new List<TimePerStar>();
            if (numOfStars > 1)
            {
                foreach (Level level in levels)
                {
                    availableStars.AddRange(level.timerPerStars);
                }
            }
            else if (numOfStars == 1)
            {
                foreach (Level level in levels)
                {
                    foreach (TimePerStar star in level.timerPerStars)
                    {
                        if (star.stars.Length == 1)
                        {
                            availableStars.Add(star);
                        }
                    }
                }
            }
            availableStars.Sort();
            // foreach (TimePerStar star in availableStars)
            // {
            //     Console.WriteLine($"Level: {star.myLevel.levelNumber}, Stars: {string.Join(",", star.stars)}, Time: {star.timeToComplete}, Time per Star: {star.timePerStar}");
            // }
            TimePerStar bestStar = availableStars[0];
            sortedLevels.Add(bestStar);
            bestStar.myLevel.RemoveStar(bestStar);
            numOfStars -= bestStar.stars.Length;
        }
        string levelConfig = "";
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
    
    public List<TimePerStar> timerPerStars;

    public Level(int star1, int star2)
    {
        time1Star = star1;
        time2Stars = star2;
        timerPerStars = new List<TimePerStar>();
        TimePerStar firstStarTime = new TimePerStar
        {
            myLevel = this,
            stars = new int[] { 1 },
            timeToComplete = time1Star
        };
        TimePerStar twoStarTimer = new TimePerStar
        {
            myLevel = this,
            stars = new int[] { 1, 2 },
            timeToComplete = time2Stars
        };
        timerPerStars.Add(firstStarTime);
        timerPerStars.Add(twoStarTimer);
    }

    public void RemoveStar(TimePerStar starToRemove)
    {
        if (starToRemove.stars.Length == 2)
        {
            firstStar = false;
            secondStar = false;
            timerPerStars = new List<TimePerStar>();
        }
        else if (starToRemove.stars[0] == 1)
        {
            firstStar = false;
            timerPerStars = new List<TimePerStar>();
            TimePerStar secondStar = new TimePerStar
            {
                myLevel = this,
                stars = new int[] { 2 },
                timeToComplete = time2Stars - time1Star
            };
            timerPerStars.Add(secondStar);
        }
        else if (starToRemove.stars[0] == 2)
        {
            secondStar = false;
            timerPerStars = new List<TimePerStar>();
        }
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