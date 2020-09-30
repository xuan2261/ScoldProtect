using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoldProtect.Core.Rename
{
    class Rename
    {
        public static void Run(ModuleDef module)
        {
            module.Name = "ScoldProtect";
            foreach (TypeDef type in module.Types)
            {
                type.Name = RandomString(Random.Next(90, 120), Ascii);
                type.Namespace = RandomString(Random.Next(90, 120), Ascii);
                if (type.IsGlobalModuleType || type.IsRuntimeSpecialName || type.IsSpecialName || type.IsWindowsRuntime || type.IsInterface)
                {
                    continue;
                }
                foreach (PropertyDef property in type.Properties)
                {
                    if (property.IsRuntimeSpecialName) continue;
                    property.Name = RandomString(Random.Next(90, 120), Ascii);
                }
                foreach (var field in type.Fields)
                {
                    field.Name = RandomString(Random.Next(90, 120), Ascii);
                }
                foreach (EventDef eventdef in type.Events)
                {
                    eventdef.Name = RandomString(Random.Next(90, 120), Ascii);
                }
                foreach (MethodDef method in type.Methods)
                {
                    if (method.IsConstructor || method.IsRuntimeSpecialName || method.IsRuntime || method.IsStaticConstructor || method.IsVirtual) continue;
                    method.Name = RandomString(Random.Next(90, 120), Ascii);
                }
            }
        }
        public static Random Random = new Random();
        public static string Ascii = "zxcvbnmasdfghjklqwertyuiop1234567890";
        private static string RandomString(int length, string chars)
        {
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}
