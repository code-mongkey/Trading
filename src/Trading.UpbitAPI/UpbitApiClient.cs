using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;

namespace Trading.UpbitAPI
{
    public class UpbitApiClient
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly HttpClient _httpClient;

        public UpbitApiClient(string accessKey, string secretKey)
        {
            _accessKey = accessKey;
            _secretKey = secretKey;
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// 모든 시장 정보를 가져옵니다.
        /// </summary>
        public async Task<string> GetAllMarketsAsync(bool isDetails = false)
        {
            var url = $"https://api.upbit.com/v1/market/all?isDetails={isDetails.ToString().ToLower()}";
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = await _httpClient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 특정 시장의 현재가 정보를 가져옵니다.
        /// </summary>
        public async Task<string> GetTickerAsync(string market)
        {
            var url = $"https://api.upbit.com/v1/ticker?markets={market}";
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = await _httpClient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 계좌 정보를 가져옵니다.
        /// </summary>
        public async Task<string> GetAccountsAsync()
        {
            var url = "https://api.upbit.com/v1/accounts";
            var token = GenerateJwtToken();

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await _httpClient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 주문을 생성합니다.
        /// </summary>
        public async Task<string> PlaceOrderAsync(string market, string side, string volume, string price, string ordType)
        {
            var url = "https://api.upbit.com/v1/orders";
            var parameters = new Dictionary<string, string>
            {
                { "market", market },
                { "side", side },
                { "volume", volume },
                { "price", price },
                { "ord_type", ordType }
            };

            var token = GenerateJwtToken(parameters);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var content = new FormUrlEncodedContent(parameters);
            var response = await _httpClient.PostAsync(url, content);
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 주문 상세 정보를 조회합니다.
        /// </summary>
        public async Task<string> GetOrderAsync(string uuid)
        {
            var url = "https://api.upbit.com/v1/order";
            var parameters = new Dictionary<string, string>
    {
        { "uuid", uuid }
    };

            var token = GenerateJwtToken(parameters);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var uriBuilder = new UriBuilder(url)
            {
                Query = HttpUtility.ParseQueryString(string.Empty).ToString()
            };

            var response = await _httpClient.GetAsync($"{uriBuilder.Uri}?uuid={uuid}");
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// JWT 토큰을 생성합니다.
        /// </summary>
        private string GenerateJwtToken(Dictionary<string, string>? parameters = null)
        {
            var payload = new Dictionary<string, object>
            {
                { "access_key", _accessKey },
                { "nonce", Guid.NewGuid().ToString() }
            };

            if (parameters != null)
            {
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                foreach (var param in parameters)
                {
                    queryString[param.Key] = param.Value;
                }

                var query = queryString.ToString();
                var queryHash = ComputeHash(query);

                payload.Add("query_hash", queryHash);
                payload.Add("query_hash_alg", "SHA512");
            }

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder jwtEncoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return jwtEncoder.Encode(payload, _secretKey);
        }

        /// <summary>
        /// SHA512 해시를 계산합니다.
        /// </summary>
        private string ComputeHash(string data)
        {
            using (var sha512 = SHA512.Create())
            {
                var hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
