using System;

namespace Steamworks
{
	// Token: 0x020001C5 RID: 453
	[Serializable]
	public struct CGameID : IEquatable<CGameID>, IComparable<CGameID>
	{
		// Token: 0x06000DA9 RID: 3497 RVA: 0x0002AE01 File Offset: 0x00029001
		public CGameID(ulong GameID)
		{
			this.m_GameID = GameID;
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x0002AE0A File Offset: 0x0002900A
		public CGameID(AppId_t nAppID)
		{
			this.m_GameID = 0UL;
			this.SetAppID(nAppID);
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x0002AE1B File Offset: 0x0002901B
		public CGameID(AppId_t nAppID, uint nModID)
		{
			this.m_GameID = 0UL;
			this.SetAppID(nAppID);
			this.SetType(CGameID.EGameIDType.k_EGameIDTypeGameMod);
			this.SetModID(nModID);
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x0002AE3A File Offset: 0x0002903A
		public bool IsSteamApp()
		{
			return this.Type() == CGameID.EGameIDType.k_EGameIDTypeApp;
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x0002AE45 File Offset: 0x00029045
		public bool IsMod()
		{
			return this.Type() == CGameID.EGameIDType.k_EGameIDTypeGameMod;
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x0002AE50 File Offset: 0x00029050
		public bool IsShortcut()
		{
			return this.Type() == CGameID.EGameIDType.k_EGameIDTypeShortcut;
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x0002AE5B File Offset: 0x0002905B
		public bool IsP2PFile()
		{
			return this.Type() == CGameID.EGameIDType.k_EGameIDTypeP2P;
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0002AE66 File Offset: 0x00029066
		public AppId_t AppID()
		{
			return new AppId_t((uint)(this.m_GameID & 16777215UL));
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0002AE7B File Offset: 0x0002907B
		public CGameID.EGameIDType Type()
		{
			return (CGameID.EGameIDType)(this.m_GameID >> 24 & 255UL);
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0002AE8E File Offset: 0x0002908E
		public uint ModID()
		{
			return (uint)(this.m_GameID >> 32 & (ulong)-1);
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0002AEA0 File Offset: 0x000290A0
		public bool IsValid()
		{
			switch (this.Type())
			{
			case CGameID.EGameIDType.k_EGameIDTypeApp:
				return this.AppID() != AppId_t.Invalid;
			case CGameID.EGameIDType.k_EGameIDTypeGameMod:
				return this.AppID() != AppId_t.Invalid && (this.ModID() & 2147483648U) > 0U;
			case CGameID.EGameIDType.k_EGameIDTypeShortcut:
				return (this.ModID() & 2147483648U) > 0U;
			case CGameID.EGameIDType.k_EGameIDTypeP2P:
				return this.AppID() == AppId_t.Invalid && (this.ModID() & 2147483648U) > 0U;
			default:
				return false;
			}
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x0002AF36 File Offset: 0x00029136
		public void Reset()
		{
			this.m_GameID = 0UL;
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x0002AF40 File Offset: 0x00029140
		public void Set(ulong GameID)
		{
			this.m_GameID = GameID;
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x0002AF49 File Offset: 0x00029149
		private void SetAppID(AppId_t other)
		{
			this.m_GameID = ((this.m_GameID & 18446744073692774400UL) | ((ulong)((uint)other) & 16777215UL));
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x0002AF6D File Offset: 0x0002916D
		private void SetType(CGameID.EGameIDType other)
		{
			this.m_GameID = ((this.m_GameID & 18446744069431361535UL) | (ulong)((ulong)((long)other & 255L) << 24));
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x0002AF92 File Offset: 0x00029192
		private void SetModID(uint other)
		{
			this.m_GameID = ((this.m_GameID & (ulong)-1) | ((ulong)other & (ulong)-1) << 32);
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x0002AFAC File Offset: 0x000291AC
		public override string ToString()
		{
			return this.m_GameID.ToString();
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x0002AFB9 File Offset: 0x000291B9
		public override bool Equals(object other)
		{
			return other is CGameID && this == (CGameID)other;
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x0002AFD6 File Offset: 0x000291D6
		public override int GetHashCode()
		{
			return this.m_GameID.GetHashCode();
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x0002AFE3 File Offset: 0x000291E3
		public static bool operator ==(CGameID x, CGameID y)
		{
			return x.m_GameID == y.m_GameID;
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x0002AFF3 File Offset: 0x000291F3
		public static bool operator !=(CGameID x, CGameID y)
		{
			return !(x == y);
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x0002AFFF File Offset: 0x000291FF
		public static explicit operator CGameID(ulong value)
		{
			return new CGameID(value);
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x0002B007 File Offset: 0x00029207
		public static explicit operator ulong(CGameID that)
		{
			return that.m_GameID;
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x0002B00F File Offset: 0x0002920F
		public bool Equals(CGameID other)
		{
			return this.m_GameID == other.m_GameID;
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0002B01F File Offset: 0x0002921F
		public int CompareTo(CGameID other)
		{
			return this.m_GameID.CompareTo(other.m_GameID);
		}

		// Token: 0x04000A75 RID: 2677
		public ulong m_GameID;

		// Token: 0x020002DA RID: 730
		public enum EGameIDType
		{
			// Token: 0x04000DD4 RID: 3540
			k_EGameIDTypeApp,
			// Token: 0x04000DD5 RID: 3541
			k_EGameIDTypeGameMod,
			// Token: 0x04000DD6 RID: 3542
			k_EGameIDTypeShortcut,
			// Token: 0x04000DD7 RID: 3543
			k_EGameIDTypeP2P
		}
	}
}
