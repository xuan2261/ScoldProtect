
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ScoldProtect.Core.String
{
    class StringEnc2
    {

        public static void Run(ModuleDefMD module)
        {
            var typeModule = ModuleDefMD.Load(typeof(StringDec2).Module);
            var typeDef = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(StringDec2).MetadataToken));
            var members = InjectHelper.Inject(typeDef, module.GlobalType, module);
            var decoderMethod = (MethodDef)members.Single(method => method.Name == "Decrypt");
            foreach (var md in module.GlobalType.Methods)
            {
                if (md.Name != ".ctor") continue;
                module.GlobalType.Remove(md);
                break;
            }
            foreach (var type in module.GetTypes())
            {
                if (type.IsGlobalModuleType) continue;
                foreach (var method in type.Methods)
                {
                    if (!method.HasBody) continue;
                    var instr = method.Body.Instructions;
                    for (var i = 0; i < instr.Count; i++)
                    {
                        if (instr[i].OpCode != OpCodes.Ldstr) continue;
                        var originalSTR = instr[i].Operand as string;
                        var encodedSTR = Encrypt(originalSTR);
                        instr[i].Operand = encodedSTR;
                        instr.Insert(i + 1, Instruction.Create(OpCodes.Call, decoderMethod));
                    }
                    method.Body.SimplifyBranches();
                }
            }
        }


        public static string Encrypt(string plainText)
        {
            byte[] pass = Encoding.UTF8.GetBytes("48235728");
            byte[] text = Encoding.UTF8.GetBytes(plainText);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            ICryptoTransform transform = des.CreateEncryptor(pass, Encoding.ASCII.GetBytes("fmwa3x6k"));
            byte[] inArray;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(text, 0, text.Length);
                    cryptoStream.FlushFinalBlock();
                    inArray = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(inArray);
        }
    }
}
