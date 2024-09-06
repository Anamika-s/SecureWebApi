using SecureWebApi.Context;
using SecureWebApi.IRepository;
using SecureWebApi.Models;

namespace WebApiUsingRepoPattern.Repository
{
    public class StudentRepository : IStudentRepository
    {
        // DI 
        AppDbContext _context;
        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }
        public Student AddStudent(Student Student)
        {
           _context.Students.Add(Student);
            _context.SaveChanges();
            return Student;
        }

        public bool DeleteStudent(int id)
        {
            foreach (var student in _context.Students)
            {
                if (student.Id == id)
                {
                    _context.Students.Remove(student);
                    break;
                }
            }
            _context.SaveChanges();
            return true;
        }

        public Student GetById(int id)
        {
            Student temp=null;
            foreach (var Student in _context.Students)
            {
                if (Student.Id == id)
                {
                   temp = Student;
                    break;
                }
            }
            return temp;
        }

        public List<Student> GetStudents()
        {
           return   _context.Students.ToList();
        }

        public bool UpdateStudent(int id, Student Student)
        {
            foreach (var obj in _context.Students)
            {
                if (obj.Id == id)
                {
                    obj.Name = Student.Name;
                    break;
                }
            }
            _context.SaveChanges();
            return true;
        }
    }
}
