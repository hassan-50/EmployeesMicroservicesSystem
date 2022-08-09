using System.ComponentModel.DataAnnotations;

namespace EmployeeService.Dtos;
public class EmployeeUpdateDto {    
    public int Id { get; set; }
    
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