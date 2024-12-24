//using Humanizer;
//using Microsoft.AspNetCore.Mvc;
//using Org.BouncyCastle.Asn1.Ocsp;
//using System;
//using System.Diagnostics;

//namespace tawjihy.Areas.Admin.Controllers
//{
//    public class TestingController : BaseController
//    {
//        private readonly IHttpClientFactory _httpClientFactory;

//        public TestingController(IHttpClientFactory httpClientFactory)
//        {
//            _httpClientFactory = httpClientFactory;
//        }

//        [HttpGet]
//        public async Task<IActionResult> SendNumbersForTesting()
//        {
//            string baseUrl = "http://shabiba-001-site1.htempurl.com/Home/Search";

//            using (HttpClient httpClient = _httpClientFactory.CreateClient())
//            {



//                var formData = new Dictionary<string, string>
//        {
//            { "seatNumber", "" }, // This will be replaced with each number.
//            { "__RequestVerificationToken", "CfDJ8Hhjqb3ri5lGk1t5bsbE5bbpbowIJ9JfVVPNylVGuaiKOJekPblx0JDrh2_1TrIDIcZt8E90OjaZqEQUL3OfaLth-PEKT7ABWRvxWlNtVYynupasZJwaGq_wfyov33pf-XD7P5t1pLQwlxO-Tl14R6Y" }, // Use the dynamic token here.
//        };


//                // Replace '1000' with the total number of requests you want to send.
//                int numberOfRequests = 101261;

//                var stopwatch = new Stopwatch();
//                stopwatch.Start(); // Start the stopwatch.
//                for (int i = 101001; i <= numberOfRequests; i++)
//                {
//                    if (stopwatch.Elapsed.TotalSeconds >= 60)
//                    {
//                        break;
//                    }

//                    string numberToSend = i.ToString("D6");



//                    formData["seatNumber"] = numberToSend;

//                    var httpContent = new FormUrlEncodedContent(formData);

//                    try
//                    {
//                        var response = await httpClient.PostAsync(baseUrl, httpContent);
//                    }
//                    catch (HttpRequestException ex)
//                    {

//                    }
//                    await Task.Delay(5);
//                }
//                stopwatch.Stop();
//            }

//            // Redirect to a view or perform any other action after sending the requests.
//            return Redirect("fgjhfgd");
//        }


//    }

//}
