using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;

namespace BIg_Two
{
    class Program
    {
        enum Hands
        {
            Pair,
            Triple,
            FourOfAkind,
            FullHouse,
            Straight,
            Flush,
            StraightFlush
        }

        private static class Results
        {
            /// <summary>
            /// Contains one element per enum.
            /// </summary>
            public static int[] _array = new int[1 + (int)Hands.StraightFlush];
        }

        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //initialise deck
            PlayingCard[] deck = Enumerable.Range(0, 52)
                .Select(x => new PlayingCard(x % 13, x / 13))
                .ToArray();

            //create array for different hands
            int pair = 0;

            const int NumRuns = 1000000;

            //start loop
            for (int i = 0; i < NumRuns; i++)
            {
                FisherYatesShuffler.Shuffle(deck);

                int[] values = new int[13];
                int[] suits = new int[4];

                PlayingCard[] ourHand = deck.Take(13).ToArray();

                for (int c = 0; c <= 12; c++)
                {
                    //loop through the 13 cards and populate an array with the count of each value - makes it easier to see
                    //if we have a pair/three of a kind/four of a kind/full house etc
                    values[ourHand[c].Value]++;
                    suits[ourHand[c].Suit]++;
                }

                //we are only interested in whether we get 1 pair/triple/four of a kind/full house - create booleans to set
                bool _pair = false;
                bool _triple = false;
                bool _four = false;
                bool _fullHouse = false;
                bool _straight = false;
                bool _flush = false;
                bool _straightFlush = false;

                //loop through these counts now to see if we have a pair/triple/four of a kind/full house
                for (int c = 0; c <= 12; c++)
                {
                    if (values[c] == 4 && !_four)
                    {
                        Results._array[(int)Hands.FourOfAkind]++;
                        _four = true;
                    }
                    if (values[c] == 3 && !_triple)
                    {
                        Results._array[(int)Hands.Triple]++;
                        _triple = true;
                    }
                    if (values[c] == 2 & !_pair)
                    {
                        Results._array[(int)Hands.Pair]++;
                        _pair = true;
                    }
                }

                if (_pair && (_triple || _four))
                {
                    Results._array[(int)Hands.FullHouse]++;
                }

                //check for straight now
                //straight - need 5 consecutive numbers
                for (int v = 0; v <= 12; v++)
                {
                    //loop through every card value and check to see if we have a card of this value
                    //if we do, check the next value upwards..and if we have 5 in a row, we have a straight
                    if (values[v] > 0 && !_straight)
                    {
                        bool weHaveTheNextFour = true;

                        //adjust for possibility of rolling from Ace-> 2
                        for (int j = 1; j <= 4; j++)
                        {
                            int index = (v + j) % 13;
                            if (values[index] == 0)
                            {
                                weHaveTheNextFour = false;
                            }
                        }

                        if (weHaveTheNextFour)
                        {
                            //if we get here, must have a straight
                            Results._array[(int)Hands.Straight]++;
                            _straight = true;
                        }
                    }
                }

                //check for flush now
                for (int j = 0; j < 4; j++)
                {
                    if (suits[j] > 4)
                    {
                        _flush = true;
                        Results._array[(int)Hands.Flush]++;
                        break;
                    }
                }

                //check for _straightFlush now
                if (_flush && _straight) //must have a flush and a straight to have a straightFlush!!
                {
                    for (int suit = 0; suit < 4; suit++)
                    {
                        if (suits[suit] > 4 && !_straightFlush) //check to see if this is the suit we have 5 or more cards in
                        {
                            foreach (PlayingCard currentCard in ourHand.Where(card => card.Suit == suit))
                            {

                                bool holeInLine = false;

                                //if we look at the next 4 values up from this card, do we have them in our hand?
                                int currentCardValue = currentCard.Value;
                                for (int j = 1; j <= 4; j++)
                                {
                                    int index = (currentCardValue + j) % 13;
                                    PlayingCard nextCardInThisSuit = ourHand.FirstOrDefault(card =>
                                        card.Value == index && card.Suit == currentCard.Suit);

                                    if (nextCardInThisSuit == null)
                                    {
                                        holeInLine = true;
                                        break;
                                    }
                                }

                                if (!holeInLine)
                                {
                                    _straightFlush = true;
                                    Results._array[(int)Hands.StraightFlush]++;
                                    break;
                                }
                            }
                        }
                    }
                }

            }
            stopwatch.Stop();

            for (Hands type = Hands.Pair; type <= Hands.StraightFlush; type++)
            {
                Console.Write($"probability of {type}");
                Console.Write(' ');
                Console.WriteLine((decimal)Results._array[(int)type] / (decimal)NumRuns);
            }

            Console.WriteLine($"time for {NumRuns} runs was {stopwatch.ElapsedMilliseconds} milliseconds");

            Console.ReadLine();

        }


    }
}
