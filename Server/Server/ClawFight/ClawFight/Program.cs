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
            GameManager.instance.Init();
            ConnectManager.instance.Start();

            Console.ReadKey();
        }
    }
}
