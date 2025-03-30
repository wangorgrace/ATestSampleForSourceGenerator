using System;
using MySourceGenerator.Base.Attributes;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Class1 class1 = new Class1();
            class1.Name = "test";
        }
    }

    [ViewModel]
    public partial class Class1
    {
        [VmProperty]
        [PropertyCallMethod(nameof(DoSomething))]
        private string _name;

        [Command]
        private void DoSomething()
        {
            Console.WriteLine("Hello world");
        }
    }
}
