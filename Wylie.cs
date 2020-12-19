using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;

public class Wylie
{
    public bool check;
    public bool check_strict;
    public bool print_warnings;
    public bool fix_spacing;

    public Dictionary<string, string> m_consonant;
    public Dictionary<string, string> m_subjoined;

    public Dictionary<string, string> m_vowel;
    public Dictionary<string, string> m_final_uni;
    public Dictionary<string, string> m_final_class;
    public Dictionary<string, string> m_other;
    public Dictionary<string, string> m_ambiguous_wylie;
    public Dictionary<string, string> m_tib_vowel_long;
    public Dictionary<string, string> m_tib_caret;
    public Dictionary<string, string> m_tib_top;
    public Dictionary<string, string> m_tib_subjoined;
    public Dictionary<string, string> m_tib_vowel;
    public Dictionary<string, string> m_tib_final_wylie;
    public Dictionary<string, string> m_tib_final_class;
    public Dictionary<string, string> m_tib_other;

    public Dictionary<string, int?> m_ambiguous_key;
    public Dictionary<string, int?> m_tokens_start;

    public List<string> m_special;
    public List<string> m_suffixes;
    public List<string> m_tib_stacks;
    public List<string> m_tokens;

    public Dictionary<string, List<string>> m_superscripts;
    public Dictionary<string, List<string>> m_subscripts;
    public Dictionary<string, List<string>> m_prefixes;
    public Dictionary<string, List<string>> m_suff2;
    //  initialize all the hashes with the correspondences between Wylie and Unicode.
    //  this gets called from a 'static section' to initialize the hashes the moment the
    //  class gets loaded.
    public void InitHashes()
    {
        //  *** Wylie to Unicode mappings ***
        //  list of wylie consonant => unicode
        this.m_consonant = new Dictionary<string, string> {
                { "k", "\u0f40" },
                { "kh", "\u0f41" },
                { "g", "\u0f42" },
                { "gh", "\u0f42\u0fb7" },
                { "g+h", "\u0f42\u0fb7" },
                { "ng", "\u0f44" },
                { "c", "\u0f45" },
                { "ch", "\u0f46" },
                { "j", "\u0f47" },
                { "ny", "\u0f49" },
                { "T", "\u0f4a" },
                { "-t", "\u0f4a" },
                { "Th", "\u0f4b" },
                { "-th", "\u0f4b" },
                { "D", "\u0f4c" },
                { "-d", "\u0f4c" },
                { "Dh", "\u0f4c\u0fb7" },
                { "D+h", "\u0f4c\u0fb7" },
                { "-dh", "\u0f4c\u0fb7" },
                { "-d+h", "\u0f4c\u0fb7" },
                { "N", "\u0f4e" },
                { "-n", "\u0f4e" },
                { "t", "\u0f4f" },
                { "th", "\u0f50" },
                { "d", "\u0f51" },
                { "dh", "\u0f51\u0fb7" },
                { "d+h", "\u0f51\u0fb7" },
                { "n", "\u0f53" },
                { "p", "\u0f54" },
                { "ph", "\u0f55" },
                { "b", "\u0f56" },
                { "bh", "\u0f56\u0fb7" },
                { "b+h", "\u0f56\u0fb7" },
                { "m", "\u0f58" },
                { "ts", "\u0f59" },
                { "tsh", "\u0f5a" },
                { "dz", "\u0f5b" },
                { "dzh", "\u0f5b\u0fb7" },
                { "dz+h", "\u0f5b\u0fb7" },
                { "w", "\u0f5d" },
                { "zh", "\u0f5e" },
                { "z", "\u0f5f" },
                { "'", "\u0f60" },
                { "\u2018", "\u0f60" },
                { "\u2019", "\u0f60" },
                { "y", "\u0f61" },
                { "r", "\u0f62" },
                { "l", "\u0f63" },
                { "sh", "\u0f64" },
                { "Sh", "\u0f65" },
                { "-sh", "\u0f65" },
                { "s", "\u0f66" },
                { "h", "\u0f67" },
                { "W", "\u0f5d" },
                { "Y", "\u0f61" },
                { "R", "\u0f6a" },
                { "f", "\u0f55\u0f39" },
                { "v", "\u0f56\u0f39" }};

        //  subjoined letters
        this.m_subjoined = new Dictionary<string, string> {
                { "k", "\u0f90" },
                { "kh", "\u0f91" },
                { "g", "\u0f92" },
                { "gh", "\u0f92\u0fb7" },
                { "g+h", "\u0f92\u0fb7" },
                { "ng", "\u0f94" },
                { "c", "\u0f95" },
                { "ch", "\u0f96" },
                { "j", "\u0f97" },
                { "ny", "\u0f99" },
                { "T", "\u0f9a" },
                { "-t", "\u0f9a" },
                { "Th", "\u0f9b" },
                { "-th", "\u0f9b" },
                { "D", "\u0f9c" },
                { "-d", "\u0f9c" },
                { "Dh", "\u0f9c\u0fb7" },
                { "D+h", "\u0f9c\u0fb7" },
                { "-dh", "\u0f9c\u0fb7" },
                { "-d+h", "\u0f9c\u0fb7" },
                { "N", "\u0f9e" },
                { "-n", "\u0f9e" },
                { "t", "\u0f9f" },
                { "th", "\u0fa0" },
                { "d", "\u0fa1" },
                { "dh", "\u0fa1\u0fb7" },
                { "d+h", "\u0fa1\u0fb7" },
                { "n", "\u0fa3" },
                { "p", "\u0fa4" },
                { "ph", "\u0fa5" },
                { "b", "\u0fa6" },
                { "bh", "\u0fa6\u0fb7" },
                { "b+h", "\u0fa6\u0fb7" },
                { "m", "\u0fa8" },
                { "ts", "\u0fa9" },
                { "tsh", "\u0faa" },
                { "dz", "\u0fab" },
                { "dzh", "\u0fab\u0fb7" },
                { "dz+h", "\u0fab\u0fb7" },
                { "w", "\u0fad" },
                { "zh", "\u0fae" },
                { "z", "\u0faf" },
                { "'", "\u0fb0" },
                { "\u2018", "\u0fb0" },
                { "\u2019", "\u0fb0" },
                { "y", "\u0fb1" },
                { "r", "\u0fb2" },
                { "l", "\u0fb3" },
                { "sh", "\u0fb4" },
                { "Sh", "\u0fb5" },
                { "-sh", "\u0fb5" },
                { "s", "\u0fb6" },
                { "h", "\u0fb7" },
                { "a", "\u0fb8" },
                { "W", "\u0fba" },
                { "Y", "\u0fbb" },
                { "R", "\u0fbc" }};
        //  vowels
        this.m_vowel = new Dictionary<string, string> {
                { "a", "\u0f68" },
                { "A", "\u0f71" },
                { "i", "\u0f72" },
                { "I", "\u0f71\u0f72" },
                { "u", "\u0f74" },
                { "U", "\u0f71\u0f74" },
                { "e", "\u0f7a" },
                { "ai", "\u0f7b" },
                { "o", "\u0f7c" },
                { "au", "\u0f7d" },
                { "-i", "\u0f80" },
                { "-I", "\u0f71\u0f80" }};
        //  final symbols to unicode
        this.m_final_uni = new Dictionary<string, string> {
                { "M", "\u0f7e" },
                { "~M`", "\u0f82" },
                { "~M", "\u0f83" },
                { "X", "\u0f37" },
                { "~X", "\u0f35" },
                { "H", "\u0f7f" },
                { "?", "\u0f84" },
                { "^", "\u0f39" }};
        //  final symbols organized by class
        this.m_final_class = new Dictionary<string, string> {
                { "M", "M" },
                { "~M`", "M" },
                { "~M", "M" },
                { "X", "X" },
                { "~X", "X" },
                { "H", "H" },
                { "?", "?" },
                { "^", "^" }};
        //  other stand-alone symbols
        this.m_other = new Dictionary<string, string> {
                { "0", "\u0f20" },
                { "1", "\u0f21" },
                { "2", "\u0f22" },
                { "3", "\u0f23" },
                { "4", "\u0f24" },
                { "5", "\u0f25" },
                { "6", "\u0f26" },
                { "7", "\u0f27" },
                { "8", "\u0f28" },
                { "9", "\u0f29" },
                { " ", "\u0f0b" },
                { "*", "\u0f0c" },
                { "/", "\u0f0d" },
                { "//", "\u0f0e" },
                { ";", "\u0f0f" },
                { "|", "\u0f11" },
                { "!", "\u0f08" },
                { ":", "\u0f14" },
                { "_", " " },
                { "=", "\u0f34" },
                { "<", "\u0f3a" },
                { ">", "\u0f3b" },
                { "(", "\u0f3c" },
                { ")", "\u0f3d" },
                { "@", "\u0f04" },
                { "#", "\u0f05" },
                { "$", "\u0f06" },
                { "%", "\u0f07" }};
        //  special characters: flag those if they occur out of context
        this.m_special = new List<string>
        {
            ".",
            "+",
            "-",
            "~",
            "^",
            "?",
            "`",
            "]"
        };
        // superscripts: hashmap of superscript => set of letters or stacks
        // below
        this.m_superscripts = new Dictionary<string, List<string>> {
                { "r", new List<string>()},
                { "l", new List<string>()},
                { "s", new List<string>()}};

        this.m_superscripts["r"].Add("k");
        this.m_superscripts["r"].Add("g");
        this.m_superscripts["r"].Add("ng");
        this.m_superscripts["r"].Add("j");
        this.m_superscripts["r"].Add("ny");
        this.m_superscripts["r"].Add("t");
        this.m_superscripts["r"].Add("d");
        this.m_superscripts["r"].Add("n");
        this.m_superscripts["r"].Add("b");
        this.m_superscripts["r"].Add("m");
        this.m_superscripts["r"].Add("ts");
        this.m_superscripts["r"].Add("dz");
        this.m_superscripts["r"].Add("k+y");
        this.m_superscripts["r"].Add("g+y");
        this.m_superscripts["r"].Add("m+y");
        this.m_superscripts["r"].Add("b+w");
        this.m_superscripts["r"].Add("ts+w");
        this.m_superscripts["r"].Add("g+w");

        this.m_superscripts["l"].Add("k");
        this.m_superscripts["l"].Add("g");
        this.m_superscripts["l"].Add("ng");
        this.m_superscripts["l"].Add("c");
        this.m_superscripts["l"].Add("j");
        this.m_superscripts["l"].Add("t");
        this.m_superscripts["l"].Add("d");
        this.m_superscripts["l"].Add("p");
        this.m_superscripts["l"].Add("b");
        this.m_superscripts["l"].Add("h");

        this.m_superscripts["s"].Add("k");
        this.m_superscripts["s"].Add("g");
        this.m_superscripts["s"].Add("ng");
        this.m_superscripts["s"].Add("ny");
        this.m_superscripts["s"].Add("t");
        this.m_superscripts["s"].Add("d");
        this.m_superscripts["s"].Add("n");
        this.m_superscripts["s"].Add("p");
        this.m_superscripts["s"].Add("b");
        this.m_superscripts["s"].Add("m");
        this.m_superscripts["s"].Add("ts");
        this.m_superscripts["s"].Add("k+y");
        this.m_superscripts["s"].Add("g+y");
        this.m_superscripts["s"].Add("p+y");
        this.m_superscripts["s"].Add("b+y");
        this.m_superscripts["s"].Add("m+y");
        this.m_superscripts["s"].Add("k+r");
        this.m_superscripts["s"].Add("g+r");
        this.m_superscripts["s"].Add("p+r");
        this.m_superscripts["s"].Add("b+r");
        this.m_superscripts["s"].Add("m+r");
        this.m_superscripts["s"].Add("n+r");
        //  subscripts => set of letters above
        this.m_subscripts = new Dictionary<string, List<string>> {
                { "y", new List<string>() },
                { "r", new List<string>() },
                { "l", new List<string>() },
                { "w", new List<string>() }};
        this.m_subscripts["y"].Add("k");
        this.m_subscripts["y"].Add("kh");
        this.m_subscripts["y"].Add("g");
        this.m_subscripts["y"].Add("p");
        this.m_subscripts["y"].Add("ph");
        this.m_subscripts["y"].Add("b");
        this.m_subscripts["y"].Add("m");
        this.m_subscripts["y"].Add("r+k");
        this.m_subscripts["y"].Add("r+g");
        this.m_subscripts["y"].Add("r+m");
        this.m_subscripts["y"].Add("s+k");
        this.m_subscripts["y"].Add("s+g");
        this.m_subscripts["y"].Add("s+p");
        this.m_subscripts["y"].Add("s+b");
        this.m_subscripts["y"].Add("s+m");
        this.m_subscripts["r"].Add("k");
        this.m_subscripts["r"].Add("kh");
        this.m_subscripts["r"].Add("g");
        this.m_subscripts["r"].Add("t");
        this.m_subscripts["r"].Add("th");
        this.m_subscripts["r"].Add("d");
        this.m_subscripts["r"].Add("n");
        this.m_subscripts["r"].Add("p");
        this.m_subscripts["r"].Add("ph");
        this.m_subscripts["r"].Add("b");
        this.m_subscripts["r"].Add("m");
        this.m_subscripts["r"].Add("sh");
        this.m_subscripts["r"].Add("s");
        this.m_subscripts["r"].Add("h");
        this.m_subscripts["r"].Add("dz");
        this.m_subscripts["r"].Add("s+k");
        this.m_subscripts["r"].Add("s+g");
        this.m_subscripts["r"].Add("s+p");
        this.m_subscripts["r"].Add("s+b");
        this.m_subscripts["r"].Add("s+m");
        this.m_subscripts["r"].Add("s+n");
        this.m_subscripts["l"].Add("k");
        this.m_subscripts["l"].Add("g");
        this.m_subscripts["l"].Add("b");
        this.m_subscripts["l"].Add("r");
        this.m_subscripts["l"].Add("s");
        this.m_subscripts["l"].Add("z");
        this.m_subscripts["w"].Add("k");
        this.m_subscripts["w"].Add("kh");
        this.m_subscripts["w"].Add("g");
        this.m_subscripts["w"].Add("c");
        this.m_subscripts["w"].Add("ny");
        this.m_subscripts["w"].Add("t");
        this.m_subscripts["w"].Add("d");
        this.m_subscripts["w"].Add("ts");
        this.m_subscripts["w"].Add("tsh");
        this.m_subscripts["w"].Add("zh");
        this.m_subscripts["w"].Add("z");
        this.m_subscripts["w"].Add("r");
        this.m_subscripts["w"].Add("l");
        this.m_subscripts["w"].Add("sh");
        this.m_subscripts["w"].Add("s");
        this.m_subscripts["w"].Add("h");
        this.m_subscripts["w"].Add("g+r");
        this.m_subscripts["w"].Add("d+r");
        this.m_subscripts["w"].Add("ph+y");
        this.m_subscripts["w"].Add("r+g");
        this.m_subscripts["w"].Add("r+ts");

        //  prefixes => set of consonants or stacks after
        this.m_prefixes = new Dictionary<string, List<string>> {
                { "g", new List<string>()},
                { "d", new List<string>()},
                { "b", new List<string>()},
                { "m", new List<string>()},
                { "'", new List<string>()},
                { "\u2018", new List<string>()},
                { "\u2019", new List<string>()}};

        m_prefixes["g"] = new List<string> {
            "c",
            "ny",
            "t",
            "d",
            "n",
            "ts",
            "zh",
            "z",
            "y",
            "sh",
            "s"
        };

        m_prefixes["d"] = new List<string> {
            "k",
            "g",
            "ng",
            "p",
            "b",
            "m",
            "k+y",
            "g+y",
            "p+y",
            "b+y",
            "m+y",
            "k+r",
            "g+r",
            "p+r",
            "b+r"
        };

        this.m_prefixes["b"] = new List<string>()
        {
            "k",
            "g",
            "c",
            "t",
            "d",
            "ts",
            "zh",
            "z",
            "sh",
            "s",
            "r",
            "l",
            "k+y",
            "g+y",
            "k+r",
            "g+r",
            "r+l",
            "s+l",
            "r+k",
            "r+g",
            "r+ng",
            "r+j",
            "r+ny",
            "r+t",
            "r+d",
            "r+n",
            "r+ts",
            "r+dz",
            "s+k",
            "s+g",
            "s+ng",
            "s+ny",
            "s+t",
            "s+d",
            "s+n",
            "s+ts",
            "r+k+y",
            "r+g+y",
            "s+k+y",
            "s+g+y",
            "s+k+r",
            "s+g+r",
            "l+d",
            "l+t",
            "k+l",
            "s+r",
            "z+l",
            "s+w"
        };

        this.m_prefixes["m"] = new List<string>()
        {
            "kh",
            "g",
            "ng",
            "ch",
            "j",
            "ny",
            "th",
            "d",
            "n",
            "tsh",
            "dz",
            "kh+y",
            "g+y",
            "kh+r",
            "g+r",
        };

        this.m_prefixes["'"] = this.m_prefixes["\u2018"] = this.m_prefixes["\u2019"] = new List<string>()
        {
            "kh",
            "g",
            "ch",
            "j",
            "th",
            "d",
            "ph",
            "b",
            "tsh",
            "dz",
            "kh+y",
            "g+y",
            "ph+y",
            "b+y",
            "kh+r",
            "g+r",
            "d+r",
            "ph+r",
            "b+r"
        };


        //  set of suffix letters
        // also included are some Skt letters b/c they occur often in suffix
        // position in Skt words
        this.m_suffixes = new List<string>
        {
            "'",
            "\u2018",
            "\u2019",
            "g",
            "ng",
            "d",
            "n",
            "b",
            "m",
            "r",
            "l",
            "s",
            "N",
            "T",
            "-n",
            "-t"
        };
        //  suffix2 => set of letters before
        this.m_suff2 = new Dictionary<string, List<string>> {
                { "s", new List<string>()},
                { "d", new List<string>()}};
        this.m_suff2["s"] = new List<string>()
        {
            "g",
        "ng",
        "b",
        "m",
        };
        this.m_suff2["d"] = new List<string>()
        {
            "n",
        "r",
        "l",
        };

        //  root letter index for very ambiguous three-stack syllables
        this.m_ambiguous_key = new Dictionary<string, int?> {
                { "dgs", 1},
                { "dms", 1},
                { "'gs", 1},
                { "mngs", 0},
                { "bgs", 0},
                { "dbs", 1}};
        this.m_ambiguous_wylie = new Dictionary<string, string> {
                { "dgs", "dgas" },
                { "dms", "dmas" },
                { "'gs", "'gas" },
                { "mngs", "mangs" },
                { "bgs", "bags" },
                { "dbs", "dbas" }};
        //  *** Unicode to Wylie mappings ***
        //  top letters
        this.m_tib_top = new Dictionary<string, string> {
                { "\u0f40", "k" },
                { "\u0f41", "kh" },
                { "\u0f42", "g" },
                { "\u0f43", "g+h" },
                { "\u0f44", "ng" },
                { "\u0f45", "c" },
                { "\u0f46", "ch" },
                { "\u0f47", "j" },
                { "\u0f49", "ny" },
                { "\u0f4a", "T" },
                { "\u0f4b", "Th" },
                { "\u0f4c", "D" },
                { "\u0f4d", "D+h" },
                { "\u0f4e", "N" },
                { "\u0f4f", "t" },
                { "\u0f50", "th" },
                { "\u0f51", "d" },
                { "\u0f52", "d+h" },
                { "\u0f53", "n" },
                { "\u0f54", "p" },
                { "\u0f55", "ph" },
                { "\u0f56", "b" },
                { "\u0f57", "b+h" },
                { "\u0f58", "m" },
                { "\u0f59", "ts" },
                { "\u0f5a", "tsh" },
                { "\u0f5b", "dz" },
                { "\u0f5c", "dz+h" },
                { "\u0f5d", "w" },
                { "\u0f5e", "zh" },
                { "\u0f5f", "z" },
                { "\u0f60", "'" },
                { "\u0f61", "y" },
                { "\u0f62", "r" },
                { "\u0f63", "l" },
                { "\u0f64", "sh" },
                { "\u0f65", "Sh" },
                { "\u0f66", "s" },
                { "\u0f67", "h" },
                { "\u0f68", "a" },
                { "\u0f69", "k+Sh" },
                { "\u0f6a", "R" }};
        //  subjoined letters
        this.m_tib_subjoined = new Dictionary<string, string> {
                { "\u0f90", "k" },
                { "\u0f91", "kh" },
                { "\u0f92", "g" },
                { "\u0f93", "g+h" },
                { "\u0f94", "ng" },
                { "\u0f95", "c" },
                { "\u0f96", "ch" },
                { "\u0f97", "j" },
                { "\u0f99", "ny" },
                { "\u0f9a", "T" },
                { "\u0f9b", "Th" },
                { "\u0f9c", "D" },
                { "\u0f9d", "D+h" },
                { "\u0f9e", "N" },
                { "\u0f9f", "t" },
                { "\u0fa0", "th" },
                { "\u0fa1", "d" },
                { "\u0fa2", "d+h" },
                { "\u0fa3", "n" },
                { "\u0fa4", "p" },
                { "\u0fa5", "ph" },
                { "\u0fa6", "b" },
                { "\u0fa7", "b+h" },
                { "\u0fa8", "m" },
                { "\u0fa9", "ts" },
                { "\u0faa", "tsh" },
                { "\u0fab", "dz" },
                { "\u0fac", "dz+h" },
                { "\u0fad", "w" },
                { "\u0fae", "zh" },
                { "\u0faf", "z" },
                { "\u0fb0", "'" },
                { "\u0fb1", "y" },
                { "\u0fb2", "r" },
                { "\u0fb3", "l" },
                { "\u0fb4", "sh" },
                { "\u0fb5", "Sh" },
                { "\u0fb6", "s" },
                { "\u0fb7", "h" },
                { "\u0fb8", "a" },
                { "\u0fb9", "k+Sh" },
                { "\u0fba", "W" },
                { "\u0fbb", "Y" },
                { "\u0fbc", "R" }};
        //  vowel signs:
        //  a-chen is not here because that's a top character, not a vowel sign.
        //  pre-composed "I" and "U" are dealt here; other pre-composed Skt vowels are more
        // easily handled by a global replace in toWylie(), b/c they turn into
        // subjoined "r"/"l".
        this.m_tib_vowel = new Dictionary<string, string> {
                { "\u0f71", "A" },
                { "\u0f72", "i" },
                { "\u0f73", "I" },
                { "\u0f74", "u" },
                { "\u0f75", "U" },
                { "\u0f7a", "e" },
                { "\u0f7b", "ai" },
                { "\u0f7c", "o" },
                { "\u0f7d", "au" },
                { "\u0f80", "-i" }};
        //  long (Skt) vowels
        this.m_tib_vowel_long = new Dictionary<string, string> {
                { "i", "I" },
                { "u", "U" },
                { "-i", "-I" }};
        //  final symbols => wylie
        this.m_tib_final_wylie = new Dictionary<string, string> {
                { "\u0f7e", "M" },
                { "\u0f82", "~M`" },
                { "\u0f83", "~M" },
                { "\u0f37", "X" },
                { "\u0f35", "~X" },
                { "\u0f39", "^" },
                { "\u0f7f", "H" },
                { "\u0f84", "?" }};
        //  final symbols by class
        this.m_tib_final_class = new Dictionary<string, string> {
                { "\u0f7e", "M" },
                { "\u0f82", "M" },
                { "\u0f83", "M" },
                { "\u0f37", "X" },
                { "\u0f35", "X" },
                { "\u0f39", "^" },
                { "\u0f7f", "H" },
                { "\u0f84", "?" }};
        //  special characters introduced by ^
        this.m_tib_caret = new Dictionary<string, string> {
                { "ph", "f" },
                { "b", "v" }};
        //  other stand-alone characters
        this.m_tib_other = new Dictionary<string, string> {
                { " ", "_" },
                { "\u0f04", "@" },
                { "\u0f05", "#" },
                { "\u0f06", "$" },
                { "\u0f07", "%" },
                { "\u0f08", "!" },
                { "\u0f0b", " " },
                { "\u0f0c", "*" },
                { "\u0f0d", "/" },
                { "\u0f0e", "//" },
                { "\u0f0f", ";" },
                { "\u0f11", "|" },
                { "\u0f14", ":" },
                { "\u0f20", "0" },
                { "\u0f21", "1" },
                { "\u0f22", "2" },
                { "\u0f23", "3" },
                { "\u0f24", "4" },
                { "\u0f25", "5" },
                { "\u0f26", "6" },
                { "\u0f27", "7" },
                { "\u0f28", "8" },
                { "\u0f29", "9" },
                { "\u0f34", "=" },
                { "\u0f3a", "<" },
                { "\u0f3b", ">" },
                { "\u0f3c", "(" },
                { "\u0f3d", ")" }};
        //  all these stacked consonant combinations don't need "+"s in them
        this.m_tib_stacks = new List<string>
        {
            "b+l",
            "b+r",
            "b+y",
            "c+w",
            "d+r",
            "d+r+w",
            "d+w",
            "dz+r",
            "g+l",
            "g+r",
            "g+r+w",
            "g+w",
            "g+y",
            "h+r",
            "h+w",
            "k+l",
            "k+r",
            "k+w",
            "k+y",
            "kh+r",
            "kh+w",
            "kh+y",
            "l+b",
            "l+c",
            "l+d",
            "l+g",
            "l+h",
            "l+j",
            "l+k",
            "l+ng",
            "l+p",
            "l+t",
            "l+w",
            "m+r",
            "m+y",
            "n+r",
            "ny+w",
            "p+r",
            "p+y",
            "ph+r",
            "ph+y",
            "ph+y+w",
            "r+b",
            "r+d",
            "r+dz",
            "r+g",
            "r+g+w",
            "r+g+y",
            "r+j",
            "r+k",
            "r+k+y",
            "r+l",
            "r+m",
            "r+m+y",
            "r+n",
            "r+ng",
            "r+ny",
            "r+t",
            "r+ts",
            "r+ts+w",
            "r+w",
            "s+b",
            "s+b+r",
            "s+b+y",
            "s+d",
            "s+g",
            "s+g+r",
            "s+g+y",
            "s+k",
            "s+k+r",
            "s+k+y",
            "s+l",
            "s+m",
            "s+m+r",
            "s+m+y",
            "s+n",
            "s+n+r",
            "s+ng",
            "s+ny",
            "s+p",
            "s+p+r",
            "s+p+y",
            "s+r",
            "s+t",
            "s+ts",
            "s+w",
            "sh+r",
            "sh+w",
            "t+r",
            "t+w",
            "th+r",
            "ts+w",
            "tsh+w",
            "z+l",
            "z+w",
            "zh+w"
        };
        //  a map used to split the input string into tokens for fromWylie().
        //  all letters which start tokens longer than one letter are mapped to the max length of
        //  tokens starting with that letter.
        this.m_tokens_start = new Dictionary<string, int?> {
                { "S", 2},
                { "/", 2},
                { "d", 4},
                { "g", 3},
                { "b", 3},
                { "D", 3},
                { "z", 2},
                { "~", 3},
                { "-", 4},
                { "T", 2},
                { "a", 2},
                { "k", 2},
                { "t", 3},
                { "s", 2},
                { "c", 2},
                { "n", 2},
                { "p", 2},
                { "\r", 2}};
        //  also for tokenization - a set of tokens longer than one letter
        this.m_tokens = new List<string>
        {
            "-d+h",
            "dz+h",
            "-dh",
            "-sh",
            "-th",
            "D+h",
            "b+h",
            "d+h",
            "dzh",
            "g+h",
            "tsh",
            "~M`",
            "-I",
            "-d",
            "-i",
            "-n",
            "-t",
            "//",
            "Dh",
            "Sh",
            "Th",
            "ai",
            "au",
            "bh",
            "ch",
            "dh",
            "dz",
            "gh",
            "kh",
            "ng",
            "ny",
            "ph",
            "sh",
            "th",
            "ts",
            "zh",
            "~M",
            "~X",
            "\r\n"
        };
    }

