namespace Common
{
    public class MessageObject
    {
        public string Code { get; set; }
        public string Language { get; set; }
        public string Message { get; set; }
        public string MessageDetail { get; set; }
        public string MessageType { get; set; } // S (Success), W (Warning), E (Error)
        public string LogId { get; set; }
        public MessageObject()
        {
            Code = string.Empty;
            Message = string.Empty;
            MessageDetail = string.Empty;
            MessageType = string.Empty;
            LogId = string.Empty;
        }
    }
}
