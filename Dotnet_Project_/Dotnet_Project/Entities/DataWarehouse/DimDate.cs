using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dotnet_Project.Entities.DataWarehouse
{
    [Table("DimDate")]
    public class DimDate
    {
        [Key]
        public int DateKey { get; set; }

        public DateTime FullDate { get; set; }

        public int Year { get; set; }

        public int Quarter { get; set; }

        public int Month { get; set; }

        [StringLength(20)]
        public string MonthName { get; set; }

        public int Day { get; set; }

        public int DayOfWeek { get; set; }

        [StringLength(20)]
        public string DayName { get; set; }

        public bool IsWeekend { get; set; }

        public bool IsHoliday { get; set; }

        // Navigation property
        public virtual ICollection<FactSales> FactSales { get; set; }
    }
}