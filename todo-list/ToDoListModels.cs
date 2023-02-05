using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace todo_list.Models;

public class Task
{
    [Required]
    public string Description { get; set; } = "";
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
        } catch(Exception e)
        {
            Console.WriteLine(e);
        }
        if(tasks == null)
        {
            tasks = new List<Task>();
        }
    }

    private void SaveTaskList()
    {
        Console.WriteLine($"New task list: {JsonSerializer.Serialize<List<Models.Task>>(tasks)}");
        File.WriteAllText(JSONFilePath, JsonSerializer.Serialize<List<Task>>(tasks));
    }

    public void AddTask(Task task)
    {
        tasks?.Add(task);
        SaveTaskList();
    }

    public String RemoveTask(Task task)
    {
        tasks?.Remove(task);
        SaveTaskList();
        return $"You removed '{task.Description} from the list";
    }

    //public String EditTask(int ID, String NewDescription)
    //{
    //    var TaskToEdit = Tasks.Find(task => task.ID == ID);
    //    if (TaskToEdit != null)
    //    {
    //        var OldDescription = TaskToEdit.Description;
    //        TaskToEdit.Description = NewDescription;
    //        return $"You changed {OldDescription} to {NewDescription}";
    //    } else
    //    {
    //        return $"Task {ID} not found";
    //    }
    //}

    public List<Task> GetTasks()
    {
        return tasks;
    }
}

