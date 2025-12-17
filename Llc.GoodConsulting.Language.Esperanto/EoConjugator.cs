using Llc.GoodConsulting.Util.Extensions;

namespace Llc.GoodConsulting.Language.Esperanto
{
    /// <summary>
    /// Esperanto verb conjugator that supports simple, compound, and passive constructions.
    /// </summary>
    public static class EoConjugator
    {
        /// <summary>
        /// Esperanto verb endings.
        /// </summary>
        static readonly Dictionary<EoVerbTense, string> VerbEndings = new()
        {
            [EoVerbTense.Present] = "as",
            [EoVerbTense.Past] = "is",
            [EoVerbTense.Future] = "os",
            [EoVerbTense.Conditional] = "us",
            [EoVerbTense.Imperative] = "u"
        };

        /// <summary>
        /// Esperanto participle endings.
        /// </summary>
        static readonly Dictionary<EoParticipleAspect, string> ParticipleEndings = new()
        {
            [EoParticipleAspect.PresentActive] = "anta",
            [EoParticipleAspect.PastActive] = "inta",
            [EoParticipleAspect.FutureActive] = "onta",
            [EoParticipleAspect.PresentPassive] = "ata",
            [EoParticipleAspect.PastPassive] = "ita",
            [EoParticipleAspect.FuturePassive] = "ota"
        };

        /// <summary>
        /// Esperanto pronouns.
        /// </summary>
        static readonly string[] Pronouns =
        [
            "mi", "vi", "li", "ŝi", "ĝi", "ni", "ili", "oni"
        ];

        /// <summary>
        /// Infinitive form of 'esti'
        /// </summary>
        const string esti = nameof(esti);

        /// <summary>
        /// 'de'
        /// </summary>
        const string de = nameof(de);

        /// <summary>
        /// 'La libro'
        /// </summary>
        const string LaLibro = "La libro";

        /// <summary>
        /// Generates the Esperanto verb root given the infinitive form of the verb.
        /// </summary>
        /// <param name="infinitive">The verb infinitive (e.g. "esti", "paroli").</param>
        /// <exception cref="ArgumentException"></exception>
        static string GetRoot(string infinitive)
        {
            if (string.IsNullOrWhiteSpace(infinitive))
                throw new ArgumentException("Verb cannot be null or empty.", nameof(infinitive));

            infinitive = infinitive.Trim().ToLowerInvariant();
            if (!infinitive.EndsWith('i'))
                throw new ArgumentException("Esperanto infinitives must end with 'i'", nameof(infinitive));

            return infinitive[..^1];
        }

        /// <summary>
        /// Determines whether or not the specified value is a valid Esperanto pronoun.
        /// </summary>
        /// <param name="pronoun">Value to check.</param>
        public static bool IsPronoun(string pronoun)
        {
            return !string.IsNullOrEmpty(GetPronoun(pronoun));
        }

