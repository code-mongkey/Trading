namespace Trading.UpbitAPI
{
    // OHLC 데이터 클래스
    public class OHLCData
    {
        public List<decimal> Open { get; set; }     // 시가 데이터 배열
        public List<decimal> High { get; set; }     // 고가 데이터 배열
        public List<decimal> Low { get; set; }      // 저가 데이터 배열
        public List<decimal> Close { get; set; }    // 종가 데이터 배열
    }
}
