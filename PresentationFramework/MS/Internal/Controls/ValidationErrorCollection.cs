using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace MS.Internal.Controls
{
	// Token: 0x02000260 RID: 608
	internal class ValidationErrorCollection : ObservableCollection<ValidationError>
	{
		// Token: 0x04000CB0 RID: 3248
		public static readonly ReadOnlyObservableCollection<ValidationError> Empty = new ReadOnlyObservableCollection<ValidationError>(new ValidationErrorCollection());
	}
}
