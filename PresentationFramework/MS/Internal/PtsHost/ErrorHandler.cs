using System;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200011B RID: 283
	internal static class ErrorHandler
	{
		// Token: 0x0600073E RID: 1854 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal static void Assert(bool condition, string message)
		{
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal static void Assert(bool condition, string format, params object[] args)
		{
		}

		// Token: 0x0400074A RID: 1866
		internal static string PtsCacheAlreadyCreated = "PTS cache is already created.";

		// Token: 0x0400074B RID: 1867
		internal static string PtsCacheAlreadyDestroyed = "PTS cache is already destroyed.";

		// Token: 0x0400074C RID: 1868
		internal static string NullPtsHost = "Valid PtsHost object is required.";

		// Token: 0x0400074D RID: 1869
		internal static string CreateContextFailed = "Failed to create PTS Context.";

		// Token: 0x0400074E RID: 1870
		internal static string EnumIntegrationError = "Some enum values has been changed. Need to update dependent code.";

		// Token: 0x0400074F RID: 1871
		internal static string NoNeedToDestroyPtsPage = "PTS page is not created, there is no need to destroy it.";

		// Token: 0x04000750 RID: 1872
		internal static string NotSupportedFiniteIncremental = "Incremental update is not supported yet.";

		// Token: 0x04000751 RID: 1873
		internal static string NotSupportedFootnotes = "Footnotes are not supported yet.";

		// Token: 0x04000752 RID: 1874
		internal static string NotSupportedCompositeColumns = "Composite columns are not supported yet.";

		// Token: 0x04000753 RID: 1875
		internal static string NotSupportedDropCap = "DropCap is not supported yet.";

		// Token: 0x04000754 RID: 1876
		internal static string NotSupportedForcedLineBreaks = "Forced vertical line break is not supported yet.";

		// Token: 0x04000755 RID: 1877
		internal static string NotSupportedMultiSection = "Multiply sections are not supported yet.";

		// Token: 0x04000756 RID: 1878
		internal static string NotSupportedSubtrackShift = "Column shifting is not supported yet.";

		// Token: 0x04000757 RID: 1879
		internal static string NullObjectInCreateHandle = "Valid object is required to create handle.";

		// Token: 0x04000758 RID: 1880
		internal static string InvalidHandle = "No object associated with the handle or type mismatch.";

		// Token: 0x04000759 RID: 1881
		internal static string HandleOutOfRange = "Object handle has to be within handle array's range.";

		// Token: 0x0400075A RID: 1882
		internal static string BreakRecordDisposed = "Break record already disposed.";

		// Token: 0x0400075B RID: 1883
		internal static string BreakRecordNotNeeded = "There is no need to create break record.";

		// Token: 0x0400075C RID: 1884
		internal static string BrokenParaHasMcs = "Broken paragraph cannot have margin collapsing state.";

		// Token: 0x0400075D RID: 1885
		internal static string BrokenParaHasTopSpace = "Top space should be always suppressed at the top of broken paragraph.";

		// Token: 0x0400075E RID: 1886
		internal static string GoalReachedHasBreakRecord = "Goal is reached, so there should be no break record.";

		// Token: 0x0400075F RID: 1887
		internal static string BrokenContentRequiresBreakRecord = "Goal is not reached, break record is required to continue.";

		// Token: 0x04000760 RID: 1888
		internal static string PTSAssert = "PTS Assert:\n\t{0}\n\t{1}\n\t{2}\n\t{3}";

		// Token: 0x04000761 RID: 1889
		internal static string ParaHandleMismatch = "Paragraph handle mismatch.";

		// Token: 0x04000762 RID: 1890
		internal static string PTSObjectsCountMismatch = "Actual number of PTS objects does not match number of requested PTS objects.";

		// Token: 0x04000763 RID: 1891
		internal static string SubmitForEmptyRange = "Submitting embedded objects for empty range.";

		// Token: 0x04000764 RID: 1892
		internal static string SubmitInvalidList = "Submitting invalid list of embedded objects.";

		// Token: 0x04000765 RID: 1893
		internal static string HandledInsideSegmentPara = "Paragraph structure invalidation should be handled by Segments.";

		// Token: 0x04000766 RID: 1894
		internal static string EmptyParagraph = "There are no lines in the paragraph.";

		// Token: 0x04000767 RID: 1895
		internal static string ParaStartsWithEOP = "NameTable is out of sync with TextContainer. The next paragraph begins with EOP.";

		// Token: 0x04000768 RID: 1896
		internal static string FetchParaAtTextRangePosition = "Trying to fetch paragraph at not supported TextPointer - TextRange.";

		// Token: 0x04000769 RID: 1897
		internal static string ParagraphCharacterCountMismatch = "Paragraph's character count is out of sync.";

		// Token: 0x0400076A RID: 1898
		internal static string ContainerNeedsTextElement = "Container paragraph can be only created for TextElement.";

		// Token: 0x0400076B RID: 1899
		internal static string CannotPositionInsideUIElement = "Cannot position TextPointer inside a UIElement.";

		// Token: 0x0400076C RID: 1900
		internal static string CannotFindUIElement = "Cannot find specified UIElement in the TextContainer.";

		// Token: 0x0400076D RID: 1901
		internal static string InvalidDocumentPage = "DocumentPage is not created for IDocumentPaginatorSource object.";

		// Token: 0x0400076E RID: 1902
		internal static string NoVisualToTransfer = "Old paragraph does not have a visual node. Cannot transfer data.";

		// Token: 0x0400076F RID: 1903
		internal static string UpdateShiftedNotValid = "Update shifted is not a valid update type for top level PTS objects.";

		// Token: 0x04000770 RID: 1904
		internal static string ColumnVisualCountMismatch = "Number of column visuals does not match number of columns.";

		// Token: 0x04000771 RID: 1905
		internal static string VisualTypeMismatch = "Visual does not match expected type.";

		// Token: 0x04000772 RID: 1906
		internal static string EmbeddedObjectTypeMismatch = "EmbeddedObject type missmatch.";

		// Token: 0x04000773 RID: 1907
		internal static string EmbeddedObjectOwnerMismatch = "Cannot transfer data from an embedded object representing another element.";

		// Token: 0x04000774 RID: 1908
		internal static string LineAlreadyDestroyed = "Line has been already disposed.";

		// Token: 0x04000775 RID: 1909
		internal static string OnlyOneRectIsExpected = "Expecting only one rect for text object.";

		// Token: 0x04000776 RID: 1910
		internal static string NotInLineBoundary = "Requesting data outside of line's range.";

		// Token: 0x04000777 RID: 1911
		internal static string FetchRunAtTextArrayStart = "Trying to fetch run at the beginning of TextContainer.";

		// Token: 0x04000778 RID: 1912
		internal static string TextFormatterHostNotInitialized = "TextFormatter host is not initialized.";

		// Token: 0x04000779 RID: 1913
		internal static string NegativeCharacterIndex = "Character index must be non-negative.";

		// Token: 0x0400077A RID: 1914
		internal static string NoClientDataForObjectRun = "ClientData should be always provided for object runs.";

		// Token: 0x0400077B RID: 1915
		internal static string UnknownDOTypeInTextArray = "Unknown DependencyObject type stored in TextContainer.";

		// Token: 0x0400077C RID: 1916
		internal static string NegativeObjectWidth = "Negative object's width within a text line.";

		// Token: 0x0400077D RID: 1917
		internal static string NoUIElementForObjectPosition = "TextContainer does not have a UIElement for position of Object type.";

		// Token: 0x0400077E RID: 1918
		internal static string InlineObjectCacheCorrupted = "Paragraph's inline object cache is corrupted.";
	}
}
