using Microsoft.AspNetCore.Mvc;
using StorageTableApi.Models;
using StorageTableApi.Services;

namespace StorageTableApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeuItemController : ControllerBase
    {
        private readonly MeuItemService _meuItemService;

        public MeuItemController(MeuItemService meuItemService)
        {
            _meuItemService = meuItemService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(MeuItem item)
        {
            item.PartitionKey = "default";
            item.RowKey = Guid.NewGuid().ToString();
            await _meuItemService.AdicionarItemAsync(item);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<MeuItem>>> Get()
        {
            var items = await _meuItemService.ObterTodosItensAsync();
            return Ok(items);
        }

        [HttpGet("{rowKey}")]
        public async Task<ActionResult<MeuItem>> Get(string rowKey)
        {
            var item = await _meuItemService.ObterItemPorRowKeyAsync(rowKey);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPut("{rowKey}")]
        public async Task<IActionResult> Put(string rowKey, MeuItem updatedItem)
        {
            var existingItem = await _meuItemService.ObterItemPorRowKeyAsync(rowKey);
            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Nome = updatedItem.Nome;
            existingItem.Idade = updatedItem.Idade;

            await _meuItemService.AtualizarItemAsync(existingItem);
            return Ok();
        }

        [HttpDelete("{rowKey}")]
        public async Task<IActionResult> Delete(string rowKey)
        {
            var existingItem = await _meuItemService.ObterItemPorRowKeyAsync(rowKey);
            if (existingItem == null)
            {
                return NotFound();
            }

            await _meuItemService.DeletarItemAsync(rowKey);
            return Ok();
        }
    }
}
