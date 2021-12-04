using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureFunctionCosmosDB.Data.Models;
using Microsoft.Azure.Cosmos;

namespace AzureFunctionCosmosDB.Data.Repositories.Students
{
    public class StudentRepository : IStudentRepository
    {
        private readonly Container _container;

        public StudentRepository(CosmosClient dbClient, string databaseName, string containerName)
        {
            _container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task<IEnumerable<Student>> GetStudents(string queryString)
        {
            var query = _container.GetItemQueryIterator<Student>(new QueryDefinition(queryString));
            var results = new List<Student>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            
            return results;
        }

        public async Task<Student> GetStudent(string id)
        {
            try
            {
                ItemResponse<Student> response = await _container.ReadItemAsync<Student>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when(ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task AddStudent(Student student)
        {
            await _container.CreateItemAsync<Student>(student, new PartitionKey(student.Id));
        }

        public async Task DeleteStudent(string id)
        {
            await _container.DeleteItemAsync<Student>(id, new PartitionKey(id));
        }

        public async Task UpdateStudent(string id, Student student)
        {
            await _container.UpsertItemAsync<Student>(student, new PartitionKey(id));
        }
    }
}