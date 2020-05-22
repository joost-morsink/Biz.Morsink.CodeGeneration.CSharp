using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace Biz.Morsink.CodeGeneration.CSharp
{
    public static partial class SyntaxBuilder
    {
        public interface IMemberBuilder
        {
            MemberDeclarationSyntax Build();
        }
    }
}
