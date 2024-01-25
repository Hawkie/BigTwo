using System;

namespace BIg_Two
{
    public class PlayingCard
    {
        private readonly int _value;
        private readonly int _suit;

        public int Value => _value;
        public string ValueName => ValueToName(_value);

        public int Suit => _suit;
        public string SuitName => SuitToName(_suit);

        public PlayingCard(int value, int suit)
        {
            this._value = value;
            this._suit = suit;
        }

        private string ValueToName(int n)
        {
            switch (n)
            {
                case 0:
                    return "Ace";
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    return (n + 1).ToString();
                case 10:
                    return "Jack";
                case 11:
                    return "Queen";
                case 12:
                    return "King";
                default:
                    throw new ArgumentException("Unrecognized card value.");

            }
        }

        private string SuitToName(int s)
        {
            switch (s)
            {
                case 0:
                    return "Clubs";
                case 1:
                    return "Diamonds";
                case 2:
                    return "Spades";
                case 3:
                    return "Hearts";
                default:
                    throw new ArgumentException("Unrecognized card suit");
            }
        }

        public override string ToString()
        {
            return $"{ValueName} of {SuitName}";
        }
    }
}