        /// <summary>
        /// Retrieves the specified Esperanto pronoun in proper UTF-8 encoding.
        /// </summary>
        /// <param name="pronoun">Esperanto pronoun to retrieve.</param>
        public static string? GetPronoun(string pronoun)
        {
            pronoun = (pronoun ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(pronoun))
                return null;
            pronoun = EoTextConverter.Convert(pronoun) ?? string.Empty;
            return Pronouns.FirstOrDefault(p => string.Equals(p, pronoun, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Retrieves the specified Esperanto pronoun with the specified text convention.
        /// </summary>
        /// <param name="pronoun">Esperanto pronoun to retrieve.</param>
        /// <param name="outputConvention">The Esperanto text convention to utilize for the output.</param>
        public static string? GetPronoun(string pronoun, EoTextConvention outputConvention)
        {
            return EoTextConverter.Convert(GetPronoun(pronoun), EoTextConvention.Utf8, outputConvention);
        }

        /// <summary>
        /// Retrieves a list of Esperanto pronouns.
        /// </summary>
        public static IEnumerable<string> GetPronouns()
        {
            foreach (var pronoun in Pronouns)
                yield return pronoun;
        }

        /// <summary>
        /// Retrieves a list of Esperanto pronouns with the specified text convention.
        /// </summary>
        /// <param name="outputConvention">The Esperanto text convention to utilize for the output.</param>
        public static IEnumerable<string> GetPronouns(EoTextConvention outputConvention)
        {
            foreach (var pronoun in GetPronouns())
                yield return EoTextConverter.Convert(pronoun, EoTextConvention.Utf8, outputConvention)!;
        }

        /// <summary>
        /// Simple synthetic conjugation: root + -as/-is/-os/-us/-u
        /// </summary>
        /// <param name="infinitive">The verb infinitive (e.g. "esti", "paroli").</param>
        /// <param name="tense">The verb tense.</param>
        public static string Conjugate(string infinitive, EoVerbTense tense)
        {
            if (string.IsNullOrWhiteSpace(infinitive))
                throw new ArgumentException("Verb infinitive is required.", nameof(infinitive));

            string root = GetRoot(infinitive);
            return root + VerbEndings[tense];
        }

        /// <summary>
        /// Determines whether or not the specified text appears to be an Esperanto infinitive.
        /// </summary>
        /// <param name="verbText">The Esperanto verb text to examine.</param>
        public static bool IsInfinitive(string verbText)
        {
            try
            {
                GetRoot(verbText);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Builds one of the six Esperanto participle forms.
        /// </summary>
        /// <param name="infinitive">The verb infinitive (e.g. "esti", "paroli").</param>
        /// <param name="aspect">The participle aspect.</param>
        public static string MakeParticiple(string infinitive, EoParticipleAspect aspect)
        {
            if (string.IsNullOrWhiteSpace(infinitive))
                throw new ArgumentException("Verb infinitive is required.", nameof(infinitive));

            string root = GetRoot(infinitive);
            return root + ParticipleEndings[aspect];
        }

        /// <summary>
        /// Builds a compound tense: 'esti' conjugated in the specified tense + participle.
        /// e.g. "mi estas vidinta", "ni estos legantaj"
        /// </summary>
        /// <param name="pronoun">The Esperanto pronoun.</param>
        /// <param name="infinitive">The verb infinitive (e.g. "esti", "paroli").</param>
        /// <param name="estiTense">The verb tense of 'esti'</param>
        /// <param name="aspect">The participle aspect.</param>
        public static string ConjugateCompound(string pronoun, string infinitive, EoVerbTense estiTense, EoParticipleAspect aspect)
        {
            if (string.IsNullOrWhiteSpace(infinitive))
                throw new ArgumentException("Verb infinitive is required.", nameof(infinitive));

            if (string.IsNullOrWhiteSpace(pronoun))
                throw new ArgumentException("Pronoun is required.", nameof(pronoun));

            if (!IsPronoun(pronoun))
                throw new ArgumentException($"Invalid Esperanto pronoun: {pronoun}", nameof(pronoun));

            string estiConj = Conjugate(esti, estiTense);
            string part = MakeParticiple(infinitive, aspect);
            return $"{pronoun} {estiConj} {part}";
        }

        /// <summary>
        /// Builds a passive voice clause, e.g. "la libro estis legita (de mi)"
        /// </summary>
        /// <param name="subjectNoun">The subject noun.</param>
        /// <param name="infinitive">The verb infinitive (e.g. "esti", "paroli").</param>
        /// <param name="estiTense">The verb tense of 'esti'</param>
        /// <param name="passiveAspect">The participle aspect.</param>
        /// <param name="agent">Optional agent.</param>
        public static string MakePassive(string subjectNoun, string infinitive, EoVerbTense estiTense,
                                         EoParticipleAspect passiveAspect, string? agent = null)
        {
            if (string.IsNullOrWhiteSpace(infinitive))
                throw new ArgumentException("Verb infinitive is required.", nameof(infinitive));

            if (string.IsNullOrWhiteSpace(subjectNoun))
                throw new ArgumentException("Subject noun is required.", nameof(subjectNoun));

            if (!IsPassiveParticiple(passiveAspect))
                throw new ArgumentException($"Use a passive participle aspect for passive forms (e.g. {nameof(EoParticipleAspect.PresentPassive)}).");

            string estiConj = Conjugate(esti, estiTense);
            string part = MakeParticiple(infinitive, passiveAspect);

            string basePhrase = $"{subjectNoun} {estiConj} {part}";
            if (!string.IsNullOrWhiteSpace(agent))
                basePhrase += $" {de} {agent}";
            return basePhrase;
        }

        /// <summary>
        /// Produces a conjugation table for the given Esperanto verb across all pronouns and tenses.
        /// </summary>
        /// <param name="infinitive">The verb infinitive (e.g. "esti", "paroli").</param>
        public static IEnumerable<EoVerbConjugation> GetConjugationTable(string infinitive)
        {
            if (string.IsNullOrWhiteSpace(infinitive))
                throw new ArgumentException("Verb infinitive is required.", nameof(infinitive));

            infinitive = EoTextConverter.Convert(infinitive, EoTextConvention.Unknown, EoTextConvention.Utf8) ?? string.Empty;

            foreach (var tense in Enum.GetValues<EoVerbTense>())
            {
                foreach (var pronoun in Pronouns)
                {
                    yield return new()
                    {
                        Tense = tense.ToString(),
                        Pronoun = pronoun,
                        VerbForm = Conjugate(infinitive, tense),
                        TextConvention = EoTextConvention.Utf8,
                    };
                }
            }
        }

        /// <summary>
        /// Produces a conjugation table for the given Esperanto verb across all pronouns and tenses 
        /// with the specified text convention.
        /// </summary>
        /// <param name="infinitive">The verb infinitive (e.g. "esti", "paroli").</param>
        /// <param name="outputConvention">The Esperanto text convention to utilize for the output.</param>
        public static IEnumerable<EoVerbConjugation> GetConjugationTable(string infinitive, EoTextConvention outputConvention)
        {
            foreach (var v in GetConjugationTable(infinitive))
            {
                yield return new()
                {
                    Pronoun = EoTextConverter.Convert(v.Pronoun, EoTextConvention.Utf8, outputConvention) ?? string.Empty,
                    Tense = v.Tense,
                    TextConvention = outputConvention,
                    VerbForm = EoTextConverter.Convert(v.VerbForm, EoTextConvention.Utf8, outputConvention) ?? string.Empty,
                };
            }
        }

        /// <summary>
        /// Determines whether or not the specified participle aspect is passive.
        /// </summary>
        /// <param name="aspect">The participle aspect.</param>
        public static bool IsPassiveParticiple(EoParticipleAspect aspect)
        {
            return ((int)aspect & (int)EoVoice.Passive) == (int)EoVoice.Passive;
        }

        /// <summary>
        /// Determines whether or not the specified participle aspect is active.
        /// </summary>
        /// <param name="aspect">The participle aspect.</param>
        public static bool IsActiveParticiple(EoParticipleAspect aspect)
        {
            return ((int)aspect & (int)EoVoice.Active) == (int)EoVoice.Active;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infinitive">The verb infinitive (e.g. "esti", "paroli").</param>
        /// <param name="pronoun">The Esperanto pronoun.</param>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<EoCompoundVerb> GetCompoundTable(string infinitive, string pronoun)
        {
            if (string.IsNullOrWhiteSpace(infinitive))
                throw new ArgumentException("Verb infinitive is required.", nameof(infinitive));

            if (string.IsNullOrWhiteSpace(pronoun))
                throw new ArgumentException("Pronoun is required.", nameof(pronoun));

            if (!IsPronoun(pronoun))
                throw new ArgumentException($"Invalid Esperanto pronoun: {pronoun}", nameof(pronoun));

            pronoun = EoTextConverter.Convert(pronoun, EoTextConvention.Unknown, EoTextConvention.Utf8) ?? string.Empty;
            infinitive = EoTextConverter.Convert(infinitive, EoTextConvention.Unknown, EoTextConvention.Utf8) ?? string.Empty;

            var tenses = Enum.GetValues<EoVerbTense>();
            var aspects = Enum.GetValues<EoParticipleAspect>();

            // active compounds
            foreach (var t in tenses)
            {
                foreach (var a in aspects)
                {
                    if (IsActiveParticiple(a))
                        yield return new EoCompoundVerb()
                        {
                            Aspect = a.ToEnumString('-'),
                            Tense = t.ToString(),
                            Pronoun = pronoun,
                            VerbForm = ConjugateCompound(pronoun, infinitive, t, a),
                            TextConvention = EoTextConvention.Utf8
                        };
                }
            }

            // passive compounds
            foreach (var t in tenses)
            {
                foreach (var a in aspects)
                {
                    if (IsPassiveParticiple(a))
                        yield return new EoCompoundVerb()
                        {
                            Aspect = a.ToEnumString('-'),
                            Tense = t.ToString(),
                            Pronoun = pronoun,
                            VerbForm = MakePassive(LaLibro, infinitive, t, a, pronoun),
                            TextConvention = EoTextConvention.Utf8
                        };
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infinitive">The verb infinitive (e.g. "esti", "paroli").</param>
        /// <param name="pronoun">The Esperanto pronoun.</param>
        /// <param name="outputConvention">The Esperanto text convention to utilize for the output.</param>
        /// <returns></returns>
        public static IEnumerable<EoCompoundVerb> GetCompoundTable(string infinitive, string pronoun, EoTextConvention outputConvention)
        {
            foreach (var v in GetCompoundTable(infinitive, pronoun))
            {
                yield return new EoCompoundVerb()
                {
                    Aspect = v.Aspect,
                    Tense = v.Tense,
                    Pronoun = EoTextConverter.Convert(v.Pronoun, EoTextConvention.Utf8, outputConvention) ?? string.Empty,
                    TextConvention = outputConvention,
                    VerbForm = EoTextConverter.Convert(v.VerbForm, EoTextConvention.Utf8, outputConvention) ?? string.Empty,
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infinitive">The verb infinitive (e.g. "esti", "paroli").</param>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<EoParticiple> GetParticipleTable(string infinitive)
        {
            if (string.IsNullOrWhiteSpace(infinitive))
                throw new ArgumentException("Verb infinitive is required.", nameof(infinitive));

            infinitive = EoTextConverter.Convert(infinitive, EoTextConvention.Unknown, EoTextConvention.Utf8) ?? string.Empty;

            // participles
            foreach (var a in Enum.GetValues<EoParticipleAspect>())
            {
                yield return new()
                {
                    Aspect = a.ToEnumString('-'),
                    Participle = MakeParticiple(infinitive, a),
                    TextConvention = EoTextConvention.Utf8,
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infinitive">The verb infinitive (e.g. "esti", "paroli").</param>
        /// <param name="outputConvention">The Esperanto text convention to utilize for the output.</param>
        public static IEnumerable<EoParticiple> GetParticipleTable(string infinitive, EoTextConvention outputConvention)
        {
            foreach (var p in GetParticipleTable(infinitive))
            {
                yield return new()
                {
                    Aspect = p.Aspect,
                    Participle = EoTextConverter.Convert(p.Participle, EoTextConvention.Utf8, outputConvention) ?? string.Empty
                };
            }
        }
    }
}

