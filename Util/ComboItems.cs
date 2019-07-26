namespace ClassMonitor3.Util
{
    public class ComboItems
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public override string ToString() {
            return Text;
        }
        public ComboItems(string value,string text) {
            Text = text;
            Value = value;
        }
    }
}
