using ImageUploader.Foundation.Private.Repositories.DbModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ImageUploader.Foundation.Private.Repositories
{
    internal class MockImageRepository : IImageRepository
    {
        private readonly ConcurrentDictionary<Guid, DbImage> mockStore;
        public MockImageRepository()
        {
            mockStore = new ConcurrentDictionary<Guid, DbImage>();
        }

        public void CreateImage(DbImage dbImage)
        {
            mockStore.AddOrUpdate(dbImage.Id, dbImage, (k, v) => v);
        }

        public void DeleteImage(Guid id)
        {
            mockStore.Remove(id, out _);
        }

        public DbImage Get(Guid id)
        {
            if(mockStore.TryGetValue(id, out var img))
            {
                return img;
            }
            return null;
        }

        public IEnumerable<DbImage> GetAll()
        {
            return mockStore.Values;
        }

        public IEnumerable<DbImage> GetByTags(IEnumerable<string> tags)
        {
            return mockStore.Values.Where(x => x.Tags.Intersect(tags, StringComparer.InvariantCultureIgnoreCase).Any());
        }

        public IEnumerable<DbImage> GetByTitle(string title)
        {
            return mockStore.Values.Where(x => x.Title.Equals(title, StringComparison.InvariantCultureIgnoreCase));
        }

        public void UpdateImage(DbImage dbImage)
        {
            mockStore.AddOrUpdate(dbImage.Id, dbImage, (k, v) => v);
        }
    }
}
