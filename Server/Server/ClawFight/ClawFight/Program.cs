﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClawFight
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpConnect tc = new TcpConnect();
            tc.Start();

            Console.ReadKey();
        }
    }
}
