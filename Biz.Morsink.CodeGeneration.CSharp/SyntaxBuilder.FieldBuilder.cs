using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;


namespace Biz.Morsink.CodeGeneration.CSharp
{
    public static partial class SyntaxBuilder
    {
        public struct FieldBuilder : IMemberBuilder
        {
            private readonly ModifierBuilder _modifiers;
            private readonly string _type;
            private readonly string _name;

            public FieldBuilder(ModifierBuilder modifiers, string type, string name)
            {
                _modifiers = modifiers;
                _type = type;
                _name = name;
            }

            public static FieldBuilder Create(ModifierBuilder modifiers, string type, string name)
                => new FieldBuilder(modifiers, type, name);

            public FieldDeclarationSyntax Build()
                => SF.FieldDeclaration(SF.VariableDeclaration(ParseType(_type), SF.SingletonSeparatedList(SF.VariableDeclarator(_name))))
                    .AddModifiers(_modifiers.Build().ToArray());

            MemberDeclarationSyntax IMemberBuilder.Build()
                => Build();
        }
    }
}
