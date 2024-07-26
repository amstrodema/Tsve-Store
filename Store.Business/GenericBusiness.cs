using App.Services;
using Store.Model;

namespace Store.Business
{
    public class GenericBusiness
    {
        public static bool IsLocked { get; set; } = false;
        //private static Guid OrleanStore { get; set; } =  //Guid.Parse("14c95916-0f4f-4d03-b7da-d7066a39d069");
        public Guid StoreID { get; set; } = Guid.Parse("14c95916-0f4f-4d03-b7da-d7066a39d069");
        public static string ShoppingCurrency { get; set; } = "NGN";
        public static string ShoppingCurrencySymbol { get; set; } = "₦";
        public GenericBusiness()
        {
            StoreID = Guid.Parse("14c95916-0f4f-4d03-b7da-d7066a39d069");
            //try
            //{
            //    var vrr = FileService.ReadFromFile("aexxj");
            //    vrr = vrr.Substring(1, vrr.Length - 2);

            //}
            //catch (Exception e)
            //{
            //    FileService.WriteToFile("\n\n" + e, "ErrorLogs");
            //}

        }
    }
}