using System;
using System.Windows.Forms;
using System.Runtime.Versioning;
[SupportedOSPlatform("windows")]

public class Game
{
    private Frame frame;

    public Game()
    {
        frame = new Frame();
    }

    public void Run()
    {
        Application.Run(frame);
    }

    public static void Main(string[] args)
    {
        Game game = new Game();
        game.Run();
    }
}
