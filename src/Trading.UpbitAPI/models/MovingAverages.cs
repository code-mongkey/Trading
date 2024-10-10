namespace Trading.UpbitAPI
{
    public class MovingAverages
    {
        public List<int> Periods { get; set; }           // 이동평균선 기간 설정: 예) [5, 20, 50, 200]
        public List<decimal> ClosePrices { get; set; }   // 종가 데이터 배열
    }
}
