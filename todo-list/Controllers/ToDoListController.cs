using Microsoft.AspNetCore.Mvc;
using todo_list.Models;

namespace todo_list.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoListController : ControllerBase
    {
        private readonly ToDoList toDoList;

        public ToDoListController()
        {
            toDoList = new ToDoList();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok(toDoList.GetTasks());
        }

        [HttpPost]
        public IActionResult AddTask(Models.Task task)
        {
            toDoList.AddTask(task);
            return Ok();
        }
    }
}