using System;
using System.Collections.Generic;

namespace ImageUploader.Foundation.Private.Repositories.DbModels
{
    internal class DbImage
    {
        internal Guid Id { get; set; }
        internal string Title { get; set; }
        internal string Description { get; set; }
        internal IEnumerable<string> Tags { get; set; }
        internal byte[] ImageData { get; set; }
        internal string FileName { get; set; }
        internal string ContentType { get; set; }
    }
}
