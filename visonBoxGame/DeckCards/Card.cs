using System.Collections.Generic;

namespace visonBoxGame.DeckCards
{
    public class Card
    {
        public static List<string> SuitsArray = new List<string> { "Hearts", "Diamonds", "Clubs", "Spades" };

        public int Value
        {
            get;
            set;
        }

        public string Suite
        {
            get;
            set;
        }

        public string NamedValue
        {
            get
            {
                string name = string.Empty;
                switch (Value)
                {
                    case (14):
                        name = "Ace";
                        break;
                    case (13):
                        name = "King";
                        break;
                    case (12):
                        name = "Queen";
                        break;
                    case (11):
                        name = "Jack";
                        break;
                    default:
                        name = Value.ToString();
                        break;
                }

                return name;
            }
        }

        public string Name
        {
            get
            {
                return NamedValue + " of  " + Suite.ToString();
            }
        }

        public Card(int Value, string Suite)
        {
            this.Value = Value;
            this.Suite = Suite;
        }
    }
}
