using System;
using System.Printing;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Media;
using System.Windows.Xps;
using System.Windows.Xps.Serialization;
using MS.Internal.Printing;

namespace System.Windows.Controls
{
	// Token: 0x020007C3 RID: 1987
	public class PrintDialog
	{
		// Token: 0x060071BB RID: 29115 RVA: 0x002DB76C File Offset: 0x002DA76C
		public PrintDialog()
		{
			this._printQueue = null;
			this._printTicket = null;
			this._isPrintableAreaWidthUpdated = false;
			this._isPrintableAreaHeightUpdated = false;
			this._pageRangeSelection = PageRangeSelection.AllPages;
			this._minPage = 1U;
			this._maxPage = 9999U;
			this._userPageRangeEnabled = false;
		}

		// Token: 0x17001A56 RID: 6742
		// (get) Token: 0x060071BC RID: 29116 RVA: 0x002DB7BB File Offset: 0x002DA7BB
		// (set) Token: 0x060071BD RID: 29117 RVA: 0x002DB7C3 File Offset: 0x002DA7C3
		public PageRangeSelection PageRangeSelection
		{
			get
			{
				return this._pageRangeSelection;
			}
			set
			{
				this._pageRangeSelection = value;
			}
		}

		// Token: 0x17001A57 RID: 6743
		// (get) Token: 0x060071BE RID: 29118 RVA: 0x002DB7CC File Offset: 0x002DA7CC
		// (set) Token: 0x060071BF RID: 29119 RVA: 0x002DB7D4 File Offset: 0x002DA7D4
		public PageRange PageRange
		{
			get
			{
				return this._pageRange;
			}
			set
			{
				if (value.PageTo <= 0 || value.PageFrom <= 0)
				{
					throw new ArgumentException(SR.Get("PrintDialogInvalidPageRange"), "PageRange");
				}
				this._pageRange = value;
				if (this._pageRange.PageFrom > this._pageRange.PageTo)
				{
					int pageFrom = this._pageRange.PageFrom;
					this._pageRange.PageFrom = this._pageRange.PageTo;
					this._pageRange.PageTo = pageFrom;
				}
			}
		}

		// Token: 0x17001A58 RID: 6744
		// (get) Token: 0x060071C0 RID: 29120 RVA: 0x002DB857 File Offset: 0x002DA857
		// (set) Token: 0x060071C1 RID: 29121 RVA: 0x002DB85F File Offset: 0x002DA85F
		public bool UserPageRangeEnabled
		{
			get
			{
				return this._userPageRangeEnabled;
			}
			set
			{
				this._userPageRangeEnabled = value;
			}
		}

		// Token: 0x17001A59 RID: 6745
		// (get) Token: 0x060071C2 RID: 29122 RVA: 0x002DB868 File Offset: 0x002DA868
		// (set) Token: 0x060071C3 RID: 29123 RVA: 0x002DB870 File Offset: 0x002DA870
		public bool SelectedPagesEnabled
		{
			get
			{
				return this._selectedPagesEnabled;
			}
			set
			{
				this._selectedPagesEnabled = value;
			}
		}

		// Token: 0x17001A5A RID: 6746
		// (get) Token: 0x060071C4 RID: 29124 RVA: 0x002DB879 File Offset: 0x002DA879
		// (set) Token: 0x060071C5 RID: 29125 RVA: 0x002DB881 File Offset: 0x002DA881
		public bool CurrentPageEnabled
		{
			get
			{
				return this._currentPageEnabled;
			}
			set
			{
				this._currentPageEnabled = value;
			}
		}

		// Token: 0x17001A5B RID: 6747
		// (get) Token: 0x060071C6 RID: 29126 RVA: 0x002DB88A File Offset: 0x002DA88A
		// (set) Token: 0x060071C7 RID: 29127 RVA: 0x002DB892 File Offset: 0x002DA892
		public uint MinPage
		{
			get
			{
				return this._minPage;
			}
			set
			{
				if (this._minPage <= 0U)
				{
					throw new ArgumentException(SR.Get("PrintDialogZeroNotAllowed", new object[]
					{
						"MinPage"
					}));
				}
				this._minPage = value;
			}
		}

