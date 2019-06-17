using System;

namespace shared
{
    public class Message {

        /// <summary>The type that identifies the purpose of the message.</summary>
        public MessageType Type { get; set; }

        /// <summary>The text of the message that is shown to users.</summary>
        public String Text { get; set; }

    }
}
