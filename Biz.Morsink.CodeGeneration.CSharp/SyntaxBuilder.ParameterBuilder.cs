using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;


namespace Biz.Morsink.CodeGeneration.CSharp
{
    public static partial class SyntaxBuilder
    {
        public struct ParameterBuilder
        {
            private readonly string? _type;
            private readonly string _name;

            private ParameterBuilder(string? type, string name)
            {
                _type = type;
                _name = name;
            }
            public static ParameterBuilder Create(string type, string name)
                => new ParameterBuilder(type, name);
            public ParameterSyntax Build()
                => _type != null
                     ? SF.Parameter(SF.List<AttributeListSyntax>(), SF.TokenList(), ParseType(_type), SF.ParseToken(_name), null)
                     : SF.Parameter(SF.List<AttributeListSyntax>(), SF.TokenList(), null, SF.ParseToken(_name), null);
            public static implicit operator ParameterBuilder((string, string) t)
                => Create(t.Item1, t.Item2);
        }
    }
}
