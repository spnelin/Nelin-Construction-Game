using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server.ServerObjects
{
    public class CardHand : IList<ICard>
    {
        private List<ICard> Cards { get; set; }

        public Player OwningPlayer { get; private set; }

        public int Count => Cards.Count;

        public bool IsReadOnly => false;

        public ICard this[int index] { get => Cards[index]; set => Cards[index] = value; }

        public CardHand(Player owningPlayer)
        {
            OwningPlayer = owningPlayer;
            Cards = new List<ICard>();
        }

        public List<ICard> ToList()
        {
            return Cards;
        }

        public int IndexOf(ICard item)
        {
            return Cards.IndexOf(item);
        }

        public void Insert(int index, ICard item)
        {
            Cards.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Cards.RemoveAt(index);
        }

        public void Add(ICard item)
        {
            Cards.Add(item);
        }

        public void Clear()
        {
            Cards.Clear();
        }

        public bool Contains(ICard item)
        {
            return Cards.Contains(item);
        }

        public void CopyTo(ICard[] array, int arrayIndex)
        {
            Cards.CopyTo(array, arrayIndex);
        }

        public bool Remove(ICard item)
        {
            return Cards.Remove(item);
        }

        public IEnumerator<ICard> GetEnumerator()
        {
            return Cards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Cards.GetEnumerator();
        }
    }

    public class HiddenCard : ICard
    {
        public CardType CardType => CardType.Hidden;

        public static ICard Card = new HiddenCard();
    }

    public interface ICard
    {
        CardType CardType { get; }
    }

    public enum CardType
    {
        Opportunity,
        Intrigue,
        Hidden
    }
}
