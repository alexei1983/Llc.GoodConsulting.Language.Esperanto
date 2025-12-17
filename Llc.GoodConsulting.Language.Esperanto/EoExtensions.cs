
namespace Llc.GoodConsulting.Language.Esperanto.Extensions
{
    /// <summary>
    /// Extension methods for working with Esperanto text and conjugating Esperanto verbs.
    /// </summary>
    public static class EoExtensions
    {
        /// <summary>
        /// Esperanto pronoun 'mi'
        /// </summary>
        const string mi = nameof(mi);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string? ToUtf8Mixed(this string? value)
        {
            return EoTextConverter.Convert(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infinitive"></param>
        /// <param name="tense"></param>
        /// <returns></returns>
        public static string ConjugateVerb(this string? infinitive, EoVerbTense tense)
        {
            if (string.IsNullOrWhiteSpace(infinitive))
                return string.Empty;
            return EoConjugator.Conjugate(infinitive, tense);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infinitive"></param>
        /// <param name="tense"></param>
        /// <param name="aspect"></param>
        /// <param name="pronoun"></param>
        /// <returns></returns>
        public static string ConjugateCompoundVerb(this string? infinitive, string? pronoun, EoVerbTense tense, EoParticipleAspect aspect)
        {
            if (string.IsNullOrWhiteSpace(infinitive))
                return string.Empty;
            if (string.IsNullOrEmpty(pronoun))
                pronoun = mi;
            return EoConjugator.ConjugateCompound(pronoun, infinitive, tense, aspect);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infinitive"></param>
        /// <param name="tense"></param>
        /// <param name="aspect"></param>
        /// <returns></returns>
        public static string ConjugateCompoundVerb(this string? infinitive, EoVerbTense tense, EoParticipleAspect aspect)
        {
            return infinitive.ConjugateCompoundVerb(null, tense, aspect);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="inputConvention"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string? ToUtf8(this string? text, EoTextConvention inputConvention = default)
        {
            return EoTextConverter.ToUtf8(text, inputConvention);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="inputConvention"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string? ToX(this string? text, EoTextConvention inputConvention = default)
        {
            return EoTextConverter.ToX(text, inputConvention);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="inputConvention"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string? ToHtmlEntities(this string? text, EoTextConvention inputConvention = default)
        {
            return EoTextConverter.ToHtmlEntities(text, inputConvention);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="inputConvention"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string? ToH(this string? text, EoTextConvention inputConvention = default)
        {
            return EoTextConverter.ToH(text, inputConvention);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="inputConvention"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string? ToLaTex(this string? text, EoTextConvention inputConvention = default)
        {
            return EoTextConverter.ToLaTex(text, inputConvention);
        }
    }
}