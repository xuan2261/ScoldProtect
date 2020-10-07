﻿using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ScoldProtect.Core.Junk
{
    class Junk
    {
        public static void Run(ModuleDefMD module)
        {
            for (int i = 0; i < 150; i++)
            {
                var junkattribute = new TypeDefUser("ScoldProtect" + RandomString(Random.Next(10, 20), Ascii), module.CorLibTypes.Object.TypeDefOrRef);
				MethodDef entryPoint = new MethodDefUser(RandomString(Random.Next(10, 20), Ascii2),
					MethodSig.CreateStatic(module.CorLibTypes.Int32, new SZArraySig(module.CorLibTypes.UIntPtr)));
				entryPoint.Attributes = MethodAttributes.Private | MethodAttributes.Static |
								MethodAttributes.HideBySig | MethodAttributes.ReuseSlot;
				entryPoint.ImplAttributes = MethodImplAttributes.IL | MethodImplAttributes.Managed;
				entryPoint.ParamDefs.Add(new ParamDefUser(RandomString(Random.Next(10, 20), Ascii2), 1));
				junkattribute.Methods.Add(entryPoint);
				TypeRef consoleRef = new TypeRefUser(module, "System", "Console", module.CorLibTypes.AssemblyRef);
				MemberRef consoleWrite1 = new MemberRefUser(module, "WriteLine",
							MethodSig.CreateStatic(module.CorLibTypes.Void, module.CorLibTypes.String),
							consoleRef);
				CilBody epBody = new CilBody();
				entryPoint.Body = epBody;
				epBody.Instructions.Add(OpCodes.Ldstr.ToInstruction(RandomString(Random.Next(10, 20), Ascii2)));
				epBody.Instructions.Add(OpCodes.Call.ToInstruction(consoleWrite1));
				epBody.Instructions.Add(OpCodes.Ldc_I4_0.ToInstruction());
				epBody.Instructions.Add(OpCodes.Ret.ToInstruction());
				module.Types.Add(junkattribute);
            }
        }
        public static Random Random = new Random();
        public static string Ascii = "1234567890";
		public static string Ascii2 = "mnbvcxzlkjhgfdsapoiuytrewq0987654321";
		private static string RandomString(int length, string chars)
        {
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}
