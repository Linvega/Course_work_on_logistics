using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistic
{
    public class Cargo
    {
        public string name, description, description_alert;
        public DateTime date = new DateTime();
        public double mass;
        public int priority;
        public int status = 0, status_s = 0;
        public bool aoi = false;

        public Cargo(string n, int p, double m, DateTime d)
        {
            name = n;
            priority = p;
            mass = m;
            date = d;
        }
    }
}
