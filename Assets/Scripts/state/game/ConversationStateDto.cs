namespace net.gubbi.goofy.state {

    public class ConversationStateDto {
        public string characterKey;
        public string positionId;
        public bool atChildren;

        public ConversationStateDto() { }

        public ConversationStateDto (ConversationState state) {
            this.characterKey = state.CharacterKey;
            this.positionId = state.PositionId;
            this.atChildren = state.AtChildren;
        }
		
    }
}