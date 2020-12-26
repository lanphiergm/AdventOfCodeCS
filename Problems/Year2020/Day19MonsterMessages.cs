//#define USESAMPLE
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Problems.Year2020
{
    class Day19MonsterMessages : ProblemBase<int>
    {
        private readonly Dictionary<int, Rule> _rules = new Dictionary<int, Rule>();

        public Day19MonsterMessages(ILogger logger) : base(logger, "Monster Messages", 2020, 19) { }

        protected override int ExecutePart1()
        {
            Initialize();
            int validCount = 0;
            var rule0 = _rules[0];
            foreach (string message in messages)
            {
                if (rule0.Evaluate(_rules, message, 0).Contains(message.Length))
                {
                    validCount++;
                }
            }
            return validCount;
        }

        protected override int ExecutePart2()
        {
            Initialize();
            _rules[8] = new OrRule(Logger, _rules[8], new SequenceRule(Logger, 42, 8));
            _rules[11] = new OrRule(Logger, _rules[11], new SequenceRule(Logger, 42, 11, 31));
            int validCount = 0;
            var rule0 = _rules[0];
            foreach (string message in messages)
            {
                if (rule0.Evaluate(_rules, message, 0).Contains(message.Length))
                {
                    validCount++;
                }
            }
            return validCount;
        }

        private abstract class Rule
        {
            protected Rule(ILogger logger)
            {
                Logger = logger;
            }

            protected ILogger Logger { get; }

            public abstract IEnumerable<int> Evaluate(Dictionary<int, Rule> rules, string message, int index);
        }

        private class ConstantRule : Rule
        {
            private readonly char _constant;

            public ConstantRule(ILogger logger, char constant) : base(logger)
            {
                _constant = constant;
            }

            public override IEnumerable<int> Evaluate(Dictionary<int, Rule> rules, string message, int index) => 
                index < message.Length && message[index] == _constant ? new int[] { index + 1 } : Array.Empty<int>();
        }

        private class SequenceRule : Rule
        {
            private readonly List<int> _sequence = new List<int>();

            public SequenceRule(ILogger logger, params int[] sequence) : base(logger) 
            {
                foreach (int i in sequence)
                {
                    _sequence.Add(i);
                }
            }

            public override IEnumerable<int> Evaluate(Dictionary<int, Rule> rules, string message, int index)
            {
                IEnumerable<int> indices = new int[] { index };
                _sequence.ForEach(rule =>
                {
                    indices = indices.SelectMany(i => rules[rule].Evaluate(rules, message, i));
                });
                return indices;
            }

            public void AddRuleId(int ruleId)
            {
                _sequence.Add(ruleId);
            }
        }

        private class OrRule : Rule
        {
            public OrRule(ILogger logger, Rule left = null,
                Rule right = null) : base(logger)
            {
                Left = left;
                Right = right;
            }

            public Rule Left { get; set; }
            public Rule Right { get; set; }

            public override IEnumerable<int> Evaluate(Dictionary<int, Rule> rules, string message, int index)
            {
                var leftConsumed = Left.Evaluate(rules, message, index);
                var rightConsumed = Right.Evaluate(rules, message, index);
                return leftConsumed.Union(rightConsumed).Where(i => i != 0);
            }
        }

        private void Initialize()
        {
            if (_rules.Any())
            {
                return;
            }

            foreach (string rule in ruleDefinitions)
            {
                var parts = rule.Split(':');
                int num = int.Parse(parts[0]);
                if (parts[1].Contains('"'))
                {
                    _rules[num] = new ConstantRule(Logger, parts[1].Trim().Trim('"')[0]);
                }
                else if (parts[1].Contains('|'))
                {
                    var orRule = new OrRule(Logger);
                    parts = parts[1].Split('|');
                    orRule.Left = ParseSequence(parts[0]);
                    orRule.Right = ParseSequence(parts[1]);
                    _rules[num] = orRule;
                }
                else
                {
                    _rules[num] = ParseSequence(parts[1]);
                }
            }
        }

        private SequenceRule ParseSequence(string sequenceDefinition)
        {
            var sequence = new SequenceRule(Logger);
            string[] parts = sequenceDefinition.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (string ruleNum in parts)
            {
                sequence.AddRuleId(int.Parse(ruleNum));
            }
            return sequence;
        }

        #region Data

#if USESAMPLE
        public static string[] ruleDefinitions =
        {
            "42: 9 14 | 10 1",
            "9: 14 27 | 1 26",
            "10: 23 14 | 28 1",
            "1: \"a\"",
            "11: 42 31",
            "5: 1 14 | 15 1",
            "19: 14 1 | 14 14",
            "12: 24 14 | 19 1",
            "16: 15 1 | 14 14",
            "31: 14 17 | 1 13",
            "6: 14 14 | 1 14",
            "2: 1 24 | 14 4",
            "0: 8 11",
            "13: 14 3 | 1 12",
            "15: 1 | 14",
            "17: 14 2 | 1 7",
            "23: 25 1 | 22 14",
            "28: 16 1",
            "4: 1 1",
            "20: 14 14 | 1 15",
            "3: 5 14 | 16 1",
            "27: 1 6 | 14 18",
            "14: \"b\"",
            "21: 14 1 | 1 14",
            "25: 1 1 | 1 14",
            "22: 14 14",
            "8: 42",
            "26: 14 22 | 1 20",
            "18: 15 15",
            "7: 14 5 | 1 21",
            "24: 14 1"
        };

        public static string[] messages =
        {
            "abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa",
            "bbabbbbaabaabba",
            "babbbbaabbbbbabbbbbbaabaaabaaa",
            "aaabbbbbbaaaabaababaabababbabaaabbababababaaa",
            "bbbbbbbaaaabbbbaaabbabaaa",
            "bbbababbbbaaaaaaaabbababaaababaabab",
            "ababaaaaaabaaab",
            "ababaaaaabbbaba",
            "baabbaaaabbaaaababbaababb",
            "abbbbabbbbaaaababbbbbbaaaababb",
            "aaaaabbaabaaaaababaa",
            "aaaabbaaaabbaaa",
            "aaaabbaabbaaaaaaabbbabbbaaabbaabaaa",
            "babaaabbbaaabaababbaabababaaab",
            "aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba"
        };
#else
        public static string[] ruleDefinitions =
        {
            "1: 65 3 | 106 55",
            "21: 106 67 | 65 98",
            "128: 65 94 | 106 17",
            "104: 13 106 | 79 65",
            "32: 106 21 | 65 9",
            "78: 127 106 | 88 65",
            "55: 106 126 | 65 59",
            "114: 122 65 | 45 106",
            "108: 65 49 | 106 12",
            "31: 106 71 | 65 100",
            "76: 65 65 | 106 65",
            "15: 12 106 | 46 65",
            "24: 65 94 | 106 91",
            "38: 106 51 | 65 91",
            "25: 22 76",
            "87: 97 65 | 48 106",
            "113: 106 12 | 65 76",
            "8: 42",
            "9: 106 117 | 65 86",
            "88: 62 65 | 75 106",
            "94: 65 65",
            "122: 106 12 | 65 56",
            "48: 49 106 | 91 65",
            "33: 76 106 | 12 65",
            "28: 81 65 | 83 106",
            "35: 106 106 | 22 65",
            "50: 106 35 | 65 51",
            "58: 106 106",
            "0: 8 11",
            "64: 106 76 | 65 58",
            "71: 90 106 | 43 65",
            "12: 106 65 | 106 106",
            "7: 81 65 | 30 106",
            "60: 22 106 | 106 65",
            "17: 106 106 | 65 65",
            "105: 120 65 | 52 106",
            "96: 106 102 | 65 29",
            "57: 94 106 | 58 65",
            "115: 106 24 | 65 45",
            "127: 65 52 | 106 38",
            "14: 54 65 | 36 106",
            "45: 94 106 | 12 65",
            "70: 63 106 | 19 65",
            "44: 17 106 | 12 65",
            "99: 51 65 | 35 106",
            "36: 119 106 | 109 65",
            "51: 106 65",
            "37: 125 106 | 5 65",
            "86: 118 65 | 114 106",
            "106: \"a\"",
            "126: 94 106",
            "79: 34 65 | 44 106",
            "61: 65 68 | 106 87",
            "119: 34 106 | 89 65",
            "29: 65 81 | 106 82",
            "120: 49 65 | 58 106",
            "53: 56 106 | 49 65",
            "5: 35 65 | 12 106",
            "73: 65 94 | 106 76",
            "10: 65 56 | 106 58",
            "83: 49 106 | 17 65",
            "89: 49 106 | 56 65",
            "43: 61 65 | 96 106",
            "68: 65 73 | 106 6",
            "82: 65 56 | 106 91",
            "4: 65 6 | 106 38",
            "2: 56 106 | 46 65",
            "30: 46 65 | 94 106",
            "3: 65 97 | 106 122",
            "56: 106 65 | 65 106",
            "74: 26 106 | 10 65",
            "107: 105 65 | 47 106",
            "47: 95 106 | 116 65",
            "77: 33 65 | 121 106",
            "80: 106 78 | 65 41",
            "66: 65 15 | 106 93",
            "118: 65 50 | 106 124",
            "112: 4 65 | 77 106",
            "69: 106 37 | 65 28",
            "92: 65 57 | 106 64",
            "101: 65 60 | 106 94",
            "91: 106 106 | 65 106",
            "90: 65 107 | 106 112",
            "109: 57 65 | 85 106",
            "39: 106 122 | 65 53",
            "40: 12 106",
            "84: 106 2 | 65 128",
            "103: 106 123 | 65 80",
            "52: 60 65 | 17 106",
            "95: 65 60 | 106 49",
            "46: 22 106 | 65 65",
            "111: 39 106 | 70 65",
            "117: 115 106 | 16 65",
            "13: 65 25 | 106 99",
            "124: 91 22",
            "62: 65 49 | 106 91",
            "98: 7 65 | 16 106",
            "59: 106 35 | 65 60",
            "125: 91 65 | 12 106",
            "81: 106 76 | 65 49",
            "49: 65 22 | 106 65",
            "34: 106 49 | 65 17",
            "85: 17 22",
            "27: 65 65 | 65 106",
            "100: 20 106 | 14 65",
            "23: 76 106 | 49 65",
            "16: 106 23 | 65 108",
            "97: 106 56",
            "11: 42 31",
            "93: 76 65 | 94 106",
            "67: 110 65 | 84 106",
            "19: 22 35",
            "26: 27 65 | 56 106",
            "22: 106 | 65",
            "18: 65 12 | 106 91",
            "65: \"b\"",
            "72: 40 106 | 113 65",
            "6: 65 51 | 106 58",
            "116: 76 106 | 17 65",
            "75: 106 76 | 65 35",
            "41: 65 66 | 106 72",
            "123: 104 65 | 1 106",
            "63: 106 51 | 65 60",
            "20: 106 69 | 65 111",
            "42: 65 103 | 106 32",
            "121: 106 17 | 65 35",
            "110: 85 65 | 18 106",
            "102: 101 106 | 50 65",
            "54: 106 74 | 65 92",
        };

        public static string[] messages =
        {
            "aaaaaabababbbbabaaababbb",
            "abbaabbabbbbbabaaabbbbab",
            "abbbabbabbbababbaaaabbabaaabaaba",
            "bbbbbaabaaaaabbbaaabbbbbabbbaabaabaaaabb",
            "babbbbbbbbabbbababbababbababbbab",
            "bbbbabbbabaaabaaabbbbabbbbabbbabaabaaabb",
            "babbbababbbabaaabaaaaaaaababbabbabbbbbaaaaabababbbaaabbabbbabbab",
            "baaabbaaaaabbbbbabbbabab",
            "aabaabbaabbababbaaaaaabbbabbaabaaaaabbbaabaabbaabaaaabbaaaababaa",
            "aaaababaaaaabbabbbbbaaaa",
            "abbaaabaaabbaabaababbbab",
            "bbbaaaabbbabaaabbbbabaab",
            "aabbbababbabaaaaabaabaaa",
            "abaaabbbabbbabbbabababbb",
            "bbbaabaaaabaabaaaababbbbbbbaabba",
            "baababbbbbbbbabaaabaabbabaaaabba",
            "abbaaaabaabbababababaabaabbbabaa",
            "baabaaaabbbbbaaababbbbaa",
            "abbbabbbaabbbbbbaabbaaaa",
            "baaabbbbbbbbbbabababbabbabbbabbaaaababbbbbaaaaaaabbbbbaababaaaab",
            "baaababbbabbaabbaaabbaaa",
            "aabbaabaaabbbbabbbabaabb",
            "bbbabaaabbbbaaabaaaabaaabbabbabaaabaaaabbaabbaabaaabbabaabababbbaaabbbbbbbaabbabbbabbbba",
            "aabbbbbbbaabbbbbaaaabaabbbbaababaaaaabbbbbabbbbb",
            "bbaabaabaabaabbaabababbabaababbaaaabaabbaabbaaabaababbbb",
            "aabababaababbbababbbbbba",
            "abbbaabbaabbbbabaabaaaba",
            "abbbbaaabaabbbbbabbbbaaaaababababbbabbababaaaaab",
            "babaababbbbbbabbaabbabbbaaaabbabaabbbbaabaaabaababaabbbbaabbbaaaaaaaaaba",
            "ababaaabbababbabbaaabbaaabbabbab",
            "baabbbbbbabaabbbaabbbbbbbabbaabaaabbbabbabbbaabbaaabbbabaaaababbaabbaabb",
            "aabbabbbbbbabababaaaabab",
            "babaabbbaaabbabbaabababb",
            "babaaababaababbaaaaabbab",
            "abaabbbbababaabbbaaaaaaaabbbbaab",
            "bbbbbabaaabababbabbabbaababbabbaabaabaabbaaaaabbaabbbaaa",
            "baabbabaaaaaababbaaabbba",
            "bbbaaaabbbbaaaaababbabbb",
            "bbbbbbabababaabbbbabbbbbabbbbaaabbabaaabaabaabbbaaabbaaabaaabbbabbbaabbbbababbbbbabbbabb",
            "aaaaaabbbbabbababbaaababaababbab",
            "aaaabaaabbbaaaababbbabab",
            "aaaabaaabaababbabbaaaabbabbbababbabbaaab",
            "abbabbbbbababbabaabbaabaaabbbbabababbaababaabbaaabaaaaaabbbabaabbabbbbaaabaabbabaabbabaa",
            "aababbbbbaabbababaababbabaaababbaaabaaaaabababaabbabbbbabbaaaaaa",
            "bbabbbbaaaaabaaababaabbbaababaabbbbbbaababbbbbbabbbbbaba",
            "aaabbbbbabababbababbabba",
            "aabbaaaabaabbabaabbbaaaa",
            "bbbbabbbbaaaaaabaaabbbab",
            "aaabaaaabaabbaaaaabaaaaa",
            "baabbabaabbaaaabbaaaaaba",
            "aabbbbabbbaaaabbababaabaabbbabab",
            "babbbbbbaabaabbbabaaaaba",
            "aaaaaabbaabababaabbababa",
            "abaabbbbbaabbbbbaabaabbbabbabaabbaaabaababbbbbba",
            "ababaabbbbabaaabaaabbaab",
            "bbababbaabbbabbaabbbaaba",
            "aabaabaabaabbbbbbbaababa",
            "aaaaaabaabaababbbababbabaaaaabaabaaaabbbbbbbaaaabbabbbba",
            "abbaabaabbbbabbbbabbaaab",
            "baaabbaabbaababbaaaabbaa",
            "bbbbabbbbbaaaabaabababab",
            "ababbbaaaaabbbbabbbbabbbbaabbbab",
            "aabaabaabbababbbbbbaabaaaaaababaaaaaaabb",
            "bababaaaaaaabbbbabaabbaa",
            "abaabbbabbabaabaabbbaaba",
            "bbbbbbaaabbabababaababaabaaaaaababbabbaa",
            "ababbbbbababbabbaaaabaabbaababaa",
            "abbbbababbbbabbababbaaab",
            "bbabbaaaabbbbbbbbbabaabaabaaaaabbbaaaabaaabababbababaabbbbaaabbb",
            "ababbbaabaaabbabbababbbb",
            "bbabaaabababaaabbaabaaab",
            "abaaabaabababbaaabaaaaab",
            "babbbbbbbabbbaaabbbabbbaabbbbbbabbbaabbbbababbbb",
            "aaaaabaabbabbbabbbbaaaabaababbbababbaaaababaaabbaaaaaaaaababaaab",
            "aaaaaabbaabbaabaaaabbbaaabaaabababbabbababbaaaaa",
            "aabbbabaababbbaabbbbbababbaaaaab",
            "bbabaaabaabbbbabaababaab",
            "bbabababaaaabbbababaabbbababbabbabbabbba",
            "bbbbbabaabbabaabbaaaabba",
            "babaaaaaaaaabaabbbbbbbaa",
            "abbaabaababaabbbabbaabab",
            "abbbbaaabbaaaaaaabaaabaabbaaabbbaabababbbbbabbaabbabaababaaaabaababbbabbaaabbabb",
            "aababbaaabbbbabbbbaaabba",
            "baaabbabbaabbabbaaaabbbb",
            "aaabaaabaaaabaabbababbba",
            "abbbbababaaabbabaabbaaab",
            "bababbaaaabaabababbbabab",
            "ababaababbabbaaaabbbbbbabbbbbabaaabbabbbbaabaabababbaabbbabbbabb",
            "abbabaabbbababbaabaaaabb",
            "bbbaaaaabaabbaababbabbaaababbbbbaabbbaaa",
            "aaabbaababbbaababbbbaaabbaabaaaaabbbbabbbbbbbbbbbabbbbabaababaabbaaabaababaaaaab",
            "aabbaababaaaaabbbbabbbba",
            "baababaaabbbaaaabaaaaaaa",
            "abbbabbabbbaabaaaaabbbaa",
            "bbbaabaabbbbbbbbbbaabbbb",
            "aabbaabaaaaaaaabbabaaabb",
            "aaaabaababbbaaababaababbabbaaabaaabbaaababaabaab",
            "aabbaaaaabaabbabbbaabbaababbabababbbaabbbbbabbaa",
            "abaaaaaabbbababbabbaaabb",
            "aabbaababbaaaabbaabbbababbaabbbb",
            "abababbaabaabbbbbabaaaab",
            "bbaabaaaaabaabaaabbbaabbaaaaabaabbaaabbabbaaaababaabbbabaabaabbb",
            "abaabbbbabbbaabbabaaaaaabaaabbbbbabbabbaaaaabbab",
            "abaaabbbbaabaabaaabbabaa",
            "bbaabaaabbbbbbabbbbbbbba",
            "bbbaaabbbabbaabbaaabaaabbabbaabbbaabaaab",
            "baababbaababaabbbababbbbabaabaaaaabbabaa",
            "abbbabbbaababbbaaabbabba",
            "baaabaabaababbbbabbbbabbaaaaabababbaabbabbaaaaababaababb",
            "aaaabaaaaaaabababbbabaab",
            "bbaaabbaaabbbaaabbbaabbbabbbbbbbbabbbaaabbabbaab",
            "aabaabaabbaabaaabbabbabbabbaabbababaababbababbbababbabbbbbaaaaaa",
            "bbbababbbbabaaabbbababaa",
            "aaaaaaababbbaabbbabbaabbbabbbbaa",
            "bbbaabaabbabbbbaabbababbaaabaaaaaaabaabaaaaababbaabaaaaababaaabbaaabaabaabbaaaaa",
            "babaaaaabbaaaababaabbbba",
            "aabbababbbbababbabbababbbbbabaab",
            "ababaaabbbbaaaaabbaabbbb",
            "bbaaaabaaabaabbabbbbbbbb",
            "baababbbbbaabbaabbaabbba",
            "ababaabbaaababbabbabaaaabbaabbbbbabaabaa",
            "aabbbbbbbbbbbbabbaabaaab",
            "bbbaaaababaabbababbaaaaa",
            "abbbbbbaababaaaabaaabbba",
            "aaaababaababaaabbbbabaaa",
            "aaabababababaabbabbaabbb",
            "abbbabbaaabbaaaabbbababa",
            "baabbbbbbaababbaabaaaaba",
            "ababaabbbbaaaabbaaabaaabbbbbabaa",
            "baabbbababaabaabaabaaaabbbbaaabaabbabbaaabbababb",
            "babbbaaaabaabaabbabbaaab",
            "bbaabbbabaabaaabaababbabbaaaabbbbbaabbbb",
            "abbbbabaaabbbbbababbabbb",
            "aababbaaaabababbbbababaabbabbaaabaaabaabababbaaaabbbbbaabaabaabb",
            "aaaabaaaaabbbbabbababbab",
            "aaaaabbbaaaaaaabbaaabbba",
            "bbaababbbaaaaaabaababaaa",
            "aaabaaaaaaababababaaabaaabaabbaa",
            "abbbaabbbbbaaabbababaaabbabbbababababbbb",
            "aabbbbabbababbabbbbaaaba",
            "baababbbababbbbbbabbbbba",
            "abababbaaaabbabbaababbbabbbaabaabbaaaaaabbabbbba",
            "aabaabbbbbbbbababbbabbbbababbbaabbaaaaaaaaabaababbaabaab",
            "babaaaababbbababbbaabbba",
            "aabbbbaabaabbbbbbababaaa",
            "bbabbbababbaaaabbbaababbbababababbaaabaa",
            "ababbbbbbbaabaaaabababbb",
            "bbbbbabaababaababaaabbaabaaabbaababbabbb",
            "babaababbabababaaaaabbab",
            "abbaabbaaaaaabbbbbbababa",
            "bbababbaaabbaababaaabaaa",
            "abaabbbaababaaabbbaaaababbaaaabaaaabaabbbbababaa",
            "abaaababbaabbaabaaaaabba",
            "abaababaabbabbbbabaaaabb",
            "aaaabaabaaaaabbababababbabaaaabbbbbabbaaaababaabaaaaaaab",
            "aaabaaaaaabbabbbbaaaabab",
            "aabaababbabbbbbbaaaaaaababbaabaabbbbbabaababbaabbbaaaaabbaaababa",
            "aabbbabaaabbababbaabaabb",
            "aababbbbbabbbbabaababaab",
            "babbabaabaaababbaababbaaabbababbbbbaabaabbabbaab",
            "aaaaaabaabababaabaaabaababbaaaaabbbabababbbabaaabbbbabbabbaaaaba",
            "aabbbbabbbbbbaabbbbabaab",
            "aaaaabbbbaabbaaabaabaababaaabaab",
            "aabbbbbbaababbbbaaaaabbaaabbbababbbbababbaaabbbbbaaabaabaabaabaa",
            "babbababbabaabbbbbbbabaa",
            "babbababbbabaabaaaabaaaababbbbabaaabaabb",
            "aaaaaabbbaabbabaabbabbab",
            "babaababaabaabbbbaaaabba",
            "baaabbbbababaaabaabaaaabababbababbaabbabbbbaaaabaaabbabababbaaaa",
            "babbabaabaababbbbbbbbaba",
            "babaaaaabababbaaaaababba",
            "bbbbbababaabbaabbaaaabbbabbabbabbaaaabaaababbbbbaababaaabbaaaaabbaaaabbb",
            "bbbaababaaaaaaabaaabaaaababbbbabbabbbaaa",
            "abaaabaabbbbabbbbbbababa",
            "abaabbbbabbabbaababbbaba",
            "ababbaaaaaaaabbabbaaaaaa",
            "baaabbabbbabbababaabaabababbaabaaabbbaabaaaaabaababbabbabbbbaaaaaabaaabbabaaabbabbbaabab",
            "bbaaaababaabbaaabbbbbbba",
            "ababaabaaaababbaaaababbb",
            "abaaababbaaabaababbababbbaabbaaabbbababa",
            "abaaabaaabbaaabbbbabababbabbaabbaaaabaaa",
            "ababaababbababbbababbbba",
            "bbabaaabbbaaababaaaababaabaababa",
            "aabbaababbabaaaabaaababbbbabbbababbbbabbbababbbaaababaaaabaaaabb",
            "babbaabbbbabbabaabbbbaabbaaabaaabbaabbaa",
            "baaaaabbaaabbbbbabbabbaaabbbbbabaababbab",
            "ababbbbbbbbbbabaaabaaabb",
            "aababbbaaaaaababbabbbbbaabbabbba",
            "aabaabaabbaaaabbbaabbaaabbabbbabbbbabaaa",
            "babbaabaaaabbbbbaaaabbbaabaabaaaabaababa",
            "baabbaaaabbaaabbaaaaaaababbbbbbb",
            "bbabbbaaababbabbabbbbabababbaaaa",
            "aabaababbabbabaabbaaaababaabbbba",
            "aaabbbbabbbbbbbbbabababb",
            "baaaaababaababbbabaaaaabaaabaaaababaabbbaaabaabbabbabaabababbabaaabbbbaa",
            "bbaaababbbaaababbbabbbaaabbaabbaabbaaabbaaaaaaaa",
            "baabbaaabbbaaaabbbbababbbaaababa",
            "babaabbbabbababbbababaab",
            "bbabaaababbabaaaaaabbbaaaaababbabaabaaaabaaaaaaa",
            "abbaaabbbabbabaababbbaaa",
            "abaabbbbabaababbbababaab",
            "baaabbabbababbaaaaabbaab",
            "bbbababbabbaabbabbbbbbabbabaaabbaaabbabababbabaaaabbbaaaaaaaaaaa",
            "bbababaaaabbbabaabbbbaabbababbaababbabbabbbbaaaabbbbbbabbbbbabbabbbbbbaaabaaabba",
            "abbaaabbbbbbbbabaaabaabb",
            "bbbbbbbbaabbbaabbbbabbba",
            "aababbaaaaabaaabbabaaaab",
            "baabbaabbaabbabbabbbaaba",
            "abbababbbbaababbabaaabbbaaaaabbbabbababa",
            "aabaabbabbaababbbbbbaabbaaabbaaa",
            "bbaaababbaabbbbbaaaabaaaababaabbbabbbababbbabaab",
            "bbbaaabbaaabaaaababbbbabbbbaaababababaab",
            "bbaababbaabaaaaababaababaabbaaaabaaabbaaaabbabbabbaabbaa",
            "aabbbabbaaaabbbaabbbaaaa",
            "baabbababbababababbbbbba",
            "bbabbbbbabbaabaabaaaabbaaaabbaba",
            "babaaaaaaaabaaaaabbaabab",
            "bbbbbaaabaaabaaabbbabbbaaabbaaabbababbab",
            "baaababbaabbabbbbabbbaaa",
            "ababbabbabbbaabbaabbbbabbbbbaaab",
            "aabbbbbabaaabbbbaabaaaab",
            "aaabaaaaaabbabbbaaabbbbaabaaaabbaabbaabb",
            "bbbaabaaabaaabababaabaaa",
            "baabbaaaabaaabaabbaaaabababaabbabbaaabaa",
            "baabbababbbaaabbaaaaaaaa",
            "aabbaababbbbbaababbabbba",
            "bbabbbaaaabbbabaabbbabbabbaababbbbabbbaaababbaaaaaaaaaaaaaaaabab",
            "abaabbabababbbabbaababbaaaaabaabaaaabaaaabaaabbababbbaba",
            "ababbbaaaabbbabaabbabaaa",
            "baaabaabbaababbaaaaabbab",
            "baaabaabbabababaaabaabbbbabaabaa",
            "aaaabababbbabbbbababbbba",
            "aabbbbababbbaabbbabbbbabbbbaaaabbbabbabbaabababb",
            "babaabaabbbbbaabaaaaabbbabababbaaaaabababaabbaaababbabba",
            "bbabbabaabaababbbbbabbba",
            "abbababbabbabbaabbaaaaaa",
            "babbbbbbabbabaababbbabbaaababbabbbbababa",
            "aabbaabaaaaaaababaaabaabbaabbabaababbabaabababaabaaababa",
            "baabaaaaaababbbaabbbabaa",
            "abaaabaabbaaababbbaabbbb",
            "aabbbbababaaabaaababaaabbabbbbababbabababbaabbbaaaaaabba",
            "aabbbaababaaabbbbabbaaab",
            "abbabbbbbaaaaabababaabababbabbbaaaabbbabbaabababababbbbbaabaababaabbabbbabbbababaaabbbab",
            "bbabbabbbababbaaaabbabaa",
            "abaaaaabaabababbbbbbaaaabaabbaaababababb",
            "baaaaaaaaaabbabbaaabbbbbabaabaababbbaababbbbbbabababbbbbaabbabbb",
            "bbbbbbbbbaabbabbbbabbababbaababababaabba",
            "baabbabbbbbbaabbbaabbabbbbbbbabbabbabbbb",
            "abbbbababbbbbabaabababab",
            "abababbabbaaaabbbbaababbaabbbbaaaabaaaaa",
            "baababbbaaaabbbaaabaabbabbabbbbbabaaababbbaabbba",
            "bbababbbabaabbbbababbababaaaaabaaabaabbaaababbbbbabaabbbaaaabbaabbbaabbaaaabbbbb",
            "aaaabaabaabbbbababbababa",
            "aabbbbbaababaabbabbbbabbbaabbababbaaaabbbababbba",
            "baaabbbbbbbaabaaaaaaabbbbaabbabbaabbbaaabbaababa",
            "aaababbaabbaabbaaaabbbaa",
            "bbbbbaabababbabbabbbbbab",
            "baabbbbbbbaababbaabbabaa",
            "aabaabbaaabbbabbababaaaa",
            "bbbabbbbaaaabaaaababbbaabaaaaaaa",
            "bbabbbbbbbababbabbaaaabbabbbbbaa",
            "baaaaabbbaabbbbbaababbab",
            "bbbaaaaababbabaabbabaabb",
            "abbbaaabbaabbaaababaaaab",
            "aabbababaabababaabbabbaaaaaaaaaaababaaaa",
            "aaaabbbaaaaaaaabbbbaabaaababbabbbbaaabaaababbabaaaababaa",
            "bbbabbbabbabaabaaabbbaaaabbaabaa",
            "abbbbaaaabaaaaaaaaabbabbbbabbbabbababaaa",
            "aabbaababbbbbbbbaaabbbab",
            "babaabbbaabbbbbbaabbaabb",
            "aababbbaaaabaaabaaaabbab",
            "aaaababaabaabbabaaaaabaabbabbbbbababbaaa",
            "bbabbbaababbbbabaabaaabb",
            "ababaabbbbbbbbbbbaaaabbb",
            "baabbaaabaaababbbabbbabb",
            "baabbbbbababaaabaabbbbabbbabbabb",
            "aabbaabaababaabbabbababa",
            "abbbabbabaabbaabbbabaabb",
            "ababbabbbbabbbaabaaaabba",
            "bbbbbbaaabbbbbababaababaaabbbbbbbbabbaaa",
            "bbbbbaababaababbaaaaabaababababbbbbabbba",
            "abbaaabbbaaabaabbaaaabba",
            "baaabbabbababbabaaabbaaa",
            "abaabbbabbabbabaabbbbaab",
            "bbbaaaaabbbababbaabaaabb",
            "bbabaaabbbbbbabababbabbb",
            "aaababbabbbbbabaaabbbaaa",
            "abbbaabbbaaabaababbabbab",
            "baaabbbbaabbbababbabbababbaaabaa",
            "abbabbaaaabaababbbbabbabaaabbbaa",
            "abbababbbabaabbbaaabbbbbaababbaababbabbbbabaabbaaaabaaba",
            "baaabaabbbbbbbbbabbbaaba",
            "ababaaabbaabbabbbaaaabaaabbaabbbaababaab",
            "abbbabbaabbabaabbabaabbbbbababaabaaabaaa",
            "baabbaaaaaaabaaaaaaaabab",
            "aaaaabaabbabababbbbabaaa",
            "aabbbaabbabbabaababbbbababbbababbaaaabbabbabbaaababbabbb",
            "bbbbabbabaabbaabbbaaababaaabbabbbbabbaaababaabbababbabba",
            "abaaabbbaaaaabaababbbaaa",
            "aaaabbbaaabababbbabbababbbabababbbabbabbababbbaa",
            "baaaaabbbabaabbbabbbbaab",
            "ababbbaabbabbaabbbbaaaabbbabbaabaababbabbaaaaaaaaaaaabbbbbaabbbaaaabaaab",
            "baababbabbbbbaaabbaabbba",
            "baaabbaabbaabaaaabbbabaa",
            "baabbaabaaabbbbbbabbabaaaaabaaba",
            "aabbabbbbbababbbbbabbaab",
            "bbabbbaaabbaaabbaaabababaaaaabbbbaaabbba",
            "baabaaaabaaabaabbaabbaaaaabbaaabaabbaabb",
            "baabaaaabbbaaaabbbaaabaa",
            "aaabbbbbbabaaaaaaaababbababaabbbbabababbbababbbbbaaaaaba",
            "bbabaaabbabbbbbbbaababbaabbaabbb",
            "baabbabbbbbbbbbbbaaaabba",
            "abaaababbbabbabaabaababb",
            "bbbabbbbaababababaababab",
            "bbbbbaaababaaaaaaabaababbbbabbbabbaaaaab",
            "aabbbaabaaaaabbbbbabbaaa",
            "aaabaaaababbaabbaabbaaaabbbbaabbbbaabbaa",
            "abababbaaabbbabbabbbabaa",
            "aabbbababbaaaabbaaaaaaabbbaababbbabbbabaabbbabababbaabab",
            "bababbaabababbaabbbbbbba",
            "bbababababbaabaaaaaaaaaa",
            "ababbbbbaabbabababaabaab",
            "bbbabbbabbabbaabbabbbabbabaaabbabbaaabbabbbbabaa",
            "bbabaaabbbbbbaaaaabbbabbabbaababbbbbbabbabbbbbbbaaaaaaaa",
            "ababaaababbabbaabbabbababbaaaaab",
            "abbbbbababababbbabaabaaabaaabababbabbaba",
            "abaabababaabbaaabbaababbbaaaabbaabaaababbbbbaaababbaaaaaaabbabbababbbaba",
            "bbbaaabababaaaaabaabbabbaabbaaabbaaaababaaabaabbbaabbabb",
            "abbabaabababaabbbbaaabbb",
            "aabbbaabbaabaaaabababaaa",
            "abaabbabaaaaaaababbbaaababaaaaaaabbbbaaabaaaaaaabaabbbba",
            "abbaabbabaabbaaabbabbaaa",
            "bbbbabbbabaababbbbababaa",
            "babbbbabbbbaaaaabbbaabab",
            "aabbababbabbbbbbbbaaaaab",
            "babaaabaaabaabbabababbbbabbabbbb",
            "aaabababbbbababbbbbaabaa",
            "aababbbbabbbbabbaabbbaaa",
            "abaaabbbbabbababbaabaaba",
            "abbbbaaaababaababbbbbaababbbabbabaaaabbabaaabaaababbbaab",
            "abbaabaaaaaabaabbbbaaaba",
            "abaabbbbbbaaababbaabbabaababaaababaabbabaaabaaababaaaaba",
            "aabbbabbbaaabaabbbaaabaa",
            "abbbbabbaabbbbaaabaaaababaabbbba",
            "aaaabababbbbaabbabbaabbb",
            "abaabbabbbbabbaababaabbaabababab",
            "aaababbababbbbbbbaababaa",
            "aaaabbbaaababababaaababbbbaabbbaabbbbbaa",
            "aababbababaabbbbabbaaaabaabbaabaaabaabaabbbbabbaababababaaabbbabbbbbabaabbbaabababaaabba",
            "baaabbbbbbbbabbabbabbaab",
            "bbabbbabbabaaaaaabababbaabbaabaaabbabbaabaababaabaabbbaa",
            "baabbaabaaabbabbaaabaaaabaaaaababbaaabaa",
            "abbaaabbbabaaaaabbbabaaa",
            "bbabaaabbabbaababababaaa",
            "bbbabbbbabbbaabbbbabaabb",
            "abbaaabaabaaababbbaabbabbbbaabaaabbbbbaabaabaabb",
            "aaabbabbbbbabbbbaabbababbabbabaabaabbbaabaaababa",
            "baaabbaabaababbabbbbaaba",
            "bbbbbababaabbaababaaaaba",
            "baaabbaabbbaabbbaababaaabbbabaababbabbbb",
            "abaabbbbabbbabbbbaaaabbb",
            "bbbabbbbbabbababaaabaabb",
            "baaababbbbbbbaabaabbbbabbabbaababbababbaabbaabbb",
            "aababbbaaabbbbbbaababbaabbaababbbaaabababbaabbbbbabababaaabaabbaababbbbb",
            "bbbbbbbbbbaaaababaaabbbbabaababbbabbaaab",
            "aababbbbabbaabbabaaabaaa",
            "abbbbabbbbabababaabbaaab",
            "bbabaababbbbbbaaababaabbbbbabaababaaaaabbbabbbababbbbbaa",
            "bbbaabaabbbbbaabbbaabaab",
            "aabbbbbbbbbbbbbbbbababbaaababbbaaaabaabb",
            "aabbbabababababaaabaababaaabbabbaabbbababbabbabaabaaabba",
            "abaababbbbbbabbababbaaab",
            "babbbbabbbaababbbbbbbbaa",
            "aaabbaaabbbbabaabbaabaab",
            "bbababbabaababbaabbbaabb",
            "abbaabaabbbaabaabbbbbaabbbbbaaba",
            "bbbaabaaabbabaabbbabaaaababbbaba",
            "ababaababbababbbbbbbababbabbabbaabbbaabaabaabaaa",
            "baaaaaababaaabaaabaababa",
            "ababaababbbaabaabaababab",
            "aabbbbbbbbabbabbabbaabbb",
            "abaabbbbbbabbabaaabaaaaa",
            "bbbbbaabaabababaabbaaaabababaabbaabbaabbabbbabab",
            "abbaaabaabaaababbaabaaab",
            "bbbbbbabbaaaaaabaabaabaabbbaaaabbbaabbbb",
            "babaaaaaaabbbbbbabababbb",
            "abaaaaaaabbababbbbbbaaaa",
            "bbbbabbbbaabbaaaabbbabbbabaaaaababaabaaa",
            "aaaaabaabababbabbbaaaaab",
            "abbababbbaaabbabbaabaabbaabbababbbaaabbbbbbbbbaabbbabaab",
            "abbabbaabbabbbababaabaaa",
            "baabbaabbbbbbbbbbbbaaaba",
            "aaaaababaaabaaaaabbbaabbabaaabaabaabbabbaabaaabababbababababaaabbabaabba",
            "bbabaababbbaaabbaaaaaabbbaaababa",
            "aaabaaaaaaaaabaaaabbabaa",
            "abbbabbbaababbbababaaaab",
            "aabbbbabaabbbbaaabbbbaaaaabbbabaaabababaaaaaaaaa",
            "baaaaaabbaaababbababaababbbaabbb",
            "baabbbbbbbbbbababbaabaaabbabbaaaabbabbbb",
            "baaabbabbaabbbbbbbbbbabaabbbabaabaabbbabbbbabbbababbbaba",
            "abbbbaaabbababbbababbbabbbbbbaaaaabbaaababaaaaabbbaabbbb",
            "aaaaabaaaaaabbbaababbbabbbabbbba",
            "bbbbabbbaabababaaaabbaab",
            "baabaaaabababbabbaaabbba",
            "bbaabaaaaabbababaabbabaa",
            "aabbababbbaaaabaaaaaabbbaaaaabbaababbaba",
            "abbbaabbbbaaaabababbbaab",
            "abaaababababaabbbbaabbba",
            "aabbaaababaabbaabbabbbbababaaaabbaaaaaba",
            "bbbbbbbbbbbbbaaaabbbbababbaaaababbbbbaabaaabbaabbabbaaababbbbbbaabbbabab",
            "abbabababbabbbbaaabaaabbaabbabba",
            "aaabbbbaababbabbbbabbbab",
            "bbbaaaaaabbbbaaaaabaabbabaaababbababbaab",
            "bbbababbbbbbbaaaaabbabba",
            "bbaaaabbbbbbbbbbaaaaabbbbbbabbbaaabababb",
            "aaabbbbaaaaaaabbbaabbaabbaabbabbaabaabaabaaaabaabababaabbbbabaaa",
            "abababbabbababbaabbababbbabbbaba",
            "aabbbabbabbbaaabaabbbaaa",
            "baaabbaabbbaaaabbaabaaab",
            "baabaaaabbbbbababaabaababbabaaabbbbabbbbbbaaabbbbaaaaaaabbbaabbb",
            "bbabaaabbbabbababababababaaaabab",
            "babababaabbbabaabbbaaaba",
            "abbaabaabbabaabaaababbab",
            "bbaaaababaaabbaaabaaabba",
            "bbabbbbbaaabbbbaaaabbaba",
            "aabaabababaabbbbababbaaa",
            "babaabbbbbababbaaababbab",
            "abbbbbbbbaaabbbababbbbbabababbbb",
            "babbabaabbbbbaabbbbbbbba",
            "abbbaabbaabbbaabaabbabaa",
            "aaaaaabaaabaabbabbaaaabbaabaabbbbbbabbab",
            "bbbaaabbaaabababaabbbaaa",
            "bbbaaaaaaabbbabbaabbaaab",
            "abaaaaaabbbbaabbbbabaaaaaabaabbbaaaaabbbbbaaaaaa"
        };
#endif
#endregion Data
    }
}
