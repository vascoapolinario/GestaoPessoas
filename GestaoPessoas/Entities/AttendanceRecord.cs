using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GestaoPessoas.Enums;

namespace GestaoPessoas.Entities
{
    [Table("AttendanceRecords")]
    public class AttendanceRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int WorkerId { get; set; }

        [Required]
        [ForeignKey(nameof(WorkerId))]
        public required Worker Worker { get; set; }


        [Required]
        public AttendanceEventType EventType { get; set; } = AttendanceEventType.Undefined;

        [Required]
        public DateTimeOffset EventDate { get; set; }
    }
}
