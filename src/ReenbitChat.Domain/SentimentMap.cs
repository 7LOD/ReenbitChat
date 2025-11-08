using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitChat.Domain
{
    public static class SentimentMap
    {
        public static int ToScore(Sentiment s) => s switch
        {

            Sentiment.Positive => 1,
            Sentiment.Negative => -1,
            _ => 0
        };
    }
}
