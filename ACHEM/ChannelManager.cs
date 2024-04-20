using System.Collections.Generic;
using System.Linq;

public class ChannelManager
{
	private Dictionary<int, Channel> channellist;

	private static ChannelManager mInstance;

	public static ChannelManager Instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = new ChannelManager();
			}
			return mInstance;
		}
	}

	private ChannelManager()
	{
		channellist = new Dictionary<int, Channel>();
	}

	public void CreateChannel(int id, int type)
	{
		channellist[type] = new Channel(id, type);
	}

	public void DestroyChannel(int id)
	{
		Channel channel = channellist.Values.Where((Channel p) => p.id == id).FirstOrDefault();
		if (channel != null)
		{
			channellist.Remove(channel.type);
		}
	}

	public Channel GetChannelByType(int type)
	{
		if (channellist.ContainsKey(type))
		{
			return channellist[type];
		}
		return null;
	}

	public void Clear()
	{
		channellist.Clear();
	}
}
