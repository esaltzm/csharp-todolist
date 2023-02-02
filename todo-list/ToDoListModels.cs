using System.Text.Json;

namespace todo_list.Models;

public class Task
{
    public string Description { get; set; } = "";
    public readonly int ID;
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
        File.WriteAllText(JSONFilePath, JsonSerializer.Serialize<List<Task>>(tasks));
    }

    public String AddTask(Task task)
    {
        Console.WriteLine("1 :", JsonSerializer.Serialize<List<Task>>(tasks));
        tasks?.Add(task);
        SaveTaskList();
        Console.WriteLine("2 :", JsonSerializer.Serialize<List<Task>>(tasks));
        return $"You added '{task.Description}' to the list";
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

