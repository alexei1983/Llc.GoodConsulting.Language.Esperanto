
namespace Llc.GoodConsulting.Language.Esperanto
{
    /// <summary>
    /// Represents an Esperanto participle.
    /// </summary>
    public class EoParticiple
    {
        /// <summary>
        /// Participle aspect.
        /// </summary>
        public required string Aspect {  get; set; }

        /// <summary>
        /// Participle.
        /// </summary>
        public required string Participle { get; set; }

        /// <summary>
        /// Esperanto text convention.
        /// </summary>
        public EoTextConvention TextConvention { get; set; }
    }
}
