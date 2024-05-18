using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace ConsoleApp1
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

                    StringContent requestData = new("{\"settings\": {" +
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

                    var response = await client.PostAsync(_NMURI, requestData);

                    if (response.IsSuccessStatusCode)
                    {
                        NMList nMList = new();
                        var responseJson  = response.Content.ReadFromJsonAsync<NMList>().Result;
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
                        string filePath = "feedbacks.json";
                        string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(feedBackList, Newtonsoft.Json.Formatting.Indented);
                        File.WriteAllText(filePath, jsonContent);
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
    }
}