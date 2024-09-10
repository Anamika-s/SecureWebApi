using SecureWebApi.Models;

namespace SecureWebApi.IRepository
{
    public interface IStudentRepository
    {
        public List<Student> GetStudents();
        public Student GetById(int id);    
        public Student AddStudent(Student student);
        public bool UpdateStudent(int id, Student student);
        public bool DeleteStudent(int id);

        public string GetRoleName(int roleId);
    }
}
