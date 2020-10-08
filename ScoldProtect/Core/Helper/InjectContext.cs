using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoldProtect.Core.Helper
{
    static class extension
    {
        /// <summary>
        ///     Context of the injection process.
        /// </summary>
        class InjectContext : ImportResolver
        {
            /// <summary>
            ///     The mapping of origin definitions to injected definitions.
            /// </summary>
            public readonly Dictionary<IDnlibDef, IDnlibDef> Map = new Dictionary<IDnlibDef, IDnlibDef>();

            /// <summary>
            ///     The module which source type originated from.
            /// </summary>
            public readonly ModuleDef OriginModule;

            /// <summary>
            ///     The module which source type is being injected to.
            /// </summary>
            public readonly ModuleDef TargetModule;

            /// <summary>
            ///     The importer.
            /// </summary>
            readonly Importer importer;

            /// <summary>
            ///     Initializes a new instance of the <see cref="InjectContext" /> class.
            /// </summary>
            /// <param name="module">The origin module.</param>
            /// <param name="target">The target module.</param>
            public InjectContext(ModuleDef module, ModuleDef target)
            {
                OriginModule = module;
                TargetModule = target;
                importer = new Importer(target, ImporterOptions.TryToUseTypeDefs);
                importer.Resolver = this;
            }

            /// <summary>
            ///     Gets the importer.
            /// </summary>
            /// <value>The importer.</value>
            public Importer Importer
            {
                get { return importer; }
            }

            /// <inheritdoc />
            public override TypeDef Resolve(TypeDef typeDef)
            {
                if (Map.ContainsKey(typeDef))
                    return (TypeDef)Map[typeDef];
                return null;
            }

            /// <inheritdoc />
            public override MethodDef Resolve(MethodDef methodDef)
            {
                if (Map.ContainsKey(methodDef))
                    return (MethodDef)Map[methodDef];
                return null;
            }

            /// <inheritdoc />
            public override FieldDef Resolve(FieldDef fieldDef)
            {
                if (Map.ContainsKey(fieldDef))
                    return (FieldDef)Map[fieldDef];
                return null;
            }
        }

        public static MethodDef copyMethod(this MethodDef originMethod, ModuleDefMD mod)
        {
            InjectContext ctx = new InjectContext(mod, mod);

            MethodDefUser newMethodDef = new MethodDefUser
            {
                Signature = ctx.Importer.Import(originMethod.Signature)
            };

            newMethodDef.Name = Guid.NewGuid().ToString().Replace("-", string.Empty);

            newMethodDef.Parameters.UpdateParameterTypes();

            if (originMethod.ImplMap != null)
                newMethodDef.ImplMap = new ImplMapUser(new ModuleRefUser(ctx.TargetModule, originMethod.ImplMap.Module.Name), originMethod.ImplMap.Name, originMethod.ImplMap.Attributes);

            foreach (CustomAttribute ca in originMethod.CustomAttributes)
                newMethodDef.CustomAttributes.Add(new CustomAttribute((ICustomAttributeType)ctx.Importer.Import(ca.Constructor)));

            if (originMethod.HasBody)
            {
                newMethodDef.Body = new CilBody(originMethod.Body.InitLocals, new List<Instruction>(), new List<ExceptionHandler>(), new List<Local>());
                newMethodDef.Body.MaxStack = originMethod.Body.MaxStack;

                var bodyMap = new Dictionary<object, object>();

                foreach (Local local in originMethod.Body.Variables)
                {
                    var newLocal = new Local(ctx.Importer.Import(local.Type));
                    newMethodDef.Body.Variables.Add(newLocal);
                    newLocal.Name = local.Name;

                    bodyMap[local] = newLocal;
                }

                foreach (Instruction instr in originMethod.Body.Instructions)
                {
                    var newInstr = new Instruction(instr.OpCode, instr.Operand);
                    newInstr.SequencePoint = instr.SequencePoint;

                    if (newInstr.Operand is IType)
                        newInstr.Operand = ctx.Importer.Import((IType)newInstr.Operand);

                    else if (newInstr.Operand is IMethod)
                        newInstr.Operand = ctx.Importer.Import((IMethod)newInstr.Operand);

                    else if (newInstr.Operand is IField)
                        newInstr.Operand = ctx.Importer.Import((IField)newInstr.Operand);

                    newMethodDef.Body.Instructions.Add(newInstr);
                    bodyMap[instr] = newInstr;
                }

                foreach (Instruction instr in newMethodDef.Body.Instructions)
                {
                    if (instr.Operand != null && bodyMap.ContainsKey(instr.Operand))
                        instr.Operand = bodyMap[instr.Operand];

                    else if (instr.Operand is Instruction[])
                        instr.Operand = ((Instruction[])instr.Operand).Select(target => (Instruction)bodyMap[target]).ToArray();
                }

                foreach (ExceptionHandler eh in originMethod.Body.ExceptionHandlers)
                    newMethodDef.Body.ExceptionHandlers.Add(new ExceptionHandler(eh.HandlerType)
                    {
                        CatchType = eh.CatchType == null ? null : (ITypeDefOrRef)ctx.Importer.Import(eh.CatchType),
                        TryStart = (Instruction)bodyMap[eh.TryStart],
                        TryEnd = (Instruction)bodyMap[eh.TryEnd],
                        HandlerStart = (Instruction)bodyMap[eh.HandlerStart],
                        HandlerEnd = (Instruction)bodyMap[eh.HandlerEnd],
                        FilterStart = eh.FilterStart == null ? null : (Instruction)bodyMap[eh.FilterStart]
                    });

                newMethodDef.Body.SimplifyMacros(newMethodDef.Parameters);
            }

            return newMethodDef;
        }
    }
}
