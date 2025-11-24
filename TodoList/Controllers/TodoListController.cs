using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoList.Data;
using TodoList.Shared.Models;
using TodoList.Shared.Models.Entities;

namespace TodoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public TodoListController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllTodoTasks()
        {
            var allTasks = dbContext.TodoTasks.ToList();

            return Ok(allTasks);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetTodoTaskById(Guid id) 
        {
            var todoTask = dbContext.TodoTasks.Find(id);

            if (todoTask == null)
            {
                return NotFound();
            }

            return Ok(todoTask);
        }

        [HttpPost]
        public IActionResult AddTodoTask(AddTodoTaskDto addTodoTaskDto)
        {
            var TodoEntity = new TodoTask() {
                Title = addTodoTaskDto.Title,
                Description = addTodoTaskDto.Description,
                Deadline = addTodoTaskDto.Deadline,
                Priority = addTodoTaskDto.Priority,
                Completion = addTodoTaskDto.Completion
            };


            dbContext.TodoTasks.Add(TodoEntity);
            dbContext.SaveChanges();

            return Ok(TodoEntity);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateTodoTask(Guid id, UpdateTodoTaskDto updateTodoTaskDto)
        {
            var todoTask = dbContext.TodoTasks.Find(id);

            if (todoTask == null) { return NotFound(); }

            todoTask.Title = updateTodoTaskDto.Title;
            todoTask.Description = updateTodoTaskDto.Description;
            todoTask.Deadline = updateTodoTaskDto.Deadline;
            todoTask.Priority = updateTodoTaskDto.Priority;
            todoTask.Completion = updateTodoTaskDto.Completion;

            dbContext.SaveChanges();

            return Ok(todoTask);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteTodoTask(Guid id)
        {
            var todoTask = dbContext.TodoTasks.Find(id);
            if (todoTask == null) { return NotFound(); }

            dbContext.TodoTasks.Remove(todoTask);
            dbContext.SaveChanges();

            return Ok(todoTask);
        }
    }
}
