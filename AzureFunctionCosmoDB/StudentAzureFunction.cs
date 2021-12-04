using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureFunctionCosmosDB.Data.Repositories.Students;
using AzureFunctionCosmosDB.Data.Models;

namespace AzureFunctionCosmoDB
{
    public class StudentAzureFunction
    {
        private readonly IStudentRepository _studentRepository;
        public StudentAzureFunction(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [FunctionName("StudentAzureFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest request,
            ILogger log)
        {
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            var student = JsonConvert.DeserializeObject<Student>(requestBody);
            await _studentRepository.AddStudent(student);
            var students = await _studentRepository.GetStudents("SELECT * FROM c");

            return new OkObjectResult(students);
        }
    }
}