    //  setup a wylie object
    public void InitWylie(bool check, bool check_strict, bool print_warnings, bool fix_spacing)
    {
        //  check_strict requires check
        if (check_strict && !check)
        {
            throw new Exception("check_strict requires check.");
        }
        this.check = check;
        this.check_strict = check_strict;
        this.print_warnings = print_warnings;
        this.fix_spacing = fix_spacing;
        this.InitHashes();
    }

    public Wylie()
    {
        InitWylie(true, true, false, true);
    }

    //  helper functions to access the various hash tables
    public virtual string consonant(string s)
    {
        return this.m_consonant.GetValueOrDefault(s, null);
    }

    public virtual string subjoined(string s)
    {
        return this.m_subjoined.GetValueOrDefault(s, null);
    }

    public virtual string vowel(string s)
    {
        return this.m_vowel.GetValueOrDefault(s, null);
    }

    public virtual string final_uni(string s)
    {
        return this.m_final_uni.GetValueOrDefault(s, null);
    }

    public virtual string final_class(string s)
    {
        return this.m_final_class.GetValueOrDefault(s, null);
    }

    public virtual string other(string s)
    {
        return this.m_other.GetValueOrDefault(s, null);
    }

    public virtual bool isSpecial(string s)
    {
        return this.m_special.Contains(s);
    }

