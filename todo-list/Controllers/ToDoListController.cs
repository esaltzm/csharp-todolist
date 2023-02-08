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
            try
            {
                if (string.IsNullOrWhiteSpace(task.Description))
                {
                    throw new ArgumentException("Description cannot be empty or null.");
                }
                toDoList.AddTask(task);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return CreatedAtAction(nameof(AddTask), task);
        }

        [HttpDelete]
        [Route("/todolist/clear")]
        public IActionResult ClearTasks()
        {
            toDoList.ClearTasks();
            return Ok();
        }

        [HttpDelete]
        [Route("/todolist/delete/{id}")]
        public IActionResult DeleteTask(int ID)
        {
            toDoList.DeleteTask(ID);
            return Ok();
        }

        [HttpPut]
        [Route("/todolist/edit/{id}")]
        public IActionResult EditTask([FromRoute] int id, [FromBody] string NewDescription)
        {
            if (string.IsNullOrEmpty(NewDescription))
            {
                return BadRequest("The NewDescription field is required.");
            }

            try
            {
                toDoList.EditTask(id, NewDescription);
                return Ok();
            }
            catch (NullReferenceException)
            {
                return BadRequest();
            }
        }
    }
}