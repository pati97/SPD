#define DEBUG_PRINT
#define PERMUTATION_PRINT
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
        static void SetValue( Machine machine, string[] name, double[] time)
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
            double[] timeStart = new double[5];
            double[] timeEnd = new double[5];
            timeStart[0] = mach1.jobs[0].time;
            timeEnd[0] = timeStart[0] + mach2.jobs[0].time;
            double Cmax = mach1.jobs[0].time;
            for (int i = 1; i<mach1.jobs.Length; i++)
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
                return number * Factorial(number-1);
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





        static void Main(string[] args)
        {
            string path = @"C:\Users\pati\Desktop\semestr 6\SPD\Lab1\Lab1\tests\test";

            int numberOfTests = 1;
            string[] paths = new string[numberOfTests];

            for (int i = 0; i < paths.Length; ++i)
            {
                int temp = i + 1;
                paths[i] = path + temp + ".txt";
            }

            for (int mainIndex = 0; mainIndex < paths.Length; ++mainIndex)
            {
#if DEBUG_PRINT
                Console.WriteLine("\n[MAIN INDEX {0}]", mainIndex);
#endif
                string[] charArray = new string[] { "A", "B", "C", "D", "E", "F" };

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

                Machine[] machineForPermutation2 = new Machine[2];
                Machine[] copyMachineToPermutation = new Machine[2];

                for (int i = 0; i < machineForPermutation2.Length; ++i)
                {
                    machineForPermutation2[i] = machines[i].DeepCopy();
                    copyMachineToPermutation[i] = machines[i].DeepCopy(); 
                }

                int factorial = Factorial(numberOfJobs);
                string[,] array = new string[factorial, numberOfJobs];
                int count = 0;

                PermutationAlgorithm.Permutation(ref charArray, 0, numberOfJobs - 1, ref array, ref count);

#if DEBUG_PRINT
                Console.WriteLine("All permutations for 2 machines");
                for (int i = 0; i < factorial; ++i)
                {
                    for (int j = 0; j < numberOfJobs; ++j)
                    {
                        Console.Write("{0} ", array[i, j]);
                    }
                    Console.Write("\n");
                }
#endif

                double theBestTime = MakeSpan(ref copyMachineToPermutation[0], ref copyMachineToPermutation[1]);
                string[] theBestSequence = new string[numberOfJobs];

                for (int i = 0; i < numberOfJobs; i++)
                {
                    theBestSequence[i] = copyMachineToPermutation[0].jobs[i].jobName;
                }

                for (int i = 1; i < factorial; ++i)
                {
                    for (int j = 0; j < numberOfJobs; ++j)
                    {
                        for (int k = 0; k < numberOfJobs; ++k)
                        {
                            if (machines[0].jobs[k].jobName == array[i, j])
                            {
                                machineForPermutation2[0].jobs[j].time = copyMachineToPermutation[0].jobs[k].time;
                                machineForPermutation2[0].jobs[j].jobName = copyMachineToPermutation[0].jobs[k].jobName;
                            }
                            if (machines[1].jobs[k].jobName == array[i, j])
                            {
                                machineForPermutation2[1].jobs[j].time = copyMachineToPermutation[1].jobs[k].time;
                                machineForPermutation2[1].jobs[j].jobName = copyMachineToPermutation[1].jobs[k].jobName;
                            }
                        }
                    }

                    double currentTime = MakeSpan(ref machineForPermutation2[0], ref machineForPermutation2[1]);

                    if (currentTime < theBestTime)
                    {
                        theBestTime = currentTime;
                        for (int p = 0; p < numberOfJobs; p++)
                        {
                            theBestSequence[p] = machineForPermutation2[0].jobs[p].jobName;
                        }
                    }
                }

#if PERMUTATION_PRINT
                Console.WriteLine("[PERMUTATION THE SEQUENCE AND TIME");
                Console.WriteLine("The best time for permutation 2 machines = {0}", theBestTime);
                for (int i = 0; i < numberOfJobs; ++i)
                {
                    Console.Write("{0} ", theBestSequence[i]);
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

                Console.WriteLine("[JOHNSON] The best time = {0}", MakeSpan(ref machineForJohnson2[0], ref machineForJohnson2[1]));

                for (int i = 0; i < numberOfJobs; ++i)
                {
                    Console.Write("{0} ", outputJobs2[i].jobName);
                }
                Console.Write(Environment.NewLine);

                // *************************************************************************************************
                // END!!! NUMBER OF MACHINES:2, ALGORITHM:JOHNSON
                // *************************************************************************************************

                /*
                Job[] outputJob3 = JohnsonAlgorithm.JohnsonAlg(ref virtualMachineToJohnson[0], ref virtualMachineToJohnson[1]);

                for (int i = 0; i < Length; ++i)
                {
                    Console.Write("{0} ", outputJob3[i].jobName);
                }

                for (int j = 0; j < Length; ++j)
                {
                    for (int k = 0; k < Length; ++k)
                    {
                        if (outputJob3[j].jobName == charArray[k])
                        {
                            virtualMachineToJohnson[0].jobs[j].time = virtualMachine[0].jobs[k].time;
                            virtualMachineToJohnson[0].jobs[j].jobName = charArray[k];

                            virtualMachineToJohnson[1].jobs[j].time = virtualMachine[1].jobs[k].time;
                            virtualMachineToJohnson[1].jobs[j].jobName = charArray[k];
                        }
                    }
                }
                Console.WriteLine("[JOHNSON {0} Machines] The best time = {1}", numberOfMachines ,MakeSpan(ref virtualMachineToJohnson[0], ref virtualMachineToJohnson[1]));
                */
            }
            Console.ReadKey();

            // // string[] jobName = new string[]{ "A", "B", "C", "D", "E" };
            // // double[] timeA = new double[] {  3.2, 4.7, 2.2, 5.8, 3.1 };
            // // double[] timeB = new double[] {  4.2, 1.5, 5.0, 4.0, 2.8 };
            // // double[] timeC = new double[] {  3.5, 2.2, 5.5, 7.1, 8.1 };
            // // int Length1 = jobName.Length;
            // //int factorial = Factorial(Length1);
            // // string[,] array = new string[factorial, Length1];
            //  //int count = 0;

            //  Machine mach1 = new Machine(Length1);
            //  Machine mach2 = new Machine(Length1);
            //  Machine mach3 = new Machine(Length1);

            //  Machine VirtualMachine1 = new Machine(Length1);
            //  Machine VirtualMachine2 = new Machine(Length1);

            //  SetValue(mach1, jobName, timeA);
            //  SetValue(mach2, jobName, timeB);
            //  SetValue(mach3, jobName, timeC);

            //  VirtualMachine1 = VirtualMachine( ref mach1,  ref mach2);
            //  VirtualMachine2 = VirtualMachine( ref mach2,  ref mach3);

            //  Print(VirtualMachine1);
            //  Print(VirtualMachine2);

            //  Machine VirtualMachine1ToJohnson = VirtualMachine1.ShallowCopy();
            //  Machine VirtualMachine2ToJohnson = VirtualMachine2.ShallowCopy();

            //  Machine VirtualMachine1Copy = VirtualMachine1.ShallowCopy();
            //  Machine VirtualMachine2Copy = VirtualMachine2.ShallowCopy();

            //  Machine VirtualMachine1CopyCo = VirtualMachine1.ShallowCopy();
            //  Machine VirtualMachine2CopyCo = VirtualMachine2.ShallowCopy();

            //  //-----------------------
            //  //-Johnson Algoritm for 3 machine
            //  //------------------------------------

            //  Console.WriteLine("3 MACHINES");

            //  Print(VirtualMachine1CopyCo);
            //  Print(VirtualMachine2CopyCo);
            //  Job[] outputJob3 = JohnsonAlgorithm.JohnsonAlg(ref VirtualMachine1CopyCo,  ref VirtualMachine2CopyCo);

            //  for (int i = 0; i < Length1; ++i)
            //  {
            //      Console.Write("{0} ", outputJob3[i].jobName);
            //  }

            //  for (int j = 0; j < Length1; ++j)
            //  {
            //      for (int k = 0; k < Length1; ++k)
            //      {
            //          if (outputJob3[j].jobName == jobName[k])
            //          {
            //              VirtualMachine1ToJohnson.jobs[j].time = VirtualMachine1CopyCo.jobs[k].time;
            //              VirtualMachine1ToJohnson.jobs[j].jobName = jobName[k];

            //              VirtualMachine2ToJohnson.jobs[j].time = VirtualMachine2CopyCo.jobs[k].time;
            //              VirtualMachine2ToJohnson.jobs[j].jobName = jobName[k];
            //          }
            //      }
            //  }
            //  Console.WriteLine("[JOHNSON] The best time = {0}", MakeSpan(ref VirtualMachine1ToJohnson, ref VirtualMachine2ToJohnson));

            //  for (int i = 0; i < Length1; ++i)
            //  {
            //      Console.Write("{0} ", outputJob3[i].jobName);
            //  }
            //  Console.Write(Environment.NewLine);
            // // Console.Write("Virtual Mach1 \n");
            // // Print(VirtualMachine1ToJohnson);
            ////  Console.Write("Virtual Mach2 \n");
            ////  Print(VirtualMachine2ToJohnson);

            //  Machine copyMach1 = mach1.ShallowCopy();
            //  Machine copyMach2 = mach2.ShallowCopy();

            //  Console.Write("Mach1 \n");
            //  Print(copyMach1);
            //  Console.Write("Mach2 \n");
            //  Print(copyMach2);

            //  Job[] outputJobs2 = JohnsonAlgorithm.JohnsonAlg(ref mach1,ref mach2);

            //  Machine Mach1ToJohnson = mach1.ShallowCopy();
            //  Machine Mach2ToJohnson = mach2.ShallowCopy();

            //  for (int j = 0; j < Length1; ++j)
            //  {
            //      for (int k = 0; k < Length1; ++k)
            //      {
            //          if (outputJobs2[j].jobName == jobName[k])
            //          {
            //              Mach1ToJohnson.jobs[j].time = timeA[k];
            //              Mach1ToJohnson.jobs[j].jobName = jobName[k];

            //              Mach2ToJohnson.jobs[j].time = timeB[k];
            //              Mach2ToJohnson.jobs[j].jobName = jobName[k];
            //          }
            //      }
            //  }
            //  Console.Write("Mach1 \n");
            //  Print(Mach1ToJohnson);
            //  Console.Write("Mach2 \n");
            //  Print(Mach2ToJohnson);

            //  Console.WriteLine("[JOHNSON] The best time = {0}", MakeSpan(ref Mach1ToJohnson, ref Mach2ToJohnson));

            //  for (int i = 0; i < Length1; ++i)
            //  {
            //      Console.Write("{0} ", outputJobs2[i].jobName);
            //  }
            //  Console.Write(Environment.NewLine);

            //  PermutationAlgorithm.Permutation(ref jobName, 0, jobName.Length - 1, ref array, ref count);

            //  Machine Mach1ToPermutation = new Machine(Length1);
            //  Machine Mach2ToPermutation = new Machine(Length1);

            //  Machine VirtualMachine1ToPermutation = new Machine(Length1); 
            //  Machine VirtualMachine2ToPermutation = new Machine(Length1);

            //  Print(VirtualMachine1Copy);
            //  Print(VirtualMachine2Copy);

            //  double theBestTime = MakeSpan(ref copyMach1, ref copyMach2);
            //  string[] theBestSequence = new string[Length1];

            //  string[] theBestSequence3 = new string[Length1];


            //  Print(VirtualMachine1Copy);
            //  Print(VirtualMachine2Copy);

            //  double theBestTime3 = MakeSpan(ref VirtualMachine1Copy, ref VirtualMachine2Copy);
            //  Print(VirtualMachine1Copy);
            //  Print(VirtualMachine2Copy);
            //  //---------------
            //  //Permutation 3 Machine
            //  //---------------

            //  for (int i = 0; i < Length1; i++)
            //  {
            //      theBestSequence3[i] = VirtualMachine1Copy.jobs[i].jobName;
            //  }

            //  for (int i = 1; i < factorial; ++i)
            //  {
            //      for (int j = 0; j < Length1; ++j)
            //      {
            //          for (int k = 0; k < Length1; ++k)
            //          {
            //              if (mach1.jobs[k].jobName == array[i, j])
            //              {
            //                  VirtualMachine1ToPermutation.jobs[j].time = VirtualMachine1Copy.jobs[k].time;
            //                  VirtualMachine1ToPermutation.jobs[j].jobName = VirtualMachine1Copy.jobs[k].jobName;
            //              }
            //              if (mach2.jobs[k].jobName == array[i, j])
            //              {
            //                  VirtualMachine2ToPermutation.jobs[j].time = VirtualMachine2Copy.jobs[k].time;
            //                  VirtualMachine2ToPermutation.jobs[j].jobName = VirtualMachine2Copy.jobs[k].jobName;
            //              }
            //          }
            //      }

            //      double currentTime3 = MakeSpan(ref VirtualMachine1ToPermutation, ref VirtualMachine2ToPermutation);

            //      if (currentTime3 < theBestTime3)
            //      {
            //          theBestTime3 = currentTime3;
            //          for (int p = 0; p < Length1; p++)
            //          {
            //              theBestSequence3[p] = VirtualMachine1ToPermutation.jobs[p].jobName;
            //          }
            //      }
            //  }
            //  Console.WriteLine("[PERMUTATION 3 Machine] The best time = {0}", theBestTime3);

            //  for (int i = 0; i < Length1; ++i)
            //  {
            //      Console.Write("{0} ", theBestSequence3[i]);
            //  }

            //  //-------------------------------------------------------------
            //  //2 maszyny
            //  //-------------------------------------------------------------
            //  for (int i = 0; i < Length1; i++)
            //  {
            //      theBestSequence[i] = copyMach1.jobs[i].jobName;
            //  }

            //  for (int i = 1; i < factorial; ++i)
            //  {
            //      for (int j = 0; j < Length1; ++j)
            //      {
            //          for (int k = 0; k < Length1; ++k)
            //          {
            //              if (mach1.jobs[k].jobName == array[i, j])
            //              {
            //                  Mach1ToPermutation.jobs[j].time = copyMach1.jobs[k].time;
            //                  Mach1ToPermutation.jobs[j].jobName = copyMach1.jobs[k].jobName;
            //              }
            //              if (mach2.jobs[k].jobName == array[i, j])
            //              {
            //                  Mach2ToPermutation.jobs[j].time = copyMach2.jobs[k].time;
            //                  Mach2ToPermutation.jobs[j].jobName = copyMach2.jobs[k].jobName;
            //              }
            //          }
            //      }

            //      double currentTime = MakeSpan(ref Mach1ToPermutation, ref Mach2ToPermutation);

            //      if (currentTime < theBestTime)
            //      {
            //          theBestTime = currentTime;
            //          for (int p = 0; p < Length1; p++)
            //          {
            //              theBestSequence[p] = Mach1ToPermutation.jobs[p].jobName;
            //          }
            //      }
            //  }

            //  Console.WriteLine("\n[PERMUTATION 2 MAchine] The best time = {0}", theBestTime);

            //  for (int i = 0; i < Length1; ++i)
            //  {
            //      Console.Write("{0} ", theBestSequence[i]);
            //  }

            //  Console.Write("\n\n");
            //  Console.ReadKey();
        }
    }
}
