using System.Collections.Concurrent;

namespace shared
{
    /// <summary>A basis for the server and client containing the shared functionality.</summary>
    public class BaseApplication {
        /// <summary>A queue containing the received messages.</summary>
        public ConcurrentQueue<Message> IncomingQueue { get; }

        /// <summary>A queue containing the messages going to be sent.</summary>
        public ConcurrentQueue<Message> OutgoingQueue { get; }

        /// <summary>Build and initialize a new base application structure.</summary>
        public BaseApplication() {
            IncomingQueue = new ConcurrentQueue<Message>();
            OutgoingQueue = new ConcurrentQueue<Message>();
        }

    }
}
