using AzureFunctionCosmosDB.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureFunctionCosmosDB.Data.Repositories.Students
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetStudents(string query);
        Task<Student> GetStudent(string id);
        Task AddStudent(Student student);
        Task UpdateStudent(string id, Student student);
        Task DeleteStudent(string id);
    }
}
