using System;

namespace Steamworks
{
	// Token: 0x02000097 RID: 151
	public static class SteamMusicRemote
	{
		// Token: 0x0600087C RID: 2172 RVA: 0x000273D8 File Offset: 0x000255D8
		public static bool RegisterSteamMusicRemote(string pchName)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamMusicRemote_RegisterSteamMusicRemote(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x00027418 File Offset: 0x00025618
		public static bool DeregisterSteamMusicRemote()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_DeregisterSteamMusicRemote();
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00027424 File Offset: 0x00025624
		public static bool BIsCurrentMusicRemote()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_BIsCurrentMusicRemote();
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00027430 File Offset: 0x00025630
		public static bool BActivationSuccess(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_BActivationSuccess(bValue);
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00027440 File Offset: 0x00025640
		public static bool SetDisplayName(string pchDisplayName)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDisplayName))
			{
				result = NativeMethods.ISteamMusicRemote_SetDisplayName(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00027480 File Offset: 0x00025680
		public static bool SetPNGIcon_64x64(byte[] pvBuffer, uint cbBufferLength)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_SetPNGIcon_64x64(pvBuffer, cbBufferLength);
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0002748E File Offset: 0x0002568E
		public static bool EnablePlayPrevious(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnablePlayPrevious(bValue);
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0002749B File Offset: 0x0002569B
		public static bool EnablePlayNext(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnablePlayNext(bValue);
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x000274A8 File Offset: 0x000256A8
		public static bool EnableShuffled(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnableShuffled(bValue);
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x000274B5 File Offset: 0x000256B5
		public static bool EnableLooped(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnableLooped(bValue);
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x000274C2 File Offset: 0x000256C2
		public static bool EnableQueue(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnableQueue(bValue);
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x000274CF File Offset: 0x000256CF
		public static bool EnablePlaylists(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnablePlaylists(bValue);
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x000274DC File Offset: 0x000256DC
		public static bool UpdatePlaybackStatus(AudioPlayback_Status nStatus)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdatePlaybackStatus(nStatus);
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x000274E9 File Offset: 0x000256E9
		public static bool UpdateShuffled(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateShuffled(bValue);
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x000274F6 File Offset: 0x000256F6
		public static bool UpdateLooped(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateLooped(bValue);
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x00027503 File Offset: 0x00025703
		public static bool UpdateVolume(float flValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateVolume(flValue);
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00027510 File Offset: 0x00025710
		public static bool CurrentEntryWillChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_CurrentEntryWillChange();
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0002751C File Offset: 0x0002571C
		public static bool CurrentEntryIsAvailable(bool bAvailable)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_CurrentEntryIsAvailable(bAvailable);
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0002752C File Offset: 0x0002572C
		public static bool UpdateCurrentEntryText(string pchText)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchText))
			{
				result = NativeMethods.ISteamMusicRemote_UpdateCurrentEntryText(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0002756C File Offset: 0x0002576C
		public static bool UpdateCurrentEntryElapsedSeconds(int nValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateCurrentEntryElapsedSeconds(nValue);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00027579 File Offset: 0x00025779
		public static bool UpdateCurrentEntryCoverArt(byte[] pvBuffer, uint cbBufferLength)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateCurrentEntryCoverArt(pvBuffer, cbBufferLength);
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x00027587 File Offset: 0x00025787
		public static bool CurrentEntryDidChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_CurrentEntryDidChange();
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x00027593 File Offset: 0x00025793
		public static bool QueueWillChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_QueueWillChange();
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0002759F File Offset: 0x0002579F
		public static bool ResetQueueEntries()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_ResetQueueEntries();
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x000275AC File Offset: 0x000257AC
		public static bool SetQueueEntry(int nID, int nPosition, string pchEntryText)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchEntryText))
			{
				result = NativeMethods.ISteamMusicRemote_SetQueueEntry(nID, nPosition, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x000275EC File Offset: 0x000257EC
		public static bool SetCurrentQueueEntry(int nID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_SetCurrentQueueEntry(nID);
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x000275F9 File Offset: 0x000257F9
		public static bool QueueDidChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_QueueDidChange();
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00027605 File Offset: 0x00025805
		public static bool PlaylistWillChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_PlaylistWillChange();
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x00027611 File Offset: 0x00025811
		public static bool ResetPlaylistEntries()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_ResetPlaylistEntries();
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x00027620 File Offset: 0x00025820
		public static bool SetPlaylistEntry(int nID, int nPosition, string pchEntryText)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchEntryText))
			{
				result = NativeMethods.ISteamMusicRemote_SetPlaylistEntry(nID, nPosition, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x00027660 File Offset: 0x00025860
		public static bool SetCurrentPlaylistEntry(int nID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_SetCurrentPlaylistEntry(nID);
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0002766D File Offset: 0x0002586D
		public static bool PlaylistDidChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_PlaylistDidChange();
		}
	}
}
