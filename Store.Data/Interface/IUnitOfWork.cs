using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Interface
{
    public interface IUnitOfWork
    {

        IKnowledge Knowledge { get; }
        ISearchKeyword SearchKeywords { get; }
        IFavourite Favourites { get; }
        IUser Users { get; }
        IOrder Orders { get; }
        ICategory Categories { get; }
        IBillingDetail BillingDetails { get; }
        ICustomer Customers { get; }
        IFeature Features { get; }
        IFeatureOption FeatureOptions { get; }
        IFile Files { get; }
        IItem Items { get; }
        IItemFeature ItemFeatures { get; }
        IKey Keys { get; }
        INotification Notifications { get; }
        IOrderItem OrderItems { get; }
        IReview Reviews { get; }
        IShippingDetail ShippingDetails { get; }
        IStore Stores { get; }
        ITracking Trackings { get; }
        IPayment Payments { get; }
        ITransaction Transactions { get; }
        ILoginMonitor LoginMonitors { get; }
        Task<int> Commit();
        void Rollback();
        IGroup Groups { get; }

        IBrand Brands { get; }
        IOffer Offers { get; }
        ISlide Slides { get; }
    }
}
