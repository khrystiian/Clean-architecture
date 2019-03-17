using System.Runtime.CompilerServices;

namespace TrainApp.Core.ApplicationService
{
    public class LogHelper
    {
        public static log4net.ILog GetLogger([CallerFilePath]string filename = "") => log4net.LogManager.GetLogger(filename);
    }
}