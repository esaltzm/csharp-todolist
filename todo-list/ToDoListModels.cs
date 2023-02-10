using System.ComponentModel.DataAnnotations;
using Microsoft.Data.Sqlite;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;


// old path /usr/local/share/dotnet/dotnet


namespace todo_list.Models;

public class ToDoItem
{
    public int id { get; set; }
    public string description { get; set; }

    public ToDoItem(int _id, string _description)
    {
        id = _id;
        description = _description;
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
                command.CommandText = "CREATE TABLE IF NOT EXISTS Tasks (ID INTEGER PRIMARY KEY AUTOINCREMENT, Description TEXT NOT NULL)";
                command.ExecuteNonQuery();
            }
        }
    }

    public async Task<List<ToDoItem>> GetTasks()
    {
        var toDoListItems = new List<ToDoItem>();
        using (var connection = new SQLiteConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "SELECT * FROM Tasks";
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var item = new ToDoItem(reader.GetInt32(0), reader.GetString(1));
                            toDoListItems.Add(item);
                        }
                    }
                }
            }
        }

        return toDoListItems;
    }

    public async Task<ToDoItem> AddItem(string description)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "INSERT INTO Tasks (Description) VALUES (@Description)";
                command.Parameters.AddWithValue("@Description", description);
                await command.ExecuteNonQueryAsync();
                int id = (int)connection.LastInsertRowId;
                return new ToDoItem(id, description);
            }
        }
    }

    public async Task<ToDoItem> DeleteItem(int id)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "DELETE FROM Tasks WHERE id = @id RETURNING *";
                command.Parameters.AddWithValue("@id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return new ToDoItem(reader.GetInt32(0), reader.GetString(1));
                    }
                    else
                    {
                        throw new Exception("Task ID not found");
                    }
                }
            }
        }
    }



    //public void ClearTasks()
    //{
    //    tasks = new List<Task>();
    //    SaveTaskList();
    //}

    //public void EditTask(int ID, String NewDescription)
    //{
    //    if (tasks != null)
    //    {
    //        var taskToEdit = tasks.Find(task => task.ID == ID);
    //        if (taskToEdit == null)
    //        {
    //            throw new KeyNotFoundException("Task ID not found");
    //        }
    //        taskToEdit.Description = NewDescription;
    //        SaveTaskList();
    //    }
    //}

    //public List<Task> GetTasks()
    //{
    //    if (tasks == null)
    //    {
    //        tasks = new List<Task>();
    //    }
    //    return tasks;
    //}
}

