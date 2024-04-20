using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace MS.Internal.Annotations
{
	// Token: 0x020002C1 RID: 705
	internal class ObservableDictionary : IDictionary<string, string>, ICollection<KeyValuePair<string, string>>, IEnumerable<KeyValuePair<string, string>>, IEnumerable, INotifyPropertyChanged
	{
		// Token: 0x06001A3E RID: 6718 RVA: 0x001633D9 File Offset: 0x001623D9
		public ObservableDictionary()
		{
			this._nameValues = new Dictionary<string, string>();
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x001633EC File Offset: 0x001623EC
		public void Add(string key, string val)
		{
			if (key == null || val == null)
			{
				throw new ArgumentNullException((key == null) ? "key" : "val");
			}
			this._nameValues.Add(key, val);
			this.FireDictionaryChanged();
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x0016341C File Offset: 0x0016241C
		public void Clear()
		{
			if (this._nameValues.Count > 0)
			{
				this._nameValues.Clear();
				this.FireDictionaryChanged();
			}
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x0016343D File Offset: 0x0016243D
		public bool ContainsKey(string key)
		{
			return this._nameValues.ContainsKey(key);
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x0016344B File Offset: 0x0016244B
		public bool Remove(string key)
		{
			bool flag = this._nameValues.Remove(key);
			if (flag)
			{
				this.FireDictionaryChanged();
			}
			return flag;
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x00163462 File Offset: 0x00162462
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._nameValues.GetEnumerator();
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x00163474 File Offset: 0x00162474
		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<string, string>>)this._nameValues).GetEnumerator();
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x00163481 File Offset: 0x00162481
		public bool TryGetValue(string key, out string value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return this._nameValues.TryGetValue(key, out value);
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x0016349E File Offset: 0x0016249E
		void ICollection<KeyValuePair<string, string>>.Add(KeyValuePair<string, string> pair)
		{
			((ICollection<KeyValuePair<string, string>>)this._nameValues).Add(pair);
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x001634AC File Offset: 0x001624AC
		bool ICollection<KeyValuePair<string, string>>.Contains(KeyValuePair<string, string> pair)
		{
			return ((ICollection<KeyValuePair<string, string>>)this._nameValues).Contains(pair);
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x001634BA File Offset: 0x001624BA
		bool ICollection<KeyValuePair<string, string>>.Remove(KeyValuePair<string, string> pair)
		{
			return ((ICollection<KeyValuePair<string, string>>)this._nameValues).Remove(pair);
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x001634C8 File Offset: 0x001624C8
		void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] target, int startIndex)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (startIndex < 0 || startIndex > target.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			((ICollection<KeyValuePair<string, string>>)this._nameValues).CopyTo(target, startIndex);
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001A4A RID: 6730 RVA: 0x001634FA File Offset: 0x001624FA
		public int Count
		{
			get
			{
				return this._nameValues.Count;
			}
		}

		// Token: 0x170004D7 RID: 1239
		public string this[string key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				string result = null;
				this._nameValues.TryGetValue(key, out result);
				return result;
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				string text = null;
				this._nameValues.TryGetValue(key, out text);
				if (text == null || text != value)
				{
					this._nameValues[key] = value;
					this.FireDictionaryChanged();
				}
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001A4D RID: 6733 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001A4E RID: 6734 RVA: 0x00163591 File Offset: 0x00162591
		public ICollection<string> Keys
		{
			get
			{
				return this._nameValues.Keys;
			}
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06001A4F RID: 6735 RVA: 0x0016359E File Offset: 0x0016259E
		public ICollection<string> Values
		{
			get
			{
				return this._nameValues.Values;
			}
		}

		// Token: 0x1400003B RID: 59
		// (add) Token: 0x06001A50 RID: 6736 RVA: 0x001635AC File Offset: 0x001625AC
		// (remove) Token: 0x06001A51 RID: 6737 RVA: 0x001635E4 File Offset: 0x001625E4
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06001A52 RID: 6738 RVA: 0x00163619 File Offset: 0x00162619
		private void FireDictionaryChanged()
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(null));
			}
		}

		// Token: 0x04000DB0 RID: 3504
		private Dictionary<string, string> _nameValues;
	}
}
