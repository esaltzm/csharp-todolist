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
    //private String JSONFilePath = "/tmp/list.json";
    //private List<Task>? tasks = null;
    private readonly string _connectionString = "Data Source=todo.db; Version=3;";

    //public ToDoList()
    //{
    //    try
    //    {
    //        using (StreamReader r = new StreamReader(JSONFilePath))
    //        {
    //            string json = r.ReadToEnd();
    //            tasks = JsonSerializer.Deserialize<List<Task>>(json);
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Console.WriteLine(e);
    //    }
    //}

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

    public List<ToDoItem> GetTasks()
    {
        var toDoListItems = new List<ToDoItem>();

        var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        var command = new SQLiteCommand(connection);
        command.CommandText = "SELECT * FROM Tasks";
        var reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                var item = new ToDoItem(reader.GetInt32(0), reader.GetString(1));
                toDoListItems.Add(item);
            }
        }
        return toDoListItems;
    }

    //private void SaveTaskList()
    //{
    //    if (tasks == null)
    //    {
    //        tasks = new List<Task>();
    //    }
    //    File.WriteAllText(JSONFilePath, JsonSerializer.Serialize<List<Task>>(tasks));
    //}

    //public void AddTask(Task task)
    //{
    //    try
    //    {
    //        if (tasks == null)
    //        {
    //            tasks = new List<Task>();
    //        }
    //        task.ID = tasks.Count + 1;
    //        tasks.Add(task);
    //        SaveTaskList();
    //    } catch(ArgumentException e)
    //    {
    //        throw e;
    //    }
    //}

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

    //public void DeleteTask(int ID)
    //{
    //    if (tasks != null)
    //    {
    //        var taskToDelete = tasks.Find(task => task.ID == ID);

    //        if (taskToDelete == null)
    //        {
    //            throw new KeyNotFoundException("Task ID not found");
    //        }

    //        tasks.Remove(taskToDelete);
    //        SaveTaskList();
    //    }
    //}


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

