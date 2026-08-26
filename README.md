# Console Games Collection

Welcome to My Console Games Collection! This repository contains a series of games developed in C#, featuring classic games in the console. The games are simple yet fun, offering an engaging experience right in your console window.

## Table of Contents

- [About the Project](#about-the-project)
- [Games Included](#games-included)
- [Download and Play](#Download-and-Play)
- [Self Compile](#Self-Compile)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [How to Play](#how-to-play)
- [Project Structure](#project-structure)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## About the Project

This project is a collection of various games implemented in C#. The goal is to create fun and interactive games that run directly in the console. Whether you're just starting out or simply exploring game development in C#, join me on this journey as we learn and build fun, interactive games together.

## Games Included

- **2048**: Slide tiles on a grid to combine them and create a tile with the number 2048.
- **Tic Tac Toe**: A classic two-player game where the goal is to get three of your marks in a row.
- **Connect 4**: Drop your pieces into columns, aiming to get four in a row.
- **Snake**: Control a growing snake, eating food while avoiding collisions.

## Download and Play

You can download the latest standalone executable from the [Releases](https://github.com/ShadeSlayer1467/Games/releases) section. Download `GamePlatform.exe` and run it.

## Self Compile
### Prerequisites

To run these games, you need:

- [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework/net472) or higher for console games.

### Installation

1. **Clone the Repository**:
    ```bash
    git clone https://github.com/ShadeSlayer1467/Games.git
    cd Games
    ```

2. **Open the Solution**: Open the `.sln` file in Visual Studio or any C# compatible IDE.

3. **Build the Project**: Build the solution to restore all dependencies and compile the code.

### How to Play

1. **Run the Project**:
   - Set `GameEngine` as the startup project in your IDE. It builds `GamePlatform.exe`.
   - Press `F5` to build and run the game.
   
2. **Select a Game**:
   - Upon starting, a menu will appear to select which game you want to play.
   - Follow the on-screen instructions to enjoy the game.

### Run Tests

From the repository root:

```bash
dotnet test ConsoleGames/GameEngine.sln
```

## Project Structure

- **ConsoleGames/GameEngine**: The main console application and game menu.
- **ConsoleGames/GameEngine/Games/2048**: Contains the implementation of 2048.
- **ConsoleGames/GameEngine/Games/TicTacToe**: Contains the implementation of Tic Tac Toe.
- **ConsoleGames/GameEngine/Games/Connect4**: Contains the implementation of Connect 4.
- **ConsoleGames/GameEngine/Games/Snake**: Contains the implementation of Snake.
- **ConsoleGames/BasicGameInterface**: Shared interface for console games.
- **ConsoleGames/GamePlatform.Tests**: Regression tests for game rules and model behavior.

Each project is self-contained, making it easy to navigate and understand the code.

## Contributing

Contributions are what make the open-source community such a great place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. **Fork the Project**
2. **Create Your Feature Branch** (`git checkout -b feature/AmazingFeature`)
3. **Commit Your Changes** (`git commit -m 'Add some AmazingFeature'`)
4. **Push to the Branch** (`git push origin feature/AmazingFeature`)
5. **Open a Pull Request**

## License

Distributed under the MIT License. See `LICENSE` for more information.

## Contact

[shadeslayer1467@gmail.com](mailto:shadeslayer1467@gmail.com)

Project Link: [https://github.com/ShadeSlayer1467/Games](https://github.com/ShadeSlayer1467/Games)
