# GridSoccer



EDIT: The english summary of my bachelor's project is now available in the repo


This simple video game is my bachelor's project for computer engineering degree. My aim is to implement reinforcement learning in this game using Unity MLToolkit.


There are two players, Red and green. In each set, a player has the ball, therefore acts as an attacker and the other play as the defender. the aim is to send the ball to the other player's goal while the defender must prevent it. If the attacker and defender reach the same block, the attacker loses its ball and becomes the defender, whereas the defender gets the ball and becomes an attacker.

Here is a picture of the game:




![GridSoccer](https://i.ibb.co/9s9H8Z5/photo-2020-03-03-12-28-13.jpg)


Each player can move four directions and they can only move one block, between cycles. For instance, if the cycle is 0.5s then every 0.5s each player can move.
