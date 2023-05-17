using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistic
{
    public class Rocket
    {
        public string name;
        public double cargo_mass;
        public string description;
        public double cost;

        public Rocket (string n, double c_m, double c)
        {
            name = n;
            cargo_mass = c_m;
            cost = c;
        }

        public string getName()
        {
            return name;
        }
    }
}
