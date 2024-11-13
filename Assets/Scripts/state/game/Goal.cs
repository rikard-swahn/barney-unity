namespace net.gubbi.goofy.state {

    public class Goal {
        public string text;
        public bool complete;

        public Goal() { }

        public Goal(string text) {
            this.text = text;
            complete = false;
        }
    }
}