using System;
using System.Threading;
using System.Text.RegularExpressions;

namespace Sierra.AGPW.HackerSim
{
    class Program
    {
        public static Scribe Scribe;
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
        static void Main(string[] args)
        {
            // INIT SCRIBE
            var scribeSpeed = new ScribeWriteSpeedTemplate(300, 25, 5);
            Scribe = new Scribe(scribeSpeed);
            
            // INIT READER
            var inputReader = new InputReader();
            var keySet1 = new KeySet(Keyword.Create, new char[]{'d','f','g'});
            var keySet2 = new KeySet(Keyword.Cult, new char[]{'e','r','s'});
            
            inputReader.AddKeySet(keySet1);
            inputReader.AddKeySet(keySet2);
            
            // INIT INTERACTIONS
            var interactionSet = new InteractionSet();

            var createCultScenario = interactionSet.AddScenario(new Keyword[] {Keyword.Create, Keyword.Cult});
            var createCultCaseDefault = createCultScenario.SetDefaultCase(
                new CaseData(
                    null,
                    100,
                    "Started a lunatic cult."));

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
                    interactionSet.PlayScenarioMatching(inputReader.CheckInput(Console.ReadLine()));
                }
            }
        }        
    }
}
