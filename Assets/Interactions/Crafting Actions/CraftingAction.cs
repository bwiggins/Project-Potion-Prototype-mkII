using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Assets.Interactions
{
    //a crafting action is logic around the particular combos of gestures a player will be asked to make. Completing the crafting action means completing the requirement to craft an item.
    public class CraftingAction 
    {
        protected bool _completed = false;
        public bool isCompleted
        {
            get { return _completed; }
            protected set { _completed = value; }
        }

        private List<string> steps;//list of things to do
        private int currentStep = 0;//place in list
        public string getCurrentStep(){ return steps[currentStep]; }

        /// <summary>
        /// updates the state of the minigame based on current input
        /// </summary>
        /// <param name="input"></param>
        public void Update()
        {
            //TODO: right now I will use a placeholder of testing against the enter button

            if (Input.GetKeyDown("Enter"))
            {
                Debug.Log("Enter");
            }
        }

        //todo: have a function to translate strings into programatical pieces
        public CraftingAction(List<string> craftingSteps)
        {
            steps = craftingSteps;
        }
    }
}