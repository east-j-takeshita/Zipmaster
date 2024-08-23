using KadaiMVCApp.Models;

namespace KadaiMVCApp.Interfaces
{
    public interface IZipRepository
    {
        Task<List<Zip>> GetZips(string postCode, string keyWord);
        Task<Zip> GetZipDetail(int id);
        void CreateZip(Zip zipMaster);
        void UpdateZip(int id, Zip zipMaster);
        void DeleteZip(int id);

    }
}
