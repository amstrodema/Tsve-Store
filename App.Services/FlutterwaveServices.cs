using System.Text;
//using Rave.Models.MobileMoney;
//using Rave.Models.VirtualCard;
//using Rave.Models.Subaccount;
//using Rave.Models.Tokens;
//using Rave.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
//using Microsoft.Extensions.Configuration;
//using Rave.api;
//using Rave.Models.Charge;
//using Rave.Models.Account;
//using Rave.Models.Card;
//using Rave.Models.Validation;
using NUnit.Framework;
//using Rave;
using System.Net.NetworkInformation;
using System.Net.Http;
//using Rave.Models.Banks;
using Store.Model;

namespace App.Services
{
    public class FlutterwaveServices
    {

        private static bool IsLive = true;
        private static string PbKey = "FLWPUBK_TEST-5a4247447fdfcf206cf26493ab0d8834-X";
        private static string ScKey = "FLWSECK_TEST-92dc7a4adc7a74b2629954bd9e5c104e-X";
        private static string Encryption = "FLWSECK_TESTc2411de73a0e";
        //private static RaveConfig raveConfig = new RaveConfig(PbKey, ScKey, false);
        //private static CardParams payload;
        //private static ChargeCard cardCharge;
        //private static RaveResponse<Rave.Models.Card.ResponseData> cha;
        private readonly HttpClient _httpClient;

        public FlutterwaveServices(IHttpClientFactory httpClientFactory)
        {

            if (IsLive)
            {
                PbKey = "FLWPUBK-c715146f541da80f2202baa859c83bd0-X";
                ScKey = "FLWSECK-894363394ecdc2f5f9146d344c2249cd-18bb3e50026vt-X";
                Encryption = "894363394ecdee6e3ae94c84";
            }

            _httpClient = httpClientFactory.CreateClient();
            // Replace 'YOUR_SECRET_KEY' with your actual secret key
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + ScKey);
            _httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");

        }
        //public static ResponseMessage<string> Pay(User user, string cardNo, string cvv, string expiry, string tranxRef, int amt, string pin)
        //{
        //    try
        //    {
        //        ResponseMessage<string> responseMessage = new ResponseMessage<string>();
        //        cardCharge = new ChargeCard(raveConfig);
        //        var exp = expiry.Split("-");
        //        payload = new CardParams(PbKey, ScKey, user.Fname, user.LName, user.Email, amt, "NGN")
        //        {
        //            CardNo = cardNo,
        //            Cvv = cvv,
        //            Expirymonth = exp[1],
        //            Expiryyear = exp[0],
        //            TxRef = tranxRef,
        //            PhoneNumber = user.Tel,
        //            Pin = pin
        //        };

        //        cha = cardCharge.Charge(payload).Result;

        //        if (cha.Status.ToLower() != "success")
        //        {
        //            responseMessage.StatusCode = 201;
        //            responseMessage.Message = cha.Message;
        //        }
        //        else
        //        {
        //            responseMessage.StatusCode = 200;
        //        }

        //        return responseMessage;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public static ResponseMessage<Tuple<string, decimal>> OTP(string otp)
        //{
        //    try
        //    {
        //        ResponseMessage<Tuple<string, decimal>> responseMessage = new ResponseMessage<Tuple<string, decimal>>();
        //        if (cha.Message == "AUTH_SUGGESTION" && cha.Data.SuggestedAuth == "PIN")
        //        {
        //            //payload.Pin = pin;
        //            payload.Otp = otp;
        //            payload.SuggestedAuth = "PIN";
        //            cha = cardCharge.Charge(payload).Result;
        //        }

        //        if (cha.Status.ToLower() != "success")
        //        {
        //            responseMessage.StatusCode = 201;
        //            responseMessage.Message = cha.Message;
        //        }
        //        else
        //        {
        //            responseMessage.StatusCode = 200;
        //            responseMessage.Data = Tuple.Create(payload.TxRef, payload.Amount);
        //        }

        //        return responseMessage;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public void Transfer()
        //{
        //    var banks = TransferService.GetBankList();
        //}

        //public async Task<ResponseMessage<string>> CreateTransfer(int accountNo)
        //{
        //    // Replace with your actual Flutterwave API endpoint
        //    string apiUrl = "https://api.flutterwave.com/v3/transfers";
        //    var banks = TransferService.GetBankList();
        //    ResponseMessage<string> responseMessage = new ResponseMessage<string>();

        //    // Replace with your actual transfer data
        //    string jsonData = @"{
        //    ""account_bank"": ""044"",
        //    ""account_number"": ""0690000040"",
        //    ""amount"": 200,
        //    ""narration"": ""Payment for things"",
        //    ""currency"": ""NGN"",
        //    ""reference"": ""jh678b3kol1Z"",
        //    ""callback_url"": ""https://webhook.site/b3e505b0-fe02-430e-a538-22bbbce8ce0d"",
        //    ""debit_currency"": ""NGN""
        //}";

        //    using (var content = new StringContent(jsonData, Encoding.UTF8, "application/json"))
        //    {
        //        var response = await _httpClient.PostAsync(apiUrl, content);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            //string responseBody = await response.Content.ReadAsStringAsync();
        //            responseMessage.StatusCode = 200;
        //            responseMessage.Message = "Completed!";
        //        }
        //        else
        //        {
        //            responseMessage.StatusCode = 201;
        //            responseMessage.Message = "Request failed";
        //            //return StatusCode((int)response.StatusCode, $"Request failed with status code {response.StatusCode}");
        //        }
        //    }

        //    return responseMessage;
        //}
    }

}