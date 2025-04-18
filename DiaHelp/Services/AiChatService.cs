using DiaHelp.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DiaHelp.Services
{
    public class AiChatService : IAiChatService
    {
        private const string ApiUrl = "https://api.deepseek.com/v1/chat/completions";
        private readonly HttpClient _client = new();

        public AiChatService()
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "sk-919c99628da046adb97552ed9a4e6e80");
        }

        public async Task<string> GetDiabetesResponseAsync(string userMessage)
        {
            var systemPrompt = @"Ты медицинский консультант по диабету. Отвечай ТОЛЬКО на вопросы о:
        - Диагностике диабета
        - Контроле уровня сахара
        - Инсулинотерапии
        - Диете для диабетиков
        - Осложнениях диабета

        На остальные вопросы отвечай: 'Я могу помочь только с вопросами о диабете.'";

            var requestData = new
            {
                model = "deepseek-chat",
                messages = new[]
                {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = userMessage }
            },
                temperature = 0.3
            };

            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(ApiUrl, content);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseJson);

                return responseObject?.choices[0]?.message?.content?.ToString()
                    ?? "Не удалось получить ответ";
            }
            catch (Exception ex)
            {
                return $"Ошибка: {ex.Message}";
            }
        }
    }
}
