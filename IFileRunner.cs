
namespace TextPad_
{
    internal interface IFileRunner
    {
        void RunScript();

        void PythonRun();

        void PythonRun(string path);

        void BatRun();

        void VbsRun();
    }
}