    public virtual bool isSuperscript(string s)
    {
        return this.m_superscripts.Keys.Contains(s);
    }

    public virtual bool superscript(string sup, string below)
    {
        var tmpSet = this.m_superscripts.GetValueOrDefault(sup, null);
        if (tmpSet == null)
        {
            return false;
        }
        return tmpSet.Contains(below);
    }

    public virtual bool isSubscript(object s)
    {
        return this.m_subscripts.Keys.Contains(s);
    }

    public virtual bool subscript(string sub, string above)
    {
        var tmpSet = this.m_subscripts.GetValueOrDefault(sub, null);
        if (tmpSet == null)
        {
            return false;
        }
        return tmpSet.Contains(above);
    }

    public virtual bool isPrefix(string s)
    {
        return this.m_prefixes.Keys.Contains(s);
    }

    public virtual bool prefix(string pref, string after)
    {
        var tmpSet = this.m_prefixes.GetValueOrDefault(pref, null);
        if (tmpSet == null)
        {
            return false;
        }
        return tmpSet.Contains(after);
    }

    public virtual bool isSuffix(string s)
    {
        return this.m_suffixes.Contains(s);
    }

    public virtual bool isSuff2(string s)
    {
        return this.m_suff2.Keys.Contains(s);
    }

    public virtual bool suff2(string suff, string before)
    {
        var tmpSet = this.m_suff2.GetValueOrDefault(suff, null);
        if (tmpSet == null)
        {
            return false;
        }
        return tmpSet.Contains(before);
    }