		// Token: 0x17001A5C RID: 6748
		// (get) Token: 0x060071C8 RID: 29128 RVA: 0x002DB8C2 File Offset: 0x002DA8C2
		// (set) Token: 0x060071C9 RID: 29129 RVA: 0x002DB8CA File Offset: 0x002DA8CA
		public uint MaxPage
		{
			get
			{
				return this._maxPage;
			}
			set
			{
				if (this._maxPage <= 0U)
				{
					throw new ArgumentException(SR.Get("PrintDialogZeroNotAllowed", new object[]
					{
						"MaxPage"
					}));
				}
				this._maxPage = value;
			}
		}

		// Token: 0x17001A5D RID: 6749
		// (get) Token: 0x060071CA RID: 29130 RVA: 0x002DB8FA File Offset: 0x002DA8FA
		// (set) Token: 0x060071CB RID: 29131 RVA: 0x002DB916 File Offset: 0x002DA916
		public PrintQueue PrintQueue
		{
			get
			{
				if (this._printQueue == null)
				{
					this._printQueue = this.AcquireDefaultPrintQueue();
				}
				return this._printQueue;
			}
			set
			{
				this._printQueue = value;
			}
		}

		// Token: 0x17001A5E RID: 6750
		// (get) Token: 0x060071CC RID: 29132 RVA: 0x002DB91F File Offset: 0x002DA91F
		// (set) Token: 0x060071CD RID: 29133 RVA: 0x002DB941 File Offset: 0x002DA941
		public PrintTicket PrintTicket
		{
			get
			{
				if (this._printTicket == null)
				{
					this._printTicket = this.AcquireDefaultPrintTicket(this.PrintQueue);
				}
				return this._printTicket;
			}
			set
			{
				this._printTicket = value;
			}
		}

		// Token: 0x17001A5F RID: 6751
		// (get) Token: 0x060071CE RID: 29134 RVA: 0x002DB94A File Offset: 0x002DA94A
		public double PrintableAreaWidth
		{
			get
			{
				if ((!this._isPrintableAreaWidthUpdated && !this._isPrintableAreaHeightUpdated) || (this._isPrintableAreaWidthUpdated && !this._isPrintableAreaHeightUpdated))
				{
					this._isPrintableAreaWidthUpdated = true;
					this._isPrintableAreaHeightUpdated = false;
					this.UpdatePrintableAreaSize();
				}
				return this._printableAreaWidth;
			}
		}

		// Token: 0x17001A60 RID: 6752
		// (get) Token: 0x060071CF RID: 29135 RVA: 0x002DB986 File Offset: 0x002DA986
		public double PrintableAreaHeight
		{
			get
			{
				if ((!this._isPrintableAreaWidthUpdated && !this._isPrintableAreaHeightUpdated) || (!this._isPrintableAreaWidthUpdated && this._isPrintableAreaHeightUpdated))
				{
					this._isPrintableAreaWidthUpdated = false;
					this._isPrintableAreaHeightUpdated = true;
					this.UpdatePrintableAreaSize();
				}
				return this._printableAreaHeight;
			}
		}

