namespace finWpf
{
    /// <summary>
    /// class for JSON deserialization
    /// </summary>
    public class Converter
    {
        public int status { get; set; }
        public string message { get; set; }
        public Data data;

        public Converter(int status, string message, Data data)
        {
            this.status = status;
            this.message = message;
            this.data = data;
        }

        public Converter()
        {
            status = 0;
            message = "";
            data = new Data();
        }
    }
}