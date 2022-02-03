namespace finWpf
{
    /// <summary>
    /// class for JSON deserialization
    /// </summary>
    public class Data
    {
        public string value { get; set; }

        public Data(string value)
        {
            this.value = value;
        }
        public Data()
        {
            value = "";
        }
    }
}
