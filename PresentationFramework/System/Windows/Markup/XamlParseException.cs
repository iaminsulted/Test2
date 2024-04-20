using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using MS.Internal;

namespace System.Windows.Markup
{
	// Token: 0x02000507 RID: 1287
	[Serializable]
	public class XamlParseException : SystemException
	{
		// Token: 0x06004004 RID: 16388 RVA: 0x001EFBCA File Offset: 0x001EEBCA
		public XamlParseException()
		{
		}

		// Token: 0x06004005 RID: 16389 RVA: 0x001EFBD2 File Offset: 0x001EEBD2
		public XamlParseException(string message) : base(message)
		{
		}

		// Token: 0x06004006 RID: 16390 RVA: 0x001EFBDB File Offset: 0x001EEBDB
		public XamlParseException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06004007 RID: 16391 RVA: 0x00212AAC File Offset: 0x00211AAC
		public XamlParseException(string message, int lineNumber, int linePosition) : this(message)
		{
			this._lineNumber = lineNumber;
			this._linePosition = linePosition;
		}

		// Token: 0x06004008 RID: 16392 RVA: 0x00212AC3 File Offset: 0x00211AC3
		public XamlParseException(string message, int lineNumber, int linePosition, Exception innerException) : this(message, innerException)
		{
			this._lineNumber = lineNumber;
			this._linePosition = linePosition;
		}

		// Token: 0x06004009 RID: 16393 RVA: 0x00212ADC File Offset: 0x00211ADC
		internal XamlParseException(string message, int lineNumber, int linePosition, Uri baseUri, Exception innerException) : this(message, innerException)
		{
			this._lineNumber = lineNumber;
			this._linePosition = linePosition;
			this._baseUri = baseUri;
		}

		// Token: 0x17000E4C RID: 3660
		// (get) Token: 0x0600400A RID: 16394 RVA: 0x00212AFD File Offset: 0x00211AFD
		// (set) Token: 0x0600400B RID: 16395 RVA: 0x00212B05 File Offset: 0x00211B05
		public int LineNumber
		{
			get
			{
				return this._lineNumber;
			}
			internal set
			{
				this._lineNumber = value;
			}
		}

		// Token: 0x17000E4D RID: 3661
		// (get) Token: 0x0600400C RID: 16396 RVA: 0x00212B0E File Offset: 0x00211B0E
		// (set) Token: 0x0600400D RID: 16397 RVA: 0x00212B16 File Offset: 0x00211B16
		public int LinePosition
		{
			get
			{
				return this._linePosition;
			}
			internal set
			{
				this._linePosition = value;
			}
		}

		// Token: 0x17000E4E RID: 3662
		// (get) Token: 0x0600400E RID: 16398 RVA: 0x00212B1F File Offset: 0x00211B1F
		// (set) Token: 0x0600400F RID: 16399 RVA: 0x00212B27 File Offset: 0x00211B27
		public object KeyContext
		{
			get
			{
				return this._keyContext;
			}
			internal set
			{
				this._keyContext = value;
			}
		}

		// Token: 0x17000E4F RID: 3663
		// (get) Token: 0x06004010 RID: 16400 RVA: 0x00212B30 File Offset: 0x00211B30
		// (set) Token: 0x06004011 RID: 16401 RVA: 0x00212B38 File Offset: 0x00211B38
		public string UidContext
		{
			get
			{
				return this._uidContext;
			}
			internal set
			{
				this._uidContext = value;
			}
		}

		// Token: 0x17000E50 RID: 3664
		// (get) Token: 0x06004012 RID: 16402 RVA: 0x00212B41 File Offset: 0x00211B41
		// (set) Token: 0x06004013 RID: 16403 RVA: 0x00212B49 File Offset: 0x00211B49
		public string NameContext
		{
			get
			{
				return this._nameContext;
			}
			internal set
			{
				this._nameContext = value;
			}
		}

		// Token: 0x17000E51 RID: 3665
		// (get) Token: 0x06004014 RID: 16404 RVA: 0x00212B52 File Offset: 0x00211B52
		// (set) Token: 0x06004015 RID: 16405 RVA: 0x00212B5A File Offset: 0x00211B5A
		public Uri BaseUri
		{
			get
			{
				return this._baseUri;
			}
			internal set
			{
				this._baseUri = value;
			}
		}

		// Token: 0x06004016 RID: 16406 RVA: 0x00212B63 File Offset: 0x00211B63
		protected XamlParseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._lineNumber = info.GetInt32("Line");
			this._linePosition = info.GetInt32("Position");
		}