		// Token: 0x060071D0 RID: 29136 RVA: 0x002DB9C4 File Offset: 0x002DA9C4
		public bool? ShowDialog()
		{
			Win32PrintDialog win32PrintDialog = new Win32PrintDialog();
			win32PrintDialog.PrintTicket = this._printTicket;
			win32PrintDialog.PrintQueue = this._printQueue;
			win32PrintDialog.MinPage = Math.Max(1U, Math.Min(this._minPage, this._maxPage));
			win32PrintDialog.MaxPage = Math.Max(win32PrintDialog.MinPage, Math.Max(this._minPage, this._maxPage));
			win32PrintDialog.PageRangeEnabled = this._userPageRangeEnabled;
			win32PrintDialog.SelectedPagesEnabled = this._selectedPagesEnabled;
			win32PrintDialog.CurrentPageEnabled = this._currentPageEnabled;
			win32PrintDialog.PageRange = new PageRange(Math.Max((int)win32PrintDialog.MinPage, this._pageRange.PageFrom), Math.Min((int)win32PrintDialog.MaxPage, this._pageRange.PageTo));
			win32PrintDialog.PageRangeSelection = this._pageRangeSelection;
			uint num = win32PrintDialog.ShowDialog();
			if (num == 2U || num == 1U)
			{
				this._printTicket = win32PrintDialog.PrintTicket;
				this._printQueue = win32PrintDialog.PrintQueue;
				this._pageRange = win32PrintDialog.PageRange;
				this._pageRangeSelection = win32PrintDialog.PageRangeSelection;
			}
			return new bool?(num == 1U);
		}

		// Token: 0x060071D1 RID: 29137 RVA: 0x002DBAE0 File Offset: 0x002DAAE0
		public void PrintVisual(Visual visual, string description)
		{
			if (visual == null)
			{
				throw new ArgumentNullException("visual");
			}
			this.CreateWriter(description).Write(visual, this._printTicket);
			this._printableAreaWidth = 0.0;
			this._printableAreaHeight = 0.0;
			this._isPrintableAreaWidthUpdated = false;
			this._isPrintableAreaHeightUpdated = false;
		}

		// Token: 0x060071D2 RID: 29138 RVA: 0x002DBB3C File Offset: 0x002DAB3C
		public void PrintDocument(DocumentPaginator documentPaginator, string description)
		{
			if (documentPaginator == null)
			{
				throw new ArgumentNullException("documentPaginator");
			}
			this.CreateWriter(description).Write(documentPaginator, this._printTicket);
			this._printableAreaWidth = 0.0;
			this._printableAreaHeight = 0.0;
			this._isPrintableAreaWidthUpdated = false;
			this._isPrintableAreaHeightUpdated = false;
		}

