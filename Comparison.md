# Comparison of different algorithms to play Six Takes

In this document the rationale for the algorithms are explained
and are compared to one another and to an non-professional player of the game (me).

## Methodology

Comparison to a human player was always done in a 1 to 4 settings,
i.e. the human played against four copies of the algorithm.
The total score over 5 games was computed and for the algorithm the average over the four copies was considered.

Note that this results in rather illustrative results for the different algorithms.


## Random player

Though the rules of the game are quite simple, there is no obvious strategy
how to play the game to be at least decent.
Thus the natural baseline algorithm is the random player.
Is it that people play the game as bad as a randomly playing player?

The result is quite clear: 5 to 84.25 in favor to the human player.
This clearly indicates that we can do much better than just the random strategy.


## Cheapest row random player

One obvious place where we can improve, is the selection of the line when the card
does not fit to any of the rows.

Instead of taking a random, we can view the selection of the line as an independent decision.
A good choice would be to select the cheapest row.

Note: Possibly better would be to select
the rows such that the gain for the player minus the average of gains for other players is minimized,
i.e. it might be beneficial to take a worse decision just to harm other players. This approach was not tried though.

Result against the completely random player is:

- 154:237 for a 2 player game
- 1191:1763 for a 8 player game


We see that this beats the complete random player stably and that the advantage decreases with increasing number of players.

The score of human player against the algorithm were 62:66.5.

We see that the human is still slightly better, but the defeat is not anywhere close to being decisive.

## Closest value player

With the line selection optimized, we turn to the card playing itself.

There are certain tips that seem to be useful when playing.
Those include:

- Generally try to get rid of higher cards early.
- Try to get rid of very low value cards when possible.
- If there are more players, try to play the closes card from above as possible.
- Do not play after a row with 5 cards in it.
- If you cannot play a card higher than the lowest row, play the highest of these cards.

This heuristic player combines the last three tips.
It works followingly:

1. If there is a card higher than any line with less than five cards, pick the closest one.
2. If not, choose the highest card lower than all rows.
3. If there is no such card, just play the highest.

Results compared to the previous algorithms:

- 193:355 in 2 players game (against the Random player)
- 958:1942 in 8 players game (against the Random player)
- 177:261 in 2 players game (against the Cheapest row random player)
- 1156:1741 in 8 players game (against the Cheapest row random player)

Which confirms that these tips are generally useful and we can improve upon the random strategy further.

The results again human are 48:70.

Surprisingly, even though the strategy beats the previous two steadily, it struggles against human player more than the previous strategy.
One reason for this might be that due to the deterministic nature of this algorithm, it is easier for the human player to counter it, even with uncomplete knowledge of the game state.

## Monte Carlo

Unlike the random algorithms the Closest value player performs well in most rounds.
In some, however, it fails to predict some more complicated patterns resulting in huge score gain.

A natural way how to design an algorithm avoiding these mistakes is to perform minimax on the game tree.
As the tree is gigantic however, we can try the Monte Carlo method - generate several randomly played instances a consider the worst case.

The results are following:

- 184:293 in 2 players game (against the Random player)
- 1202:1727 in 8 players game (against the Random player)
- 210:214 in 2 players game (against the Cheapest row random player)
- 1377:1422 in 8 players game (against the Cheapest row random player)
- 268:220 in 2 players game (against the Closest value player)
- 1494:1342 in 8 players game (against the Closest value player)

The algorithm loses to the Closest value player.
In the two players game, it is not better than the random algorithm, in more players it gets closer to the performance of Closest value player.

The result against the human player is 65:54.


This is the first algorithm to beat the human player, though slightly.

## Expected value Player

The idea in Monte Carlo seems to be good and working for more players, but especially for fewer players, it has problems.
Possible cause is that the worst case scenarios usually do not happen. As a result a very strong moves which in extremely rare case backfire are never played.

Thus it makes sense not to consider the worst case for each move, but the average case, i.e. approximate the expected value of score gain for each move.
For playing against a random player this approximates the optimum.

The results are following:

- 96:362 in 2 players game (against the Random player)
- 880:2099 in 8 players game (against the Random player)
- 156:272 in 2 players game (against the Cheapest row random player)
- 996:1958 in 8 players game (against the Cheapest row random player)
- 195:257 in 2 players game (against the Closest value player)
- 1627:1425 in 8 players game (against the Closest value player)
- 169:263 in 2 players game (against the Monte Carlo player)
- 1344:1551 in 8 players game (against the Monte Carlo player)

The result against human player is 80:67.75.

This seems to be the best algorithm so far, though against the Closest value player, the results are very close.
It also beats the human player quite convincingly.

We assume that these results are still far from what is the optimal approach for this game.

