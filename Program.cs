using System;
using System.Threading;
using System.Text.RegularExpressions;

namespace Sierra.AGPW.HackerSim
{
    class Program
    {
        static void Main(string[] args)
        {
            var scribeSpeed = new ScribeWriteSpeedTemplate(300, 32, 10);
            var scribe = new Scribe(scribeSpeed);
            
            var inputReader = new InputReader();
            var keySet1 = new KeySet(Keyword.Create, new char[]{'d','f','g'});
            var keySet2 = new KeySet(Keyword.Cult, new char[]{'e','r','s'});
            
            inputReader.AddKeySet(keySet1);
            inputReader.AddKeySet(keySet2);

            var keywords = inputReader.CheckInput(Console.ReadLine());
            foreach (Keyword keyword in keywords)
            {
                Console.Write(keyword.ToString() + " ");
            }
            Console.WriteLine();
            
            var interactionSet = new InteractionSet();

            var defualtData = new CaseData(null, 1000, "Started a lunatic cult");
            var defaultCase = new DefaultCase(defualtData);

            var case1Condition1 = new CaseCondition();

            var scenario1 = new Scenario(new Keyword[]{Keyword.Create}, defaultCase, interactionSet);
            var scenario2 = new Scenario(new Keyword[]{Keyword.Cult, Keyword.Create}, defaultCase, interactionSet);
            interactionSet.AddScenario(scenario1);
            interactionSet.AddScenario(scenario2);

            interactionSet.PlayScenarioMatching(keywords);
        }        
    }
}
