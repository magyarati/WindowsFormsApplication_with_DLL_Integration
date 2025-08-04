namespace WindowsFormsApplication_with_DLL_Integration
{
    public interface ILogView
    {
        bool ShowTimestamps { get; }
        bool ShowLineNumbers { get; }
        bool SaveSilentSegments { get; }

        void AppendText(string text);
        string GetText();
        void SetText(string text);
    }
}