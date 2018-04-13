
namespace Website.Models
{
    public class ActivityType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ActivityType()
        {

        }

        public ActivityType(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
