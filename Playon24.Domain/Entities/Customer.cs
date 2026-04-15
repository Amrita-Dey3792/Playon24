using System.ComponentModel.DataAnnotations;

namespace Playon24.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public int Id { get; set; }


        [StringLength(100)]
        public string FirstName { get; set; }


        [StringLength(100)]
        public string LastName { get; set; }


        [StringLength(200)]
        public string Email { get; set; }


        [StringLength(20)]
        public string Phone { get; set; }


        [StringLength(300)]
        public string Address { get; set; }


        [StringLength(100)]
        public string City { get; set; }

    }
}
