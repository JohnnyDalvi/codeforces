using System;
using System.IO;
using System.Text;

public class Program
{    static void Solve(FastReader reader, StreamWriter writer)
    {
        
        // YOUR SOLUTION LOGIC GOES HERE

        int a = reader.NextInt();

        if (a % 2 == 0 && a > 2)
        {
            writer.WriteLine("YES");
        }
        else
        {
            writer.WriteLine("NO");
        }
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