# Development documentation

This documentation serves as a general overview of the project's structure from the development point of view.
Behavior of concrete functions is documented in their respective docstrings.

## `Game.cs`

This file contains the models for the application.
The main model is `Game` which stores all the information about the game.
This includes:

- Information about players. Information about each player is represented in the model `PlayerInfo`
storing the total number of points received so far and the list of cards in the hand.
- Information about game table. `Game` stores list of four lines of cards represented via the model `Line`.
This stores the cards in the line and provides several methods for interaction with the line.
- History of the cards played.

`Game` provides several methods for interacting with the game state, the most notable being `PlayCards`
which simulates one turn of the game.

There are two reasons for `Game` containing majority of logic for simulating the game.
- The methods are unit-testable.
- It enables this logic to be reused in strategies which simulate the game (such as Monte Carlo).

## `Dealer.cs`

Class `Dealer` provides logic for initializing new games accordingly.

## `Player.cs`

`Player` provides abstract base class for all types of players.
As there are several different types of players with different implementations,
it is necessary to keep as few logic in `Player` as possible.

Therefore `Player` only contains two major abstract functions.
- `Play` which returns the card to be played at the and of each turn.
- `SelectRow` which is called when the played card is lower than all rows and returns the row to be taken in such case.

Files `ClosestValuePlayer.cs`, `MonteCarloPlayer.cs`, `RandomPlayer.cs` contain classes deriving from `Player`
and implementing different strategies to play the game. How these algorithms work is described in `Comparison.md`,
including rationale for those algorithms and their comparison.

Class `UserPlayer.cs` provides an inherited class for interaction with the user.

## `InputHandler.cs`

All the interaction with the user is done via the file `InputHandler.cs`.
This provides functions for retrieving initial information about the game,
printing the game state and interaction with user player during the game.

If in future, the interface of this game changes, i.e. to a graphical interface,
this will be the only place that will need to be changed.

## `GameController.cs`

`GameController` provides logic for simulating one game. It interacts both with the `Game` and with `Player`s.
As the logic for one turn is in `Game`, the logic here is very simple.

## `Program.cs`

Entry point for the application. This glues all the previous parts together, namely initialization, construction of games via `Dealer` and simulation of the games via `GameController`.


