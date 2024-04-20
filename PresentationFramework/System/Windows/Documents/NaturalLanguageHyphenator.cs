using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Media.TextFormatting;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000645 RID: 1605
	internal class NaturalLanguageHyphenator : TextLexicalService, IDisposable
	{
		// Token: 0x06004F4F RID: 20303 RVA: 0x00243D00 File Offset: 0x00242D00
		internal NaturalLanguageHyphenator()
		{
			try
			{
				this._hyphenatorResource = NaturalLanguageHyphenator.UnsafeNativeMethods.NlCreateHyphenator();
			}
			catch (DllNotFoundException)
			{
			}
			catch (EntryPointNotFoundException)
			{
			}
		}

		// Token: 0x06004F50 RID: 20304 RVA: 0x00243D44 File Offset: 0x00242D44
		~NaturalLanguageHyphenator()
		{
			this.CleanupInternal(true);
		}

		// Token: 0x06004F51 RID: 20305 RVA: 0x00243D74 File Offset: 0x00242D74
		void IDisposable.Dispose()
		{
			GC.SuppressFinalize(this);
			this.CleanupInternal(false);
		}

		// Token: 0x06004F52 RID: 20306 RVA: 0x00243D83 File Offset: 0x00242D83
		private void CleanupInternal(bool finalizing)
		{
			if (!this._disposed && this._hyphenatorResource != IntPtr.Zero)
			{
				NaturalLanguageHyphenator.UnsafeNativeMethods.NlDestroyHyphenator(ref this._hyphenatorResource);
				this._disposed = true;
			}
		}

		// Token: 0x06004F53 RID: 20307 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool IsCultureSupported(CultureInfo culture)
		{
			return true;
		}

		// Token: 0x06004F54 RID: 20308 RVA: 0x00243DB4 File Offset: 0x00242DB4
		public override TextLexicalBreaks AnalyzeText(char[] characterSource, int length, CultureInfo textCulture)
		{
			Invariant.Assert(characterSource != null && characterSource.Length != 0 && length > 0 && length <= characterSource.Length);
			if (this._hyphenatorResource == IntPtr.Zero)
			{
				return null;
			}
			if (this._disposed)
			{
				throw new ObjectDisposedException(SR.Get("HyphenatorDisposed"));
			}
			byte[] array = new byte[(length + 7) / 8];
			NaturalLanguageHyphenator.UnsafeNativeMethods.NlHyphenate(this._hyphenatorResource, characterSource, length, (textCulture != null && textCulture != CultureInfo.InvariantCulture) ? textCulture.LCID : 0, array, array.Length);
			return new NaturalLanguageHyphenator.HyphenBreaks(array, length);
		}

		// Token: 0x04002851 RID: 10321
		private IntPtr _hyphenatorResource;

		// Token: 0x04002852 RID: 10322
		private bool _disposed;

		// Token: 0x02000B3D RID: 2877
		private class HyphenBreaks : TextLexicalBreaks
		{
			// Token: 0x06008CC1 RID: 36033 RVA: 0x0033E167 File Offset: 0x0033D167
			internal HyphenBreaks(byte[] isHyphenPositions, int numPositions)
			{
				this._isHyphenPositions = isHyphenPositions;
				this._numPositions = numPositions;
			}

			// Token: 0x17001ED2 RID: 7890
			private bool this[int index]
			{
				get
				{
					return ((int)this._isHyphenPositions[index / 8] & 1 << index % 8) != 0;
				}
			}

			// Token: 0x17001ED3 RID: 7891
			// (get) Token: 0x06008CC3 RID: 36035 RVA: 0x0033E195 File Offset: 0x0033D195
			public override int Length
			{
				get
				{
					return this._numPositions;
				}
			}

			// Token: 0x06008CC4 RID: 36036 RVA: 0x0033E1A0 File Offset: 0x0033D1A0
			public override int GetNextBreak(int currentIndex)
			{
				if (this._isHyphenPositions != null && currentIndex >= 0)
				{
					int num = currentIndex + 1;
					while (num < this._numPositions && !this[num])
					{
						num++;
					}
					if (num < this._numPositions)
					{
						return num;
					}
				}
				return -1;
			}

			// Token: 0x06008CC5 RID: 36037 RVA: 0x0033E1E4 File Offset: 0x0033D1E4
			public override int GetPreviousBreak(int currentIndex)
			{
				if (this._isHyphenPositions != null && currentIndex < this._numPositions)
				{
					int num = currentIndex;
					while (num > 0 && !this[num])
					{
						num--;
					}
					if (num > 0)
					{
						return num;
					}
				}
				return -1;
			}

			// Token: 0x04004853 RID: 18515
			private byte[] _isHyphenPositions;

			// Token: 0x04004854 RID: 18516
			private int _numPositions;
		}

		// Token: 0x02000B3E RID: 2878
		private static class UnsafeNativeMethods
		{
			// Token: 0x06008CC6 RID: 36038
			[DllImport("PresentationNative_cor3.dll", PreserveSig = false)]
			internal static extern IntPtr NlCreateHyphenator();

			// Token: 0x06008CC7 RID: 36039
			[DllImport("PresentationNative_cor3.dll")]
			internal static extern void NlDestroyHyphenator(ref IntPtr hyphenator);

			// Token: 0x06008CC8 RID: 36040
			[DllImport("PresentationNative_cor3.dll", PreserveSig = false)]
			internal static extern void NlHyphenate(IntPtr hyphenator, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeParamIndex = 2)] [In] char[] inputText, int textLength, int localeID, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)] [In] byte[] hyphenBreaks, int numPositions);
		}
	}
}
