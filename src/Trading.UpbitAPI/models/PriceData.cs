namespace Trading.UpbitAPI
{
    // 가격 데이터 클래스
    public class PriceData
    {
        public string TimeFrame { get; set; }       // 시간 프레임: "1m", "1h", "1D" 등
        public string StartDate { get; set; }       // 데이터 시작 날짜: "YYYY-MM-DD"
        public string EndDate { get; set; }         // 데이터 종료 날짜: "YYYY-MM-DD"
        public List<decimal> Open { get; set; }     // 시가 데이터 배열
        public List<decimal> High { get; set; }     // 고가 데이터 배열
        public List<decimal> Low { get; set; }      // 저가 데이터 배열
        public List<decimal> Close { get; set; }    // 종가 데이터 배열
    }
}
