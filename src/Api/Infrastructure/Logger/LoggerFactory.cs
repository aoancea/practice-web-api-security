using Microsoft.Owin.Logging;

namespace Phobos.Api.Infrastructure.Logger
{
	public class LoggerFactory : ILoggerFactory
	{
		public ILogger Create(string name)
		{
			return new Logger();
		}
	}
}