using Mgl;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.util;
using ui;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.say.conversation {
    public class ConversationChecks : MonoBehaviour {

        private Conversation[] conversations;
        private Text optionText;

        private void Awake() {
            var convOpt = GameObject.FindWithTag(Tags.CONVERSATION)?.findChildWithTag(Tags.CONV_OPT);
            if (convOpt != null) {
                optionText = convOpt.GetComponentInChildren<Text>();
                optionText.font = convOpt.GetComponentInChildren<TextButton>().font;
            }
        }

        private void Start() {
            conversations = GetComponentsInChildren<Conversation>();
            
            conversations.ForEach(c => {
                c.initConversations();
                checkOptionsLength(c.getOptionsToCheckLengthFor());
            });
        }
        
        public virtual void checkOptionsLength(params ConversationOption[] options) {
            options.ForEach(o => {
                checkOptionsLength(o);    
            });
        }

        private void checkOptionsLength(ConversationOption option) {
            if (option.Value.SelfOption && option.Parent != null && option.Parent.getChildren().Count > 1) {
                var text = getOptionText(option);
                
                if (text != null && optionText != null) {
                    optionText.text = "> " + text;
                    optionText.SetLayoutDirty();
                    if (optionText.preferredWidth > 2475) {
                        Debug.LogWarning("Option text '" + text + "' width too big: " + optionText.preferredWidth + " in " + this.GetType().Name);
                    }
                }
            }

            option.getChildren().ForEach(c => {
                checkOptionsLength(c);
            });
        }
        
        private string getOptionText(ConversationOption option) {
            string text = option.Value.SayText;

            if (text == null) {
                return null;
            }
                
            string customOptKey = "+" + text;
            string customOpt = I18n.Instance.__(customOptKey);
            if (!customOpt.Equals(customOptKey)) {
                return customOpt;
            }

            text = I18n.Instance.__(text);
            return text.splitAndFirst(I18nUtil.LINE_DELIM);
        }        
    }
}