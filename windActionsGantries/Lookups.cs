﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace windActionsGantries
{
    class Lookups
    {
        public static Dictionary<string, double> InputTerrain()
        {
            int number = 0;
            Console.WriteLine("Terrain Category as per Table 4.1 (0 to 4) : ");
            string input = Console.ReadLine();
            while (!Int32.TryParse(input, out number) || number < 0 || number > 4)
            {
                Console.WriteLine("Value entered is not an integer between 0 and 4! Try Again");
                Console.WriteLine("Terrain Category as per Table 4.1 (0 to 4) : ");
                input = Console.ReadLine();
            }
            //Create new dictionary to be used in switch statement.
            Dictionary<string, double> z0zmin = new Dictionary<string, double>();
            //Check for input value and return z0 and zmin values
            switch (number)
            {
                case 0:
                    z0zmin.Add("z0", 0.003);
                    z0zmin.Add("zmin", 1);
                    break;
                case 1:
                    z0zmin.Add("z0", 0.01);
                    z0zmin.Add("zmin", 1);
                    break;
                case 2:
                    z0zmin.Add("z0", 0.05);
                    z0zmin.Add("zmin", 2);
                    break;
                case 3:
                    z0zmin.Add("z0", 0.3);
                    z0zmin.Add("zmin", 5);
                    break;
                case 4:
                    z0zmin.Add("z0", 1);
                    z0zmin.Add("zmin", 10);
                    break;
            }
            return z0zmin;
        }
        /// <summary>
        /// Input Connection Type, validate its input and return Damping Factor delta_s
        /// </summary>
        /// <returns>Logarithmic decrement of structural damping in the fundamental mode</returns>
        public static double InputConnecType()
        {
            int number = 0;
            Console.WriteLine(@"Primary plate connections perpendicular to longitudinal axis for Damping
            1 =[welded] 2 =[high resistance bolts] 3 =[ordinary bolts] 4 =[Enter number...] : ");
            string input = Console.ReadLine();
            while (!Int32.TryParse(input, out number) || number < 1 || number > 4)
            {
                Console.WriteLine("Value entered is not an integer between 1 and 4! Try Again");
                input = Console.ReadLine();
            }
            //Initialise delta_s variable
            double delta_s = 0;
            //Check for input value and return delta_s - damping factor
            switch (number)
            {
                case 1:
                    delta_s = 0.02;
                    break;
                case 2:
                    delta_s = 0.03;
                    break;
                case 3:
                    delta_s = 0.05;
                    break;
                case 4:
                    delta_s = Validation.inputNumber("Please enter a structural damping factor [0 to 0.15 typ] : ");
                    break;
            }
            return delta_s;
        }

        public static double inputDampingAS()
        {
            int number = 0;
            Console.WriteLine(@"Input structural type and case for damping calculations AS1170 Cl6.2.2
            1 =[steel ULS] 2 =[steel SLS deflection] 3 =[steel SLS acceleration]
                4 =[concrete ULS] 5 =[concrete SLS deflection] 6 =[concrete SLS acceleration]
                7 =[Enter number...] : ");
            string input = Console.ReadLine();
            while (!Int32.TryParse(input, out number) || number < 1 || number > 7)
            {
                Console.WriteLine("Value entered is not an integer between 1 and 7! Try Again");
                input = Console.ReadLine();
            }
            //Initialise delta_s variable
            double output = 0;
            //Check for input value and return delta_s - damping factor
            switch (number)
            {
                case 1:
                    output = 0.02;
                    break;
                case 2:
                    output = 0.012;
                    break;
                case 3:
                    output = 0.01;
                    break;
                case 4:
                    output = 0.03;
                    break;
                case 5:
                    output = 0.015;
                    break;
                case 6:
                    output = 0.01;
                    break;
                case 7:
                    output = Validation.inputNumber("Please enter a structural damping factor [0 to 0.3 typ] : ");
                    break;
            }
            return output;
        }

        /// <summary>
        /// Function to find intersection point between 2 cartesian points with a known x-value
        /// </summary>
        /// <param name="x">Known x-value for which a y-value is needed</param>
        /// <param name="x0">smaller x-value</param>
        /// <param name="x1">larger x-value</param>
        /// <param name="y0">y-value associated with smaller x-value</param>
        /// <param name="y1">y-value associated with larger x-value</param>
        /// <returns></returns>
        static double linear(double x, double x0, double x1, double y0, double y1)
        {
            if ((x1 - x0) == 0)
            {
                return (y0 + y1) / 2;
            }
            return y0 + (x - x0) * (y1 - y0) / (x1 - x0);
        }

        /// <summary>
        /// Input Terrain and Height as per Australian Code and get the Iz value as per Table 6.1
        /// </summary>
        /// <param name="x">Height of structure under consideration</param>
        /// <returns></returns>
        public static double InputTerrainIh(double x)
        {
            int terrain = 0;
            Console.WriteLine("Terrain Category as per Table 6.1 (1 to 4) : ");
            string input = Console.ReadLine();
            while (!Int32.TryParse(input, out terrain) || terrain < 1 || terrain > 4)
            {
                Console.WriteLine("Value entered is not an integer between 1 and 4! Try Again");
                Console.WriteLine("Terrain Category as per Table 6.1 (1 to 4) : ");
                input = Console.ReadLine();
            }

            int[] h_vals = { 0, 5, 10, 15, 20, 30, 40, 50, 75, 100, 150, 200 };
            double[,] intensity = new double[,] {{ .165, .165, .157, .152, .147, .140, .133, .128, .118, .108, .095, .085 },
                                                { .196, .196, .183, .176, .171, .162, .156, .151, .140, .131, .117, .107},
                                                { .271, .271, .239, .225, .215, .203, .195, .188, .176, .166, .150, .139},
                                                { .342, .342, .342, .342, .342, .305, .285, .270, .248, .233, .210, .196} };
            int i = 0;
            while (h_vals[i] < x)
            {
                i++;
            }
            return linear(x, h_vals[i-1], h_vals[i],intensity[terrain-1, i-1],intensity[terrain-1, i]);
        }

        public static double inputStrouhal(double x)
        {
            int[] db_vals = { 0, 1, 2, 3, 4, 5, 10};
            double[] St_vals = new double[] { .12, .12, .06, .06, .15, .11, .09};
            int i = 0;
            while (db_vals[i] < x)
            {
                i++;
            }
            return linear(x, db_vals[i - 1], db_vals[i], St_vals[i - 1], St_vals[i]);
        }
    }
}
