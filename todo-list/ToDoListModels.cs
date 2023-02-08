using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace todo_list.Models;

public class Task
{
    public int ID { get; set; }
    public string Description { get; set; }

    public Task(int id, string description)
    {
        ID = id;
        Description = description;
    }
}

public class ToDoList
{
    private String JSONFilePath = "/tmp/list.json";
    private List<Task>? tasks = null;

    public ToDoList()
    {
        try
        {
            using (StreamReader r = new StreamReader(JSONFilePath))
            {
                string json = r.ReadToEnd();
                tasks = JsonSerializer.Deserialize<List<Task>>(json);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }


    private void SaveTaskList()
    {
        if (tasks == null)
        {
            tasks = new List<Task>();
        }
        File.WriteAllText(JSONFilePath, JsonSerializer.Serialize<List<Task>>(tasks));
    }

    public void AddTask(Task task)
    {
        try
        {
            if (tasks == null)
            {
                tasks = new List<Task>();
            }
            task.ID = tasks.Count + 1;
            tasks.Add(task);
            SaveTaskList();
        } catch(ArgumentException e)
        {
            throw e;
        }
    }

    public void DeleteTask(int ID)
    {
        if (tasks != null)
        {
            tasks.RemoveAll(task => task.ID == ID);
            SaveTaskList();
        }
    }

    public void ClearTasks()
    {
        tasks = new List<Task>();
        SaveTaskList();
    }

    public void EditTask(int ID, String NewDescription)
    {
        try
        {
            if (tasks != null)
            {
                var taskToEdit = tasks.Find(task => task.ID == ID);
                taskToEdit.Description = NewDescription;
                SaveTaskList();
            }
        } catch(NullReferenceException e)
        {
            throw e;
        }
    }

    public List<Task> GetTasks()
    {
        if (tasks == null)
        {
            tasks = new List<Task>();
        }
        return tasks;
    }
}

