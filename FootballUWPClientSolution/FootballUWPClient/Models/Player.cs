using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FootballUWPClient.Models
{
    public class Player
    {
        public int ID { get; set; }

        public string FullName
        {
            get
            {
                return Name + " " + LastName;
            }
        }

        public string FormalName
        {
            get
            {
                return LastName + ", " + Name;
            }
        }

        public int Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int a = today.Year - DOB.Year
                    - ((today.Month < DOB.Month || (today.Month == DOB.Month && today.Day < DOB.Day) ? 1 : 0));
                return a; /*Note: You could add .PadLeft(3) but spaces disappear in a web page. */
            }
        }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Jersey { get; set; }

        public DateTime DOB { get; set; }

        public double FeePaid { get; set; }

        public string EMail { get; set; }

        public Byte[] RowVersion { get; set; }

        public ICollection<PlayerTeam> PlayerTeams { get; set; } = new HashSet<PlayerTeam>();

    }
}
