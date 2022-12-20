using TodoApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace TodoApi.Services;

public class TodosService
{
    private readonly IMongoCollection<TodoItem> _todosCollection;

    public TodosService(IOptions<TodoDatabaseSettings> databaseSettings)
    {
        // Set up the database connection
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

        _todosCollection = mongoDatabase.GetCollection<TodoItem>(databaseSettings.Value.TodosCollectionName);
    }

    public async Task<List<TodoItem>> GetTodosAsync() => await _todosCollection
        .Find(_ => true)
        .ToListAsync();

    public async Task<TodoItem?> GetTodoAsync(long id) => await _todosCollection
        .Find(x => x.Id == id)
        .FirstOrDefaultAsync();

    public async Task CreateAsync(TodoItem todoItem) => await _todosCollection
        .InsertOneAsync(todoItem);

    public async Task UpdateAsync(long id, TodoItem todoItem) => await _todosCollection
        .ReplaceOneAsync(x => x.Id == id, todoItem);

    public async Task RemoveAsync(long id) => await _todosCollection
        .DeleteOneAsync(x => x.Id == id);
}