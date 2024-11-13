namespace ui.text {
    public interface LineBreakStrategy {
        string breakString(string str, int maxLineWidth);
    }
}