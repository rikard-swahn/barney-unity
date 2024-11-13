namespace net.gubbi.goofy.state {
    public class ConversationState {
        public string CharacterKey { get; private set; }
        public string PositionId { get; set;}
        public bool AtChildren { get; set;}

        public ConversationState (string characterKey) {
            this.CharacterKey = characterKey;
        }		

        public ConversationState (ConversationStateDto dto) {
            this.CharacterKey = dto.characterKey;
            this.PositionId = dto.positionId;
            this.AtChildren = dto.atChildren;
        }		

    }
}