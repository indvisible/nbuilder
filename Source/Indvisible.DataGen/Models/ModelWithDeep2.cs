namespace Indvisible.DataGen.Models
{
    public class ModelWithDeep2
    {
        public int Name { get; set; }

        public string CompanyName { get; set; }

        public ModelWithNested ModelWithNested { get; set; }
    }
}