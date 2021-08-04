using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server.ServerObjects
{
    public class Deck<T>
    {
        private List<T> DrawPile { get; set; }

        public List<T> DiscardPile { get; private set; }

        public bool DiscardReshuffle { get; private set; }

        private Random rand = new Random();

        public Deck(List<T> initialDeck, bool discardReshuffle = true)
        {
            //Copy the deck, so we don't accidentally cause side effects
            DrawPile = initialDeck.ToList();
            Shuffle();
            DiscardPile = new List<T>();
            DiscardReshuffle = discardReshuffle;
        }

        public void Shuffle()
        {
            List<T> oldDeck = DrawPile;
            DrawPile = new List<T>();
            while (oldDeck.Count > 0)
            {
                int index = rand.Next(0, oldDeck.Count);
                DrawPile.Add(oldDeck[index]);
                oldDeck.RemoveAt(index);
            }
        }

        public List<T> Draw(int numberCards)
        {
            if (numberCards < 0)
            {
                throw new ArgumentOutOfRangeException("Number of cards to be drawn cannot be less than zero.");
            }
            List<T> ret = new List<T>();
            for (int i = 0; i < numberCards; i++)
            {
                if (DrawPile.Count > 0)
                {
                    ret.Add(DrawPile[0]);
                    DrawPile.RemoveAt(0);
                }
                else if (DiscardReshuffle && DiscardPile.Count > 0)
                {
                    DrawPile = DiscardPile;
                    DiscardPile = new List<T>();
                    Shuffle();
                    ret.Add(DrawPile[0]);
                    DrawPile.RemoveAt(0);
                }
            }
            return ret;
        }

        public void Discard(params T[] discardedCards)
        {
            DiscardPile.AddRange(discardedCards);
        }
    }
}
