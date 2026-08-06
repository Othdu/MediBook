using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace MediBook.Application.Dtos
{
    public class PatientDto
    {

        [Required,StringLength(100,MinimumLength =2)]
        public string Name { get; set; } = string.Empty;


    }

    public class PatientResponseDto
    {
               public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
