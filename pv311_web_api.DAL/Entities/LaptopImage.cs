using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pv311_web_api.DAL.Entities
{
    public class LaptopImage : BaseEntity<string>
    {
        public override string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }
        [Required]
        [MaxLength(255)]
        public required string Path { get; set; }

        [ForeignKey("Laptop")]
        public string? LaptopId { get; set; }
        public Laptop? Laptop { get; set; }
    }
}
