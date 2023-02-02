namespace todo_list.Models;

public class Task
{
    public string? Description { get; set; }
}

public class ToDoList
{
    private List<Task> Tasks { get; set; }

    public ToDoList()
    {
        Tasks = new List<Task>();
    }

    public void AddTask(Task task)
    {
        Tasks.Add(task);
    }

    public List<Task> GetTasks()
    {
        return Tasks;
    }
}

