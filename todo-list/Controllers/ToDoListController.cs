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
        public async Task<IActionResult> GetTasks()
        {
            List <Models.ToDoItem> taskList = await toDoList.GetTasks();
            return Ok(taskList);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                return BadRequest("The Description field is required");
            }

            Models.ToDoItem insertedTask = await toDoList.AddItem(description);
            int id = insertedTask.id;

            return Created($"/tasks/{id}", new { id = id, description = description });
        }


        //[HttpDelete]
        //[Route("/todolist/clear")]
        //public IActionResult ClearTasks()
        //{
        //    toDoList.ClearTasks();
        //    return Ok();
        //}

        //[HttpDelete]
        //[Route("/todolist/delete/{id}")]
        //public IActionResult DeleteTask(int ID)
        //{
        //    try
        //    {
        //        toDoList.DeleteTask(ID);
        //        return Ok();
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        return NotFound("Task ID not found");
        //    }
        //}

        //[HttpPut]
        //[Route("/todolist/edit/{id}")]
        //public IActionResult EditTask([FromRoute] int id, [FromBody] string NewDescription)
        //{
        //    if (string.IsNullOrEmpty(NewDescription))
        //    {
        //        return BadRequest("The NewDescription field is required.");
        //    }

        //    try
        //    {
        //        toDoList.EditTask(id, NewDescription);
        //        return Ok();
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        return NotFound("Task ID not found");
        //    }
        //}
    }
}