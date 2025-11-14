using System.Text;
using Spectre.Console;
using Spectre.Console.Rendering;
using System.Text.Json;
using System.Diagnostics;

/// <summary>
/// Spectre.Console UI helpers for WordleClient, including banner, pop‑ups, layouts,
/// and the controlling‑team decentralization sequence.
/// </summary>
public class SpectreUI
{
    // Optional ASCII art banner (Base‑64) goes here.
    private const string base64Ascii = "CiAuLS0tLiAgLi0tLS0uIC4tLiAuLS4gLi0tLS4gLi0tLS0uICAuLS0tLS4gLi0uICAgCi8gIF9fX30vICB7fSAgXHwgIGB8IHx7XyAgIF99fCB7fSAgfS8gIHt9ICBcfCB8ICAgClwgICAgIH1cICAgICAgL3wgfFwgIHwgIHwgfCAgfCAuLS4gXFwgICAgICAvfCBgLS0uCiBgLS0tJyAgYC0tLS0nIGAtJyBgLScgIGAtJyAgYC0nIGAtJyBgLS0tLScgYC0tLS0nCg==";
    private const string red = "CuKWiOKWiOKWiOKWiOKWiOKWiOKVlyDilojilojilojilojilojilojilojilZfilojilojilojilojilojilojilZcgCuKWiOKWiOKVlOKVkOKVkOKWiOKWiOKVl+KWiOKWiOKVlOKVkOKVkOKVkOKVkOKVneKWiOKWiOKVlOKVkOKVkOKWiOKWiOKVlwrilojilojilojilojilojilojilZTilZ3ilojilojilojilojilojilZcgIOKWiOKWiOKVkSAg4paI4paI4pWRCuKWiOKWiOKVlOKVkOKVkOKWiOKWiOKVl+KWiOKWiOKVlOKVkOKVkOKVnSAg4paI4paI4pWRICDilojilojilZEK4paI4paI4pWRICDilojilojilZHilojilojilojilojilojilojilojilZfilojilojilojilojilojilojilZTilZ0K4pWa4pWQ4pWdICDilZrilZDilZ3ilZrilZDilZDilZDilZDilZDilZDilZ3ilZrilZDilZDilZDilZDilZDilZ0gCiAgICAgICAgICAgICAgICAgICAgICAgIAo=";
    private const string blue = "CuKWiOKWiOKWiOKWiOKWiOKWiOKVlyDilojilojilZcgICAgIOKWiOKWiOKVlyAgIOKWiOKWiOKVl+KWiOKWiOKWiOKWiOKWiOKWiOKWiOKVlwrilojilojilZTilZDilZDilojilojilZfilojilojilZEgICAgIOKWiOKWiOKVkSAgIOKWiOKWiOKVkeKWiOKWiOKVlOKVkOKVkOKVkOKVkOKVnQrilojilojilojilojilojilojilZTilZ3ilojilojilZEgICAgIOKWiOKWiOKVkSAgIOKWiOKWiOKVkeKWiOKWiOKWiOKWiOKWiOKVlyAgCuKWiOKWiOKVlOKVkOKVkOKWiOKWiOKVl+KWiOKWiOKVkSAgICAg4paI4paI4pWRICAg4paI4paI4pWR4paI4paI4pWU4pWQ4pWQ4pWdICAK4paI4paI4paI4paI4paI4paI4pWU4pWd4paI4paI4paI4paI4paI4paI4paI4pWX4pWa4paI4paI4paI4paI4paI4paI4pWU4pWd4paI4paI4paI4paI4paI4paI4paI4pWXCuKVmuKVkOKVkOKVkOKVkOKVkOKVnSDilZrilZDilZDilZDilZDilZDilZDilZ0g4pWa4pWQ4pWQ4pWQ4pWQ4pWQ4pWdIOKVmuKVkOKVkOKVkOKVkOKVkOKVkOKVnQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAK";
    private const string finalScreen = "CiDilojilojiloDilojilojiloggIOKWk+KWiOKWiOKWiOKWiOKWiCDilpPilojilojilojilojilojiloQgICAgIOKWiOKWiOKWkiAgIOKWiOKWkyAg4paI4paI4paI4paI4paI4paIICAgICAgICAgIOKWhOKWhOKWhOKWhCAgICDilojilojilpMgICAgIOKWiCAgICDilojilogg4paT4paI4paI4paI4paI4paIIArilpPilojilogg4paSIOKWiOKWiOKWkuKWk+KWiCAgIOKWgCDilpLilojilojiloAg4paI4paI4paMICAg4paT4paI4paI4paRICAg4paI4paS4paS4paI4paIICAgIOKWkiAgICAgICAgIOKWk+KWiOKWiOKWiOKWiOKWiOKWhCDilpPilojilojilpIgICAgIOKWiOKWiCAg4paT4paI4paI4paS4paT4paIICAg4paAIArilpPilojilogg4paR4paE4paIIOKWkuKWkuKWiOKWiOKWiCAgIOKWkeKWiOKWiCAgIOKWiOKWjCAgICDilpPilojiloggIOKWiOKWkuKWkeKWkSDilpPilojilojiloQgICAgICAgICAgIOKWkuKWiOKWiOKWkiDiloTilojilojilpLilojilojilpEgICAg4paT4paI4paIICDilpLilojilojilpHilpLilojilojiloggICAK4paS4paI4paI4paA4paA4paI4paEICDilpLilpPiloggIOKWhCDilpHilpPilojiloQgICDilowgICAgIOKWkuKWiOKWiCDilojilpHilpEgIOKWkiAgIOKWiOKWiOKWkiAgICAgICAg4paS4paI4paI4paR4paI4paAICDilpLilojilojilpEgICAg4paT4paT4paIICDilpHilojilojilpHilpLilpPiloggIOKWhCAK4paR4paI4paI4paTIOKWkuKWiOKWiOKWkuKWkeKWkuKWiOKWiOKWiOKWiOKWkuKWkeKWkuKWiOKWiOKWiOKWiOKWkyAgICAgICDilpLiloDilojilpEgIOKWkuKWiOKWiOKWiOKWiOKWiOKWiOKWkuKWkiDilojilojilpMgICAg4paR4paT4paIICDiloDilojilpPilpHilojilojilojilojilojilojilpLilpLilpLilojilojilojilojilojilpMg4paR4paS4paI4paI4paI4paI4paSCuKWkSDilpLilpMg4paR4paS4paT4paR4paR4paRIOKWkuKWkSDilpEg4paS4paS4paTICDilpIgICAgICAg4paRIOKWkOKWkSAg4paSIOKWkuKWk+KWkiDilpIg4paRIOKWkuKWk+KWkiAgICDilpHilpLilpPilojilojilojiloDilpLilpEg4paS4paR4paTICDilpHilpHilpLilpPilpIg4paSIOKWkiDilpHilpEg4paS4paRIOKWkQogIOKWkeKWkiDilpEg4paS4paRIOKWkSDilpEgIOKWkSDilpEg4paSICDilpIgICAgICAg4paRIOKWkeKWkSAg4paRIOKWkeKWkiAg4paRIOKWkSDilpHilpIgICAgIOKWkuKWkeKWkiAgIOKWkSDilpEg4paRIOKWkiAg4paR4paR4paR4paS4paRIOKWkSDilpEgIOKWkSDilpEgIOKWkQogIOKWkeKWkSAgIOKWkSAgICDilpEgICAg4paRIOKWkSAg4paRICAgICAgICAg4paR4paRICDilpEgIOKWkSAg4paRICAg4paRICAgICAgIOKWkSAgICDilpEgICDilpEg4paRICAgIOKWkeKWkeKWkSDilpEg4paRICAgIOKWkSAgIAogICDilpEgICAgICAgIOKWkSAg4paRICAg4paRICAgICAgICAgICAgIOKWkSAgICAgICAg4paRICAgIOKWkSAgICAgIOKWkSAgICAgICAgICDilpEgIOKWkSAgIOKWkSAgICAgICAg4paRICDilpEKICAgICAgICAgICAgICAgICDilpEgICAgICAgICAgICAgIOKWkSAgICAgICAgICAgICAg4paRICAgICAgICAgICDilpEgICAgICAgICAgICAgICAgICAgICAgICAgCg==";

