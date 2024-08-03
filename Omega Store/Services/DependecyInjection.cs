using App.Logger.Business;
using App.Logger.IRepos;
using App.Logger.Repos;
using App.Services;
using Microsoft.IdentityModel.Logging;
using Store.Business;
using Store.Data;
using Store.Data.Interface;
using Store.Data.Repository;

namespace Omega_Store.Services
{
    public class DependecyInjection
    {
        public static void Register(IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            services

                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .Configure<GoogleCSESettings>(config.GetSection("GoogleCSESettings"))
                .AddSingleton<GenericBusiness>()
                .AddSingleton<GoogleCSEService>()
                .AddTransient<LoginValidator>()
                .AddScoped<DbContext>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<ISearchKeyword, SearchKeywordRepository>()
                .AddScoped<IKnowledge, KnowledgeRepository>()
                .AddScoped<IUser, UserRepository>()
                .AddScoped<IOrder, OrderRepository>()
                .AddScoped<ICategory, CategoryRepository>()
                .AddScoped<IBillingDetail, BillingDetailRepository>()
                .AddScoped<ICustomer, CustomerRepository>()
                .AddScoped<IFeature, FeatureRepository>()
                .AddScoped<IFeatureOption, FeatureOptionRepository>()
                .AddScoped<IFile, FileRepository>()
                .AddScoped<IItem, ItemRepository>()
                .AddScoped<IItemFeature, ItemFeatureRepository>()
                .AddScoped<IKey, KeyRepository>()
                .AddScoped<INotification, NotificationRepository>()
                .AddScoped<IOrderItem, OrderItemRepository>()
                .AddScoped<IReview, ReviewRepository>()
                .AddScoped<IShippingDetail, ShippingDetailRepository>()
                .AddScoped<IStore, StoreRepository>()
                .AddScoped<ITracking, TrackingRepository>()
                .AddScoped<IPayment, PaymentRepository>()
                .AddScoped<ITransaction, TransactionRepository>()
                .AddScoped<ILoginMonitor, LoginMonitorRepository>()
                .AddScoped<IGroup, GroupRepository>()
                .AddScoped<IFavourite, FavouriteRepository>()
                .AddScoped<IOffer, OfferRepository>()
                .AddScoped<IBrand, BrandRepository>()
                .AddScoped<ISlide, SlideRepository>()

                .AddScoped<LoggerContext>()
                .AddScoped<IUnitOfLogger, UnitOfLogger>()
                .AddScoped<IError, ErrorRepository>()
                .AddScoped<IAction, ActionRepository>()
                .AddScoped<ITraffic, TrafficRepository>()
                .AddScoped<LoggerBusiness>()

                .AddScoped<GeneralBusiness>()
                .AddScoped<SearchBusiness>()
                .AddScoped<GroupBusiness>()
                .AddScoped<BillingDetailBusiness>()
                .AddScoped<CategoryBusiness>()
                .AddScoped<CustomerBusiness>()
                .AddScoped<FeatureBusiness>()
                .AddScoped<FeatureOptionBusiness>()
                .AddScoped<FileBusiness>()
                .AddScoped<ItemBusiness>()
                .AddScoped<ItemFeatureBusiness>()
                .AddScoped<KeyBusiness>()
                .AddScoped<NotificationBusiness>()
                .AddScoped<OrderBusiness>()
                .AddScoped<OrderItemBusiness>()
                .AddScoped<PaymentBusiness>()
                .AddScoped<ReviewBusiness>()
                .AddScoped<ShippingDetailBusiness>()
                .AddScoped<StoreBusiness>()
                .AddScoped<TrackingBusiness>()
                .AddScoped<TransactionBusiness>()
                .AddScoped<UserBusiness>()
                .AddScoped<OfferBusiness>()
                .AddScoped<SlideBusiness>()
                .AddScoped<BrandBusiness>()
                ;
        }

    }
}
