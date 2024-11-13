using System;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.say.conversation;
using net.gubbi.goofy.util;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class ConversationBehaviour : CharacterBehaviour {

        private ConversationService conversationService;

        protected override void Awake () {
            base.Awake ();
            this.conversationService = gameObject.getAncestorWithTag(Tags.CHARACTER).GetComponent<ConversationService>();
        }

        protected override void doBehaviour (Action onCompleteBehaviour) {
            conversationService.begin ();
            onCompleteBehaviour ();
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            return false;
        }
    }
}