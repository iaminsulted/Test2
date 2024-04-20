using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
	// Token: 0x020001BF RID: 447
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4, Size = 372)]
	public class gameserveritem_t
	{
		// Token: 0x06000D70 RID: 3440 RVA: 0x0002A974 File Offset: 0x00028B74
		public string GetGameDir()
		{
			return Encoding.UTF8.GetString(this.m_szGameDir, 0, Array.IndexOf<byte>(this.m_szGameDir, 0));
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x0002A993 File Offset: 0x00028B93
		public void SetGameDir(string dir)
		{
			this.m_szGameDir = Encoding.UTF8.GetBytes(dir + "\0");
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x0002A9B0 File Offset: 0x00028BB0
		public string GetMap()
		{
			return Encoding.UTF8.GetString(this.m_szMap, 0, Array.IndexOf<byte>(this.m_szMap, 0));
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x0002A9CF File Offset: 0x00028BCF
		public void SetMap(string map)
		{
			this.m_szMap = Encoding.UTF8.GetBytes(map + "\0");
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x0002A9EC File Offset: 0x00028BEC
		public string GetGameDescription()
		{
			return Encoding.UTF8.GetString(this.m_szGameDescription, 0, Array.IndexOf<byte>(this.m_szGameDescription, 0));
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x0002AA0B File Offset: 0x00028C0B
		public void SetGameDescription(string desc)
		{
			this.m_szGameDescription = Encoding.UTF8.GetBytes(desc + "\0");
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x0002AA28 File Offset: 0x00028C28
		public string GetServerName()
		{
			if (this.m_szServerName[0] == 0)
			{
				return this.m_NetAdr.GetConnectionAddressString();
			}
			return Encoding.UTF8.GetString(this.m_szServerName, 0, Array.IndexOf<byte>(this.m_szServerName, 0));
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0002AA5D File Offset: 0x00028C5D
		public void SetServerName(string name)
		{
			this.m_szServerName = Encoding.UTF8.GetBytes(name + "\0");
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x0002AA7A File Offset: 0x00028C7A
		public string GetGameTags()
		{
			return Encoding.UTF8.GetString(this.m_szGameTags, 0, Array.IndexOf<byte>(this.m_szGameTags, 0));
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x0002AA99 File Offset: 0x00028C99
		public void SetGameTags(string tags)
		{
			this.m_szGameTags = Encoding.UTF8.GetBytes(tags + "\0");
		}

		// Token: 0x04000A5E RID: 2654
		public servernetadr_t m_NetAdr;

		// Token: 0x04000A5F RID: 2655
		public int m_nPing;

		// Token: 0x04000A60 RID: 2656
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bHadSuccessfulResponse;

		// Token: 0x04000A61 RID: 2657
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bDoNotRefresh;

		// Token: 0x04000A62 RID: 2658
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		private byte[] m_szGameDir;

		// Token: 0x04000A63 RID: 2659
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		private byte[] m_szMap;

		// Token: 0x04000A64 RID: 2660
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] m_szGameDescription;

		// Token: 0x04000A65 RID: 2661
		public uint m_nAppID;

		// Token: 0x04000A66 RID: 2662
		public int m_nPlayers;

		// Token: 0x04000A67 RID: 2663
		public int m_nMaxPlayers;

		// Token: 0x04000A68 RID: 2664
		public int m_nBotPlayers;

		// Token: 0x04000A69 RID: 2665
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bPassword;

		// Token: 0x04000A6A RID: 2666
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bSecure;

		// Token: 0x04000A6B RID: 2667
		public uint m_ulTimeLastPlayed;

		// Token: 0x04000A6C RID: 2668
		public int m_nServerVersion;

		// Token: 0x04000A6D RID: 2669
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] m_szServerName;

		// Token: 0x04000A6E RID: 2670
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		private byte[] m_szGameTags;

		// Token: 0x04000A6F RID: 2671
		public CSteamID m_steamID;
	}
}