    private static double redTeamScore = 0;
    private static double blueTeamScore = 0;
    private static DateTime endGame = DateTime.Now.AddMinutes(15);
    private static int decentralizationTime = 10;

    // ─────────────────────────────────────────────────────────────────────────────
    // Generic helpers
    // ─────────────────────────────────────────────────────────────────────────────

    public static void WriteGameLog(string path, decimal redTeamScore, decimal blueTeamScore, string winningTeamColor){
        // Create a new log entry
        string logEntry = $"{DateTime.Now}: RED TEAM: {redTeamScore}, BLUE TEAM: {blueTeamScore}, WINNING TEAM: {winningTeamColor}\n";
        // Append the log entry to the file
        File.AppendAllText(path, logEntry);
        // Print the log entry to the console
        Console.WriteLine(logEntry);
    }

    public static void ShowBanner()
    {
        if (string.IsNullOrWhiteSpace(base64Ascii))
            return;

        var ascii = Encoding.ASCII.GetString(Convert.FromBase64String(base64Ascii));
        AnsiConsole.MarkupLine($"[bold yellow]{ascii}[/]");
    }

    public static void EndGame(){
        // Create new layout and display the final screen
        // Render also finalScreen from base64
        string finalMessage = "";
        if (redTeamScore > blueTeamScore)
            finalMessage = "This point was held longer by the [bold red]RED TEAM[/]!";
        else if (blueTeamScore > redTeamScore)
            finalMessage = "This point was held longer by the [bold blue]BLUE TEAM[/]!";
        else
            finalMessage = "This point was held equally by [bold]both teams[/]!";
        var finalScreenMarkup = Encoding.UTF8.GetString(Convert.FromBase64String(finalScreen));
        int padLines = Math.Max(0, Console.WindowHeight - 27);
        RowsRenderable rows = new RowsRenderable(
            new Align(new Markup("\n\n\n\n\n" + finalScreenMarkup + "\n\n\n\n"), HorizontalAlignment.Center),
            new Align(new Markup($"[bold red]RED TEAM SCORE: {redTeamScore}[/]"), HorizontalAlignment.Center),
            new Align(new Markup($"[bold blue]    BLUE TEAM SCORE: {blueTeamScore}[/]"), HorizontalAlignment.Center),
            new Align(new Markup(new string('\n', padLines/2)), HorizontalAlignment.Center),
            new Align(new Markup(finalMessage), HorizontalAlignment.Center),
            new Align(new Markup("\n[grey][[Press ENTER to exit]][/]"), HorizontalAlignment.Center),
            new Align(new Markup(new string('\n', padLines/2 - 4)), HorizontalAlignment.Center)
        );
        var popup = new Panel(rows)
            .Header("[bold]GAME OVER[/]")
            .Border(BoxBorder.Rounded)
            .BorderStyle(Style.Parse("yellow"))
            .Expand();
        AnsiConsole.Clear();
        AnsiConsole.Write(new Align(popup, HorizontalAlignment.Center, VerticalAlignment.Middle));
        while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
        AnsiConsole.Clear();
        Console.CursorVisible = true;
        WriteGameLog("game_log.txt", (decimal)redTeamScore, (decimal)blueTeamScore, redTeamScore > blueTeamScore ? "RED" : (blueTeamScore > redTeamScore ? "BLUE" : "BOTH"));
        Process.GetCurrentProcess().Kill();
    }
    