    public virtual int? ambiguous_key(string syll)
    {
        return this.m_ambiguous_key.GetValueOrDefault(syll, null);
    }

    public virtual string ambiguous_wylie(string syll)
    {
        return this.m_ambiguous_wylie.GetValueOrDefault(syll, null);
    }

    public virtual string tib_top(string c)
    {
        return this.m_tib_top.GetValueOrDefault(c, null);
    }

    public virtual string tib_subjoined(string c)
    {
        return this.m_tib_subjoined.GetValueOrDefault(c, null);
    }

    public virtual string tib_vowel(string c)
    {
        return this.m_tib_vowel.GetValueOrDefault(c, null);
    }

    public virtual string tib_vowel_long(string s)
    {
        return this.m_tib_vowel_long.GetValueOrDefault(s, null);
    }

    public virtual string tib_final_wylie(string c)
    {
        return this.m_tib_final_wylie.GetValueOrDefault(c, null);
    }

    public virtual string tib_final_class(string c)
    {
        return this.m_tib_final_class.GetValueOrDefault(c, null);
    }

    public virtual string tib_caret(string s)
    {
        return this.m_tib_caret.GetValueOrDefault(s, null);
    }

    public virtual string tib_other(string c)
    {
        return this.m_tib_other.GetValueOrDefault(c, null);
    }

    public virtual bool tib_stack(string s)
    {
        return this.m_tib_stacks.Contains(s);
    }

    //  split a string into Wylie tokens;
    // make sure there is room for at least one null element at the end of the
    // array
    public virtual List<string> splitIntoTokens(string str_)
    {
        // noqa: C901
        var i = 0;
        var o = 0;
        var maxlen = str_.Length;
        var tokens = Enumerable.Repeat(string.Empty, maxlen + 2).ToList();
        while (i < maxlen)
        {
            try
            {
                var c = str_[i];
                var mlo = this.m_tokens_start.GetValueOrDefault(c.ToString(), null);
                // if there are multi-char tokens starting with this char, try
                // them
                if (mlo != null)
                {
                    var length = mlo.Value;
                    while (length > 1)
                    {
                        if (i <= maxlen - length)
                        {
                            var tr = str_.Substring(i, length);
                            if (this.m_tokens.Contains(tr))
                            {
                                tokens[o] = tr;
                                o += 1;
                                i += length;
                                length -= 1;
                                throw new Exception("Continue");
                            }
                        }
                        length -= 1;
                    }
                }
                //  things starting with backslash are special
                if (c == '\\' && i <= maxlen - 2)
                {
                    if (str_[i + 1] == 'u' && i <= maxlen - 6)
                    {
                        tokens[o] = str_.Substring(i, 6);
                        o += 1;
                        //  \\uxxxx
                        i += 6;
                    }
                    else if (str_[i + 1] == 'U' && i <= maxlen - 10)
                    {
                        tokens[o] = str_.Substring(i, 10);
                        o += 1;
                        //  \\Uxxxxxxxx
                        i += 10;
                    }
                    else
                    {
                        tokens[o] = str_.Substring(i, 2);
                        o += 1;
                        //  \\x
                        i += 2;
                    }
                    continue;
                }
                //  otherwise just take one char
                tokens[o] = c.ToString();
                o += 1;
                i += 1;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Continue") continue;
                throw ex;
            }

        }
        return tokens;
    }

