using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashQuizz_v2.Models
{
    public class CardStats
    {
        /// <summary>
        /// The card linked to the stats
        /// </summary>
        public Card Card { get; set; }

        /// <summary>
        /// If true, the card is not tested in this training, meaniing the user got it right
        /// </summary>
        public bool IsDone { get; set; }

        /// <summary>
        /// The number of times this card has been shown
        /// </summary>
        public int NumberOTrials { get; set; }

        /// <summary>
        /// The basic contructor, since we want all 3 data there's no other constructors
        /// </summary>
        /// <param name="card">The card, the data will be linked to</param>
        public CardStats(Card card)
        {
            this.Card = card;
            this.NumberOTrials = 0;
            this.IsDone = false;
        }
    }
}
