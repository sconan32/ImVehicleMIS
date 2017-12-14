using System;
using System.Collections.Generic;
using System.Text;

namespace Socona.SearchParser
{
    public class LevenshteinDistance
    {


        public static int GetDistance(string fromString, string toString)
        {
            if (fromString == null) fromString = "";
            if (toString == null) toString = "";
            int[,] mem = new int[fromString.Length + 1, toString.Length + 1];
            mem[0, 0] = 0;
            for (int i = 0; i < fromString.Length; i++)
            {
                mem[i + 1, 0] = i + 1;
            }
            for (int j = 0; j < toString.Length; j++)
            {
                mem[0, j + 1] = j + 1;
            }
            for (int i = 0; i < fromString.Length; i++)
            {
                for (int j = 0; j < toString.Length; j++)
                {
                    if (fromString[i] == toString[i])
                    {
                        mem[i + 1, j + 1] = mem[i, j];
                    }
                    else
                    {
                        mem[i + 1, j + 1] = Math.Max(
                            mem[i, j] + 1,
                            Math.Max(mem[i, j + 1] + 1, mem[i + 1, j] + 1)
                            );
                    }
                }
            }
            return mem[fromString.Length, toString.Length];
        }
    }
}
