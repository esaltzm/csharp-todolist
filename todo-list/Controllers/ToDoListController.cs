using System.Text.Json;
using System.Threading.Tasks;
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
        public IActionResult AddTask([FromBody] Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            toDoList.AddTask(task);
            return CreatedAtAction(nameof(AddTask), task);
        }


        [HttpDelete]
        public IActionResult RemoveTask(Models.Task task)
        {
            return Ok(toDoList.RemoveTask(task));
        }

        //[HttpPut]
        //public IActionResult EditTask(int ID, String N)
        //{
        //    return Ok(toDoList.EditTask(task));
        //}
    }
}