using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using GPhotoStats.BL.Models;

namespace GPhotoStats.BL.Interfaces
{
    public interface IPhotoDataRepository
    {
        DocumentReference Insert(PhotoData data);
        Task<List<PhotoData>> FindAll();
    }
}