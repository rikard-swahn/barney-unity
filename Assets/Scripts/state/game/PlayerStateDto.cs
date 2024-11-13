namespace net.gubbi.goofy.state {
    public class PlayerStateDto {

        public Vector2Dto? position;
        public Vector2Dto? direction;
        public int mapPosition;
        public string sceneActionTargetName;
        public int? itemActionIndex;
        public int? itemActionGroupIndex;
        public Vector2Dto? sceneTarget;
        public Vector2Dto? lastWaypoint;
        public Vector2Dto? nextWaypoint;
        public bool disableCollision;

        public PlayerStateDto() { }

        public PlayerStateDto (PlayerState playerState) {                                    
            position = Vector2Dto.fromVector2(playerState.getPosition ());                        
            direction = Vector2Dto.fromVector2(playerState.getDirection ());            
            mapPosition = playerState.MapPosition;
            sceneActionTargetName = playerState.SceneActionTargetName;
            itemActionIndex = playerState.ItemActionIndex;
            itemActionGroupIndex = playerState.ItemActionGroupIndex;
            sceneTarget = Vector2Dto.fromVector2(playerState.SceneTarget);
            lastWaypoint = Vector2Dto.fromVector2(playerState.LastWaypoint);
            nextWaypoint = Vector2Dto.fromVector2(playerState.NextWaypoint);
            disableCollision = playerState.DisableCollision;
        }
        
    }
}