		// Token: 0x060071D3 RID: 29139 RVA: 0x002DBB98 File Offset: 0x002DAB98
		private PrintQueue AcquireDefaultPrintQueue()
		{
			PrintQueue result = null;
			try
			{
				result = new LocalPrintServer().DefaultPrintQueue;
			}
			catch (PrintSystemException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060071D4 RID: 29140 RVA: 0x002DBBCC File Offset: 0x002DABCC
		private PrintTicket AcquireDefaultPrintTicket(PrintQueue printQueue)
		{
			PrintTicket printTicket = null;
			try
			{
				if (printQueue != null)
				{
					printTicket = printQueue.UserPrintTicket;
					if (printTicket == null)
					{
						printTicket = printQueue.DefaultPrintTicket;
					}
				}
			}
			catch (PrintSystemException)
			{
				printTicket = null;
			}
			if (printTicket == null)
			{
				printTicket = new PrintTicket();
			}
			return printTicket;
		}

		// Token: 0x060071D5 RID: 29141 RVA: 0x002DBC10 File Offset: 0x002DAC10
		private void UpdatePrintableAreaSize()
		{
			PrintQueue printQueue = null;
			PrintTicket printTicket = null;
			this.PickCorrectPrintingEnvironment(ref printQueue, ref printTicket);
			PrintCapabilities printCapabilities = null;
			if (printQueue != null)
			{
				printCapabilities = printQueue.GetPrintCapabilities(printTicket);
			}
			if (printCapabilities != null && printCapabilities.OrientedPageMediaWidth != null && printCapabilities.OrientedPageMediaHeight != null)
			{
				this._printableAreaWidth = printCapabilities.OrientedPageMediaWidth.Value;
				this._printableAreaHeight = printCapabilities.OrientedPageMediaHeight.Value;
				return;
			}
			this._printableAreaWidth = 816.0;
			this._printableAreaHeight = 1056.0;
			if (printTicket.PageMediaSize != null && printTicket.PageMediaSize.Width != null && printTicket.PageMediaSize.Height != null)
			{
				this._printableAreaWidth = printTicket.PageMediaSize.Width.Value;
				this._printableAreaHeight = printTicket.PageMediaSize.Height.Value;
			}
			if (printTicket.PageOrientation != null)
			{
				PageOrientation value = printTicket.PageOrientation.Value;
				if (value == PageOrientation.Landscape || value == PageOrientation.ReverseLandscape)
				{
					double printableAreaWidth = this._printableAreaWidth;
					this._printableAreaWidth = this._printableAreaHeight;
					this._printableAreaHeight = printableAreaWidth;
				}
			}
		}

		// Token: 0x060071D6 RID: 29142 RVA: 0x002DBD54 File Offset: 0x002DAD54
		private XpsDocumentWriter CreateWriter(string description)
		{
			PrintQueue printQueue = null;
			PrintTicket printTicket = null;
			this.PickCorrectPrintingEnvironment(ref printQueue, ref printTicket);
			if (printQueue != null)
			{
				printQueue.CurrentJobSettings.Description = description;
			}
			XpsDocumentWriter xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(printQueue);
			PrintDialog.PrintDlgPrintTicketEventHandler @object = new PrintDialog.PrintDlgPrintTicketEventHandler(printTicket);
			xpsDocumentWriter.WritingPrintTicketRequired += @object.SetPrintTicket;
			return xpsDocumentWriter;
		}

		// Token: 0x060071D7 RID: 29143 RVA: 0x002DBD9D File Offset: 0x002DAD9D
		private void PickCorrectPrintingEnvironment(ref PrintQueue printQueue, ref PrintTicket printTicket)
		{
			if (this._printQueue == null)
			{
				this._printQueue = this.AcquireDefaultPrintQueue();
			}
			if (this._printTicket == null)
			{
				this._printTicket = this.AcquireDefaultPrintTicket(this._printQueue);
			}
			printQueue = this._printQueue;
			printTicket = this._printTicket;
		}

		// Token: 0x04003734 RID: 14132
		private PrintTicket _printTicket;

		// Token: 0x04003735 RID: 14133
		private PrintQueue _printQueue;

		// Token: 0x04003736 RID: 14134
		private PageRangeSelection _pageRangeSelection;

		// Token: 0x04003737 RID: 14135
		private PageRange _pageRange;

		// Token: 0x04003738 RID: 14136
		private bool _userPageRangeEnabled;

		// Token: 0x04003739 RID: 14137
		private bool _selectedPagesEnabled;

		// Token: 0x0400373A RID: 14138
		private bool _currentPageEnabled;

		// Token: 0x0400373B RID: 14139
		private uint _minPage;

		// Token: 0x0400373C RID: 14140
		private uint _maxPage;

		// Token: 0x0400373D RID: 14141
		private double _printableAreaWidth;

		// Token: 0x0400373E RID: 14142
		private double _printableAreaHeight;

		// Token: 0x0400373F RID: 14143
		private bool _isPrintableAreaWidthUpdated;

		// Token: 0x04003740 RID: 14144
		private bool _isPrintableAreaHeightUpdated;

		// Token: 0x02000C14 RID: 3092
		internal class PrintDlgPrintTicketEventHandler
		{
			// Token: 0x06009062 RID: 36962 RVA: 0x00346803 File Offset: 0x00345803
			public PrintDlgPrintTicketEventHandler(PrintTicket printTicket)
			{
				this._printTicket = printTicket;
			}

			// Token: 0x06009063 RID: 36963 RVA: 0x00346812 File Offset: 0x00345812
			public void SetPrintTicket(object sender, WritingPrintTicketRequiredEventArgs args)
			{
				if (args.CurrentPrintTicketLevel == PrintTicketLevel.FixedDocumentSequencePrintTicket)
				{
					args.CurrentPrintTicket = this._printTicket;
				}
			}

			// Token: 0x04004AEA RID: 19178
			private PrintTicket _printTicket;
		}
	}
}
