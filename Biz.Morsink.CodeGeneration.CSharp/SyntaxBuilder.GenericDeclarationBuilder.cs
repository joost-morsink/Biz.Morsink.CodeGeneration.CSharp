using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;


namespace Biz.Morsink.CodeGeneration.CSharp
{
    public static partial class SyntaxBuilder
    {
        public struct GenericDeclarationBuilder
        {
            private readonly Kind _kind;
            private readonly string _name;

            public enum Kind { Invariant, In, Out }
            public static GenericDeclarationBuilder Create(string name) => new GenericDeclarationBuilder(Kind.Invariant, name);
            private GenericDeclarationBuilder(Kind kind, string name)
            {
                _kind = kind;
                _name = name;
            }
            public GenericDeclarationBuilder In()
                => new GenericDeclarationBuilder(Kind.In, _name);
            public GenericDeclarationBuilder Out()
                => new GenericDeclarationBuilder(Kind.Out, _name);
            public TypeParameterSyntax Build()
            {
                var res = SF.TypeParameter(_name);
                if (_kind == Kind.In)
                    res = res.WithVarianceKeyword(SF.Token(SyntaxKind.InKeyword));
                else if (_kind == Kind.Out)
                    res = res.WithVarianceKeyword(SF.Token(SyntaxKind.OutKeyword));
                return res;
            }
            public static implicit operator GenericDeclarationBuilder(string str)
                => Create(str);
        }
    }
}
