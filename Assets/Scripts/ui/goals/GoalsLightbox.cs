using System.Collections.Generic;
using System.Linq;
using Mgl;
using net.gubbi.goofy.state;
using net.gubbi.goofy.ui.menu.ingame;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui.goals {
    public class GoalsLightbox : InGameLightbox {

        public Button closeButton;
        
        private IEnumerable<GoalUi> goalUis;
        private GridLayoutGroup gridLayoutGroup;

        protected override void Awake() {
            goalUis = GetComponentsInChildren<GoalUi> ();
            gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();
            base.Awake();
        }

        public override void show() {
            base.show();

            closeButton.interactable = true;            
            IList<Goal> goals = GameState.Instance.StateData.Goals;
            updateGoalsUi(goals);
        }

        public override void close() {
            base.close();
            
            closeButton.interactable = false;
        }
        
        public override void back() {
            close();
        }

        private void updateGoalsUi(IList<Goal> goals) {
            //TODO: hack!
            int cellHeight = I18n.Instance.__(goals[0].text).Length >= 40 ? 80 : 60;  
            gridLayoutGroup.cellSize = new Vector2(gridLayoutGroup.cellSize.x, cellHeight);
            
            for(int i = 0; i < goalUis.Count(); i++) {                
                if (goals.Count > i) {
                    goalUis.ElementAt(i).setEnabled(true);    
                    goalUis.ElementAt(i).setUi(goals.ElementAt(i));
                } else {
                    goalUis.ElementAt(i).setEnabled(false);
                }
            }
        }
    }
}