namespace Trading.UpbitAPI
{
    // 추가 데이터 클래스
    public class AdditionalData
    {
        public List<decimal> OpenInterest { get; set; }    // 미결제 약정 데이터 (선택 사항)
        public List<OrderBookEntry> OrderBookData { get; set; }  // 주문 장부 데이터 (선택 사항)
        public List<MarketSentiment> MarketSentiment { get; set; } // 시장 심리 지표 (선택 사항)
    }
}
