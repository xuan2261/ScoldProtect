using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoldProtect.Core.Flow
{
    class ControlFlow
    {

        public static void Execute(ModuleDefMD Module)
        {
                for (int x = 0; x < Module.Types.Count; x++)
                {
                    var tDef = Module.Types[x];
                    if (tDef != Module.GlobalType)
                        for (int i = 0; i < tDef.Methods.Count; i++)
                        {
                            var mDef = tDef.Methods[i];
                            if (!mDef.Name.StartsWith("get_") && !mDef.Name.StartsWith("set_"))
                            {
                                if (!mDef.HasBody || mDef.IsConstructor) continue;
                                mDef.Body.SimplifyBranches();
                                ExecuteMethod(mDef, Module);
                            ExecuteMethod2(mDef, Module);
                            ExecuteMethod3(mDef, Module);
                            ExecuteMethod4(mDef, Module);
                            //ExecuteMethod6(mDef, Module);
                        }
                        }
                }
        }
        public static void ExecuteMethod4(MethodDef method, ModuleDefMD Module)
        {

            method.Body.SimplifyMacros(method.Parameters);
            List<Block> blocks = Blocks.Block(method);
            blocks = Randomize(blocks);
            method.Body.Instructions.Clear();
            Local local = new Local(Module.CorLibTypes.UInt32);
            method.Body.Variables.Add(local);
            Instruction target = Instruction.Create(OpCodes.Nop);
            Instruction instr = Instruction.Create(OpCodes.Br, target);
            foreach (Instruction instruction in Calc(0))
                method.Body.Instructions.Add(instruction);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Br, instr));
            method.Body.Instructions.Add(target);
            foreach (var block in blocks.Where(block => block != blocks.Single(x => x.Number == blocks.Count - 1)))
            {

                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, local));
                foreach (Instruction instruction in Calc(block.Number))
                    method.Body.Instructions.Add(instruction);
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ceq));
                Instruction instruction4 = Instruction.Create(OpCodes.Nop);
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instruction4));
                foreach (Instruction instruction in block.Instructions)
                    method.Body.Instructions.Add(instruction);
                foreach (Instruction instruction in Calc(block.Number + 1))
                    method.Body.Instructions.Add(instruction);

                method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local));
                method.Body.Instructions.Add(instruction4);
            }
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, local));
            foreach (Instruction instruction in Calc(blocks.Count - 1))
                method.Body.Instructions.Add(instruction);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ceq));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instr));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Br, blocks.Single(x => x.Number == blocks.Count - 1).Instructions[0]));
            method.Body.Instructions.Add(instr);
            foreach (Instruction lastBlock in blocks.Single(x => x.Number == blocks.Count - 1).Instructions)
                method.Body.Instructions.Add(lastBlock);
        }
        public static void ExecuteMethod5(MethodDef method, ModuleDefMD Module)
        {

            method.Body.SimplifyMacros(method.Parameters);
            List<Block> blocks = Blocks.Block(method);
            blocks = Randomize(blocks);
            method.Body.Instructions.Clear();
            Local local = new Local(Module.CorLibTypes.UInt64);
            method.Body.Variables.Add(local);
            Instruction target = Instruction.Create(OpCodes.Nop);
            Instruction instr = Instruction.Create(OpCodes.Br, target);
            foreach (Instruction instruction in Calc(0))
                method.Body.Instructions.Add(instruction);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Br, instr));
            method.Body.Instructions.Add(target);
            foreach (var block in blocks.Where(block => block != blocks.Single(x => x.Number == blocks.Count - 1)))
            {

                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, local));
                foreach (Instruction instruction in Calc(block.Number))
                    method.Body.Instructions.Add(instruction);
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ceq));
                Instruction instruction4 = Instruction.Create(OpCodes.Nop);
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instruction4));
                foreach (Instruction instruction in block.Instructions)
                    method.Body.Instructions.Add(instruction);
                foreach (Instruction instruction in Calc(block.Number + 1))
                    method.Body.Instructions.Add(instruction);

                method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local));
                method.Body.Instructions.Add(instruction4);
            }
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, local));
            foreach (Instruction instruction in Calc(blocks.Count - 1))
                method.Body.Instructions.Add(instruction);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ceq));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instr));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Br, blocks.Single(x => x.Number == blocks.Count - 1).Instructions[0]));
            method.Body.Instructions.Add(instr);
            foreach (Instruction lastBlock in blocks.Single(x => x.Number == blocks.Count - 1).Instructions)
                method.Body.Instructions.Add(lastBlock);
        }
        public static void ExecuteMethod6(MethodDef method, ModuleDefMD Module)
        {

            method.Body.SimplifyMacros(method.Parameters);
            List<Block> blocks = Blocks.Block(method);
            blocks = Randomize(blocks);
            method.Body.Instructions.Clear();
            Local local = new Local(Module.CorLibTypes.Int16);
            method.Body.Variables.Add(local);
            Instruction target = Instruction.Create(OpCodes.Nop);
            Instruction instr = Instruction.Create(OpCodes.Br, target);
            foreach (Instruction instruction in Calc(0))
                method.Body.Instructions.Add(instruction);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Br, instr));
            method.Body.Instructions.Add(target);
            foreach (var block in blocks.Where(block => block != blocks.Single(x => x.Number == blocks.Count - 1)))
            {

                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, local));
                foreach (Instruction instruction in Calc(block.Number))
                    method.Body.Instructions.Add(instruction);
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ceq));
                Instruction instruction4 = Instruction.Create(OpCodes.Nop);
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instruction4));
                foreach (Instruction instruction in block.Instructions)
                    method.Body.Instructions.Add(instruction);
                foreach (Instruction instruction in Calc(block.Number + 1))
                    method.Body.Instructions.Add(instruction);

                method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local));
                method.Body.Instructions.Add(instruction4);
            }
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, local));
            foreach (Instruction instruction in Calc(blocks.Count - 1))
                method.Body.Instructions.Add(instruction);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ceq));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instr));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Br, blocks.Single(x => x.Number == blocks.Count - 1).Instructions[0]));
            method.Body.Instructions.Add(instr);
            foreach (Instruction lastBlock in blocks.Single(x => x.Number == blocks.Count - 1).Instructions)
                method.Body.Instructions.Add(lastBlock);
        }
        public static void ExecuteMethod3(MethodDef method, ModuleDefMD Module)
        {

            method.Body.SimplifyMacros(method.Parameters);
            List<Block> blocks = Blocks.Block(method);
            blocks = Randomize(blocks);
            method.Body.Instructions.Clear();
            Local local = new Local(Module.CorLibTypes.UInt16);
            method.Body.Variables.Add(local);
            Instruction target = Instruction.Create(OpCodes.Nop);
            Instruction instr = Instruction.Create(OpCodes.Br, target);
            foreach (Instruction instruction in Calc(0))
                method.Body.Instructions.Add(instruction);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Br, instr));
            method.Body.Instructions.Add(target);
            foreach (var block in blocks.Where(block => block != blocks.Single(x => x.Number == blocks.Count - 1)))
            {

                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, local));
                foreach (Instruction instruction in Calc(block.Number))
                    method.Body.Instructions.Add(instruction);
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ceq));
                Instruction instruction4 = Instruction.Create(OpCodes.Nop);
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instruction4));
                foreach (Instruction instruction in block.Instructions)
                    method.Body.Instructions.Add(instruction);
                foreach (Instruction instruction in Calc(block.Number + 1))
                    method.Body.Instructions.Add(instruction);

                method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local));
                method.Body.Instructions.Add(instruction4);
            }
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, local));
            foreach (Instruction instruction in Calc(blocks.Count - 1))
                method.Body.Instructions.Add(instruction);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ceq));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instr));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Br, blocks.Single(x => x.Number == blocks.Count - 1).Instructions[0]));
            method.Body.Instructions.Add(instr);
            foreach (Instruction lastBlock in blocks.Single(x => x.Number == blocks.Count - 1).Instructions)
                method.Body.Instructions.Add(lastBlock);
        }
        public static void ExecuteMethod2(MethodDef method, ModuleDefMD Module)
        {

            method.Body.SimplifyMacros(method.Parameters);
            List<Block> blocks = Blocks.Block(method);
            blocks = Randomize(blocks);
            method.Body.Instructions.Clear();
            Local local = new Local(Module.CorLibTypes.IntPtr);
            method.Body.Variables.Add(local);
            Instruction target = Instruction.Create(OpCodes.Nop);
            Instruction instr = Instruction.Create(OpCodes.Br, target);
            foreach (Instruction instruction in Calc(0))
                method.Body.Instructions.Add(instruction);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Br, instr));
            method.Body.Instructions.Add(target);
            foreach (var block in blocks.Where(block => block != blocks.Single(x => x.Number == blocks.Count - 1)))
            {

                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, local));
                foreach (Instruction instruction in Calc(block.Number))
                    method.Body.Instructions.Add(instruction);
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ceq));
                Instruction instruction4 = Instruction.Create(OpCodes.Nop);
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instruction4));
                foreach (Instruction instruction in block.Instructions)
                    method.Body.Instructions.Add(instruction);
                foreach (Instruction instruction in Calc(block.Number + 1))
                    method.Body.Instructions.Add(instruction);

                method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local));
                method.Body.Instructions.Add(instruction4);
            }
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, local));
            foreach (Instruction instruction in Calc(blocks.Count - 1))
                method.Body.Instructions.Add(instruction);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ceq));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instr));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Br, blocks.Single(x => x.Number == blocks.Count - 1).Instructions[0]));
            method.Body.Instructions.Add(instr);
            foreach (Instruction lastBlock in blocks.Single(x => x.Number == blocks.Count - 1).Instructions)
                method.Body.Instructions.Add(lastBlock);
        }
        public static void ExecuteMethod(MethodDef method , ModuleDefMD Module)
        {

            method.Body.SimplifyMacros(method.Parameters);
            List<Block> blocks = Blocks.Block(method);
            blocks = Randomize(blocks);
            method.Body.Instructions.Clear();
            Local local = new Local(Module.CorLibTypes.UIntPtr);
            method.Body.Variables.Add(local);
            Instruction target = Instruction.Create(OpCodes.Nop);
            Instruction instr = Instruction.Create(OpCodes.Br, target);
            foreach (Instruction instruction in Calc(0))
                method.Body.Instructions.Add(instruction);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Br, instr));
            method.Body.Instructions.Add(target);
            foreach (var block in blocks.Where(block => block != blocks.Single(x => x.Number == blocks.Count - 1)))
            {

                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, local));
                foreach (Instruction instruction in Calc(block.Number))
                        method.Body.Instructions.Add(instruction);
                    method.Body.Instructions.Add(Instruction.Create(OpCodes.Ceq));
                    Instruction instruction4 = Instruction.Create(OpCodes.Nop);
                    method.Body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instruction4));
                    foreach (Instruction instruction in block.Instructions)
                        method.Body.Instructions.Add(instruction);
                    foreach (Instruction instruction in Calc(block.Number + 1))
                        method.Body.Instructions.Add(instruction);

                    method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local));
                method.Body.Instructions.Add(instruction4);
                }
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, local));
            foreach (Instruction instruction in Calc(blocks.Count - 1))
                method.Body.Instructions.Add(instruction);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ceq));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instr));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Br, blocks.Single(x => x.Number == blocks.Count - 1).Instructions[0]));
            method.Body.Instructions.Add(instr);
            foreach (Instruction lastBlock in blocks.Single(x => x.Number == blocks.Count - 1).Instructions)
                method.Body.Instructions.Add(lastBlock);
        }
        public static Random Random = new Random();
        public static List<Block> Randomize(List<Block> input)
        {
            List<Block> ret = new List<Block>();
            foreach (var group in input)
                ret.Insert(Random.Next(0, ret.Count), group);
            return ret;
        }


        public static List<Instruction> Calc(int value)
        {
            List<Instruction> instructions = new List<Instruction>();
            instructions.Add(Instruction.Create(OpCodes.Ldc_I4, value));
            return instructions;
        }

        public static void AddJump(IList<Instruction> instrs, Instruction target)
        {
            instrs.Add(Instruction.Create(OpCodes.Br, target));
        }
    }
}
