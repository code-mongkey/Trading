using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using WebSocketSharp;

namespace Trading.UpbitAPI
{
    public class UpbitWebSocketAPIClient
    {
        private WebSocket _webSocket;

        private void ConnectWebSocket()
        {
            _webSocket = new WebSocket("wss://api.upbit.com/websocket/v1");
            _webSocket.OnMessage += WebSocket_OnMessage;

            _webSocket.Connect();

            var channels = new object[]
            {
        new
        {
            ticket = Guid.NewGuid().ToString(),
            type = "ticker",
            codes = new string[] { "KRW-BTC", "KRW-ETH" }, // 원하는 종목
            isOnlyRealtime = true
        }
            };

            var json = JsonConvert.SerializeObject(channels);
            _webSocket.Send(json);
        }

        private void WebSocket_OnMessage(object sender, MessageEventArgs e)
        {
            var data = e.RawData;
            var str = Encoding.UTF8.GetString(data);
            var json = JObject.Parse(str);

            // 실시간 데이터 처리 로직 추가
        }
    }
}
