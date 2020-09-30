using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoldProtect.Core.SizeOFF
{
    class SizeOFF
    {
        public static void Run(ModuleDefMD module)
        {
            foreach (TypeDef type in module.GetTypes())
            {
                if (type.IsGlobalModuleType) continue;
                foreach (MethodDef methodDef in type.Methods)
                {
                    if (!methodDef.HasBody) continue;
                    var instructions = methodDef.Body.Instructions;
                    for (int i = 0; i < instructions.Count; i++)
                    {
                        if (instructions[i].IsLdcI4() && IsSafe(instructions.ToList(), i))
                        {
                            instructions.Insert(i + 1, Instruction.Create(OpCodes.Sizeof, methodDef.Module.Import(typeof(bool))));
                            instructions.Insert(i + 2, Instruction.Create(OpCodes.Add));
                            i += 2;
                        }
                    }
                }
            }
        }
        private static bool IsSafe(List<Instruction> instructions, int i)
        {
            if (new[] { -2, -1, 0, 1, 2 }.Contains(instructions[i].GetLdcI4Value()))
                return false;
            return true;
        }
    }
}
