using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skender.Stock.Indicators;
using System.Data;
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
            lblOpenAIResponse.Text = chatResponse;

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

        private async void btnPlaceOrderByGPT_Click(object sender, EventArgs e)
        {
            // Upbit API에서 데이터 가져오기
            string market = "KRW-BTC";
            int count = 200; // 가져올 캔들 수

            var priceData = await _apiClient.GetUpbitCandleData(market, count);

            // 가져온 데이터를 클래스 인스턴스로 변환
            var analysisInput = MapToAnalysisInput(priceData);
            
            // ChatGPT에게 질문하기 (OpenAI API 사용)
            string response = await AskChatGPT(analysisInput);
            
            // 결과 출력
            Console.WriteLine("ChatGPT의 분석 결과:");
            Console.WriteLine(response);
            lblOpenAIResponse.Text = response;
        }



        // UpbitCandle 데이터를 AnalysisInput 클래스에 매핑하는 메서드
        public static AnalysisInput MapToAnalysisInput(List<Candle> candles)
        {
            // 시간 역순으로 받아오기 때문에 역순으로 정렬
            candles.Reverse();
        
            var priceData = new PriceData
            {
                TimeFrame = "1D",
                StartDate = candles[0].CandleDateTimeKst.ToString("yyyy-MM-dd"),
                EndDate = candles[candles.Count - 1].CandleDateTimeKst.ToString("yyyy-MM-dd"),
                Open = new List<decimal>(),
                High = new List<decimal>(),
                Low = new List<decimal>(),
                Close = new List<decimal>()
            };
        
            var volumeData = new VolumeData
            {
                Volume = new List<decimal>()
            };
        
            foreach (var candle in candles)
            {
                priceData.Open.Add(candle.OpeningPrice);
                priceData.High.Add(candle.HighPrice);
                priceData.Low.Add(candle.LowPrice);
                priceData.Close.Add(candle.TradePrice);
        
                volumeData.Volume.Add(candle.CandleAccTradeVolume);
            }
        
            var analysisInput = new AnalysisInput
            {
                PriceData = priceData,
                VolumeData = volumeData,
                // 필요한 경우 Indicators 및 기타 데이터도 설정
                Indicators = new Indicators
                {
                    MovingAverages = new MovingAverages
                    {
                        Periods = new List<int> { 5, 20, 50, 200 },
                        ClosePrices = priceData.Close
                    },
                    RSI = new RSI
                    {
                        Period = 14,
                        ClosePrices = priceData.Close
                    },
                    MACD = new MACD
                    {
                        FastEMA = 12,
                        SlowEMA = 26,
                        SignalLine = 9,
                        ClosePrices = priceData.Close
                    },
                    BollingerBands = new BollingerBands
                    {
                        Period = 20,
                        StandardDeviation = 2,
                        ClosePrices = priceData.Close
                    },
                    FibonacciRetracement = new FibonacciRetracement
                    {
                        HighPrice = priceData.High[priceData.High.Count - 1],
                        LowPrice = priceData.Low[0]
                    },
                    Oscillators = new Oscillators
                    {
                        Stochastic = new Stochastic
                        {
                            KPeriod = 14,
                            DPeriod = 3,
                            ClosePrices = priceData.Close,
                            HighPrices = priceData.High,
                            LowPrices = priceData.Low
                        },
                        CCI = new CCI
                        {
                            Period = 20,
                            TypicalPrices = CalculateTypicalPrices(priceData.High, priceData.Low, priceData.Close)
                        }
                    }
                },
                ChartPatterns = new ChartPatterns
                {
                    OHLCData = new OHLCData
                    {
                        Open = priceData.Open,
                        High = priceData.High,
                        Low = priceData.Low,
                        Close = priceData.Close
                    }
                },
                VolumeAnalysis = new VolumeAnalysis
                {
                    Volume = volumeData.Volume
                },
                AdditionalData = new AdditionalData
                {
                    // 필요에 따라 추가 데이터 설정
                }
            };
        
            return analysisInput;
        }

        // Typical Price 계산 메서드
        public static List<decimal> CalculateTypicalPrices(List<decimal> highPrices, List<decimal> lowPrices, List<decimal> closePrices)
        {
            var typicalPrices = new List<decimal>();
        
            for (int i = 0; i < highPrices.Count; i++)
            {
                decimal typicalPrice = (highPrices[i] + lowPrices[i] + closePrices[i]) / 3;
                typicalPrices.Add(typicalPrice);
            }
        
            return typicalPrices;
        }
        
        // ChatGPT에게 질문하는 메서드 (OpenAI API 사용)
        public async Task<string> AskChatGPT(AnalysisInput analysisInput)
        {
            var keys = _security.LoadKeys();
            if (keys.TryGetValue("OpenAIKey", out var apiKey))
            {
            }
            else
            {
            }

            var client = new OpenAiClient(apiKey);


            // // ChatGPT에게 보낼 프롬프트 생성
            // // 분석 입력 데이터를 JSON 문자열로 변환
            // string inputData = JsonConvert.SerializeObject(analysisInput);
            // string prompt = $"다음은 비트코인 가격 데이터입니다:\n{inputData}\n\n이 데이터를 기반으로 기술적 분석을 해주세요.";


            // 가격 및 거래량 데이터를 Quote 형식으로 변환
            var quotes = new List<Quote>();
            for (int i = 0; i < analysisInput.PriceData.Close.Count; i++)
            {
                quotes.Add(new Quote
                {
                    Date = DateTime.Parse(analysisInput.PriceData.StartDate).AddDays(i),
                    Open = analysisInput.PriceData.Open[i],
                    High = analysisInput.PriceData.High[i],
                    Low = analysisInput.PriceData.Low[i],
                    Close = analysisInput.PriceData.Close[i],
                    Volume = analysisInput.VolumeData.Volume[i]
                });
            }

            // 이동평균선 계산
            var sma20 = quotes.GetSma(20).ToList();
            var sma50 = quotes.GetSma(50).ToList();
            var sma200 = quotes.GetSma(200).ToList();

            // RSI 계산
            var rsi = quotes.GetRsi(14).ToList();

            // MACD 계산
            var macd = quotes.GetMacd(12, 26, 9).ToList();

            // 볼린저 밴드 계산
            var bb = quotes.GetBollingerBands(20, 2).ToList();

            // 필요한 지표 값 추출 (최근 값)
            var latestSma20 = sma20.LastOrDefault();
            var latestRsi = rsi.LastOrDefault();
            var latestMacd = macd.LastOrDefault();
            var latestBb = bb.LastOrDefault();

            // 프롬프트 구성
            string prompt = $"비트코인에 대한 최신 기술적 지표는 다음과 같습니다:\n\n" +
                            $"- 20일 이동평균선(SMA): {latestSma20?.Sma}\n" +
                            $"- RSI(14): {latestRsi?.Rsi}\n" +
                            $"- MACD: {latestMacd?.Macd}\n" +
                            $"- MACD 시그널: {latestMacd?.Signal}\n" +
                            $"- 볼린저 밴드 상한: {latestBb?.UpperBand}\n" +
                            $"- 볼린저 밴드 중간: {latestBb?.Sma}\n" +
                            $"- 볼린저 밴드 하한: {latestBb?.LowerBand}\n\n" +
                             $"이러한 지표들을 기반으로 현재 비트코인 시장의 기술적 분석을 제공해 주세요. 각 지표의 의미와 시장에 대한 영향을 근거와 함께 설명해 주세요.";






            return await client.GetChatCompletionAsync(prompt);


            // // OpenAI API를 통해 ChatGPT에게 질문
            // using (HttpClient client = new HttpClient())
            // {
            //     client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            // 
            //     var requestContent = new
            //     {
            //         model = "gpt-3.5-turbo",
            //         messages = new[]
            //         {
            //             new { role = "user", content = prompt }
            //         },
            //         max_tokens = 1000
            //     };
            // 
            //     var httpContent = new StringContent(JsonConvert.SerializeObject(requestContent), System.Text.Encoding.UTF8, "application/json");
            // 
            //     HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", httpContent);
            //     string responseContent = await response.Content.ReadAsStringAsync();
            // 
            //     // 응답 파싱
            //     var chatResponse = JsonConvert.DeserializeObject<ChatGPTResponse>(responseContent);
            // 
            //     if (chatResponse != null && chatResponse.Choices != null && chatResponse.Choices.Length > 0)
            //     {
            //         return chatResponse.Choices[0].Message.Content;
            //     }
            //     else
            //     {
            //         return "ChatGPT로부터 유효한 응답을 받지 못했습니다.";
            //     }
            // }
        }
    }
}
