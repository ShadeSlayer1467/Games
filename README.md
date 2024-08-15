# Console and MonoGame Games Collection

Welcome to the Console and MonoGame Games Collection! This repository contains a series of games developed in C#, featuring both classic console games and MonoGame implementations. The games are simple yet fun, offering an engaging experience right in your console window.

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

This project is a collection of various games implemented in C#. The goal is to create fun and interactive games that run directly in the console, as well as plans to add more complex games using MonoGame. Whether you're just starting out or simply exploring game development in C#, join me on this journey as we learn and build fun, interactive games together.

## Games Included

- **2048**: Slide tiles on a grid to combine them and create a tile with the number 2048.
- **Tic Tac Toe**: A classic two-player game where the goal is to get three of your marks in a row.
- **Connect 4** (In Progress): Drop your pieces into columns, aiming to get four in a row.
- **Snake** (Planned): Control a growing snake, eating food while avoiding collisions.

## Download and Play

You can download the latest version of the games as standalone executables from the [Releases](https://github.com/ShadeSlayer1467/Games/releases) section. Simply download the ZIP file for the games, extract it, and run the `.exe` file.

## Self Compile
### Prerequisites

To run these games, you need:

- [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework/net472) or higher for console games.
- [.NET Core 6.0](https://dotnet.microsoft.com/download/dotnet/6.0) for MonoGame projects.

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
   - Set `GamePlatform` as the startup project in your IDE.
   - Press `F5` to build and run the game.
   
2. **Select a Game**:
   - Upon starting, a menu will appear to select which game you want to play.
   - Follow the on-screen instructions to enjoy the game.

## Project Structure

The repository is organized into several projects, each representing a different game:

- **2048Game**: Contains the implementation of the 2048 game.
- **TikTacToe**: Contains the implementation of the Tic Tac Toe game.
- **GamePlatform**: A central project that ties all games together, providing a menu-based interface.
- **Pong**: A MonoGame project for a Pong game.

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

Matthew Read - [shadeslayer1467@gmail.com](mailto:shadeslayer1467@gmail.com)

Project Link: [https://github.com/ShadeSlayer1467/Games](https://github.com/ShadeSlayer1467/YourRepositoryName)
