namespace Store.Model
{
    public class Email
    {
        public List<string> Recipients { get; set; } = new List<string>();
        public string Message { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }
}