    public static void Popup(RowsRenderable rows, string header = "Popup", string borderColor = "yellow")
    {
        var popup = new Panel(rows)
            .Header($"[bold]{header.ToUpper()}[/]")
            .Border(BoxBorder.Rounded)
            .BorderStyle(Style.Parse(borderColor));

        AnsiConsole.Clear();
        AnsiConsole.Write(new Align(popup, HorizontalAlignment.Center, VerticalAlignment.Middle));
        while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
        AnsiConsole.Clear();
    }

    // ─────────────────────────────────────────────────────────────────────────────
    // Demo helpers (menu & simple question/answer layout)
    // ─────────────────────────────────────────────────────────────────────────────
    public static void addPoints(string teamColor, double points)
    {
        if (endGame < DateTime.Now)
            return;
        if (teamColor.Contains("RED", StringComparison.OrdinalIgnoreCase))
            redTeamScore += points;
        else if (teamColor.Contains("BLUE", StringComparison.OrdinalIgnoreCase))
            blueTeamScore += points; 
    }
    public static async Task WaitForEndGame(CancellationToken token)
    {
        while (!token.IsCancellationRequested && DateTime.Now < endGame)
            await Task.Delay(1000, token);

        // Nic zde nevykreslujeme!
    }

    public static void SetEndGame(DateTime newEndGame)
    {
        endGame = newEndGame;
    }
    public static async Task<string?> ShowMainMenuAsync(CancellationToken token)
    {
        // Prompt běží v pracovním vlákně, aby šel přerušit tokenem.
        return await Task.Run(() =>
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an option:")
                    .AddChoices("RED TEAM", "BLUE TEAM"));
        }, token);
    }

    public static (Layout root, Layout questionPanel, Layout answerPanel) ChallangeLayout(
        string questionMarkup = "", string answerPrompt = "Enter your answer:")
    {
        var root = new Layout("Root").SplitRows(
            new Layout("Question").Ratio(4),
            new Layout("Answer").Ratio(1));

        root["Question"].Update(
            new Panel(new Markup(questionMarkup).Centered())
                .Header("[bold]Question[/]")
                .Border(BoxBorder.Rounded)
                .BorderStyle(Style.Parse("cyan"))
                .Expand());

        root["Answer"].Update(
            new Panel(answerPrompt)
                .Header("[bold]Answer[/]")
                .Border(BoxBorder.Rounded)
                .BorderStyle(Style.Parse("grey"))
                .Expand());

        return (root, root["Question"], root["Answer"]);
    }


    public static void Render(Layout layout) => AnsiConsole.Write(layout);

    // ─────────────────────────────────────────────────────────────────────────────
    // Controlling‑team & decentralization sequence
    // ─────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Shows a full‑screen team panel. On &lt;ENTER&gt; it launches a centred
    /// DECENTRALIZATION pop‑up with a 10‑second ASCII progress bar. &lt;ENTER&gt;
    /// cancels mid‑way.
    /// </summary>
    public static void ControllingTeam(string teamColor)
    {
        if (string.IsNullOrWhiteSpace(teamColor))
            throw new ArgumentNullException(nameof(teamColor));

        var border = teamColor.Equals("RED", StringComparison.OrdinalIgnoreCase) ? "red" : "blue";

        int padLines = Math.Max(0, Console.WindowHeight - 14);
        var body = new RowsRenderable(
            // New line and red decoded ASCII art.
            new Align(new Markup(  new string('\n', padLines/2) + "[bold]This point is currently being held by team:[/]\n"  + Encoding.UTF8.GetString(Convert.FromBase64String(teamColor.Equals("RED", StringComparison.OrdinalIgnoreCase) ? red : blue)) + new string('\n', padLines/2)), HorizontalAlignment.Center),
            new Align(new Markup("[grey][[For point decentralization press ENTER]][/]"), HorizontalAlignment.Center)
        );

        var panel = new Panel(body)
            .Header($"[bold]CAPTURED[/]")
            .Border(BoxBorder.Rounded)
            .BorderStyle(Style.Parse(border))
            .Expand();

        AnsiConsole.Clear();
        AnsiConsole.Write(panel);
        while (true)
        {
            // ── 1.  Stop if the game-time is over ────────────────────────────────
            if (DateTime.Now >= endGame)
            {
                EndGame();
                break;              // or return;  – whichever makes more sense
            }

            // ── 2.  Stop if the user has pressed <Enter> ─────────────────────────
            if (Console.KeyAvailable)                 // ← non-blocking check
            {
                var key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Enter)      // only break on Enter
                    break;
                /*  Optional: handle other keys here
                    e.g. arrow keys to move a cursor, etc.                        */
            }

            // ── 3.  Add points every 250 ms ──────────────────────────────────────
            addPoints(teamColor, 0.25);

            Thread.Sleep(250);        // ¼-second tick – keeps CPU usage low
        }

        if (endGame < DateTime.Now)
        {
            EndGame();
            return;
        }

        ShowDecentralization(border);
    }

    private static void ShowDecentralization(string borderColor){
        int totalSeconds = decentralizationTime;
        int current = 0;

        // Initial renderable
        var bar = new string('─', totalSeconds * 2);
        var barMarkup = $"          [[{bar}]] 00/{totalSeconds:D2}";
        bool cancelled = false;

        var content = new RowsRenderable(
            new Align(new Markup("For cancellation press ENTER"), HorizontalAlignment.Center),
            new Align(new Markup(barMarkup), HorizontalAlignment.Center)
        );
        Console.Clear();

        var panel = new Panel(content)
            .Header("")
            .Border(BoxBorder.None)
            .BorderStyle(Style.Parse(borderColor));

        // Use Live display for dynamic updates
        AnsiConsole.Live(panel)
            .AutoClear(false)
            .Start(ctx =>
            {
                while (current <= totalSeconds)
                {
                    // Handle cancellation
                    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
                    {
                        cancelled = true;
                        break;
                    }

                    // Build updated bar
                    string filled = new string('█', current * 2);
                    string empty = new string('─', (totalSeconds * 2) - (current * 2));
                    string barUpdated = $"    [[{filled}{empty}]] {current:D2}/{totalSeconds:D2}";

                    // Update content
                    var updatedContent = new RowsRenderable(
                        new Align(new Markup("For cancellation press ENTER"), HorizontalAlignment.Center),
                        new Align(new Markup(barUpdated), HorizontalAlignment.Center)
                    );

                    panel = new Panel(updatedContent)
                        .Header("")
                        .Border(BoxBorder.None)
                        .BorderStyle(Style.Parse(borderColor));

                    // Refresh live display
                    ctx.UpdateTarget(panel);

                    Thread.Sleep(1000);
                    current++;
                }

                // Done — clear and exit
            });
        if (cancelled){
            Console.Clear();
            ControllingTeam(borderColor);
            return;
        }
        Console.Clear();
        return;
    }


    // Helper renderable that stacks items vertically.
    public sealed class RowsRenderable : IRenderable
    {
        private readonly IRenderable[] _rows;
        public RowsRenderable(params IRenderable[] rows) => _rows = rows;

        public Measurement Measure(RenderOptions options, int maxWidth) =>
            new Measurement(0, maxWidth);

        public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
        {
            foreach (var row in _rows)
                foreach (var seg in row.Render(options, maxWidth))
                    yield return seg;
        }
    }
    public static DateTime GetEndGame()
    {
        return endGame;
    }
    public static void SetDecentralizationTime(int time)
    {
        if (time < 0)
            throw new ArgumentOutOfRangeException(nameof(time), "Time must be positive.");
        else
            decentralizationTime = time;
    }
}

