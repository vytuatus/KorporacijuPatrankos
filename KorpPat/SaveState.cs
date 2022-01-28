using System;
using System.Collections.Generic;
using System.Text;

namespace TRexGame
{
    // marks this class as seriazable. puts it in a storable format
    [Serializable]
    public class SaveState
    {
        public int HighScore { get; set; }
        public DateTime HighscoreDate { get; set; }

    }
}
