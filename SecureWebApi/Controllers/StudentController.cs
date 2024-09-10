using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecureWebApi.IRepository;
using SecureWebApi.Models;

namespace SecureWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

   
    public class StudentController : ControllerBase
    {
        IStudentRepository _repo;
        public StudentController(IStudentRepository repo)
        {
            _repo = repo;

        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult Index()
        {
            return Ok(_repo.GetStudents());
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Details(int id)
        {
            return Ok(_repo.GetById(id));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles="Admin")]
        public IActionResult Delete(int id)
        {
            return Ok(_repo.DeleteStudent(id));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, Student student)
        {
            return Ok(_repo.UpdateStudent(id, student));
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Post(Student student)
        {
            _repo.AddStudent(student);
            return Created("added", student);
        }

    }
}