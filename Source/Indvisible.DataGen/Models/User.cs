namespace Indvisible.DataGen.Models
{
    using System;
    using System.Collections.Generic;

    class User
    {
        public int Id { get; set; }

        public string Address { get; set; }

        public DateTime DateObirth { get; set; }

        public ICollection<Mail> Mails { get; set; } 

    }
}