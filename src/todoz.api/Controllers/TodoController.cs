using Microsoft.AspNetCore.Mvc;

using todoz.api.Models;

namespace todoz.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _repository;


        public TodoController(ITodoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<List<Todo>> GetAll()
        {
            var todos = _repository.GetAll();
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public ActionResult<Todo> Get(int id)
        {
            var todo = _repository.Get(id);

            if (todo == null)
                return NotFound();

            return Ok(todo);
        }

        [HttpPost]
        public ActionResult Create(Todo todo)
        {
            _repository.Add(todo);
            _repository.Save();

            return CreatedAtAction(nameof(Get), new { id = todo.Id }, todo);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, Todo todo)
        {
            if (id != todo.Id)
                return BadRequest();

            var existingTodo = _repository.Get(id);
            if (existingTodo is null)
                return NotFound();

            _repository.Update(todo);
            _repository.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var todo = _repository.Get(id);

            if (todo is null)
                return NotFound();

            _repository.Delete(id);
            _repository.Save();

            return NoContent();
        }
    }
}
