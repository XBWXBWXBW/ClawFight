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
            //generate proto script
            GenerateProtoScript();

            //add EMessageType
            AddMessageType();


            Console.WriteLine("Done!");
            Console.ReadKey();
        }
        private static void GenerateProtoScript() {
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
            string arg1 = string.Format("--csharp_out={0}", csharp_out);
            foreach (FileInfo fi in di.GetFiles())
            {
                string protoName = fi.Name;
                string arg = string.Format("{0} {1} {2}", arg0, protoName, arg1);

                p.StartInfo.Arguments = arg;
                p.Start();
                Console.WriteLine(protoName);
            }

            string clientPath = @"..\Client\ClawFight\ClawFight\Assets\Scripts\Message\Message";
            DirectoryInfo clientDI = new DirectoryInfo(clientPath);
            while (clientDI.GetFiles().Length != 0)
            {
                clientDI.GetFiles()[0].Delete();
            }

            string serverPath = @"..\Server\Server\ClawFight\ClawFight\Message";
            DirectoryInfo serverDI = new DirectoryInfo(serverPath);
            while (serverDI.GetFiles().Length != 0)
            {
                serverDI.GetFiles()[0].Delete();
            }

            string csharpPath = @"csharp";
            DirectoryInfo _csharpDI = new DirectoryInfo(csharpPath);
            foreach (var fi in _csharpDI.GetFiles())
            {
                string cDst = string.Format(@"{0}\{1}", clientPath, fi.Name);
                fi.CopyTo(cDst, true);

                string sDst = string.Format(@"{0}\{1}", serverPath, fi.Name);
                fi.CopyTo(sDst, true);
            }
        }
        private static void AddMessageType() {
            string originalTypePath = @"EMessageType.txt";
            string content = File.ReadAllText(originalTypePath);
            string final = string.Format("public enum EMessageType{{\n{0}\n}}",content);

            string clientTypePath = @"..\Client\ClawFight\ClawFight\Assets\Scripts\Message\EMessageType.cs";
            if (!File.Exists(clientTypePath)) {
                File.Create(clientTypePath);
            }
            File.WriteAllText(clientTypePath, final);

            string serverTypePath = @"..\Server\Server\ClawFight\ClawFight\EMessageType\EMessageType.cs";
            if (!File.Exists(serverTypePath)) {
                File.Create(serverTypePath);
            }
            File.WriteAllText(serverTypePath, final);
        }
    }
}
