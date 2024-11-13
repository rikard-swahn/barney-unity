using System;

namespace net.gubbi.goofy.say.conversation {
	
    public class AncestorIdConversationOption : ConversationOption {

        private string ancestorId;

        public AncestorIdConversationOption (string ancestorId, bool transient) : base(ConversationOptionValue.EMPTY) {
            this.ancestorId = ancestorId;
            this.Transient = transient;
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
                throw new Exception ("Ancestor id " + ancestorId + " not found!");
            }

            return ancestor;
        }	

        private ConversationOption tryResolve() {
            ConversationOption ancestor = this;

            while(ancestor != null && !ancestorId.Equals(ancestor.Value.Id)) {				
                ancestor = ancestor.Parent;
            }

            return ancestor;			
        }


    }

}