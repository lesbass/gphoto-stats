using GPhotoStats.BL.Interfaces;

namespace GPhotoStats.BL.Models
{
    public class GlobalSettings : IGlobalSettings
    {
        public string GPhotosClientId { get; set; }
        public string GPhotosClientSecret { get; set; }
        public string FireStoreProjectId { get; set; }
        public string FireStoreCollection { get; set; }
        public FireStoreJsonCredentialsData FireStoreJsonCredentials { get; set; }
    }
}