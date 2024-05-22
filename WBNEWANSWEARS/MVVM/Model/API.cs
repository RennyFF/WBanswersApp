using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace WBNEWANSWEARS.MVVM.Model
{
    public class API
    {
        static private string CONTENT_URL = "https://suppliers-api.wildberries.ru/";
        static private string FEEDBACK_URL = "https://feedbacks-api.wildberries.ru/";
        public async Task<NMList> SendPostRequestNMList(string authorization)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string _NMURI = $"{CONTENT_URL}" + "content/v2/get/cards/list?locale=en";

                    StringContent requestDataOrigin = new("{\"settings\": {" +
                        "\"cursor\": {" +
                        "\"limit\": 100" +
                        "}," +
                        "\"filter\": {" +
                        "\"withPhoto\": -1}" +
                        "}" +
                        "}",
                        Encoding.UTF8,
                        "application/json");

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authorization);

                    var response = await client.PostAsync(_NMURI, requestDataOrigin);

                    if (response.IsSuccessStatusCode)
                    {
                        NMList nMList = new();
                        var responseJson  = response.Content.ReadFromJsonAsync<NMList>().Result;
                        nMList = responseJson;
                        while (nMList.cursor.total == 100)
                        {
                            NMList _tempList = await SendPostRequestNMListOverLimit(authorization, nMList.cursor);
                            nMList.cursor = _tempList.cursor;
                            nMList.cards.AddRange(_tempList.cards);
                        }

                        return nMList;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public async Task<NMList> SendPostRequestNMListOverLimit(string authorization, Cursor cursor)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string _NMURI = $"{CONTENT_URL}" + "content/v2/get/cards/list?locale=en";


                    StringContent requestDataIfOverLimit = new("{\"settings\": {" +
                                                                    "\"filter\": {" +
                                                                        "\"withPhoto\": -1 }," +
                                                                    "\"cursor\": {" +
                                                                    $"\"updatedAt\": \"{cursor.updatedAt}\"," +
                                                                    $"\"nmID\": {cursor.nmID}," +
                                                                    $"\"limit\": 100 }}}}}}",
                        Encoding.UTF8,
                        "application/json");
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authorization);

                    var response = await client.PostAsync(_NMURI, requestDataIfOverLimit);

                    if (response.IsSuccessStatusCode)
                    {
                        NMList nMList = new();
                        var responseJson = response.Content.ReadFromJsonAsync<NMList>().Result;
                        nMList = responseJson;
                        return nMList;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public async Task<FeedBackList> SendGetRequestFeedbackList(string authorization, long NMID)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string _FEEDBACKURILIST = $"{FEEDBACK_URL}" + $"api/v1/feedbacks?isAnswered=false&nmId={NMID}&take=5000&skip=0&order=dateAsc";

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authorization);

                    var response = await client.GetAsync(_FEEDBACKURILIST);

                    if (response.IsSuccessStatusCode)
                    {
                        FeedBackList feedBackList = new();
                        var responseJson = response.Content.ReadFromJsonAsync<FeedBackData>().Result;
                        feedBackList = responseJson.data;
                        return feedBackList;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public async Task<int> SendGetUnanswered (string authorization)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string _FEEDBACKURILIST = $"{FEEDBACK_URL}" + $"api/v1/feedbacks/count-unanswered";

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authorization);

                    var response = await client.GetAsync(_FEEDBACKURILIST);

                    if (response.IsSuccessStatusCode)
                    {
                        FeedBackCount feedBackUnAnswered = new();
                        var responseJson = response.Content.ReadFromJsonAsync<FeedBackCountData>().Result;
                        feedBackUnAnswered = responseJson.data;
                        return Convert.ToInt32(feedBackUnAnswered.countUnanswered);
                    }
                    return -1;
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }
        public async Task<bool> SendPatchRequestAnswer(string authorization, string feedbackid, string answertext)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string _FEEDBACKURIANSWER = $"{FEEDBACK_URL}" + $"api/v1/feedbacks";

                    StringContent requestData = new($"{{\"id\": \"{feedbackid}\"," +
                                                    $"\"text\": \"{answertext}\"}}",
                        Encoding.UTF8,
                        "application/json");

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authorization);

                    var response = await client.PatchAsync(_FEEDBACKURIANSWER, requestData);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public async Task<bool> ProcessUserFeedbacksAsync(UsersStructure user)
        {
            // Получаем список NMList
            NMList nmList = await SendPostRequestNMList(user.TokenContent);
            if (nmList == null || nmList.cards == null || !nmList.cards.Any())
                return false;

            // Список для хранения всех отзывов
            List<FeedBack> allFeedbacks = new List<FeedBack>();

            // Проходим по каждому NMID и получаем отзывы
            foreach (var card in nmList.cards)
            {
                try
                {
                    // Получаем список отзывов для каждого NMID
                    await Task.Delay(1000);
                    FeedBackList feedbackList = await SendGetRequestFeedbackList(user.TokenFeedBack, card.nmID);
                    if (feedbackList != null && feedbackList.feedbacks != null)
                    {
                        allFeedbacks.AddRange(feedbackList.feedbacks);
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            // Обрабатываем каждый отзыв из итогового списка
            foreach (var feedback in allFeedbacks)
            {
                string answerText;
                try
                {
                    answerText = SelectAnswerText(user, feedback);
                }
                catch (Exception e)
                {
                    return false;
                }
                if(answerText !=null) { 
                    await Task.Delay(1000);
                    bool patchResult = await SendPatchRequestAnswer(user.TokenFeedBack, feedback.id, answerText);
                    if (!patchResult)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        private string? SelectAnswerText(UsersStructure user, FeedBack feedback)
        {
            List<int> presetIds = ParsePreset(user.Preset);
            HashSet<int> targetRatings = new HashSet<int>();
            foreach (int id in presetIds)
            {
                var answer = user.Answers.FirstOrDefault(a => a.Id == id);
                if (answer != null)
                {
                    var ratings = ParseTargetRating(answer.TargetRating);
                    foreach (var rating in ratings)
                    {
                        targetRatings.Add(rating);
                    }
                    if (targetRatings.Contains(feedback.productValuation))
                    {
                        return answer.Text;
                    }
                }
            }
            return null;
        }

        private List<int> ParsePreset(string preset)
        {
            List<int> ids = preset.Split('/')
                                  .Select(int.Parse)
                                  .ToList();
            return ids;
        }

        private HashSet<int> ParseTargetRating(string targetRating)
        {
            HashSet<int> ratings = new HashSet<int>();
            var parts = targetRating.Split(new[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                if (part.Contains("-"))
                {
                    var rangeParts = part.Split('-');
                    if (rangeParts.Length == 2 && int.TryParse(rangeParts[0], out int start) && int.TryParse(rangeParts[1], out int end))
                    {
                        for (int i = start; i <= end; i++)
                        {
                            ratings.Add(i);
                        }
                    }
                }
                else
                {
                    if (int.TryParse(part, out int rating))
                    {
                        ratings.Add(rating);
                    }
                }
            }

            return ratings;
        }
    }
}