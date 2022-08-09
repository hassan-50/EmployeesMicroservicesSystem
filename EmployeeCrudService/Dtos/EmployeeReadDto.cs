using System.ComponentModel.DataAnnotations;

namespace EmployeeCrudService.Dtos;
public class EmployeeReadDto {
    
    public int Id { get; set; }
    public string Name { get; set; } = null!;     
    public int Age { get; set; }    
    public Char Sex { get; set; }    
    public string job { get; set; } = null!;       
    public int Salary { get; set; }
}