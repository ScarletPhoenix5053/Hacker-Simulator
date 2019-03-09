using System;

namespace Sierra.AGPW.HackerSim
{
    /// <summary>
    /// Tracks the score of the player through a game session.
    /// </summary>
    class ScoreTracker
    {
        private int _score;
        public ScoreMultiplier Multiplier = new ScoreMultiplier(1f);
        
        /// <summary>
        /// Increases this object's score value
        /// </summary>
        /// <param name="points"></param>
        public void Increase(int points)        
        {
            _score += points;
        }
        /// <summary>
        /// Decreases this object's sore value. Cannot decrease it below 0.
        /// </summary>
        /// <param name="points"></param>
        public void Decrease(int points)
        {
            _score -= points;
            if (_score < 0)
            {
                _score = 0;
            }
        }
    }
    /// <summary>
    /// Struct for handling multiplier values inside <see cref = "ScoreMultiplier"/>
    /// </summary>
    struct ScoreMultiplier
    {
        private float _multiplier;

        public float Value {get {return _multiplier;}}
        
        public ScoreMultiplier(float initialMultiplier)
        {
            _multiplier = initialMultiplier;
        }

        public void Increase(float value)
        {
            _multiplier += value;
        }
        public void Decrease(float value)
        {
            _multiplier -= value;
        }
        public void Reset()
        {
            _multiplier = 0;
        }
    }
    
}