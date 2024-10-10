namespace Trading.UpbitAPI
{
    // MACD 클래스
    public class MACD
    {
        public int FastEMA { get; set; }                 // 단기 EMA 기간: 예) 12
        public int SlowEMA { get; set; }                 // 장기 EMA 기간: 예) 26
        public int SignalLine { get; set; }              // 신호선 기간: 예) 9
        public List<decimal> ClosePrices { get; set; }   // 종가 데이터 배열
    }
}