public class Questionare{
    public sealed class Question
    {
        public string                QuestionText { get; set; } = default!;
        public bool                  Outsorced    { get; set; }
        public string?               Exec         { get; set; }
        public Answer                Answer       { get; set; } = default!;
    }

    public sealed class Answer
    {
        public bool            CheckerNeeded { get; set; }
        public string?         Checker       { get; set; }
        public bool            Dynamic       { get; set; }
        public List<string>?   AnswerList    { get; set; }
        public string?         CorrectAnswer { get; set; }
    }

    public List<Question> Questions { get; set; } = new();
    // Random number generator
    private static readonly Random random = new Random();
    private string _path = "./Modules/questions.json";
    private int _questionIndex = 0;
    public string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";


    public Questionare(string path)
    {
        _path = path;
        LoadQuestions();
    }
    public void LoadQuestions()
    {
        if (File.Exists(_path))
        {
            string? json = File.ReadAllText(_path);
            var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Questions = JsonSerializer.Deserialize<List<Question>>(json, opts) ?? new();
            if (Questions.Count == 0)
            {
                Console.WriteLine("No questions found in the file.");
                System.Environment.Exit(1);
            }
        }
        else
        {
            Console.WriteLine($"File not found: {_path}");
            System.Environment.Exit(1);
        }
    }

