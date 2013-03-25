namespace Indvisible.DataGen.Models
{
    using System;
    using System.Collections.Generic;

    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string Address { get; set; }

        public DateTime DateObirth { get; set; }

        public IList<Mail> Mails { get; set; }
    }
}