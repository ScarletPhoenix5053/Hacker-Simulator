using System;
using System.Collections.Generic;

namespace Sierra.AGPW.HackerSim
{
    /// <summary>
    /// Reads key-bashedinput from the player and translates it into keywords.
    /// </summary>
    class InputReader
    {
        public bool DebugLog {get; set;}
        private List<KeySet> KeySets;

        public InputReader()
        {
            KeySets = new List<KeySet>();           

            KeySetsShareNoChars();
        }

        /// <summary>
        /// Prevents keysets from having any matching characters
        /// </summary>
        /// <returns></returns>
        private bool KeySetsShareNoChars()
        {
            // For each Keyset
            for (int ka = 0; ka < KeySets.Count; ka++)
            {
                var keySet = KeySets[ka];

                // Compare against the keys in all other keysets, and ensure none match
                for (int kb =  0; kb < KeySets.Count; kb++)
                {
                    if (ka == kb) continue;

                    foreach (char keyA in KeySets[ka].Keys)
                    {
                        foreach (char KeyB in KeySets[kb].Keys)
                        {
                            if (keyA == KeyB) throw new DuplicateKeyException(
                                "Duplicate keys fond in KeySets " + ka + " and " + kb +
                                ". (" + keyA + " and " + KeyB);                            
                        }
                    }
                }
            }

            // If method reaches end without throwing, there are no duplicates
            return true;
        }
        /// <summary>
        /// Matches characters in a string to keywords using <see cref = "KeySets"/> and returns them.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>An array of <see cref = "Keyword"/></returns>
        public Keyword[] CheckInput(string input)
        {
            var inputChars = input.ToCharArray();
            var keywords = new List<Keyword>();

            // For each input set
            for (int keySetIndex = 0; keySetIndex < KeySets.Count; keySetIndex++)
            {
                var currentKeySet = KeySets[keySetIndex];
                var oneKeyMatched = false;

                // For each char in set keyset
                for (int keyIndex = 0; keyIndex < currentKeySet.Keys.Length; keyIndex++)
                {
                    if (oneKeyMatched) break;

                    var currentKey = currentKeySet.Keys[keyIndex];

                    // For each input (char) in string
                    for (int inputIndex = 0; inputIndex < inputChars.Length; inputIndex++)
                    {
                        var currentInputChar = inputChars[inputIndex];

                        if (currentKey == currentInputChar)
                        {
                            // Matched a key: stop checking this KeySet
                            if (DebugLog) Console.WriteLine("Match between {0} and {1}", currentInputChar, currentKey);
                            oneKeyMatched = true;
                            break;
                        }
                        else
                        {
                            if (DebugLog) Console.WriteLine("No match between {0} and {1}", currentInputChar, currentKey);
                        }
                    }
                }

                if (oneKeyMatched) keywords.Add(currentKeySet.Keyword);
            }
            return keywords.ToArray();
        }
        /// <summary>
        /// Adds a keyset. Causes program to fail if the new keyset contains keys found in older keysets.
        /// </summary>
        /// <param name="newKeySet"></param>
        public void AddKeySet(KeySet newKeySet)
        {
            KeySets.Add(newKeySet);
            KeySetsShareNoChars();
        }
    }
    /// <summary>
    /// Used by <see cref = "InputReader"/>. Contains a keyword and an array of keys (as chars) that point to it.
    /// </summary>
    struct KeySet
    {
        public Keyword Keyword { get; private set;}
        public char[] Keys {get; set;}

        public KeySet(Keyword keyword, char[] keys)
        {
            Keyword = keyword;
            Keys = keys;
        }
    }
    enum Keyword 
    {
        Create,
        Cult
    }
}