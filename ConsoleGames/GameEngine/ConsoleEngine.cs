using _2048Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractGame;
using TikTacToe;
using System.Reflection;
using BasicGameInterface;
using GamePlatform.Utilities;

namespace GameEngine
{
    internal class ConsoleEngine
    {
        internal void Run()
        {
            while (true)
            {
                GameConsoleUI.ClearConsole();
                ConsoleGame game = SelectGame();
                do
                {
                    game.InitializeGame();
                    game.RunGame();
                    game.CleanUp();
                } while (PlayAgainPrompt());
            }
        }
        private ConsoleGame SelectGame()
        {
            Type game = NativeSelectGameMenuType();
#if INCLUDE_NATIVE_GAMES
                 game = NativeSelectGameMenuType();
#elif ENABLE_REFLECTION
                 game = ReflectionSelectGameMenuType();
#endif

            return (ConsoleGame)Activator.CreateInstance(game);
        }
        private Type ReflectionSelectGameMenuType()
        {
            char response = ' ';
            StringBuilder SelectGameMenuBuilder = new StringBuilder(SELECT_GAME_MENU).AppendLine();
            List<(Type type, string Name)> types = GetDLLTypes();

            types.AddRange(Assembly.GetExecutingAssembly()
                                   .GetTypes()
                                   .Where(typeWhere => typeWhere.IsSubclassOf(typeof(ConsoleGame)))
                                   .Select(typeSelect => (typeSelect, typeSelect.GetCustomAttribute<GameNameAttribute>()?.Name ?? typeSelect.Name)));
            if (types.Count == 0)
            {
                GameConsoleUI.WriteLine("No games found. Press any key to exit.");
                GameConsoleUI.ReadKey();
                Environment.Exit(0);
            }
            for (int i = 0; i < types.Count; i++)
            {
                SelectGameMenuBuilder.AppendLine($"{(char)('a' + i)}. {types[i].Name} ");
            }
            SelectGameMenuBuilder.AppendLine(EXIT_MENU_OPTION_KEY + ". " + EXIT_MENU_OPTION);

            GameConsoleUI.WriteLine(SelectGameMenuBuilder.ToString(), SELECT_GAME_MENU_C.left, SELECT_GAME_MENU_C.top);
            GameConsoleUI.SetCursorPosition(SELECT_GAME_INPUT.left, SELECT_GAME_INPUT.top);
            while (true)
            {
                response = GameConsoleUI.ReadKey(true).KeyChar;
                if (response == EXIT_MENU_OPTION_KEY[0])
                {
                    Environment.Exit(0);
                }
                if (response >= 'a' && response <= 'z' && response - 'a' < types.Count)
                {
                    break;
                }
                // invalid input
                GameConsoleUI.WriteLine(SelectGameMenuBuilder.ToString(), SELECT_GAME_MENU_C.left, SELECT_GAME_MENU_C.top);
                GameConsoleUI.SetCursorPosition(SELECT_GAME_INPUT.left, SELECT_GAME_INPUT.top);
            }
            return types[response - 'a'].type;
        }
        private Type NativeSelectGameMenuType()
        {
            char response = ' ';
            StringBuilder SelectGameMenuBuilder = new StringBuilder(SELECT_GAME_MENU).AppendLine();
            List<(Type type, string Name)> types = new List<(Type type, string Name)>(Assembly.GetExecutingAssembly()
                                                                                              .GetTypes()
                                                                                              .Where(typeWhere => typeWhere.IsSubclassOf(typeof(ConsoleGame)))
                                                                                              .Select(typeSelect => (typeSelect, typeSelect.GetCustomAttribute<GameNameAttribute>()?.Name ?? typeSelect.Name)));
            if (types.Count == 0)
            {
                GameConsoleUI.WriteLine("No games found. Press any key to exit.");
                GameConsoleUI.ReadKey();
                Environment.Exit(0);
            }
            for (int i = 0; i < types.Count; i++)
            {
                SelectGameMenuBuilder.AppendLine($"{(char)('a' + i)}. {types[i].Name} ");
            }
            SelectGameMenuBuilder.AppendLine(EXIT_MENU_OPTION_KEY + ". " + EXIT_MENU_OPTION);

            GameConsoleUI.WriteLine(SelectGameMenuBuilder.ToString(), SELECT_GAME_MENU_C.left, SELECT_GAME_MENU_C.top);
            GameConsoleUI.SetCursorPosition(SELECT_GAME_INPUT.left, SELECT_GAME_INPUT.top);
            while (true)
            {
                response = GameConsoleUI.ReadKey(true).KeyChar;
                if (response == EXIT_MENU_OPTION_KEY[0])
                {
                    Environment.Exit(0);
                }
                if (response >= 'a' && response <= 'z' && response - 'a' < types.Count)
                {
                    break;
                }
                // invalid input
                GameConsoleUI.WriteLine(SelectGameMenuBuilder.ToString(), SELECT_GAME_MENU_C.left, SELECT_GAME_MENU_C.top);
                GameConsoleUI.SetCursorPosition(SELECT_GAME_INPUT.left, SELECT_GAME_INPUT.top);
            }
            return types[response - 'a'].type;
        }
        private bool PlayAgainPrompt()
        {
            GameConsoleUI.FlushKeyBuffer();
            GameConsoleUI.Write(PLAY_AGAIN_PROMPT);
            char inputResponse = GameConsoleUI.ReadKey(true).KeyChar;
            bool response = inputResponse.ToString().ToLower() == PLAY_AGAIN_YES;
            ClearConsoleBuffer(GameConsoleUI.CursorTop);
            return response;
        }
        private void ClearConsoleBuffer(int top)
        {
            GameConsoleUI.ClearConsoleLineBuffer(top);
        }
        private List<(Type type, string Name)> GetDLLTypes()
        {
            List<Assembly> assemblies = LoadAllDLLAssemblies();

            var gameTypes = assemblies.SelectMany(assembly => assembly.GetTypes())
                          .Where(typeWhere => typeWhere.IsSubclassOf(typeof(ConsoleGame)))
                          .Select(typeSelect => (typeSelect, typeSelect.GetCustomAttribute<GameNameAttribute>()?.Name ?? typeSelect.Name))
                          .ToList();

            return gameTypes;
        }
        private List<Assembly> LoadAllDLLAssemblies()
        {
            List<Assembly> assemblies = new List<Assembly>();
            foreach (string file in System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory(), "*.dll"))
            {
                assemblies.Add(Assembly.LoadFile(file));
            }
            return assemblies;
        }

        private readonly (int left, int top) SELECT_GAME_MENU_C = (0, 0);
        private const string SELECT_GAME_MENU = "Select a game to play: ";
        private readonly (int left, int top) SELECT_GAME_INPUT = (23,0);
        private const string INVALID_INPUT = " <-Invalid input. Please try again.";
        private const string EXIT_MENU_OPTION_KEY = "1";
        private const string EXIT_MENU_OPTION = "Exit";
        private const string PLAY_AGAIN_PROMPT = "Do you want to play again? (Y/N): ";
        private const string PLAY_AGAIN_YES = "y";
    }
}
