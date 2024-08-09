using Store.Data.Interface;
using Store.Model.ViewModel;
using Store.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Business
{
    public class TrackingBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        public TrackingBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseMessage<CheckOutVM>> UpdateTracking(Tracking tracking, string orderRef, bool isFInished, Guid userID)
        {
            ResponseMessage<CheckOutVM> responseMessage = new ResponseMessage<CheckOutVM>();
            try
            {
                var order = await _unitOfWork.Orders.GetByOrderRef(orderRef);
                tracking.ID = Guid.NewGuid();
                tracking.OrderID = order.ID;
                tracking.CreatedBy = userID;
                tracking.DateCreated = DateTime.UtcNow.AddHours(1);
                tracking.IsActive = true;

                if (order.IsFinished)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Order Completed!";
                    return responseMessage;
                }

                if (isFInished)
                {
                    order.IsFinished = true;
                    _unitOfWork.Orders.Update(order);
                }

                await _unitOfWork.Trackings.Create(tracking);
                if(await _unitOfWork.Commit() > 0)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Tracking Updated!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Tracking Not Updated";
                }

                responseMessage.Data = new CheckOutVM()
                {
                    Order = order,
                    Trackings = await _unitOfWork.Trackings.GetByOrderID(order.ID)
                };
            }
            catch {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Tracking Not Updated!";
            }
            return responseMessage;
        }

        public async Task<ResponseMessage<CheckOutVM>> Tracking(string orderRef)
        {
            ResponseMessage<CheckOutVM> responseMessage = new ResponseMessage<CheckOutVM>();
            try
            {
                var order = await _unitOfWork.Orders.GetByOrderRef(orderRef);
                responseMessage.Data = new CheckOutVM()
                {
                    Order = order,
                    Trackings = await _unitOfWork.Trackings.GetByOrderID(order.ID),
                    OrderRef = orderRef
                };
            }
            catch {
                responseMessage.Data = new CheckOutVM()
                {
                    OrderRef = orderRef
                };
            }
            return responseMessage;
        }

        public async Task<ResponseMessage<string>> Delete(Guid tID)
        {
            ResponseMessage<string> responseMessage = new ResponseMessage<string>();
            try
            {
                var tracking = await _unitOfWork.Trackings.Find(tID);
                var order = await _unitOfWork.Orders.Find(tracking.OrderID);
                responseMessage.Data = order.Ref;
                _unitOfWork.Trackings.Delete(tracking);

                if (await _unitOfWork.Commit() > 0)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Tracking Deleted!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Tracking Not Deleted";
                }
            }
            catch
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Tracking Not Deleted!";
            }
            return responseMessage;
        }
    }
}
