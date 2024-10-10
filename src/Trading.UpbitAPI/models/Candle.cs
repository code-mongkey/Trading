using Newtonsoft.Json;

namespace Trading.UpbitAPI
{
    public class Candle
    {
        [JsonProperty("candle_date_time_utc")]
        public DateTime CandleDateTimeUtc { get; set; }

        [JsonProperty("candle_date_time_kst")]
        public DateTime CandleDateTimeKst { get; set; }

        [JsonProperty("opening_price")]
        public decimal OpeningPrice { get; set; }

        [JsonProperty("high_price")]
        public decimal HighPrice { get; set; }

        [JsonProperty("low_price")]
        public decimal LowPrice { get; set; }

        [JsonProperty("trade_price")]
        public decimal TradePrice { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("candle_acc_trade_price")]
        public decimal CandleAccTradePrice { get; set; }

        [JsonProperty("candle_acc_trade_volume")]
        public decimal CandleAccTradeVolume { get; set; }

        [JsonProperty("prev_closing_price")]
        public decimal PrevClosingPrice { get; set; }

        [JsonProperty("change_price")]
        public decimal ChangePrice { get; set; }

        [JsonProperty("change_rate")]
        public decimal ChangeRate{ get; set; }
    }
}
