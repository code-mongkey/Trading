namespace Trading.UpbitAPI
{
    // RSI 클래스
    public class RSI
    {
        public int Period { get; set; }                  // RSI 기간 설정: 일반적으로 14
        public List<decimal> ClosePrices { get; set; }   // 종가 데이터 배열
    }
}
