using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.General
{
    //Base class for entities common properties
    public class Base<T>
    {
        [Key]
        public T? Id { get; set; }

        public int? EntryBy { get; set; }

        public DateTime? EntryDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedDate { get; set; }  
    }
}
