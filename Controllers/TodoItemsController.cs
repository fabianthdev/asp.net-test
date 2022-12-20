using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Services;

namespace asp.net_test.Controllers
{
    [Route("api/todos")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodosService _todosService;

        public TodoItemsController(TodosService service)
        {
            _todosService = service;
        }

        // GET: api/todos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            // var todos = await _todosService.GetTodosAsync();
            return await ItemDTOsAsync(_todosService.GetTodosAsync());
        }

        // GET: api/todos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _todosService.GetTodoAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(todoItem);
        }

        // PUT: api/todos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            // Find corresponding TodoItem
            var todoItem = await _todosService.GetTodoAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            // Update the TodoItem with the values from the DTO
            todoItem.Name = todoItemDTO.Name;
            todoItem.IsComplete = todoItemDTO.IsComplete;

            await _todosService.UpdateAsync(id, todoItem);

            return NoContent();
        }

        // POST: api/todos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItemDTO todoItemDTO)
        {
            // Create a new TodoItem from the values provided in the DTO
            var todoItem = new TodoItem
            {
                Name = todoItemDTO.Name,
                IsComplete = todoItemDTO.IsComplete,
            };

            await _todosService.CreateAsync(todoItem);

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, ItemToDTO(todoItem));
        }

        // DELETE: api/todos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _todosService.GetTodoAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            await _todosService.RemoveAsync(id);

            return NoContent();
        }

        /// <summary>Convert <c>TodoItem</c> to Data Transfer Object</summary>
        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
            new TodoItemDTO
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete,
            };

        private async static Task<TodoItemDTO> ItemToDTOAsync(Task<TodoItem> todoItem) =>
            ItemToDTO(await todoItem);

        private async static Task<List<TodoItemDTO>> ItemDTOsAsync(Task<List<TodoItem>> todoItems) =>
            (await todoItems).Select(x => ItemToDTO(x)) as List<TodoItemDTO>;
            
    }
}
