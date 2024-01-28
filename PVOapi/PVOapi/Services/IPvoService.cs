using PVOapi.Models;

namespace PVOapi.Services
{
    public interface IPvoService
    {
        Task<PvoModel> UploadFile(IFormFile file);
        


    }
}