    public static int GetRandomNumber(int min, int max)
    {
        return random.Next(min, max);
    }

    public void ExecuteProgram(string command)
    {
        string shell;
        string prefix;

        if (OperatingSystem.IsWindows())
        {
            shell = "cmd.exe";
            prefix = "/c ";
        }
        else
        {
            shell = "/bin/bash";
            prefix = "-c ";
        }

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = shell,
                Arguments = prefix + command,
                UseShellExecute = false,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                CreateNoWindow = true
            }
        };

        process.Start(); // Runs in background
        
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        using (var logStream = new StreamWriter("./debug.log", append: true))
        {
            logStream.WriteLine($"Executing command: {command}");
            logStream.WriteLine($"Output: {output}");
        }

    }


    public bool ExecuteChecker(string command)
    {
        string shell;
        string prefix;

        if (OperatingSystem.IsWindows())
        {
            shell = "cmd.exe";
            prefix = "/c ";
        }
        else
        {
            shell = "/bin/bash";
            prefix = "-c ";
        }

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = shell,
                Arguments = prefix + command,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };

        process.Start();

        string output = process.StandardOutput.ReadToEnd()
                                            .Trim()
                                            .ToLower();

        process.WaitForExit();

        // using (var logStream = new StreamWriter("./debug.log", append: true))
        // {
        //     logStream.WriteLine($"Checker command: {command}");
        //     logStream.WriteLine($"Checker output: {output}");
        // }

        return output.Contains("true");
    }

