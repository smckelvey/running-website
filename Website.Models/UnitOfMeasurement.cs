
namespace Website.Models
{
    public class UnitOfMeasurement
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public UnitOfMeasurement()
        {

        }

        public UnitOfMeasurement(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
