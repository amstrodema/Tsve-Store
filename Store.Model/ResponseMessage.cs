namespace Store.Model
{
    public class ResponseMessage<T>
    {
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public T ? Data { get; set; }
        public int Flag { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Control { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
    }
}