public async Task<bool> HandleQuestion(
    Layout root,
    Layout questionPanel,
    Layout answerPanel,
    string teamColor = "RED")
{
    // 1) Pick a random question
    if (Questions.Count == 0)
    {
        Console.WriteLine("No questions available.");
        Environment.Exit(1);
    }

    _questionIndex = GetRandomNumber(0, Questions.Count);
    var q = Questions[_questionIndex];

    // 2) Pull out question fields
    string questionText      = q.QuestionText;
    bool   outsorced         = q.Outsorced;
    string? exec             = q.Exec;
    bool   checkerNeeded     = q.Answer.CheckerNeeded;
    string? checker          = q.Answer.Checker;
    bool   dynamic           = q.Answer.Dynamic;
    List<string>? answerList = q.Answer.AnswerList;
    string? correct          = q.Answer.CorrectAnswer;

    // 3) Fire off external exec if needed
    if (outsorced)
    {
        if (string.IsNullOrWhiteSpace(exec))
        {
            Console.WriteLine("No exec found in the question.");
            Environment.Exit(1);
        }
        ExecuteProgram(exec!);
    }

    bool result = false;
    string userInput = "";

    // 4) Single Live session driving the Q&A loop
    await AnsiConsole.Live(root)
        .AutoClear(false)
        .Overflow(VerticalOverflow.Ellipsis)
        .Cropping(VerticalOverflowCropping.Top)
        .StartAsync(async ctx =>
        {
            int totalVertical = Console.WindowHeight - 17;
            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            while (DateTime.Now < SpectreUI.GetEndGame())
            {
                // Build the options markup
                var optionsMarkup = "";
                if (answerList?.Count > 0)
                {
                    optionsMarkup = "[bold]Your options:[/] ";
                    for (int i = 0; i < answerList.Count; i++)
                        optionsMarkup += $"[bold]{alphabet[i]})[/] {answerList[i]} ";
                }

                // Update question panel
                int paddingLines = Math.Max(0, totalVertical - (answerList?.Count > 0 ? 1 : 0));
                var questionBody = new SpectreUI.RowsRenderable(
                    new Align(new Markup(questionText), HorizontalAlignment.Center),
                    new Align(new Markup(optionsMarkup + "\n" + new string('\n', paddingLines)), HorizontalAlignment.Center),
                    new Align(new Markup("[grey][[Shift+Q to cancel]][/]"), HorizontalAlignment.Center)
                );
                questionPanel.Update(
                    new Panel(questionBody)
                        .Header("[bold]Question[/]")
                        .Border(BoxBorder.Rounded)
                        .BorderStyle(Style.Parse("cyan"))
                        .Expand()
                );

                // Update answer panel
                answerPanel.Update(
                    new Panel($"Enter your answer: [blue]{userInput}[/]")
                        .Header("[bold]WordEnter[/]")
                        .Border(BoxBorder.Rounded)
                        .BorderStyle(Style.Parse("blue"))
                        .Expand()
                );

                // Diff-based repaint
                ctx.Refresh();

                // Non-blocking input
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true);

                    if (key.Key == ConsoleKey.Enter)
                    {
                        if (!dynamic && userInput.Length != 1)
                            continue;

                        var guess = userInput.ToLowerInvariant();
                        bool isCorrect = false;

                        if (dynamic)
                        {
                            if (!checkerNeeded && string.Equals(guess, correct?.ToLower(), StringComparison.Ordinal))
                            {
                                isCorrect = true;
                            }
                            else if (checkerNeeded && checker is not null && ExecuteChecker($"{checker} {guess}"))
                            {
                                isCorrect = true;
                            }
                        }
                        else
                        {
                            if (string.Equals(guess, correct?.ToLower(), StringComparison.Ordinal))
                            {
                                isCorrect = true;
                            }
                            else if (userInput.Length == 1 && answerList?.Count > 0)
                            {
                                int idx = alphabet.IndexOf(userInput.ToUpper());
                                if (idx >= 0 &&
                                    string.Equals(answerList[idx], correct, StringComparison.OrdinalIgnoreCase))
                                {
                                    isCorrect = true;
                                }
                            }
                        }

                        if (isCorrect)
                        {
                            result = true;                          // ← set result
                        }
                        else
                        {
                            result = false;                         // ← wrong → lockdown
                        }

                        break; // exit the live loop
                    }
                    else if (key.Key == ConsoleKey.Backspace && userInput.Length > 0)
                    {
                        try{
                            userInput = userInput[..^1];
                        } catch (IndexOutOfRangeException){
                            userInput = "";
                        }
                    }
                    // SHIFT+Q → cancel
                    else if (key.Key == ConsoleKey.Q && key.Modifiers.HasFlag(ConsoleModifiers.Shift))
                    {
                        result = false; // treat cancel as wrong answer
                        break;
                    }
                    // Any other character
                    else if (!char.IsControl(key.KeyChar))
                    {
                        if (dynamic || userInput.Length == 0)
                            userInput += char.ToUpper(key.KeyChar);
                    }
                }


                await Task.Delay(50);
            }
        });

    // 5) Clear and return final result
    Console.Clear();
    return result;
}
}

