using System.IO;

namespace BitcoinExplorer.Core.Structs.Net
{
	public class Ping : EmptyPayload, IPayload
	{
		public static Ping FromStream(Stream s)
		{
			return new Ping();
		}
	}
}