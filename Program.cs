using System;

namespace func
{
    class Program
    {
        static void Main(string[] args)
        {
            var advToeter = new AdvancedToeter();
            advToeter.SendEmail(2,6,4);
        }
    }
}
