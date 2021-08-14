using System;
using Google.Cloud.Firestore;

namespace GPhotoStats.BL.Models
{
    [FirestoreData]
    public class PhotoData
    {
        [FirestoreProperty] public string Id { get; set; }

        [FirestoreProperty] public string Url { get; set; }

        [FirestoreProperty] public DateTime? CreationTime { get; set; }

        [FirestoreProperty] public string Maker { get; set; }

        [FirestoreProperty] public string Model { get; set; }

        [FirestoreProperty] public string MakerAndModel => Model.Contains(Maker) ? Model : $"{Maker} {Model}";

        [FirestoreProperty] public string MimeType { get; set; }
    }
}