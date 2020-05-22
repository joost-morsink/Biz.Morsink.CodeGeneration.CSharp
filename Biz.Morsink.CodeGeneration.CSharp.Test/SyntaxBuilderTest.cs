using System;
using System.IO;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Biz.Morsink.CodeGeneration.CSharp.SyntaxBuilder;
namespace Biz.Morsink.CodeGeneration.CSharp.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SourceTest()
        {
            var cls = Public().Partial().Class("Program")
                        .Add(Private().Field("string", "_name"),
                            Public().Property("string", "Name").Add(
                                Get().With(Expression("_name")),
                                Private().Set().With(Block(Statement("_name = value;")))),
                            Public().Static().Method("void", "Main").AddParameters(("string[]", "args"))
                                .With(Block()
                                    .Add(TypeName("Console").Call("WriteLine", Literal("Hello world!")))),
                            Public().Override().Method("string", "ToString")
                                .With(Block()
                                    .Add(If(Literal("Joost").Member("Length") == Literal(5),
                                        Literal("Joost").Return(),
                                        Literal("Morsink").Return()))),
                            Public().Method("void", "Test")
                                .With(Block()
                                    .Add(For("var", "i", Literal(0), "i" < Literal(10), "i++",
                                        TypeName("Console").Call("WriteLine", Literal("Hi")))))
                            );

            var unit = CompilationUnit.Using("System")
                .Add(Namespace("Hi").Add(cls));

            Console.WriteLine(unit.Build().NormalizeWhitespace());

            var comp = CSharpCompilation.Create("Test", new[] { unit.Build().SyntaxTree },
                new[] { MetadataReference.CreateFromFile("/usr/local/share/dotnet/sdk/3.1.300/ref/netstandard.dll") },
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                );

            var emitResult = comp.Emit("Test.dll");

            Assert.IsTrue(emitResult.Success);

            var path = Path.Combine(Path.GetDirectoryName(typeof(UnitTest1).Assembly.Location), "Test.dll");
            var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
            var type = asm.GetType("Hi.Program");
            // var x = Activator.CreateInstance(type);
            type.GetMethod("Main")?.Invoke(null, new object?[] { new string[0] });
        }
    }
}
