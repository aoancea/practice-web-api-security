using Microsoft.Owin.Logging;

namespace Phobos.Web.Infrastructure.Logger
{
	public class LoggerFactory : ILoggerFactory
	{
		public ILogger Create(string name)
		{
			return new Logger();
		}
	}
}