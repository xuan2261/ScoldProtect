using dnlib.DotNet;
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
		public static void junkfield(ModuleDefMD module)
		{
			foreach (TypeDef typeDef in module.Types)
			{
				foreach (MethodDef methodDef in typeDef.Methods.ToArray<MethodDef>())
				{
					FieldDefUser fieldDefUser = new FieldDefUser(RandomString(Random.Next(10, 20), Ascii2), new FieldSig(module.CorLibTypes.String), FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Static);
					FieldDefUser fieldDefUser2 = new FieldDefUser(RandomString(Random.Next(10, 20), Ascii2), new FieldSig(module.CorLibTypes.UIntPtr), FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Static);
					FieldDefUser fieldDefUser3 = new FieldDefUser(RandomString(Random.Next(10, 20), Ascii2), new FieldSig(module.CorLibTypes.IntPtr), FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Static);
					FieldDefUser fieldDefUser4 = new FieldDefUser(RandomString(Random.Next(10, 20), Ascii2), new FieldSig(module.CorLibTypes.UInt32), FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Static);
					typeDef.Fields.Add(fieldDefUser);
					typeDef.Fields.Add(fieldDefUser2);
					typeDef.Fields.Add(fieldDefUser3);
					typeDef.Fields.Add(fieldDefUser4);
					if (methodDef.HasBody && methodDef.Body.HasInstructions && !methodDef.Body.HasExceptionHandlers) ;
					{
						if (methodDef.IsVirtual) continue;
						Local local = new Local(module.CorLibTypes.UIntPtr);
						Local local2 = new Local(module.CorLibTypes.UInt32);
						Local local3 = new Local(module.CorLibTypes.IntPtr);
						Local local4 = new Local(module.CorLibTypes.String);
						methodDef.Body.Variables.Add(local);
						methodDef.Body.Variables.Add(local2);
						methodDef.Body.Variables.Add(local3);
						methodDef.Body.Variables.Add(local4);
						for (int i = 0; i < methodDef.Body.Instructions.Count - 2; i++)
						{
							methodDef.Body.Instructions.Insert(i + 1, OpCodes.Ldsfld.ToInstruction(fieldDefUser));
							methodDef.Body.Instructions.Insert(i + 2, OpCodes.Stloc.ToInstruction(local4));
							methodDef.Body.Instructions.Insert(i + 3, OpCodes.Ldsfld.ToInstruction(fieldDefUser2));
							methodDef.Body.Instructions.Insert(i + 4, OpCodes.Stloc.ToInstruction(local));
							methodDef.Body.Instructions.Insert(i + 5, OpCodes.Ldsfld.ToInstruction(fieldDefUser4));
							methodDef.Body.Instructions.Insert(i + 6, OpCodes.Stloc.ToInstruction(local2));
							methodDef.Body.Instructions.Insert(i + 7, OpCodes.Ldsfld.ToInstruction(fieldDefUser3));
							methodDef.Body.Instructions.Insert(i + 8, OpCodes.Stloc.ToInstruction(local3));
							i += 8;
						}
					}
				}
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
