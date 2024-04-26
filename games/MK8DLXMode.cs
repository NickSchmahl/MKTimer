namespace MKTimer {
    public enum MK8DLXMode {
        _150CC,
        _200CC
    }
    public static class ModeStrings {
    public static string getMode(this MK8DLXMode mode) {
        if (mode == MK8DLXMode._150CC) {
            return "150cc";
        } else {
            return "200cc";
        }
    }}
}