public class Program
{
    public static async Task<string> ChangeLockTeam(){
        // Wait for 10 seconds
        await Task.Delay(10000);
        // Change the lock team to NONE
        return "NONE";
    }
    public static async Task Main(string[] args)
    {
        Console.Clear();
        // If there is first argument, use it as the path to the questions file, second as new end game time (in format mm:ss), third as decentralization time
        string? path = args.Length > 0 ? args[0] : "./Questions/test.json";
        string? endGameTime = args.Length > 1 ? args[1] : null;
        string? decentralizationTime = args.Length > 2 ? args[2] : null;
        DateTime newEndGame = DateTime.Now.AddMinutes(15);
        int decentralizationTimeValue = 10;
        if (decentralizationTime != null)
        {
            // Parse the decentralization time
            if (int.TryParse(decentralizationTime, out int time))
            {
                decentralizationTimeValue = time;
            }
        }
        if (endGameTime != null)
        {
            // Parse the end game time
            string[] timeParts = endGameTime.Split(':');
            if (timeParts.Length == 2 && int.TryParse(timeParts[0], out int minutes) && int.TryParse(timeParts[1], out int seconds))
            {
                newEndGame = DateTime.Now.AddMinutes(minutes).AddSeconds(seconds);
            }
        }
        // Set the decentralization time
        SpectreUI.SetDecentralizationTime(decentralizationTimeValue);
        SpectreUI.SetEndGame(newEndGame);
        Questionare questionare = new Questionare(path);

        bool teamSuccess = true;
        string lockTeam = "NONE";
        string? option;

        
        // Start the end game timer
        var cts = new CancellationTokenSource();

    // úloha, která po uplynutí času zruší token
    var timerTask = Task.Run(async () =>
    {
        var delay = newEndGame - DateTime.Now;
        if (delay < TimeSpan.Zero) delay = TimeSpan.Zero;
        await Task.Delay(delay);
        cts.Cancel();                    // → přeruší prompt
    });

    while (true)
    {
        // 1) spustíme prompt s tokenem, aby šel přerušit
        var menuTask = SpectreUI.ShowMainMenuAsync(cts.Token);

        // 2) počkáme, která úloha skončí dřív
        var completed = await Task.WhenAny(menuTask, timerTask);

        // 3) vypršel čas → menu rušíme a čekáme, až se opravdu ukončí
        if (completed == timerTask)
        {
            try { await menuTask; } catch { /* schltneme OperationCanceledException */ }

            Console.Clear();     // žádná další vlákna už do konzole nepíší
            SpectreUI.EndGame(); // vykreslí panel a volá Environment.Exit(0)
            return;              // pro jistotu
        }

        // --- sem se dostaneme jen, pokud uživatel opravdu vybral možnost ---
        option = menuTask.Result;
            if (string.IsNullOrWhiteSpace(option)){
                continue;
            }
            if (option.Contains("RED", StringComparison.OrdinalIgnoreCase))
                option = "RED";
            else if (option.Contains("BLUE", StringComparison.OrdinalIgnoreCase))
                option = "BLUE";
            else
                break;
            if (teamSuccess == false && lockTeam == option)
            {
                // Lockdown protocol
                SpectreUI.RowsRenderable rows = new SpectreUI.RowsRenderable(
                    new Align(new Markup($"[bold red]LOCKDOWN![/]\n\n[bold {option}]{lockTeam}[/] team has been locked down for 10 seconds."), HorizontalAlignment.Center),
                    new Align(new Markup("Press [bold yellow]<ENTER>[/] to continue."), HorizontalAlignment.Center)
                );
                SpectreUI.Popup(rows, "Popup", "yellow");
                continue;
            }
            var (root, historyPanel, inputPanel) = SpectreUI.ChallangeLayout();
            // Run the question handler
            teamSuccess = await questionare.HandleQuestion(root, historyPanel, inputPanel, option);
            if (teamSuccess == true){
                SpectreUI.ControllingTeam(option);
            }
            if (teamSuccess == false)
            {
                // Lockdown protocol
                lockTeam = option;
                SpectreUI.RowsRenderable rows = new SpectreUI.RowsRenderable(
                    new Align(new Markup($"[bold red]LOCKDOWN![/]\n\n[bold {option}]{lockTeam}[/] team has been locked down for 10 seconds."), HorizontalAlignment.Center),
                    new Align(new Markup("Press [bold yellow]<ENTER>[/] to continue."), HorizontalAlignment.Center)
                );
                SpectreUI.Popup(rows, "Popup", "yellow");
                // Start the lock team change task
                _ = ChangeLockTeam().ContinueWith(t => lockTeam = t.Result);
            }
        }
    }
}