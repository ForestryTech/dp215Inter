using System;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace dp215Inter
{
    class Program
    {
        static void Main(string[] args) {
            if (args.Count() != 1) {
                Console.WriteLine("Usage: dp215Inter.exe {i}filename\n");
                return;
            }

            Stopwatch st = Stopwatch.StartNew();
            SortingNetwork sn = new SortingNetwork();

            try {
                sn.BuildSortingNetwork(@args[0]);
                sn.RunSortingNetwork();
            }
            catch (FileNotFoundException f) {

                Console.Write("File {0} not found.\n", args[0]);
                Console.Write(f.Message);
                return;
            }
            catch (IndexOutOfRangeException ex) {
                Console.Write("Bad input.\n");
                Console.Write(ex.Message);
            }
            catch (Exception e) {
                Console.Write("Error. Program could not complete.\n");
                Console.Write(e.Message);
            }
            st.Stop();
            Console.Write(sn.ToString());
            Console.Write("\r\nTime: {0}", st.Elapsed);
        }
    }

    class SortingNetwork
    {
        int numberOfWires;
        int numberOfComparators;
        int[,] comparators;
        double maxValuesToCompare;
        bool isValid;

        
        public override string ToString() {
            string ans;
            if (isValid)
                ans = "valid";
            else
                ans = "not valid";

            return "Network is " + ans;
        }

        public void BuildSortingNetwork(string filename) {
            bool firstLineInFile = true;
            string input = null;
            int ctr = 0;
            isValid = true;

            try {
                using (StreamReader sr = File.OpenText(filename)) {
                    while ((input = sr.ReadLine()) != null) {
                        int[] vals = input.Split(' ').Select(s => Convert.ToInt32(s)).ToArray();
                        if (firstLineInFile) {
                            numberOfWires = vals[0];
                            numberOfComparators = vals[1];
                            comparators = new int[numberOfComparators, 2];
                            maxValuesToCompare = Math.Pow(2, numberOfWires);
                            firstLineInFile = false;
                        } else {
                            comparators[ctr, 0] = vals[0];
                            comparators[ctr, 1] = vals[1];
                            ctr++;
                        }
                    }
                }
            }
            catch (FileNotFoundException f) {
                throw f;
            }
            catch (IndexOutOfRangeException ex) {
                throw ex;
            }
            catch (Exception e) {
                throw e;
            }

        }

        private int[] getValuesToSort(int numberToConvert) {
            int[] nums = new int[numberOfWires];
            for (int i = 0; i < nums.Count(); i++) {
                nums[i] = numberToConvert & 1;
                numberToConvert = numberToConvert >> 1;
            }
            return nums;
        }

        public void RunSortingNetwork() {
            int[] input;
            int ctr = 1;
            int temp;
            bool goodNetwork = true;
            try {
                while (ctr < maxValuesToCompare && goodNetwork) {
                    input = getValuesToSort(ctr);
                    for (int i = 0; i < numberOfComparators; i++) {
                        int upperWire = comparators[i, 0];
                        int lowerWire = comparators[i, 1];
                        if (input[upperWire] > input[lowerWire]) {
                            temp = input[upperWire];
                            input[upperWire] = input[lowerWire];
                            input[lowerWire] = temp;
                        }
                    }

                    ctr++;
                    if (!isSorted(input)) {
                        goodNetwork = false;
                        isValid = false;
                    }
                }
            }
            catch (IndexOutOfRangeException ex) {
                throw ex;
            }
            catch (Exception e) {
                throw e;
            }
        }

        private bool isSorted(int[] valuesToCheck) {
            int lowNumber = 0;
            for (int i = 0; i < valuesToCheck.Count() - 1; i++) {
                if (valuesToCheck[i] >= lowNumber) {
                    lowNumber = valuesToCheck[i];
                } else {
                    // not sorted
                    return false;
                }
            }
            return true;
        }


    }
}
