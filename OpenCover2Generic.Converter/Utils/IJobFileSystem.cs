namespace OpenCover2Generic.Converter
{
    public interface IJobFileSystem
    {
        string GetOpenCoverLogPath(string assembly);
        string GetOpenCoverOutputPath(string assembly);
        string GetTestResultsPath(string assembly);
        string GetIntermediateCoverageDirectory();
        string GetTestResultsDirectory();
        void CreateRoot(string v);
    }
}