﻿using System;

namespace func
{
    class Program
    {
        static void Main(string[] args)
        {
            // var advToeter = new AdvancedToeterAsync();
            // advToeter.SendEmail(2,6,4).Wait();

            var car = WeirdCarCreator.CreateCarA(1, 666, "brand");
        }
    }
}
