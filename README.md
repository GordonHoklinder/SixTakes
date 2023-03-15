# Six Takes

This is a C# console application for playing the game [Six Takes](https://en.wikipedia.org/wiki/6_nimmt!) against various algorithms.

The project serves two main purposes:

- Evaluate different strategies to play this game.
- Provide a very simple interface to play against these strategies.

## How to install

Clone the repo via `git clone https://github.com/GordonHoklinder/SixTakes`.

For running the application you'll have to have `dotnet` installed.

## How to play

If you are Windows user, you can run the game using `Visual Studio`.
If you are Linux user run `dotnet run` in the project's root.

After running the application you'll be propted to enter the players for the game.
You'll have to enter a string consisting on 2 to 10 character, each representing one player as described in the interface.
Then, after selecting the number of games, the games will either simulate themselves (if no user player is playing), or you'll be asked to enter your plays against the computer.

You can view the rules of the game here: https://en.wikipedia.org/wiki/6_nimmt!


## Links

For the rationale behind and comparison between the implemented algorithms, see `Comparison.md`.

For the programmer's documentation, see `Documentation.md`.

## Remarks

This project was written for the Programming in C# Language.