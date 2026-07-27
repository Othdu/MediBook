using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediBook.Domain.Entities
{
    public class Specialty
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Doctor> Doctors { get; set; } = new();
    }
}
