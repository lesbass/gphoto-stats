namespace GPhotoStats.BL.Models
{
    public class SyncResult
    {
        public SyncResult(int addedItems, int skippedItems, int errors)
        {
            AddedItems = addedItems;
            SkippedItems = skippedItems;
            Errors = errors;
        }

        public int AddedItems { get; set; }
        public int SkippedItems { get; set; }
        public int Errors { get; set; }
    }
}