namespace BookApi
{
    public interface IMessageProducer
    {
        /// <summary>
        /// Sends a message of type T.
        /// </summary>
        /// <typeparam name="T">The type of the message.</typeparam>
        /// <param name="message">The message to send.</param>
        void SendMessage<T> (T message);

    }
}