    // Converts successive stacks of Wylie into unicode, starting at the given index
    // within the array of tokens.
    //
    // Assumes that the first available token is valid, and is either a vowel or a consonant.
    public virtual WylieTsekbar fromWylieOneTsekbar(List<string> tokens, int i)
    {
        // noqa: C901
        var orig_i = i;
        var t = tokens[i];
        // variables for tracking the state within the syllable as we parse it
        WylieStack stack = null;
        string prev_cons = null;
        var visarga = false;
        // variables for checking the root letter, after parsing a whole tsekbar made of only single
        // consonants and one consonant with "a" vowel
        var check_root = true;
        var consonants = new List<string>();
        var root_idx = -1;
        var output = "";
        var warns = new List<string>();
        // the type of token that we are expecting next in the input stream
        //   - PREFIX : expect a prefix consonant, or a main stack
        //   - MAIN   : expect only a main stack
        //   - SUFF1  : expect a 1st suffix
        //   - SUFF2  : expect a 2nd suffix
        //   - NONE   : expect nothing (after a 2nd suffix)
        //
        // the state machine is actually more lenient than this, in that a "main stack" is allowed
        // to come at any moment, even after suffixes.  this is because such syllables are sometimes
        // found in abbreviations or other places.  basically what we check is that prefixes and
        // suffixes go with what they are attached to.
        //
        // valid tsek-bars end in one of these states: SUFF1, SUFF2, NONE
        var state = State.PREFIX;
        // iterate over the stacks of a tsek-bar
        while (t != null && (this.vowel(t) != null || this.consonant(t) != null) && !visarga)
        {
            // STACK
            // translate a stack
            if (stack != null)
            {
                prev_cons = stack.single_consonant;
            }
            stack = this.fromWylieOneStack(tokens, i);
            i += stack.tokens_used;
            t = tokens[i];
            output += stack.uni_string;
            warns.AddRange(stack.warns);
            visarga = stack.visarga;
            if (!this.check)
            {
                continue;
            }
            // check for syllable structure consistency by iterating a simple state machine
            // - prefix consonant
            if (state == State.PREFIX && stack.single_consonant != null)
            {
                consonants.Add(stack.single_consonant);
                if (this.isPrefix(stack.single_consonant))
                {
                    var next = t;
                    if (this.check_strict)
                    {
                        next = this.consonantString(tokens, i);
                    }
                    if (next != null && !this.prefix(stack.single_consonant, next))
                    {
                        next = next.Replace("+", "");
                        warns.Add("Prefix \"" + stack.single_consonant + "\" does not occur before \"" + next + "\".");
                    }
                }
                else
                {
                    warns.Add("Invalid prefix consonant: \"" + stack.single_consonant + "\".");
                }
                state = State.MAIN;
            }
            else if (stack.single_consonant == null)
            {
                // - main stack with vowel or multiple consonants
                state = State.SUFF1;
                // keep track of the root consonant if it was a single cons with
                // an "a" vowel
                if (root_idx >= 0)
                {
                    check_root = false;
                }
                else if (stack.single_cons_a != null)
                {
                    consonants.Add(stack.single_cons_a);
                    root_idx = consonants.Count - 1;
                }
            }
            else if (state == State.MAIN)
            {
                // - unexpected single consonant after prefix
                warns.Add("Expected vowel after \"" + stack.single_consonant + "\".");
            }
            else if (state == State.SUFF1)
            {
                // - 1st suffix
                consonants.Add(stack.single_consonant);
                // check this one only in strict mode b/c it trips on lots of
                // Skt stuff
                if (this.check_strict)
                {
                    if (!this.isSuffix(stack.single_consonant))
                    {
                        warns.Add("Invalid suffix consonant: \"" + stack.single_consonant + "\".");
                    }
                }
                state = State.SUFF2;
            }
            else if (state == State.SUFF2)
            {
                // - 2nd suffix
                consonants.Add(stack.single_consonant);
                if (this.isSuff2(stack.single_consonant))
                {
                    if (!this.suff2(stack.single_consonant, prev_cons))
                    {
                        warns.Add("Second suffix \"" + stack.single_consonant + "\" does not occur after \"" + prev_cons + "\".");
                    }
                }
                else
                {
                    warns.Add("Invalid 2nd suffix consonant: \"" + stack.single_consonant + "\".");
                }
                state = State.NONE;
            }
            else if (state == State.NONE)
            {
                // - more crap after a 2nd suffix
                warns.Add("Cannot have another consonant \"" + stack.single_consonant + "\" after 2nd suffix.");
            }
        }
        if (state == State.MAIN && stack.single_consonant != null && this.isPrefix(stack.single_consonant))
        {
            warns.Add("Vowel expected after \"" + stack.single_consonant + "\".");
        }
        // check root consonant placement only if there were no warnings so far, and the syllable
        // looks ambiguous.  not many checks are needed here because the previous state machine
        // already takes care of most illegal combinations.
        if (this.check && warns.Count == 0 && check_root && root_idx >= 0)
        {
            // 2 letters where each could be prefix/suffix: root is 1st
            if (consonants.Count == 2 && root_idx != 0 && this.prefix(consonants[0], consonants[1]) && this.isSuffix(consonants[1]))
            {
                warns.Add("Syllable should probably be \"" + consonants[0] + "a" + consonants[1] + "\".");
            }
            else if (consonants.Count == 3 && this.isPrefix(consonants[0]) && this.suff2("s", consonants[1]) && consonants[2] == "s")
            {
                // 3 letters where 1st can be prefix, 2nd can be postfix before "s" and last is "s":
                // use a lookup table as this is completely ambiguous.
                var cc = this.joinStrings(consonants, "");
                cc = cc.Replace("\u2018", "\'");
                cc = cc.Replace("\u2019", "\'");
                var expect_key = this.ambiguous_key(cc);
                if (expect_key != null && Convert.ToInt32(expect_key) != root_idx)
                {
                    warns.Add("Syllable should probably be \"" + this.ambiguous_wylie(cc) + "\".");
                }
            }
        }
        // return the stuff as a WylieTsekbar struct
        var ret = new WylieTsekbar
        {
            uni_string = output,
            tokens_used = i - orig_i,
            warns = warns
        };
        return ret;
    }

    public virtual string unicodeEscape(List<string> warns, int line, string t)
    {
        var hex = t.Substring(0, 2);
        if (string.IsNullOrEmpty(hex))
        {
            return null;
        }
        if (!this.validHex(hex))
        {
            this.warnl(warns, line, "\"" + t + "\": invalid hex code.");
            return "";
        }
        return Convert.ToInt32(hex, 16).ToString();
        // Character.valueOf(str(int(hex, base=16))).__str__()
        //  Converts a Wylie (EWTS) string to unicode.  If 'warns' is not the null List, puts warnings into it.
        // @fromWylie.register(object, str, List)
    }

