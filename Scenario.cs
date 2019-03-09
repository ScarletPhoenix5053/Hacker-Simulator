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
            
        }
        public void AddScenario(Scenario newScenario)
        {
            _scenarios.Add(newScenario);
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
    }
    /// <summary>
    /// Checks against other scenarios in the same set
    /// </summary>
    class Scenario
    {
        private readonly List<Case> _cases;
        private readonly Case _defaultCase;
        private readonly Keyword[] _keywords;
        public int CallCount {get; private set;}
        
        public Scenario(Case defaultCase, List<Case> otherCases, Keyword[] keywords)
        {
            _defaultCase = defaultCase;
            _cases = otherCases;
            _keywords = keywords;
        }

        public void Play()
        {
            
        }
        public bool Matches(Keyword[] keywords)
        {
            // For each of this scenario's keywords
            for (int i = 0; i < _keywords.Length; i++)
            {
                var thisMatches = false;

                // Check against passed keywords
                for (int j = 0; j < keywords.Length; i++)
                {
                    if (_keywords[i] == keywords[j])
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
    }
    /// <summary>
    /// Instructions carried out by a scenario. Contains conditions (previous scenario openings)
    /// and outcomes (text to display, score modifiers and defeat conditions).
    /// </summary>
    class Case
    {
        private readonly List<CaseCondition> _conditions;
        private readonly int _scoreValue;
        private readonly string _outcome;

        public Case(CaseData data)
        {
            _conditions = data.Conditions;
            _scoreValue = data.ScoreValue;
            _outcome = data.Outcome;
        }

        public bool Check()
        {
            foreach (CaseCondition condition in _conditions)
            {
                if (condition.IsMet()) return true;
            }
            return false;
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
        public DefaultCase(CaseData data)
            : base(new CaseData(new List<CaseCondition>(), data.ScoreValue, data.Outcome))
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
        private Scenario _scenario;
        private int _count;

        public CaseCondition(Scenario scenario, int count)
        {
            _scenario = scenario;
            _count = count;
        }

        public bool IsMet()
        {
            return _scenario.CallCount >= _count;
        }
    }

}