using Newtonsoft.Json.Linq;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using Trading.OpenAiAPI;
using Trading.UpbitAPI;
using Trading.Utiliy;

namespace Trading
{
    public partial class MainForm : Form
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private UpbitApiClient _apiClient;
        private Security _security;

        public MainForm(string accessKey, string secretKey)
        {
            InitializeComponent();
            _accessKey = accessKey;
            _secretKey = secretKey;
            _apiClient = new UpbitApiClient(_accessKey, _secretKey);
            _security = new Security();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadAccountInfo();
            LoadMarketInfo();
            LoadOrderHistory();
            LoadControls();
            txtOpenAIAPIKey.Text = _security.LoadKeys().Where(x => x.Key == "OpenAIKey").FirstOrDefault().Value;
        }

        private async void LoadControls()
        {
            // 모든 시장 정보 가져오기
            var marketsJson = await _apiClient.GetAllMarketsAsync(true);
            var allMarkets = JArray.Parse(marketsJson);

            // 거래 가능한 마켓 필터링 ("market_warning"이 "NONE")
            var activeMarkets = allMarkets
                .Where(m => m["market_warning"].ToString() == "NONE")
                .ToDictionary(m => m["market"].ToString(), m => m);

            cmbMarket.Items.AddRange(activeMarkets.Keys.OrderBy(x => x).ToArray());
        }

