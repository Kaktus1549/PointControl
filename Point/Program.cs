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
        RowsRenderable rows = new RowsRenderable(
            new Align(new Markup(finalScreenMarkup + "\n\n"), HorizontalAlignment.Center),
            new Align(new Markup($"[bold red]RED TEAM SCORE: {redTeamScore}[/]"), HorizontalAlignment.Center),
            new Align(new Markup($"[bold blue]    BLUE TEAM SCORE: {blueTeamScore}[/]"), HorizontalAlignment.Center),
            new Align(new Markup(finalMessage), HorizontalAlignment.Center)
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

    private static void ShowDecentralization(string borderColor)
    {
        int totalSeconds = decentralizationTime;
        int current = 0;

        while (true)
        {
        // Progress bar string (15 characters) – escape markup brackets.
        string bar = new string('█', current * 2) + new string('─', (totalSeconds * 2) - (current * 2));
        string barMarkup = $"          [[{bar}]] {current:D2}/{totalSeconds:D2}";

        var content = new RowsRenderable(
            new Align(new Markup("For cancellation press ENTER"), HorizontalAlignment.Center),
            new Align(new Markup(barMarkup), HorizontalAlignment.Center)
        );

        var popup = new Panel(content)
            .Header("")
            .Border(BoxBorder.None)
            .BorderStyle(Style.Parse(borderColor));

        AnsiConsole.Clear();
        // Display as a popup centered on the screen.
        AnsiConsole.Write(new Align(popup, HorizontalAlignment.Center, VerticalAlignment.Middle));

        // Handle completion or cancellation.
        if (current == totalSeconds)
            break;
        if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
        {
            // Call ControllingTeam() with the same border color.
            ControllingTeam(borderColor);
            return;
        }

        Thread.Sleep(1000);
        current++;
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
        var parts = command.Split(' ', 2); // Split into program and arguments
        string fileName = parts[0];
        string arguments = parts.Length > 1 ? parts[1] : "";

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                CreateNoWindow = true // Optional: don't show a console window
            }
        };

        process.Start(); // Run in background, method continues without waiting
    }

    public bool ExecuteChecker(string command)
    {
        var parts = command.Split(' ', 2); // Split into program and arguments
        string fileName = parts[0];
        string arguments = parts.Length > 1 ? parts[1] : "";

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };

        process.Start();

        // Read the output (can be async too, but here we keep it simple)
        string output = process.StandardOutput.ReadToEnd().Trim().ToLower();

        process.WaitForExit();

        return output.Contains("true");
    }
    public async Task<bool> HandleQuestion(Layout root, Layout questionPanel, Layout answerPanel, string teamColor="RED")
    {
        if (Questions.Count == 0)
        {
            Console.WriteLine("No questions available.");
            System.Environment.Exit(1);
            return false;
        }
        // Randomly select a question
        _questionIndex = GetRandomNumber(0, Questions.Count);
        var q = Questions[_questionIndex];
        //  Example -> {
        //     "questionText": "What type of modulation we dont have when using radio waves?",
        //     "outsorced": false,
        //     "exec": null,
        //     "answer":{
        //         "checkerNeeded": false,
        //         "checker": null,
        //         "dynamic": false,
        //         "answerList": ["AM", "FM", "KKM"],
        //         "correctAnswer": "KKM"
        //     }
        // }
        // Render the question via SpectreUI
        string questionText = q.QuestionText;
        bool   outsorced     = q.Outsorced;
        string? exec        = q.Exec;
        bool   checkerNeeded = q.Answer.CheckerNeeded;
        bool   dynamic       = q.Answer.Dynamic;
        string? checker      = q.Answer.Checker;
        var    answerList    = q.Answer.AnswerList;     
        string? correct      = q.Answer.CorrectAnswer;
        // Display the question
        string answerLayoutList = "";
        int lines = Math.Max(0, Console.WindowHeight - 17);
        if (answerList != null && answerList.Count > 0)
        {
            // Your options: a) AM, b) FM, c) KKM
            answerLayoutList += "\nYour options: ";
            for (int i = 0; i < answerList.Count; i++)
            {
                answerLayoutList += $"[bold]{alphabet[i]})[/] {answerList[i]} ";
            }
            answerLayoutList += "\n";
            lines -= 1;
        }
        Markup cancelationMarkup = new Markup("[grey][[For cancellation press SHIFT + Q]][/]");
        var questionBody = new SpectreUI.RowsRenderable(
            new Align(new Markup(questionText), HorizontalAlignment.Center),
            // Answer list if available
            new Align(new Markup(answerLayoutList + new string('\n', lines)), HorizontalAlignment.Center),
            new Align(cancelationMarkup, HorizontalAlignment.Center)
        );
        
        questionPanel.Update(
            new Panel(questionBody)
                .Header("[bold]Question[/]")
                .Border(BoxBorder.Rounded)
                .BorderStyle(Style.Parse("cyan"))
                .Expand());
        // Display the question
        AnsiConsole.Clear();
        AnsiConsole.Write(root);
        if(outsorced){
            if (string.IsNullOrWhiteSpace(exec))
            {
                Console.WriteLine("No exec found in the question.");
                System.Environment.Exit(1);
            }
            // Execute the program in the background
            ExecuteProgram(exec);
        }
        string userInput = string.Empty;
            while (true){
                if(SpectreUI.GetEndGame() < DateTime.Now)
                {
                    // End the game if the time is up
                    SpectreUI.EndGame();
                    break;
                }
                Console.CursorVisible = false;
                // Display the `WordEnter` panel with user input text dynamically
                answerPanel.Update(new Panel($"Enter your answer: [blue]{userInput}[/]")
                    .Header("[bold]WordEnter[/]")
                    .Border(BoxBorder.Rounded)
                    .BorderStyle(Style.Parse("blue"))
                    .Expand()
                );
                AnsiConsole.Clear();
                AnsiConsole.Write(root);

                // Capture single character input
                if (Console.KeyAvailable){
                    var key = Console.ReadKey(intercept: true);
                    if (key.Key == ConsoleKey.Enter)
                    {
                        if (dynamic == false && userInput.Length != 1){
                            continue;
                        }
                        // Process the completed input
                        var guess = userInput.ToLower();
                        
                        if (dynamic){
                            // Check if input is equal to the correct answer in lower case
                            if (correct != null && guess == correct.ToLower() && !checkerNeeded)
                            {
                                if (teamColor.Contains("RED", StringComparison.OrdinalIgnoreCase))
                                    SpectreUI.ControllingTeam("RED");
                                else
                                    SpectreUI.ControllingTeam("BLUE");
                                break;
                            }

                            if(checkerNeeded){
                                // Check if the answer is correct using the checker program
                                if (checker != null && ExecuteChecker($"{checker} {guess}"))
                                {
                                    if (teamColor.Contains("RED", StringComparison.OrdinalIgnoreCase))
                                        SpectreUI.ControllingTeam("RED");
                                    else
                                        SpectreUI.ControllingTeam("BLUE");
                                    break;
                                }
                                else
                                {
                                    // Lockdown protocol
                                    return false;
                                }
                            }
                            else
                            {
                                // Lockdown protocol
                                return false;
                            }
                            
                        }
                        else{
                            // Check if input is equal to the correct answer in lower case
                            if (correct != null && guess == correct.ToLower())
                            {
                                if (teamColor.Contains("RED", StringComparison.OrdinalIgnoreCase))
                                    SpectreUI.ControllingTeam("RED");
                                else
                                    SpectreUI.ControllingTeam("BLUE");
                                break;
                            }
                            if (userInput.Length == 1)
                            {
                                if (answerList != null && answerList.Count > 0)
                                {
                                    // Check if the input is in the answer list
                                    int index = alphabet.IndexOf(userInput.ToUpper());  
                                    string answer = string.Empty;
                                    try{
                                        answer = answerList[index];
                                    }
                                    catch (ArgumentOutOfRangeException)
                                    {
                                        // Lockdown protocol
                                        return false;
                                    }
                                    if (answer != null && answer.ToLower() == correct?.ToLower())
                                    {
                                        if (teamColor.Contains("RED", StringComparison.OrdinalIgnoreCase))
                                            SpectreUI.ControllingTeam("RED");
                                        else
                                            SpectreUI.ControllingTeam("BLUE");
                                        break;
                                    }
                                    else
                                    {
                                        // Lockdown protocol
                                        return false;
                                    }
                                }
                                else
                                {
                                    // Lockdown protocol
                                    return false;
                                }
                            }
                        }
                        // Clear the user input
                        userInput = string.Empty;
                    }
                    else if (key.Key == ConsoleKey.Backspace && userInput.Length > 0)
                    {
                        userInput = userInput.Substring(0, userInput.Length - 1);
                    }
                    else if (key.Key == ConsoleKey.Q && key.Modifiers.HasFlag(ConsoleModifiers.Shift))
                    {
                        // Handle SHIFT + Q for cancellation
                        break;
                    }
                    else if (!char.IsControl(key.KeyChar))
                    {
                        if (dynamic == false && userInput.Length >= 1)
                        {
                            // Continue if the user input is not 1 character, without any other checks
                            continue;
                        }
                        userInput += char.ToUpper(key.KeyChar);
                    }
                    // Sleep for 25ms to avoid screen flickering
                }
                await Task.Delay(50);
            }
        // Clear the console after the answer is processed
        Console.Clear();
        // Remove the question from the list
        return true;
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