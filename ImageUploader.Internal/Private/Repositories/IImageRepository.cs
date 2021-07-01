using ImageUploader.Foundation.Private.Repositories.DbModels;
using System;
using System.Collections.Generic;

namespace ImageUploader.Foundation.Private.Repositories
{
    internal interface IImageRepository
    {
        public void CreateImage(DbImage dbImage);
        public void UpdateImage(DbImage dbImage);
        public void DeleteImage(Guid id);
        public DbImage Get(Guid id);
        public IEnumerable<DbImage> GetAll();
        public IEnumerable<DbImage> GetByTags(IEnumerable<string> tags);
        public IEnumerable<DbImage> GetByTitle(string title);
    }
}
