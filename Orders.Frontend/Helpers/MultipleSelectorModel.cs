namespace Orders.Frontend.Helpers
{
    public class MultipleSelectorModel
    {
        public MultipleSelectorModel(string key, string value)
        {
            Key = key; 
            Value = value; 
        }
        public string Key { get; set; } //Lo que tiene
        public string Value { get; set; } //Lo que almacena
    }

}
