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
        public async Task<IActionResult> GetAllItems()
        {
            List <Models.ToDoItem> taskList = await toDoList.GetAllItems();
            return Ok(taskList);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                return BadRequest("Description field required");
            }

            Models.ToDoItem insertedItem = await toDoList.AddItem(description);
            int id = insertedItem.id;

            return Created($"/tasks/{id}", new { id = id, description = description });
        }


        [HttpDelete]
        [Route("/todolist/delete/all")]
        public async Task<IActionResult> DeleteAllItems()
        {
            await toDoList.DeleteAllItems();
            return Ok();
        }

        [HttpDelete]
        [Route("/todolist/delete/{id}")]
        public async Task<IActionResult> DeleteItemByID(int id)
        {
            try
            {
                Models.ToDoItem deletedItem = await toDoList.DeleteItemByID(id);
                return NoContent();
            }
            catch (Exception ex) when (ex.Message == "Task ID not found")
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }


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