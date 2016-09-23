using Microsoft.Owin.Logging;
using System;
using System.Diagnostics;
using System.Text;

namespace Phobos.Api.Infrastructure.Logger
{
	public class Logger : ILogger
	{
		public bool WriteCore(TraceEventType eventType, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
		{
			using (EventLog eventLog = new EventLog("Application"))
			{
				eventLog.Source = "Phobos";
				eventLog.WriteEntry(ExceptionToString(exception, (string)state), EventLogEntryType.Error);
			}

			return true;
		}

		private string ExceptionToString(Exception exception, string message)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(string.Format("Message: {0}", message));

			string label = "==EXCEPTION==============================================================";

			while (exception != null)
			{
				sb.AppendLine(label);
				sb.AppendLine("[Exception type] " + exception.GetType().FullName);
				sb.AppendLine("[Message] " + exception.Message);
				sb.AppendLine("[Source] " + exception.Source);
				sb.AppendLine("[Stack trace] " + exception.StackTrace);
				sb.AppendLine("[Target site] " + exception.TargetSite);

				exception = exception.InnerException;
				label = "===============INNER EXCEPTION==============================================";
			}

			return sb.ToString();
		}
	}
}