using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001C6 RID: 454
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CSteamID : IEquatable<CSteamID>, IComparable<CSteamID>
	{
		// Token: 0x06000DC2 RID: 3522 RVA: 0x0002B032 File Offset: 0x00029232
		public CSteamID(AccountID_t unAccountID, EUniverse eUniverse, EAccountType eAccountType)
		{
			this.m_SteamID = 0UL;
			this.Set(unAccountID, eUniverse, eAccountType);
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0002B045 File Offset: 0x00029245
		public CSteamID(AccountID_t unAccountID, uint unAccountInstance, EUniverse eUniverse, EAccountType eAccountType)
		{
			this.m_SteamID = 0UL;
			this.InstancedSet(unAccountID, unAccountInstance, eUniverse, eAccountType);
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0002B05A File Offset: 0x0002925A
		public CSteamID(ulong ulSteamID)
		{
			this.m_SteamID = ulSteamID;
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x0002B063 File Offset: 0x00029263
		public void Set(AccountID_t unAccountID, EUniverse eUniverse, EAccountType eAccountType)
		{
			this.SetAccountID(unAccountID);
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(eAccountType);
			if (eAccountType == EAccountType.k_EAccountTypeClan || eAccountType == EAccountType.k_EAccountTypeGameServer)
			{
				this.SetAccountInstance(0U);
				return;
			}
			this.SetAccountInstance(1U);
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x0002B091 File Offset: 0x00029291
		public void InstancedSet(AccountID_t unAccountID, uint unInstance, EUniverse eUniverse, EAccountType eAccountType)
		{
			this.SetAccountID(unAccountID);
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(eAccountType);
			this.SetAccountInstance(unInstance);
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0002B0B0 File Offset: 0x000292B0
		public void Clear()
		{
			this.m_SteamID = 0UL;
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x0002B0BA File Offset: 0x000292BA
		public void CreateBlankAnonLogon(EUniverse eUniverse)
		{
			this.SetAccountID(new AccountID_t(0U));
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(EAccountType.k_EAccountTypeAnonGameServer);
			this.SetAccountInstance(0U);
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0002B0DD File Offset: 0x000292DD
		public void CreateBlankAnonUserLogon(EUniverse eUniverse)
		{
			this.SetAccountID(new AccountID_t(0U));
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(EAccountType.k_EAccountTypeAnonUser);
			this.SetAccountInstance(0U);
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x0002B101 File Offset: 0x00029301
		public bool BBlankAnonAccount()
		{
			return this.GetAccountID() == new AccountID_t(0U) && this.BAnonAccount() && this.GetUnAccountInstance() == 0U;
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x0002B129 File Offset: 0x00029329
		public bool BGameServerAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeGameServer || this.GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer;
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0002B13F File Offset: 0x0002933F
		public bool BPersistentGameServerAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeGameServer;
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0002B14A File Offset: 0x0002934A
		public bool BAnonGameServerAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer;
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x0002B155 File Offset: 0x00029355
		public bool BContentServerAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeContentServer;
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x0002B160 File Offset: 0x00029360
		public bool BClanAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeClan;
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x0002B16B File Offset: 0x0002936B
		public bool BChatAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeChat;
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x0002B176 File Offset: 0x00029376
		public bool IsLobby()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeChat && (this.GetUnAccountInstance() & 262144U) > 0U;
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x0002B192 File Offset: 0x00029392
		public bool BIndividualAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeIndividual || this.GetEAccountType() == EAccountType.k_EAccountTypeConsoleUser;
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0002B1A9 File Offset: 0x000293A9
		public bool BAnonAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeAnonUser || this.GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer;
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x0002B1C0 File Offset: 0x000293C0
		public bool BAnonUserAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeAnonUser;
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0002B1CC File Offset: 0x000293CC
		public bool BConsoleUserAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeConsoleUser;
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x0002B1D8 File Offset: 0x000293D8
		public void SetAccountID(AccountID_t other)
		{
			this.m_SteamID = ((this.m_SteamID & 18446744069414584320UL) | ((ulong)((uint)other) & (ulong)-1));
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x0002B1FB File Offset: 0x000293FB
		public void SetAccountInstance(uint other)
		{
			this.m_SteamID = ((this.m_SteamID & 18442240478377148415UL) | ((ulong)other & 1048575UL) << 32);
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x0002B220 File Offset: 0x00029420
		public void SetEAccountType(EAccountType other)
		{
			this.m_SteamID = ((this.m_SteamID & 18379190079298994175UL) | (ulong)((ulong)((long)other & 15L) << 52));
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x0002B242 File Offset: 0x00029442
		public void SetEUniverse(EUniverse other)
		{
			this.m_SteamID = ((this.m_SteamID & 72057594037927935UL) | (ulong)((ulong)((long)other & 255L) << 56));
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x0002B267 File Offset: 0x00029467
		public void ClearIndividualInstance()
		{
			if (this.BIndividualAccount())
			{
				this.SetAccountInstance(0U);
			}
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x0002B278 File Offset: 0x00029478
		public bool HasNoIndividualInstance()
		{
			return this.BIndividualAccount() && this.GetUnAccountInstance() == 0U;
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x0002B28D File Offset: 0x0002948D
		public AccountID_t GetAccountID()
		{
			return new AccountID_t((uint)(this.m_SteamID & (ulong)-1));
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0002B29E File Offset: 0x0002949E
		public uint GetUnAccountInstance()
		{
			return (uint)(this.m_SteamID >> 32 & 1048575UL);
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0002B2B1 File Offset: 0x000294B1
		public EAccountType GetEAccountType()
		{
			return (EAccountType)(this.m_SteamID >> 52 & 15UL);
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x0002B2C1 File Offset: 0x000294C1
		public EUniverse GetEUniverse()
		{
			return (EUniverse)(this.m_SteamID >> 56 & 255UL);
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x0002B2D4 File Offset: 0x000294D4
		public bool IsValid()
		{
			return this.GetEAccountType() > EAccountType.k_EAccountTypeInvalid && this.GetEAccountType() < EAccountType.k_EAccountTypeMax && this.GetEUniverse() > EUniverse.k_EUniverseInvalid && this.GetEUniverse() < EUniverse.k_EUniverseMax && (this.GetEAccountType() != EAccountType.k_EAccountTypeIndividual || (!(this.GetAccountID() == new AccountID_t(0U)) && this.GetUnAccountInstance() <= 4U)) && (this.GetEAccountType() != EAccountType.k_EAccountTypeClan || (!(this.GetAccountID() == new AccountID_t(0U)) && this.GetUnAccountInstance() == 0U)) && (this.GetEAccountType() != EAccountType.k_EAccountTypeGameServer || !(this.GetAccountID() == new AccountID_t(0U)));
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0002B376 File Offset: 0x00029576
		public override string ToString()
		{
			return this.m_SteamID.ToString();
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x0002B383 File Offset: 0x00029583
		public override bool Equals(object other)
		{
			return other is CSteamID && this == (CSteamID)other;
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x0002B3A0 File Offset: 0x000295A0
		public override int GetHashCode()
		{
			return this.m_SteamID.GetHashCode();
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0002B3AD File Offset: 0x000295AD
		public static bool operator ==(CSteamID x, CSteamID y)
		{
			return x.m_SteamID == y.m_SteamID;
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0002B3BD File Offset: 0x000295BD
		public static bool operator !=(CSteamID x, CSteamID y)
		{
			return !(x == y);
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0002B3C9 File Offset: 0x000295C9
		public static explicit operator CSteamID(ulong value)
		{
			return new CSteamID(value);
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x0002B3D1 File Offset: 0x000295D1
		public static explicit operator ulong(CSteamID that)
		{
			return that.m_SteamID;
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x0002B3D9 File Offset: 0x000295D9
		public bool Equals(CSteamID other)
		{
			return this.m_SteamID == other.m_SteamID;
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x0002B3E9 File Offset: 0x000295E9
		public int CompareTo(CSteamID other)
		{
			return this.m_SteamID.CompareTo(other.m_SteamID);
		}

		// Token: 0x04000A76 RID: 2678
		public static readonly CSteamID Nil = default(CSteamID);

		// Token: 0x04000A77 RID: 2679
		public static readonly CSteamID OutofDateGS = new CSteamID(new AccountID_t(0U), 0U, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);

		// Token: 0x04000A78 RID: 2680
		public static readonly CSteamID LanModeGS = new CSteamID(new AccountID_t(0U), 0U, EUniverse.k_EUniversePublic, EAccountType.k_EAccountTypeInvalid);

		// Token: 0x04000A79 RID: 2681
		public static readonly CSteamID NotInitYetGS = new CSteamID(new AccountID_t(1U), 0U, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);

		// Token: 0x04000A7A RID: 2682
		public static readonly CSteamID NonSteamGS = new CSteamID(new AccountID_t(2U), 0U, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);

		// Token: 0x04000A7B RID: 2683
		public ulong m_SteamID;
	}
}
