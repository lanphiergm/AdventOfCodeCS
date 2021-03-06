﻿// ############################################################################
// # Galen Lanphier                                                           #
// # https://github.com/lanphiergm/AdventOfCodeCS                             #
// # MIT License. See LICENSE file                                            #
// ############################################################################

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Puzzles.Year2015
{
    /// <summary>
    /// Day 5: Doesn't He Have Intern-Elves For This?
    /// https://adventofcode.com/2015/day/5
    /// </summary>
    [TestClass]
    public class Day05DoesntHeHaveInternElvesForThis
    {
        /// <summary>
        /// Part 1 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part1_SampleInput()
        {
            Assert.AreEqual(2, ExecutePart1(sampleInput1));
        }

        /// <summary>
        /// Part 1 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part1_PuzzleInput()
        {
            Assert.AreEqual(255, ExecutePart1(puzzleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part2_SampleInput()
        {
            Assert.AreEqual(2, ExecutePart2(sampleInput2));
        }

        /// <summary>
        /// Part 2 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part2_PuzzleInput()
        {
            Assert.AreEqual(55, ExecutePart2(puzzleInput));
        }

        /// <summary>
        /// Executes part 1 of the puzzle
        /// </summary>
        /// <param name="strings">The strings to evaluate</param>
        /// <returns>The number of nice strings</returns>
        private static int ExecutePart1(string[] strings)
        {
            int niceCount = 0;

            foreach (string str in strings)
            {
                if (GetVowelCount(str) >= 3 && HasPair(str) && !HasForbidden(str))
                {
                    niceCount++;
                }
            }

            return niceCount;
        }

        /// <summary>
        /// Executes part 2 of the puzzle
        /// </summary>
        /// <param name="strings">The strings to evaluate</param>
        /// <returns>The number of nice strings</returns>
        private static int ExecutePart2(string[] strings)
        {
            int niceCount = 0;

            foreach (string str in strings)
            {
                if (HasDoublePair(str) && HasSeparatedPair(str))
                {
                    niceCount++;
                }
            }

            return niceCount;
        }

        /// <summary>
        /// Counts the number of vowels
        /// </summary>
        /// <param name="str">The string to evaluate</param>
        /// <returns>The number of vowels</returns>
        private static int GetVowelCount(string str)
        {
            return str.Count(c => "aeiou".Contains(c));
        }

        /// <summary>
        /// Whether or not there is a pair of letters in the string
        /// </summary>
        /// <param name="str">The string to evaluate</param>
        /// <returns>true if there is a pair of letters; otherwise, false</returns>
        private static bool HasPair(string str)
        {
            for (int i = 0; i < str.Length - 1; i++)
            {
                if (str[i] == str[i+1])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Whether or not there are at least two non-overlapping pairs of letters in the string
        /// </summary>
        /// <param name="str">The string to evaluate</param>
        /// <returns>true if there is a double pair present; otherwise, false</returns>
        private static bool HasDoublePair(string str)
        {
            for (int i = 0; i < str.Length - 3; i++)
            {
                string pair = $"{str[i]}{str[i+1]}";
                if (str[(i + 2)..].Contains(pair))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Whether or not there is a pair separated by one other letter in the string
        /// </summary>
        /// <param name="str">The string to evaluate</param>
        /// <returns>true if there is a separated pair; otherwise, false</returns>
        private static bool HasSeparatedPair(string str)
        {
            for (int i = 0; i < str.Length - 2; i++)
            {
                if (str[i] == str[i+2])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Whether or not there are any forbidden strings present
        /// </summary>
        /// <param name="str">The string to evaluate</param>
        /// <returns>true if there is a forbidden string; otherwise, false</returns>
        private static bool HasForbidden(string str)
        {
            foreach (string forbidden in forbiddens)
            {
                if (str.Contains(forbidden))
                {
                    return true;
                }
            }
            return false;
        }

        private static readonly List<string> forbiddens = 
            new List<string>() { "ab", "cd", "pq", "xy" };

        #region Data

        private static readonly string[] sampleInput1 =
        {
            "ugknbfddgicrmopn",
            "aaa",
            "jchzalrnumimnmhp",
            "haegwjzuvuyypxyu",
            "dvszwmarrgswjxmb"
        };

        private static readonly string[] sampleInput2 =
        {
            "qjhvhtzxzqqjkmpb",
            "xxyxx",
            "uurcxstgmygtbstg",
            "ieodomkazucvgmuy"
        };

        private static readonly string[] puzzleInput =
        {
            "sszojmmrrkwuftyv",
            "isaljhemltsdzlum",
            "fujcyucsrxgatisb",
            "qiqqlmcgnhzparyg",
            "oijbmduquhfactbc",
            "jqzuvtggpdqcekgk",
            "zwqadogmpjmmxijf",
            "uilzxjythsqhwndh",
            "gtssqejjknzkkpvw",
            "wrggegukhhatygfi",
            "vhtcgqzerxonhsye",
            "tedlwzdjfppbmtdx",
            "iuvrelxiapllaxbg",
            "feybgiimfthtplui",
            "qxmmcnirvkzfrjwd",
            "vfarmltinsriqxpu",
            "oanqfyqirkraesfq",
            "xilodxfuxphuiiii",
            "yukhnchvjkfwcbiq",
            "bdaibcbzeuxqplop",
            "ivegnnpbiyxqsion",
            "ybahkbzpditgwdgt",
            "dmebdomwabxgtctu",
            "ibtvimgfaeonknoh",
            "jsqraroxudetmfyw",
            "dqdbcwtpintfcvuz",
            "tiyphjunlxddenpj",
            "fgqwjgntxagidhah",
            "nwenhxmakxqkeehg",
            "zdoheaxqpcnlhnen",
            "tfetfqojqcdzlpbm",
            "qpnxkuldeiituggg",
            "xwttlbdwxohahwar",
            "hjkwzadmtrkegzye",
            "koksqrqcfwcaxeof",
            "wulwmrptktliyxeq",
            "gyufbedqhhyqgqzj",
            "txpunzodohikzlmj",
            "jloqfuejfkemcrvu",
            "amnflshcheuddqtc",
            "pdvcsduggcogbiia",
            "yrioavgfmeafjpcz",
            "uyhbtmbutozzqfvq",
            "mwhgfwsgyuwcdzik",
            "auqylgxhmullxpaa",
            "lgelzivplaeoivzh",
            "uyvcepielfcmswoa",
            "qhirixgwkkccuzlp",
            "zoonniyosmkeejfg",
            "iayfetpixkedyana",
            "ictqeyzyqswdskiy",
            "ejsgqteafvmorwxe",
            "lhaiqrlqqwfbrqdx",
            "ydjyboqwhfpqfydc",
            "dwhttezyanrnbybv",
            "edgzkqeqkyojowvr",
            "rmjfdwsqamjqehdq",
            "ozminkgnkwqctrxz",
            "bztjhxpjthchhfcd",
            "vrtioawyxkivrpiq",
            "dpbcsznkpkaaclyy",
            "vpoypksymdwttpvz",
            "hhdlruwclartkyap",
            "bqkrcbrksbzcggbo",
            "jerbbbnxlwfvlaiw",
            "dwkasufidwjrjfbf",
            "kkfxtjhbnmqbmfwf",
            "vmnfziwqxmioukmj",
            "rqxvcultipkecdtu",
            "fhmfdibhtjzkiqsd",
            "hdpjbuzzbyafqrpd",
            "emszboysjuvwwvts",
            "msyigmwcuybfiooq",
            "druyksfnbluvnwoh",
            "fvgstvynnfbvxhsx",
            "bmzalvducnqtuune",
            "lzwkzfzttsvpllei",
            "olmplpvjamynfyfd",
            "padcwfkhystsvyfb",
            "wjhbvxkwtbfqdilb",
            "hruaqjwphonnterf",
            "bufjobjtvxtzjpmj",
            "oiedrjvmlbtwyyuy",
            "sgiemafwfztwsyju",
            "nsoqqfudrtwszyqf",
            "vonbxquiiwxnazyl",
            "yvnmjxtptujwqudn",
            "rrnybqhvrcgwvrkq",
            "taktoxzgotzxntfu",
            "quffzywzpxyaepxa",
            "rfvjebfiddcfgmwv",
            "iaeozntougqwnzoh",
            "scdqyrhoqmljhoil",
            "bfmqticltmfhxwld",
            "brbuktbyqlyfpsdl",
            "oidnyhjkeqenjlhd",
            "kujsaiqojopvrygg",
            "vebzobmdbzvjnjtk",
            "uunoygzqjopwgmbg",
            "piljqxgicjzgifso",
            "ikgptwcjzywswqnw",
            "pujqsixoisvhdvwi",
            "trtuxbgigogfsbbk",
            "mplstsqclhhdyaqk",
            "gzcwflvmstogdpvo",
            "tfjywbkmimyyqcjd",
            "gijutvhruqcsiznq",
            "ibxkhjvzzxgavkha",
            "btnxeqvznkxjsgmq",
            "tjgofgauxaelmjoq",
            "sokshvyhlkxerjrv",
            "ltogbivktqmtezta",
            "uduwytzvqvfluyuf",
            "msuckpthtgzhdxan",
            "fqmcglidvhvpirzr",
            "gwztkqpcwnutvfga",
            "bsjfgsrntdhlpqbx",
            "xloczbqybxmiopwt",
            "orvevzyjliomkkgu",
            "mzjbhmfjjvaziget",
            "tlsdxuhwdmghdyjb",
            "atoecyjhwmznaewi",
            "pyxpyvvipbqibiox",
            "ajbfmpqqobfsmesj",
            "siknbzefjblnohgd",
            "eqfhgewbblwdfkmc",
            "opylbscrotckkrbk",
            "lbwxbofgjkzdxkle",
            "ceixfjstaptdomvm",
            "hnkrqxifjmmjktie",
            "aqykzeuzvvetoygd",
            "fouahjimfcisxima",
            "prkzhutbqsyrhjzx",
            "qqwliakathnsbzne",
            "sayhgqtlcqqidqhj",
            "ygduolbysehdudra",
            "zricvxhdzznuxuce",
            "ucvzakslykpgsixd",
            "udirhgcttmyspgsb",
            "yuwzppjzfsjhhdzi",
            "gtqergjiuwookwre",
            "xvxexbjyjkxovvwf",
            "mlpaqhnnkqxrmwmm",
            "ezuqbrjozwuqafhb",
            "mcarusdthcbsonoq",
            "weeguqeheeiigrue",
            "pngtfugozxofaqxv",
            "copphvbjcmfspenv",
            "jiyahihykjjkdaya",
            "gdqnmesvptuyrfwp",
            "vbdscfywqmfxbohh",
            "crtrfuxyjypzubrg",
            "seihvevtxywxhflp",
            "fvvpmgttnapklwou",
            "qmqaqsajmqwhetpk",
            "zetxvrgjmblxvakr",
            "kpvwblrizaabmnhz",
            "mwpvvzaaicntrkcp",
            "clqyjiegtdsswqfm",
            "ymrcnqgcpldgfwtm",
            "nzyqpdenetncgnwq",
            "cmkzevgacnmdkqro",
            "kzfdsnamjqbeirhi",
            "kpxrvgvvxapqlued",
            "rzskbnfobevzrtqu",
            "vjoahbfwtydugzap",
            "ykbbldkoijlvicbl",
            "mfdmroiztsgjlasb",
            "quoigfyxwtwprmdr",
            "ekxjqafwudgwfqjm",
            "obtvyjkiycxfcdpb",
            "lhoihfnbuqelthof",
            "eydwzitgxryktddt",
            "rxsihfybacnpoyny",
            "bsncccxlplqgygtw",
            "rvmlaudsifnzhcqh",
            "huxwsyjyebckcsnn",
            "gtuqzyihwhqvjtes",
            "zreeyomtngvztveq",
            "nwddzjingsarhkxb",
            "nuqxqtctpoldrlsh",
            "wkvnrwqgjooovhpf",
            "kwgueyiyffudtbyg",
            "tpkzapnjxefqnmew",
            "ludwccvkihagvxal",
            "lfdtzhfadvabghna",
            "njqmlsnrkcfhtvbb",
            "cajzbqleghhnlgap",
            "vmitdcozzvqvzatp",
            "eelzefwqwjiywbcz",
            "uyztcuptfqvymjpi",
            "aorhnrpkjqqtgnfo",
            "lfrxfdrduoeqmwwp",
            "vszpjvbctblplinh",
            "zexhadgpqfifcqrz",
            "ueirfnshekpemqua",
            "qfremlntihbwabtb",
            "nwznunammfexltjc",
            "zkyieokaaogjehwt",
            "vlrxgkpclzeslqkq",
            "xrqrwfsuacywczhs",
            "olghlnfjdiwgdbqc",
            "difnlxnedpqcsrdf",
            "dgpuhiisybjpidsj",
            "vlwmwrikmitmoxbt",
            "sazpcmcnviynoktm",
            "pratafauetiknhln",
            "ilgteekhzwlsfwcn",
            "ywvwhrwhkaubvkbl",
            "qlaxivzwxyhvrxcf",
            "hbtlwjdriizqvjfb",
            "nrmsononytuwslsa",
            "mpxqgdthpoipyhjc",
            "mcdiwmiqeidwcglk",
            "vfbaeavmjjemfrmo",
            "qzcbzmisnynzibrc",
            "shzmpgxhehhcejhb",
            "wirtjadsqzydtyxd",
            "qjlrnjfokkqvnpue",
            "dxawdvjntlbxtuqc",
            "wttfmnrievfestog",
            "eamjfvsjhvzzaobg",
            "pbvfcwzjgxahlrag",
            "omvmjkqqnobvnzkn",
            "lcwmeibxhhlxnkzv",
            "uiaeroqfbvlazegs",
            "twniyldyuonfyzqw",
            "wgjkmsbwgfotdabi",
            "hnomamxoxvrzvtew",
            "ycrcfavikkrxxfgw",
            "isieyodknagzhaxy",
            "mgzdqwikzullzyco",
            "mumezgtxjrrejtrs",
            "nwmwjcgrqiwgfqel",
            "wjgxmebfmyjnxyyp",
            "durpspyljdykvzxf",
            "zuslbrpooyetgafh",
            "kuzrhcjwbdouhyme",
            "wyxuvbciodscbvfm",
            "kbnpvuqwmxwfqtqe",
            "zddzercqogdpxmft",
            "sigrdchxtgavzzjh",
            "lznjolnorbuddgcs",
            "ycnqabxlcajagwbt",
            "bnaudeaexahdgxsj",
            "rlnykxvoctfwanms",
            "jngyetkoplrstfzt",
            "tdpxknwacksotdub",
            "yutqgssfoptvizgr",
            "lzmqnxeqjfnsxmsa",
            "iqpgfsfmukovsdgu",
            "qywreehbidowtjyz",
            "iozamtgusdctvnkw",
            "ielmujhtmynlwcfd",
            "hzxnhtbnmmejlkyf",
            "ftbslbzmiqkzebtd",
            "bcwdqgiiizmohack",
            "dqhfkzeddjzbdlxu",
            "mxopokqffisxosci",
            "vciatxhtuechbylk",
            "khtkhcvelidjdena",
            "blatarwzfqcapkdt",
            "elamngegnczctcck",
            "xeicefdbwrxhuxuf",
            "sawvdhjoeahlgcdr",
            "kmdcimzsfkdfpnir",
            "axjayzqlosrduajb",
            "mfhzreuzzumvoggr",
            "iqlbkbhrkptquldb",
            "xcvztvlshiefuhgb",
            "pkvwyqmyoazocrio",
            "ajsxkdnerbmhyxaj",
            "tudibgsbnpnizvsi",
            "cxuiydkgdccrqvkh",
            "cyztpjesdzmbcpot",
            "nnazphxpanegwitx",
            "uphymczbmjalmsct",
            "yyxiwnlrogyzwqmg",
            "gmqwnahjvvdyhnfa",
            "utolskxpuoheugyl",
            "mseszdhyzoyavepd",
            "ycqknvbuvcjfgmlc",
            "sknrxhxbfpvpeorn",
            "zqxqjetooqcodwml",
            "sesylkpvbndrdhsy",
            "fryuxvjnsvnjrxlw",
            "mfxusewqurscujnu",
            "mbitdjjtgzchvkfv",
            "ozwlyxtaalxofovd",
            "wdqcduaykxbunpie",
            "rlnhykxiraileysk",
            "wgoqfrygttlamobg",
            "kflxzgxvcblkpsbz",
            "tmkisflhativzhde",
            "owsdrfgkaamogjzd",
            "gaupjkvkzavhfnes",
            "wknkurddcknbdleg",
            "lltviwincmbtduap",
            "qwzvspgbcksyzzmb",
            "ydzzkumecryfjgnk",
            "jzvmwgjutxoysaam",
            "icrwpyhxllbardkr",
            "jdopyntshmvltrve",
            "afgkigxcuvmdbqou",
            "mfzzudntmvuyhjzt",
            "duxhgtwafcgrpihc",
            "tsnhrkvponudumeb",
            "sqtvnbeiigdzbjgv",
            "eczmkqwvnsrracuo",
            "mhehsgqwiczaiaxv",
            "kaudmfvifovrimpd",
            "lupikgivechdbwfr",
            "mwaaysrndiutuiqx",
            "aacuiiwgaannunmm",
            "tjqjbftaqitukwzp",
            "lrcqyskykbjpaekn",
            "lirrvofbcqpjzxmr",
            "jurorvzpplyelfml",
            "qonbllojmloykjqe",
            "sllkzqujfnbauuqp",
            "auexjwsvphvikali",
            "usuelbssqmbrkxyc",
            "wyuokkfjexikptvv",
            "wmfedauwjgbrgytl",
            "sfwvtlzzebxzmuvw",
            "rdhqxuechjsjcvaf",
            "kpavhqkukugocsxu",
            "ovnjtumxowbxduts",
            "zgerpjufauptxgat",
            "pevvnzjfwhjxdoxq",
            "pmmfwxajgfziszcs",
            "difmeqvaghuitjhs",
            "icpwjbzcmlcterwm",
            "ngqpvhajttxuegyh",
            "mosjlqswdngwqsmi",
            "frlvgpxrjolgodlu",
            "eazwgrpcxjgoszeg",
            "bbtsthgkjrpkiiyk",
            "tjonoglufuvsvabe",
            "xhkbcrofytmbzrtk",
            "kqftfzdmpbxjynps",
            "kmeqpocbnikdtfyv",
            "qjjymgqxhnjwxxhp",
            "dmgicrhgbngdtmjt",
            "zdxrhdhbdutlawnc",
            "afvoekuhdboxghvx",
            "hiipezngkqcnihty",
            "bbmqgheidenweeov",
            "suprgwxgxwfsgjnx",
            "adeagikyamgqphrj",
            "zzifqinoeqaorjxg",
            "adhgppljizpaxzld",
            "lvxyieypvvuqjiyc",
            "nljoakatwwwoovzn",
            "fcrkfxclcacshhmx",
            "ownnxqtdhqbgthch",
            "lmfylrcdmdkgpwnj",
            "hlwjfbvlswbzpbjr",
            "mkofhdtljdetcyvp",
            "synyxhifbetzarpo",
            "agnggugngadrcxoc",
            "uhttadmdmhidpyjw",
            "ohfwjfhunalbubpr",
            "pzkkkkwrlvxiuysn",
            "kmidbxmyzkjrwjhu",
            "egtitdydwjxmajnw",
            "civoeoiuwtwgbqqs",
            "dfptsguzfinqoslk",
            "tdfvkreormspprer",
            "zvnvbrmthatzztwi",
            "ffkyddccrrfikjde",
            "hrrmraevdnztiwff",
            "qaeygykcpbtjwjbr",
            "purwhitkmrtybslh",
            "qzziznlswjaussel",
            "dfcxkvdpqccdqqxj",
            "tuotforulrrytgyn",
            "gmtgfofgucjywkev",
            "wkyoxudvdkbgpwhd",
            "qbvktvfvipftztnn",
            "otckgmojziezmojb",
            "inxhvzbtgkjxflay",
            "qvxapbiatuudseno",
            "krpvqosbesnjntut",
            "oqeukkgjsfuqkjbb",
            "prcjnyymnqwqksiz",
            "vuortvjxgckresko",
            "orqlyobvkuwgathr",
            "qnpyxlnazyfuijox",
            "zwlblfkoklqmqzkw",
            "hmwurwtpwnrcsanl",
            "jzvxohuakopuzgpf",
            "sfcpnxrviphhvxmx",
            "qtwdeadudtqhbely",
            "dbmkmloasqphnlgj",
            "olylnjtkxgrubmtk",
            "nxsdbqjuvwrrdbpq",
            "wbabpirnpcsmpipw",
            "hjnkyiuxpqrlvims",
            "enzpntcjnxdpuqch",
            "vvvqhlstzcizyimn",
            "triozhqndbttglhv",
            "fukvgteitwaagpzx",
            "uhcvukfbmrvskpen",
            "tizcyupztftzxdmt",
            "vtkpnbpdzsaluczz",
            "wodfoyhoekidxttm",
            "otqocljrmwfqbxzu",
            "linfbsnfvixlwykn",
            "vxsluutrwskslnye",
            "zbshygtwugixjvsi",
            "zdcqwxvwytmzhvoo",
            "wrseozkkcyctrmei",
            "fblgtvogvkpqzxiy",
            "opueqnuyngegbtnf",
            "qxbovietpacqqxok",
            "zacrdrrkohfygddn",
            "gbnnvjqmkdupwzpq",
            "qgrgmsxeotozvcak",
            "hnppukzvzfmlokid",
            "dzbheurndscrrtcl",
            "wbgdkadtszebbrcw",
            "fdmzppzphhpzyuiz",
            "bukomunhrjrypohj",
            "ohodhelegxootqbj",
            "rsplgzarlrknqjyh",
            "punjjwpsxnhpzgvu",
            "djdfahypfjvpvibm",
            "mlgrqsmhaozatsvy",
            "xwktrgyuhqiquxgn",
            "wvfaoolwtkbrisvf",
            "plttjdmguxjwmeqr",
            "zlvvbwvlhauyjykw",
            "cigwkbyjhmepikej",
            "masmylenrusgtyxs",
            "hviqzufwyetyznze",
            "nzqfuhrooswxxhus",
            "pdbdetaqcrqzzwxf",
            "oehmvziiqwkzhzib",
            "icgpyrukiokmytoy",
            "ooixfvwtiafnwkce",
            "rvnmgqggpjopkihs",
            "wywualssrmaqigqk",
            "pdbvflnwfswsrirl",
            "jeaezptokkccpbuj",
            "mbdwjntysntsaaby",
            "ldlgcawkzcwuxzpz",
            "lwktbgrzswbsweht",
            "ecspepmzarzmgpjm",
            "qmfyvulkmkxjncai",
            "izftypvwngiukrns",
            "zgmnyjfeqffbooww",
            "nyrkhggnprhedows",
            "yykzzrjmlevgffah",
            "mavaemfxhlfejfki",
            "cmegmfjbkvpncqwf",
            "zxidlodrezztcrij",
            "fseasudpgvgnysjv",
            "fupcimjupywzpqzp",
            "iqhgokavirrcvyys",
            "wjmkcareucnmfhui",
            "nftflsqnkgjaexhq",
            "mgklahzlcbapntgw",
            "kfbmeavfxtppnrxn",
            "nuhyvhknlufdynvn",
            "nviogjxbluwrcoec",
            "tyozixxxaqiuvoys",
            "kgwlvmvgtsvxojpr",
            "moeektyhyonfdhrb",
            "kahvevmmfsmiiqex",
            "xcywnqzcdqtvhiwd",
            "fnievhiyltbvtvem",
            "jlmndqufirwgtdxd",
            "muypbfttoeelsnbs",
            "rypxzbnujitfwkou",
            "ubmmjbznskildeoj",
            "ofnmizdeicrmkjxp",
            "rekvectjbmdnfcib",
            "yohrojuvdexbctdh",
            "gwfnfdeibynzjmhz",
            "jfznhfcqdwlpjull",
            "scrinzycfhwkmmso",
            "mskutzossrwoqqsi",
            "rygoebkzgyzushhr",
            "jpjqiycflqkexemx",
            "arbufysjqmgaapnl",
            "dbjerflevtgweeoj",
            "snybnnjlmwjvhois",
            "fszuzplntraprmbj",
            "mkvaatolvuggikvg",
            "zpuzuqygoxesnuyc",
            "wnpxvmxvllxalulm",
            "eivuuafkvudeouwy",
            "rvzckdyixetfuehr",
            "qgmnicdoqhveahyx",
            "miawwngyymshjmpj",
            "pvckyoncpqeqkbmx",
            "llninfenrfjqxurv",
            "kzbjnlgsqjfuzqtp",
            "rveqcmxomvpjcwte",
            "bzotkawzbopkosnx",
            "ktqvpiribpypaymu",
            "wvlzkivbukhnvram",
            "uohntlcoguvjqqdo",
            "ajlsiksjrcnzepkt",
            "xsqatbldqcykwusd",
            "ihbivgzrwpmowkop",
            "vfayesfojmibkjpb",
            "uaqbnijtrhvqxjtb",
            "hhovshsfmvkvymba",
            "jerwmyxrfeyvxcgg",
            "hncafjwrlvdcupma",
            "qyvigggxfylbbrzt",
            "hiiixcyohmvnkpgk",
            "mmitpwopgxuftdfu",
            "iaxderqpceboixoa",
            "zodfmjhuzhnsqfcb",
            "sthtcbadrclrazsi",
            "bkkkkcwegvypbrio",
            "wmpcofuvzemunlhj",
            "gqwebiifvqoeynro",
            "juupusqdsvxcpsgv",
            "rbhdfhthxelolyse",
            "kjimpwnjfrqlqhhz",
            "rcuigrjzarzpjgfq",
            "htxcejfyzhydinks",
            "sxucpdxhvqjxxjwf",
            "omsznfcimbcwaxal",
            "gufmtdlhgrsvcosb",
            "bssshaqujtmluerz",
            "uukotwjkstgwijtr",
            "kbqkneobbrdogrxk",
            "ljqopjcjmelgrakz",
            "rwtfnvnzryujwkfb",
            "dedjjbrndqnilbeh",
            "nzinsxnpptzagwlb",
            "lwqanydfirhnhkxy",
            "hrjuzfumbvfccxno",
            "okismsadkbseumnp",
            "sfkmiaiwlktxqvwa",
            "hauwpjjwowbunbjj",
            "nowkofejwvutcnui",
            "bqzzppwoslaeixro",
            "urpfgufwbtzenkpj",
            "xgeszvuqwxeykhef",
            "yxoldvkyuikwqyeq",
            "onbbhxrnmohzskgg",
            "qcikuxakrqeugpoa",
            "lnudcqbtyzhlpers",
            "nxduvwfrgzaailgl",
            "xniuwvxufzxjjrwz",
            "ljwithcqmgvntjdj",
            "awkftfagrfzywkhs",
            "uedtpzxyubeveuek",
            "bhcqdwidbjkqqhzl",
            "iyneqjdmlhowwzxx",
            "kvshzltcrrururty",
            "zgfpiwajegwezupo",
            "tkrvyanujjwmyyri",
            "ercsefuihcmoaiep",
            "ienjrxpmetinvbos",
            "jnwfutjbgenlipzq",
            "bgohjmrptfuamzbz",
            "rtsyamajrhxbcncw",
            "tfjdssnmztvbnscs",
            "bgaychdlmchngqlp",
            "kfjljiobynhwfkjo",
            "owtdxzcpqleftbvn",
            "ltjtimxwstvzwzjj",
            "wbrvjjjajuombokf",
            "zblpbpuaqbkvsxye",
            "gwgdtbpnlhyqspdi",
            "abipqjihjqfofmkx",
            "nlqymnuvjpvvgova",
            "avngotmhodpoufzn",
            "qmdyivtzitnrjuae",
            "xfwjmqtqdljuerxi",
            "csuellnlcyqaaamq",
            "slqyrcurcyuoxquo",
            "dcjmxyzbzpohzprl",
            "uqfnmjwniyqgsowb",
            "rbmxpqoblyxdocqc",
            "ebjclrdbqjhladem",
            "ainnfhxnsgwqnmyo",
            "eyytjjwhvodtzquf",
            "iabjgmbbhilrcyyp",
            "pqfnehkivuelyccc",
            "xgjbyhfgmtseiimt",
            "jwxyqhdbjiqqqeyy",
            "gxsbrncqkmvaryln",
            "vhjisxjkinaejytk",
            "seexagcdmaedpcvh",
            "lvudfgrcpjxzdpvd",
            "fxtegyrqjzhmqean",
            "dnoiseraqcoossmc",
            "nwrhmwwbykvwmgep",
            "udmzskejvizmtlce",
            "hbzvqhvudfdlegaa",
            "cghmlfqejbxewskv",
            "bntcmjqfwomtbwsb",
            "qezhowyopjdyhzng",
            "todzsocdkgfxanbz",
            "zgjkssrjlwxuhwbk",
            "eibzljqsieriyrzr",
            "wamxvzqyycrxotjp",
            "epzvfkispwqynadu",
            "dwlpfhtrafrxlyie",
            "qhgzujhgdruowoug",
            "girstvkahaemmxvh",
            "baitcrqmxhazyhbl",
            "xyanqcchbhkajdmc",
            "gfvjmmcgfhvgnfdq",
            "tdfdbslwncbnkzyz",
            "jojuselkpmnnbcbb",
            "hatdslkgxtqpmavj",
            "dvelfeddvgjcyxkj",
            "gnsofhkfepgwltse",
            "mdngnobasfpewlno",
            "qssnbcyjgmkyuoga",
            "glvcmmjytmprqwvn",
            "gwrixumjbcdffsdl",
            "lozravlzvfqtsuiq",
            "sicaflbqdxbmdlch",
            "inwfjkyyqbwpmqlq",
            "cuvszfotxywuzhzi",
            "igfxyoaacoarlvay",
            "ucjfhgdmnjvgvuni",
            "rvvkzjsytqgiposh",
            "jduinhjjntrmqroz",
            "yparkxbgsfnueyll",
            "lyeqqeisxzfsqzuj",
            "woncskbibjnumydm",
            "lltucklragtjmxtl",
            "ubiyvmyhlesfxotj",
            "uecjseeicldqrqww",
            "xxlxkbcthufnjbnm",
            "lhqijovvhlffpxga",
            "fzdgqpzijitlogjz",
            "efzzjqvwphomxdpd",
            "jvgzvuyzobeazssc",
            "hejfycgxywfjgbfw",
            "yhjjmvkqfbnbliks",
            "sffvfyywtlntsdsz",
            "dwmxqudvxqdenrur",
            "asnukgppdemxrzaz",
            "nwqfnumblwvdpphx",
            "kqsmkkspqvxzuket",
            "cpnraovljzqiquaz",
            "qrzgrdlyyzbyykhg",
            "opoahcbiydyhsmqe",
            "hjknnfdauidjeydr",
            "hczdjjlygoezadow",
            "rtflowzqycimllfv",
            "sfsrgrerzlnychhq",
            "bpahuvlblcolpjmj",
            "albgnjkgmcrlaicl",
            "pijyqdhfxpaxzdex",
            "eeymiddvcwkpbpux",
            "rqwkqoabywgggnln",
            "vckbollyhgbgmgwh",
            "ylzlgvnuvpynybkm",
            "hpmbxtpfosbsjixt",
            "ocebeihnhvkhjfqz",
            "tvctyxoujdgwayze",
            "efvhwxtuhapqxjen",
            "rusksgefyidldmpo",
            "nkmtjvddfmhirmzz",
            "whvtsuadwofzmvrt",
            "iiwjqvsdxudhdzzk",
            "gucirgxaxgcassyo",
            "rmhfasfzexeykwmr",
            "hynlxcvsbgosjbis",
            "huregszrcaocueen",
            "pifezpoolrnbdqtv",
            "unatnixzvdbqeyox",
            "xtawlpduxgacchfe",
            "bdvdbflqfphndduf",
            "xtdsnjnmzccfptyt",
            "nkhsdkhqtzqbphhg",
            "aqcubmfkczlaxiyb",
            "moziflxpsfubucmv",
            "srdgnnjtfehiimqx",
            "pwfalehdfyykrohf",
            "sysxssmvewyfjrve",
            "brsemdzosgqvvlxe",
            "bimbjoshuvflkiat",
            "hkgjasmljkpkwwku",
            "sbnmwjvodygobpqc",
            "bbbqycejueruihhd",
            "corawswvlvneipyc",
            "gcyhknmwsczcxedh",
            "kppakbffdhntmcqp",
            "ynulzwkfaemkcefp",
            "pyroowjekeurlbii",
            "iwksighrswdcnmxf",
            "glokrdmugreygnsg",
            "xkmvvumnfzckryop",
            "aesviofpufygschi",
            "csloawlirnegsssq",
            "fkqdqqmlzuxbkzbc",
            "uzlhzcfenxdfjdzp",
            "poaaidrktteusvyf",
            "zrlyfzmjzfvivcfr",
            "qwjulskbniitgqtx",
            "gjeszjksbfsuejki",
            "vczdejdbfixbduaq",
            "knjdrjthitjxluth",
            "jweydeginrnicirl",
            "bottrfgccqhyycsl",
            "eiquffofoadmbuhk",
            "lbqfutmzoksscswf",
            "xfmdvnvfcnzjprba",
            "uvugkjbkhlaoxmyx",
            "wadlgtpczgvcaqqv",
            "inzrszbtossflsxk",
            "dbzbtashaartczrj",
            "qbjiqpccefcfkvod",
            "hluujmokjywotvzy",
            "thwlliksfztcmwzh",
            "arahybspdaqdexrq",
            "nuojrmsgyipdvwyx",
            "hnajdwjwmzattvst",
            "sulcgaxezkprjbgu",
            "rjowuugwdpkjtypw",
            "oeugzwuhnrgiaqga",
            "wvxnyymwftfoswij",
            "pqxklzkjpcqscvde",
            "tuymjzknntekglqj",
            "odteewktugcwlhln",
            "exsptotlfecmgehc",
            "eeswfcijtvzgrqel",
            "vjhrkiwmunuiwqau",
            "zhlixepkeijoemne",
            "pavfsmwesuvebzdd",
            "jzovbklnngfdmyws",
            "nbajyohtzfeoiixz",
            "ciozmhrsjzrwxvhz",
            "gwucrxieqbaqfjuv",
            "uayrxrltnohexawc",
            "flmrbhwsfbcquffm",
            "gjyabmngkitawlxc",
            "rwwtggvaygfbovhg",
            "xquiegaisynictjq",
            "oudzwuhexrwwdbyy",
            "lengxmguyrwhrebb",
            "uklxpglldbgqsjls",
            "dbmvlfeyguydfsxq",
            "zspdwdqcrmtmdtsc",
            "mqfnzwbfqlauvrgc",
            "amcrkzptgacywvhv",
            "ndxmskrwrqysrndf",
            "mwjyhsufeqhwisju",
            "srlrukoaenyevykt",
            "tnpjtpwawrxbikct",
            "geczalxmgxejulcv",
            "tvkcbqdhmuwcxqci",
            "tiovluvwezwwgaox",
            "zrjhtbgajkjqzmfo",
            "vcrywduwsklepirs",
            "lofequdigsszuioy",
            "wxsdzomkjqymlzat",
            "iabaczqtrfbmypuy",
            "ibdlmudbajikcncr",
            "rqcvkzsbwmavdwnv",
            "ypxoyjelhllhbeog",
            "fdnszbkezyjbttbg",
            "uxnhrldastpdjkdz",
            "xfrjbehtxnlyzcka",
            "omjyfhbibqwgcpbv",
            "eguucnoxaoprszmp",
            "xfpypldgcmcllyzz",
            "aypnmgqjxjqceelv",
            "mgzharymejlafvgf",
            "tzowgwsubbaigdok",
            "ilsehjqpcjwmylxc",
            "pfmouwntfhfnmrwk",
            "csgokybgdqwnduwp",
            "eaxwvxvvwbrovypz",
            "nmluqvobbbmdiwwb",
            "lnkminvfjjzqbmio",
            "mjiiqzycqdhfietz",
            "towlrzriicyraevq",
            "obiloewdvbrsfwjo",
            "lmeooaajlthsfltw",
            "ichygipzpykkesrw",
            "gfysloxmqdsfskvt",
            "saqzntehjldvwtsx",
            "pqddoemaufpfcaew",
            "mjrxvbvwcreaybwe",
            "ngfbrwfqnxqosoai",
            "nesyewxreiqvhald",
            "kqhqdlquywotcyfy",
            "liliptyoqujensfi",
            "nsahsaxvaepzneqq",
            "zaickulfjajhctye",
            "gxjzahtgbgbabtht",
            "koxbuopaqhlsyhrp",
            "jhzejdjidqqtjnwe",
            "dekrkdvprfqpcqki",
            "linwlombdqtdeyop",
            "dvckqqbnigdcmwmx",
            "yaxygbjpzkvnnebv",
            "rlzkdkgaagmcpxah",
            "cfzuyxivtknirqvt",
            "obivkajhsjnrxxhn",
            "lmjhayymgpseuynn",
            "bbjyewkwadaipyju",
            "lmzyhwomfypoftuu",
            "gtzhqlgltvatxack",
            "jfflcfaqqkrrltgq",
            "txoummmnzfrlrmcg",
            "ohemsbfuqqpucups",
            "imsfvowcbieotlok",
            "tcnsnccdszxfcyde",
            "qkcdtkwuaquajazz",
            "arcfnhmdjezdbqku",
            "srnocgyqrlcvlhkb",
            "mppbzvfmcdirbyfw",
            "xiuarktilpldwgwd",
            "ypufwmhrvzqmexpc",
            "itpdnsfkwgrdujmj",
            "cmpxnodtsswkyxkr",
            "wayyxtjklfrmvbfp",
            "mfaxphcnjczhbbwy",
            "sjxhgwdnqcofbdra",
            "pnxmujuylqccjvjm",
            "ivamtjbvairwjqwl",
            "deijtmzgpfxrclss",
            "bzkqcaqagsynlaer",
            "tycefobvxcvwaulz",
            "ctbhnywezxkdsswf",
            "urrxxebxrthtjvib",
            "fpfelcigwqwdjucv",
            "ngfcyyqpqulwcphb",
            "rltkzsiipkpzlgpw",
            "qfdsymzwhqqdkykc",
            "balrhhxipoqzmihj",
            "rnwalxgigswxomga",
            "ghqnxeogckshphgr",
            "lyyaentdizaumnla",
            "exriodwfzosbeoib",
            "speswfggibijfejk",
            "yxmxgfhvmshqszrq",
            "hcqhngvahzgawjga",
            "qmhlsrfpesmeksur",
            "eviafjejygakodla",
            "kvcfeiqhynqadbzv",
            "fusvyhowslfzqttg",
            "girqmvwmcvntrwau",
            "yuavizroykfkdekz",
            "jmcwohvmzvowrhxf",
            "kzimlcpavapynfue",
            "wjudcdtrewfabppq",
            "yqpteuxqgbmqfgxh",
            "xdgiszbuhdognniu",
            "jsguxfwhpftlcjoh",
            "whakkvspssgjzxre",
            "ggvnvjurlyhhijgm",
            "krvbhjybnpemeptr",
            "pqedgfojyjybfbzr",
            "jzhcrsgmnkwwtpdo",
            "yyscxoxwofslncmp",
            "gzjhnxytmyntzths",
            "iteigbnqbtpvqumi",
            "zjevfzusnjukqpfw",
            "xippcyhkfuounxqk",
            "mcnhrcfonfdgpkyh",
            "pinkcyuhjkexbmzj",
            "lotxrswlxbxlxufs",
            "fmqajrtoabpckbnu",
            "wfkwsgmcffdgaqxg",
            "qfrsiwnohoyfbidr",
            "czfqbsbmiuyusaqs",
            "ieknnjeecucghpoo",
            "cevdgqnugupvmsge",
            "gjkajcyjnxdrtuvr",
            "udzhrargnujxiclq",
            "zqqrhhmjwermjssg",
            "ggdivtmgoqajydzz",
            "wnpfsgtxowkjiivl",
            "afbhqawjbotxnqpd",
            "xjpkifkhfjeqifdn",
            "oyfggzsstfhvticp",
            "kercaetahymeawxy",
            "khphblhcgmbupmzt",
            "iggoqtqpvaebtiol",
            "ofknifysuasshoya",
            "qxuewroccsbogrbv",
            "apsbnbkiopopytgu",
            "zyahfroovfjlythh",
            "bxhjwfgeuxlviydq",
            "uvbhdtvaypasaswa",
            "qamcjzrmesqgqdiz",
            "hjnjyzrxntiycyel",
            "wkcrwqwniczwdxgq",
            "hibxlvkqakusswkx",
            "mzjyuenepwdgrkty",
            "tvywsoqslfsulses",
            "jqwcwuuisrclircv",
            "xanwaoebfrzhurct",
            "ykriratovsvxxasf",
            "qyebvtqqxbjuuwuo",
            "telrvlwvriylnder",
            "acksrrptgnhkeiaa",
            "yemwfjhiqlzsvdxf",
            "banrornfkcymmkcc",
            "ytbhxvaeiigjpcgm",
            "crepyazgxquposkn",
            "xlqwdrytzwnxzwzv",
            "xtrbfbwopxscftps",
            "kwbytzukgseeyjla",
            "qtfdvavvjogybxjg",
            "ytbmvmrcxwfkgvzw",
            "nbscbdskdeocnfzr",
            "sqquwjbdxsxhcseg",
            "ewqxhigqcgszfsuw",
            "cvkyfcyfmubzwsee",
            "dcoawetekigxgygd",
            "ohgqnqhfimyuqhvi",
            "otisopzzpvnhctte",
            "bauieohjejamzien",
            "ewnnopzkujbvhwce",
            "aeyqlskpaehagdiv",
            "pncudvivwnnqspxy",
            "ytugesilgveokxcg",
            "zoidxeelqdjesxpr",
            "ducjccsuaygfchzj",
            "smhgllqqqcjfubfc",
            "nlbyyywergronmir",
            "prdawpbjhrzsbsvj",
            "nmgzhnjhlpcplmui",
            "eflaogtjghdjmxxz",
            "qolvpngucbkprrdc",
            "ixywxcienveltgho",
            "mwnpqtocagenkxut",
            "iskrfbwxonkguywx",
            "ouhtbvcaczqzmpua",
            "srewprgddfgmdbao",
            "dyufrltacelchlvu",
            "czmzcbrkecixuwzz",
            "dtbeojcztzauofuk",
            "prrgoehpqhngfgmw",
            "baolzvfrrevxsyke",
            "zqadgxshwiarkzwh",
            "vsackherluvurqqj",
            "surbpxdulvcvgjbd",
            "wqxytarcxzgxhvtx",
            "vbcubqvejcfsgrac",
            "zqnjfeapshjowzja",
            "hekvbhtainkvbynx",
            "knnugxoktxpvoxnh",
            "knoaalcefpgtvlwm",
            "qoakaunowmsuvkus",
            "ypkvlzcduzlezqcb",
            "ujhcagawtyepyogh",
            "wsilcrxncnffaxjf",
            "gbbycjuscquaycrk",
            "aduojapeaqwivnly",
            "ceafyxrakviagcjy",
            "nntajnghicgnrlst",
            "vdodpeherjmmvbje",
            "wyyhrnegblwvdobn",
            "xlfurpghkpbzhhif",
            "xyppnjiljvirmqjo",
            "kglzqahipnddanpi",
            "omjateouxikwxowr",
            "ocifnoopfglmndcx",
            "emudcukfbadyijev",
            "ooktviixetfddfmh",
            "wtvrhloyjewdeycg",
            "cgjncqykgutfjhvb",
            "nkwvpswppeffmwad",
            "hqbcmfhzkxmnrivg",
            "mdskbvzguxvieilr",
            "anjcvqpavhdloaqh",
            "erksespdevjylenq",
            "fadxwbmisazyegup",
            "iyuiffjmcaahowhj",
            "ygkdezmynmltodbv",
            "fytneukxqkjattvh",
            "woerxfadbfrvdcnz",
            "iwsljvkyfastccoa",
            "movylhjranlorofe",
            "drdmicdaiwukemep",
            "knfgtsmuhfcvvshg",
            "ibstpbevqmdlhajn",
            "tstwsswswrxlzrqs",
            "estyydmzothggudf",
            "jezogwvymvikszwa",
            "izmqcwdyggibliet",
            "nzpxbegurwnwrnca",
            "kzkojelnvkwfublh",
            "xqcssgozuxfqtiwi",
            "tcdoigumjrgvczfv",
            "ikcjyubjmylkwlwq",
            "kqfivwystpqzvhan",
            "bzukgvyoqewniivj",
            "iduapzclhhyfladn",
            "fbpyzxdfmkrtfaeg",
            "yzsmlbnftftgwadz"
        };

        #endregion Data
    }
}
