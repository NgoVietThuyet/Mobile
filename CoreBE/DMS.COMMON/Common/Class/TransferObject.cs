namespace Common
{
    public class TransferObject
    {
        public bool Status { get; set; }

        public object Data { get; set; }

        public MessageObject MessageObject { get; set; }

        public TransferObject()
        {
            Status = true;
            MessageObject = new MessageObject();
        }
    }

    public class TransferObject<T>
    {
        public bool Status { get; set; }

        public T Data { get; set; }

        public MessageObject MessageObject { get; set; }

        public TransferObject()
        {
            Status = true;
            MessageObject = new MessageObject();
        }
    }
}
