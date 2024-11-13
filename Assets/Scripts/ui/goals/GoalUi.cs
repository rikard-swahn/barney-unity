using Mgl;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.state;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui.goals {
    public class GoalUi : MonoBehaviour {

        private TextMeshProUGUI text;
        private Image[] strikethroughImages;

        private void Awake() {
            text = GetComponentInChildren<TextMeshProUGUI>();
            strikethroughImages = GetComponentsInChildren<Image>();
        }

        public void setEnabled(bool enabled) {
            gameObject.SetActive(enabled);
        }

        public void setUi(Goal goal) {
            text.text = I18n.Instance.__(goal.text);            
            strikethroughImages.ForEach(i => i.enabled = goal.complete);            
        }
    }
}