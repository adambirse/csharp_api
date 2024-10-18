using System.Text.Json;

namespace services.todo;

public sealed class TodoService(
    IHttpClientFactory httpClientFactory, ILogger<TodoService> logger) : ITodoService
{
    public async Task<Todo[]> GetUserTodosAsync()
    {
        // Create the client
        using HttpClient client = httpClientFactory.CreateClient();

        try
        {
            // Make HTTP GET request
            // Parse JSON response deserialize into Todo types
            Todo[]? todos = await client.GetFromJsonAsync<Todo[]>(
                $"https://jsonplaceholder.typicode.com/todos",
                new JsonSerializerOptions(JsonSerializerDefaults.Web));
            logger.LogInformation("Successfully retrieved todos from the network");
            return todos ?? [];
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve todos from the network");
            throw;
        }
    }
}