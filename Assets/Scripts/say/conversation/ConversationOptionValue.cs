using System;

namespace net.gubbi.goofy.say.conversation {
	
    public class ConversationOptionValue {

        private Func<ConversationOption, bool> isAvailableCallback;
        public Action BeforeSayCallback {get; set;}
        public Action<ConversationOption, Action> AfterSayCallback {get; private set;}
        public string SayText {get; set;}
        public object[] SayArgs {get; private set;}
        public bool SelfOption {get; private set;}
        public string Id {get; private set;}

        public static ConversationOptionValue EMPTY = new ConversationOptionValue();
        public bool IsDelayed {get; private set;}

        private ConversationOptionValue () {
            SelfOption = true;
        }

        public static Builder builder() {
            return new Builder ();
        }
			
        public virtual bool isAvailable (ConversationOption option) {
            return isAvailableCallback == null || isAvailableCallback (option);
        }

        public class Builder {
            private ConversationOptionValue val;

            public Builder() {
                val = new ConversationOptionValue();
            }

            public Builder availableIf(Func<ConversationOption, bool> isAvailableCallback) {
                if (val.isAvailableCallback != null) {
                    Func<ConversationOption, bool> curCallback = val.isAvailableCallback;

                    val.isAvailableCallback = option => {
                        return curCallback(option) && isAvailableCallback(option);
                    };
                }
                else {
                    val.isAvailableCallback = isAvailableCallback;
                }

                return this;
            }

            public Builder id (string id) {
                val.Id = id;
                return this;
            }

            public Builder playerOption(string sayText) {
                val.SayText = sayText;
                return this;
            }

            public Builder beforeSay (Action beforeSay) {

                if (val.BeforeSayCallback != null) {
                    Action curCallback = val.BeforeSayCallback;

                    val.BeforeSayCallback = () => {
                        curCallback();
                        beforeSay();
                    };
                }
                else {
                    val.BeforeSayCallback = beforeSay;
                }


                return this;
            }

            public Builder afterSayDelayed(Action<ConversationOption, Action> afterSay) {
                if (val.AfterSayCallback != null) {
                    Action<ConversationOption, Action> curCallback = val.AfterSayCallback;

                    val.AfterSayCallback = (opt,  afterCallback) => {
                        curCallback(opt, null);
                        afterSay(opt, afterCallback);
                    };
                }
                else {
                    val.AfterSayCallback = afterSay;
                }

                return this;                
            }

            public Builder afterSay (Action<ConversationOption> afterSay) {

                if (val.AfterSayCallback != null) {
                    Action<ConversationOption, Action> curCallback = val.AfterSayCallback;

                    val.AfterSayCallback = (opt,  afterCallback) => {
                        curCallback(opt, null);
                        afterSay(opt);
                        afterCallback();
                    };
                }
                else {
                    val.AfterSayCallback = (opt, afterCallback) => {
                        afterSay(opt);
                        afterCallback();
                    };
                }

                return this;
            }


            public Builder delayed() {
                val.IsDelayed = true;
                return this;
            }

            public Builder characterOption(string sayText, params object[] args) {
                val.SelfOption = false;
                val.SayText = sayText;
                val.SayArgs = args;
                return this;
            }
				
            public ConversationOptionValue build() {
                return val;
            }

        }
    }
}