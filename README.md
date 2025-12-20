# Llc.GoodConsulting.Language.Esperanto

A lightweight, dependency-free **Esperanto language** utility library for C# 8.  
This package provides core building blocks for Esperanto verb conjugation, participles, compound verbs, and text conversion, designed for correctness, clarity, and reuse.

---

## 📦 Installation

```powershell
dotnet add package Llc.GoodConsulting.Language.Esperanto
```

---

## 📚 Usage Examples

### `EoConjugator`
Core engine for conjugating Esperanto verbs.

```csharp
var conjugation = EoConjugator.Conjugate("esti", EoVerbTense.Past);
Console.WriteLine(conjugation);    // estis
```

```csharp
var conjugation = EoConjugator.Conjugate("vidi", EoVerbTense.Conditional);
Console.WriteLine(conjugation);    // vidus
```

---

### `EoTextConverter`
Handles Esperanto diacritic conversions to/from X-system, H-system, LaTex, HTML entities, and UTF-8.

```csharp
var ascii = "cxu vi sxatas Esperanton?";
var unicode = EoTextConverter.Convert(ascii);

// ĉu vi ŝatas Esperanton?
```

```csharp
var ascii = "chu vi shatas Esperanton?";
var textConvention = EoTextConverter.GuessConvention(ascii);

// EoTextConvention.H
```

---

### `EoExtensions`
Helpful extension methods for Esperanto strings.

---

## 🎯 Target Frameworks

- .NET Standard
- .NET Core
- .NET 5+
- C# 8 compatible

---

## 📄 License

MIT License  
