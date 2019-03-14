using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public class Machine
    {
        public Job[] jobs;

        public Machine(int N)
        {
            jobs = new Job[N];
        }

        public Machine DeepCopy()
        {
            Machine machine = new Machine(jobs.Length);

            for (int i = 0; i < jobs.Length; ++i)
            {
                machine.jobs[i].jobName = this.jobs[i].jobName;
                machine.jobs[i].time = this.jobs[i].time;
                machine.jobs[i].taken = false;
            }

            return machine;
        }

    }
}