    public virtual string fromWylie(string str_, List<string> warns = null)
    {
        // noqa: C901
        var output = new List<string>();
        var line = 1;
        var units = 0;
        //  remove initial spaces if required
        if (this.fix_spacing)
        {
            str_ = Regex.Replace(str_, "^\\s+", "");
        }
        //  split into tokens
        var tokens = this.splitIntoTokens(str_);
        var i = 0;
        //  iterate over the tokens
        // __i_5 = i
        while (tokens[i] != "")
        {
            // ITER
            try
            {
                var t = tokens[i];
                string o = null;
                //  [non-tibetan text] : pass through, nesting brackets
                if (t == "[")
                {
                    var nesting = 1;
                    i += 1;
                    while (tokens[i] != null)
                    {
                        // ESC
                        t = tokens[i];
                        i += 1;
                        if (t == "[")
                        {
                            nesting += 1;
                        }
                        if (t == "]")
                        {
                            nesting -= 1;
                        }
                        if (nesting == 0)
                        {
                            throw new Exception("Continue");
                        }
                        // handle unicode escapes and \1-char escapes within
                        // [comments]...
                        if (t.StartsWith("\\u") || t.StartsWith("\\U"))
                        {
                            o = this.unicodeEscape(warns, line, t);
                            if (o != null)
                            {
                                output.Add(o);
                                continue;
                            }
                        }
                        if (t.StartsWith("\\"))
                        {
                            o = t.Substring(0, 1);
                        }
                        else
                        {
                            o = t;
                        }
                        output.Add(o);
                    }
                    this.warnl(warns, line, "Unfinished [non-Wylie stuff].");
                    break;
                }
                //  punctuation, numbers, etc
                o = this.other(t);
                if (o != null)
                {
                    output.Add(o);
                    i += 1;
                    units += 1;
                    //  collapse multiple spaces?
                    if (t == " " && this.fix_spacing)
                    {
                        while (tokens[i] != null && tokens[i] == " ")
                        {
                            i++;
                        }
                    }
                    continue;
                }
                if (this.vowel(t) != null || this.consonant(t) != null)
                {
                    var tb = this.fromWylieOneTsekbar(tokens, i);
                    var word = "";
                    var j = 0;
                    while (j < tb.tokens_used)
                    {
                        word += tokens[i + j];
                        j += 1;
                    }
                    output.Add(tb.uni_string);
                    i += tb.tokens_used;
                    units += 1;
                    foreach (var w in tb.warns)
                    {
                        this.warnl(warns, line, "\"" + word + "\": " + w);
                    }
                    continue;
                }
                if (t == "\ufeff" || t == "\u200b")
                {
                    i += 1;
                    continue;
                }
                if (t.StartsWith("\\u") || t.StartsWith("\\U"))
                {
                    o = this.unicodeEscape(warns, line, t);
                    if (o != null)
                    {
                        i += 1;
                        output.Add(o);
                        continue;
                    }
                }
                if (t.StartsWith("\\"))
                {
                    output.Add(t.Substring(0, 1));
                    i += 1;
                    continue;
                }
                if (t == "\r\n" || t == "\n" || t == "\r")
                {
                    line += 1;
                    output.Add(t);
                    i += 1;
                    if (this.fix_spacing)
                    {
                        while (tokens[i] != null && tokens[i] == " ")
                        {
                        }
                    }
                    continue;
                }
                if (t == "")
                {
                    i += 1;
                    continue;
                }
                var c = t[0];
                if (this.isSpecial(t) || c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z')
                {
                    this.warnl(warns, line, "Unexpected character \"" + t + "\".");
                }
                output.Add(t);
                i += 1;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Continue") continue;
                throw ex;
            }
        }
        if (units == 0)
        {
            this.warn(warns, "No Tibetan characters found!");
        }
        return string.Join("", output);
    }

    public virtual bool validHex(string t)
    {
        var i = 0;
        while (i < t.Length)
        {
            var c = t[i];
            if (!(c >= 'a' && c <= 'f' || c >= '0' && c <= '9'))
            {
                return false;
            }
            i += 1;
        }
        return true;
    }

    public virtual void warn(List<string> warns, string str_)
    {
        if (warns != null)
        {
            warns.Add(str_);
        }
        if (this.print_warnings)
        {
            Console.WriteLine(str_);
        }
    }

    public virtual void warnl(List<string> warns, int line, string str_)
    {
        this.warn(warns, "line " + line.ToString() + ": " + str_);
    }

    public virtual void debug(string str_)
    {
        Console.WriteLine(str_);
    }

    public virtual void debugvar(string o, object name)
    {
        Console.WriteLine(">>" + name + "<< : (" + o == null ? "NULL" : o + ")");
    }

    public virtual string joinStrings(List<string> a, string sep)
    {
        return string.Join(sep, (from x in a
                                    where x != null
                                    select x).ToList());
    }

    // Converts one stack's worth of Wylie into unicode, starting at the given index
    // within the array of tokens.
    // Assumes that the first available token is valid, and is either a vowel or a consonant.
    // Returns a WylieStack object.
    public virtual WylieStack fromWylieOneStack(List<string> tokens, int i)
    {
        var orig_i = i;
        string t = null;
        string t2 = null;
        // o = None
        var output = "";
        var warns = new List<string>();
        var consonants = 0;
        string vowel_found = null;
        // any vowel signs (that go under or above the main stack)
        object vowel_sign = null;
        string single_consonant = null;
        var plus = false;
        var caret = 0;
        var final_found = new Dictionary<string, string>
        {
        };
        // do we have a superscript?
        t = tokens[i];
        t2 = tokens[i + 1];
        if (t2 != null && this.isSuperscript(t) && this.superscript(t, t2))
        {
            if (this.check_strict)
            {
                var next = this.consonantString(tokens, i + 1);
                if (!this.superscript(t, next))
                {
                    next = next.Replace("+", "");
                    warns.Add("Superscript \"" + t + "\" does not occur above combination \"" + next + "\".");
                }
            }
            output += this.consonant(t);
            consonants += 1;
            i += 1;
            while (tokens[i] != null && tokens[i] == "^")
            {
                caret += 1;
                i += 1;
            }
        }
        // main consonant + stuff underneath.
        // this is usually executed just once, but the "+" subjoining
        // operator makes it come back here
        while (true)
        {
            // MAIN
            // main consonant (or a "a" after a "+")
            t = tokens[i];
            if (this.consonant(t) != null || output.Length > 0 && this.subjoined(t) != null)
            {
                if (output.Length > 0)
                {
                    output += this.subjoined(t);
                }
                else
                {
                    output += this.consonant(t);
                }
                i += 1;
                if (t == "a")
                {
                    vowel_found = "a";
                }
                else
                {
                    consonants += 1;
                    single_consonant = t;
                }
                while (tokens[i] != null && tokens[i] == "^")
                {
                    caret += 1;
                    i += 1;
                }
                // subjoined: rata, yata, lata, wazur.  there can be up two
                // subjoined letters in a stack.
                var z = 0;
                while (z < 2)
                {
                    t2 = tokens[i];
                    if (t2 != null && this.isSubscript(t2))
                    {
                        // lata does not occur below multiple consonants
                        // (otherwise we mess up "brla" = "b.r+la")
                        if (t2 == "l" && consonants > 1)
                        {
                            break;
                        }
                        // full stack checking (disabled by "+")
                        if (this.check_strict && !plus)
                        {
                            var prev = this.consonantStringBackwards(tokens, i - 1, orig_i);
                            if (!this.subscript(t2, prev))
                            {
                                prev = prev.Replace("+", "");
                                warns.Add("Subjoined \"" + t2 + "\" not expected after \"" + prev + "\".");
                            }
                        }
                        else if (this.check)
                        {
                            // simple check only
                            if (!this.subscript(t2, t) && !(z == 1 && t2 == "w" && t == "y"))
                            {
                                warns.Add("Subjoined \"" + t2 + "\"not expected after \"" + t + "\".");
                            }
                        }
                        output += this.subjoined(t2);
                        i += 1;
                        consonants += 1;
                        while (tokens[i] != null && tokens[i] == "^")
                        {
                            caret += 1;
                            i += 1;
                        }
                        t = t2;
                    }
                    else
                    {
                        break;
                    }
                    z += 1;
                }
            }
            // caret (^) can come anywhere in Wylie but in Unicode we generate it at the end of
            // the stack but before vowels if it came there (seems to be what OpenOffice expects),
            // or at the very end of the stack if that's how it was in
            // the Wylie.
            if (caret > 0)
            {
                if (caret > 1)
                {
                    warns.Add("Cannot have more than one \"^\" applied to the same stack.");
                }
                final_found[this.final_class("^")] = "^";
                output += this.final_uni("^");
                caret = 0;
            }
            // vowel(s)
            t = tokens[i];
            if (t != null && this.vowel(t) != null)
            {
                if (0 == output.Length)
                {
                    output += this.vowel("a");
                }
                if (!(t == "a"))
                {
                    output += this.vowel(t);
                }
                i += 1;
                vowel_found = t;
                if (!(t == "a"))
                {
                    vowel_sign = t;
                }
            }
            // plus sign: forces more subjoining
            t = tokens[i];
            if (t != null && t == "+")
            {
                i += 1;
                plus = true;
                // sanity check: next token must be vowel or subjoinable
                // consonant.
                t = tokens[i];
                if (t == null || this.vowel(t) == null && this.subjoined(t) == null)
                {
                    if (this.check)
                    {
                        warns.Add("Expected vowel or consonant after \"+\".");
                    }
                    break;
                }
                // consonants after vowels doesn't make much sense but process
                // it anyway
                if (this.check)
                {
                    if (this.vowel(t) == null && vowel_sign != null)
                    {
                        warns.Add("Cannot subjoin consonant (" + t + ") after vowel (" + vowel_sign + ") in same stack.");
                    }
                    else if (t == "a" && vowel_sign != null)
                    {
                        warns.Add("Cannot subjoin a-chen (a) after vowel (" + vowel_sign + ") in same stack.");
                    }
                }
                continue;
            }
            break;
        }
        // final tokens
        t = tokens[i];
        while (t != null && this.final_class(t) != null)
        {
            var uni = this.final_uni(t);
            var klass = this.final_class(t);
            // check for duplicates
            if (final_found.ContainsKey(klass))
            {
                if (final_found[klass] == t)
                {
                    warns.Add("Cannot have two \"" + t + "\" applied to the same stack.");
                }
                else
                {
                    warns.Add("Cannot have \"" + t + "\" and \"" + final_found[klass] + "\" applied to the same stack.");
                }
            }
            else
            {
                final_found[klass] = t;
                output += uni;
            }
            i += 1;
            single_consonant = null;
            t = tokens[i];
        }
        // if next is a dot "." (stack separator), skip it.
        if (tokens[i] != null && tokens[i] == ".")
        {
            i += 1;
        }
        // if we had more than a consonant and no vowel, and no explicit "+" joining, backtrack and
        // return the 1st consonant alone
        if (consonants > 1 && vowel_found == null)
        {
            if (plus)
            {
                if (this.check)
                {
                    warns.Add("Stack with multiple consonants should end with vowel.");
                }
            }
            else
            {
                i = orig_i + 1;
                consonants = 1;
                single_consonant = tokens[orig_i];
                output = "";
                output += this.consonant(single_consonant);
            }
        }
        // calculate "single consonant"
        if (consonants != 1 || plus)
        {
            single_consonant = null;
        }
        // return the stuff as a WylieStack struct
        var ret = new WylieStack();
        ret.uni_string = output;
        ret.tokens_used = i - orig_i;
        if (vowel_found != null)
        {
            ret.single_consonant = null;
        }
        else
        {
            ret.single_consonant = single_consonant;
        }
        if (vowel_found != null && vowel_found == "a")
        {
            ret.single_cons_a = single_consonant;
        }
        else
        {
            ret.single_cons_a = null;
        }
        ret.warns = warns;
        ret.visarga = final_found.ContainsKey("H");
        return ret;
    }

    public virtual string consonantString(List<string> tokens, int i)
    {
        var output = new List<string>();
        while (tokens[i] != null)
        {
            var t = tokens[i];
            i += 1;
            if (t == "+" || t == "^")
            {
                continue;
            }
            if (this.consonant(t) == null)
            {
                break;
            }
            output.Add(t);
        }
        return this.joinStrings(output, "+");
    }

    public virtual string consonantStringBackwards(List<string> tokens, int i, int orig_i)
    {
        var output = new List<string>();
        while (i >= orig_i && tokens[i] != null)
        {
            var t = tokens[i];
            i -= 1;
            if (t == "+" || t == "^")
            {
                continue;
            }
            if (this.consonant(t) == null)
            {
                break;
            }
            output.Insert(0, t);
        }
        return this.joinStrings(output, "+");
    }

    public virtual int handleSpaces(string str_, int i, string output)
    {
        var found = 0;
        // orig_i = i
        while (i < str_.Length && str_[i] == ' ')
        {
            i += 1;
            found += 1;
        }
        if (found == 0 || i == str_.Length)
        {
            return 0;
        }
        var t = str_[i];
        if (this.tib_top(t.ToString()) == null && this.tib_other(t.ToString()) == null)
        {
            return 0;
        }
        while (i < found)
        {
            output += "_";
            i += 1;
        }
        return found;
    }

    // Converts from Unicode strings to Wylie (EWTS) transliteration, without warnings,
    // including escaping of non-tibetan into [comments].
    public virtual string toWylie(string str_)
    {
        return this.toWylieOptions(str_, null, true);
    }

    // Converts from Unicode strings to Wylie (EWTS) transliteration.
    //
    // Arguments are:
    //    str   : the unicode string to be converted
    //    escape: whether to escape non-tibetan characters according to Wylie encoding.
    //            if escape == false, anything that is not tibetan will be just passed through.
    //
    // Returns: the transliterated string.
    //
    // To get the warnings, call getWarnings() afterwards.
    // @toWylie.register(object, str, List, bool)
    public virtual string toWylieOptions(string str_, List<string> warns, bool escape)
    {
        // noqa: C901
        var output = "";
        var line = 1;
        var units = 0;
        // globally search and replace some deprecated pre-composed Sanskrit
        // vowels
        str_ = str_.Replace("\u0f76", "\u0fb2\u0f80");
        str_ = str_.Replace("\u0f77", "\u0fb2\u0f71\u0f80");
        str_ = str_.Replace("\u0f78", "\u0fb3\u0f80");
        str_ = str_.Replace("\u0f79", "\u0fb3\u0f71\u0f80");
        str_ = str_.Replace("\u0f81", "\u0f71\u0f80");
        str_ = str_.Replace("\u0f00", "\u0F68\u0F7C\u0F7E");
        var i = 0;
        var length = str_.Length;
        // iterate over the string, codepoint by codepoint
        while (i < length)
        {
            // ITER
            var t = str_[i];
            // found tibetan script - handle one tsekbar
            if (this.tib_top(t.ToString()) != null)
            {
                var tb = this.toWylieOneTsekbar(str_, length, i);
                output += tb.wylie;
                i += tb.tokens_used;
                units += 1;
                foreach (var w in tb.warns)
                {
                    this.warnl(warns, line, w);
                }
                if (!escape)
                {
                    i += this.handleSpaces(str_, i, output);
                }
                continue;
            }
            // punctuation and special stuff.  spaces are tricky:
            // - in non-escaping mode: spaces are not turned to '_' here (handled by handleSpaces)
            // - in escaping mode: don't do spaces if there is non-tibetan coming, so they become part
            var o = this.tib_other(t.ToString());
            if (o != null && (t != ' ' || escape && !this.followedByNonTibetan(str_, i)))
            {
                output += o;
                i += 1;
                units += 1;
                if (!escape)
                {
                    i += this.handleSpaces(str_, i, output);
                }
                continue;
            }
            // newlines, count lines.  "\r\n" together count as one newline.
            if (t == '\r' || t == '\n')
            {
                line += 1;
                i += 1;
                output += t;
                if (t == '\r' && i < length && str_[i] == '\n')
                {
                    i += 1;
                    output += "\n";
                }
                continue;
            }
            // ignore BOM and zero-width space
            if (t == '\ufeff' || t == '\u200b')
            {
                i += 1;
                continue;
            }
            // anything else - pass along?
            if (!escape)
            {
                output += t;
                i += 1;
                continue;
            }
            // other characters in the tibetan plane, escape with \\u0fxx
            if (t > '\u0f00' && t <= '\u0fff')
            {
                // c = t.encode("utf8")
                output += t;
                i += 1;
                // warn for tibetan codepoints that should appear only after a
                // tib_top
                if (this.tib_subjoined(t.ToString()) != null || this.tib_vowel(t.ToString()) != null || this.tib_final_wylie(t.ToString()) != null)
                {
                    this.warnl(warns, line, "Tibetan sign " + t + " needs a top symbol to attach to.");
                }
                continue;
            }
            // ... or escape according to Wylie:
            // put it in [comments], escaping[] sequences and closing at
            // line ends
            output += "[";
            while (this.tib_top(t.ToString()) == null && (this.tib_other(t.ToString()) == null || t == ' ') && t != '\r' && t != '\n')
            {
                // \escape [opening and closing] brackets
                if (t == '[' || t == ']')
                {
                    output += "\\";
                    output += t;
                }
                else if (t > '\u0f00' && t <= '\u0fff')
                {
                    // unicode-escape anything in the tibetan plane (i.e characters
                    // not handled by Wylie)
                    output += this.formatHex(t.ToString());
                }
                else
                {
                    // and just pass through anything else!
                    output += t;
                }
                i += 1;
                if (i >= length)
                {
                    break;
                }
                t = str_[i];
            }
            output += "]";
        }
        return output;
    }

    public virtual string formatHex(string t)
    {
        return string.Join("", from c in t
                                select 32 <= c && c <= 126 ? c.ToString() : string.Format("\\u%04x", Convert.ToInt32(c)));
    }

    public virtual bool followedByNonTibetan(string str_, int i)
    {
        var length = str_.Length;
        while (i < length && str_[i] == ' ')
        {
            i += 1;
        }
        if (i == length)
        {
            return false;
        }
        var t = str_[i];
        return this.tib_top(t.ToString()) == null && this.tib_other(t.ToString()) == null && t != '\r' && t != '\n';
    }

    // C onvert Unicode to Wylie: one tsekbar
    public virtual ToWylieTsekbar toWylieOneTsekbar(string str_, int length, int i)
    {
        // noqa: C901
        var orig_i = i;
        var warns = new List<string>();
        var stacks = new List<ToWylieStack>();
        while (true)
        {
            // ITER
            var st = this.toWylieOneStack(str_, length, i);
            stacks.Add(st);
            if (st.warns != null)
            {
                warns.AddRange(st.warns);
            }
            i += st.tokens_used;
            if (st.visarga)
            {
                break;
            }
            if (i >= length || this.tib_top(str_[i].ToString()) == null)
            {
                break;
            }
        }
        // figure out if some of these stacks can be prefixes or suffixes (in which case
        // they don't need their "a" vowels)
        if (stacks.Count > 1 && stacks[0].single_cons != null)
        {
            var cs = stacks[1].cons_str.Replace("+w", "");
            if (this.prefix(stacks[0].single_cons, cs))
            {
                stacks[0].prefix = true;
            }
        }
        if (stacks.Count > 1 && stacks.Last().single_cons != null && this.isSuffix(stacks.Last().single_cons))
        {
            stacks.Last().suffix = true;
        }
        if (stacks.Count > 2 && stacks.Last().single_cons != null && stacks.ElementAt(stacks.Count - 2).single_cons != null && this.isSuffix(stacks.ElementAt(stacks.Count - 2).single_cons) && this.suff2(stacks.Last().single_cons, stacks.ElementAt(stacks.Count - 2).single_cons))
        {
            stacks.Last().suff2 = true;
            stacks.ElementAt(stacks.Count - 2).suffix = true;
        }
        if (stacks.Count == 2 && stacks[0].prefix && stacks[1].suffix)
        {
            stacks[0].prefix = false;
        }
        if (stacks.Count == 3 && stacks[0].prefix && stacks[1].suffix && stacks[2].suff2)
        {
            var strb = "";
            foreach (var st in stacks)
            {
                strb += st.single_cons;
            }
            var ztr = strb;
            var root = this.ambiguous_key(ztr);
            if (root == null)
            {
                warns.Add("Ambiguous syllable found: root consonant not known for \"" + ztr + "\".");
                root = 1;
            }
            stacks[root.Value].prefix = false;
            stacks[root.Value + 1].suff2 = false;
        }
        if (stacks[0].prefix && this.tib_stack(stacks[0].single_cons + "+" + stacks[1].cons_str))
        {
            stacks[0].dot = true;
        }
        var output = "";
        foreach (var st in stacks)
        {
            output += this.putStackTogether(st);
        }
        var ret = new ToWylieTsekbar
        {
            wylie = output,
            tokens_used = i - orig_i,
            warns = warns
        };
        return ret;
    }

    // Unicode to Wylie: one stack at a time
    public virtual ToWylieStack toWylieOneStack(string str_, int length, int i)
    {
        // noqa: C901
        var orig_i = i;
        string ffinal = null;
        string vowel = null;
        string klass = null;
        // split the stack into a ToWylieStack object:
        //   - top symbol
        //   - stacked signs (first is the top symbol again, then subscribed main characters...)
        //   - caret (did we find a stray tsa-phru or not?)
        //   - vowel signs (including small subscribed a-chung, "-i" Skt signs, etc)
        //   - final stuff (including anusvara, visarga, halanta...)
        //   - and some more variables to keep track of what has been found
        var st = new ToWylieStack();
        // assume: tib_top(t) exists
        var t = str_[i];
        i += 1;
        st.top = this.tib_top(t.ToString());
        st.stack.Add(this.tib_top(t.ToString()));
        // grab everything else below the top sign and classify in various
        // categories
        while (i < length)
        {
            t = str_[i];
            var o = this.tib_subjoined(t.ToString());
            var o1 = this.tib_vowel(t.ToString());
            var o2 = this.tib_final_wylie(t.ToString());
            if (o != null)
            {
                i += 1;
                st.stack.Add(o);
                // check for bad ordering
                if (st.finals.Count > 0)
                {
                    st.warns.Add("Subjoined sign \"" + o + "\" found after final sign \"" + ffinal + "\".");
                }
                else if (st.vowels.Count > 0)
                {
                    st.warns.Add("Subjoined sign \"" + o + "\" found after vowel sign \"" + vowel + "\".");
                }
            }
            else if (o1 != null)
            {
                i += 1;
                st.vowels.Add(o1);
                if (vowel == null)
                {
                    vowel = o1;
                }
                if (st.finals.Count > 0)
                {
                    st.warns.Add("Vowel sign \"" + o1 + "\" found after final sign \"" + ffinal + "\".");
                }
            }
            else if (o2 != null)
            {
                i += 1;
                klass = this.tib_final_class(t.ToString());
                if (o2 == "^")
                {
                    st.caret = true;
                }
                else
                {
                    if (o2 == "H")
                    {
                        st.visarga = true;
                    }
                    st.finals.Add(o2);
                    if (ffinal == null)
                    {
                        ffinal = o2;
                    }
                    if (st.finals_found.ContainsKey(klass))
                    {
                        st.warns.Add("Final sign \"" + o2 + "\" should not combine with found after final sign \"" + ffinal + "\".");
                    }
                    else
                    {
                        st.finals_found[klass] = o2;
                    }
                }
            }
            else
            {
                break;
            }
        }
        // now analyze the stack according to various rules
        // a - chen with vowel signs: remove the "a" and keep the vowel
        // signs
        if (st.top == "a" && st.stack.Count == 1 && st.vowels.Count > 0)
        {
            st.stack.RemoveAt(0);
        }
        // handle long vowels: A+i becomes I, etc.
        string vowel_long = null;
        if (st.vowels.Count > 1 && st.vowels[0] == "A" && this.tib_vowel_long(st.vowels[1]) != null)
        {
            vowel_long = this.tib_vowel_long(st.vowels[1]);
            st.vowels.RemoveAt(0);
            st.vowels.RemoveAt(0);
            st.vowels.Insert(0, vowel_long);
        }
        if (st.caret && st.stack.Count == 1 && this.tib_caret(st.top) != null)
        {
            var caret = this.tib_caret(st.top);
            st.top = vowel_long;
            st.stack.RemoveAt(0);
            st.stack.Insert(0, caret);
            st.caret = false;
        }
        st.cons_str = this.joinStrings(st.stack, "+");
        if (st.stack.Count == 1 && !(st.stack[0] == "a") && !st.caret && st.vowels.Count == 0 && st.finals.Count == 0)
        {
            st.single_cons = st.cons_str;
        }
        st.tokens_used = i - orig_i;
        return st;
    }

    public virtual object putStackTogether(ToWylieStack st)
    {
        var output = "";
        if (this.tib_stack(st.cons_str))
        {
            output += this.joinStrings(st.stack, "");
        }
        else
        {
            output += st.cons_str;
        }
        if (st.caret)
        {
            output += "^";
        }
        if (st.vowels.Count > 0)
        {
            output += this.joinStrings(st.vowels, "+");
        }
        else if (!st.prefix && !st.suffix && !st.suff2 && (st.cons_str.Length == 0 || st.cons_str.Last() != 'a'))
        {
            output += "a";
        }
        output += this.joinStrings(st.finals, "");
        if (st.dot)
        {
            output += ".";
        }
        return output;
    }

    public enum State
    {
        PREFIX,
        MAIN,
        SUFF1,
        SUFF2,
        NONE,
    }

    public class WylieStack
    {
        public string uni_string;
        public int tokens_used;
        public string single_consonant;
        public string single_cons_a;
        public List<string> warns;
        public bool visarga;
    }

    public class WylieTsekbar
    {
        public string uni_string;
        public int tokens_used;
        public List<string> warns;
    }

    public class ToWylieStack
    {
        public string top = null;

        public List<string> stack = new List<string>();

        public bool caret;

        public List<string> vowels = new List<string>();

        public List<string> finals = new List<string>();

        public Dictionary<string, string> finals_found = null;

        public bool visarga;

        public string cons_str = null;

        public string single_cons = null;

        public bool prefix;

        public bool suffix;

        public bool suff2;

        public bool dot;

        public int tokens_used;

        public List<string> warns;

        public ToWylieStack()
        {
            this.stack = new List<string>();
            this.vowels = new List<string>();
            this.finals = new List<string>();
            this.finals_found = new Dictionary<string, string>();
            this.warns = new List<string>();
        }
    }

    public class ToWylieTsekbar
    {
        public string wylie = null;

        public int tokens_used;

        public List<string> warns = null;
    }
}
