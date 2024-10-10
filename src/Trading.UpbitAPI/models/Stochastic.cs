namespace Trading.UpbitAPI
{
    // 스토캐스틱 오실레이터 클래스
    public class Stochastic
    {
        public int KPeriod { get; set; }                 // %K 기간: 일반적으로 14
        public int DPeriod { get; set; }                 // %D 기간: 일반적으로 3
        public List<decimal> ClosePrices { get; set; }   // 종가 데이터 배열
        public List<decimal> HighPrices { get; set; }    // 고가 데이터 배열
        public List<decimal> LowPrices { get; set; }     // 저가 데이터 배열
    }
}
