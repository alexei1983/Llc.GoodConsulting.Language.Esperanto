
namespace Llc.GoodConsulting.Language.Esperanto
{
    /// <summary>
    /// Represents an Esperanto compound verb form.
    /// </summary>
    public class EoCompoundVerb : EoVerbConjugation
    {
        /// <summary>
        /// Participle aspect.
        /// </summary>
        public required string Aspect { get; set; }
    }
}
