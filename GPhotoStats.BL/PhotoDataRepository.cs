using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using GPhotoStats.BL.Interfaces;
using GPhotoStats.BL.Models;
using Newtonsoft.Json;

namespace GPhotoStats.BL
{
    public class PhotoDataRepository : IPhotoDataRepository
    {
        private readonly CollectionReference _collection;

        public PhotoDataRepository(IGlobalSettings globalSettings)
        {
            var builder = new FirestoreClientBuilder
            {
                JsonCredentials = JsonConvert.SerializeObject(globalSettings.FireStoreJsonCredentials)
            };

            var db = FirestoreDb.Create(globalSettings.FireStoreProjectId, builder.Build());
            _collection = db.Collection(globalSettings.FireStoreCollection);
        }

        public DocumentReference Insert(PhotoData data)
        {
            var task = _collection.AddAsync(data);
            task.Wait();
            return task.Result;
        }

        public async Task<List<PhotoData>> FindAll()
        {
            var snapshot = await _collection.GetSnapshotAsync();
            return snapshot.Documents
                .Select(doc => doc.ConvertTo<PhotoData>()).ToList();
        }
    }
}