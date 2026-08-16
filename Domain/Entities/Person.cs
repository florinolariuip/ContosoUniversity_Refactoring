using Domain.Entities.Common;

namespace Domain.Entities
{
    public class Person : BaseEntity
    {
        public string? LastName { get; set; }
        public string? FirstMidName { get; set; }

        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }
    }
}
