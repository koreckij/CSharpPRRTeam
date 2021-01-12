using System.IO;

namespace BitcoinExplorer.Core.Structs.Net
{
	public class GetAddr : EmptyPayload, IPayload
	{
		public static GetAddr FromStream (Stream s) {
			return new GetAddr();
		}
	}
}
