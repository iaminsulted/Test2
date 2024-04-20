using System;
using System.Collections;
using System.Globalization;
using Microsoft.Win32;

namespace System.Windows.Documents
{
	// Token: 0x02000673 RID: 1651
	internal class FontTable : ArrayList
	{
		// Token: 0x06005178 RID: 20856 RVA: 0x0024F75A File Offset: 0x0024E75A
		internal FontTable() : base(20)
		{
			this._fontMappings = null;
		}

		// Token: 0x06005179 RID: 20857 RVA: 0x0024F76C File Offset: 0x0024E76C
		internal FontTableEntry DefineEntry(int index)
		{
			FontTableEntry fontTableEntry = this.FindEntryByIndex(index);
			if (fontTableEntry != null)
			{
				fontTableEntry.IsPending = true;
				fontTableEntry.Name = null;
				return fontTableEntry;
			}
			fontTableEntry = new FontTableEntry();
			fontTableEntry.Index = index;
			this.Add(fontTableEntry);
			return fontTableEntry;
		}

		// Token: 0x0600517A RID: 20858 RVA: 0x0024F7AC File Offset: 0x0024E7AC
		internal FontTableEntry FindEntryByIndex(int index)
		{
			for (int i = 0; i < this.Count; i++)
			{
				FontTableEntry fontTableEntry = this.EntryAt(i);
				if (fontTableEntry.Index == index)
				{
					return fontTableEntry;
				}
			}
			return null;
		}

		// Token: 0x0600517B RID: 20859 RVA: 0x0024F7E0 File Offset: 0x0024E7E0
		internal FontTableEntry FindEntryByName(string name)
		{
			for (int i = 0; i < this.Count; i++)
			{
				FontTableEntry fontTableEntry = this.EntryAt(i);
				if (name.Equals(fontTableEntry.Name))
				{
					return fontTableEntry;
				}
			}
			return null;
		}

		// Token: 0x0600517C RID: 20860 RVA: 0x0024F817 File Offset: 0x0024E817
		internal FontTableEntry EntryAt(int index)
		{
			return (FontTableEntry)this[index];
		}

		// Token: 0x0600517D RID: 20861 RVA: 0x0024F828 File Offset: 0x0024E828
		internal int DefineEntryByName(string name)
		{
			int num = -1;
			for (int i = 0; i < this.Count; i++)
			{
				FontTableEntry fontTableEntry = this.EntryAt(i);
				if (name.Equals(fontTableEntry.Name))
				{
					return fontTableEntry.Index;
				}
				if (fontTableEntry.Index > num)
				{
					num = fontTableEntry.Index;
				}
			}
			FontTableEntry fontTableEntry2 = new FontTableEntry();
			fontTableEntry2.Index = num + 1;
			this.Add(fontTableEntry2);
			fontTableEntry2.Name = name;
			return num + 1;
		}

		// Token: 0x0600517E RID: 20862 RVA: 0x0024F898 File Offset: 0x0024E898
		internal void MapFonts()
		{
			Hashtable fontMappings = this.FontMappings;
			for (int i = 0; i < this.Count; i++)
			{
				FontTableEntry fontTableEntry = this.EntryAt(i);
				if (fontTableEntry.Name != null)
				{
					string text = (string)fontMappings[fontTableEntry.Name.ToLower(CultureInfo.InvariantCulture)];
					if (text != null)
					{
						fontTableEntry.Name = text;
					}
					else
					{
						int num = fontTableEntry.Name.IndexOf('(');
						if (num >= 0)
						{
							while (num > 0 && fontTableEntry.Name[num - 1] == ' ')
							{
								num--;
							}
							fontTableEntry.Name = fontTableEntry.Name.Substring(0, num);
						}
					}
				}
			}
		}

		// Token: 0x1700132E RID: 4910
		// (get) Token: 0x0600517F RID: 20863 RVA: 0x0024F944 File Offset: 0x0024E944
		internal FontTableEntry CurrentEntry
		{
			get
			{
				if (this.Count == 0)
				{
					return null;
				}
				for (int i = this.Count - 1; i >= 0; i--)
				{
					FontTableEntry fontTableEntry = this.EntryAt(i);
					if (fontTableEntry.IsPending)
					{
						return fontTableEntry;
					}
				}
				return this.EntryAt(this.Count - 1);
			}
		}

		// Token: 0x1700132F RID: 4911
		// (get) Token: 0x06005180 RID: 20864 RVA: 0x0024F990 File Offset: 0x0024E990
		internal Hashtable FontMappings
		{
			get
			{
				if (this._fontMappings == null)
				{
					this._fontMappings = new Hashtable();
					RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\FontSubstitutes");
					if (registryKey != null)
					{
						foreach (string text in registryKey.GetValueNames())
						{
							string text2 = (string)registryKey.GetValue(text);
							if (text.Length > 0 && text2.Length > 0)
							{
								string text3 = text;
								string text4 = string.Empty;
								string text5 = text2;
								string text6 = string.Empty;
								int num = text.IndexOf(',');
								if (num >= 0)
								{
									text3 = text.Substring(0, num);
									text4 = text.Substring(num + 1, text.Length - num - 1);
								}
								num = text2.IndexOf(',');
								if (num >= 0)
								{
									text5 = text2.Substring(0, num);
									text6 = text2.Substring(num + 1, text2.Length - num - 1);
								}
								if (text3.Length > 0 && text5.Length > 0)
								{
									bool flag = false;
									if (text4.Length > 0 && text6.Length > 0)
									{
										if (string.Compare(text4, text6, StringComparison.OrdinalIgnoreCase) == 0)
										{
											flag = true;
										}
									}
									else if (text4.Length == 0 && text6.Length == 0)
									{
										if (text3.Length > text5.Length && string.Compare(text3.Substring(0, text5.Length), text5, StringComparison.OrdinalIgnoreCase) == 0)
										{
											flag = true;
										}
									}
									else if (text4.Length > 0 && text6.Length == 0)
									{
										flag = true;
									}
									if (flag)
									{
										string key = text3.ToLower(CultureInfo.InvariantCulture);
										if (this._fontMappings[key] == null)
										{
											this._fontMappings.Add(key, text5);
										}
									}
								}
							}
						}
					}
				}
				return this._fontMappings;
			}
		}

		// Token: 0x04002E6C RID: 11884
		private Hashtable _fontMappings;
	}
}
