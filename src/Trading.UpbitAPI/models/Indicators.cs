namespace Trading.UpbitAPI
{
    public class Indicators
    {
        public MovingAverages MovingAverages { get; set; }
        public RSI RSI { get; set; }
        public MACD MACD { get; set; }
        public BollingerBands BollingerBands { get; set; }
        public FibonacciRetracement FibonacciRetracement { get; set; }
        public Oscillators Oscillators { get; set; }
    }
}
