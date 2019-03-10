using System;
using System.Collections.Generic;

namespace Sierra.AGPW.HackerSim
{
    
    /// <summary>
    /// Container for all scenarios used by this game
    /// </summary>
    class InteractionSet
    {    
        private List<Scenario> _scenarios;

        public InteractionSet()
        {
            _scenarios = new List<Scenario>();
        }
        public InteractionSet(List<Scenario> scenarios)
        {
            _scenarios = scenarios;
        }

        /// <summary>
        /// Plays a scenario with the same keywords entered
        /// </summary>
        /// <param name="keywords"></param>
        public void PlayScenarioMatching(Keyword[] keywords)
        {
            foreach (Scenario scenario in _scenarios)
            {
                if (scenario.Matches(keywords)) scenario.Play();
            }
        }
        public void AddScenario(Scenario newScenario)
        {
            _scenarios.Add(newScenario);
        }
        public Scenario AddScenario(Keyword[] keywords)
        {
            var newScenario = new Scenario(keywords, this);
            AddScenario(newScenario);
            return newScenario;
        }
        public void RemoveScenario(Scenario scenario)
        {
            try
            {
                _scenarios.Remove(scenario);
            }
            catch(Exception)
            {
                Console.WriteLine("Scenarrio dosen't exist in this interaction set. Cannot remove it.");
            }
        }
        public int GetScenarioCallCount(string scenarioName)
        {
            for (int i = 0; i < _scenarios.Count; i++)
            {
                var scenario = _scenarios[i];
                if (scenario.Name == scenarioName) return scenario.CallCount;
            }

            // If reached this far, there was no matching scenario: throw exception
            throw new ScenarioNotFoundException();
        }
    }
    /// <summary>
    /// Checks against other scenarios in the same set
    /// </summary>
    class Scenario
    {
        #region Vars
        private readonly InteractionSet _parentSet;
        private List<Case> _cases = new List<Case>();
        private Case _defaultCase = null;
        private readonly Keyword[] _keywords;
        public int CallCount {get; private set;}
        public InteractionSet Parent {get {return _parentSet;}}
        public string Name 
        {
            get
            {
                return GenerateName(_keywords);
            }
        }
        #endregion
        
        public Scenario(Keyword[] keywords, InteractionSet parentSet)
        {
            _keywords = keywords;
            _parentSet = parentSet;
        }

        public Case AddCase(CaseData data)
        {
            var newCase = new Case(data, this);
            _cases.Add(newCase);
            return newCase;
        }
        public Case SetDefaultCase(CaseData data)
        {
            var newCase = new Case(data, this);
            _defaultCase = newCase;
            return newCase;
        }
        public void Play()
        {
            Console.WriteLine("Playing scenario with keywords {0}", Name);

            for (int i = 0; i < _cases.Count; i++)
            {
                if (_cases[i].Check())
                {
                     _cases[i].Run();
                     return;
                }
            }

            _defaultCase.Run();
        }
        public bool Matches(Keyword[] keywords)
        {
            // For each of the passed keywords
            for (int i = 0; i < keywords.Length; i++)
            {
                var thisMatches = false;

                // Check this scenario's keywords
                for (int j = 0; j < _keywords.Length; j++)
                {
                    if (keywords[i] == _keywords[j])
                    {
                        thisMatches = true;
                        break;
                    }
                }

                // If one does not match, return false
                if (!thisMatches) return false;
            }

            // If all match, return true;
            return true;
        }
        public static string GenerateName(Keyword[] keywords)
        {
            var name = "";
            foreach (Keyword keyword in keywords)
            {
                name += keyword.ToString() + " ";
            }
            return name;
        }
    }
    /// <summary>
    /// Instructions carried out by a scenario. Contains conditions (previous scenario openings)
    /// and outcomes (text to display, score modifiers and defeat conditions).
    /// </summary>
    class Case
    {
        private readonly Scenario _parentScenario;
        private readonly List<CaseCondition> _conditions;
        private readonly int _scoreValue;
        private readonly string _outcome;

        public Scenario Parent {get {return _parentScenario;}}

        public Case(CaseData data, Scenario parentScenario)
        {
            _conditions = data.Conditions;
            _scoreValue = data.ScoreValue;
            _outcome = data.Outcome;
            _parentScenario = parentScenario;
        }

        public CaseCondition AddCondition(string scenarioName, int count)
        {
            var newCondition = new CaseCondition(scenarioName, count, this);
            _conditions.Add(newCondition);
            return newCondition;
        }
        public bool Check()
        {
            foreach (CaseCondition condition in _conditions)
            {
                if (condition.IsMet()) return true;
            }
            return false;
        }
        public void Run()
        {
            Console.WriteLine(_outcome);
            Console.WriteLine("Score + {0} points", _scoreValue);
        }
    }
    /// <summary>
    /// A case that has no conditions. 
    /// </summary>
    class DefaultCase : Case
    {
        /// <summary>
        /// Creates a default case. Erases any conditions entered.
        /// </summary>
        /// <param name="data"></param>
        public DefaultCase(CaseData data, Scenario parentScenario)
            : base(new CaseData(new List<CaseCondition>(), data.ScoreValue, data.Outcome), parentScenario)
        {

        }
    }
    /// <summary>
    /// Used to pass data into a <see cref = "Case"/>
    /// </summary>
    struct CaseData
    {
        public readonly List<CaseCondition> Conditions;
        public readonly int ScoreValue;
        public readonly string Outcome;

        public CaseData(List<CaseCondition> conditions, int scoreValue, string outcome)
        {
            Conditions = conditions;
            ScoreValue = scoreValue;
            Outcome = outcome;
        }
    }
    /// <summary>
    /// Used by cases to check if a scenario has been called a certain number of times.
    /// </summary>
    struct CaseCondition
    {
        private string _scenarioName;
        private int _count;
        private Case _parentCase;

        public CaseCondition(string scenarioName, int count, Case parentCase)
        {
            _scenarioName = scenarioName;
            _count = count;
            _parentCase = parentCase;
        }

        public bool IsMet()
        {
            return _parentCase.Parent.Parent.GetScenarioCallCount(_scenarioName) >= _count;
        }
    }

}