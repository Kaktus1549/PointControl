using System.Text;
using Spectre.Console;
using Spectre.Console.Rendering;
using System.Text.Json;
using System.Diagnostics;

/// <summary>
/// Spectre.Console UI helpers for WordleClient, including banner, pop‑ups, layouts,
/// and the controlling‑team decentralization sequence.
/// </summary>
public static class SpectreUI
{
    // Optional ASCII art banner (Base‑64) goes here.
    private const string base64Ascii = "CiAuLS0tLiAgLi0tLS0uIC4tLiAuLS4gLi0tLS4gLi0tLS0uICAuLS0tLS4gLi0uICAgCi8gIF9fX30vICB7fSAgXHwgIGB8IHx7XyAgIF99fCB7fSAgfS8gIHt9ICBcfCB8ICAgClwgICAgIH1cICAgICAgL3wgfFwgIHwgIHwgfCAgfCAuLS4gXFwgICAgICAvfCBgLS0uCiBgLS0tJyAgYC0tLS0nIGAtJyBgLScgIGAtJyAgYC0nIGAtJyBgLS0tLScgYC0tLS0nCg==";
    private const string red = "CuKWiOKWiOKWiOKWiOKWiOKWiOKVlyDilojilojilojilojilojilojilojilZfilojilojilojilojilojilojilZcgCuKWiOKWiOKVlOKVkOKVkOKWiOKWiOKVl+KWiOKWiOKVlOKVkOKVkOKVkOKVkOKVneKWiOKWiOKVlOKVkOKVkOKWiOKWiOKVlwrilojilojilojilojilojilojilZTilZ3ilojilojilojilojilojilZcgIOKWiOKWiOKVkSAg4paI4paI4pWRCuKWiOKWiOKVlOKVkOKVkOKWiOKWiOKVl+KWiOKWiOKVlOKVkOKVkOKVnSAg4paI4paI4pWRICDilojilojilZEK4paI4paI4pWRICDilojilojilZHilojilojilojilojilojilojilojilZfilojilojilojilojilojilojilZTilZ0K4pWa4pWQ4pWdICDilZrilZDilZ3ilZrilZDilZDilZDilZDilZDilZDilZ3ilZrilZDilZDilZDilZDilZDilZ0gCiAgICAgICAgICAgICAgICAgICAgICAgIAo=";
    private const string blue = "CuKWiOKWiOKWiOKWiOKWiOKWiOKVlyDilojilojilZcgICAgIOKWiOKWiOKVlyAgIOKWiOKWiOKVl+KWiOKWiOKWiOKWiOKWiOKWiOKWiOKVlwrilojilojilZTilZDilZDilojilojilZfilojilojilZEgICAgIOKWiOKWiOKVkSAgIOKWiOKWiOKVkeKWiOKWiOKVlOKVkOKVkOKVkOKVkOKVnQrilojilojilojilojilojilojilZTilZ3ilojilojilZEgICAgIOKWiOKWiOKVkSAgIOKWiOKWiOKVkeKWiOKWiOKWiOKWiOKWiOKVlyAgCuKWiOKWiOKVlOKVkOKVkOKWiOKWiOKVl+KWiOKWiOKVkSAgICAg4paI4paI4pWRICAg4paI4paI4pWR4paI4paI4pWU4pWQ4pWQ4pWdICAK4paI4paI4paI4paI4paI4paI4pWU4pWd4paI4paI4paI4paI4paI4paI4paI4pWX4pWa4paI4paI4paI4paI4paI4paI4pWU4pWd4paI4paI4paI4paI4paI4paI4paI4pWXCuKVmuKVkOKVkOKVkOKVkOKVkOKVnSDilZrilZDilZDilZDilZDilZDilZDilZ0g4pWa4pWQ4pWQ4pWQ4pWQ4pWQ4pWdIOKVmuKVkOKVkOKVkOKVkOKVkOKVkOKVnQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAK";


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

    public static void Popup(string message, string header = "Popup", string borderColor = "yellow")
    {
        var popup = new Panel(new Markup(message))
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

    public static string ShowMainMenu()
    {
        ShowBanner();
        Console.WriteLine();
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select an option:")
                .AddChoices("RED TEAM", "BLUE TEAM"));
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
        while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

        ShowDecentralization(border);
    }

    private static void ShowDecentralization(string borderColor)
    {
        const int totalSeconds = 4;
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

        return output.Contains("true") || output.Contains("yes");
    }
    public async Task HandleQuestion(Layout root, Layout questionPanel, Layout answerPanel, string teamColor="RED")
    {
        if (Questions.Count == 0)
        {
            Console.WriteLine("No questions available.");
            System.Environment.Exit(1);
            return;
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
                        }
                        else
                        {
                            // Lockdown protocol
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
                                string answer = answerList[index];
                                if (answer != null && answer.ToLower() == correct?.ToLower())
                                {
                                    if (teamColor.Contains("RED", StringComparison.OrdinalIgnoreCase))
                                        SpectreUI.ControllingTeam("RED");
                                    else
                                        SpectreUI.ControllingTeam("BLUE");
                                    break;
                                }
                            }
                            else
                            {
                                // Lockdown protocol
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
                await Task.Delay(50);
            }
        // Clear the console after the answer is processed
        Console.Clear();
        // Remove the question from the list
        return;
    }
}

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.Clear();
        Questionare questionare = new Questionare("./Questions/test.json");
        while (true){
            // Example usage of the SpectreUI class.
            string option = SpectreUI.ShowMainMenu();
            if (option.Contains("RED", StringComparison.OrdinalIgnoreCase))
                option = "RED";
            else if (option.Contains("BLUE", StringComparison.OrdinalIgnoreCase))
                option = "BLUE";
            else
                break;
            var (root, historyPanel, inputPanel) = SpectreUI.ChallangeLayout();
            // Run the question handler
            await questionare.HandleQuestion(root, historyPanel, inputPanel, option);
        }
    }
}