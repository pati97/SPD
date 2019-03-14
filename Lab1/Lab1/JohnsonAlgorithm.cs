using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class JohnsonAlgorithm
    {
        public static Job[] JohnsonAlg(ref Machine inputJobsA, ref Machine inputJobsB)
        {
            int length = inputJobsA.jobs.Length;
            Job[] output_job = new Job[length];
            Job first = inputJobsA.jobs[0];
            Job second = inputJobsB.jobs[0];
            int first_mod_index = 0;
            int second_mod_index = 0;
            int first_index = 0;
            int second_index = inputJobsA.jobs.Length - 1;

            for (int i = 0; i < length; i++)
            {
                first_mod_index = 0;
                second_mod_index = 0;

                for (int j = 0; j < length; ++j)
                {
                    if (inputJobsA.jobs[j].taken == false && inputJobsB.jobs[j].taken == false)
                    {
                        first = inputJobsA.jobs[j];
                        second = inputJobsB.jobs[j];
                        first_mod_index = j;
                        second_mod_index = j;
                        break;
                    }
                }

                for (int f = 1; f < length; f++)
                {
                    if (inputJobsA.jobs[f].time < first.time && inputJobsA.jobs[f].taken == false)
                    {
                        first = inputJobsA.jobs[f];
                        first_mod_index = f;
                    }
                }

                for (int s = 1; s < length; s++)
                {
                    if (inputJobsB.jobs[s].time < second.time && inputJobsB.jobs[s].taken == false)
                    {
                        second = inputJobsB.jobs[s];
                        second_mod_index = s;
                    }
                }

                if (first.time <= second.time)
                {
                    output_job[first_index].time = first.time;
                    output_job[first_index].jobName = first.jobName;
                    inputJobsA.jobs[first_mod_index].time = first.time;
                    inputJobsA.jobs[first_mod_index].taken = true;

                    for (int x = 0; x < length; x++)
                    {
                        if (inputJobsB.jobs[x].jobName == first.jobName)
                            inputJobsB.jobs[x].taken = true;
                    }

                    ++first_index;
                    
                }
                else
                {
                    output_job[second_index].time = second.time;
                    output_job[second_index].jobName = second.jobName;
                    inputJobsB.jobs[second_mod_index].time = second.time;
                    inputJobsB.jobs[second_mod_index].taken = true;

                    for (int x = 0; x < length; x++)
                    {
                        if (inputJobsA.jobs[x].jobName == second.jobName)
                            inputJobsA.jobs[x].taken = true;
                    }
                    --second_index;
                    
                }

            }

            return output_job;
        }
    }
}
