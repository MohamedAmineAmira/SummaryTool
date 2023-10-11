using Microsoft.AspNetCore.Mvc;
using Neoledge.AI.Orchestrator.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Neoledge.AI.Orchestrator.Workers
{
    public sealed class Orchestrator : BackgroundService
    {
        private readonly ILogger<Orchestrator> _logger;
        private readonly HttpClient _httpClient;
        public Orchestrator(ILogger<Orchestrator> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await RunAiTask();
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(5_000, stoppingToken);
            }
        }

        private async Task RunAiTask()
        {
            try
            {
                var httpContent = await SearchText(State.Cleaning);
                string httpContentString = await httpContent.ReadAsStringAsync();
                var emptyJsonString = new StringContent("", Encoding.UTF8, "application/json").ReadAsStringAsync().Result;
                var text = JsonConvert.DeserializeObject<Text>(httpContentString);
                if (httpContentString != emptyJsonString)
                {
                    var language = text?.Language;
                    if (text?.State == State.Created)
                    {
                        var httpContentCleaning = await ChangeState(httpContent, State.Cleaning);
                        var putResponseCleaning = await EditText(httpContentCleaning);
                        var urlCleaning = await GetUrlCleaning(language);
                        if (urlCleaning == "NotFound")
                        {
                            var httpContentCleaned = await ChangeState(httpContentCleaning, State.Cleaned);
                            var putResponseCleaned = await EditText(httpContentCleaned);
                        }
                        else
                        {
                            var postResponseCleaning = await CleaningText(httpContentCleaning, "https://localhost:7262/api/Text/edit", urlCleaning);
                            if (!postResponseCleaning.IsSuccessStatusCode)
                            {
                                await ChangeState(httpContent, State.Error);
                            }
                        }

                    }

                    if (text?.State == State.Cleaned)
                    {
                        var httpContentProcessing = await ChangeState(httpContent, State.Processing);
                        var putResponseProcessing = await EditText(httpContentProcessing);
                        var urlProcessing = await GetUrlProcessing(language);
                        var postResponseProcessing = await ProcessingText(httpContentProcessing, "https://localhost:7262/api/Text/edit", urlProcessing);
                        if (!postResponseProcessing.IsSuccessStatusCode)
                        {
                            await ChangeState(httpContent, State.Error);
                        }

                    }

                    if (text?.State == State.Processed)
                    {
                        var httpContentProcessing = await ChangeState(httpContent, State.Done);
                        var putResponseProcessing = await EditText(httpContentProcessing);

                    }


                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
        }
        private async Task<HttpContent> SearchText(State state)
        {
            var response = await _httpClient.GetAsync("https://localhost:7262/api/text/search");

            if (response.IsSuccessStatusCode)
            {
                return response.Content;
            }
            else
            {
                return new StringContent(""); // Return an empty HttpContent for error handling
            }
        }

        private async Task<HttpResponseMessage> CleaningText(HttpContent httpContent, string urlReturn, string urlCleaning)
        {

            var httpContentCleaning = await AddUrl(httpContent, urlReturn);
            var postResponse = await _httpClient.PostAsync(urlCleaning, httpContentCleaning);
            return postResponse;
        }

        private async Task<IActionResult> EditText(HttpContent httpContent)
        {
            var putResponse = await _httpClient.PutAsync("https://localhost:7262/api/text/edit", httpContent);

            if (putResponse.IsSuccessStatusCode)
            {
                return new OkResult();
            }
            else
            {
                var statusCode = (int)putResponse.StatusCode;
                return new ObjectResult($"Failed to edit text. Status code: {statusCode}")
                {
                    StatusCode = statusCode
                };
            }
        }

        private async Task<HttpResponseMessage> ProcessingText(HttpContent httpContent, string url, string urlProcessing)
        {
            var httpContentProcessing = await AddUrl(httpContent, url);
            var postResponse = await _httpClient.PostAsync(urlProcessing, httpContentProcessing);
            return postResponse;
        }

        private async Task<HttpContent> ChangeState(HttpContent httpContent, State state)
        {
            var jsonResponse = await httpContent.ReadAsStringAsync();
            var text = JsonConvert.DeserializeObject<Text>(jsonResponse);
            text.State = state;
            var jsonText = JsonConvert.SerializeObject(text);
            return new StringContent(jsonText, Encoding.UTF8, "application/json");
        }

        private async Task<HttpContent> AddUrl(HttpContent httpContent, string url)
        {
            string content = await httpContent.ReadAsStringAsync();
            var jsonObject = JObject.Parse(content);
            jsonObject["Url"] = url;
            string modifiedContent = jsonObject.ToString();

            // Create a new StringContent with the modified JSON content
            HttpContent modifiedHttpContent = new StringContent(modifiedContent, System.Text.Encoding.UTF8, "application/json");

            return modifiedHttpContent;
        }

        private async Task<string> GetUrlCleaning(string language)
        {
            var response = await _httpClient.GetAsync("https://localhost:7262/api/dataPreprocessor/getUrl/" + language);
            var httpContent = response.Content;
            string httpContentString = await httpContent.ReadAsStringAsync();
            return httpContentString;
        }
        private async Task<string> GetUrlProcessing(string language)
        {
            var response = await _httpClient.GetAsync("https://localhost:7262/api/textAnalyticsToolbox/search/" + language);
            var httpContent = response.Content;
            string httpContentString = await httpContent.ReadAsStringAsync();
            return httpContentString;
        }


    }
}
