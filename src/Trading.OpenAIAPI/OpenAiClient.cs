using OpenAI;
using OpenAI.Chat;

namespace Trading.OpenAiAPI
{
    public class OpenAiClient
    {

        private readonly OpenAIClient _openAiClient;
        private readonly string _apiKey;

        public OpenAiClient(string apiKey)
        {
            // OpenAIClient 초기화
            _openAiClient = new OpenAIClient(apiKey);
            _apiKey = apiKey;
        }

        /// <summary>
        /// ChatGPT 모델을 사용하여 채팅 응답을 생성합니다.
        /// </summary>
        /// <param name="prompt">사용자 입력 또는 메시지 내용</param>
        /// <param name="model">사용할 모델 (기본값: gpt-3.5-turbo)</param>
        /// <returns>AI의 응답 메시지</returns>
        public async Task<string> GetChatCompletionAsync(string prompt, string model = "gpt-4o-mini")
        {
            ChatClient client = new ChatClient(model, _apiKey);

            ChatCompletion chatCompletion = client.CompleteChat(
            [
                new UserChatMessage(prompt),
            ]);
            return "";
        }
    }
}
