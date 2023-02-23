using Newtonsoft.Json.Linq;
using SixTakes;

namespace SixTakesUnitTest
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void TestCardValue()
        {
            var tests = new []
            {
                new { expected = 1, value = 37},
                new { expected = 7, value = 55},
                new { expected = 3, value = 20},
                new { expected = 2, value = 25},
                new { expected = 5, value = 66},
                new { expected = 1, value = 104},
            };

            foreach (var test in tests)
            {
                Assert.AreEqual(test.expected, Line.CardValue(test.value));
            }
        }

        [TestMethod]
        public void TestValue()
        {
            var tests = new[]
            {
                new { expected = 1, cards = new List<int>{ 1, } },
                new { expected = 13, cards = new List<int> { 10, 30, 55 } },
                new { expected = 10, cards = new List<int> { 11, 12, 13, 14, 15 } },
            };

            foreach (var test in tests)
            {
                Line line = new Line(test.cards.First());
                for (int i = 1; i < test.cards.Count; i++) line.InsertCard(test.cards[i]);

                Assert.AreEqual(test.expected, line.Value);
            }
        }

        [TestMethod]
        public void TestInsertCard()
        {
            var cards = new List<int> { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21};

            Line line = new Line(cards.First());
            for (int i = 0; i < cards.Count; i++)
            {

                if (i > 0)
                {
                    int ret = line.InsertCard(cards[i]);
                    Assert.AreEqual(ret > 0, i % 5 == 0);
                }

                Assert.AreEqual(line.Cards.Count, i % 5 + 1);
                for (int j = 0; j <= i % 5; j++) Assert.AreEqual(line.Cards[i % 5 - j], cards[i - j]);
            }
        }

        [TestMethod]
        public void TestGameInsertCard()
        {
            Game game = new Game(new List<Line> { new Line(1), new Line(2), new Line(3), new Line(4) }, null);
            Assert.AreEqual(4, game.History.Count);
            var tests = new[]
            {
                new { expected = 0, card = 5, line = 0 },
                new { expected = 0, card = 6, line = 0 },
                new { expected = 0, card = 7, line = 1 },
                new { expected = 0, card = 8, line = 0 },
                new { expected = 0, card = 9, line = 0 },
                new { expected = 6, card = 10, line = 0 },
                new { expected = 0, card = 11, line = 0 },
            };

            foreach (var test in tests)
            {
                Assert.AreEqual(test.expected, game.InsertCard(test.line, test.card));
            }

            Assert.AreEqual(2, game.Lines[0].Cards.Count);
            Assert.AreEqual(2, game.Lines[1].Cards.Count);
            Assert.AreEqual(10, game.Lines[0].Cards.First());
            Assert.AreEqual(11, game.History.Count);
        }

        Game GetDefaultGame()
        {
            Game ret = new Game(new List<Line> { new Line(25), new Line(33), new Line(71), new Line(80) }, null);
            ret.InsertCard(1, 42);
            ret.InsertCard(2, 72);
            return ret;
        }

        [TestMethod]
        public void TestGetLineToPlay()
        {
            var game = GetDefaultGame();
            var tests = new[]
            {
                new { expected = (int?)1, card =  55 },
                new { expected = (int?)0, card =  41 },
                new { expected = (int?)null, card =  20 },
            };

            foreach (var test in tests)
            {
                Assert.AreEqual(test.expected, game.GetLineToPlay(test.card));
            }
        }

        [TestMethod]
        public void TestCheapestRow()
        {
            var game = GetDefaultGame();
            Assert.AreEqual(0, game.CheapestRow());
            game.InsertCard(0, 30);
            Assert.AreEqual(2, game.CheapestRow());
            game.InsertCard(0, 73);
            Assert.AreEqual(2, game.CheapestRow());
            game.InsertCard(0, 74);
            Assert.AreEqual(2, game.CheapestRow());
        }

        [TestMethod]
        public void TestPlayCards()
        {
            var game = GetDefaultGame();

            game.Players = new List<PlayerInfo> { new PlayerInfo(new List<int> { 2, 90 }), new PlayerInfo(new List<int> { 37, 55 }), };

            game.PlayCards(new List<int> { 2, 37 });

            Assert.AreEqual(2, game.Players[0].Points);
            Assert.AreEqual(0, game.Players[1].Points);
            Assert.AreEqual(2, game.Lines[0].Cards.Count);
        }
    }
}