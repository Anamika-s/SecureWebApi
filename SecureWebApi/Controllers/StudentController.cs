using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecureWebApi.IRepository;
using SecureWebApi.Models;

namespace SecureWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize]
    public class StudentController : ControllerBase
    {
        IStudentRepository _repo;
        public StudentController(IStudentRepository repo)
        {
            _repo = repo;

        }
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(_repo.GetStudents());
        }

        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            return Ok(_repo.GetById(id));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(_repo.DeleteStudent(id));
        }

        [HttpPut("{id}")]
        public IActionResult Edit(int id, Student student)
        {
            return Ok(_repo.UpdateStudent(id, student));
        }


        [HttpPost]
        public IActionResult Post(Student student)
        {
            _repo.AddStudent(student);
            return Created("added", student);
        }

    }
}