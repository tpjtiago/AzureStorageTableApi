using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using StorageTableApi.Models;

namespace StorageTableApi.Services
{
    public class MeuItemService
    {
        private readonly TableClient _tableClient;

        public MeuItemService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureTableStorage:ConnectionString"];
            var tableName = configuration["AzureTableStorage:TableName"];
            var serviceClient = new TableServiceClient(connectionString);
            _tableClient = serviceClient.GetTableClient(tableName);
            _tableClient.CreateIfNotExists();
        }

        public async Task AdicionarItemAsync(MeuItem item)
        {
            await _tableClient.AddEntityAsync(item);
        }
        public async Task<List<MeuItem>> ObterTodosItensAsync()
        {
            var items = new List<MeuItem>();
            await foreach (var item in _tableClient.QueryAsync<MeuItem>())
            {
                items.Add(item);
            }
            return items;
        }

        public async Task<MeuItem> ObterItemPorRowKeyAsync(string rowKey)
        {
            var result = await _tableClient.GetEntityAsync<MeuItem>("default", rowKey);
            return result.Value;
        }

        public async Task AtualizarItemAsync(MeuItem item)
        {
            await _tableClient.UpdateEntityAsync(item, ETag.All, TableUpdateMode.Replace);
        }

        public async Task DeletarItemAsync(string rowKey)
        {
            await _tableClient.DeleteEntityAsync("default", rowKey);
        }
    }
}
