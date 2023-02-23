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
    }
}