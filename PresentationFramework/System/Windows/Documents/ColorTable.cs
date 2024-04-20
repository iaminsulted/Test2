using System;
using System.Collections;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x02000675 RID: 1653
	internal class ColorTable : ArrayList
	{
		// Token: 0x06005189 RID: 20873 RVA: 0x0024FC17 File Offset: 0x0024EC17
		internal ColorTable() : base(20)
		{
			this._inProgress = false;
		}

		// Token: 0x0600518A RID: 20874 RVA: 0x0024FC28 File Offset: 0x0024EC28
		internal Color ColorAt(int index)
		{
			if (index >= 0 && index < this.Count)
			{
				return this.EntryAt(index).Color;
			}
			return Color.FromArgb(byte.MaxValue, 0, 0, 0);
		}

		// Token: 0x0600518B RID: 20875 RVA: 0x0024FC54 File Offset: 0x0024EC54
		internal void FinishColor()
		{
			if (this._inProgress)
			{
				this._inProgress = false;
				return;
			}
			int index = this.AddColor(Color.FromArgb(byte.MaxValue, 0, 0, 0));
			this.EntryAt(index).IsAuto = true;
		}

		// Token: 0x0600518C RID: 20876 RVA: 0x0024FC94 File Offset: 0x0024EC94
		internal int AddColor(Color color)
		{
			for (int i = 0; i < this.Count; i++)
			{
				if (this.ColorAt(i) == color)
				{
					return i;
				}
			}
			this.Add(new ColorTableEntry
			{
				Color = color
			});
			return this.Count - 1;
		}

		// Token: 0x0600518D RID: 20877 RVA: 0x0024FCE0 File Offset: 0x0024ECE0
		internal ColorTableEntry EntryAt(int index)
		{
			if (index >= 0 && index < this.Count)
			{
				return (ColorTableEntry)this[index];
			}
			return null;
		}

		// Token: 0x17001335 RID: 4917
		// (set) Token: 0x0600518E RID: 20878 RVA: 0x0024FD00 File Offset: 0x0024ED00
		internal byte NewRed
		{
			set
			{
				ColorTableEntry inProgressEntry = this.GetInProgressEntry();
				if (inProgressEntry != null)
				{
					inProgressEntry.Red = value;
				}
			}
		}

		// Token: 0x17001336 RID: 4918
		// (set) Token: 0x0600518F RID: 20879 RVA: 0x0024FD20 File Offset: 0x0024ED20
		internal byte NewGreen
		{
			set
			{
				ColorTableEntry inProgressEntry = this.GetInProgressEntry();
				if (inProgressEntry != null)
				{
					inProgressEntry.Green = value;
				}
			}
		}

		// Token: 0x17001337 RID: 4919
		// (set) Token: 0x06005190 RID: 20880 RVA: 0x0024FD40 File Offset: 0x0024ED40
		internal byte NewBlue
		{
			set
			{
				ColorTableEntry inProgressEntry = this.GetInProgressEntry();
				if (inProgressEntry != null)
				{
					inProgressEntry.Blue = value;
				}
			}
		}

		// Token: 0x06005191 RID: 20881 RVA: 0x0024FD60 File Offset: 0x0024ED60
		private ColorTableEntry GetInProgressEntry()
		{
			if (this._inProgress)
			{
				return this.EntryAt(this.Count - 1);
			}
			this._inProgress = true;
			ColorTableEntry colorTableEntry = new ColorTableEntry();
			this.Add(colorTableEntry);
			return colorTableEntry;
		}

		// Token: 0x04002E6F RID: 11887
		private bool _inProgress;
	}
}
