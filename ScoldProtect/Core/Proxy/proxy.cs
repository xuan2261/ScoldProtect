using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScoldProtect.Core.Helper;
namespace ScoldProtect.Core.Proxy
{
    class proxy
    {
        // this form https://github.com/XenocodeRCE/BasicProxyObfucator/blob/master/ProxyCalld/Program.cs
        public static List<MethodDef> proxyMethod = new List<MethodDef>();
     public   static void Run(ModuleDefMD mod)
        {
            int intensity = 1;
            for (int o = 0; o < intensity; o++)
            {
                foreach (var t in mod.Types)
                {

                    if (t.IsGlobalModuleType) continue;

                    int mCount = t.Methods.Count;
                    for (int i = 0; i < mCount; i++)
                    {
                        var m = t.Methods[i];

                        if (!m.HasBody) continue;
                        var inst = m.Body.Instructions;

                        for (int z = 0; z < inst.Count; z++)
                        {
                            if (inst[z].OpCode == OpCodes.Call)
                            {

                                try
                                {
                                    MethodDef targetMetod = inst[z].Operand as MethodDef;
                                    if (!targetMetod.FullName.Contains(mod.Assembly.Name))
                                    {
                                        continue;
                                    }
                                    if (targetMetod.Parameters.Count == 0)
                                    {
                                        continue;
                                    }
                                    if (targetMetod.Parameters.Count > 4)
                                    {
                                        continue;
                                    }
                                    MethodDef newMeth = targetMetod.copyMethod(mod);
                                    TypeDef typeOfMethod = targetMetod.DeclaringType;
                                    typeOfMethod.Methods.Add(newMeth);
                                    proxyMethod.Add(newMeth);
                                    Clonesignature(targetMetod, newMeth);
                                    CilBody body = new CilBody();
                                    body.Instructions.Add(OpCodes.Nop.ToInstruction());
                                    for (int x = 0; x < targetMetod.Parameters.Count; x++)
                                    {
                                        //for future references, you will need it
                                        var typeofParam = targetMetod.Parameters[x];

                                        switch (x)
                                        {
                                            case 0:
                                                body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
                                                break;
                                            case 1:
                                                body.Instructions.Add(OpCodes.Ldarg_1.ToInstruction());
                                                break;
                                            case 2:
                                                body.Instructions.Add(OpCodes.Ldarg_2.ToInstruction());
                                                break;
                                            case 3:
                                                body.Instructions.Add(OpCodes.Ldarg_3.ToInstruction());
                                                break;
                                        }
                                    }
                                    body.Instructions.Add(Instruction.Create(OpCodes.Call, newMeth));
                                    body.Instructions.Add(OpCodes.Ret.ToInstruction());

                                    targetMetod.Body = body;

                                }
                                catch (Exception ex)
                                {
                                    continue;
                                }

                            }
                        }

                    }

                }
            }

        }
        public static MethodDef Clonesignature(MethodDef from, MethodDef to)
        {
            to.Attributes = from.Attributes;

            if (from.IsHideBySig)
                to.IsHideBySig = true;

            return to;
        }

    }
}
