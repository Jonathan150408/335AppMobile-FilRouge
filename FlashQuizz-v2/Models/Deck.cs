using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashQuizz_v2.Models
{
    public class Deck
    {
        /// <summary>
        /// Positive unique integer that references an instance of a Deck
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// A string that defines the Deck's name
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// A facultative string that describe the Deck
        /// </summary>
        public string Description { get; set; } = string.Empty;
        public ObservableCollection<Card> Cards { get; set; }
        public int CardCount { get; set; }

        public Deck()
        {
            Cards = new ObservableCollection<Card>();
        }
    }
}
