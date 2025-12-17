using System.Text;

namespace Llc.GoodConsulting.Language.Esperanto
{
    /// <summary>
    /// 
    /// </summary>
    public static class EoTextConverter
    {
        /// <summary>
        /// Attempts to detect the Esperanto text convention used in the specified <see cref="string"/>.
        /// </summary>
        /// <param name="text">Esperanto text to examine.</param>
        /// <returns><see cref="EoTextConvention"/></returns>
        public static EoTextConvention GuessConvention(string? text)
        {
            text = text?.Trim();
            var tempConv = EoTextConvention.Unknown;

            if (string.IsNullOrEmpty(text))
                return tempConv;

            if (text.Any(c => Utf8.ContainsKey(c)))
                tempConv = EoTextConvention.Utf8;

            for (var i = 0; i < text.Length; i++)
            {
                var tempVal = SwitchXConvention(text, i);
                int code;

                if (tempVal is int cx)
                {
                    code = cx;
                    if (Utf8.TryGetValue(code, out _))
                    {
                        if (tempConv != EoTextConvention.X && tempConv != EoTextConvention.Unknown)
                            return EoTextConvention.Mixed;
                        else
                            tempConv = EoTextConvention.X;
                    }
                }

                tempVal = SwitchHConvention(text, i);

                if (tempVal is int ch)
                {
                    code = ch;
                    if (Utf8.TryGetValue(code, out _))
                    {
                        if (tempConv != EoTextConvention.H && tempConv != EoTextConvention.Unknown)
                            return EoTextConvention.Mixed;
                        else
                            tempConv = EoTextConvention.H;
                    }
                }

                tempVal = SwitchLaTexConvention(text, i);

                if (tempVal is int cl)
                {
                    code = cl;
                    if (Utf8.TryGetValue(code, out _))
                    {
                        if (tempConv != EoTextConvention.LaTex && tempConv != EoTextConvention.Unknown)
                            return EoTextConvention.Mixed;
                        else
                            tempConv = EoTextConvention.LaTex;
                    }
                }

                tempVal = SwitchHtmlEntity(text, ref i);

                if (tempVal is int che)
                {
                    code = che;
                    if (Utf8.TryGetValue(code, out _))
                    {
                        if (tempConv != EoTextConvention.HtmlEntities && tempConv != EoTextConvention.Unknown)
                            return EoTextConvention.Mixed;
                        else
                            tempConv = EoTextConvention.HtmlEntities;
                    }
                }
            }
            return tempConv;
        }

        /// <summary>
        /// Converts the specified <see cref="string"/> to the Esperanto text convention <see cref="EoTextConvention.X"/>
        /// </summary>
        /// <param name="text">Esperanto text to convert.</param>
        /// <param name="inputConvention"><see cref="EoTextConvention"/> of the input text.</param>
        /// <returns><see cref="string"/></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string? ToX(string? text, EoTextConvention inputConvention = default)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            if (inputConvention == EoTextConvention.X)
            {
                var conv = GuessConvention(text);
                if (conv == EoTextConvention.X || conv == EoTextConvention.Unknown)
                    return text;
                else
                    inputConvention = EoTextConvention.Mixed;
            }

            if (inputConvention == EoTextConvention.Unknown || inputConvention == EoTextConvention.Mixed)
                return ToX(Convert(text), EoTextConvention.Utf8);

            var outStr = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (char.IsWhiteSpace(text[i]))
                {
                    outStr.Append(text[i]);
                    continue;
                }

                object tempChar = inputConvention switch
                {
                    EoTextConvention.Utf8 => SwitchUtf8Convention(text, i),
                    EoTextConvention.H => SwitchHConvention(text, i),
                    EoTextConvention.LaTex => SwitchLaTexConvention(text, i),
                    EoTextConvention.HtmlEntities => SwitchHtmlEntity(text, ref i),
                    _ => throw new ArgumentException("Invalid Esperanto input convention.", nameof(inputConvention))
                };

