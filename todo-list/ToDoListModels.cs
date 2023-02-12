using System.ComponentModel.DataAnnotations;
using Microsoft.Data.Sqlite;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace todo_list.Models;

public class ToDoItem
{
    public int id { get; set; }
    public string description { get; set; }
    public bool completed { get; set; }

    public ToDoItem(int _id, string _description, bool _completed)
    {
        id = _id;
        description = _description;
        completed = _completed;
    }
}

public class ToDoList
{
    private readonly string _connectionString = "Data Source=todo.db; Version=3;";

    public ToDoList()
    {
        CreateDatabase();
    }

    private void CreateDatabase()
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS tasks (id INTEGER PRIMARY KEY AUTOINCREMENT, description TEXT NOT NULL, completed BOOLEAN DEFAULT 0)";
                command.ExecuteNonQuery();
            }
        }
    }

    public async Task<List<ToDoItem>> GetAllItems()
    {
        var toDoListItems = new List<ToDoItem>();
        using (var connection = new SQLiteConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "SELECT * FROM tasks";
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var item = new ToDoItem(reader.GetInt32(0), reader.GetString(1), reader.GetBoolean(2));
                            toDoListItems.Add(item);
                        }
                    }
                }
            }
        }
        return toDoListItems;
    }

    public async Task<ToDoItem> GetItemByID(int id)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "SELECT * FROM tasks WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return new ToDoItem(reader.GetInt32(0), reader.GetString(1), reader.GetBoolean(2));
                    }
                    else
                    {
                        throw new Exception("Task ID not found");
                    }
                }
            }
        }
    }

    public async Task<ToDoItem> AddItem(string description)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "INSERT INTO tasks (description) VALUES (@description)";
                command.Parameters.AddWithValue("@description", description);
                await command.ExecuteNonQueryAsync();
                int id = (int)connection.LastInsertRowId;
                return new ToDoItem(id, description, false);
            }
        }
    }

    public async Task<ToDoItem> DeleteItemByID(int id)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "DELETE FROM tasks WHERE id = @id RETURNING *";
                command.Parameters.AddWithValue("@id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return new ToDoItem(reader.GetInt32(0), reader.GetString(1), reader.GetBoolean(2));
                    }
                    else
                    {
                        throw new Exception("Task ID not found");
                    }
                }
            }
        }
    }

    public async Task DeleteAllItems()
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "DELETE FROM tasks";
                await command.ExecuteNonQueryAsync();
                command.CommandText = "DELETE FROM sqlite_sequence WHERE name = 'tasks'";
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<ToDoItem> UpdateItemDescriptionByID (int id, String newDescription)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "UPDATE tasks SET description = @newDescription WHERE id = @id RETURNING *";
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@newDescription", newDescription);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return new ToDoItem(reader.GetInt32(0), reader.GetString(1), reader.GetBoolean(2));
                    }
                    else
                    {
                        throw new Exception("Task ID not found");
                    }
                }
            }
        }
    }

    public async Task<ToDoItem> ToggleItemCompletedByID(int id)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "UPDATE tasks SET completed = NOT completed WHERE id = @id RETURNING *";
                command.Parameters.AddWithValue("@id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return new ToDoItem(reader.GetInt32(0), reader.GetString(1), reader.GetBoolean(2));
                    }
                    else
                    {
                        throw new Exception("Task ID not found");
                    }
                }
            }
        }
    }
}

