using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoldProtect.Core.String
{
    class StringEnc
    {
        public static void Run(ModuleDefMD module)
        {
            var typeModule = ModuleDefMD.Load(typeof(StringDec).Module);
            var typeDef2 = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(StringDec).MetadataToken));
            var members = InjectHelper.Inject(typeDef2, module.GlobalType, module);
            var decoderMethod = (MethodDef)members.Single(method => method.Name == "Decrypt");
            foreach (var md in module.GlobalType.Methods)
            {
                if (md.Name != ".ctor") continue;
                module.GlobalType.Remove(md);
                break;
            }
            foreach (var typeDef in module.GetTypes().Where(x => x.HasMethods))
            {
                foreach (var methodDef in typeDef.Methods.Where(x => x.HasBody))
                {
                    var instructions = methodDef.Body.Instructions;
                    for (var i = 0; i < instructions.Count; i++)
                    {
                        if (instructions[i].OpCode == OpCodes.Ldstr &&
                            !string.IsNullOrEmpty(instructions[i].Operand.ToString()))
                        {
                            var key = methodDef.Name.Length;
                            var encryptedString =
                               Ecrypt(new Tuple<string, int>(instructions[i].Operand.ToString(), key));
                            instructions[i].OpCode = OpCodes.Ldstr;
                            instructions[i].Operand = encryptedString;
                            instructions.Insert(i + 1, OpCodes.Ldc_I4.ToInstruction(key));
                            instructions.Insert(i + 2, OpCodes.Call.ToInstruction(decoderMethod));
                            i += 2;
                        }
                    }

                    methodDef.Body.SimplifyMacros(methodDef.Parameters);
                }
            }
        }
        public static string Ecrypt(Tuple<string, int> values)
        {
            byte[] b = new byte[values.Item1.Length];
            for (int i = 0; i < values.Item1.Length; i++)
                b[i] = (byte)(values.Item1[i] ^ values.Item2);
            return Encoding.UTF8.GetString(b);
        }
    }
}
