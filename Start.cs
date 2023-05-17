using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistic
{
    public class Start
    {
        public List<Cargo> cargo_start = new List<Cargo> { };
        public List<Rocket> rocket_start = new List<Rocket> { };
        public double result_mass = 0;
        public int status;
        public string date;
        public Start(Rocket r)
        {
            rocket_start.Add(r);
        }

        public void result (Cargo c)
        {
            cargo_start.Add(c);
            result_mass = result_mass + c.mass;
        }
    }
}
