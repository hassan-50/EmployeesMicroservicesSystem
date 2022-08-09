using System.Text;
using System.Text.Json;
using EmployeeService.Dtos;

namespace EmployeeService.SyncDataServices;
public class HttpEmployeeDataClient : IHttpEmployeeDataClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _cfg;

    public HttpEmployeeDataClient(HttpClient httpClient, IConfiguration cfg)
    {
        _httpClient = httpClient;
        _cfg = cfg;
    }
    public async Task SendCreateEmployee(EmployeeCreateDto emp)
    {
        var httpContent = new StringContent (
            JsonSerializer.Serialize(emp),Encoding.UTF8,"application/json"
        );
        var response = await _httpClient.PostAsync(_cfg["EmployeeCrudService"],httpContent);
        if(response.IsSuccessStatusCode){
                Console.WriteLine("--> Sync POST To EmployeeCrud Was Ok");
            }
            else{
                Console.WriteLine("--> Sync POST To EmployeeCrud Was Not Ok");
            }
    }

    public async Task SendDeleteEmployee(int id)
    {        
        var response = await _httpClient.DeleteAsync($"{_cfg["EmployeeCrudService"]}/{id}");
        if(response.IsSuccessStatusCode){
                Console.WriteLine("--> Sync Delete To EmployeeCrud Was Ok");
            }
            else{
                Console.WriteLine("--> Sync Delete To EmployeeCrud Was Not Ok");
            }
    }

    public async Task SendUpdateEmployee(int id, EmployeeUpdateDto employee)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(employee), Encoding.UTF8, "application/json"
        );

        var response = await _httpClient.PutAsync($"{_cfg["EmployeeCrudService"]}/{id}",httpContent);

        if(response.IsSuccessStatusCode){
                Console.WriteLine("--> Sync Update To EmployeeCrud Was Ok");
            }
            else{
                Console.WriteLine("--> Sync Update To EmployeeCrud Was Not Ok");
            }
    }
}