		// Token: 0x06004017 RID: 16407 RVA: 0x00212B8F File Offset: 0x00211B8F
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Line", this._lineNumber);
			info.AddValue("Position", this._linePosition);
		}

		// Token: 0x06004018 RID: 16408 RVA: 0x00212BBC File Offset: 0x00211BBC
		internal static string GetMarkupFilePath(Uri resourceUri)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			if (resourceUri != null)
			{
				if (resourceUri.IsAbsoluteUri)
				{
					text = resourceUri.GetComponents(UriComponents.Path, UriFormat.Unescaped);
				}
				else
				{
					text = resourceUri.OriginalString;
				}
				text2 = text.Replace(".baml", ".xaml");
				if (-1 == text2.LastIndexOf(".xaml", StringComparison.Ordinal))
				{
					text2 = string.Empty;
				}
			}
			return text2;
		}

		// Token: 0x06004019 RID: 16409 RVA: 0x00212C20 File Offset: 0x00211C20
		internal static string GenerateErrorMessageContext(int lineNumber, int linePosition, Uri baseUri, XamlObjectIds xamlObjectIds, Type objectType)
		{
			string result = " ";
			string markupFilePath = XamlParseException.GetMarkupFilePath(baseUri);
			string text = null;
			if (xamlObjectIds != null)
			{
				if (xamlObjectIds.Name != null)
				{
					text = xamlObjectIds.Name;
				}
				else if (xamlObjectIds.Key != null)
				{
					text = xamlObjectIds.Key.ToString();
				}
				else if (xamlObjectIds.Uid != null)
				{
					text = xamlObjectIds.Uid;
				}
			}
			if (text == null && objectType != null)
			{
				text = objectType.ToString();
			}
			XamlParseException.ContextBits contextBits = (XamlParseException.ContextBits)0;
			if (text != null)
			{
				contextBits |= XamlParseException.ContextBits.Type;
			}
			if (!string.IsNullOrEmpty(markupFilePath))
			{
				contextBits |= XamlParseException.ContextBits.File;
			}
			if (lineNumber > 0)
			{
				contextBits |= XamlParseException.ContextBits.Line;
			}
			switch (contextBits)
			{
			case (XamlParseException.ContextBits)0:
				result = string.Empty;
				break;
			case XamlParseException.ContextBits.Type:
				result = SR.Get("ParserErrorContext_Type", new object[]
				{
					text
				});
				break;
			case XamlParseException.ContextBits.File:
				result = SR.Get("ParserErrorContext_File", new object[]
				{
					markupFilePath
				});
				break;
			case XamlParseException.ContextBits.Type | XamlParseException.ContextBits.File:
				result = SR.Get("ParserErrorContext_Type_File", new object[]
				{
					text,
					markupFilePath
				});
				break;
			case XamlParseException.ContextBits.Line:
				result = SR.Get("ParserErrorContext_Line", new object[]
				{
					lineNumber,
					linePosition
				});
				break;
			case XamlParseException.ContextBits.Type | XamlParseException.ContextBits.Line:
				result = SR.Get("ParserErrorContext_Type_Line", new object[]
				{
					text,
					lineNumber,
					linePosition
				});
				break;
			case XamlParseException.ContextBits.File | XamlParseException.ContextBits.Line:
				result = SR.Get("ParserErrorContext_File_Line", new object[]
				{
					markupFilePath,
					lineNumber,
					linePosition
				});
				break;
			case XamlParseException.ContextBits.Type | XamlParseException.ContextBits.File | XamlParseException.ContextBits.Line:
				result = SR.Get("ParserErrorContext_Type_File_Line", new object[]
				{
					text,
					markupFilePath,
					lineNumber,
					linePosition
				});
				break;
			}
			return result;
		}

		// Token: 0x0600401A RID: 16410 RVA: 0x00212DD4 File Offset: 0x00211DD4
		internal static void ThrowException(string message, Exception innerException, int lineNumber, int linePosition, Uri baseUri, XamlObjectIds currentXamlObjectIds, XamlObjectIds contextXamlObjectIds, Type objectType)
		{
			if (innerException != null && innerException.Message != null)
			{
				StringBuilder stringBuilder = new StringBuilder(message);
				if (innerException.Message != string.Empty)
				{
					stringBuilder.Append(" ");
				}
				stringBuilder.Append(innerException.Message);
				message = stringBuilder.ToString();
			}
			string str = XamlParseException.GenerateErrorMessageContext(lineNumber, linePosition, baseUri, currentXamlObjectIds, objectType);
			message = message + "  " + str;
			XamlParseException ex;
			if (innerException is TargetInvocationException && innerException.InnerException is XamlParseException)
			{
				ex = (XamlParseException)innerException.InnerException;
			}
			else if (lineNumber > 0)
			{
				ex = new XamlParseException(message, lineNumber, linePosition, innerException);
			}
			else
			{
				ex = new XamlParseException(message, innerException);
			}
			if (contextXamlObjectIds != null)
			{
				ex.NameContext = contextXamlObjectIds.Name;
				ex.UidContext = contextXamlObjectIds.Uid;
				ex.KeyContext = contextXamlObjectIds.Key;
			}
			ex.BaseUri = baseUri;
			if (TraceMarkup.IsEnabled)
			{
				TraceMarkup.TraceActivityItem(TraceMarkup.ThrowException, ex);
			}
			throw ex;
		}

		// Token: 0x0600401B RID: 16411 RVA: 0x00212EC3 File Offset: 0x00211EC3
		internal static void ThrowException(ParserContext parserContext, int lineNumber, int linePosition, string message, Exception innerException)
		{
			XamlParseException.ThrowException(message, innerException, lineNumber, linePosition);
		}

		// Token: 0x0600401C RID: 16412 RVA: 0x00212ECF File Offset: 0x00211ECF
		internal static void ThrowException(string message, Exception innerException, int lineNumber, int linePosition)
		{
			XamlParseException.ThrowException(message, innerException, lineNumber, linePosition, null, null, null, null);
		}

		// Token: 0x040023F3 RID: 9203
		internal const string BamlExt = ".baml";

		// Token: 0x040023F4 RID: 9204
		internal const string XamlExt = ".xaml";

		// Token: 0x040023F5 RID: 9205
		private int _lineNumber;

		// Token: 0x040023F6 RID: 9206
		private int _linePosition;

		// Token: 0x040023F7 RID: 9207
		private object _keyContext;

		// Token: 0x040023F8 RID: 9208
		private string _uidContext;

		// Token: 0x040023F9 RID: 9209
		private string _nameContext;

		// Token: 0x040023FA RID: 9210
		private Uri _baseUri;

		// Token: 0x02000AFD RID: 2813
		[Flags]
		private enum ContextBits
		{
			// Token: 0x0400475B RID: 18267
			Type = 1,
			// Token: 0x0400475C RID: 18268
			File = 2,
			// Token: 0x0400475D RID: 18269
			Line = 4
		}
	}
}