        private async void LoadAccountInfo()
        {
            try
            {
                var accountsJson = await _apiClient.GetAccountsAsync();
                var accounts = JArray.Parse(accountsJson);

                // 모든 시장 정보 가져오기
                var marketsJson = await _apiClient.GetAllMarketsAsync(true);
                var allMarkets = JArray.Parse(marketsJson);

                // 거래 가능한 마켓 필터링 ("market_warning"이 "NONE")
                var activeMarkets = allMarkets
                    .Where(m => m["market_warning"].ToString() == "NONE")
                    .ToDictionary(m => m["market"].ToString(), m => m);

                // 보유 중인 코인 중 거래 가능한 것만 선택
                var currencyList = accounts
                    .Select(a => (string)a["currency"])
                    .Where(c => c != "KRW")
                    .ToList();

                var markets = currencyList
                    .Select(c => $"KRW-{c}")
                    .Where(m => activeMarkets.ContainsKey(m))
                    .ToList();

                if (markets.Count == 0) return;

                // 현재가 정보 가져오기
                var tickersJson = await _apiClient.GetTickerAsync(string.Join(",", markets));
                var tickers = JArray.Parse(tickersJson).ToDictionary(t => (string)t["market"], t => t);

                // DataTable 생성
                var dt = new DataTable();
                dt.Columns.Add("화폐", typeof(string));
                dt.Columns.Add("보유 수량", typeof(decimal));
                dt.Columns.Add("매수 평균가", typeof(decimal));
                dt.Columns.Add("현재가", typeof(decimal));
                dt.Columns.Add("평가 금액", typeof(decimal));
                dt.Columns.Add("평가 손익", typeof(decimal));
                dt.Columns.Add("수익률(%)", typeof(decimal));

                foreach (var account in accounts)
                {
                    var currency = (string)account["currency"];
                    var balance = decimal.Parse((string)account["balance"]);
                    var avgBuyPrice = decimal.Parse((string)account["avg_buy_price"]);
                    var market = $"KRW-{currency}";

                    decimal currentPrice = 0;

                    if (currency != "KRW" && tickers.ContainsKey(market))
                    {
                        currentPrice = tickers[market]["trade_price"].Value<decimal>();
                    }

                    var evalAmount = balance * currentPrice;
                    var purchaseAmount = balance * avgBuyPrice;
                    var profitLoss = evalAmount - purchaseAmount;
                    var profitRate = purchaseAmount != 0 ? (profitLoss / purchaseAmount) * 100 : 0;

                    dt.Rows.Add(
                        currency,
                        balance,
                        avgBuyPrice,
                        currentPrice,
                        evalAmount,
                        profitLoss,
                        profitRate
                    );
                }

                dgvAccounts.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"계좌 정보를 가져오는 중 오류가 발생했습니다.\n{ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void LoadMarketInfo()
        {
            try
            {
                // 모든 시장 정보 가져오기
                var marketsJson = await _apiClient.GetAllMarketsAsync();
                var markets = JArray.Parse(marketsJson)
                    .Where(m => m["market"].ToString().StartsWith("KRW-"))
                    .ToList();

                var marketCodes = markets.Select(m => m["market"].ToString()).ToList();

                // 현재가 정보 가져오기
                var tickersJson = await _apiClient.GetTickerAsync(string.Join(",", marketCodes));
                var tickers = JArray.Parse(tickersJson);

                // DataTable 생성
                var dt = new DataTable();
                dt.Columns.Add("시장");
                dt.Columns.Add("현재가", typeof(decimal));
                dt.Columns.Add("전일 대비", typeof(decimal));
                dt.Columns.Add("거래량", typeof(decimal));

                foreach (var ticker in tickers)
                {
                    var market = ticker["market"].ToString();
                    var tradePrice = ticker["trade_price"].Value<decimal>();
                    var changeRate = ticker["signed_change_rate"].Value<decimal>() * 100; // 퍼센트로 변환
                    var accTradeVolume = ticker["acc_trade_volume_24h"].Value<decimal>();

                    dt.Rows.Add(
                        market,
                        tradePrice,
                        changeRate,
                        accTradeVolume
                    );
                }

                dgvMarketInfo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"시장 정보를 가져오는 중 오류가 발생했습니다.\n{ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            try
            {
                var market = cmbMarket.SelectedItem.ToString();
                var side = rdoBuy.Checked ? "bid" : "ask";
                var ordType = cmbOrderType.SelectedItem.ToString();
                var volume = txtVolume.Text.Trim();
                var price = txtPrice.Text.Trim();

                // 입력 값 검증
                if (string.IsNullOrEmpty(market) || string.IsNullOrEmpty(ordType))
                {
                    MessageBox.Show("시장과 주문 유형을 선택하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (ordType == "limit" && (string.IsNullOrEmpty(price) || string.IsNullOrEmpty(volume)))
                {
                    MessageBox.Show("지정가 주문의 경우 가격과 수량을 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (ordType == "price" && string.IsNullOrEmpty(price))
                {
                    MessageBox.Show("시장가 매수의 경우 가격을 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (ordType == "market" && string.IsNullOrEmpty(volume))
                {
                    MessageBox.Show("시장가 매도의 경우 수량을 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var resultJson = await _apiClient.PlaceOrderAsync(market, side, volume, price, ordType);

                var result = JObject.Parse(resultJson);

                MessageBox.Show($"주문이 완료되었습니다.\n주문 번호: {result["uuid"]}", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 주문 내역 갱신
                LoadOrderHistory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"주문 중 오류가 발생했습니다.\n{ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void LoadOrderHistory()
        {
            try
            {
                var ordersJson = await _apiClient.GetOrdersAsync(state: "wait");
                var orders = JArray.Parse(ordersJson);

                var dt = new DataTable();
                dt.Columns.Add("주문 번호");
                dt.Columns.Add("시장");
                dt.Columns.Add("종류");
                dt.Columns.Add("주문 유형");
                dt.Columns.Add("가격", typeof(decimal));
                dt.Columns.Add("수량", typeof(decimal));
                dt.Columns.Add("체결 수량", typeof(decimal));
                dt.Columns.Add("남은 수량", typeof(decimal));
                dt.Columns.Add("상태");
                dt.Columns.Add("취소");

                foreach (var order in orders)
                {
                    dt.Rows.Add(
                        order["uuid"].ToString(),
                        order["market"].ToString(),
                        order["side"].ToString(),
                        order["ord_type"].ToString(),
                        decimal.Parse(order["price"].ToString()),
                        decimal.Parse(order["volume"].ToString()),
                        decimal.Parse(order["executed_volume"].ToString()),
                        decimal.Parse(order["remaining_volume"].ToString()),
                        order["state"].ToString(),
                        "취소"
                    );
                }

                dgvOrderHistory.DataSource = dt;
                dgvOrderHistory.Columns["취소"].DefaultCellStyle.ForeColor = Color.Blue;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"주문 내역을 가져오는 중 오류가 발생했습니다.\n{ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvOrderHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvOrderHistory.Columns["취소"].Index && e.RowIndex >= 0)
            {
                var uuid = dgvOrderHistory.Rows[e.RowIndex].Cells["주문 번호"].Value.ToString();
                CancelOrder(uuid);
            }
        }

        private async void CancelOrder(string uuid)
        {
            try
            {
                var resultJson = await _apiClient.CancelOrderAsync(uuid);
                var result = JObject.Parse(resultJson);

                MessageBox.Show($"주문이 취소되었습니다.\n주문 번호: {result["uuid"]}", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 주문 내역 갱신
                LoadOrderHistory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"주문 취소 중 오류가 발생했습니다.\n{ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnOpenAI_Click(object sender, EventArgs e)
        {
            var keys = _security.LoadKeys();
            if (keys.TryGetValue("OpenAIKey", out var apiKey))
            {
            }
            else
            {
            }

            var client = new OpenAiClient(apiKey);

            // Chat Completion 예제
            var chatPrompt = "안녕하세요! 오늘 날씨는 어떤가요?";
            var chatResponse = await client.GetChatCompletionAsync(chatPrompt);


            // var chatResponse = await client.GetChatCompletionAsync(chatPrompt, "gpt-4");
            // Console.WriteLine("ChatGPT 응답:");
            // Console.WriteLine(chatResponse);
            // 
            // // Completion 예제
            // var prompt = "Once upon a time";
            // var completionResponse = await client.GetCompletionAsync(prompt);
            // Console.WriteLine("\nText Completion 응답:");
            // Console.WriteLine(completionResponse);
        }

        private void txtOpenAIAPIKey_TextChanged(object sender, EventArgs e)
        {
            _security.SaveKey("OpenAIKey", txtOpenAIAPIKey.Text);
        }
    }
}
