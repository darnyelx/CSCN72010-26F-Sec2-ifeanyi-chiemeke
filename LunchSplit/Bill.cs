namespace LunchSplit
{
    // The raw bill before it is split.
    public class Bill
    {
        public decimal Subtotal { get; }
        public decimal Tax { get; }
        public TipMode TipMode { get; }
        public decimal TipInput { get; }

        public Bill(decimal subtotal, decimal tax, TipMode tipMode, decimal tipInput)
        {
            Subtotal = subtotal;
            Tax = tax;
            TipMode = tipMode;
            TipInput = tipInput;
        }

        // Subtotal plus tax plus tip. The finished shares must add up to this.
        public decimal GrandTotal
        {
            get
            {
                Splitter splitter = new Splitter();
                decimal tip = splitter.ComputeTip(Subtotal, TipMode, TipInput);
                return Subtotal + Tax + tip;
            }
        }
    }
}
