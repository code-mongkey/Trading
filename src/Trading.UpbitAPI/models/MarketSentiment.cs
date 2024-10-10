namespace Trading.UpbitAPI
{
    // 시장 심리 지표 클래스 (선택 사항)
    public class MarketSentiment
    {
        public DateTime Date { get; set; }       // 날짜
        public decimal Value { get; set; }       // 지표 값 (예: 공포와 탐욕 지수)
    }
}
