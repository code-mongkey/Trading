namespace Trading.UpbitAPI
{
    // CCI 클래스
    public class CCI
    {
        public int Period { get; set; }                      // CCI 기간: 일반적으로 20
        public List<decimal> TypicalPrices { get; set; }     // (고가 + 저가 + 종가) / 3 의 배열
    }
}
