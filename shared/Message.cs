using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace shared
{
    public class Message {

        /// <summary>The address of the sender of this message. Only used with incoming messages.</summary>
        [JsonIgnore]
        public IPEndPoint Source { get; set; }

        /// <summary>The addresses where this message is going to be sent. Only used with outgoing messages.</summary>
        [JsonIgnore]
        public HashSet<IPEndPoint> Targets { get; }

        /// <summary>The type that identifies the purpose of the message.</summary>
        public MessageType Type { get; set; }

        /// <summary>The text of the message that is shown to users.</summary>
        public String Text { get; set; }

        /// <summary>Build a new message structure for the UDP communication.</summary>
        public Message() {
            Targets = new HashSet<IPEndPoint>();
        }

    }
}