                if (tempChar is int code)
                {
                    if (inputConvention == EoTextConvention.H)
                    {
                        if (code == 364 || code == 365)
                            outStr.Append(XEntities[code]);
                        else if (outStr.Length > 0)
                        {
                            outStr.Length -= 1;
                            outStr.Append(XEntities[code]);
                        }
                    }
                    else
                    {
                        outStr.Append(XEntities[code]);

                        if (inputConvention != EoTextConvention.Utf8)
                            i++;
                    }
                }
                else
                {
                    outStr.Append((char)tempChar);
                }
            }
            return outStr.ToString();
        }

        /// <summary>
        /// Converts the specified <see cref="string"/> to the Esperanto text convention <see cref="EoTextConvention.Utf8"/>
        /// </summary>
        /// <param name="text">Esperanto text to convert.</param>
        /// <param name="inputConvention"><see cref="EoTextConvention"/> of the input text.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string? ToUtf8(string? text, EoTextConvention inputConvention = default)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            if (inputConvention == EoTextConvention.Utf8)
            {
                var conv = GuessConvention(text);
                if (conv == EoTextConvention.Utf8 || conv == EoTextConvention.Unknown)
                    return text;
                else
                    inputConvention = EoTextConvention.Mixed;
            }

            if (inputConvention == EoTextConvention.Unknown || inputConvention == EoTextConvention.Mixed)
                return Convert(text);

            var outStr = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (char.IsWhiteSpace(text[i]))
                {
                    outStr.Append(text[i]);
                    continue;
                }

                var tempChar = inputConvention switch
                {
                    EoTextConvention.X => SwitchXConvention(text, i),
                    EoTextConvention.H => SwitchHConvention(text, i),
                    EoTextConvention.LaTex => SwitchLaTexConvention(text, i),
                    EoTextConvention.HtmlEntities => SwitchHtmlEntity(text, ref i),
                    _ => throw new ArgumentException("Invalid Esperanto input convention.", nameof(inputConvention))
                };

                if (tempChar is int code)
                {
                    var espChar = Utf8[code];

                    if (inputConvention == EoTextConvention.H)
                    {
                        if (code == 364 || code == 365)
                            outStr.Append(espChar);
                        else if (outStr.Length > 0)
                        {
                            outStr.Length -= 1;
                            outStr.Append(espChar);
                        }
                    }
                    else if (inputConvention == EoTextConvention.X && outStr.Length > 0)
                    {
                        outStr.Length -= 1;
                        outStr.Append(espChar);
                    }
                    else
                    {
                        outStr.Append(espChar);
                        i++;
                    }
                }
                else
                {
                    outStr.Append((char)tempChar);
                }
            }
            return outStr.ToString();
        }

        /// <summary>
        /// Converts the specified <see cref="string"/> to the Esperanto text convention <see cref="EoTextConvention.HtmlEntities"/>
        /// </summary>
        /// <param name="text">Esperanto text to convert.</param>
        /// <param name="inputConvention"><see cref="EoTextConvention"/> of the input text.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string? ToHtmlEntities(string? text, EoTextConvention inputConvention = default)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            if (inputConvention == EoTextConvention.HtmlEntities)
            {
                var conv = GuessConvention(text);
                if (conv == EoTextConvention.HtmlEntities || conv == EoTextConvention.Unknown)
                    return text;
                else
                    inputConvention = EoTextConvention.Mixed;
            }

            if (inputConvention == EoTextConvention.Unknown || inputConvention == EoTextConvention.Mixed)
                return ToHtmlEntities(Convert(text), EoTextConvention.Utf8);

            var outStr = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (char.IsWhiteSpace(text[i]))
                {
                    outStr.Append(text[i]);
                    continue;
                }

                object tempChar = inputConvention switch
                {
                    EoTextConvention.Utf8 => SwitchUtf8Convention(text, i),
                    EoTextConvention.X => SwitchXConvention(text, i),
                    EoTextConvention.H => SwitchHConvention(text, i),
                    EoTextConvention.LaTex => SwitchLaTexConvention(text, i),
                    EoTextConvention.HtmlEntities => SwitchHtmlEntity(text, ref i),
                    _ => throw new ArgumentException("Invalid Esperanto input convention.", nameof(inputConvention))
                };

                if (tempChar is int code)
                {
                    if (inputConvention == EoTextConvention.X && outStr.Length > 0)
                    {
                        outStr.Length -= 1;
                        outStr.Append($"&#{code};");
                    }
                    else if (inputConvention == EoTextConvention.H)
                    {
                        if (code == 364 || code == 365)
                            outStr.Append($"&#{code};");
                        else if (outStr.Length > 0)
                        {
                            outStr.Length -= 1;
                            outStr.Append($"&#{code};");
                        }
                    }
                    else
                    {
                        outStr.Append($"&#{code};");

                        if (inputConvention != EoTextConvention.Utf8)
                            i++;
                    }
                }
                else
                {
                    outStr.Append((char)tempChar);
                }
            }
            return outStr.ToString();
        }

        /// <summary>
        /// Converts the specified <see cref="string"/> to the Esperanto text convention <see cref="EoTextConvention.H"/>
        /// </summary>
        /// <param name="text">Esperanto text to convert.</param>
        /// <param name="inputConvention"><see cref="EoTextConvention"/> of the input text.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string? ToH(string? text, EoTextConvention inputConvention = default)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            if (inputConvention == EoTextConvention.H)
            {
                var conv = GuessConvention(text);
                if (conv == EoTextConvention.H || conv == EoTextConvention.Unknown)
                    return text;
                else
                    inputConvention = EoTextConvention.Mixed;
            }

            if (inputConvention == EoTextConvention.Unknown || inputConvention == EoTextConvention.Mixed)
                return ToH(Convert(text), EoTextConvention.Utf8);

            var outStr = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (char.IsWhiteSpace(text[i]))
                {
                    outStr.Append(text[i]);
                    continue;
                }

                object tempChar = inputConvention switch
                {
                    EoTextConvention.Utf8 => SwitchUtf8Convention(text, i),
                    EoTextConvention.X => SwitchXConvention(text, i),
                    EoTextConvention.LaTex => SwitchLaTexConvention(text, i),
                    EoTextConvention.HtmlEntities => SwitchHtmlEntity(text, ref i),
                    _ => throw new ArgumentException("Invalid Esperanto input convention.", nameof(inputConvention))
                };

                if (tempChar is int code)
                {
                    if (inputConvention == EoTextConvention.X && outStr.Length > 0)
                    {
                        outStr.Length -= 1;
                        outStr.Append(HEntities[code]);
                    }
                    else
                    {
                        outStr.Append(HEntities[code]);

                        if (inputConvention != EoTextConvention.Utf8)
                            i++;
                    }
                }
                else
                {
                    outStr.Append((char)tempChar);
                }
            }
            return outStr.ToString();
        }

        /// <summary>
        /// Converts the specified <see cref="string"/> to the Esperanto text convention <see cref="EoTextConvention.LaTex"/>
        /// </summary>
        /// <param name="text">Esperanto text to convert.</param>
        /// <param name="inputConvention"><see cref="EoTextConvention"/> of the input text.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string? ToLaTex(string? text, EoTextConvention inputConvention = default)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            if (inputConvention == EoTextConvention.LaTex)
            {
                var conv = GuessConvention(text);
                if (conv == EoTextConvention.LaTex || conv == EoTextConvention.Unknown)
                    return text;
                else
                    inputConvention = EoTextConvention.Mixed;
            }

            if (inputConvention == EoTextConvention.Unknown || inputConvention == EoTextConvention.Mixed)
                return ToLaTex(Convert(text), EoTextConvention.Utf8);

            var outStr = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (char.IsWhiteSpace(text[i]))
                {
                    outStr.Append(text[i]);
                    continue;
                }

                object tempChar = inputConvention switch
                {
                    EoTextConvention.Utf8 => SwitchUtf8Convention(text, i),
                    EoTextConvention.X => SwitchXConvention(text, i),
                    EoTextConvention.H => SwitchHConvention(text, i),
                    EoTextConvention.HtmlEntities => SwitchHtmlEntity(text, ref i),
                    _ => throw new ArgumentException("Invalid Esperanto input convention.", nameof(inputConvention))
                };

                if (tempChar is int code)
                {
                    if (inputConvention == EoTextConvention.X && outStr.Length > 0)
                    {
                        outStr.Length -= 1;
                        outStr.Append(LaTexEntities[code]);
                    }
                    else if (inputConvention == EoTextConvention.H)
                    {
                        if (code == 364 || code == 365)
                            outStr.Append(LaTexEntities[code]);
                        else if (outStr.Length > 0)
                        {
                            outStr.Length -= 1;
                            outStr.Append(LaTexEntities[code]);
                        }
                    }
                    else if (inputConvention == EoTextConvention.HtmlEntities)
                    {
                        outStr.Append(LaTexEntities[code]);
                        i++;
                    }
                    else if (inputConvention == EoTextConvention.Utf8)
                    {
                        outStr.Append(LaTexEntities[code]);
                    }
                }
                else
                {
                    outStr.Append((char)tempChar);
                }
            }
            return outStr.ToString();
        }

        /// <summary>
        /// Converts the specified <see cref="string"/> to the specified Esperanto text convention.
        /// </summary>
        /// <param name="text">Esperanto text to convert.</param>
        /// <param name="inputConvention"><see cref="EoTextConvention"/> of the input text.</param>
        /// <param name="outputConvention"><see cref="EoTextConvention"/> of the output text.</param>
        /// <returns><see cref="string"/></returns>
        public static string? Convert(string? text, EoTextConvention inputConvention, EoTextConvention outputConvention)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            if (outputConvention == EoTextConvention.Unknown || outputConvention == EoTextConvention.Mixed)
                outputConvention = EoTextConvention.Utf8;

            if (inputConvention == EoTextConvention.Mixed)
                inputConvention = EoTextConvention.Unknown;

            return outputConvention switch
            {
                EoTextConvention.X => ToX(text, inputConvention),
                EoTextConvention.H => ToH(text, inputConvention),
                EoTextConvention.LaTex => ToLaTex(text, inputConvention),
                EoTextConvention.HtmlEntities => ToHtmlEntities(text, inputConvention),
                _ => ToUtf8(text, inputConvention),
            };
        }

        /// <summary>
        /// Converts the specified <see cref="string"/> to the Esperanto text convention <see cref="EoTextConvention.Utf8"/>.
        /// </summary>
        /// <param name="text">Esperanto text to convert.</param>
        /// <returns><see cref="string"/></returns>
        public static string? Convert(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var sb = new StringBuilder();

            for (int x = 0; x < text.Length; x++)
            {
                char c = text[x];

                // if we already have esperanto character, append and continue
                if (Utf8.ContainsKey(c))
                {
                    sb.Append(c);
                    continue;
                }

                var tempVal = SwitchXConvention(text, x);
                int code;

                if (tempVal is int cx)
                {
                    code = cx;
                    if (Utf8.TryGetValue(code, out char v))
                    {
                        if (sb.Length > 0)
                            sb.Length -= 1;
                        sb.Append(v);
                        continue;
                    }
                }

                tempVal = SwitchHConvention(text, x);

                if (tempVal is int ch)
                {
                    code = ch;
                    if (Utf8.TryGetValue(code, out char v))
                    {
                        if (sb.Length > 0)
                        {
                            if (code != 364 && code != 365)
                                sb.Length -= 1;
                        }
                        sb.Append(v);
                        continue;
                    }
                }

                tempVal = SwitchLaTexConvention(text, x);

                if (tempVal is int cl)
                {
                    code = cl;
                    if (Utf8.TryGetValue(code, out char v))
                    {
                        sb.Append(v);
                        x++;
                        continue;
                    }
                }

                tempVal = SwitchHtmlEntity(text, ref x);

                if (tempVal is int che)
                {
                    code = che;
                    if (Utf8.TryGetValue(code, out char v))
                    {
                        sb.Append(v);
                        x++;
                        continue;
                    }
                }

                sb.Append(c);
            }

            return sb.ToString();
        }

        static readonly Dictionary<char, int> HtmlEntities = new()
        {
            ['c'] = 265,
            ['C'] = 264,
            ['g'] = 285,
            ['G'] = 284,
            ['j'] = 309,
            ['J'] = 308,
            ['s'] = 349,
            ['S'] = 348,
            ['h'] = 293,
            ['H'] = 292,
            ['u'] = 365,
            ['U'] = 364
        };

        static readonly Dictionary<int, char> Utf8 = new()
        {
            [265] = 'ĉ',
            [264] = 'Ĉ',
            [285] = 'ĝ',
            [284] = 'Ĝ',
            [309] = 'ĵ',
            [308] = 'Ĵ',
            [349] = 'ŝ',
            [348] = 'Ŝ',
            [293] = 'ĥ',
            [292] = 'Ĥ',
            [365] = 'ŭ',
            [364] = 'Ŭ'
        };

        static readonly Dictionary<int, string> HEntities = new()
        {
            [265] = "ch",
            [264] = "Ch",
            [285] = "gh",
            [284] = "Gh",
            [309] = "jh",
            [308] = "Jh",
            [349] = "sh",
            [348] = "Sh",
            [293] = "hh",
            [292] = "Hh",
            [365] = "u",
            [364] = "U"
        };

        static readonly Dictionary<int, string> XEntities = new()
        {
            [265] = "cx",
            [264] = "Cx",
            [285] = "gx",
            [284] = "Gx",
            [309] = "jx",
            [308] = "Jx",
            [349] = "sx",
            [348] = "Sx",
            [293] = "hx",
            [292] = "Hx",
            [365] = "ux",
            [364] = "Ux"
        };

        static readonly Dictionary<int, string> LaTexEntities = new()
        {
            [265] = "^c",
            [264] = "^C",
            [285] = "^g",
            [284] = "^G",
            [309] = "^j",
            [308] = "^J",
            [349] = "^s",
            [348] = "^S",
            [293] = "^h",
            [292] = "^H",
            [365] = "^u",
            [364] = "^U"
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        static object SwitchXConvention(string text, int i)
        {
            char c = text[i];
            if (c == 'x' || c == 'X')
            {
                if (i > 0 && HtmlEntities.TryGetValue(text[i - 1], out int value))
                    return value;
            }
            return c;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        static object SwitchUtf8Convention(string text, int i)
        {
            char c = text[i];
            if (Utf8.ContainsKey(c))
                return (int)c;
            return c;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        static object SwitchLaTexConvention(string text, int i)
        {
            char c = text[i];
            if (c == '^' && i + 1 < text.Length)
            {
                char next = text[i + 1];
                if (HtmlEntities.TryGetValue(next, out int value))
                    return value;
            }
            return c;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        static object SwitchHConvention(string text, int i)
        {
            char c = text[i];
            switch (c)
            {
                case 'h':
                case 'H':
                    if (i > 0 && HtmlEntities.TryGetValue(text[i - 1], out int value))
                        return value;
                    break;

                case 'u':
                case 'U':
                    if (i > 0)
                    {
                        char prev = text[i - 1];
                        if (prev == 'a' || prev == 'e')
                            return HtmlEntities['u'];
                        if (prev == 'A' || prev == 'E')
                            return HtmlEntities['U'];
                    }
                    break;
            }
            return c;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        static object SwitchHtmlEntity(string text, ref int i)
        {
            if (text[i] == '&' && i + 1 < text.Length && text[i + 1] == '#')
            {
                int start = i + 2;
                var code = new StringBuilder();
                while (start < text.Length && char.IsDigit(text[start]))
                {
                    code.Append(text[start]);
                    start++;
                }
                if (int.TryParse(code.ToString(), out int num) && Utf8.ContainsKey(num))
                {
                    i = start - 1;
                    return num;
                }
            }
            return text[i];
        }
    }
}
