using System.Windows;
using System.Windows.Input;

namespace ChessGame.Utils
{
    public static class ChessCursors
    {
        public static readonly Cursor WhiteCursor = LoadCursor("Assets/CursorW.cur");
        public static readonly Cursor BlackCursor = LoadCursor("Assets/CursorB.cur");

        private static Cursor LoadCursor(string filePath)
        {
            var uri = new Uri($"pack://application:,,,/{filePath}", UriKind.Absolute);
            var stream = Application.GetResourceStream(uri).Stream;
            return new Cursor(stream, true);
        }
    }
}
