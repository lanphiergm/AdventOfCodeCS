﻿//#define USESAMPLE
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Problems.Year2020
{
    class Day21AllergenAssessment : ProblemBase<int>
    {
        public Day21AllergenAssessment(ILogger logger) : base(logger, "Allergen Assessment", 2020, 21) { }

        private readonly List<Food> foods = new List<Food>();
        private readonly HashSet<string> allergens = new HashSet<string>();

        protected override int ExecutePart1()
        {
            Initialize();
            while (allergens.Any())
            {
                var allergensCopy = new HashSet<string>(allergens);
                foreach (string allergen in allergensCopy)
                {
                    var foodsWithAllergen = foods.Where(f => f.Allergens.Contains(allergen));
                    IEnumerable<string> intersection = null;
                    foreach (var food in foodsWithAllergen)
                    {
                        if (intersection == null)
                        {
                            intersection = food.Ingredients;
                        }
                        else
                        {
                            intersection = intersection.Intersect(food.Ingredients);
                        }
                    }
                    if (intersection.Count() == 1)
                    {
                        allergens.Remove(allergen);
                        string ingredient = intersection.First();
                        foreach (var food in foods)
                        {
                            food.Allergens.Remove(allergen);
                            food.Ingredients.Remove(ingredient);
                        }
                    }
                }
            }

            int ingredientOccurrences = 0;
            foreach (var food in foods)
            {
                ingredientOccurrences += food.Ingredients.Count;
            }
            return ingredientOccurrences;
        }

        protected override int ExecutePart2()
        {
            Initialize();
            var dangerousIngredients = new SortedList<string, string>();
            while (allergens.Any())
            {
                var allergensCopy = new HashSet<string>(allergens);
                foreach (string allergen in allergensCopy)
                {
                    var foodsWithAllergen = foods.Where(f => f.Allergens.Contains(allergen));
                    IEnumerable<string> intersection = null;
                    foreach (var food in foodsWithAllergen)
                    {
                        if (intersection == null)
                        {
                            intersection = food.Ingredients;
                        }
                        else
                        {
                            intersection = intersection.Intersect(food.Ingredients);
                        }
                    }
                    if (intersection.Count() == 1)
                    {
                        string ingredient = intersection.First();
                        dangerousIngredients.Add(allergen, ingredient);

                        allergens.Remove(allergen);
                        foreach (var food in foods)
                        {
                            food.Allergens.Remove(allergen);
                            food.Ingredients.Remove(ingredient);
                        }
                    }
                }
            }

            Logger.LogInformation("Canonical Dangerous Ingredient List: {0}",
                string.Join(',', dangerousIngredients.Values));
            return dangerousIngredients.Count;
        }

        private class Food
        {
            public List<string> Allergens { get; } = new List<string>();
            public List<string> Ingredients { get; } = new List<string>();
        }

        private void Initialize()
        {
            foods.Clear();
            foreach (string foodDefinition in foodDefinitionList)
            {
                var match = foodDefinitionRegex.Match(foodDefinition);
                System.Diagnostics.Debug.Assert(match.Success);
                var food = new Food();
                foreach (string ingredient in match.Groups[1].Value.Split(' '))
                {
                    food.Ingredients.Add(ingredient);
                }
                foreach (string allergen in match.Groups[2].Value.Split(','))
                {
                    allergens.Add(allergen.Trim());
                    food.Allergens.Add(allergen.Trim());
                }
                foods.Add(food);
            }
        }

        private static readonly Regex foodDefinitionRegex = new Regex(@"^(.*) \(contains ([^\)]+)\)$");

        #region Data

#if USESAMPLE
        private static readonly string[] foodDefinitionList =
        {
            "mxmxvkd kfcds sqjhc nhms (contains dairy, fish)",
            "trh fvjkl sbzzf mxmxvkd (contains dairy)",
            "sqjhc fvjkl (contains soy)",
            "sqjhc mxmxvkd sbzzf (contains fish)"
        };
#else
        private static readonly string[] foodDefinitionList =
        {
            "vsnt bdxflb vljjtr ddtbfc kqvpgdhf dhmrf nbmtklt sqlqzh dknpsz cmzsnxn xkmv lgdfltvn gcmcg vltq zvx bjsmg cbsp lp lpfrt vgkq fmmthkn rpgbn dzcsh zjfsc hkjjt tjczdt lfqd ddjbr xjkx hgxjcl vttzcj dmfrzs xjqv ptlf jpx szj qfcsfg zvbmzc mqvk bmfl cgvzlqx tgrx rnlscmt frvr jdcc hkflr ctmcqjf hfq hlqz dpmdps cgjd ttrmcgd bfrq lpvss slvx xmgprh qdgx fdrsmkkq nrpgkq nplshs njcck mhtzd krfnsd kvttspgs srxphcm dmp snmxl htrm hjhvx fxqzh mzjnpdv lffvh vfzrhg cdmxp qbtdklh rcdjqss rm (contains shellfish)",
            "cqtt nplshs cghn xtxzvx txbg vkdfkzh qfcsfg bd lgdfltvn ktrpqv lgvlk vttzcj mzjnpdv srxphcm ttgvz xkmv ssg zvx lfqd mqvk bfrq fgnjv hsxvvn qbtdklh rcdjqss xmgprh slvx cdmxp dqshc jpx rjddcn htrm lpvss vltq zlmxj hkflr tvbtx sxmph pr tfssqq bgfzbxd jfpmgbdn nbmtklt tvqvrb snmxl ssr cmzsnxn hfq cgthc rnnqzt dmp mrgqf dhjvbzc cthbr dxbtm vgkq (contains peanuts)",
            "zvx rpgbn gzpcm fmmthkn hjhvx xjqv srxphcm xzbphmgb hkflr mzjnpdv vttzcj zvbmzc mjsc lnznjm sjsxqxz hndj snmxl bfrq mhtzd hsxvvn rznd xmgprh gkndl hcgr bsgnt htrm dzcsh bd fdrsmkkq vfzrhg lcnppvn nxrv txbg cbznj xhjkg hlqz cghn fnlq stznpz dmfrzs tfssqq mqvk bjsmg gtpzdxx (contains soy)",
            "bmfl rznd jnmf xkmv mhpxlvb bgfzbxd snmxl jpx xtxzvx lffvh pzqvqs mdgrf gvkpnl ctmcqjf tvqvrb ssr kvttspgs gzpcm tvbtx fnlq cgjd sffch gmdp dnhm qdgx lp zvx vsnt tjczdt kqvpgdhf rtvzjs rphm bjtrq mqvk sjsjg mjsc fxqzh cgvzlqx ddtbfc dhjvbzc clqmstt bfrq fgnjv hmnf bd bjhtz gmmbtr sxmph bdxflb szj fmmthkn zlqn dmp zcpmb nnhjj htrm qfql kpns czt cbznj tln dzcsh zlmxj mqbhv zbnb lcnppvn hndj bxzbk fdrsmkkq qbtdklh skqfq njkjxs pr vbrr dpmdps nrpgkq sjsxqxz pzmmq mrqnv jdcc srxphcm gcmcg cbsp gkndl ssg cgplh rcdjqss dqshc ktrpqv ddjbr gtpzdxx qcznk hcgr mzjnpdv mmhghd dqxcvg nplshs (contains nuts, sesame)",
            "kpns ccnvx snmxl dqshc gvkpnl rnnqzt cgvzlqx jpx xzbphmgb nxrv vzrlps rcdjqss frvr szj rznd vbrr mrgqf zxd hndj bffb bzjsxg hkflr bfrq cgjd srxphcm fnlq ddtbfc blrk fmmthkn lgdfltvn pzmmq rpgbn mnqcl tvqvrb mfqjps fgnjv bd ptrgd sxmph xv cqtt tvbtx lp fxqzh pzqvqs mmhghd sjzlr cghn tjczdt pr dqxcvg jnmf rm xmgprh kvttspgs gkgzf knfs dsshnl njkjxs tgrx dpmdps ctmcqjf sjsjg hmnf fkctmn cmzsnxn nrpgkq hsxvvn kvlj nnh zvx qbtdklh xhjkg stql (contains eggs, nuts)",
            "srxphcm hkjjt fnlq cgjd hmnf dnhm qcznk ssr bch bsgnt qfql bffb vttzcj ddjbr ttgvz xmgprh lfqd kvlj qpdq pzmmq nnh vzrlps szj sjsxqxz tln rznd brfzm gmdp lcnppvn dhmrf rtvzjs zvx mqvk tvbtx vbrr lpvss fkctmn zcpmb fdrsmkkq xtxzvx mhtzd skqfq bkmrq hjhvx bfrq frvr vfzrhg zlqn bd jdcc kqvpgdhf rjddcn mfqjps ctmcqjf fgnjv stznpz mmhghd lpfrt tjczdt fgp gkgzf gzpcm cbznj snmxl hgxjcl (contains shellfish, nuts)",
            "lcnppvn xhjkg mqvk stql qpdq ssr ttrmcgd hgxjcl qhknjnv ktrpqv mhtzd gkndl qcznk hkflr ctmcqjf dhjvbzc qbtdklh bfrq dpmdps srxphcm rhs fgp krfnsd sffch sqlqzh ddjbr sjzlr zlqn mmhghd bjtrq bxpjj lffvh cbznj mhpxlvb hhkl gzpcm qdgx dmp gvkpnl vkdfkzh bgfzbxd ssg stznpz rjddcn cghn jdcc xjkx rnlscmt nglclj njkjxs bd dzcsh pzmmq snmxl zxd mdgrf bbjfnr (contains nuts, soy)",
            "bgfzbxd nxrv cgvzlqx fmmthkn blrk bdxflb vtnmr dqxcvg rznd szj bfrq zjfsc fnlq srxphcm vgkq nbmtklt tgrx hndj dnhm tfssqq zvx njkjxs ktrpqv zcpmb nplshs qpdq qcznk lfqd tdkd ctmcqjf qdgx lnznjm gvkpnl clqmstt xjqv fdrsmkkq rpgbn lpvss stql mhtzd ddxjjp mfqjps njcck tvbtx mqvk bch bsgnt mnqcl vltq mmhghd ssr bxpjj dpmdps zbnb ttgvz pr stznpz hmnf hcgr xmgprh cbznj bd mqbhv gkgzf ptrgd kvttspgs bzjsxg snmxl sjzlr rhs fxqzh fkctmn dmp gmmbtr cqtt vttzcj dsshnl (contains peanuts)",
            "sffch rjxkdp zbnb mjsc htrm clqmstt vltq rm ssg bsgnt bch zvx zxd dzcsh fxqzh gkndl dhmrf cthbr nglclj hkflr hhkl ttrmcgd xzbphmgb ddjbr dnhm bd ssr dqxcvg cgjd jdcc mrqnv bdxflb srxphcm lcdgsh zcpmb kvttspgs vtnmr xhjkg bjsmg bfrq cqtt gmdp snmxl bxpjj cgplh mmhghd vfzrhg nbmtklt szj mqvk xjkx cbsp (contains eggs, soy, peanuts)",
            "bxzbk hlqz jfpmgbdn hfq nbmtklt rjddcn zvx ssg kvttspgs srxphcm lgdfltvn gcmcg kpns dknpsz tzzg kqvpgdhf vfzrhg hkflr cmzsnxn bd fvlq dxbtm vtnmr vltq snmxl hmnf cthbr xkmv hkjjt bdxflb vttzcj cgjd hhkl xzbphmgb cgthc cgvzlqx ptrgd xjqv clqmstt bbjfnr nplshs szj mmhghd ctmcqjf mjsc qpdq lcnppvn dqxcvg lp zbnb mhpxlvb zvbmzc vkdfkzh bzjsxg xjkx qdgx nxrv sqlqzh pzqvqs hcgr bfrq sffch fmmthkn bxpjj (contains peanuts, soy)",
            "hsxvvn ktrpqv krfnsd mhpxlvb tvbtx dhmrf kvlj bxzbk snmxl zvbmzc bjtrq dmp pzqvqs ssg vsnt tgrx bxpjj jfpmgbdn hlqz mhtzd cgthc tvqvrb bfrq jdcc rqhl xtxzvx ltvrs dmfrzs qdgx xjkx mdgrf hndj gcmcg dknpsz mhph mqbhv bgfzbxd rjxkdp bjhtz ccnvx lnznjm dxbtm vttzcj ddxjjp vltq fgnjv qfcsfg vzrlps hjhvx gtpzdxx cgvzlqx ssr bd zlqn nnh cghn cgjd lp hmnf zvx fkctmn mrqnv cmzsnxn dnhm rtvzjs slvx xhjkg sffch mqvk vjvdx sjsxqxz kqvpgdhf nnhjj bdxflb ttrmcgd vfzrhg jpx mnqcl sjsjg sjzlr srxphcm cthbr hkflr qbtdklh rcdjqss (contains nuts)",
            "jpx nglclj zvbmzc vbrr slvx hsxvvn sjsjg rm mrqnv zbnb krfnsd hgxjcl tdkd fvlq lgdfltvn tzzg dqshc tfssqq gmdp qfql hkjjt njcck rjddcn ddxjjp ctmcqjf qhknjnv bjsmg zvx cbsp mjsc srxphcm dknpsz hkflr zlmxj mfqjps vttzcj sjsxqxz skqfq kpns tvbtx vzrlps ddjbr snmxl bfrq qcznk mhph ktrpqv bd ttrmcgd mdgrf dqxcvg bsgnt dsshnl cmzsnxn cgvzlqx bbjfnr tgrx fkm gzpcm zjfsc (contains nuts)",
            "szj srxphcm vttzcj fkm pr qcznk hfq xmgprh jfpmgbdn bfrq hkflr brfzm lfqd bch gkgzf tdkd bd bbjfnr fmmthkn nnhjj njkjxs gcmcg htrm stznpz tln ddxjjp cgthc mmhghd zvx zlmxj rnlscmt hmnf jdcc cbsp xkmv nnh snmxl zvbmzc dmp zlqn tzzg tfssqq ctmcqjf rjddcn (contains eggs, soy)",
            "gkndl skqfq xhjkg njcck sjsxqxz dhjvbzc fkm mfqjps gmdp ctmcqjf fmmthkn mrqnv rnlscmt jnmf ptlf xv fxqzh tvbtx stql rpgbn bffb qfcsfg hcgr brfzm mhph fnlq vkdfkzh dqshc srxphcm dhmrf lgvlk rjxkdp hndj dknpsz bjsmg ddjbr rphm fgnjv krfnsd lp lffvh tln fkctmn hkflr rznd clqmstt vfzrhg zjfsc sffch zvbmzc hjhvx xkmv bmfl mqvk dmp vjvdx qhknjnv snmxl cglpr zxd ktrpqv dnhm fgp dsshnl dpmdps mnqcl nrpgkq zvx jdcc bd mzjnpdv sjzlr zlmxj zcpmb (contains fish, sesame, soy)",
            "blrk ccnvx bfrq bffb fkm ctmcqjf fgp ttgvz lp dhjvbzc bxpjj xmgprh ssg cgplh nnh nglclj frvr rcdjqss vsnt qdgx snmxl vtnmr rnnqzt rqhl mnqcl fvlq jdcc nplshs rnlscmt srxphcm mqvk hcgr mqbhv cdmxp dknpsz kvlj mfqjps qbtdklh hkflr qcznk mhph lpvss cthbr mzjnpdv bd vkdfkzh gmdp rphm qhknjnv mrgqf mhpxlvb cgvzlqx xhjkg stznpz (contains soy)",
            "jnmf zlmxj hlqz lnznjm cgvzlqx lpfrt qcznk sxmph hgxjcl rm frvr mhph nrpgkq brfzm sjsjg mfqjps srxphcm gmdp krfnsd gtpzdxx ttgvz zvx ssg lcdgsh cghn mqvk mqbhv zcpmb njkjxs bdxflb rznd lfqd hmnf fnlq jpx szj vbrr vltq bd lcnppvn bgfzbxd qbtdklh vfzrhg ctmcqjf vzrlps rnlscmt xjqv qhknjnv sffch fvlq qfql ddtbfc hkflr xkmv ssr lgvlk jdcc cdmxp bkmrq rnnqzt bfrq ptlf fmmthkn vkdfkzh xmgprh htrm mmhghd dmp bjsmg czt ktrpqv ptrgd txbg rjddcn sqlqzh tjczdt dhmrf (contains wheat, peanuts, shellfish)",
            "mhpxlvb mqvk bfrq gkgzf qpdq sjsjg zvx srxphcm ddtbfc ctmcqjf cgplh gzpcm lfqd mfqjps lp gtpzdxx qhknjnv bjhtz bch dpmdps snmxl ptlf rnnqzt bmfl ptrgd pzmmq rphm ddjbr ddxjjp hkflr lgdfltvn dhmrf hfq sjsxqxz dknpsz mmhghd hgxjcl pzqvqs xtxzvx cthbr vtnmr mnqcl (contains shellfish, nuts)",
            "bjtrq zbnb lgvlk sqlqzh mjsc zlqn tzzg sxmph bjsmg zxd zvx xhjkg mhpxlvb vkdfkzh rhs brfzm qdgx ccnvx bd nplshs ddjbr bfrq tvqvrb sjsjg gzpcm ssr gmmbtr lpfrt jnmf cgvzlqx nxrv hfq hlqz njkjxs mfqjps cgplh fgp fmmthkn bdxflb dzcsh xmgprh qbtdklh szj zlmxj slvx vjvdx snmxl hmnf ttgvz nnh srxphcm cbsp vljjtr qhknjnv dsshnl rjxkdp mzjnpdv zcpmb krfnsd bzjsxg nnhjj gtpzdxx clqmstt rtvzjs cgjd nglclj lp pzmmq hkflr htrm sjzlr lfqd xjkx dqxcvg rcdjqss hsxvvn stznpz dhmrf bsgnt hndj mqvk fxqzh (contains peanuts, sesame)",
            "xzbphmgb fgnjv lgvlk vgkq vttzcj nglclj fgp zvx cbsp lcdgsh hkflr cglpr hlqz hjhvx ddtbfc jfpmgbdn frvr tvbtx fkm bd pzmmq lcnppvn hcgr dqshc xhjkg dpmdps qhknjnv dknpsz qdgx brfzm cbznj bjhtz srxphcm vbrr gcmcg ctmcqjf rjddcn sffch fvlq zcpmb bfrq clqmstt htrm lffvh mqvk mqbhv ktrpqv (contains wheat)",
            "blrk lffvh zvbmzc tln vtnmr qbtdklh bd czt rcdjqss snmxl skqfq fnlq ktrpqv srxphcm bfrq rhs mqvk gmmbtr bffb xkmv mrqnv hkflr kqvpgdhf lgdfltvn hjhvx lpfrt cqtt mrgqf zvx mmhghd fkm jfpmgbdn vkdfkzh clqmstt cbsp sffch nrpgkq fkctmn hgxjcl dmp mqbhv sxmph htrm mhpxlvb cglpr bxzbk (contains wheat)",
            "fgp vkdfkzh xv mdgrf kpns nbmtklt zvx ttrmcgd cmzsnxn hkflr bjtrq dhjvbzc bffb cbznj hgxjcl ddjbr tzzg lpfrt szj qdgx sjsjg clqmstt lpvss mrgqf bgfzbxd tvbtx skqfq mhtzd ddtbfc fnlq vzrlps ptlf rm nplshs gzpcm vfzrhg vsnt fdrsmkkq hsxvvn lnznjm bd ssg qfql rpgbn vljjtr qpdq rjxkdp bxpjj srxphcm njcck hndj cbsp jdcc sxmph cthbr snmxl ccnvx ctmcqjf zjfsc zvbmzc bfrq kvttspgs lfqd rtvzjs cglpr zlqn mmhghd qbtdklh (contains nuts)",
            "cdmxp fgp njkjxs mqvk bjhtz njcck srxphcm mhpxlvb xkmv gmdp mrqnv ktrpqv lpvss bmfl frvr clqmstt skqfq bd ddxjjp vgkq xjqv dsshnl jfpmgbdn zlqn gcmcg kvttspgs lnznjm rtvzjs zvx qcznk tdkd hkjjt txbg hfq hndj tln hmnf dmfrzs hkflr cgthc ctmcqjf mrgqf cglpr bfrq knfs zjfsc pr dmp fgnjv bch nplshs kvlj vfzrhg bffb qdgx lp bjsmg gkgzf mqbhv xzbphmgb (contains fish, wheat, shellfish)",
            "pzmmq lffvh bch tgrx mdgrf ttgvz lcnppvn bxzbk srxphcm rnnqzt fdrsmkkq ctmcqjf clqmstt brfzm hhkl rtvzjs dsshnl dnhm gmmbtr kvttspgs kvlj mjsc zbnb czt szj ptrgd snmxl cbsp dmfrzs fnlq qdgx tln slvx zlmxj dzcsh fgp gmdp vltq xtxzvx skqfq ssg lp bfrq gkgzf vbrr stql rznd rhs cmzsnxn pzqvqs mqvk fkctmn bmfl qbtdklh jfpmgbdn vjvdx sqlqzh blrk fgnjv bjhtz ttrmcgd hmnf mrqnv gkndl bjsmg rphm cthbr vttzcj mqbhv bd sffch lgvlk zvx mrgqf hfq hgxjcl lpvss (contains wheat)",
            "kvlj skqfq pr knfs bdxflb bfrq dxbtm ddjbr tvbtx mrqnv ddxjjp xjkx hgxjcl rqhl mmhghd dmfrzs tln dknpsz ktrpqv kpns nnhjj zcpmb rm dsshnl lffvh frvr lp fgnjv vsnt qdgx xv vljjtr bch nplshs bjhtz ctmcqjf sffch tjczdt hsxvvn hkjjt xtxzvx cqtt dmp mdgrf zvx dhjvbzc mjsc sxmph lnznjm fkctmn gkgzf qpdq szj rjxkdp dnhm nnh gcmcg kvttspgs fnlq bsgnt hkflr tfssqq snmxl mqvk bd nbmtklt zlmxj lcnppvn tgrx (contains fish)",
            "lcdgsh cqtt njcck dmfrzs lgdfltvn dknpsz ltvrs tvbtx fnlq fvlq ddxjjp hfq srxphcm cghn jdcc pr nnhjj sjzlr stznpz htrm vbrr qcznk lgvlk rtvzjs rznd bkmrq zjfsc bd qdgx nbmtklt cgplh rm czt xjkx dnhm dqshc hkflr dhmrf qhknjnv ssr xkmv hhkl xv rnlscmt nxrv mqvk rnnqzt zvx rcdjqss vjvdx zlqn zbnb bbjfnr zlmxj ttrmcgd vtnmr sxmph mjsc gkgzf vzrlps skqfq ktrpqv bfrq fkm hndj xtxzvx gtpzdxx szj frvr stql mmhghd txbg bjhtz kvttspgs mhpxlvb tzzg bxpjj qpdq zxd tln fdrsmkkq bxzbk snmxl dhjvbzc (contains peanuts)",
            "sxmph qcznk rqhl bxpjj hjhvx snmxl dhmrf tln gzpcm bfrq cgthc hkflr cbznj vjvdx lgvlk ssg rphm nrpgkq njcck mfqjps zlmxj xv pzmmq bd vfzrhg bxzbk cdmxp mdgrf vbrr fgp cghn lcnppvn mrgqf tjczdt rjddcn tzzg fnlq lgdfltvn vtnmr zvx rznd hfq clqmstt nplshs srxphcm cbsp dpmdps bkmrq zcpmb cgplh dsshnl rpgbn fdrsmkkq ptrgd lnznjm qfcsfg dqshc fgnjv ctmcqjf (contains nuts)",
            "fgnjv dhmrf ssg bbjfnr gcmcg tvqvrb skqfq cgplh nbmtklt rnnqzt qbtdklh dnhm cbznj tdkd srxphcm ttgvz xkmv nrpgkq sjsxqxz gzpcm htrm gkgzf zlqn xjkx snmxl zvx mzjnpdv zcpmb rnlscmt hkflr rcdjqss bjtrq txbg dsshnl dmp rphm cbsp tzzg nnhjj bfrq hjhvx nnh zvbmzc mfqjps ctmcqjf krfnsd pzmmq vtnmr cmzsnxn fkctmn ttrmcgd mqvk knfs bch rjddcn hlqz nglclj sjsjg brfzm cqtt sjzlr xjqv (contains sesame, wheat, nuts)",
            "lpfrt njkjxs fgnjv tdkd tgrx dmfrzs mdgrf bd lnznjm gvkpnl zbnb nnh mhtzd rtvzjs kvlj rjddcn hkflr bjhtz vltq mhpxlvb lcdgsh xkmv rcdjqss dhjvbzc zvx bjtrq nglclj bfrq bbjfnr mnqcl fvlq vsnt ddtbfc cbznj rnnqzt mqvk gcmcg gkndl srxphcm gtpzdxx nbmtklt hkjjt hcgr mqbhv krfnsd dhmrf czt ctmcqjf cmzsnxn hsxvvn dxbtm ptrgd mmhghd hmnf vttzcj bxpjj txbg hfq pzqvqs slvx sjsxqxz kvttspgs cdmxp sjzlr vkdfkzh qhknjnv xtxzvx cbsp fgp nnhjj zlmxj dpmdps hgxjcl bgfzbxd qfql dzcsh vljjtr ddjbr cgjd fnlq jpx (contains nuts, wheat, fish)",
            "rqhl zbnb qfql xtxzvx ssr hlqz cgplh lgdfltvn kvlj zjfsc zlqn fnlq xzbphmgb hcgr xv bd fkctmn stznpz rtvzjs tdkd srxphcm blrk tvbtx vtnmr bgfzbxd mzjnpdv rm vljjtr mqbhv jpx kqvpgdhf dnhm lfqd mnqcl mfqjps skqfq ptlf snmxl czt qdgx hndj bxzbk qfcsfg cgthc xjqv vltq ttrmcgd lcnppvn lgvlk jdcc hkflr tgrx rjxkdp bzjsxg hsxvvn ddtbfc mqvk dmp hhkl hgxjcl xkmv ctmcqjf gzpcm fmmthkn vgkq hjhvx cmzsnxn vzrlps ttgvz pzqvqs zvx pzmmq tvqvrb rhs kpns lp tjczdt dzcsh (contains eggs, soy, shellfish)",
            "mdgrf qfql mrqnv pr ltvrs zxd xkmv lcnppvn pzqvqs hmnf cghn mfqjps hlqz cbsp rhs zvbmzc snmxl szj njkjxs bxpjj rjddcn zcpmb njcck fvlq cgjd kvlj rnlscmt frvr jdcc kpns ctmcqjf hkflr dqxcvg sjzlr sjsxqxz pzmmq dmp ssg vttzcj ttrmcgd ddjbr lpfrt srxphcm cglpr gzpcm vfzrhg mjsc lcdgsh dknpsz mqvk vzrlps dzcsh nxrv cgplh qhknjnv dnhm dxbtm bd nglclj mhtzd mzjnpdv stql bfrq kvttspgs (contains peanuts, eggs)",
            "qfcsfg gkndl kvlj tln hkflr stql brfzm bkmrq rnnqzt lffvh ssg jdcc sqlqzh knfs bmfl vzrlps fkm cqtt bzjsxg zbnb lgdfltvn snmxl bjsmg sjsxqxz cthbr zlqn tgrx mnqcl nbmtklt dsshnl bbjfnr mfqjps gkgzf cbznj bsgnt tvbtx blrk hfq qbtdklh vsnt slvx lgvlk xjqv rcdjqss ddxjjp lfqd mqvk srxphcm mrgqf vljjtr rpgbn vttzcj gcmcg zvx zvbmzc txbg rphm szj rjxkdp xjkx dmp bjtrq njcck fdrsmkkq qpdq cdmxp bch cbsp skqfq nglclj tjczdt jnmf bfrq kvttspgs ctmcqjf sxmph vltq bgfzbxd (contains wheat, fish)",
            "bffb fdrsmkkq xv bxpjj qpdq sffch xjkx hlqz hjhvx cglpr jnmf rm mfqjps mhph kvlj hgxjcl gmdp knfs fxqzh blrk tzzg vsnt ctmcqjf hsxvvn czt cgvzlqx dknpsz lgvlk hkjjt qcznk pzqvqs ddtbfc lpfrt hmnf zcpmb sjsjg lffvh cghn tvbtx kqvpgdhf mzjnpdv srxphcm qfql kpns clqmstt nrpgkq hkflr ttrmcgd dnhm zvx zxd bd zlqn bxzbk bfrq qfcsfg gzpcm tfssqq dhjvbzc hcgr lcnppvn bgfzbxd sxmph lpvss htrm vzrlps ddjbr cbsp dmp mqvk xmgprh rcdjqss rpgbn rqhl vljjtr vbrr nnhjj cdmxp pzmmq mjsc (contains fish, nuts, eggs)",
            "bsgnt rcdjqss mrqnv skqfq dsshnl fgnjv mzjnpdv fnlq zxd pzmmq sjsxqxz lcdgsh knfs mqvk bkmrq mhph nplshs lcnppvn mrgqf ttgvz rqhl vtnmr kvlj mhtzd ddjbr nnhjj tvbtx bd qbtdklh gcmcg jpx rm dhmrf cgplh lnznjm snmxl tzzg mnqcl gtpzdxx mmhghd vjvdx jnmf ctmcqjf qhknjnv nglclj cgvzlqx rnlscmt hkjjt bfrq vltq sqlqzh nrpgkq dqshc zvx hhkl xmgprh bxzbk hkflr pzqvqs cghn xjkx hgxjcl bdxflb lgvlk dmfrzs (contains nuts, sesame)",
            "hmnf pr hkflr ttgvz rjddcn fmmthkn htrm dmfrzs nxrv hndj zvx dnhm rznd nglclj ttrmcgd frvr cgjd vljjtr hkjjt dknpsz mjsc czt bdxflb zlmxj skqfq clqmstt zvbmzc ctmcqjf zbnb lnznjm dhmrf dsshnl njkjxs mhpxlvb snmxl srxphcm bd tjczdt lpfrt dmp fvlq bch mqvk jfpmgbdn pzqvqs fgnjv jdcc nnhjj blrk rnlscmt vttzcj bjtrq qfcsfg gtpzdxx knfs stql gzpcm vbrr ktrpqv gmmbtr ssr rcdjqss kvlj (contains wheat)",
            "mzjnpdv bdxflb vjvdx rqhl gmdp hhkl skqfq lcnppvn sxmph ddtbfc nnhjj tzzg zlmxj pzmmq zvx brfzm mqvk mjsc vsnt mdgrf dknpsz hsxvvn bfrq qcznk dmp lgdfltvn clqmstt lnznjm dmfrzs xjqv mmhghd ttgvz hkflr bjsmg htrm cbznj sqlqzh tfssqq cmzsnxn pzqvqs bd dsshnl nglclj bjhtz cthbr fkctmn slvx cqtt zcpmb gkndl frvr vfzrhg lgvlk snmxl fkm stql srxphcm vtnmr (contains soy, sesame, nuts)",
            "lnznjm dnhm dzcsh snmxl hjhvx njcck fnlq fvlq bsgnt lgvlk qhknjnv hkjjt mdgrf cdmxp bfrq ddxjjp fmmthkn tfssqq dmp ttrmcgd mfqjps bjsmg zlmxj kpns hkflr ktrpqv hfq ctmcqjf rm jpx kvlj mqvk bmfl knfs vsnt hhkl tjczdt zvx rjxkdp hgxjcl rnnqzt srxphcm (contains sesame, peanuts)",
            "fkctmn bffb tzzg sjsxqxz rpgbn gkndl gcmcg lpfrt ctmcqjf qfql bzjsxg bfrq ccnvx qhknjnv xjqv sffch krfnsd jpx pzqvqs mqvk frvr zvx ssg dxbtm kvlj czt hmnf srxphcm bd snmxl xkmv fvlq gkgzf ssr lcnppvn lpvss fnlq cgvzlqx rm vfzrhg bxzbk dqxcvg vtnmr cglpr rtvzjs mnqcl gvkpnl zlmxj mqbhv fgp (contains shellfish, sesame, nuts)",
            "nbmtklt bch srxphcm tzzg hkflr qbtdklh fkctmn nrpgkq rpgbn kpns dsshnl xv fgp xzbphmgb mrqnv stznpz cgjd lcnppvn qhknjnv cdmxp vljjtr ddtbfc hhkl hfq hcgr ssg zcpmb gkndl brfzm gmmbtr lpvss kqvpgdhf rjddcn vkdfkzh slvx qfql ssr nxrv bxpjj bd cghn dhjvbzc snmxl zvx rnlscmt sffch dhmrf cgplh gvkpnl ltvrs ctmcqjf fnlq mzjnpdv mfqjps fkm zbnb cgvzlqx nplshs rtvzjs cqtt rhs zlmxj cbsp cthbr jpx lpfrt mqvk mdgrf (contains soy)",
            "bch gmmbtr sjsxqxz jdcc qbtdklh mqvk hsxvvn tvbtx cqtt zlqn dzcsh gzpcm njcck zlmxj dxbtm mhph slvx bjsmg kvlj hfq hmnf bxzbk vsnt jnmf bdxflb zxd skqfq nplshs ptlf hkflr qfcsfg rjddcn bfrq qdgx vttzcj szj cgvzlqx tfssqq bd hcgr rphm mrgqf pzmmq kqvpgdhf snmxl hlqz fvlq mhtzd xmgprh bffb ctmcqjf krfnsd dqshc lpvss fgnjv srxphcm hjhvx hgxjcl dnhm htrm lnznjm vtnmr bsgnt rtvzjs frvr lcnppvn (contains nuts, peanuts, soy)",
            "blrk lcdgsh nbmtklt xtxzvx ddtbfc kqvpgdhf fdrsmkkq srxphcm snmxl ktrpqv nrpgkq ltvrs dsshnl cglpr cmzsnxn sjzlr kvttspgs rnlscmt qdgx krfnsd dhmrf qfcsfg rnnqzt lgdfltvn rtvzjs bsgnt vsnt nplshs bffb mrqnv vfzrhg bxpjj mnqcl cbznj lnznjm tdkd xjkx cthbr jpx ddjbr bd jnmf fnlq dmp hkflr njkjxs hjhvx sjsjg szj cghn mqvk fxqzh lpvss bfrq rhs ddxjjp bxzbk hndj zvx nnhjj tjczdt rpgbn cqtt (contains shellfish)",
            "vbrr lpvss rm lcnppvn pr xtxzvx srxphcm bfrq fgp njcck snmxl qhknjnv gtpzdxx bzjsxg dxbtm ttrmcgd ccnvx nglclj rhs tvbtx fkctmn ssg ssr njkjxs fnlq cbsp zvx rphm hgxjcl sqlqzh hkjjt dqshc mqvk nbmtklt kvttspgs qfcsfg qpdq rpgbn dpmdps vsnt hsxvvn dhjvbzc hfq bd gkgzf skqfq ctmcqjf rjddcn hcgr clqmstt tzzg mqbhv sxmph zcpmb ddtbfc nplshs xkmv sjsxqxz ddjbr nnh cthbr dmfrzs rnlscmt bxzbk tln rjxkdp pzmmq zjfsc mrgqf (contains peanuts, soy)",
            "hsxvvn hjhvx cgthc pr fkctmn rnlscmt qdgx bdxflb vttzcj rcdjqss mhpxlvb cdmxp lpfrt rznd xv ktrpqv cgplh qpdq hmnf xjkx bfrq ptrgd mhtzd vljjtr rnnqzt lgdfltvn lpvss cqtt vtnmr snmxl ssg sjzlr ctmcqjf vgkq zvbmzc fvlq tgrx hkjjt lnznjm dqxcvg stznpz hkflr zjfsc bmfl bffb bkmrq xjqv zcpmb rtvzjs dsshnl lp vjvdx hndj fmmthkn vzrlps lcdgsh ssr mqvk mfqjps tln sffch mmhghd dpmdps lgvlk bzjsxg ltvrs brfzm dhjvbzc nglclj vsnt srxphcm fkm ddtbfc ddjbr ttrmcgd bd (contains nuts, wheat, soy)",
            "nglclj kvlj bsgnt xjqv qhknjnv bjtrq zcpmb fkm cthbr lp rqhl rhs dmp bxzbk fgp mmhghd sjzlr vgkq ltvrs xmgprh htrm bd hcgr kqvpgdhf ttrmcgd nnhjj qdgx vkdfkzh zlqn srxphcm hmnf stznpz vltq nnh vfzrhg tzzg knfs xhjkg rnlscmt ccnvx rpgbn ssg bdxflb mqvk zbnb qpdq dsshnl lnznjm bmfl sqlqzh dhjvbzc mdgrf slvx mqbhv nrpgkq xkmv bxpjj cbznj skqfq ddjbr fdrsmkkq tln lffvh zxd ptrgd gkndl qfql bbjfnr cghn bzjsxg bch ctmcqjf fnlq bfrq mnqcl xjkx njcck hgxjcl lpfrt gcmcg jnmf zvx kpns mzjnpdv rznd hkflr vjvdx bffb vljjtr (contains peanuts, wheat)"
        };
#endif

        #endregion Data
    }
}
