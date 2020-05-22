using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;


namespace Biz.Morsink.CodeGeneration.CSharp
{
    public static partial class SyntaxBuilder
    {
        public struct CompilationUnitBuilder
        {
            private readonly ImmutableList<UsingDirectiveSyntax> _usings;
            private readonly ImmutableList<NamespaceBuilder> _namespaces;

            private CompilationUnitBuilder(ImmutableList<UsingDirectiveSyntax> usings, ImmutableList<NamespaceBuilder> namespaces)
            {
                _usings = usings;
                _namespaces = namespaces;
            }
            public static CompilationUnitBuilder Empty => new CompilationUnitBuilder(ImmutableList<UsingDirectiveSyntax>.Empty, ImmutableList<NamespaceBuilder>.Empty);
            public CompilationUnitBuilder Add(params NamespaceBuilder[] namespaces)
                => new CompilationUnitBuilder(_usings, _namespaces.AddRange(namespaces));
            public CompilationUnitBuilder Using(params string[] usings)
                => new CompilationUnitBuilder(_usings.AddRange(usings.Select(u => SF.UsingDirective(SF.ParseName(u)))), _namespaces);
            public CompilationUnitSyntax Build()
                => SF.CompilationUnit().AddUsings(_usings.ToArray())
                   .AddMembers(_namespaces.Select(ns => ns.Build()).ToArray());
        }
    }
}
