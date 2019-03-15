#define DEBUG_PRINT
//#define PERMUTATION
//#define JOHNSON_2MACHINES_PRINT
//#define PERMUTATION_2MACHINES_PRINT
#define PERMUTATION_3MACHINES_PRINT
#define JOHNSON_3MACHINES_PRINT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lab1
{
    class Program
    {
        static void SetValue(Machine machine, string[] name, double[] time)
        {
            for (int i = 0; i < 5; ++i)
            {

                machine.jobs[i].jobName = name[i];
                machine.jobs[i].time = time[i];
                machine.jobs[i].taken = false;
            }
        }

        static double MakeSpan(ref Machine mach1, ref Machine mach2)
        {
            double[] timeStart = new double[mach1.jobs.Length];
            double[] timeEnd = new double[mach1.jobs.Length];
            timeStart[0] = mach1.jobs[0].time;
            timeEnd[0] = timeStart[0] + mach2.jobs[0].time;
            double Cmax = mach1.jobs[0].time;
            for (int i = 1; i < mach1.jobs.Length; i++)
            {
                Cmax += mach1.jobs[i].time;
                if (Cmax > timeEnd[i - 1])
                {
                    timeStart[i] = Cmax;
                    timeEnd[i] = Cmax + mach2.jobs[i].time;
                }
                else
                {
                    timeStart[i] = timeEnd[i - 1];
                    timeEnd[i] = timeStart[i] + mach2.jobs[i].time;
                }
            }
            Cmax = timeEnd[mach1.jobs.Length - 1];
            return Cmax;
        }

        static int Factorial(int number)
        {
            if (number == 1)
                return 1;
            else
                return number * Factorial(number - 1);
        }

        static void Print(Machine machine)
        {
            int length = machine.jobs.Length;

            for (int i = 0; i < length; ++i)
            {
                Console.WriteLine("JobName {0}, Time {1}", machine.jobs[i].jobName, machine.jobs[i].time);
            }
            Console.Write(Environment.NewLine);
        }
        static Machine VirtualMachine(ref Machine machine1, ref Machine machine2)
        {
            Machine machine = new Machine(machine1.jobs.Length);
            for (int i = 0; i < machine1.jobs.Length; ++i)
            {
                machine.jobs[i].jobName = machine1.jobs[i].jobName;
                machine.jobs[i].time = machine1.jobs[i].time + machine2.jobs[i].time;
            }
            return machine;
        }

        public static Machine[] allocMemoryForMachines(int numberOfMachines, int numberOfJobs)
        {
            Machine[] machines = new Machine[numberOfMachines];
            for (int i = 0; i < numberOfMachines; ++i)
            {
                machines[i] = new Machine(numberOfJobs);
            }
            return machines;
        }

        // *************************************************************************************************
        //  MAIN fUNCTION
        // *************************************************************************************************

        static void Main(string[] args)
        {
            string path = @"C:\Users\pati\Desktop\semestr 6\SPD\Lab1\Lab1\tests\test";
            string theBestSequence3machines = String.Empty;
            string theBestSequence3Johnson = String.Empty;
            string theBestSequence2machines = String.Empty;
            string theBestSequence2Johnson = String.Empty;
            double theBestTime3 = 0;
            double theBestTimeJohnson2 = 0;
            double theBestTimeJohnson3 = 0;
            double theBestTime2 = 0;
            int numberOfTests = 10;
            string[] paths = new string[numberOfTests];

            for (int i = 0; i < paths.Length; ++i)
            {
                int temp = i + 1;
                paths[i] = path + temp + ".txt";
            }
            // *************************************************************************************************
            // Clear context of file begin the run of project
            // *************************************************************************************************
            using (StreamWriter file = new StreamWriter(@"C:\Users\pati\Desktop\semestr 6\SPD\Lab1\Lab1\tests\WriteTest.txt", false))
            {
                file.Write("");
            }
            // *************************************************************************************************
            // End Clear context of file begin the run of project
            // *************************************************************************************************

            // *************************************************************************************************
            //  MAIN LOOP
            // *************************************************************************************************
            for (int mainIndex = 0; mainIndex < paths.Length; ++mainIndex)
            {
#if DEBUG_PRINT
                Console.WriteLine("\n[MAIN INDEX {0}]", mainIndex);
#endif
                string[] charArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

                string[] lines = File.ReadAllLines(paths[mainIndex]);

                int numberOfJobs = int.Parse(lines[0].Substring(0, 1));
                int numberOfMachines = int.Parse(lines[0].Substring(1, 2));

                Machine[] machines = allocMemoryForMachines(numberOfMachines, numberOfJobs);

                for (int i = 0; i < numberOfJobs; i++)
                {
                    for (int j = 0; j < numberOfMachines; ++j)
                    {
                        string[] values = lines[i + 1].Split(null);
                        machines[j].jobs[i].jobName = charArray[i];
                        machines[j].jobs[i].time = double.Parse(values[j]);
                        machines[j].jobs[i].taken = false;
                    }
                }

#if DEBUG_PRINT
                Console.WriteLine("[MACHINES FROM FILE]");
                for (int i = 0; i < numberOfMachines; ++i)
                {
                    Console.WriteLine("MACHINE{0}", i);
                    Print(machines[i]);
                }
#endif
                // *************************************************************************************************
                // NUMBER OF MACHINES:2, ALGORITHM:PERMUTATION
                // *************************************************************************************************
                if (numberOfMachines == 2)
                {
                    Machine[] machineForPermutation2 = new Machine[2];
                    Machine[] copyMachineToPermutation2 = new Machine[2];

                    for (int i = 0; i < machineForPermutation2.Length; ++i)
                    {
                        machineForPermutation2[i] = machines[i].DeepCopy();
                        copyMachineToPermutation2[i] = machines[i].DeepCopy();
                    }

                    int factorial2 = Factorial(numberOfJobs);
                    string[,] array2 = new string[factorial2, numberOfJobs];
                    int count2 = 0;

                    PermutationAlgorithm.Permutation(ref charArray, 0, numberOfJobs - 1, ref array2, ref count2);

#if PERMUTATION_2MACHINES_PRINT
                Console.WriteLine("All permutations for 2 machines");
                for (int i = 0; i < factorial2; ++i)
                {
                    for (int j = 0; j < numberOfJobs; ++j)
                    {
                        Console.Write("{0} ", array2[i, j]);
                    }
                    Console.Write("\n");
                }
#endif

                    theBestTime2 = MakeSpan(ref copyMachineToPermutation2[0], ref copyMachineToPermutation2[1]);
                    string[] theBestSequence2 = new string[numberOfJobs];

                    for (int i = 0; i < numberOfJobs; i++)
                    {
                        theBestSequence2[i] = copyMachineToPermutation2[0].jobs[i].jobName;
                    }

                    for (int i = 1; i < factorial2; ++i)
                    {
                        for (int j = 0; j < numberOfJobs; ++j)
                        {
                            for (int k = 0; k < numberOfJobs; ++k)
                            {
                                if (machines[0].jobs[k].jobName == array2[i, j])
                                {
                                    machineForPermutation2[0].jobs[j].time = copyMachineToPermutation2[0].jobs[k].time;
                                    machineForPermutation2[0].jobs[j].jobName = copyMachineToPermutation2[0].jobs[k].jobName;
                                }
                                if (machines[1].jobs[k].jobName == array2[i, j])
                                {
                                    machineForPermutation2[1].jobs[j].time = copyMachineToPermutation2[1].jobs[k].time;
                                    machineForPermutation2[1].jobs[j].jobName = copyMachineToPermutation2[1].jobs[k].jobName;
                                }
                            }
                        }

                        double currentTime = MakeSpan(ref machineForPermutation2[0], ref machineForPermutation2[1]);

                        if (currentTime < theBestTime2)
                        {
                            theBestTime2 = currentTime;
                            for (int p = 0; p < numberOfJobs; p++)
                            {
                                theBestSequence2[p] = machineForPermutation2[0].jobs[p].jobName;
                            }
                        }
                    }

#if PERMUTATION_2MACHINES_PRINT
                Console.WriteLine("[PERMUTATION THE SEQUENCE AND TIME");
                Console.WriteLine("The best time for permutation 2 machines = {0}", theBestTime2);
                for (int i = 0; i < numberOfJobs; ++i)
                {
                    Console.Write("{0} ", theBestSequence2[i]);
                }
#endif

                    // *************************************************************************************************
                    // END!!! NUMBER OF MACHINES:2, ALGORITHM:PERMUTATION
                    // *************************************************************************************************


                    // *************************************************************************************************
                    // NUMBER OF MACHINES:2, ALGORITHM:JOHNSON
                    // *************************************************************************************************

                    Machine[] machineForJohnson2 = new Machine[2];
                    Machine[] copyMachineForJohnson2 = new Machine[2];

                    for (int i = 0; i < machineForPermutation2.Length; ++i)
                    {
                        machineForJohnson2[i] = machines[i].DeepCopy();
                        copyMachineForJohnson2[i] = machines[i].DeepCopy();
                    }

                    Job[] outputJobs2 = JohnsonAlgorithm.JohnsonAlg(ref copyMachineForJohnson2[0], ref copyMachineForJohnson2[1]);

                    for (int j = 0; j < numberOfJobs; ++j)
                    {
                        for (int k = 0; k < numberOfJobs; ++k)
                        {
                            if (outputJobs2[j].jobName == charArray[k])
                            {
                                machineForJohnson2[0].jobs[j].time = copyMachineForJohnson2[0].jobs[k].time;
                                machineForJohnson2[0].jobs[j].jobName = copyMachineForJohnson2[0].jobs[k].jobName;

                                machineForJohnson2[1].jobs[j].time = copyMachineForJohnson2[1].jobs[k].time;
                                machineForJohnson2[1].jobs[j].jobName = copyMachineForJohnson2[1].jobs[k].jobName;
                            }
                        }
                    }
                    theBestTimeJohnson2 = MakeSpan(ref machineForJohnson2[0], ref machineForJohnson2[1]);

#if JOHNSON_2MACHINES_PRINT
                Console.WriteLine("[JOHNSON][2 MACHINES]");
                Console.WriteLine("The best time = {0}", MakeSpan(ref machineForJohnson2[0], ref machineForJohnson2[1]));
                for (int i = 0; i < numberOfJobs; ++i)
                {
                    Console.Write("{0} ", outputJobs2[i].jobName);
                }
                Console.Write(Environment.NewLine);
#endif
                    foreach (string value in theBestSequence2)
                    {
                        theBestSequence2machines += value;
                    }
                    for (int i = 0; i < numberOfJobs; ++i)
                    {
                        theBestSequence2Johnson += outputJobs2[i].jobName;
                    }
                }
                // *************************************************************************************************
                // END!!! NUMBER OF MACHINES:2, ALGORITHM:JOHNSON
                // *************************************************************************************************


                // *************************************************************************************************
                // NUMBER OF MACHINES:3, ALGORITHM:PERMUTATION
                // *************************************************************************************************
                if (numberOfMachines > 2)
                {
                    Machine[] machineForPermutation3 = new Machine[3];
                    Machine[] copyMachineToPermutation3 = new Machine[3];

                    for (int i = 0; i < machineForPermutation3.Length; ++i)
                    {
                        machineForPermutation3[i] = machines[i].DeepCopy();
                        copyMachineToPermutation3[i] = machines[i].DeepCopy();
                    }

                    Machine[] virtualMachineForPermutation3 = new Machine[2];

                    for (int i = 0; i < virtualMachineForPermutation3.Length; ++i)
                    {
                        virtualMachineForPermutation3[i] = VirtualMachine(ref machineForPermutation3[i], ref machineForPermutation3[i + 1]);
                    }

#if PERMUTATION_3MACHINES_PRINT
                    Console.WriteLine("\n[PERMUTATION][3 MACHINES]");
                    Console.WriteLine("Virtual Machines");
                    for (int i = 0; i < virtualMachineForPermutation3.Length; ++i)
                    {
                        for (int j = 0; j < numberOfJobs; ++j)
                        {
                            Console.Write("{0}={1} ", virtualMachineForPermutation3[i].jobs[j].jobName, virtualMachineForPermutation3[i].jobs[j].time);
                        }
                        Console.Write("\n");
                    }
#endif

                    Machine[] copyVirtualMachineForPermutation3 = new Machine[2];

                    for (int i = 0; i < copyVirtualMachineForPermutation3.Length; ++i)
                    {
                        copyVirtualMachineForPermutation3[i] = virtualMachineForPermutation3[i].DeepCopy();
                    }

                    int factorial3 = Factorial(numberOfJobs);
                    string[,] array3 = new string[factorial3, numberOfJobs];
                    int count3 = 0;

                    PermutationAlgorithm.Permutation(ref charArray, 0, numberOfJobs - 1, ref array3, ref count3);

#if PERMUTATION
                Console.WriteLine("[PERMUTATION][3 MACHINES]");
                for (int i = 0; i < factorial3; ++i)
                {
                    for (int j = 0; j < numberOfJobs; ++j)
                    {
                        Console.Write("{0} ", array3[i, j]);
                    }
                    Console.Write("\n");
                }
#endif
                    theBestTime3 = MakeSpan(ref copyVirtualMachineForPermutation3[0], ref copyVirtualMachineForPermutation3[1]);
                    string[] theBestSequence3 = new string[numberOfJobs];

                    for (int i = 0; i < numberOfJobs; i++)
                    {
                        theBestSequence3[i] = copyVirtualMachineForPermutation3[0].jobs[i].jobName;
                    }

                    for (int i = 1; i < factorial3; ++i)
                    {
                        for (int j = 0; j < numberOfJobs; ++j)
                        {
                            for (int k = 0; k < numberOfJobs; ++k)
                            {
                                if (machines[0].jobs[k].jobName == array3[i, j])
                                {
                                    virtualMachineForPermutation3[0].jobs[j].time = copyVirtualMachineForPermutation3[0].jobs[k].time;
                                    virtualMachineForPermutation3[0].jobs[j].jobName = copyVirtualMachineForPermutation3[0].jobs[k].jobName;
                                }
                                if (machines[1].jobs[k].jobName == array3[i, j])
                                {
                                    virtualMachineForPermutation3[1].jobs[j].time = copyVirtualMachineForPermutation3[1].jobs[k].time;
                                    virtualMachineForPermutation3[1].jobs[j].jobName = copyVirtualMachineForPermutation3[1].jobs[k].jobName;
                                }
                            }
                        }

                        double currentTime = MakeSpan(ref virtualMachineForPermutation3[0], ref virtualMachineForPermutation3[1]);

                        if (currentTime < theBestTime3)
                        {
                            theBestTime3 = currentTime;
                            for (int p = 0; p < numberOfJobs; p++)
                            {
                                theBestSequence3[p] = virtualMachineForPermutation3[0].jobs[p].jobName;
                            }
                        }
                    }

#if PERMUTATION_3MACHINES_PRINT
                    Console.WriteLine("\n[PERMUTATION][3 MACHINES][THE BEST TIME AND SEQUENCES]");
                    Console.WriteLine("The best time for permutation 3 machines = {0}", theBestTime3);
                    for (int i = 0; i < numberOfJobs; ++i)
                    {
                        Console.Write("{0} ", theBestSequence3[i]);
                    }
                    Console.Write("\n");
#endif

                    // *************************************************************************************************
                    // END!!! NUMBER OF MACHINES:3, ALGORITHM:PERMUTATION
                    // *************************************************************************************************


                    // *************************************************************************************************
                    // NUMBER OF MACHINES:3, ALGORITHM:JOHNSON
                    // *************************************************************************************************

                    Machine[] machineForJohnson3 = new Machine[3];
                    Machine[] copyMachineForJohnson3 = new Machine[3];

                    for (int i = 0; i < machineForPermutation3.Length; ++i)
                    {
                        machineForJohnson3[i] = machines[i].DeepCopy();
                        copyMachineForJohnson3[i] = machines[i].DeepCopy();
                    }

                    Machine[] virtualMachineForJohnson3 = new Machine[2];

                    for (int i = 0; i < virtualMachineForJohnson3.Length; ++i)
                    {
                        virtualMachineForJohnson3[i] = VirtualMachine(ref machineForJohnson3[i], ref machineForJohnson3[i + 1]);
                    }

#if JOHNSON_3MACHINES_PRINT
                    Console.WriteLine("\n[JOHNSON][3 MACHINES]");
                    Console.WriteLine("Virtual Machines");
                    for (int i = 0; i < virtualMachineForJohnson3.Length; ++i)
                    {
                        for (int j = 0; j < numberOfJobs; ++j)
                        {
                            Console.Write("{0}={1} ", virtualMachineForJohnson3[i].jobs[j].jobName, virtualMachineForJohnson3[i].jobs[j].time);
                        }
                        Console.Write("\n");
                    }
#endif

                    Machine[] copyVirtualMachineForJohnson3 = new Machine[2];

                    for (int i = 0; i < copyVirtualMachineForJohnson3.Length; ++i)
                    {
                        copyVirtualMachineForJohnson3[i] = virtualMachineForJohnson3[i].DeepCopy();
                    }

                    Job[] outputJobs3 = JohnsonAlgorithm.JohnsonAlg(ref copyVirtualMachineForJohnson3[0], ref copyVirtualMachineForJohnson3[1]);

                    for (int j = 0; j < numberOfJobs; ++j)
                    {
                        for (int k = 0; k < numberOfJobs; ++k)
                        {
                            if (outputJobs3[j].jobName == charArray[k])
                            {
                                virtualMachineForJohnson3[0].jobs[j].time = copyVirtualMachineForJohnson3[0].jobs[k].time;
                                virtualMachineForJohnson3[0].jobs[j].jobName = copyVirtualMachineForJohnson3[0].jobs[k].jobName;

                                virtualMachineForJohnson3[1].jobs[j].time = copyVirtualMachineForJohnson3[1].jobs[k].time;
                                virtualMachineForJohnson3[1].jobs[j].jobName = copyVirtualMachineForJohnson3[1].jobs[k].jobName;
                            }
                        }
                    }
                    theBestTimeJohnson3 = MakeSpan(ref virtualMachineForJohnson3[0], ref virtualMachineForJohnson3[1]);

#if JOHNSON_3MACHINES_PRINT
                    Console.WriteLine("\n[JOHNSON][3 MACHINES]");
                    Console.WriteLine("The best time = {0}", MakeSpan(ref virtualMachineForJohnson3[0], ref virtualMachineForJohnson3[1]));
                    for (int i = 0; i < numberOfJobs; ++i)
                    {
                        Console.Write("{0} ", outputJobs3[i].jobName);
                    }
                    Console.Write(Environment.NewLine);
#endif
                    foreach (string value in theBestSequence3)
                    {
                        theBestSequence3machines += value;
                    }
                    for (int i = 0; i < numberOfJobs; ++i)
                    {
                        theBestSequence3Johnson += outputJobs3[i].jobName;
                    }
                }
                // *************************************************************************************************
                // END!!! NUMBER OF MACHINES:3, ALGORITHM:JOHNSON
                // *************************************************************************************************

                // *************************************************************************************************
                // BEGIN: Write to file
                // *************************************************************************************************
                using (StreamWriter file = new StreamWriter(@"C:\Users\pati\Desktop\semestr 6\SPD\Lab1\Lab1\tests\WriteTest.txt", true))
                {
                    if (numberOfMachines == 3)
                    {
                        file.Write("test" + (mainIndex + 1) + ": " + Environment.NewLine +"\n"+ "[3 Machines] " + Environment.NewLine + "[Permutation]" + "\t Sequences : " + theBestSequence3machines + "\t Time: " + theBestTime3.ToString());
                        file.Write(Environment.NewLine + "[Johnson]" + "\t Sequences : " + theBestSequence3Johnson + "\t Time: " + theBestTimeJohnson3.ToString() + Environment.NewLine);
                        file.Write(Environment.NewLine);
                    }
                    else
                    {
                        file.Write("test" + (mainIndex + 1) + ": " + Environment.NewLine + "\n"+ "[2 Machines] " + Environment.NewLine + "[Permutation]" + "\t Sequences : " + theBestSequence2machines + "\t Time: " + theBestTime2.ToString() );
                        file.Write(Environment.NewLine + "[Johnson]" + "\t Sequences : " + theBestSequence2Johnson + "\t Time: " + theBestTimeJohnson2.ToString() + Environment.NewLine);
                        file.Write(Environment.NewLine);
                    }
                }
                // *************************************************************************************************
                // END!!! Write to file
                // *************************************************************************************************
                theBestSequence3machines = String.Empty;
                theBestSequence3Johnson = String.Empty;
                theBestSequence2machines = String.Empty;
                theBestSequence2Johnson = String.Empty;
                theBestTime3 = 0;
                theBestTimeJohnson2 = 0;
                theBestTimeJohnson3 = 0;
                theBestTime2 = 0;
            }
            Console.ReadKey();
        }
    }
}
