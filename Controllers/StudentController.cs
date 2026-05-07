using Microsoft.AspNetCore.Mvc;

namespace StudentGradeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private static List<Student> _students = new List<Student>
        {
            new Student { Id = 1, Name = "Alice Johnson", Course = "Computer Science", Grade = 95.5 },
            new Student { Id = 2, Name = "Bob Smith", Course = "Mathematics", Grade = 87.0 },
            new Student { Id = 3, Name = "Carol White", Course = "Physics", Grade = 91.3 }
        };

        [HttpGet]
        public IActionResult GetAll() => Ok(_students);

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null) return NotFound(new { message = $"Student {id} not found" });
            return Ok(student);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Student student)
        {
            student.Id = _students.Count + 1;
            _students.Add(student);
            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Student updated)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null) return NotFound(new { message = $"Student {id} not found" });
            student.Grade = updated.Grade;
            student.Course = updated.Course;
            return Ok(student);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null) return NotFound(new { message = $"Student {id} not found" });
            _students.Remove(student);
            return Ok(new { message = $"{student.Name} deleted!" });
        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public double Grade { get; set; }
    }
}