using System.Net.Http.Json;

namespace SuggestionBot.Modules.Suggestions
{
    public class SuggestionService
    {
        private readonly HttpClient _client;

        public SuggestionService(HttpClient client)
        {
            _client = client;
        }


        public async Task<Suggestion> GetSuggestionAsync() =>
            await _client.GetFromJsonAsync<Suggestion>("http://www.boredapi.com/api/activity");
    }
}