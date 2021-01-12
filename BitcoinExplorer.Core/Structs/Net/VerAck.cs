using System.IO;

namespace BitcoinExplorer.Core.Structs.Net
{
	public class VerAck : EmptyPayload, IPayload
	{
		public static GetAddr FromStream(Stream s)
		{
			return new GetAddr();
		}
	}
}
