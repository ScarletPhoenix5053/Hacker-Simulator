using System;
using System.Collections.Generic;
using System.Threading;
using System.Text.RegularExpressions;

namespace Sierra.AGPW.HackerSim
{
    public class Program
    {
        public static Scribe Scribe;
        public static ScoreTracker Score;
        public static bool Running = true;
        private static string[] _exitTerms = new string[]
        {
            // If the player types any of these strings, the game will exit.
            "exit",
            "quit",
            "return",
            "esc",
            "escape"
        };
        private static string[] _resetLoopTerms = new string[]
        {
            // If the player types any of these strings, the game will reset.
            "reset",
            "restart"
        };
        static void Main(string[] args)
        {
            // INIT SCRIBE
            var scribeSpeed = new ScribeWriteSpeedTemplate(300, 25, 5);
            Scribe = new Scribe(scribeSpeed);
            
            // INIT READER
            var inputReader = new InputReader();
            var keySets = new List<KeySet>();

            keySets.Add(new KeySet(Keyword.Create, new char[]{'q','w','a','s','z','x'}));
            keySets.Add(new KeySet(Keyword.Donate, new char[]{'o','p','l',';','.','/'}));
            keySets.Add(new KeySet(Keyword.Cult, new char[]{'e','r','d','f','c','v'}));            
            keySets.Add(new KeySet(Keyword.Eat, new char[]{'1','2','3','4','5','6'}));
            keySets.Add(new KeySet(Keyword.Virus, new char[]{'t','y','g','h','b','n'}));
            keySets.Add(new KeySet(Keyword.TheRich, new char[]{'u','i','j','k','m',','}));
            
            foreach (KeySet keySet in keySets)
            {
                inputReader.AddKeySet(keySet);
            } 

            // INIT SCORE
            Score = new ScoreTracker();
            
            // INIT INTERACTIONS
            var interactionSet = new InteractionSet();

            // Defaults
            var createCultScenario = interactionSet.AddScenario(new Keyword[] {Keyword.Create, Keyword.Cult});
            createCultScenario.SetDefaultCase(
                new CaseData(
                    null,
                    320,
                    "You come accross the transcript for a religeous manuscript so utterly obsurd, only \n"+
                    "the perfect blend of a yes man, basement dweller and alex jones fanatic would ever believe it. \n" +
                    "You find that exact person and send the transcript to them, watch in glee as the antivax and flat earth movments \n" +
                    "are brushed to the wayside by this tidal wave of stupidity."
                )
            );
            var donateCultScenario = interactionSet.AddScenario(new Keyword[] {Keyword.Donate, Keyword.Cult});
            donateCultScenario.SetDefaultCase(
                new CaseData(
                    null,
                    10,
                    "You donate to a random cult. They use your money to grab a feed at maccas."
                )
            );            
            var createDonateScenario = interactionSet.AddScenario(new Keyword[] {Keyword.Create, Keyword.Donate});
            createDonateScenario.SetDefaultCase(
                new CaseData(
                    null,
                    150,
                    "You transfer several small donations of a billion dollars from a variety of swiss bank accounts into you own. \n" +
                    "Your lack of subtletey means several global powers are activley hunting you down. Good luck."
                )
            );
            var donateEatScenario = interactionSet.AddScenario(new Keyword[] {Keyword.Donate, Keyword.Eat});
            donateEatScenario.SetDefaultCase(
                new CaseData(
                    null,
                    10,
                    "You donate to a homeless shelter. That was very nice of you."
                )
            );            
            var createEatScenario = interactionSet.AddScenario(new Keyword[] {Keyword.Create, Keyword.Eat});
            createEatScenario.SetDefaultCase(
                new CaseData(
                    null,
                    10,
                    "You use the overwheming power of this machine to order some kfc through uber-eats. Nice."
                )
            );                        
            var eatVirusScenario = interactionSet.AddScenario(new Keyword[] {Keyword.Eat, Keyword.Virus});
            eatVirusScenario.SetDefaultCase(
                new CaseData(
                    null,
                    200,
                    "You order an experiment in which someone is subjected to the black plauge. The test subject breaks \n" +
                    "loose and the virus is brought out into the world."
                )
            );
            var createVirusScenario = interactionSet.AddScenario(new Keyword[] {Keyword.Create, Keyword.Virus});
            createVirusScenario.SetDefaultCase(
                new CaseData(
                    null,
                    110,
                    "The machine hums as it trawls through the databases of medical research facilitie accross the globe. \n" +
                    "It finds the perfect storm at a reseach centre in Busan. A small tweak to some doses 'accidentally' \n" +
                    "creates a zombie virus. It stays dormant and contained... for now."
                )
            );
            var createRichScenario = interactionSet.AddScenario(new Keyword[] {Keyword.Create, Keyword.TheRich});
            createRichScenario.SetDefaultCase(
                new CaseData(
                    null,
                    50,
                    "With some stealthy manipulation of automated taxing algoryhtms, the rich suddenly grow richer."
                )
            );
            var donateRichScenario = interactionSet.AddScenario(new Keyword[] {Keyword.Donate, Keyword.TheRich});
            donateRichScenario.SetDefaultCase(
                new CaseData(
                    null,
                    50,
                    "For whatever reason you decide to drain funds from the poor and send them to the 1%, you monster. \n" +
                    "Together they purcase a small tropical island and build palace made of diamond. God knows why they thought that was necessary."
                )
            );
            var eatTheRichScenario = interactionSet.AddScenario(new Keyword[] {Keyword.Eat, Keyword.TheRich});
            eatTheRichScenario.SetDefaultCase(
                new CaseData(
                    null,
                    250,
                    "You target a group of extreme anarchists with just the right content. They start a cannabalistic renegade group \n" +
                    "that attempts to raid the rich and barbeque their delicious belly fat. The movement is squashed instantly, and the fat \n" +
                    "cats live to banquet another day."
                )
            ); 
            var cultTheRichScenario = interactionSet.AddScenario(new Keyword[] {Keyword.Cult, Keyword.TheRich});
            cultTheRichScenario.SetDefaultCase(
                new CaseData(
                    null,
                    200,
                    "You make a random cult ultra rich. Thier national govornment is buying none of it and swats the movement aside. \n" +
                    "Said goveornment now has its eye on you."
                )
            ); 
            var eatCultScenario = interactionSet.AddScenario(new Keyword[] {Keyword.Eat, Keyword.Cult});
            eatCultScenario.SetDefaultCase(
                new CaseData(
                    null,
                    500,
                    "You manage to convince a great deal of cults that biting one another is somehow benificial. The internet blows up over it."
                )
            );

            // Variants
            var dcScen_Cc1 = donateCultScenario.AddCase(
                new CaseData(
                    new List<CaseCondition>(),
                    4600,
                    "You divert funds into the account of the leader of the new riduculed cult you started. Turns out the cultists were onto something, \n" +
                    "as the elaborate ritual they perform with your donation leads to aliens invading the earth."
                )
            );
            dcScen_Cc1.AddCondition(Scenario.GenerateName(new Keyword[]{Keyword.Create, Keyword.Cult}), 1);

            var etrScen_Cv1_Ctr1s = eatTheRichScenario.AddCase(
                new CaseData(
                    new List<CaseCondition>(),
                    5050,
                    "By targeting a group of extreme anarchists with the perfect blend of propaganda, you send them into a feeding frenzy \n" +
                    "targeted at the rich elite. A cell of this movment in korea tears through the facility containing the zombie virus. \n" +
                    "Apocalypse ensues."
                )
            );
            etrScen_Cv1_Ctr1s.AddCondition(Scenario.GenerateName(new Keyword[]{Keyword.Create, Keyword.Virus}), 1);
            etrScen_Cv1_Ctr1s.AddCondition(Scenario.GenerateName(new Keyword[]{Keyword.Create, Keyword.TheRich}), 1);

            var etrScen_Cv1_Dtr1 = eatTheRichScenario.AddCase(
                new CaseData(
                    new List<CaseCondition>(),
                    3050,
                    "By targeting a group of extreme anarchists with the perfect blend of propaganda, you send them into a feeding frenzy \n" +
                    "targeted at the rich elite. A cell of this movment in korea tears through the facility containing the zombie virus. \n" +
                    "Apocalypse ensues."
                )
            );
            etrScen_Cv1_Dtr1.AddCondition(Scenario.GenerateName(new Keyword[]{Keyword.Create, Keyword.Virus}), 1);
            etrScen_Cv1_Dtr1.AddCondition(Scenario.GenerateName(new Keyword[]{Keyword.Donate, Keyword.TheRich}), 1);

            var etrScen_Ctr1_Dtr1 = eatTheRichScenario.AddCase(
                new CaseData(
                    new List<CaseCondition>(),
                    1470,
                    "You target a group of extreme anarchists with just the right content. They start a cannabalistic renegade group \n" +
                    "Due to outrage at the audacity of the world's elite to build a diamond palace while others struggle to get by, \n" +
                    "the movement grows incredibly large. Law enforcement fails to stop raid after raid, and militaries accross the world \n" +
                    "get involved, leading to the breakout of war accross the globe."
                )
            );  
            etrScen_Ctr1_Dtr1.AddCondition(Scenario.GenerateName(new Keyword[]{Keyword.Create, Keyword.TheRich}), 1);
            etrScen_Ctr1_Dtr1.AddCondition(Scenario.GenerateName(new Keyword[]{Keyword.Donate, Keyword.TheRich}), 1);

            var etrScen_Dtr1 = eatTheRichScenario.AddCase(
                new CaseData(
                    new List<CaseCondition>(),
                        450,
                        "You target a group of extreme anarchists with just the right content. They start a cannabalistic renegade group \n" +
                        "Outraged by the abusurdity of the diamond palace built by the global elite, the movement grows surprisingly large. \n" +
                        "Though beaten back by armed law enforcement, the movement dosen't completley die."
                )
            );
            etrScen_Dtr1.AddCondition(Scenario.GenerateName(new Keyword[]{Keyword.Donate, Keyword.TheRich}), 1);

            var etrScen_Ctr1 = eatTheRichScenario.AddCase(
                new CaseData(
                    new List<CaseCondition>(),
                        450,
                        "You target a group of extreme anarchists with just the right content. They start a cannabalistic renegade group \n" +
                        "Outraged by inequality, the movement grows surprisingly large. \n" +
                        "Though beaten back by armed law enforcement, the movement dosen't completley die."
                )
            );
            etrScen_Ctr1.AddCondition(Scenario.GenerateName(new Keyword[]{Keyword.Create, Keyword.TheRich}), 1);
            
            var ecScen_eV = eatCultScenario.AddCase(
                new CaseData(
                    new List<CaseCondition>(),
                        1280,
                        "The loonies in the cult you made start biting each other as part of their rituals. One of them catches the plauge and \n" +
                        "spreads it to everyone. The plauge spreads wildly after this."
                )
            );
            etrScen_Ctr1.AddCondition(Scenario.GenerateName(new Keyword[]{Keyword.Eat, Keyword.Virus}), 1);
            etrScen_Ctr1.AddCondition(Scenario.GenerateName(new Keyword[]{Keyword.Create, Keyword.Cult}), 1);

            // DISPLAY GAME START
            Scribe.WriteLineFast("Init CrowdControlGlobal_0.13.1456b.3f6.exe");
            Scribe.WriteLine("please verify your identity");
            Scribe.WriteFast(" > ");
            Scribe.Write("************");
            Scribe.Pause(300);       
            Scribe.WriteLine("... ");
            Scribe.WriteLineFast("welcome administrator");
            Scribe.WriteNewLine();
            Scribe.WriteLineFast("Press any key to begin");
            Console.ReadKey();


            // GAME LOOP
            while (Running)
            {
                // READ KEYWORDS   
                Scribe.WriteNewLine();             
                Scribe.Write(" > ");
                var input = Console.ReadLine();

                // CHECK IF EXIT WORD
                foreach (string exitTerm in _exitTerms)
                {
                    if (input.ToLower() == exitTerm.ToLower()) 
                    {
                        // Exit game loop if an exit term is entered
                        Running = false;

                        Scribe.WriteLineFast("Exiting program.");
                    }
                }
                if (!Running) break;

                // PROCESS INPUT INTO KEYWORDS
                var keywords = inputReader.CheckInput(input);
                for (int i = 0; i < keywords.Length; i++)
                {
                    var keyword = keywords[i];
                    Console.Write(keyword.ToString() + " ");
                }
                Console.WriteLine();
                Console.WriteLine();
                
                // RUN INTERACTION
                // Ensure there are two keywords before running
                if (keywords.Length <= 1)
                {
                    Scribe.WriteFast("Command not recognized. Please pass an additional parameter.");
                }
                else
                {
                    interactionSet.PlayScenarioMatching(keywords);
                }
            }
        }
    }
}
