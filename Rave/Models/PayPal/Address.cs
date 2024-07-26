using System;
namespace Rave.Models.PayPal
{
    public class Address
    {
        public Address(string billingaddress, string billingcity, string billingstate, string billingzip, string billingcountry)
        {
            BillingAddress = billingaddress;
            BillingCity = billingcity;
            BillingState = billingstate;
            BillingZip = billingzip;
            BillingCountry = billingcountry;
        }

        public string BillingAddress { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingZip { get; set; }
        public string BillingCountry { get; set; }
    }
}
