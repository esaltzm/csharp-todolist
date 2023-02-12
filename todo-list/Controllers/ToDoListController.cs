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
            try
            {
                List<Models.ToDoItem> itemList = await toDoList.GetAllItems();
                return Ok(itemList);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("/todolist/{id}")]
        public async Task<IActionResult> GetItemByID([FromRoute] int id)
        {
            try
            {
                Models.ToDoItem item = await toDoList.GetItemByID(id);
                return Ok(item);
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

        [HttpPost]
        [Route("/todolist/add")]
        public async Task<IActionResult> AddItem([FromBody] string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                return BadRequest("Description field required");
            }
            try
            {
                Models.ToDoItem insertedItem = await toDoList.AddItem(description);
                int id = insertedItem.id;
                return Created($"/tasks/{id}", insertedItem);
            } catch
            {
                return StatusCode(500);
            }
        }


        [HttpDelete]
        [Route("/todolist/delete/all")]
        public async Task<IActionResult> DeleteAllItems()
        {
            try
            {
                await toDoList.DeleteAllItems();
                return Ok();
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("/todolist/delete/{id}")]
        public async Task<IActionResult> DeleteItemByID([FromRoute] int id)
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

        [HttpPut]
        [Route("/todolist/edit/description/{id}")]
        public async Task<IActionResult> UpdateItemDescriptionByID ([FromRoute] int id, [FromBody] string newDescription)
        {
            if (string.IsNullOrEmpty(newDescription))
            {
                return BadRequest("Description field required");
            }
            try
            {
                Models.ToDoItem updatedItem = await toDoList.UpdateItemDescriptionByID(id, newDescription);
                return Ok(updatedItem);
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

        [HttpPut]
        [Route("/todolist/edit/completed/{id}")]
        public async Task<IActionResult> ToggleItemCompletedByID([FromRoute] int id)
        {
            try
            {
                Models.ToDoItem updatedItem = await toDoList.ToggleItemCompletedByID(id);
                return Ok(updatedItem);
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
    }
}