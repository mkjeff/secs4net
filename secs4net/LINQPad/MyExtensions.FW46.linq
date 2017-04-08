<Query Kind="Program">
  <Reference>E:\src\secs4net\secs4net\Secs4net.Json\bin\Debug\net46\Secs4Net.dll</Reference>
  <Reference>E:\src\secs4net\secs4net\Secs4net.Json\bin\Debug\net46\Secs4net.Json.dll</Reference>
  <Namespace>Secs4Net</Namespace>
</Query>

void Main()
{
	// Write code to test your extensions here. Press F5 to compile and run.
}

public static class MyExtensions
{
}

// You can also define non-static classes, enums, etc.
static object ToDump(object input)
{
	if (input is SecsMessage msg)
	{
		if (msg.SecsItem != null)
			return new { SxFy = $"S{msg.S} F{msg.F} {msg.Name}", W = msg.ReplyExpected, Item = Util.OnDemand("Item", () => msg.SecsItem), Raw = Util.OnDemand("Raw", () => msg.RawBytes.Select(r => r.ToArray())) };
		return new { SxFy = $"S{msg.S}F{msg.F} {msg.Name}", W = msg.ReplyExpected, Raw = Util.OnDemand("Raw", () => msg.RawBytes.Select(r => r.ToArray())) };
	}
	else if (input is SecsItem item)
	{
		if (item.Format == SecsFormat.List)
			return new
			{
				Format =  item.Format,
				Count = item.Count,
				Value = Util.OnDemand("Items", () => item.Items),
				//Text = item.ToString(),
				Raw = Util.OnDemand("Raw", () => item.RawBytes),
			};

		if (item.Format == SecsFormat.ASCII || item.Format == SecsFormat.JIS8)
			return new
			{
				Format =  item.Format,	
				Count = item.Count,
				Value = item.GetString(),
				//Text = item.ToString(),
				Raw = Util.OnDemand("Raw",() => item.RawBytes),
			};

		if (item.Format == SecsFormat.Binary)
			return new
			{
				Format = item.Format,
				Count = item.Count,
				Value = item.GetValues<double>(),
				//Text = item.ToString(),
				Raw = Util.OnDemand("Raw",() => item.RawBytes),
			};
			
		return new
		{
			Format =  item.Format,		
			Count = item.Count,
			Value = Util.OnDemand("Values", () => item.GetValues<double>()), 
			//Text = item.ToString(),
			Raw = Util.OnDemand("Raw",() => item.RawBytes),
		};
	}

	//	if (input is byte)
	//		return ((byte)input).ToString("X");

	return input;
}