using System.ComponentModel.DataAnnotations;

namespace EmployeeCrudService.Dtos;
public class EmployeeCreateDto {
    
    [Required]
    public string Name { get; set; } = null!; 

    [Required]
    public int Age { get; set; }

    [Required]
    public Char Sex { get; set; }

    [Required]
    public string job { get; set; } = null!;   

    [Required]
    public int Salary { get; set; }
}