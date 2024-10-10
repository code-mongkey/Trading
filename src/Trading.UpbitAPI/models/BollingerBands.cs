namespace Trading.UpbitAPI
{
    // 볼린저 밴드 클래스
    public class BollingerBands
    {
        public int Period { get; set; }                  // 이동평균선 기간: 일반적으로 20
        public int StandardDeviation { get; set; }       // 표준편차 배수: 일반적으로 2
        public List<decimal> ClosePrices { get; set; }   // 종가 데이터 배열
    }
}
