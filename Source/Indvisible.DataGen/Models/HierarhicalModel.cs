namespace Indvisible.DataGen.Models
{
    public class HierarhicalModel
    {
        public int Id { get; set; }

        public float Price { get; set; }

        public HierarhicalModel Hierarhical { get; set; }
    }
}