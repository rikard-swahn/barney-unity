using System;

namespace net.gubbi.goofy.say.conversation {
	
    public class AncestorNConversationOption : ConversationOption {

        private int ancestorSteps;

        public AncestorNConversationOption (int ancestorSteps) : base(ConversationOptionValue.EMPTY) {
            this.ancestorSteps = ancestorSteps;
        }		

        public override bool isAvailable () {
            return tryResolve () != null && base.isAvailable();
        }

        public override bool isReference () {
            return true;
        }

        public override ConversationOption resolve () {
            ConversationOption ancestor = tryResolve ();

            if (ancestor == null) {
                throw new Exception ("Ancestor (" + ancestorSteps + ") not found!");
            }

            return ancestor;
        }	

        private ConversationOption tryResolve() {
            ConversationOption ancestor = this;

            for (int step = 0; step < ancestorSteps && ancestor != null; step++) {
                ancestor = ancestor.Parent;
            }

            return ancestor;

        }
			

    }

}