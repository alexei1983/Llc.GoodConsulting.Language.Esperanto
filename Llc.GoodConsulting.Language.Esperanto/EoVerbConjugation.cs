
namespace Llc.GoodConsulting.Language.Esperanto
{
    /// <summary>
    /// Represents an Esperanto verb conjugation.
    /// </summary>
    public class EoVerbConjugation
    {
        /// <summary>
        /// Verb tense.
        /// </summary>
        public required string Tense {  get; set; }

        /// <summary>
        /// Pronoun.
        /// </summary>
        public required string Pronoun {  get; set; }

        /// <summary>
        /// Conjugated verb form.
        /// </summary>
        public required string VerbForm { get; set; }

        /// <summary>
        /// Esperanto text convention.
        /// </summary>
        public EoTextConvention TextConvention { get; set; }
    }
}
