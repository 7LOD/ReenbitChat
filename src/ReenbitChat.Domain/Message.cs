using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitChat.Domain
{
    public enum Sentiment { None = 0, Positive = 1, Neutral = 2, Negative = 3}
    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Room { get; set; } = "general";
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public Sentiment Sentiment { get; set; } = Sentiment.None;
    }
}
