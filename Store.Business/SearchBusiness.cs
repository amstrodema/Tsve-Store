using App.Services;
using Store.Data.Interface;
using Store.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Store.Business
{
    public class SearchBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GoogleCSEService _googleCSEService;
        public SearchBusiness(IUnitOfWork unitOfWork, GoogleCSEService googleCSEService)
        {
            _unitOfWork = unitOfWork;
            _googleCSEService = googleCSEService;
        }
        public async Task<ResponseMessage<string>> AddQuestion(string question, string response, Guid userID)
        {
            ResponseMessage<string> responseMessage = new ResponseMessage<string>();
            try
            {
               
                Knowledge knowledge = new Knowledge()
                {
                    ID = Guid.NewGuid(),
                    CreatedBy = userID,
                    DateCreated = DateTime.UtcNow.AddHours(1),
                    IsActive = true,
                    Question = question,
                    Response = response,
                    Tag = GenericService.GetTag(question)
                };

                if(await _unitOfWork.Knowledge.GetByTag(knowledge.Tag) != null)
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Knowledge exists already!";
                    return responseMessage;
                }

                await _unitOfWork.Knowledge.Create(knowledge);

                if (await _unitOfWork.Commit() > 0)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Knowledge Saved!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Knowledge Not Saved";
                }

            }
            catch (Exception)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Knowledge Not Saved!";
            }

            return responseMessage;
        }

        public async Task<Knowledge> GetQuestion(string tag)
        {
            return await _unitOfWork.Knowledge.GetByTag(tag);
        }
        public async Task<IEnumerable<Knowledge>> GetQuestions(int? pageNumber)
        {
            return (PaginatedList<Knowledge>.Create((await _unitOfWork.Knowledge.GetAll()).ToList(), pageNumber ?? 1, 20)).Items; 
        }

        public async Task<ResponseMessage<IEnumerable<GoogleCSEItem>>> SearchQuestion(string searchQuery, int? pageNumber)
        {
            ResponseMessage<IEnumerable<GoogleCSEItem>> responseMessage = new ResponseMessage<IEnumerable<GoogleCSEItem>>();
            try
            {
                SearchKeyword keyword = new SearchKeyword()
                {
                    ID = Guid.NewGuid(),
                    Keyword = searchQuery,
                    DateCreated = DateTime.UtcNow.AddHours(1)
                };

                var result = await _googleCSEService.SearchAsync(searchQuery);
                var val = PaginatedList<GoogleCSEItem>.Create(result.ToList(), pageNumber ?? 1, 20);
                responseMessage.Data = val.Items;

                await _unitOfWork.SearchKeywords.Create(keyword);

                if (await _unitOfWork.Commit() > 0)
                {
                    responseMessage.StatusCode = 200;
                    responseMessage.Message = "Search Completed!";
                }
                else
                {
                    responseMessage.StatusCode = 201;
                    responseMessage.Message = "Search Not Completed";
                    responseMessage.Data = new List<GoogleCSEItem>();
                }

            }
            catch (Exception)
            {
                responseMessage.StatusCode = 201;
                responseMessage.Message = "Search Not Completed!";
                responseMessage.Data = new  List<GoogleCSEItem>();
            }
            return responseMessage;
        }

    }
}
