using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
namespace dp215Inter
{
    class Program
    {
        static void Main(string[] args) {
            Stopwatch st = Stopwatch.StartNew();
            SortingNetwork sn = new SortingNetwork();
            sn.BuildSortingNetwork(@"input4.txt");
            sn.RunSortingNetwork();
            st.Stop();
            Console.Write(sn.ToString());
            Console.WriteLine("\nTime: {0}", st.Elapsed);
            Console.ReadLine();
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

            return ans = "Network is " + ans;
        }

        public void BuildSortingNetwork(string filename) {
            bool firstLineInFile = true;
            string input = null;
            int ctr = 0;
            isValid = true;
            using(StreamReader sr = File.OpenText(filename)) {
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
