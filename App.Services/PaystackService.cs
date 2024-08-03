using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace App.Services
{
    public class PaystackService
    {
        
        public PaystackService() { }
        public static async Task<PaystackBanksResponse?> GetBankListAsync(string paystackSecretKey)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", paystackSecretKey);

                    var response = await client.GetAsync("https://api.paystack.co/bank");
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<PaystackBanksResponse>(await response.Content.ReadAsStringAsync());
                    }
                    else
                    {
                        throw new Exception($"Error: {response.StatusCode}, {response.ReasonPhrase}");
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
        }
        public static async Task<PaystackTransactionResponse?> GetTransactionsAsync(string paystackSecretKey)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", paystackSecretKey);

                    var response = await client.GetAsync("https://api.paystack.co/transaction");
                    if (response.IsSuccessStatusCode)
                    {
                        var rez = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<PaystackTransactionResponse>(rez);
                    }
                    else
                    {
                        throw new Exception($"Error: {response.StatusCode}, {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static async Task<IEnumerable<TransactionData>> GetTransactionsAsync(string paystackSecretKey, string email)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", paystackSecretKey);

                    var response = await client.GetAsync("https://api.paystack.co/transaction");
                    if (response.IsSuccessStatusCode)
                    {
                        var rez = await response.Content.ReadAsStringAsync();
                        var val = JsonConvert.DeserializeObject<PaystackTransactionResponse>(rez);
                        if(val == null) { throw new Exception(); }
                        return val.Data.Where(p => p.Customer.Email == email && p.Status == "success");
                    }
                    else
                    {
                        throw new Exception($"Error: {response.StatusCode}, {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static async Task<PaystackTransferResponse?> InitiateTransferAsync(string paystackSecretKey, string currency, TransferDetail transferDetail)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", paystackSecretKey);

                    var jsonContent = JsonConvert.SerializeObject(transferDetail);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("https://api.paystack.co/transfer", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<PaystackTransferResponse>(await response.Content.ReadAsStringAsync());
                    }
                    else
                    {
                        throw new Exception($"Error: {response.StatusCode}, {response.ReasonPhrase}");
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        public static async Task<PaystackBulkTransferResponse?> InitiateBulkTransferAsync(string paystackSecretKey, string currency, List<TransferDetail> transferDetails )
        {
            try
            {
                PaystackBulkTransferRequest bulkTransferRequest = new PaystackBulkTransferRequest()
                {
                    Currency = currency,
                    Source = "balance",
                    Transfers = transferDetails
                };

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", paystackSecretKey);

                    var jsonContent = JsonConvert.SerializeObject(bulkTransferRequest);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("https://api.paystack.co/transfer/bulk", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<PaystackBulkTransferResponse>(await response.Content.ReadAsStringAsync());
                    }
                    else
                    {
                        throw new Exception($"Error: {response.StatusCode}, {response.ReasonPhrase}");
                    }
                }

            }
            catch(Exception)
            {
                throw;
            }
        }
        public static async Task<PaystackPaymentResponse?> InitializePaymentAsync(string paystackSecretKey, string email, decimal amount, string itemID, string type, string callBack)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", paystackSecretKey);

                    PaystackPaymentInitializationRequest paymentRequest = new ()
                    {
                        Email = email,
                        Amount = amount,
                        Metadata = new Metadata() {CustomFields = [
                            new CustomField() { VariableName = "Type", Value = type},
                            new CustomField() { VariableName = "ItemID", Value = itemID},
                            ] },
                        CallbackUrl = callBack, // Replace with your callback URL
                        Reference = Guid.NewGuid().ToString()
                    };

                    var jsonContent = JsonConvert.SerializeObject(paymentRequest);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("https://api.paystack.co/transaction/initialize", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<PaystackPaymentResponse>(await response.Content.ReadAsStringAsync());
                    }
                    else
                    {
                        throw new Exception($"Error: {response.StatusCode}, {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static async Task<string> VerifyOtpAsync(string paystackSecretKey, string reference, string otp)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", paystackSecretKey);

                var otpVerificationRequest = new
                {
                    reference = reference,
                    otp = otp
                };

                var jsonContent = JsonConvert.SerializeObject(otpVerificationRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://api.paystack.co/charge/submit_otp", content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return $"Error: {response.StatusCode}, {response.ReasonPhrase}";
                }
            }
        }
        public static async Task<CreateRecipientResponse?> CreateTransferRecipient(CreateRecipientRequest request, string key)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
                request.Type = "nuban";
                request.Currency = "NGN";

                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://api.paystack.co/transferrecipient", content);
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CreateRecipientResponse>(responseString);
            }
        }

        public static async Task<DeleteRecipientResponse?> DeleteTransferRecipient(string recipientCode, string key)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
                var response = await client.DeleteAsync($"https://api.paystack.co/transferrecipient/{recipientCode}");
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<DeleteRecipientResponse>(responseString);
            }
        }

    }

    public class PaystackPaymentResponse
        {
            [JsonProperty("status")]
            public bool Status { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("data")]
            public PaymentData Data { get; set; }
        }

        public class PaymentData
        {
            [JsonProperty("authorization_url")]
            public string AuthorizationUrl { get; set; }

            [JsonProperty("access_code")]
            public string AccessCode { get; set; }

            [JsonProperty("reference")]
            public string Reference { get; set; }
    }

       
        public class PaystackBanksResponse
        {
            [JsonProperty("status")]
            public bool Status { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("data")]
            public List<BankData> Data { get; set; }
        }

        public class BankData
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("slug")]
            public string Slug { get; set; }

            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("longcode")]
            public string Longcode { get; set; }

            [JsonProperty("gateway")]
            public string Gateway { get; set; }

            [JsonProperty("pay_with_bank")]
            public bool PayWithBank { get; set; }

            [JsonProperty("active")]
            public bool Active { get; set; }

            [JsonProperty("is_deleted")]
            public bool IsDeleted { get; set; }

            [JsonProperty("country")]
            public string Country { get; set; }

            [JsonProperty("currency")]
            public string Currency { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("createdAt")]
            public string CreatedAt { get; set; }

            [JsonProperty("updatedAt")]
            public string UpdatedAt { get; set; }
    }

    public class PaystackTransferResponse
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }
    public class PaystackBulkTransferRequest
        {
            [JsonProperty("currency")]
            public string Currency { get; set; }

            [JsonProperty("source")]
            public string Source { get; set; }

            [JsonProperty("transfers")]
            public List<TransferDetail> Transfers { get; set; }
        }

        public class TransferDetail
        {
            [JsonProperty("amount")]
            public int Amount { get; set; }

            [JsonProperty("recipient")]
            public string Recipient { get; set; }

            [JsonProperty("reference")]
            public string Reference { get; set; }

            [JsonProperty("reason")]
            public string Reason { get; set; }
    }

        public class PaystackBulkTransferResponse
        {
            [JsonProperty("status")]
            public bool Status { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("data")]
            public List<BulkTransferDataPayStck> Data { get; set; }
        }

        public class BulkTransferDataPayStck
    {
            [JsonProperty("transfer_code")]
            public string TransferCode { get; set; }

            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("amount")]
            public int Amount { get; set; }

            [JsonProperty("currency")]
            public string Currency { get; set; }

            [JsonProperty("recipient")]
            public string Recipient { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("createdAt")]
            public DateTime CreatedAt { get; set; }

            [JsonProperty("updatedAt")]
            public DateTime UpdatedAt { get; set; }

            [JsonProperty("reason")]
            public string Reason { get; set; }
    }

        public class PaystackPaymentInitializationRequest
        {
            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("amount")]
            public decimal Amount { get; set; }

            [JsonProperty("user_id")]
            public string UserID { get; set; }

            [JsonProperty("callback_url")]
            public string CallbackUrl { get; set; }

            [JsonProperty("metadata")]
            public Metadata Metadata { get; set; }

            [JsonProperty("reference")]
            public string Reference { get; set; }
        }

        public class PaystackTransactionResponse
    {
            [JsonProperty("status")]
            public bool Status { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("data")]
            public List<TransactionData> Data { get; set; }

            [JsonProperty("meta")]
            public MetaData Meta { get; set; }
        }

        public class TransactionData
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("domain")]
            public string? Domain { get; set; }

            [JsonProperty("status")]
            public string? Status { get; set; }

            [JsonProperty("reference")]
            public string Reference { get; set; }

            [JsonProperty("amount")]
            public decimal Amount { get; set; }

            [JsonProperty("message")]
            public string? Message { get; set; }

            [JsonProperty("gateway_response")]
            public string? GatewayResponse { get; set; }

            [JsonProperty("paid_at")]
            public DateTime? PaidAt { get; set; }

            [JsonProperty("created_at")]
            public DateTime? CreatedAt { get; set; }

            [JsonProperty("channel")]
            public string? Channel { get; set; }

            [JsonProperty("currency")]
            public string? Currency { get; set; }

            [JsonProperty("ip_address")]
            public string? IpAddress { get; set; }

            [JsonProperty("metadata")]
            public Metadata? Metadata { get; set; }

            [JsonProperty("log")]
            public LogData? Log { get; set; }

            [JsonProperty("fees")]
            public int? Fees { get; set; }

            [JsonProperty("fees_split")]
            public object? FeesSplit { get; set; }

            [JsonProperty("customer")]
            public CustomerData? Customer { get; set; }

            [JsonProperty("authorization")]
            public AuthorizationData? Authorization { get; set; }


            [JsonProperty("order_id")]
            public object? OrderId { get; set; }

            [JsonProperty("paidAt")]
            public DateTime? PaidAtUtc { get; set; }

            [JsonProperty("createdAt")]
            public DateTime? CreatedAtUtc { get; set; }

            [JsonProperty("requested_amount")]
            public int? RequestedAmount { get; set; }

            [JsonProperty("source")]
            public SourceData? Source { get; set; }

            [JsonProperty("connect")]
            public object? Connect { get; set; }

            [JsonProperty("pos_transaction_data")]
            public object? PosTransactionData { get; set; }
        }

        public class Metadata
        {
            [JsonProperty("custom_fields")]
            public List<CustomField> CustomFields { get; set; }
        }

        public class CustomField
        {
            [JsonProperty("display_name")]
            public string DisplayName { get; set; }

            [JsonProperty("variable_name")]
            public string VariableName { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }
        }

        public class LogData
        {
            [JsonProperty("start_time")]
            public long StartTime { get; set; }

            [JsonProperty("time_spent")]
            public int TimeSpent { get; set; }

            [JsonProperty("attempts")]
            public int Attempts { get; set; }

            [JsonProperty("errors")]
            public int Errors { get; set; }

            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("mobile")]
            public bool Mobile { get; set; }

            [JsonProperty("input")]
            public List<object> Input { get; set; }

            [JsonProperty("history")]
            public List<HistoryData> History { get; set; }
        }

        public class HistoryData
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("time")]
            public int Time { get; set; }
        }

        public class CustomerData
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("first_name")]
            public string FirstName { get; set; }

            [JsonProperty("last_name")]
            public string LastName { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("phone")]
            public string Phone { get; set; }

            [JsonProperty("metadata")]
            public object Metadata { get; set; }

            [JsonProperty("customer_code")]
            public string CustomerCode { get; set; }

            [JsonProperty("risk_action")]
            public string RiskAction { get; set; }
        }

        public class AuthorizationData
        {
            [JsonProperty("authorization_code")]
            public string AuthorizationCode { get; set; }

            [JsonProperty("bin")]
            public string Bin { get; set; }

            [JsonProperty("last4")]
            public string Last4 { get; set; }

            [JsonProperty("exp_month")]
            public string ExpMonth { get; set; }

            [JsonProperty("exp_year")]
            public string ExpYear { get; set; }

            [JsonProperty("channel")]
            public string Channel { get; set; }

            [JsonProperty("card_type")]
            public string CardType { get; set; }

            [JsonProperty("bank")]
            public string Bank { get; set; }

            [JsonProperty("country_code")]
            public string CountryCode { get; set; }

            [JsonProperty("brand")]
            public string Brand { get; set; }

            [JsonProperty("reusable")]
            public bool Reusable { get; set; }

            [JsonProperty("signature")]
            public string Signature { get; set; }

            [JsonProperty("account_name")]
            public object AccountName { get; set; }
        }

        public class SourceData
        {
            [JsonProperty("source")]
            public string Source { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("identifier")]
            public object Identifier { get; set; }

            [JsonProperty("entry_point")]
            public string EntryPoint { get; set; }
        }

        public class MetaData
        {
            [JsonProperty("total")]
            public int Total { get; set; }

            [JsonProperty("total_volume")]
            public double TotalVolume { get; set; }

            [JsonProperty("skipped")]
            public int Skipped { get; set; }

            [JsonProperty("perPage")]
            public int PerPage { get; set; }

            [JsonProperty("page")]
            public int Page { get; set; }

            [JsonProperty("pageCount")]
            public int PageCount { get; set; }
        }


    public class CreateRecipientRequest
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("bank_code")]
        public string BankCode { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }

    public class CreateRecipientResponse
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public RecipientData Data { get; set; }
    }

    public class RecipientData
    {
        [JsonProperty("recipient_code")]
        public string RecipientCode { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("bank_code")]
        public string BankCode { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }

    public class DeleteRecipientResponse
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
