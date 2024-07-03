using Azure;
using Azure.Data.Tables;

namespace StorageTableApi.Models
{
    public class MeuItem : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
