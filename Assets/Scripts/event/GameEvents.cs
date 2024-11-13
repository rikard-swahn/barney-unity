using net.gubbi.goofy.scene.load.transition;
using net.gubbi.goofy.ui.cursor;
using UnityEngine;

namespace net.gubbi.goofy.events {
    public static class GameEvents  {
        
        public interface GameEvent {}
        public interface SceneEvent : GameEvent{}

        public class GoalsSetEvent : SceneEvent {}
        public class PlayerControlEnabledEvent : SceneEvent {}
        public class PlayerControlDisabledEvent : SceneEvent {}
        public class PlayerRenderPositionEvent : GameEvent {
            public Vector2 Position { get; private set; }
            public PlayerRenderPositionEvent(Vector2 position) {
                Position = position;
            }
        }
        public class SceneChangeInCurrentGameEvent : SceneEvent {
            public string NextScene { get; private set;}
            public SceneChangeInCurrentGameEvent(string nextScene) {
                NextScene = nextScene;
            }
        }
        public class RepaintEvent : GameEvent {}
        public class SceneChangeInCurrentGameInitEvent : SceneEvent {
            public string NextScene { get; private set;}
            public SceneChangeInCurrentGameInitEvent(string nextScene) {
                NextScene = nextScene;
            }            
        }

        public class SceneChangeInitEvent : SceneEvent {
            public bool ImmediateChange { get; } 
            public SceneTransition OutTransition { get; } 
            
            
            public SceneChangeInitEvent(SceneTransition outTransition) {
                OutTransition = outTransition;
                ImmediateChange = outTransition == null;
            }              
        }        
        public class SceneTransitionStartedEvent : SceneEvent {}
        public class SceneTransitionCompletedEvent : SceneEvent {}

        public class SceneLoadedEvent : SceneEvent {
            public string Scene { get; private set;}
            public SceneLoadedEvent(string scene) {
                Scene = scene;
            }
        }

        public class MouseEvent : SceneEvent {
            public Vector3 MousePos { get; private set;}
            public Event Event { get; private set;}
            public int? MouseButton { get; private set;}
            public bool buttonOneUp() {
                return MouseButton == 0;
            }

            public MouseEvent (Vector3 mousePos, Event currentEvent) {
                MousePos = mousePos;
                Event = currentEvent;
                MouseButton = (Event.type == EventType.MouseUp) ? Event.button : (int?)null;
            }
        }

        public class KeyUpEvent : SceneEvent {
            public KeyCode KeyCode { get; }
            public bool Consumed { get; set; }

            public KeyUpEvent(KeyCode keyCode) {
                KeyCode = keyCode;
            }
        }
        
        public class LateKeyUpEvent : SceneEvent {
            public KeyCode KeyCode { get; }

            public LateKeyUpEvent(KeyCode keyCode) {
                KeyCode = keyCode;
            }
        }

        public class LateMouseEvent : MouseEvent {
            public LateMouseEvent (Vector3 mousePos, Event currentEvent) : base (mousePos, currentEvent) {				
            }				
        }
        
        public class ConversationStartEvent : SceneEvent {}
        public class ConversationEndEvent : SceneEvent {}
        
        public class UIActionStartEvent : SceneEvent {}
        public class UIActionEndEvent : SceneEvent {}

        public class CursorEvent : SceneEvent {
            public CursorType Type { get; private set;}
            
            public CursorEvent(CursorType type) {
                Type = type;
            }            
        }

        public class MusicEnabledEvent : SceneEvent {}
        public class MusicDisabledEvent : SceneEvent {}
        public class MusicFinishedEvent : SceneEvent {}
        public class SfxEnabledEvent : SceneEvent {}
        public class SfxDisabledEvent : SceneEvent {}
        public class PauseGameEvent : SceneEvent {}
        public class UnPauseGameEvent : SceneEvent {}
        
        public class UnityGamingServicesInitializedEvent : GameEvent {}        

        public class PurchaseEvent : GameEvent {
            public string productId { get; private set;}

            public PurchaseEvent(string productId) {
                this.productId = productId;
            }
        }
        
        public class GameTimeEvent : SceneEvent {
            public float deltaTime { get; private set;}

            public GameTimeEvent(float deltaTime) {
                this.deltaTime = deltaTime;
            }
        }

        public class CameraMovementEvent : SceneEvent {
            public Vector2 Movement { get; }
            public Vector2 AbsoluteMovement { get; }

            public CameraMovementEvent(Vector2 absoluteMovement, Vector2 movement) {
                this.AbsoluteMovement = absoluteMovement;
                this.Movement = movement;
            }
        }
        
        public class NavAgentUpdateEvent : SceneEvent {}

        public class LocaleChangedEvent : SceneEvent {
            public string Locale { get; }
            public LocaleChangedEvent(string locale) {
                Locale = locale;
            }
        }
        
    }
}