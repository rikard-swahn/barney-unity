using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using net.gubbi.goofy.extensions;

namespace net.gubbi.goofy.say.conversation {
    public class ConversationOption {

        public ConversationOption Parent { get; private set; }
        public ConversationOptionValue Value {get; private set;}
        public bool Transient {get; protected set;}
        private readonly List<ConversationOption> children = new List<ConversationOption>();

        private static readonly char POS_ID_SEPARATOR = '.';
        private bool used;
        private Action onResult;
        private bool delayedResultAvailable;
        private Func<ConversationOption[]> childrenCallback;

        protected ConversationOption(ConversationOptionValue value) {
            this.Value = value;
        }

        public static Builder value(ConversationOptionValue value) {
            return new Builder(value);
        }			

        public ReadOnlyCollection<ConversationOption> getChildren() {
            return children.AsReadOnly();
        }

        public virtual bool isAvailable() {
            return Value.isAvailable(this);
        }

        public virtual bool isReference () {
            return false;
        }
        
        public bool isDelayed() {
            return Value.IsDelayed;
        }

        public void setDelayedResultAvailable() {
            delayedResultAvailable = true;
            handleDelayedResult();            
        }

        public virtual ConversationOption resolve () {
            throw new NotSupportedException ("This is not a reference option, do not call resolve on this");
        }

        /// <summary>
        /// Gets the position of this option, as a string of option indexes, relative to its ancestors.
        /// </summary>
        public string getPositionId () {
            if (Parent == null) {
                return "";
            }

            string parentId = Parent.getPositionId ();
            string thisId = Parent.getChildren ().IndexOf (this).ToString();

            return parentId.Equals ("") ? thisId : parentId + "." + thisId;
        }

        public void setUsed() {
            this.used = true;
        }

        public bool isUsed() {
            return this.used;
        }

        [CanBeNull]
        public ConversationOption gotoPosition (string positionId) {
            string[] parts = positionId.Split (POS_ID_SEPARATOR);
            List<int> childIndexes = parts.Select (partStr => Int32.Parse (partStr)).ToList ();
            return gotoPositionHelper (childIndexes);
        }

        [CanBeNull]
        private ConversationOption gotoPositionHelper (List<int> childIndexes) {
            if (childIndexes.Count > 0) {

                ReadOnlyCollection<ConversationOption> children = getChildren();

                //The persisted state refers to an index that does not exist, return a null option to indicate this
                if (children.Count <= childIndexes.First()) {
                    return null;
                }
                
                return children.ElementAt (childIndexes.First())
                    .gotoPositionHelper (
                        childIndexes.GetRange(1, childIndexes.Count - 1)
                    );
            }

            return this;
        }

        public void refreshChildren() {
            if (childrenCallback != null) {
                children.Clear();
                ConversationOption[] opts = childrenCallback();
                opts.ForEach(o => {                    
                    o.Parent = this;
                    children.Add(o);
                });
            }
        }

        public void onDelayedResult(Action onResult) {
            this.onResult = onResult;
            handleDelayedResult();
        }


        private void handleDelayedResult() {
            if (onResult != null && delayedResultAvailable) {
                onResult();
            }
        }

        public class Builder {
            private ConversationOption option;            

            public Builder(ConversationOptionValue value) {
                option = new ConversationOption(value);
            }

            public ConversationOption options(params ConversationOption[] childOptions) {
                foreach (ConversationOption childOption in childOptions) {
                    childOption.Parent = option;
                    option.children.Add(childOption);
                }

                return option;
            }

            public Builder optionsCallback(Func<ConversationOption[]> childrenCallback) {
                option.childrenCallback = childrenCallback;
                return this;
            }

            public Builder transient() {
                option.Transient = true;
                return this;
            }

            public ConversationOption build () {
                return option;
            }
        }
    }

}