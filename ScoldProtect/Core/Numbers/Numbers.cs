using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoldProtect.Core.Numbers
{
    class Numbers
    {
		public static void Run(ModuleDefMD md)
		{
			foreach (TypeDef typeDef in md.Types)
			{
				foreach (MethodDef methodDef in typeDef.Methods)
				{
					bool flag = !methodDef.HasBody;
					if (!flag)
					{
						for (int i = 0; i < methodDef.Body.Instructions.Count; i++)
						{
							bool flag2 = methodDef.Body.Instructions[i].IsLdcI4();
							if (flag2)
							{
								int ldcI4Value = methodDef.Body.Instructions[i].GetLdcI4Value();
								bool flag3 = ldcI4Value <= 0;
								if (!flag3)
								{
									methodDef.Body.Instructions[i].OpCode = OpCodes.Ldstr;
									methodDef.Body.Instructions[i].Operand = Random2(ldcI4Value);
									methodDef.Body.Instructions.Insert(i + 1, OpCodes.Call.ToInstruction(md.Import(typeof(string).GetMethod("get_Length"))));
								}
							}
						}
					}
				}
			}
		}
		public static Random Random = new Random();
		public static string Random2(int length)
		{
			return new string((from s in Enumerable.Repeat<string>("難金女月弓日尸木火土十大中手田水口廿卜山戈人心", length)
							   select s[Random.Next(s.Length)]).ToArray<char>());
		}
	}
}
