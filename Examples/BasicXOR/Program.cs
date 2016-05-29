using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;

namespace BasicXOR
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new NamedPipeClientStream("SharpBrain");

            Console.Write("Connecting...");
            client.Connect();
            Console.WriteLine("DONE");

            var reader = new StreamReader(client);
            var writer = new StreamWriter(client)
            {
                AutoFlush = true
            };
                        
            var train = new List<string>();
            train.Add("n0=0 n1=0 n2=0 -e");//   0   0   = 0
            train.Add("n0=0 n1=1 n2=1 -e");//   0   1   = 1
            train.Add("n0=1 n1=0 n2=1 -e");//   1   0   = 1
            train.Add("n0=1 n1=1 n2=0 -e");//   1   1   = 0

            for (int i = 0; i < 1e5; i++)
            {
                double error = 0;
                for (int j = 0; j < train.Count; j++)
                {
                    writer.WriteLine(train[j]);
                    var str = reader.ReadLine();
                    error += double.Parse(str);
                }
                error /= train.Count;
                Console.WriteLine(string.Format("#iteration-{0}: {1}", i, error));
            }


            var test = new List<string>();
            test.Add("n0=0 n1=0");
            test.Add("n0=0 n1=1");
            test.Add("n0=1 n1=0");
            test.Add("n0=1 n1=1");

            Console.WriteLine("Testing...");
            for (int i = 0; i < test.Count; i++)
            {
                writer.WriteLine(test[i]);
                Console.Write(test[i] + "\t");
                Console.WriteLine(reader.ReadLine());
            }
            
            while (true)
            {
                try
                {
                    writer.WriteLine(Console.ReadLine());
                    Console.WriteLine(reader.ReadLine());
                }
                catch (Exception e)
                { Console.WriteLine("Error => " + e.Message); }
            }
        }
    }
}
