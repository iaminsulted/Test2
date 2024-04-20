using System;

namespace Steamworks
{
	// Token: 0x02000096 RID: 150
	public static class SteamMusic
	{
		// Token: 0x06000873 RID: 2163 RVA: 0x0002736A File Offset: 0x0002556A
		public static bool BIsEnabled()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusic_BIsEnabled();
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x00027376 File Offset: 0x00025576
		public static bool BIsPlaying()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusic_BIsPlaying();
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x00027382 File Offset: 0x00025582
		public static AudioPlayback_Status GetPlaybackStatus()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusic_GetPlaybackStatus();
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x0002738E File Offset: 0x0002558E
		public static void Play()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_Play();
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x0002739A File Offset: 0x0002559A
		public static void Pause()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_Pause();
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x000273A6 File Offset: 0x000255A6
		public static void PlayPrevious()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_PlayPrevious();
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x000273B2 File Offset: 0x000255B2
		public static void PlayNext()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_PlayNext();
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x000273BE File Offset: 0x000255BE
		public static void SetVolume(float flVolume)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_SetVolume(flVolume);
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x000273CB File Offset: 0x000255CB
		public static float GetVolume()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusic_GetVolume();
		}
	}
}
