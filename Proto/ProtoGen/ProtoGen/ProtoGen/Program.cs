using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProtoGen
{
    class Program
    {
        static void Main(string[] args)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo()
            {
                FileName = @"protoc.exe",
                WindowStyle = ProcessWindowStyle.Hidden,
            };
            string protoDir = @"proto";
            string csharp_out = @"csharp";
            DirectoryInfo di = new DirectoryInfo(protoDir);
            string arg0 = string.Format("--proto_path={0}", protoDir);
            string arg1 = string.Format("--csharp_out={0}",csharp_out);
            foreach (FileInfo fi in di.GetFiles())
            {
                string protoName = fi.Name;
                string arg = string.Format("{0} {1} {2}", arg0, protoName, arg1);

                p.StartInfo.Arguments = arg;
                p.Start();
                Console.WriteLine(protoName);
            }
            Console.WriteLine("Done!");
            Console.ReadKey();
        }
    }
}
