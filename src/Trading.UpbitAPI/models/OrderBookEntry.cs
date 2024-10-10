namespace Trading.UpbitAPI
{
    // 주문 장부 항목 클래스 (선택 사항)
    public class OrderBookEntry
    {
        public decimal Price { get; set; }       // 가격
        public decimal Quantity { get; set; }    // 수량
        public string Side { get; set; }         // "Bid" 또는 "Ask"
    }
}
