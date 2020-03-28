using DLL.ApplicationDbContext;
using DLL.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DLL.Repository
{
    public interface IStudentRepository
    {
        public Student AddStudent(Student student);
        public List<Student> GetAllStudent();
        public Student GetStudentByEmail(string email);
    }

    public class StudentRepository: IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public Student AddStudent(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
            return student;
        }

        public List<Student> GetAllStudent()
        {
           return _context.Students.ToList();
        }

        public Student GetStudentByEmail(string email)
        {
            return _context.Students.FirstOrDefault(x=>x.Email==email);
        }
    }
}
