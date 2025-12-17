
namespace Llc.GoodConsulting.Language.Esperanto
{
    #region EoVerbTense

    /// <summary>
    /// Esperanto verb tense/mood.
    /// </summary>
    [Flags]
    public enum EoVerbTense
    {
        /// <summary>
        /// Present tense.
        /// </summary>
        Present = 1,

        /// <summary>
        /// Past tense.
        /// </summary>
        Past = 2,

        /// <summary>
        /// Future tense.
        /// </summary>
        Future = 4,

        /// <summary>
        /// Conditional mood.
        /// </summary>
        Conditional = 8,

        /// <summary>
        /// Imperative mood.
        /// </summary>
        Imperative = 16
    }

    #endregion

    #region EoParticipleAspect

    /// <summary>
    /// Esperanto participles (active/passive, past/present/future).
    /// </summary>
    [Flags]
    public enum EoParticipleAspect
    {
        /// <summary>
        /// Present-active aspect (ends in -anta).
        /// </summary>
        PresentActive = EoVerbTense.Present | EoVoice.Active,

        /// <summary>
        /// Past-active aspect (ends in -inta)
        /// </summary>
        PastActive = EoVerbTense.Past | EoVoice.Active,

        /// <summary>
        /// Future-active aspect (ends in -onta)
        /// </summary>
        FutureActive = EoVerbTense.Future | EoVoice.Active,

        /// <summary>
        /// Present-passive aspect (ends in -ata)
        /// </summary>
        PresentPassive = EoVerbTense.Present | EoVoice.Passive,

        /// <summary>
        /// Past-passive aspect (ends in -ita)
        /// </summary>
        PastPassive = EoVerbTense.Past | EoVoice.Passive,

        /// <summary>
        /// Future-passive aspect (ends in -ota)
        /// </summary>
        FuturePassive = EoVerbTense.Future | EoVoice.Passive
    }

    #endregion

    #region EoVoice

    /// <summary>
    /// Esperanto voice.
    /// </summary>
    [Flags]
    public enum EoVoice
    {
        /// <summary>
        /// Active voice.
        /// </summary>
        Active = 128,

        /// <summary>
        /// Passive voice.
        /// </summary>
        Passive = 256
    }

    #endregion

    #region EoTextConvention

    /// <summary>
    /// Esperanto text convention.
    /// </summary>
    public enum EoTextConvention
    {
        /// <summary>
        /// Convention is not known.
        /// </summary>
        Unknown,

        /// <summary>
        /// Multiple/mixed conventions.
        /// </summary>
        Mixed,

        /// <summary>
        /// X-convention.
        /// </summary>
        X,

        /// <summary>
        /// H-convention.
        /// </summary>
        H,

        /// <summary>
        /// LaTex convention.
        /// </summary>
        LaTex,

        /// <summary>
        /// HTML entities.
        /// </summary>
        HtmlEntities,

        /// <summary>
        /// UTF-8 characters.
        /// </summary>
        Utf8,
    }

    #endregion
}
