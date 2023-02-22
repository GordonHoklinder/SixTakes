namespace SixTakes
{
    internal class Program
    {
        static Dealer? Dealer { get; set; }

        static Game? Game { get; set; }

        static List<Player> PlayerList { get; set; } = new List<Player>();

        static int Rounds { get; set; } = 0;



        static void Main(string[] args)
        {
            PlayerList = InputHandler.GetPlayers();
            Rounds= InputHandler.GetRounds();
            Dealer = new Dealer(PlayerList.Count);
            for (int i = 0; i < Rounds; i++)
            {
                Game = Dealer?.Deal();
                for (int j = 0; j < PlayerList.Count; j++)
                {
                    PlayerList[j].Game = Game;
                }
                new GameController(Game, PlayerList).Play();
            }
        }
    }
}