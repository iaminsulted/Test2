using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020004A5 RID: 1189
	internal class BamlNamedElementStartRecord : BamlElementStartRecord
	{
		// Token: 0x06003D37 RID: 15671 RVA: 0x001FCD65 File Offset: 0x001FBD65
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.TypeId = bamlBinaryReader.ReadInt16();
			this.RuntimeName = bamlBinaryReader.ReadString();
		}

		// Token: 0x06003D38 RID: 15672 RVA: 0x001FCD7F File Offset: 0x001FBD7F
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(base.TypeId);
			if (this.RuntimeName != null)
			{
				bamlBinaryWriter.Write(this.RuntimeName);
			}
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x001FCDA1 File Offset: 0x001FBDA1
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlNamedElementStartRecord bamlNamedElementStartRecord = (BamlNamedElementStartRecord)record;
			bamlNamedElementStartRecord._isTemplateChild = this._isTemplateChild;
			bamlNamedElementStartRecord._runtimeName = this._runtimeName;
		}

		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x06003D3A RID: 15674 RVA: 0x001FCDC7 File Offset: 0x001FBDC7
		// (set) Token: 0x06003D3B RID: 15675 RVA: 0x001FCDCF File Offset: 0x001FBDCF
		internal string RuntimeName
		{
			get
			{
				return this._runtimeName;
			}
			set
			{
				this._runtimeName = value;
			}
		}

		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x06003D3C RID: 15676 RVA: 0x001FCDD8 File Offset: 0x001FBDD8
		// (set) Token: 0x06003D3D RID: 15677 RVA: 0x001FCDE0 File Offset: 0x001FBDE0
		internal bool IsTemplateChild
		{
			get
			{
				return this._isTemplateChild;
			}
			set
			{
				this._isTemplateChild = value;
			}
		}

		// Token: 0x04001ECF RID: 7887
		private bool _isTemplateChild;

		// Token: 0x04001ED0 RID: 7888
		private string _runtimeName;
	}
}
