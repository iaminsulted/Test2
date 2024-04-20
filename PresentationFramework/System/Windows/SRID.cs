using System;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
using FxResources.PresentationFramework;

namespace System.Windows
{
	// Token: 0x020003E8 RID: 1000
	internal static class SRID
	{
		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x06002B00 RID: 11008 RVA: 0x001A0D08 File Offset: 0x0019FD08
		internal static ResourceManager ResourceManager
		{
			get
			{
				ResourceManager result;
				if ((result = SRID.s_resourceManager) == null)
				{
					result = (SRID.s_resourceManager = new ResourceManager(typeof(SR)));
				}
				return result;
			}
		}

		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x06002B01 RID: 11009 RVA: 0x001A0D28 File Offset: 0x0019FD28
		// (set) Token: 0x06002B02 RID: 11010 RVA: 0x001A0D2F File Offset: 0x0019FD2F
		internal static CultureInfo Culture { get; set; }

		// Token: 0x06002B03 RID: 11011 RVA: 0x001A0D37 File Offset: 0x0019FD37
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static string GetResourceString(string resourceKey, string defaultValue = null)
		{
			return SRID.ResourceManager.GetString(resourceKey, SRID.Culture);
		}

		// Token: 0x04001579 RID: 5497
		private static ResourceManager s_resourceManager;

		// Token: 0x0400157B RID: 5499
		internal const string AbsoluteUriNotAllowed = "AbsoluteUriNotAllowed";

		// Token: 0x0400157C RID: 5500
		internal const string AbsoluteUriOnly = "AbsoluteUriOnly";

		// Token: 0x0400157D RID: 5501
		internal const string AccessCollectionAfterShutDown = "AccessCollectionAfterShutDown";

		// Token: 0x0400157E RID: 5502
		internal const string AddAnnotationsNotImplemented = "AddAnnotationsNotImplemented";

		// Token: 0x0400157F RID: 5503
		internal const string AddedItemNotAtIndex = "AddedItemNotAtIndex";

		// Token: 0x04001580 RID: 5504
		internal const string AddedItemNotInCollection = "AddedItemNotInCollection";

		// Token: 0x04001581 RID: 5505
		internal const string AdornedElementNotFound = "AdornedElementNotFound";

		// Token: 0x04001582 RID: 5506
		internal const string AdornedElementPlaceholderMustBeInTemplate = "AdornedElementPlaceholderMustBeInTemplate";

		// Token: 0x04001583 RID: 5507
		internal const string AdornerNotFound = "AdornerNotFound";

		// Token: 0x04001584 RID: 5508
		internal const string AffectedByMsCtfIssue = "AffectedByMsCtfIssue";

		// Token: 0x04001585 RID: 5509
		internal const string AlreadyHasLogicalChildren = "AlreadyHasLogicalChildren";

		// Token: 0x04001586 RID: 5510
		internal const string AlreadyHasParent = "AlreadyHasParent";

		// Token: 0x04001587 RID: 5511
		internal const string AnnotationAdorner_NotUIElement = "AnnotationAdorner_NotUIElement";

		// Token: 0x04001588 RID: 5512
		internal const string AnnotationAlreadyExistInService = "AnnotationAlreadyExistInService";

		// Token: 0x04001589 RID: 5513
		internal const string AnnotationAlreadyExists = "AnnotationAlreadyExists";

		// Token: 0x0400158A RID: 5514
		internal const string AnnotationIsNull = "AnnotationIsNull";

		// Token: 0x0400158B RID: 5515
		internal const string AnnotationServiceAlreadyExists = "AnnotationServiceAlreadyExists";

		// Token: 0x0400158C RID: 5516
		internal const string AnnotationServiceIsAlreadyEnabled = "AnnotationServiceIsAlreadyEnabled";

		// Token: 0x0400158D RID: 5517
		internal const string AnnotationServiceNotEnabled = "AnnotationServiceNotEnabled";

		// Token: 0x0400158E RID: 5518
		internal const string AppActivationException = "AppActivationException";

		// Token: 0x0400158F RID: 5519
		internal const string ApplicationAlreadyRunning = "ApplicationAlreadyRunning";

		// Token: 0x04001590 RID: 5520
		internal const string ApplicationShuttingDown = "ApplicationShuttingDown";

		// Token: 0x04001591 RID: 5521
		internal const string ArgumentLengthMismatch = "ArgumentLengthMismatch";

		// Token: 0x04001592 RID: 5522
		internal const string ArgumentPropertyMustNotBeNull = "ArgumentPropertyMustNotBeNull";

		// Token: 0x04001593 RID: 5523
		internal const string Argument_InvalidOffLen = "Argument_InvalidOffLen";

		// Token: 0x04001594 RID: 5524
		internal const string ArrangeReentrancyInvalid = "ArrangeReentrancyInvalid";

		// Token: 0x04001595 RID: 5525
		internal const string AssemblyIdNegative = "AssemblyIdNegative";

		// Token: 0x04001596 RID: 5526
		internal const string AssemblyIdOutOfSequence = "AssemblyIdOutOfSequence";

		// Token: 0x04001597 RID: 5527
		internal const string AssemblyTagMissing = "AssemblyTagMissing";

		// Token: 0x04001598 RID: 5528
		internal const string AttachablePropertyNotFound = "AttachablePropertyNotFound";

		// Token: 0x04001599 RID: 5529
		internal const string AudioVideo_CannotControlMedia = "AudioVideo_CannotControlMedia";

		// Token: 0x0400159A RID: 5530
		internal const string AudioVideo_InvalidDependencyObject = "AudioVideo_InvalidDependencyObject";

		// Token: 0x0400159B RID: 5531
		internal const string AudioVideo_InvalidMediaState = "AudioVideo_InvalidMediaState";

		// Token: 0x0400159C RID: 5532
		internal const string AuxiliaryFilterReturnedAnomalousCountOfCharacters = "AuxiliaryFilterReturnedAnomalousCountOfCharacters";

		// Token: 0x0400159D RID: 5533
		internal const string AxNoConnectionPoint = "AxNoConnectionPoint";

		// Token: 0x0400159E RID: 5534
		internal const string AxNoConnectionPointContainer = "AxNoConnectionPointContainer";

		// Token: 0x0400159F RID: 5535
		internal const string AxNoEventInterface = "AxNoEventInterface";

		// Token: 0x040015A0 RID: 5536
		internal const string AXNohWnd = "AXNohWnd";

		// Token: 0x040015A1 RID: 5537
		internal const string AxNoSinkAdvise = "AxNoSinkAdvise";

		// Token: 0x040015A2 RID: 5538
		internal const string AxNoSinkImplementation = "AxNoSinkImplementation";

		// Token: 0x040015A3 RID: 5539
		internal const string AxRequiresApartmentThread = "AxRequiresApartmentThread";

		// Token: 0x040015A4 RID: 5540
		internal const string AxWindowlessControl = "AxWindowlessControl";

		// Token: 0x040015A5 RID: 5541
		internal const string BadDistance = "BadDistance";

		// Token: 0x040015A6 RID: 5542
		internal const string BadFixedTextPosition = "BadFixedTextPosition";

		// Token: 0x040015A7 RID: 5543
		internal const string BadTargetArray = "BadTargetArray";

		// Token: 0x040015A8 RID: 5544
		internal const string BadTextPositionOrder = "BadTextPositionOrder";

		// Token: 0x040015A9 RID: 5545
		internal const string BamlAssemblyIdNotFound = "BamlAssemblyIdNotFound";

		// Token: 0x040015AA RID: 5546
		internal const string BamlBadExtensionValue = "BamlBadExtensionValue";

		// Token: 0x040015AB RID: 5547
		internal const string BamlIsNotSupportedOutsideOfApplicationResources = "BamlIsNotSupportedOutsideOfApplicationResources";

		// Token: 0x040015AC RID: 5548
		internal const string BamlReaderClosed = "BamlReaderClosed";

		// Token: 0x040015AD RID: 5549
		internal const string BamlReaderNoOwnerType = "BamlReaderNoOwnerType";

		// Token: 0x040015AE RID: 5550
		internal const string BamlScopeError = "BamlScopeError";

		// Token: 0x040015AF RID: 5551
		internal const string BamlTypeIdNotFound = "BamlTypeIdNotFound";

		// Token: 0x040015B0 RID: 5552
		internal const string BamlWriterBadAssembly = "BamlWriterBadAssembly";

		// Token: 0x040015B1 RID: 5553
		internal const string BamlWriterBadScope = "BamlWriterBadScope";

		// Token: 0x040015B2 RID: 5554
		internal const string BamlWriterBadStream = "BamlWriterBadStream";

		// Token: 0x040015B3 RID: 5555
		internal const string BamlWriterBadXmlns = "BamlWriterBadXmlns";

		// Token: 0x040015B4 RID: 5556
		internal const string BamlWriterClosed = "BamlWriterClosed";

		// Token: 0x040015B5 RID: 5557
		internal const string BamlWriterNoInElement = "BamlWriterNoInElement";

		// Token: 0x040015B6 RID: 5558
		internal const string BamlWriterStartDoc = "BamlWriterStartDoc";

		// Token: 0x040015B7 RID: 5559
		internal const string BamlWriterUnknownMarkupExtension = "BamlWriterUnknownMarkupExtension";

		// Token: 0x040015B8 RID: 5560
		internal const string BindingCollectionContainsNonBinding = "BindingCollectionContainsNonBinding";

		// Token: 0x040015B9 RID: 5561
		internal const string BindingConflict = "BindingConflict";

		// Token: 0x040015BA RID: 5562
		internal const string BindingExpressionIsDetached = "BindingExpressionIsDetached";

		// Token: 0x040015BB RID: 5563
		internal const string BindingExpressionStatusChanged = "BindingExpressionStatusChanged";

		// Token: 0x040015BC RID: 5564
		internal const string BindingGroup_CannotChangeGroups = "BindingGroup_CannotChangeGroups";

		// Token: 0x040015BD RID: 5565
		internal const string BindingGroup_NoEntry = "BindingGroup_NoEntry";

		// Token: 0x040015BE RID: 5566
		internal const string BindingGroup_ValueUnavailable = "BindingGroup_ValueUnavailable";

		// Token: 0x040015BF RID: 5567
		internal const string BindingListCannotCustomFilter = "BindingListCannotCustomFilter";

		// Token: 0x040015C0 RID: 5568
		internal const string BindingListCanOnlySortByOneProperty = "BindingListCanOnlySortByOneProperty";

		// Token: 0x040015C1 RID: 5569
		internal const string BufferOffsetNegative = "BufferOffsetNegative";

		// Token: 0x040015C2 RID: 5570
		internal const string BufferTooSmall = "BufferTooSmall";

		// Token: 0x040015C3 RID: 5571
		internal const string ByteRangeDownloaderNotInitialized = "ByteRangeDownloaderNotInitialized";

		// Token: 0x040015C4 RID: 5572
		internal const string CalendarAutomationPeer_BlackoutDayHelpText = "CalendarAutomationPeer_BlackoutDayHelpText";

		// Token: 0x040015C5 RID: 5573
		internal const string CalendarAutomationPeer_CalendarButtonLocalizedControlType = "CalendarAutomationPeer_CalendarButtonLocalizedControlType";

		// Token: 0x040015C6 RID: 5574
		internal const string CalendarAutomationPeer_DayButtonLocalizedControlType = "CalendarAutomationPeer_DayButtonLocalizedControlType";

		// Token: 0x040015C7 RID: 5575
		internal const string CalendarAutomationPeer_DecadeMode = "CalendarAutomationPeer_DecadeMode";

		// Token: 0x040015C8 RID: 5576
		internal const string CalendarAutomationPeer_MonthMode = "CalendarAutomationPeer_MonthMode";

		// Token: 0x040015C9 RID: 5577
		internal const string CalendarAutomationPeer_YearMode = "CalendarAutomationPeer_YearMode";

		// Token: 0x040015CA RID: 5578
		internal const string CalendarCollection_MultiThreadedCollectionChangeNotSupported = "CalendarCollection_MultiThreadedCollectionChangeNotSupported";

		// Token: 0x040015CB RID: 5579
		internal const string CalendarNamePropertyValueNotValid = "CalendarNamePropertyValueNotValid";

		// Token: 0x040015CC RID: 5580
		internal const string Calendar_CheckSelectionMode_InvalidOperation = "Calendar_CheckSelectionMode_InvalidOperation";

		// Token: 0x040015CD RID: 5581
		internal const string Calendar_NextButtonName = "Calendar_NextButtonName";

		// Token: 0x040015CE RID: 5582
		internal const string Calendar_OnDisplayModePropertyChanged_InvalidValue = "Calendar_OnDisplayModePropertyChanged_InvalidValue";

		// Token: 0x040015CF RID: 5583
		internal const string Calendar_OnFirstDayOfWeekChanged_InvalidValue = "Calendar_OnFirstDayOfWeekChanged_InvalidValue";

		// Token: 0x040015D0 RID: 5584
		internal const string Calendar_OnSelectedDateChanged_InvalidOperation = "Calendar_OnSelectedDateChanged_InvalidOperation";

		// Token: 0x040015D1 RID: 5585
		internal const string Calendar_OnSelectedDateChanged_InvalidValue = "Calendar_OnSelectedDateChanged_InvalidValue";

		// Token: 0x040015D2 RID: 5586
		internal const string Calendar_OnSelectionModeChanged_InvalidValue = "Calendar_OnSelectionModeChanged_InvalidValue";

		// Token: 0x040015D3 RID: 5587
		internal const string Calendar_PreviousButtonName = "Calendar_PreviousButtonName";

		// Token: 0x040015D4 RID: 5588
		internal const string Calendar_UnSelectableDates = "Calendar_UnSelectableDates";

		// Token: 0x040015D5 RID: 5589
		internal const string CancelEditNotSupported = "CancelEditNotSupported";

		// Token: 0x040015D6 RID: 5590
		internal const string CancelledText = "CancelledText";

		// Token: 0x040015D7 RID: 5591
		internal const string CancelledTitle = "CancelledTitle";

		// Token: 0x040015D8 RID: 5592
		internal const string CannotBeInsidePopup = "CannotBeInsidePopup";

		// Token: 0x040015D9 RID: 5593
		internal const string CannotBeSelfParent = "CannotBeSelfParent";

		// Token: 0x040015DA RID: 5594
		internal const string CannotCallRunFromBrowserHostedApp = "CannotCallRunFromBrowserHostedApp";

		// Token: 0x040015DB RID: 5595
		internal const string CannotCallRunMultipleTimes = "CannotCallRunMultipleTimes";

		// Token: 0x040015DC RID: 5596
		internal const string CannotChangeAfterSealed = "CannotChangeAfterSealed";

		// Token: 0x040015DD RID: 5597
		internal const string CannotChangeLiveShaping = "CannotChangeLiveShaping";

		// Token: 0x040015DE RID: 5598
		internal const string CannotChangeMainWindowInBrowser = "CannotChangeMainWindowInBrowser";

		// Token: 0x040015DF RID: 5599
		internal const string CannotDetermineSortByPropertiesForCollection = "CannotDetermineSortByPropertiesForCollection";

		// Token: 0x040015E0 RID: 5600
		internal const string CannotEditPlaceholder = "CannotEditPlaceholder";

		// Token: 0x040015E1 RID: 5601
		internal const string CannotFilterView = "CannotFilterView";

		// Token: 0x040015E2 RID: 5602
		internal const string CannotFindRemovedItem = "CannotFindRemovedItem";

		// Token: 0x040015E3 RID: 5603
		internal const string CannotGroupView = "CannotGroupView";

		// Token: 0x040015E4 RID: 5604
		internal const string CannotHaveEventHandlersInThemeStyle = "CannotHaveEventHandlersInThemeStyle";

		// Token: 0x040015E5 RID: 5605
		internal const string CannotHaveOverridesDefaultStyleInThemeStyle = "CannotHaveOverridesDefaultStyleInThemeStyle";

		// Token: 0x040015E6 RID: 5606
		internal const string CannotHavePropertyInStyle = "CannotHavePropertyInStyle";

		// Token: 0x040015E7 RID: 5607
		internal const string CannotHavePropertyInTemplate = "CannotHavePropertyInTemplate";

		// Token: 0x040015E8 RID: 5608
		internal const string CannotHookupFCERoot = "CannotHookupFCERoot";

		// Token: 0x040015E9 RID: 5609
		internal const string CannotInvokeScript = "CannotInvokeScript";

		// Token: 0x040015EA RID: 5610
		internal const string CannotModifyLogicalChildrenDuringTreeWalk = "CannotModifyLogicalChildrenDuringTreeWalk";

		// Token: 0x040015EB RID: 5611
		internal const string CannotMoveToUnknownPosition = "CannotMoveToUnknownPosition";

		// Token: 0x040015EC RID: 5612
		internal const string CannotParseId = "CannotParseId";

		// Token: 0x040015ED RID: 5613
		internal const string CannotProcessInkCommand = "CannotProcessInkCommand";

		// Token: 0x040015EE RID: 5614
		internal const string CannotQueryPropertiesWhenPageNotInTreeWithWindow = "CannotQueryPropertiesWhenPageNotInTreeWithWindow";

		// Token: 0x040015EF RID: 5615
		internal const string CannotRecyleHeterogeneousTypes = "CannotRecyleHeterogeneousTypes";

		// Token: 0x040015F0 RID: 5616
		internal const string CannotRemoveUnrealizedItems = "CannotRemoveUnrealizedItems";

		// Token: 0x040015F1 RID: 5617
		internal const string CannotSelectNotSelectableItem = "CannotSelectNotSelectableItem";

		// Token: 0x040015F2 RID: 5618
		internal const string CannotSerializeInvalidInstance = "CannotSerializeInvalidInstance";

		// Token: 0x040015F3 RID: 5619
		internal const string CannotSetNegativePosition = "CannotSetNegativePosition";

		// Token: 0x040015F4 RID: 5620
		internal const string CannotSetOwnerToItself = "CannotSetOwnerToItself";

		// Token: 0x040015F5 RID: 5621
		internal const string CannotSortView = "CannotSortView";

		// Token: 0x040015F6 RID: 5622
		internal const string CannotUseItemsSource = "CannotUseItemsSource";

		// Token: 0x040015F7 RID: 5623
		internal const string CannotWriteToReadOnly = "CannotWriteToReadOnly";

		// Token: 0x040015F8 RID: 5624
		internal const string CanOnlyHaveOneChild = "CanOnlyHaveOneChild";

		// Token: 0x040015F9 RID: 5625
		internal const string CantSetInMarkup = "CantSetInMarkup";

		// Token: 0x040015FA RID: 5626
		internal const string CantSetOwnerAfterDialogIsShown = "CantSetOwnerAfterDialogIsShown";

		// Token: 0x040015FB RID: 5627
		internal const string CantSetOwnerToClosedWindow = "CantSetOwnerToClosedWindow";

		// Token: 0x040015FC RID: 5628
		internal const string CantSetOwnerWhosHwndIsNotCreated = "CantSetOwnerWhosHwndIsNotCreated";

		// Token: 0x040015FD RID: 5629
		internal const string CantShowMBServiceWithOwner = "CantShowMBServiceWithOwner";

		// Token: 0x040015FE RID: 5630
		internal const string CantShowModalOnNonInteractive = "CantShowModalOnNonInteractive";

		// Token: 0x040015FF RID: 5631
		internal const string CantShowOnDifferentThread = "CantShowOnDifferentThread";

		// Token: 0x04001600 RID: 5632
		internal const string CantSwitchVirtualizationModePostMeasure = "CantSwitchVirtualizationModePostMeasure";

		// Token: 0x04001601 RID: 5633
		internal const string ChangeNotAllowedAfterShow = "ChangeNotAllowedAfterShow";

		// Token: 0x04001602 RID: 5634
		internal const string ChangeSealedBinding = "ChangeSealedBinding";

		// Token: 0x04001603 RID: 5635
		internal const string ChangingCollectionNotSupported = "ChangingCollectionNotSupported";

		// Token: 0x04001604 RID: 5636
		internal const string ChangingIdNotAllowed = "ChangingIdNotAllowed";

		// Token: 0x04001605 RID: 5637
		internal const string ChangingTypeNotAllowed = "ChangingTypeNotAllowed";

		// Token: 0x04001606 RID: 5638
		internal const string ChildHasWrongType = "ChildHasWrongType";

		// Token: 0x04001607 RID: 5639
		internal const string ChildNameMustBeNonEmpty = "ChildNameMustBeNonEmpty";

		// Token: 0x04001608 RID: 5640
		internal const string ChildNameNamePatternReserved = "ChildNameNamePatternReserved";

		// Token: 0x04001609 RID: 5641
		internal const string ChildTemplateInstanceDoesNotExist = "ChildTemplateInstanceDoesNotExist";

		// Token: 0x0400160A RID: 5642
		internal const string ChildWindowMustHaveCorrectParent = "ChildWindowMustHaveCorrectParent";

		// Token: 0x0400160B RID: 5643
		internal const string ChildWindowNotCreated = "ChildWindowNotCreated";

		// Token: 0x0400160C RID: 5644
		internal const string CircularOwnerChild = "CircularOwnerChild";

		// Token: 0x0400160D RID: 5645
		internal const string ClearHighlight = "ClearHighlight";

		// Token: 0x0400160E RID: 5646
		internal const string ClipboardCopyMode_Disabled = "ClipboardCopyMode_Disabled";

		// Token: 0x0400160F RID: 5647
		internal const string ClipToBoundsNotSupported = "ClipToBoundsNotSupported";

		// Token: 0x04001610 RID: 5648
		internal const string CollectionAddEventMissingItem = "CollectionAddEventMissingItem";

		// Token: 0x04001611 RID: 5649
		internal const string CollectionChangeIndexOutOfRange = "CollectionChangeIndexOutOfRange";

		// Token: 0x04001612 RID: 5650
		internal const string CollectionContainerMustBeUniqueForComposite = "CollectionContainerMustBeUniqueForComposite";

		// Token: 0x04001613 RID: 5651
		internal const string CollectionViewTypeIsInitOnly = "CollectionViewTypeIsInitOnly";

		// Token: 0x04001614 RID: 5652
		internal const string CollectionView_MissingSynchronizationCallback = "CollectionView_MissingSynchronizationCallback";

		// Token: 0x04001615 RID: 5653
		internal const string CollectionView_NameTypeDuplicity = "CollectionView_NameTypeDuplicity";

		// Token: 0x04001616 RID: 5654
		internal const string CollectionView_ViewTypeInsufficient = "CollectionView_ViewTypeInsufficient";

		// Token: 0x04001617 RID: 5655
		internal const string CollectionView_WrongType = "CollectionView_WrongType";

		// Token: 0x04001618 RID: 5656
		internal const string Collection_NoNull = "Collection_NoNull";

		// Token: 0x04001619 RID: 5657
		internal const string ColorConvertedBitmapExtensionNoSourceImage = "ColorConvertedBitmapExtensionNoSourceImage";

		// Token: 0x0400161A RID: 5658
		internal const string ColorConvertedBitmapExtensionNoSourceProfile = "ColorConvertedBitmapExtensionNoSourceProfile";

		// Token: 0x0400161B RID: 5659
		internal const string ColorConvertedBitmapExtensionSyntax = "ColorConvertedBitmapExtensionSyntax";

		// Token: 0x0400161C RID: 5660
		internal const string CompatibilityPreferencesSealed = "CompatibilityPreferencesSealed";

		// Token: 0x0400161D RID: 5661
		internal const string ComponentAlreadyInPresentationContext = "ComponentAlreadyInPresentationContext";

		// Token: 0x0400161E RID: 5662
		internal const string ComponentNotInPresentationContext = "ComponentNotInPresentationContext";

		// Token: 0x0400161F RID: 5663
		internal const string CompositeCollectionResetOnlyOnClear = "CompositeCollectionResetOnlyOnClear";

		// Token: 0x04001620 RID: 5664
		internal const string ConditionCannotUseBothPropertyAndBinding = "ConditionCannotUseBothPropertyAndBinding";

		// Token: 0x04001621 RID: 5665
		internal const string ConditionValueOfExpressionNotSupported = "ConditionValueOfExpressionNotSupported";

		// Token: 0x04001622 RID: 5666
		internal const string ConditionValueOfMarkupExtensionNotSupported = "ConditionValueOfMarkupExtensionNotSupported";

		// Token: 0x04001623 RID: 5667
		internal const string ContentControlCannotHaveMultipleContent = "ContentControlCannotHaveMultipleContent";

		// Token: 0x04001624 RID: 5668
		internal const string ContentTypeNotSupported = "ContentTypeNotSupported";

		// Token: 0x04001625 RID: 5669
		internal const string ContextMenuInDifferentDispatcher = "ContextMenuInDifferentDispatcher";

		// Token: 0x04001626 RID: 5670
		internal const string CopyToNotEnoughSpace = "CopyToNotEnoughSpace";

		// Token: 0x04001627 RID: 5671
		internal const string CorePropertyEnumeratorPositionedOutOfBounds = "CorePropertyEnumeratorPositionedOutOfBounds";

		// Token: 0x04001628 RID: 5672
		internal const string CreateHighlight = "CreateHighlight";

		// Token: 0x04001629 RID: 5673
		internal const string CreateInkNote = "CreateInkNote";

		// Token: 0x0400162A RID: 5674
		internal const string CreateRootPopup_ChildHasLogicalParent = "CreateRootPopup_ChildHasLogicalParent";

		// Token: 0x0400162B RID: 5675
		internal const string CreateRootPopup_ChildHasVisualParent = "CreateRootPopup_ChildHasVisualParent";

		// Token: 0x0400162C RID: 5676
		internal const string CreateTextNote = "CreateTextNote";

		// Token: 0x0400162D RID: 5677
		internal const string CrossThreadAccessOfUnshareableFreezable = "CrossThreadAccessOfUnshareableFreezable";

		// Token: 0x0400162E RID: 5678
		internal const string CustomContentStateMustBeSerializable = "CustomContentStateMustBeSerializable";

		// Token: 0x0400162F RID: 5679
		internal const string CustomDictionaryFailedToLoadDictionaryUri = "CustomDictionaryFailedToLoadDictionaryUri";

		// Token: 0x04001630 RID: 5680
		internal const string CustomDictionaryFailedToUnLoadDictionaryUri = "CustomDictionaryFailedToUnLoadDictionaryUri";

		// Token: 0x04001631 RID: 5681
		internal const string CustomDictionaryItemAlreadyExists = "CustomDictionaryItemAlreadyExists";

		// Token: 0x04001632 RID: 5682
		internal const string CustomDictionaryNullItem = "CustomDictionaryNullItem";

		// Token: 0x04001633 RID: 5683
		internal const string CustomDictionarySourcesUnsupportedURI = "CustomDictionarySourcesUnsupportedURI";

		// Token: 0x04001634 RID: 5684
		internal const string CyclicStyleReferenceDetected = "CyclicStyleReferenceDetected";

		// Token: 0x04001635 RID: 5685
		internal const string CyclicThemeStyleReferenceDetected = "CyclicThemeStyleReferenceDetected";

		// Token: 0x04001636 RID: 5686
		internal const string DataGridCellItemAutomationPeer_LocalizedControlType = "DataGridCellItemAutomationPeer_LocalizedControlType";

		// Token: 0x04001637 RID: 5687
		internal const string DataGridCellItemAutomationPeer_NameCoreFormat = "DataGridCellItemAutomationPeer_NameCoreFormat";

		// Token: 0x04001638 RID: 5688
		internal const string DataGridColumnHeaderItemAutomationPeer_NameCoreFormat = "DataGridColumnHeaderItemAutomationPeer_NameCoreFormat";

		// Token: 0x04001639 RID: 5689
		internal const string DataGridColumnHeaderItemAutomationPeer_Unresizable = "DataGridColumnHeaderItemAutomationPeer_Unresizable";

		// Token: 0x0400163A RID: 5690
		internal const string DataGridColumnHeaderItemAutomationPeer_Unsupported = "DataGridColumnHeaderItemAutomationPeer_Unsupported";

		// Token: 0x0400163B RID: 5691
		internal const string DataGridLength_Infinity = "DataGridLength_Infinity";

		// Token: 0x0400163C RID: 5692
		internal const string DataGridLength_InvalidType = "DataGridLength_InvalidType";

		// Token: 0x0400163D RID: 5693
		internal const string DataGridRow_CannotSelectRowWhenCells = "DataGridRow_CannotSelectRowWhenCells";

		// Token: 0x0400163E RID: 5694
		internal const string DataGrid_AutomationInvokeFailed = "DataGrid_AutomationInvokeFailed";

		// Token: 0x0400163F RID: 5695
		internal const string DataGrid_CannotSelectCell = "DataGrid_CannotSelectCell";

		// Token: 0x04001640 RID: 5696
		internal const string DataGrid_ColumnDisplayIndexOutOfRange = "DataGrid_ColumnDisplayIndexOutOfRange";

		// Token: 0x04001641 RID: 5697
		internal const string DataGrid_ColumnIndexOutOfRange = "DataGrid_ColumnIndexOutOfRange";

		// Token: 0x04001642 RID: 5698
		internal const string DataGrid_ColumnIsReadOnly = "DataGrid_ColumnIsReadOnly";

		// Token: 0x04001643 RID: 5699
		internal const string DataGrid_DisplayIndexOutOfRange = "DataGrid_DisplayIndexOutOfRange";

		// Token: 0x04001644 RID: 5700
		internal const string DataGrid_DuplicateDisplayIndex = "DataGrid_DuplicateDisplayIndex";

		// Token: 0x04001645 RID: 5701
		internal const string DataGrid_InvalidColumnReuse = "DataGrid_InvalidColumnReuse";

		// Token: 0x04001646 RID: 5702
		internal const string DataGrid_InvalidSortDescription = "DataGrid_InvalidSortDescription";

		// Token: 0x04001647 RID: 5703
		internal const string DataGrid_NewColumnInvalidDisplayIndex = "DataGrid_NewColumnInvalidDisplayIndex";

		// Token: 0x04001648 RID: 5704
		internal const string DataGrid_NullColumn = "DataGrid_NullColumn";

		// Token: 0x04001649 RID: 5705
		internal const string DataGrid_ProbableInvalidSortDescription = "DataGrid_ProbableInvalidSortDescription";

		// Token: 0x0400164A RID: 5706
		internal const string DataGrid_ReadonlyCellsItemsSource = "DataGrid_ReadonlyCellsItemsSource";

		// Token: 0x0400164B RID: 5707
		internal const string DataTypeCannotBeObject = "DataTypeCannotBeObject";

		// Token: 0x0400164C RID: 5708
		internal const string DatePickerAutomationPeer_LocalizedControlType = "DatePickerAutomationPeer_LocalizedControlType";

		// Token: 0x0400164D RID: 5709
		internal const string DatePickerTextBox_DefaultWatermarkText = "DatePickerTextBox_DefaultWatermarkText";

		// Token: 0x0400164E RID: 5710
		internal const string DatePickerTextBox_TemplatePartIsOfIncorrectType = "DatePickerTextBox_TemplatePartIsOfIncorrectType";

		// Token: 0x0400164F RID: 5711
		internal const string DatePicker_DropDownButtonName = "DatePicker_DropDownButtonName";

		// Token: 0x04001650 RID: 5712
		internal const string DatePicker_OnSelectedDateFormatChanged_InvalidValue = "DatePicker_OnSelectedDateFormatChanged_InvalidValue";

		// Token: 0x04001651 RID: 5713
		internal const string DatePicker_WatermarkText = "DatePicker_WatermarkText";

		// Token: 0x04001652 RID: 5714
		internal const string DeferringLoaderNoContext = "DeferringLoaderNoContext";

		// Token: 0x04001653 RID: 5715
		internal const string DeferringLoaderNoSave = "DeferringLoaderNoSave";

		// Token: 0x04001654 RID: 5716
		internal const string DeferSelectionActive = "DeferSelectionActive";

		// Token: 0x04001655 RID: 5717
		internal const string DeferSelectionNotActive = "DeferSelectionNotActive";

		// Token: 0x04001656 RID: 5718
		internal const string DeleteAnnotations = "DeleteAnnotations";

		// Token: 0x04001657 RID: 5719
		internal const string DeleteNotes = "DeleteNotes";

		// Token: 0x04001658 RID: 5720
		internal const string DeployText = "DeployText";

		// Token: 0x04001659 RID: 5721
		internal const string DeployTitle = "DeployTitle";

		// Token: 0x0400165A RID: 5722
		internal const string DesignerMetadata_CustomCategory_Accessibility = "DesignerMetadata_CustomCategory_Accessibility";

		// Token: 0x0400165B RID: 5723
		internal const string DesignerMetadata_CustomCategory_Content = "DesignerMetadata_CustomCategory_Content";

		// Token: 0x0400165C RID: 5724
		internal const string DesignerMetadata_CustomCategory_Navigation = "DesignerMetadata_CustomCategory_Navigation";

		// Token: 0x0400165D RID: 5725
		internal const string DialogResultMustBeSetAfterShowDialog = "DialogResultMustBeSetAfterShowDialog";

		// Token: 0x0400165E RID: 5726
		internal const string DisplayMemberPathAndItemTemplateDefined = "DisplayMemberPathAndItemTemplateDefined";

		// Token: 0x0400165F RID: 5727
		internal const string DisplayMemberPathAndItemTemplateSelectorDefined = "DisplayMemberPathAndItemTemplateSelectorDefined";

		// Token: 0x04001660 RID: 5728
		internal const string DocumentApplicationCannotInitializeUI = "DocumentApplicationCannotInitializeUI";

		// Token: 0x04001661 RID: 5729
		internal const string DocumentApplicationContextMenuFirstPageInputGesture = "DocumentApplicationContextMenuFirstPageInputGesture";

		// Token: 0x04001662 RID: 5730
		internal const string DocumentApplicationContextMenuLastPageInputGesture = "DocumentApplicationContextMenuLastPageInputGesture";

		// Token: 0x04001663 RID: 5731
		internal const string DocumentApplicationContextMenuNextPageHeader = "DocumentApplicationContextMenuNextPageHeader";

		// Token: 0x04001664 RID: 5732
		internal const string DocumentApplicationContextMenuNextPageInputGesture = "DocumentApplicationContextMenuNextPageInputGesture";

		// Token: 0x04001665 RID: 5733
		internal const string DocumentApplicationContextMenuPreviousPageHeader = "DocumentApplicationContextMenuPreviousPageHeader";

		// Token: 0x04001666 RID: 5734
		internal const string DocumentApplicationContextMenuPreviousPageInputGesture = "DocumentApplicationContextMenuPreviousPageInputGesture";

		// Token: 0x04001667 RID: 5735
		internal const string DocumentApplicationNotInFullTrust = "DocumentApplicationNotInFullTrust";

		// Token: 0x04001668 RID: 5736
		internal const string DocumentApplicationRegistryKeyNotFound = "DocumentApplicationRegistryKeyNotFound";

		// Token: 0x04001669 RID: 5737
		internal const string DocumentApplicationStatusLoaded = "DocumentApplicationStatusLoaded";

		// Token: 0x0400166A RID: 5738
		internal const string DocumentApplicationUnableToOpenDocument = "DocumentApplicationUnableToOpenDocument";

		// Token: 0x0400166B RID: 5739
		internal const string DocumentApplicationUnknownFileFormat = "DocumentApplicationUnknownFileFormat";

		// Token: 0x0400166C RID: 5740
		internal const string DocumentGridInvalidViewMode = "DocumentGridInvalidViewMode";

		// Token: 0x0400166D RID: 5741
		internal const string DocumentGridVisualTreeContainsNonBorderAsFirstElement = "DocumentGridVisualTreeContainsNonBorderAsFirstElement";

		// Token: 0x0400166E RID: 5742
		internal const string DocumentGridVisualTreeContainsNonDocumentGridPage = "DocumentGridVisualTreeContainsNonDocumentGridPage";

		// Token: 0x0400166F RID: 5743
		internal const string DocumentGridVisualTreeContainsNonUIElement = "DocumentGridVisualTreeContainsNonUIElement";

		// Token: 0x04001670 RID: 5744
		internal const string DocumentGridVisualTreeOutOfSync = "DocumentGridVisualTreeOutOfSync";

		// Token: 0x04001671 RID: 5745
		internal const string DocumentPageView_ParentNotDocumentPageHost = "DocumentPageView_ParentNotDocumentPageHost";

		// Token: 0x04001672 RID: 5746
		internal const string DocumentReadOnly = "DocumentReadOnly";

		// Token: 0x04001673 RID: 5747
		internal const string DocumentReferenceHasInvalidDocument = "DocumentReferenceHasInvalidDocument";

		// Token: 0x04001674 RID: 5748
		internal const string DocumentReferenceNotFound = "DocumentReferenceNotFound";

		// Token: 0x04001675 RID: 5749
		internal const string DocumentReferenceUnsupportedMimeType = "DocumentReferenceUnsupportedMimeType";

		// Token: 0x04001676 RID: 5750
		internal const string DocumentStructureUnexpectedParameterType1 = "DocumentStructureUnexpectedParameterType1";

		// Token: 0x04001677 RID: 5751
		internal const string DocumentStructureUnexpectedParameterType2 = "DocumentStructureUnexpectedParameterType2";

		// Token: 0x04001678 RID: 5752
		internal const string DocumentStructureUnexpectedParameterType4 = "DocumentStructureUnexpectedParameterType4";

		// Token: 0x04001679 RID: 5753
		internal const string DocumentStructureUnexpectedParameterType6 = "DocumentStructureUnexpectedParameterType6";

		// Token: 0x0400167A RID: 5754
		internal const string DocumentViewerArgumentMustBeInteger = "DocumentViewerArgumentMustBeInteger";

		// Token: 0x0400167B RID: 5755
		internal const string DocumentViewerArgumentMustBePercentage = "DocumentViewerArgumentMustBePercentage";

		// Token: 0x0400167C RID: 5756
		internal const string DocumentViewerCanHaveOnlyOneChild = "DocumentViewerCanHaveOnlyOneChild";

		// Token: 0x0400167D RID: 5757
		internal const string DocumentViewerChildMustImplementIDocumentPaginatorSource = "DocumentViewerChildMustImplementIDocumentPaginatorSource";

		// Token: 0x0400167E RID: 5758
		internal const string DocumentViewerOneMasterPage = "DocumentViewerOneMasterPage";

		// Token: 0x0400167F RID: 5759
		internal const string DocumentViewerOnlySupportsFixedDocumentSequence = "DocumentViewerOnlySupportsFixedDocumentSequence";

		// Token: 0x04001680 RID: 5760
		internal const string DocumentViewerPageViewsCollectionEmpty = "DocumentViewerPageViewsCollectionEmpty";

		// Token: 0x04001681 RID: 5761
		internal const string DocumentViewerSearchCompleteTitle = "DocumentViewerSearchCompleteTitle";

		// Token: 0x04001682 RID: 5762
		internal const string DocumentViewerSearchDownCompleteLabel = "DocumentViewerSearchDownCompleteLabel";

		// Token: 0x04001683 RID: 5763
		internal const string DocumentViewerSearchUpCompleteLabel = "DocumentViewerSearchUpCompleteLabel";

		// Token: 0x04001684 RID: 5764
		internal const string DocumentViewerStyleMustIncludeContentHost = "DocumentViewerStyleMustIncludeContentHost";

		// Token: 0x04001685 RID: 5765
		internal const string DocumentViewerViewFitToHeightCommandText = "DocumentViewerViewFitToHeightCommandText";

		// Token: 0x04001686 RID: 5766
		internal const string DocumentViewerViewFitToMaxPagesAcrossCommandText = "DocumentViewerViewFitToMaxPagesAcrossCommandText";

		// Token: 0x04001687 RID: 5767
		internal const string DocumentViewerViewFitToWidthCommandText = "DocumentViewerViewFitToWidthCommandText";

		// Token: 0x04001688 RID: 5768
		internal const string DocumentViewerViewThumbnailsCommandText = "DocumentViewerViewThumbnailsCommandText";

		// Token: 0x04001689 RID: 5769
		internal const string DownloadText = "DownloadText";

		// Token: 0x0400168A RID: 5770
		internal const string DownloadTitle = "DownloadTitle";

		// Token: 0x0400168B RID: 5771
		internal const string DragMoveFail = "DragMoveFail";

		// Token: 0x0400168C RID: 5772
		internal const string DuplicatedCompatibleUri = "DuplicatedCompatibleUri";

		// Token: 0x0400168D RID: 5773
		internal const string DuplicatedUri = "DuplicatedUri";

		// Token: 0x0400168E RID: 5774
		internal const string DuplicatesNotAllowed = "DuplicatesNotAllowed";

		// Token: 0x0400168F RID: 5775
		internal const string ElementMustBeInPopup = "ElementMustBeInPopup";

		// Token: 0x04001690 RID: 5776
		internal const string ElementMustBelongToTemplate = "ElementMustBelongToTemplate";

		// Token: 0x04001691 RID: 5777
		internal const string EmptySelectionNotSupported = "EmptySelectionNotSupported";

		// Token: 0x04001692 RID: 5778
		internal const string EndInitWithoutBeginInitNotSupported = "EndInitWithoutBeginInitNotSupported";

		// Token: 0x04001693 RID: 5779
		internal const string EntryAssemblyIsNull = "EntryAssemblyIsNull";

		// Token: 0x04001694 RID: 5780
		internal const string EnumeratorCollectionDisposed = "EnumeratorCollectionDisposed";

		// Token: 0x04001695 RID: 5781
		internal const string EnumeratorInvalidOperation = "EnumeratorInvalidOperation";

		// Token: 0x04001696 RID: 5782
		internal const string EnumeratorNotStarted = "EnumeratorNotStarted";

		// Token: 0x04001697 RID: 5783
		internal const string EnumeratorReachedEnd = "EnumeratorReachedEnd";

		// Token: 0x04001698 RID: 5784
		internal const string EnumeratorVersionChanged = "EnumeratorVersionChanged";

		// Token: 0x04001699 RID: 5785
		internal const string EventTriggerBadAction = "EventTriggerBadAction";

		// Token: 0x0400169A RID: 5786
		internal const string EventTriggerDoesNotEnterExit = "EventTriggerDoesNotEnterExit";

		// Token: 0x0400169B RID: 5787
		internal const string EventTriggerDoNotSetProperties = "EventTriggerDoNotSetProperties";

		// Token: 0x0400169C RID: 5788
		internal const string EventTriggerEventUnresolvable = "EventTriggerEventUnresolvable";

		// Token: 0x0400169D RID: 5789
		internal const string EventTriggerNeedEvent = "EventTriggerNeedEvent";

		// Token: 0x0400169E RID: 5790
		internal const string EventTriggerOnStyleNotAllowedToHaveTarget = "EventTriggerOnStyleNotAllowedToHaveTarget";

		// Token: 0x0400169F RID: 5791
		internal const string EventTriggerTargetNameUnresolvable = "EventTriggerTargetNameUnresolvable";

		// Token: 0x040016A0 RID: 5792
		internal const string ExceptionInGetPage = "ExceptionInGetPage";

		// Token: 0x040016A1 RID: 5793
		internal const string ExceptionValidationRuleValidateNotSupported = "ExceptionValidationRuleValidateNotSupported";

		// Token: 0x040016A2 RID: 5794
		internal const string ExpectedBamlSchemaContext = "ExpectedBamlSchemaContext";

		// Token: 0x040016A3 RID: 5795
		internal const string ExpectedBinaryContent = "ExpectedBinaryContent";

		// Token: 0x040016A4 RID: 5796
		internal const string ExpectedResourceDictionaryTarget = "ExpectedResourceDictionaryTarget";

		// Token: 0x040016A5 RID: 5797
		internal const string FailedResumePageFunction = "FailedResumePageFunction";

		// Token: 0x040016A6 RID: 5798
		internal const string FailedToConvertResource = "FailedToConvertResource";

		// Token: 0x040016A7 RID: 5799
		internal const string FailToLaunchDefaultBrowser = "FailToLaunchDefaultBrowser";

		// Token: 0x040016A8 RID: 5800
		internal const string FailToNavigateUsingHyperlinkTarget = "FailToNavigateUsingHyperlinkTarget";

		// Token: 0x040016A9 RID: 5801
		internal const string FileDialogBufferTooSmall = "FileDialogBufferTooSmall";

		// Token: 0x040016AA RID: 5802
		internal const string FileDialogCreatePrompt = "FileDialogCreatePrompt";

		// Token: 0x040016AB RID: 5803
		internal const string FileDialogFileNotFound = "FileDialogFileNotFound";

		// Token: 0x040016AC RID: 5804
		internal const string FileDialogInvalidFileName = "FileDialogInvalidFileName";

		// Token: 0x040016AD RID: 5805
		internal const string FileDialogInvalidFilter = "FileDialogInvalidFilter";

		// Token: 0x040016AE RID: 5806
		internal const string FileDialogInvalidFilterIndex = "FileDialogInvalidFilterIndex";

		// Token: 0x040016AF RID: 5807
		internal const string FileDialogOverwritePrompt = "FileDialogOverwritePrompt";

		// Token: 0x040016B0 RID: 5808
		internal const string FileDialogSubClassFailure = "FileDialogSubClassFailure";

		// Token: 0x040016B1 RID: 5809
		internal const string FileNameMustNotBeNull = "FileNameMustNotBeNull";

		// Token: 0x040016B2 RID: 5810
		internal const string FileNameNullOrEmpty = "FileNameNullOrEmpty";

		// Token: 0x040016B3 RID: 5811
		internal const string FileToFilterNotLoaded = "FileToFilterNotLoaded";

		// Token: 0x040016B4 RID: 5812
		internal const string FilterBindRegionNotImplemented = "FilterBindRegionNotImplemented";

		// Token: 0x040016B5 RID: 5813
		internal const string FilterEndOfChunks = "FilterEndOfChunks";

		// Token: 0x040016B6 RID: 5814
		internal const string FilterGetChunkNoStream = "FilterGetChunkNoStream";

		// Token: 0x040016B7 RID: 5815
		internal const string FilterGetTextBufferOverflow = "FilterGetTextBufferOverflow";

		// Token: 0x040016B8 RID: 5816
		internal const string FilterGetTextNotSupported = "FilterGetTextNotSupported";

		// Token: 0x040016B9 RID: 5817
		internal const string FilterGetValueAlreadyCalledOnCurrentChunk = "FilterGetValueAlreadyCalledOnCurrentChunk";

		// Token: 0x040016BA RID: 5818
		internal const string FilterGetValueMustBeStringOrDateTime = "FilterGetValueMustBeStringOrDateTime";

		// Token: 0x040016BB RID: 5819
		internal const string FilterGetValueNotSupported = "FilterGetValueNotSupported";

		// Token: 0x040016BC RID: 5820
		internal const string FilterInitInvalidAttributes = "FilterInitInvalidAttributes";

		// Token: 0x040016BD RID: 5821
		internal const string FilterIPersistFileIsReadOnly = "FilterIPersistFileIsReadOnly";

		// Token: 0x040016BE RID: 5822
		internal const string FilterIPersistStreamIsReadOnly = "FilterIPersistStreamIsReadOnly";

		// Token: 0x040016BF RID: 5823
		internal const string FilterLoadInvalidModeFlag = "FilterLoadInvalidModeFlag";

		// Token: 0x040016C0 RID: 5824
		internal const string FilterNullGetTextBufferPointer = "FilterNullGetTextBufferPointer";

		// Token: 0x040016C1 RID: 5825
		internal const string FilterPropSpecUnknownUnionSelector = "FilterPropSpecUnknownUnionSelector";

		// Token: 0x040016C2 RID: 5826
		internal const string FixedDocumentExpectsDependencyObject = "FixedDocumentExpectsDependencyObject";

		// Token: 0x040016C3 RID: 5827
		internal const string FixedDocumentReadonly = "FixedDocumentReadonly";

		// Token: 0x040016C4 RID: 5828
		internal const string FlowDocumentFormattingReentrancy = "FlowDocumentFormattingReentrancy";

		// Token: 0x040016C5 RID: 5829
		internal const string FlowDocumentInvalidContnetChange = "FlowDocumentInvalidContnetChange";

		// Token: 0x040016C6 RID: 5830
		internal const string FlowDocumentPageViewerOnlySupportsFlowDocument = "FlowDocumentPageViewerOnlySupportsFlowDocument";

		// Token: 0x040016C7 RID: 5831
		internal const string FlowDocumentReaderCanHaveOnlyOneChild = "FlowDocumentReaderCanHaveOnlyOneChild";

		// Token: 0x040016C8 RID: 5832
		internal const string FlowDocumentReaderCannotDisableAllViewingModes = "FlowDocumentReaderCannotDisableAllViewingModes";

		// Token: 0x040016C9 RID: 5833
		internal const string FlowDocumentReaderDecoratorMarkedAsContentHostMustHaveNoContent = "FlowDocumentReaderDecoratorMarkedAsContentHostMustHaveNoContent";

		// Token: 0x040016CA RID: 5834
		internal const string FlowDocumentReaderViewingModeEnabledConflict = "FlowDocumentReaderViewingModeEnabledConflict";

		// Token: 0x040016CB RID: 5835
		internal const string FlowDocumentReader_MultipleViewProvider_PageViewName = "FlowDocumentReader_MultipleViewProvider_PageViewName";

		// Token: 0x040016CC RID: 5836
		internal const string FlowDocumentReader_MultipleViewProvider_ScrollViewName = "FlowDocumentReader_MultipleViewProvider_ScrollViewName";

		// Token: 0x040016CD RID: 5837
		internal const string FlowDocumentReader_MultipleViewProvider_TwoPageViewName = "FlowDocumentReader_MultipleViewProvider_TwoPageViewName";

		// Token: 0x040016CE RID: 5838
		internal const string FlowDocumentScrollViewerCanHaveOnlyOneChild = "FlowDocumentScrollViewerCanHaveOnlyOneChild";

		// Token: 0x040016CF RID: 5839
		internal const string FlowDocumentScrollViewerDocumentBelongsToAnotherFlowDocumentScrollViewerAlready = "FlowDocumentScrollViewerDocumentBelongsToAnotherFlowDocumentScrollViewerAlready";

		// Token: 0x040016D0 RID: 5840
		internal const string FlowDocumentScrollViewerMarkedAsContentHostMustHaveNoContent = "FlowDocumentScrollViewerMarkedAsContentHostMustHaveNoContent";

		// Token: 0x040016D1 RID: 5841
		internal const string FormatRestrictionsExceeded = "FormatRestrictionsExceeded";

		// Token: 0x040016D2 RID: 5842
		internal const string FrameNoAddChild = "FrameNoAddChild";

		// Token: 0x040016D3 RID: 5843
		internal const string FrameworkElementFactoryAlreadyParented = "FrameworkElementFactoryAlreadyParented";

		// Token: 0x040016D4 RID: 5844
		internal const string FrameworkElementFactoryCannotAddText = "FrameworkElementFactoryCannotAddText";

		// Token: 0x040016D5 RID: 5845
		internal const string FrameworkElementFactoryMustBeSealed = "FrameworkElementFactoryMustBeSealed";

		// Token: 0x040016D6 RID: 5846
		internal const string GenerationInProgress = "GenerationInProgress";

		// Token: 0x040016D7 RID: 5847
		internal const string GenerationNotInProgress = "GenerationNotInProgress";

		// Token: 0x040016D8 RID: 5848
		internal const string Generator_CountIsWrong = "Generator_CountIsWrong";

		// Token: 0x040016D9 RID: 5849
		internal const string Generator_Inconsistent = "Generator_Inconsistent";

		// Token: 0x040016DA RID: 5850
		internal const string Generator_ItemIsWrong = "Generator_ItemIsWrong";

		// Token: 0x040016DB RID: 5851
		internal const string Generator_MoreErrors = "Generator_MoreErrors";

		// Token: 0x040016DC RID: 5852
		internal const string Generator_Readme0 = "Generator_Readme0";

		// Token: 0x040016DD RID: 5853
		internal const string Generator_Readme1 = "Generator_Readme1";

		// Token: 0x040016DE RID: 5854
		internal const string Generator_Readme2 = "Generator_Readme2";

		// Token: 0x040016DF RID: 5855
		internal const string Generator_Readme3 = "Generator_Readme3";

		// Token: 0x040016E0 RID: 5856
		internal const string Generator_Readme4 = "Generator_Readme4";

		// Token: 0x040016E1 RID: 5857
		internal const string Generator_Readme5 = "Generator_Readme5";

		// Token: 0x040016E2 RID: 5858
		internal const string Generator_Readme6 = "Generator_Readme6";

		// Token: 0x040016E3 RID: 5859
		internal const string Generator_Readme7 = "Generator_Readme7";

		// Token: 0x040016E4 RID: 5860
		internal const string Generator_Readme8 = "Generator_Readme8";

		// Token: 0x040016E5 RID: 5861
		internal const string Generator_Readme9 = "Generator_Readme9";

		// Token: 0x040016E6 RID: 5862
		internal const string Generator_Unnamed = "Generator_Unnamed";

		// Token: 0x040016E7 RID: 5863
		internal const string GetResponseFailed = "GetResponseFailed";

		// Token: 0x040016E8 RID: 5864
		internal const string GetStreamFailed = "GetStreamFailed";

		// Token: 0x040016E9 RID: 5865
		internal const string GlyphsAdvanceWidthCannotBeNegative = "GlyphsAdvanceWidthCannotBeNegative";

		// Token: 0x040016EA RID: 5866
		internal const string GlyphsCaretStopsContainsHexDigits = "GlyphsCaretStopsContainsHexDigits";

		// Token: 0x040016EB RID: 5867
		internal const string GlyphsCaretStopsLengthCorrespondsToUnicodeString = "GlyphsCaretStopsLengthCorrespondsToUnicodeString";

		// Token: 0x040016EC RID: 5868
		internal const string GlyphsClusterBadCharactersBeforeBracket = "GlyphsClusterBadCharactersBeforeBracket";

		// Token: 0x040016ED RID: 5869
		internal const string GlyphsClusterMisplacedSeparator = "GlyphsClusterMisplacedSeparator";

		// Token: 0x040016EE RID: 5870
		internal const string GlyphsClusterNoMatchingBracket = "GlyphsClusterNoMatchingBracket";

		// Token: 0x040016EF RID: 5871
		internal const string GlyphsClusterNoNestedClusters = "GlyphsClusterNoNestedClusters";

		// Token: 0x040016F0 RID: 5872
		internal const string GlyphsIndexRequiredIfNoUnicode = "GlyphsIndexRequiredIfNoUnicode";

		// Token: 0x040016F1 RID: 5873
		internal const string GlyphsIndexRequiredWithinCluster = "GlyphsIndexRequiredWithinCluster";

		// Token: 0x040016F2 RID: 5874
		internal const string GlyphsTooManyCommas = "GlyphsTooManyCommas";

		// Token: 0x040016F3 RID: 5875
		internal const string GlyphsUnicodeStringAndIndicesCannotBothBeEmpty = "GlyphsUnicodeStringAndIndicesCannotBothBeEmpty";

		// Token: 0x040016F4 RID: 5876
		internal const string GlyphsUnicodeStringIsTooShort = "GlyphsUnicodeStringIsTooShort";

		// Token: 0x040016F5 RID: 5877
		internal const string GridCollection_CannotModifyReadOnly = "GridCollection_CannotModifyReadOnly";

		// Token: 0x040016F6 RID: 5878
		internal const string GridCollection_DestArrayInvalidLength = "GridCollection_DestArrayInvalidLength";

		// Token: 0x040016F7 RID: 5879
		internal const string GridCollection_DestArrayInvalidLowerBound = "GridCollection_DestArrayInvalidLowerBound";

		// Token: 0x040016F8 RID: 5880
		internal const string GridCollection_DestArrayInvalidRank = "GridCollection_DestArrayInvalidRank";

		// Token: 0x040016F9 RID: 5881
		internal const string GridCollection_InOtherCollection = "GridCollection_InOtherCollection";

		// Token: 0x040016FA RID: 5882
		internal const string GridCollection_MustBeCertainType = "GridCollection_MustBeCertainType";

		// Token: 0x040016FB RID: 5883
		internal const string Grid_UnexpectedParameterType = "Grid_UnexpectedParameterType";

		// Token: 0x040016FC RID: 5884
		internal const string HandlerTypeIllegal = "HandlerTypeIllegal";

		// Token: 0x040016FD RID: 5885
		internal const string HasLogicalParent = "HasLogicalParent";

		// Token: 0x040016FE RID: 5886
		internal const string HostedWindowMustBeAChildWindow = "HostedWindowMustBeAChildWindow";

		// Token: 0x040016FF RID: 5887
		internal const string HostingStatusCancelled = "HostingStatusCancelled";

		// Token: 0x04001700 RID: 5888
		internal const string HostingStatusDownloadApp = "HostingStatusDownloadApp";

		// Token: 0x04001701 RID: 5889
		internal const string HostingStatusDownloadAppInfo = "HostingStatusDownloadAppInfo";

		// Token: 0x04001702 RID: 5890
		internal const string HostingStatusFailed = "HostingStatusFailed";

		// Token: 0x04001703 RID: 5891
		internal const string HostingStatusPreparingToRun = "HostingStatusPreparingToRun";

		// Token: 0x04001704 RID: 5892
		internal const string HostingStatusVerifying = "HostingStatusVerifying";

		// Token: 0x04001705 RID: 5893
		internal const string HwndHostDoesNotSupportChildKeyboardSinks = "HwndHostDoesNotSupportChildKeyboardSinks";

		// Token: 0x04001706 RID: 5894
		internal const string HyperLinkTargetNotFound = "HyperLinkTargetNotFound";

		// Token: 0x04001707 RID: 5895
		internal const string HyphenatorDisposed = "HyphenatorDisposed";

		// Token: 0x04001708 RID: 5896
		internal const string IconMustBeBitmapFrame = "IconMustBeBitmapFrame";

		// Token: 0x04001709 RID: 5897
		internal const string IDPInvalidContentPosition = "IDPInvalidContentPosition";

		// Token: 0x0400170A RID: 5898
		internal const string IDPNegativePageNumber = "IDPNegativePageNumber";

		// Token: 0x0400170B RID: 5899
		internal const string IllegalTreeChangeDetected = "IllegalTreeChangeDetected";

		// Token: 0x0400170C RID: 5900
		internal const string IllegalTreeChangeDetectedPostAction = "IllegalTreeChangeDetectedPostAction";

		// Token: 0x0400170D RID: 5901
		internal const string Illegal_InheritanceBehaviorSettor = "Illegal_InheritanceBehaviorSettor";

		// Token: 0x0400170E RID: 5902
		internal const string ImplementOtherMembersWithSort = "ImplementOtherMembersWithSort";

		// Token: 0x0400170F RID: 5903
		internal const string InavalidStartItem = "InavalidStartItem";

		// Token: 0x04001710 RID: 5904
		internal const string IncompatibleCLRText = "IncompatibleCLRText";

		// Token: 0x04001711 RID: 5905
		internal const string IncompatibleWinFXText = "IncompatibleWinFXText";

		// Token: 0x04001712 RID: 5906
		internal const string InconsistentBindingList = "InconsistentBindingList";

		// Token: 0x04001713 RID: 5907
		internal const string IncorrectAnchorLength = "IncorrectAnchorLength";

		// Token: 0x04001714 RID: 5908
		internal const string IncorrectFlowDirection = "IncorrectFlowDirection";

		// Token: 0x04001715 RID: 5909
		internal const string IncorrectLocatorPartType = "IncorrectLocatorPartType";

		// Token: 0x04001716 RID: 5910
		internal const string IndexedPropDescNotImplemented = "IndexedPropDescNotImplemented";

		// Token: 0x04001717 RID: 5911
		internal const string InDifferentParagraphs = "InDifferentParagraphs";

		// Token: 0x04001718 RID: 5912
		internal const string InDifferentScope = "InDifferentScope";

		// Token: 0x04001719 RID: 5913
		internal const string InDifferentTextContainers = "InDifferentTextContainers";

		// Token: 0x0400171A RID: 5914
		internal const string InkCanvasDeselectKeyDisplayString = "InkCanvasDeselectKeyDisplayString";

		// Token: 0x0400171B RID: 5915
		internal const string InputScopeAttribute_E_OUTOFMEMORY = "InputScopeAttribute_E_OUTOFMEMORY";

		// Token: 0x0400171C RID: 5916
		internal const string InputStreamMustBeReadable = "InputStreamMustBeReadable";

		// Token: 0x0400171D RID: 5917
		internal const string InsertInDeferSelectionActive = "InsertInDeferSelectionActive";

		// Token: 0x0400171E RID: 5918
		internal const string IntegerCollectionLengthLessThanZero = "IntegerCollectionLengthLessThanZero";

		// Token: 0x0400171F RID: 5919
		internal const string InvalidAnchorPosition = "InvalidAnchorPosition";

		// Token: 0x04001720 RID: 5920
		internal const string InvalidAttachedAnchor = "InvalidAttachedAnchor";

		// Token: 0x04001721 RID: 5921
		internal const string InvalidAttachedAnnotation = "InvalidAttachedAnnotation";

		// Token: 0x04001722 RID: 5922
		internal const string InvalidAttributeValue = "InvalidAttributeValue";

		// Token: 0x04001723 RID: 5923
		internal const string InvalidByteRanges = "InvalidByteRanges";

		// Token: 0x04001724 RID: 5924
		internal const string InvalidClipboardFormat = "InvalidClipboardFormat";

		// Token: 0x04001725 RID: 5925
		internal const string InvalidCompositionTarget = "InvalidCompositionTarget";

		// Token: 0x04001726 RID: 5926
		internal const string InvalidControlTemplateTargetType = "InvalidControlTemplateTargetType";

		// Token: 0x04001727 RID: 5927
		internal const string InvalidCtorParameterNoInfinity = "InvalidCtorParameterNoInfinity";

		// Token: 0x04001728 RID: 5928
		internal const string InvalidCtorParameterNoNaN = "InvalidCtorParameterNoNaN";

		// Token: 0x04001729 RID: 5929
		internal const string InvalidCtorParameterNoNegative = "InvalidCtorParameterNoNegative";

		// Token: 0x0400172A RID: 5930
		internal const string InvalidCtorParameterUnknownFigureUnitType = "InvalidCtorParameterUnknownFigureUnitType";

		// Token: 0x0400172B RID: 5931
		internal const string InvalidCtorParameterUnknownGridUnitType = "InvalidCtorParameterUnknownGridUnitType";

		// Token: 0x0400172C RID: 5932
		internal const string InvalidCtorParameterUnknownVirtualizationCacheLengthUnitType = "InvalidCtorParameterUnknownVirtualizationCacheLengthUnitType";

		// Token: 0x0400172D RID: 5933
		internal const string InvalidCustomSerialize = "InvalidCustomSerialize";

		// Token: 0x0400172E RID: 5934
		internal const string InvalidDeployText = "InvalidDeployText";

		// Token: 0x0400172F RID: 5935
		internal const string InvalidDeployTitle = "InvalidDeployTitle";

		// Token: 0x04001730 RID: 5936
		internal const string InvalidDeSerialize = "InvalidDeSerialize";

		// Token: 0x04001731 RID: 5937
		internal const string InvalidDiameter = "InvalidDiameter";

		// Token: 0x04001732 RID: 5938
		internal const string InvalidDSContentType = "InvalidDSContentType";

		// Token: 0x04001733 RID: 5939
		internal const string InvalidEmptyArray = "InvalidEmptyArray";

		// Token: 0x04001734 RID: 5940
		internal const string InvalidEmptyStrokeCollection = "InvalidEmptyStrokeCollection";

		// Token: 0x04001735 RID: 5941
		internal const string InvalidEndOfBaml = "InvalidEndOfBaml";

		// Token: 0x04001736 RID: 5942
		internal const string InvalidEventHandle = "InvalidEventHandle";

		// Token: 0x04001737 RID: 5943
		internal const string InvalidGuid = "InvalidGuid";

		// Token: 0x04001738 RID: 5944
		internal const string InvalidHighlightColor = "InvalidHighlightColor";

		// Token: 0x04001739 RID: 5945
		internal const string InvalidInkForeground = "InvalidInkForeground";

		// Token: 0x0400173A RID: 5946
		internal const string InvalidItemContainer = "InvalidItemContainer";

		// Token: 0x0400173B RID: 5947
		internal const string InvalidLocalizabilityValue = "InvalidLocalizabilityValue";

		// Token: 0x0400173C RID: 5948
		internal const string InvalidLocatorPart = "InvalidLocatorPart";

		// Token: 0x0400173D RID: 5949
		internal const string InvalidLocCommentTarget = "InvalidLocCommentTarget";

		// Token: 0x0400173E RID: 5950
		internal const string InvalidLocCommentValue = "InvalidLocCommentValue";

		// Token: 0x0400173F RID: 5951
		internal const string InvalidNamespace = "InvalidNamespace";

		// Token: 0x04001740 RID: 5952
		internal const string InvalidOperationDuringClosing = "InvalidOperationDuringClosing";

		// Token: 0x04001741 RID: 5953
		internal const string InvalidOperation_AddBackEntryNoContent = "InvalidOperation_AddBackEntryNoContent";

		// Token: 0x04001742 RID: 5954
		internal const string InvalidOperation_CannotClearFwdStack = "InvalidOperation_CannotClearFwdStack";

		// Token: 0x04001743 RID: 5955
		internal const string InvalidOperation_CannotReenterPageFunction = "InvalidOperation_CannotReenterPageFunction";

		// Token: 0x04001744 RID: 5956
		internal const string InvalidOperation_CantChangeJournalOwnership = "InvalidOperation_CantChangeJournalOwnership";

		// Token: 0x04001745 RID: 5957
		internal const string InvalidOperation_IComparerFailed = "InvalidOperation_IComparerFailed";

		// Token: 0x04001746 RID: 5958
		internal const string InvalidOperation_MustImplementIPCCSOrHandleNavigating = "InvalidOperation_MustImplementIPCCSOrHandleNavigating";

		// Token: 0x04001747 RID: 5959
		internal const string InvalidOperation_NoJournal = "InvalidOperation_NoJournal";

		// Token: 0x04001748 RID: 5960
		internal const string InvalidPageFunctionType = "InvalidPageFunctionType";

		// Token: 0x04001749 RID: 5961
		internal const string InvalidPoint = "InvalidPoint";

		// Token: 0x0400174A RID: 5962
		internal const string InvalidPropertyValue = "InvalidPropertyValue";

		// Token: 0x0400174B RID: 5963
		internal const string InvalidSelectionPages = "InvalidSelectionPages";

		// Token: 0x0400174C RID: 5964
		internal const string InvalidSetterValue = "InvalidSetterValue";

		// Token: 0x0400174D RID: 5965
		internal const string InvalidSFContentType = "InvalidSFContentType";

		// Token: 0x0400174E RID: 5966
		internal const string InvalidStartNodeForTextSelection = "InvalidStartNodeForTextSelection";

		// Token: 0x0400174F RID: 5967
		internal const string InvalidStartOfBaml = "InvalidStartOfBaml";

		// Token: 0x04001750 RID: 5968
		internal const string InvalidStickyNoteAnnotation = "InvalidStickyNoteAnnotation";

		// Token: 0x04001751 RID: 5969
		internal const string InvalidStickyNoteTemplate = "InvalidStickyNoteTemplate";

		// Token: 0x04001752 RID: 5970
		internal const string InvalidStoryFragmentsMarkup = "InvalidStoryFragmentsMarkup";

		// Token: 0x04001753 RID: 5971
		internal const string InvalidStringCornerRadius = "InvalidStringCornerRadius";

		// Token: 0x04001754 RID: 5972
		internal const string InvalidStringThickness = "InvalidStringThickness";

		// Token: 0x04001755 RID: 5973
		internal const string InvalidStringVirtualizationCacheLength = "InvalidStringVirtualizationCacheLength";

		// Token: 0x04001756 RID: 5974
		internal const string InvalidSubTreeProcessor = "InvalidSubTreeProcessor";

		// Token: 0x04001757 RID: 5975
		internal const string InvalidTempFileName = "InvalidTempFileName";

		// Token: 0x04001758 RID: 5976
		internal const string InvalidValueForTopLeft = "InvalidValueForTopLeft";

		// Token: 0x04001759 RID: 5977
		internal const string InvalidValueSpecified = "InvalidValueSpecified";

		// Token: 0x0400175A RID: 5978
		internal const string InvalidXmlContent = "InvalidXmlContent";

		// Token: 0x0400175B RID: 5979
		internal const string ItemCollectionHasNoCollection = "ItemCollectionHasNoCollection";

		// Token: 0x0400175C RID: 5980
		internal const string ItemCollectionRemoveArgumentOutOfRange = "ItemCollectionRemoveArgumentOutOfRange";

		// Token: 0x0400175D RID: 5981
		internal const string ItemCollectionShouldUseInnerSyncRoot = "ItemCollectionShouldUseInnerSyncRoot";

		// Token: 0x0400175E RID: 5982
		internal const string ItemsControl_ParentNotFrameworkElement = "ItemsControl_ParentNotFrameworkElement";

		// Token: 0x0400175F RID: 5983
		internal const string ItemsPanelNotAPanel = "ItemsPanelNotAPanel";

		// Token: 0x04001760 RID: 5984
		internal const string ItemsPanelNotSingleNode = "ItemsPanelNotSingleNode";

		// Token: 0x04001761 RID: 5985
		internal const string ItemsSourceInUse = "ItemsSourceInUse";

		// Token: 0x04001762 RID: 5986
		internal const string ItemTemplateSelectorBreaksDisplayMemberPath = "ItemTemplateSelectorBreaksDisplayMemberPath";

		// Token: 0x04001763 RID: 5987
		internal const string JumpItemsRejectedEventArgs_CountMismatch = "JumpItemsRejectedEventArgs_CountMismatch";

		// Token: 0x04001764 RID: 5988
		internal const string JumpList_CantApplyUntilEndInit = "JumpList_CantApplyUntilEndInit";

		// Token: 0x04001765 RID: 5989
		internal const string JumpList_CantCallUnbalancedEndInit = "JumpList_CantCallUnbalancedEndInit";

		// Token: 0x04001766 RID: 5990
		internal const string JumpList_CantNestBeginInitCalls = "JumpList_CantNestBeginInitCalls";

		// Token: 0x04001767 RID: 5991
		internal const string KeyAlignCenterDisplayString = "KeyAlignCenterDisplayString";

		// Token: 0x04001768 RID: 5992
		internal const string KeyAlignJustifyDisplayString = "KeyAlignJustifyDisplayString";

		// Token: 0x04001769 RID: 5993
		internal const string KeyAlignLeftDisplayString = "KeyAlignLeftDisplayString";

		// Token: 0x0400176A RID: 5994
		internal const string KeyAlignRightDisplayString = "KeyAlignRightDisplayString";

		// Token: 0x0400176B RID: 5995
		internal const string KeyAltUndoDisplayString = "KeyAltUndoDisplayString";

		// Token: 0x0400176C RID: 5996
		internal const string KeyApplyBackground = "KeyApplyBackground";

		// Token: 0x0400176D RID: 5997
		internal const string KeyApplyBackgroundDisplayString = "KeyApplyBackgroundDisplayString";

		// Token: 0x0400176E RID: 5998
		internal const string KeyApplyDoubleSpaceDisplayString = "KeyApplyDoubleSpaceDisplayString";

		// Token: 0x0400176F RID: 5999
		internal const string KeyApplyFontFamily = "KeyApplyFontFamily";

		// Token: 0x04001770 RID: 6000
		internal const string KeyApplyFontFamilyDisplayString = "KeyApplyFontFamilyDisplayString";

		// Token: 0x04001771 RID: 6001
		internal const string KeyApplyFontSize = "KeyApplyFontSize";

		// Token: 0x04001772 RID: 6002
		internal const string KeyApplyFontSizeDisplayString = "KeyApplyFontSizeDisplayString";

		// Token: 0x04001773 RID: 6003
		internal const string KeyApplyForeground = "KeyApplyForeground";

		// Token: 0x04001774 RID: 6004
		internal const string KeyApplyForegroundDisplayString = "KeyApplyForegroundDisplayString";

		// Token: 0x04001775 RID: 6005
		internal const string KeyApplyOneAndAHalfSpaceDisplayString = "KeyApplyOneAndAHalfSpaceDisplayString";

		// Token: 0x04001776 RID: 6006
		internal const string KeyApplySingleSpaceDisplayString = "KeyApplySingleSpaceDisplayString";

		// Token: 0x04001777 RID: 6007
		internal const string KeyBackspaceDisplayString = "KeyBackspaceDisplayString";

		// Token: 0x04001778 RID: 6008
		internal const string KeyCollectionHasInvalidKey = "KeyCollectionHasInvalidKey";

		// Token: 0x04001779 RID: 6009
		internal const string KeyCopyDisplayString = "KeyCopyDisplayString";

		// Token: 0x0400177A RID: 6010
		internal const string KeyCopyFormatDisplayString = "KeyCopyFormatDisplayString";

		// Token: 0x0400177B RID: 6011
		internal const string KeyCorrectionList = "KeyCorrectionList";

		// Token: 0x0400177C RID: 6012
		internal const string KeyCorrectionListDisplayString = "KeyCorrectionListDisplayString";

		// Token: 0x0400177D RID: 6013
		internal const string KeyCtrlInsertDisplayString = "KeyCtrlInsertDisplayString";

		// Token: 0x0400177E RID: 6014
		internal const string KeyCutDisplayString = "KeyCutDisplayString";

		// Token: 0x0400177F RID: 6015
		internal const string KeyDecreaseFontSizeDisplayString = "KeyDecreaseFontSizeDisplayString";

		// Token: 0x04001780 RID: 6016
		internal const string KeyDecreaseIndentationDisplayString = "KeyDecreaseIndentationDisplayString";

		// Token: 0x04001781 RID: 6017
		internal const string KeyDeleteColumnsDisplayString = "KeyDeleteColumnsDisplayString";

		// Token: 0x04001782 RID: 6018
		internal const string KeyDeleteDisplayString = "KeyDeleteDisplayString";

		// Token: 0x04001783 RID: 6019
		internal const string KeyDeleteNextWordDisplayString = "KeyDeleteNextWordDisplayString";

		// Token: 0x04001784 RID: 6020
		internal const string KeyDeletePreviousWordDisplayString = "KeyDeletePreviousWordDisplayString";

		// Token: 0x04001785 RID: 6021
		internal const string KeyDeleteRows = "KeyDeleteRows";

		// Token: 0x04001786 RID: 6022
		internal const string KeyDeleteRowsDisplayString = "KeyDeleteRowsDisplayString";

		// Token: 0x04001787 RID: 6023
		internal const string KeyEnterLineBreakDisplayString = "KeyEnterLineBreakDisplayString";

		// Token: 0x04001788 RID: 6024
		internal const string KeyEnterParagraphBreakDisplayString = "KeyEnterParagraphBreakDisplayString";

		// Token: 0x04001789 RID: 6025
		internal const string KeyIncreaseFontSizeDisplayString = "KeyIncreaseFontSizeDisplayString";

		// Token: 0x0400178A RID: 6026
		internal const string KeyIncreaseIndentationDisplayString = "KeyIncreaseIndentationDisplayString";

		// Token: 0x0400178B RID: 6027
		internal const string KeyInsertColumnsDisplayString = "KeyInsertColumnsDisplayString";

		// Token: 0x0400178C RID: 6028
		internal const string KeyInsertRowsDisplayString = "KeyInsertRowsDisplayString";

		// Token: 0x0400178D RID: 6029
		internal const string KeyInsertTableDisplayString = "KeyInsertTableDisplayString";

		// Token: 0x0400178E RID: 6030
		internal const string KeyMergeCellsDisplayString = "KeyMergeCellsDisplayString";

		// Token: 0x0400178F RID: 6031
		internal const string KeyMoveDownByLineDisplayString = "KeyMoveDownByLineDisplayString";

		// Token: 0x04001790 RID: 6032
		internal const string KeyMoveDownByPageDisplayString = "KeyMoveDownByPageDisplayString";

		// Token: 0x04001791 RID: 6033
		internal const string KeyMoveDownByParagraphDisplayString = "KeyMoveDownByParagraphDisplayString";

		// Token: 0x04001792 RID: 6034
		internal const string KeyMoveLeftByCharacterDisplayString = "KeyMoveLeftByCharacterDisplayString";

		// Token: 0x04001793 RID: 6035
		internal const string KeyMoveLeftByWordDisplayString = "KeyMoveLeftByWordDisplayString";

		// Token: 0x04001794 RID: 6036
		internal const string KeyMoveRightByCharacterDisplayString = "KeyMoveRightByCharacterDisplayString";

		// Token: 0x04001795 RID: 6037
		internal const string KeyMoveRightByWordDisplayString = "KeyMoveRightByWordDisplayString";

		// Token: 0x04001796 RID: 6038
		internal const string KeyMoveToColumnEndDisplayString = "KeyMoveToColumnEndDisplayString";

		// Token: 0x04001797 RID: 6039
		internal const string KeyMoveToColumnStartDisplayString = "KeyMoveToColumnStartDisplayString";

		// Token: 0x04001798 RID: 6040
		internal const string KeyMoveToDocumentEndDisplayString = "KeyMoveToDocumentEndDisplayString";

		// Token: 0x04001799 RID: 6041
		internal const string KeyMoveToDocumentStartDisplayString = "KeyMoveToDocumentStartDisplayString";

		// Token: 0x0400179A RID: 6042
		internal const string KeyMoveToLineEndDisplayString = "KeyMoveToLineEndDisplayString";

		// Token: 0x0400179B RID: 6043
		internal const string KeyMoveToLineStartDisplayString = "KeyMoveToLineStartDisplayString";

		// Token: 0x0400179C RID: 6044
		internal const string KeyMoveToWindowBottomDisplayString = "KeyMoveToWindowBottomDisplayString";

		// Token: 0x0400179D RID: 6045
		internal const string KeyMoveToWindowTopDisplayString = "KeyMoveToWindowTopDisplayString";

		// Token: 0x0400179E RID: 6046
		internal const string KeyMoveUpByLineDisplayString = "KeyMoveUpByLineDisplayString";

		// Token: 0x0400179F RID: 6047
		internal const string KeyMoveUpByPageDisplayString = "KeyMoveUpByPageDisplayString";

		// Token: 0x040017A0 RID: 6048
		internal const string KeyMoveUpByParagraphDisplayString = "KeyMoveUpByParagraphDisplayString";

		// Token: 0x040017A1 RID: 6049
		internal const string KeyPasteFormatDisplayString = "KeyPasteFormatDisplayString";

		// Token: 0x040017A2 RID: 6050
		internal const string KeyRedoDisplayString = "KeyRedoDisplayString";

		// Token: 0x040017A3 RID: 6051
		internal const string KeyRemoveListMarkersDisplayString = "KeyRemoveListMarkersDisplayString";

		// Token: 0x040017A4 RID: 6052
		internal const string KeyResetFormatDisplayString = "KeyResetFormatDisplayString";

		// Token: 0x040017A5 RID: 6053
		internal const string KeySelectAllDisplayString = "KeySelectAllDisplayString";

		// Token: 0x040017A6 RID: 6054
		internal const string KeySelectDownByLineDisplayString = "KeySelectDownByLineDisplayString";

		// Token: 0x040017A7 RID: 6055
		internal const string KeySelectDownByPageDisplayString = "KeySelectDownByPageDisplayString";

		// Token: 0x040017A8 RID: 6056
		internal const string KeySelectDownByParagraphDisplayString = "KeySelectDownByParagraphDisplayString";

		// Token: 0x040017A9 RID: 6057
		internal const string KeySelectLeftByCharacterDisplayString = "KeySelectLeftByCharacterDisplayString";

		// Token: 0x040017AA RID: 6058
		internal const string KeySelectLeftByWordDisplayString = "KeySelectLeftByWordDisplayString";

		// Token: 0x040017AB RID: 6059
		internal const string KeySelectRightByCharacterDisplayString = "KeySelectRightByCharacterDisplayString";

		// Token: 0x040017AC RID: 6060
		internal const string KeySelectRightByWordDisplayString = "KeySelectRightByWordDisplayString";

		// Token: 0x040017AD RID: 6061
		internal const string KeySelectToColumnEndDisplayString = "KeySelectToColumnEndDisplayString";

		// Token: 0x040017AE RID: 6062
		internal const string KeySelectToColumnStartDisplayString = "KeySelectToColumnStartDisplayString";

		// Token: 0x040017AF RID: 6063
		internal const string KeySelectToDocumentEndDisplayString = "KeySelectToDocumentEndDisplayString";

		// Token: 0x040017B0 RID: 6064
		internal const string KeySelectToDocumentStartDisplayString = "KeySelectToDocumentStartDisplayString";

		// Token: 0x040017B1 RID: 6065
		internal const string KeySelectToLineEndDisplayString = "KeySelectToLineEndDisplayString";

		// Token: 0x040017B2 RID: 6066
		internal const string KeySelectToLineStartDisplayString = "KeySelectToLineStartDisplayString";

		// Token: 0x040017B3 RID: 6067
		internal const string KeySelectToWindowBottomDisplayString = "KeySelectToWindowBottomDisplayString";

		// Token: 0x040017B4 RID: 6068
		internal const string KeySelectToWindowTopDisplayString = "KeySelectToWindowTopDisplayString";

		// Token: 0x040017B5 RID: 6069
		internal const string KeySelectUpByLineDisplayString = "KeySelectUpByLineDisplayString";

		// Token: 0x040017B6 RID: 6070
		internal const string KeySelectUpByPageDisplayString = "KeySelectUpByPageDisplayString";

		// Token: 0x040017B7 RID: 6071
		internal const string KeySelectUpByParagraphDisplayString = "KeySelectUpByParagraphDisplayString";

		// Token: 0x040017B8 RID: 6072
		internal const string KeyShiftBackspaceDisplayString = "KeyShiftBackspaceDisplayString";

		// Token: 0x040017B9 RID: 6073
		internal const string KeyShiftDeleteDisplayString = "KeyShiftDeleteDisplayString";

		// Token: 0x040017BA RID: 6074
		internal const string KeyShiftInsertDisplayString = "KeyShiftInsertDisplayString";

		// Token: 0x040017BB RID: 6075
		internal const string KeyShiftSpaceDisplayString = "KeyShiftSpaceDisplayString";

		// Token: 0x040017BC RID: 6076
		internal const string KeySpaceDisplayString = "KeySpaceDisplayString";

		// Token: 0x040017BD RID: 6077
		internal const string KeySplitCellDisplayString = "KeySplitCellDisplayString";

		// Token: 0x040017BE RID: 6078
		internal const string KeySwitchViewingModeDisplayString = "KeySwitchViewingModeDisplayString";

		// Token: 0x040017BF RID: 6079
		internal const string KeyTabBackwardDisplayString = "KeyTabBackwardDisplayString";

		// Token: 0x040017C0 RID: 6080
		internal const string KeyTabForwardDisplayString = "KeyTabForwardDisplayString";

		// Token: 0x040017C1 RID: 6081
		internal const string KeyToggleBoldDisplayString = "KeyToggleBoldDisplayString";

		// Token: 0x040017C2 RID: 6082
		internal const string KeyToggleBulletsDisplayString = "KeyToggleBulletsDisplayString";

		// Token: 0x040017C3 RID: 6083
		internal const string KeyToggleInsertDisplayString = "KeyToggleInsertDisplayString";

		// Token: 0x040017C4 RID: 6084
		internal const string KeyToggleItalicDisplayString = "KeyToggleItalicDisplayString";

		// Token: 0x040017C5 RID: 6085
		internal const string KeyToggleNumberingDisplayString = "KeyToggleNumberingDisplayString";

		// Token: 0x040017C6 RID: 6086
		internal const string KeyToggleSpellCheck = "KeyToggleSpellCheck";

		// Token: 0x040017C7 RID: 6087
		internal const string KeyToggleSpellCheckDisplayString = "KeyToggleSpellCheckDisplayString";

		// Token: 0x040017C8 RID: 6088
		internal const string KeyToggleSubscriptDisplayString = "KeyToggleSubscriptDisplayString";

		// Token: 0x040017C9 RID: 6089
		internal const string KeyToggleSuperscriptDisplayString = "KeyToggleSuperscriptDisplayString";

		// Token: 0x040017CA RID: 6090
		internal const string KeyToggleUnderlineDisplayString = "KeyToggleUnderlineDisplayString";

		// Token: 0x040017CB RID: 6091
		internal const string KeyUndoDisplayString = "KeyUndoDisplayString";

		// Token: 0x040017CC RID: 6092
		internal const string KillBitEnforcedShutdown = "KillBitEnforcedShutdown";

		// Token: 0x040017CD RID: 6093
		internal const string KnownTypeIdNegative = "KnownTypeIdNegative";

		// Token: 0x040017CE RID: 6094
		internal const string LengthFormatError = "LengthFormatError";

		// Token: 0x040017CF RID: 6095
		internal const string ListBoxInvalidAnchorItem = "ListBoxInvalidAnchorItem";

		// Token: 0x040017D0 RID: 6096
		internal const string ListBoxSelectAllKeyDisplayString = "ListBoxSelectAllKeyDisplayString";

		// Token: 0x040017D1 RID: 6097
		internal const string ListBoxSelectAllSelectionMode = "ListBoxSelectAllSelectionMode";

		// Token: 0x040017D2 RID: 6098
		internal const string ListBoxSelectAllText = "ListBoxSelectAllText";

		// Token: 0x040017D3 RID: 6099
		internal const string ListElementItemNotAChildOfList = "ListElementItemNotAChildOfList";

		// Token: 0x040017D4 RID: 6100
		internal const string ListView_GridViewColumnCollectionIsReadOnly = "ListView_GridViewColumnCollectionIsReadOnly";

		// Token: 0x040017D5 RID: 6101
		internal const string ListView_IllegalChildrenType = "ListView_IllegalChildrenType";

		// Token: 0x040017D6 RID: 6102
		internal const string ListView_MissingParameterlessConstructor = "ListView_MissingParameterlessConstructor";

		// Token: 0x040017D7 RID: 6103
		internal const string ListView_NotAllowShareColumnToTwoColumnCollection = "ListView_NotAllowShareColumnToTwoColumnCollection";

		// Token: 0x040017D8 RID: 6104
		internal const string ListView_ViewCannotBeShared = "ListView_ViewCannotBeShared";

		// Token: 0x040017D9 RID: 6105
		internal const string LogicalTreeLoop = "LogicalTreeLoop";

		// Token: 0x040017DA RID: 6106
		internal const string LoopDetected = "LoopDetected";

		// Token: 0x040017DB RID: 6107
		internal const string MarkupExtensionBadStatic = "MarkupExtensionBadStatic";

		// Token: 0x040017DC RID: 6108
		internal const string MarkupExtensionDynamicOrBindingInCollection = "MarkupExtensionDynamicOrBindingInCollection";

		// Token: 0x040017DD RID: 6109
		internal const string MarkupExtensionDynamicOrBindingOnClrProp = "MarkupExtensionDynamicOrBindingOnClrProp";

		// Token: 0x040017DE RID: 6110
		internal const string MarkupExtensionNoContext = "MarkupExtensionNoContext";

		// Token: 0x040017DF RID: 6111
		internal const string MarkupExtensionProperty = "MarkupExtensionProperty";

		// Token: 0x040017E0 RID: 6112
		internal const string MarkupExtensionResourceKey = "MarkupExtensionResourceKey";

		// Token: 0x040017E1 RID: 6113
		internal const string MarkupExtensionResourceNotFound = "MarkupExtensionResourceNotFound";

		// Token: 0x040017E2 RID: 6114
		internal const string MarkupExtensionStaticMember = "MarkupExtensionStaticMember";

		// Token: 0x040017E3 RID: 6115
		internal const string MarkupWriter_CannotSerializeGenerictype = "MarkupWriter_CannotSerializeGenerictype";

		// Token: 0x040017E4 RID: 6116
		internal const string MarkupWriter_CannotSerializeNestedPublictype = "MarkupWriter_CannotSerializeNestedPublictype";

		// Token: 0x040017E5 RID: 6117
		internal const string MarkupWriter_CannotSerializeNonPublictype = "MarkupWriter_CannotSerializeNonPublictype";

		// Token: 0x040017E6 RID: 6118
		internal const string MaximumNoteSizeExceeded = "MaximumNoteSizeExceeded";

		// Token: 0x040017E7 RID: 6119
		internal const string MaxLengthExceedsBufferSize = "MaxLengthExceedsBufferSize";

		// Token: 0x040017E8 RID: 6120
		internal const string MeasureReentrancyInvalid = "MeasureReentrancyInvalid";

		// Token: 0x040017E9 RID: 6121
		internal const string MediaElement_CannotSetSourceOnMediaElementDrivenByClock = "MediaElement_CannotSetSourceOnMediaElementDrivenByClock";

		// Token: 0x040017EA RID: 6122
		internal const string MemberNotAllowedDuringAddOrEdit = "MemberNotAllowedDuringAddOrEdit";

		// Token: 0x040017EB RID: 6123
		internal const string MemberNotAllowedDuringTransaction = "MemberNotAllowedDuringTransaction";

		// Token: 0x040017EC RID: 6124
		internal const string MemberNotAllowedForView = "MemberNotAllowedForView";

		// Token: 0x040017ED RID: 6125
		internal const string MissingAnnotationHighlightLayer = "MissingAnnotationHighlightLayer";

		// Token: 0x040017EE RID: 6126
		internal const string MissingContentSource = "MissingContentSource";

		// Token: 0x040017EF RID: 6127
		internal const string MissingTagInNamespace = "MissingTagInNamespace";

		// Token: 0x040017F0 RID: 6128
		internal const string MissingTriggerProperty = "MissingTriggerProperty";

		// Token: 0x040017F1 RID: 6129
		internal const string MissingValueConverter = "MissingValueConverter";

		// Token: 0x040017F2 RID: 6130
		internal const string ModificationEarlierThanCreation = "ModificationEarlierThanCreation";

		// Token: 0x040017F3 RID: 6131
		internal const string ModifyingLogicalTreeViaStylesNotImplemented = "ModifyingLogicalTreeViaStylesNotImplemented";

		// Token: 0x040017F4 RID: 6132
		internal const string MoreThanOneAttachedAnnotation = "MoreThanOneAttachedAnnotation";

		// Token: 0x040017F5 RID: 6133
		internal const string MoreThanOneStartingParts = "MoreThanOneStartingParts";

		// Token: 0x040017F6 RID: 6134
		internal const string MoveInDeferSelectionActive = "MoveInDeferSelectionActive";

		// Token: 0x040017F7 RID: 6135
		internal const string MultiBindingHasNoConverter = "MultiBindingHasNoConverter";

		// Token: 0x040017F8 RID: 6136
		internal const string MultipleAssemblyMatches = "MultipleAssemblyMatches";

		// Token: 0x040017F9 RID: 6137
		internal const string MultiSelectorSelectAll = "MultiSelectorSelectAll";

		// Token: 0x040017FA RID: 6138
		internal const string MultiSingleton = "MultiSingleton";

		// Token: 0x040017FB RID: 6139
		internal const string MultiThreadedCollectionChangeNotSupported = "MultiThreadedCollectionChangeNotSupported";

		// Token: 0x040017FC RID: 6140
		internal const string MustBaseOnStyleOfABaseType = "MustBaseOnStyleOfABaseType";

		// Token: 0x040017FD RID: 6141
		internal const string MustBeCondition = "MustBeCondition";

		// Token: 0x040017FE RID: 6142
		internal const string MustBeFrameworkDerived = "MustBeFrameworkDerived";

		// Token: 0x040017FF RID: 6143
		internal const string MustBeFrameworkOr3DDerived = "MustBeFrameworkOr3DDerived";

		// Token: 0x04001800 RID: 6144
		internal const string MustBeOfType = "MustBeOfType";

		// Token: 0x04001801 RID: 6145
		internal const string MustBeTriggerAction = "MustBeTriggerAction";

		// Token: 0x04001802 RID: 6146
		internal const string MustBeTypeOrString = "MustBeTypeOrString";

		// Token: 0x04001803 RID: 6147
		internal const string MustImplementIUriContext = "MustImplementIUriContext";

		// Token: 0x04001804 RID: 6148
		internal const string MustNotTemplateUnassociatedControl = "MustNotTemplateUnassociatedControl";

		// Token: 0x04001805 RID: 6149
		internal const string MustUseWindowStyleNone = "MustUseWindowStyleNone";

		// Token: 0x04001806 RID: 6150
		internal const string NamedObjectMustBeFrameworkElement = "NamedObjectMustBeFrameworkElement";

		// Token: 0x04001807 RID: 6151
		internal const string NameNotEmptyString = "NameNotEmptyString";

		// Token: 0x04001808 RID: 6152
		internal const string NameNotFound = "NameNotFound";

		// Token: 0x04001809 RID: 6153
		internal const string NameScopeDuplicateNamesNotAllowed = "NameScopeDuplicateNamesNotAllowed";

		// Token: 0x0400180A RID: 6154
		internal const string NameScopeInvalidIdentifierName = "NameScopeInvalidIdentifierName";

		// Token: 0x0400180B RID: 6155
		internal const string NameScopeNameNotEmptyString = "NameScopeNameNotEmptyString";

		// Token: 0x0400180C RID: 6156
		internal const string NameScopeNameNotFound = "NameScopeNameNotFound";

		// Token: 0x0400180D RID: 6157
		internal const string NameScopeNotFound = "NameScopeNotFound";

		// Token: 0x0400180E RID: 6158
		internal const string NamesNotSupportedInsideResourceDictionary = "NamesNotSupportedInsideResourceDictionary";

		// Token: 0x0400180F RID: 6159
		internal const string NavWindowMenuCurrentPage = "NavWindowMenuCurrentPage";

		// Token: 0x04001810 RID: 6160
		internal const string NeedToBeComVisible = "NeedToBeComVisible";

		// Token: 0x04001811 RID: 6161
		internal const string NegativeValue = "NegativeValue";

		// Token: 0x04001812 RID: 6162
		internal const string NestedBeginInitNotSupported = "NestedBeginInitNotSupported";

		// Token: 0x04001813 RID: 6163
		internal const string NoAddChild = "NoAddChild";

		// Token: 0x04001814 RID: 6164
		internal const string NoAttachedAnnotationToModify = "NoAttachedAnnotationToModify";

		// Token: 0x04001815 RID: 6165
		internal const string NoBackEntry = "NoBackEntry";

		// Token: 0x04001816 RID: 6166
		internal const string NoCheckOrChangeWhenDeferred = "NoCheckOrChangeWhenDeferred";

		// Token: 0x04001817 RID: 6167
		internal const string NoDefaultUpdateSourceTrigger = "NoDefaultUpdateSourceTrigger";

		// Token: 0x04001818 RID: 6168
		internal const string NoElement = "NoElement";

		// Token: 0x04001819 RID: 6169
		internal const string NoElementObject = "NoElementObject";

		// Token: 0x0400181A RID: 6170
		internal const string NoForwardEntry = "NoForwardEntry";

		// Token: 0x0400181B RID: 6171
		internal const string NoMulticastHandlers = "NoMulticastHandlers";

		// Token: 0x0400181C RID: 6172
		internal const string NonClsActivationException = "NonClsActivationException";

		// Token: 0x0400181D RID: 6173
		internal const string NonCLSException = "NonCLSException";

		// Token: 0x0400181E RID: 6174
		internal const string NonPackAppAbsoluteUriNotAllowed = "NonPackAppAbsoluteUriNotAllowed";

		// Token: 0x0400181F RID: 6175
		internal const string NonPackSooAbsoluteUriNotAllowed = "NonPackSooAbsoluteUriNotAllowed";

		// Token: 0x04001820 RID: 6176
		internal const string NonWhiteSpaceInAddText = "NonWhiteSpaceInAddText";

		// Token: 0x04001821 RID: 6177
		internal const string NoPresentationContextForGivenElement = "NoPresentationContextForGivenElement";

		// Token: 0x04001822 RID: 6178
		internal const string NoProcessorForSelectionType = "NoProcessorForSelectionType";

		// Token: 0x04001823 RID: 6179
		internal const string NoScopingElement = "NoScopingElement";

		// Token: 0x04001824 RID: 6180
		internal const string NotAllowedBeforeShow = "NotAllowedBeforeShow";

		// Token: 0x04001825 RID: 6181
		internal const string NotHighlightAnnotationType = "NotHighlightAnnotationType";

		// Token: 0x04001826 RID: 6182
		internal const string NotInAssociatedContainer = "NotInAssociatedContainer";

		// Token: 0x04001827 RID: 6183
		internal const string NotInAssociatedTree = "NotInAssociatedTree";

		// Token: 0x04001828 RID: 6184
		internal const string NotInThisTree = "NotInThisTree";

		// Token: 0x04001829 RID: 6185
		internal const string NotSupported = "NotSupported";

		// Token: 0x0400182A RID: 6186
		internal const string NotSupportedInBrowser = "NotSupportedInBrowser";

		// Token: 0x0400182B RID: 6187
		internal const string NoUpdateSourceTriggerForInnerBindingOfMultiBinding = "NoUpdateSourceTriggerForInnerBindingOfMultiBinding";

		// Token: 0x0400182C RID: 6188
		internal const string NullParentNode = "NullParentNode";

		// Token: 0x0400182D RID: 6189
		internal const string NullPropertyIllegal = "NullPropertyIllegal";

		// Token: 0x0400182E RID: 6190
		internal const string NullTypeIllegal = "NullTypeIllegal";

		// Token: 0x0400182F RID: 6191
		internal const string NullUri = "NullUri";

		// Token: 0x04001830 RID: 6192
		internal const string ObjectDataProviderCanHaveOnlyOneSource = "ObjectDataProviderCanHaveOnlyOneSource";

		// Token: 0x04001831 RID: 6193
		internal const string ObjectDataProviderHasNoSource = "ObjectDataProviderHasNoSource";

		// Token: 0x04001832 RID: 6194
		internal const string ObjectDataProviderNonCLSException = "ObjectDataProviderNonCLSException";

		// Token: 0x04001833 RID: 6195
		internal const string ObjectDataProviderNonCLSExceptionInvoke = "ObjectDataProviderNonCLSExceptionInvoke";

		// Token: 0x04001834 RID: 6196
		internal const string ObjectDataProviderParameterCollectionIsNotInUse = "ObjectDataProviderParameterCollectionIsNotInUse";

		// Token: 0x04001835 RID: 6197
		internal const string ObjectDisposed_StoreClosed = "ObjectDisposed_StoreClosed";

		// Token: 0x04001836 RID: 6198
		internal const string OnlyFlowAndFixedSupported = "OnlyFlowAndFixedSupported";

		// Token: 0x04001837 RID: 6199
		internal const string OnlyFlowFixedSupported = "OnlyFlowFixedSupported";

		// Token: 0x04001838 RID: 6200
		internal const string PageCacheSizeNotAllowed = "PageCacheSizeNotAllowed";

		// Token: 0x04001839 RID: 6201
		internal const string PageCannotHaveMultipleContent = "PageCannotHaveMultipleContent";

		// Token: 0x0400183A RID: 6202
		internal const string PageContentNotFound = "PageContentNotFound";

		// Token: 0x0400183B RID: 6203
		internal const string PageContentUnsupportedMimeType = "PageContentUnsupportedMimeType";

		// Token: 0x0400183C RID: 6204
		internal const string PageContentUnsupportedPageType = "PageContentUnsupportedPageType";

		// Token: 0x0400183D RID: 6205
		internal const string PanelIsNotItemsHost = "PanelIsNotItemsHost";

		// Token: 0x0400183E RID: 6206
		internal const string Panel_BoundPanel_NoChildren = "Panel_BoundPanel_NoChildren";

		// Token: 0x0400183F RID: 6207
		internal const string Panel_ItemsControlNotFound = "Panel_ItemsControlNotFound";

		// Token: 0x04001840 RID: 6208
		internal const string Panel_NoNullChildren = "Panel_NoNullChildren";

		// Token: 0x04001841 RID: 6209
		internal const string Panel_NoNullVisualParent = "Panel_NoNullVisualParent";

		// Token: 0x04001842 RID: 6210
		internal const string ParameterMustBeLogicalNode = "ParameterMustBeLogicalNode";

		// Token: 0x04001843 RID: 6211
		internal const string ParentOfPageMustBeWindowOrFrame = "ParentOfPageMustBeWindowOrFrame";

		// Token: 0x04001844 RID: 6212
		internal const string ParserAbandonedTypeConverterText = "ParserAbandonedTypeConverterText";

		// Token: 0x04001845 RID: 6213
		internal const string ParserAsyncOnRoot = "ParserAsyncOnRoot";

		// Token: 0x04001846 RID: 6214
		internal const string ParserAttachedPropInheritError = "ParserAttachedPropInheritError";

		// Token: 0x04001847 RID: 6215
		internal const string ParserAttributeArgsLow = "ParserAttributeArgsLow";

		// Token: 0x04001848 RID: 6216
		internal const string ParserAttributeNamespaceMisMatch = "ParserAttributeNamespaceMisMatch";

		// Token: 0x04001849 RID: 6217
		internal const string ParserBadAssemblyName = "ParserBadAssemblyName";

		// Token: 0x0400184A RID: 6218
		internal const string ParserBadAssemblyPath = "ParserBadAssemblyPath";

		// Token: 0x0400184B RID: 6219
		internal const string ParserBadChild = "ParserBadChild";

		// Token: 0x0400184C RID: 6220
		internal const string ParserBadConstructorParams = "ParserBadConstructorParams";

		// Token: 0x0400184D RID: 6221
		internal const string ParserBadEncoding = "ParserBadEncoding";

		// Token: 0x0400184E RID: 6222
		internal const string ParserBadKey = "ParserBadKey";

		// Token: 0x0400184F RID: 6223
		internal const string ParserBadMemberReference = "ParserBadMemberReference";

		// Token: 0x04001850 RID: 6224
		internal const string ParserBadName = "ParserBadName";

		// Token: 0x04001851 RID: 6225
		internal const string ParserBadNullableType = "ParserBadNullableType";

		// Token: 0x04001852 RID: 6226
		internal const string ParserBadString = "ParserBadString";

		// Token: 0x04001853 RID: 6227
		internal const string ParserBadSyncMode = "ParserBadSyncMode";

		// Token: 0x04001854 RID: 6228
		internal const string ParserBadTypeInArrayProperty = "ParserBadTypeInArrayProperty";

		// Token: 0x04001855 RID: 6229
		internal const string ParserBadUidOrNameME = "ParserBadUidOrNameME";

		// Token: 0x04001856 RID: 6230
		internal const string ParserBamlEvent = "ParserBamlEvent";

		// Token: 0x04001857 RID: 6231
		internal const string ParserBamlVersion = "ParserBamlVersion";

		// Token: 0x04001858 RID: 6232
		internal const string ParserCannotAddAnyChildren = "ParserCannotAddAnyChildren";

		// Token: 0x04001859 RID: 6233
		internal const string ParserCannotAddAnyChildren2 = "ParserCannotAddAnyChildren2";

		// Token: 0x0400185A RID: 6234
		internal const string ParserCannotAddChild = "ParserCannotAddChild";

		// Token: 0x0400185B RID: 6235
		internal const string ParserCannotConvertInitializationText = "ParserCannotConvertInitializationText";

		// Token: 0x0400185C RID: 6236
		internal const string ParserCannotConvertPropertyValue = "ParserCannotConvertPropertyValue";

		// Token: 0x0400185D RID: 6237
		internal const string ParserCannotConvertPropertyValueString = "ParserCannotConvertPropertyValueString";

		// Token: 0x0400185E RID: 6238
		internal const string ParserCannotConvertString = "ParserCannotConvertString";

		// Token: 0x0400185F RID: 6239
		internal const string ParserCannotReuseXamlReader = "ParserCannotReuseXamlReader";

		// Token: 0x04001860 RID: 6240
		internal const string ParserCannotSetValue = "ParserCannotSetValue";

		// Token: 0x04001861 RID: 6241
		internal const string ParserCanOnlyHaveOneChild = "ParserCanOnlyHaveOneChild";

		// Token: 0x04001862 RID: 6242
		internal const string ParserCantCreateDelegate = "ParserCantCreateDelegate";

		// Token: 0x04001863 RID: 6243
		internal const string ParserCantCreateInstanceType = "ParserCantCreateInstanceType";

		// Token: 0x04001864 RID: 6244
		internal const string ParserCantCreateTextComplexProp = "ParserCantCreateTextComplexProp";

		// Token: 0x04001865 RID: 6245
		internal const string ParserCantGetDPOrPi = "ParserCantGetDPOrPi";

		// Token: 0x04001866 RID: 6246
		internal const string ParserCantGetProperty = "ParserCantGetProperty";

		// Token: 0x04001867 RID: 6247
		internal const string ParserCantSetAttribute = "ParserCantSetAttribute";

		// Token: 0x04001868 RID: 6248
		internal const string ParserCantSetContentProperty = "ParserCantSetContentProperty";

		// Token: 0x04001869 RID: 6249
		internal const string ParserCantSetTriggerCondition = "ParserCantSetTriggerCondition";

		// Token: 0x0400186A RID: 6250
		internal const string ParserCompatDuplicate = "ParserCompatDuplicate";

		// Token: 0x0400186B RID: 6251
		internal const string ParserContentMustBeContiguous = "ParserContentMustBeContiguous";

		// Token: 0x0400186C RID: 6252
		internal const string ParserDefaultConverterElement = "ParserDefaultConverterElement";

		// Token: 0x0400186D RID: 6253
		internal const string ParserDefaultConverterProperty = "ParserDefaultConverterProperty";

		// Token: 0x0400186E RID: 6254
		internal const string ParserDeferContentAsync = "ParserDeferContentAsync";

		// Token: 0x0400186F RID: 6255
		internal const string ParserDefSharedOnlyInCompiled = "ParserDefSharedOnlyInCompiled";

		// Token: 0x04001870 RID: 6256
		internal const string ParserDefTag = "ParserDefTag";

		// Token: 0x04001871 RID: 6257
		internal const string ParserDictionarySealed = "ParserDictionarySealed";

		// Token: 0x04001872 RID: 6258
		internal const string ParserDupDictionaryKey = "ParserDupDictionaryKey";

		// Token: 0x04001873 RID: 6259
		internal const string ParserDuplicateMarkupExtensionProperty = "ParserDuplicateMarkupExtensionProperty";

		// Token: 0x04001874 RID: 6260
		internal const string ParserDuplicateProperty1 = "ParserDuplicateProperty1";

		// Token: 0x04001875 RID: 6261
		internal const string ParserDuplicateProperty2 = "ParserDuplicateProperty2";

		// Token: 0x04001876 RID: 6262
		internal const string ParserEmptyComplexProp = "ParserEmptyComplexProp";

		// Token: 0x04001877 RID: 6263
		internal const string ParserEntityReference = "ParserEntityReference";

		// Token: 0x04001878 RID: 6264
		internal const string ParserErrorContext_File = "ParserErrorContext_File";

		// Token: 0x04001879 RID: 6265
		internal const string ParserErrorContext_File_Line = "ParserErrorContext_File_Line";

		// Token: 0x0400187A RID: 6266
		internal const string ParserErrorContext_Line = "ParserErrorContext_Line";

		// Token: 0x0400187B RID: 6267
		internal const string ParserErrorContext_Type = "ParserErrorContext_Type";

		// Token: 0x0400187C RID: 6268
		internal const string ParserErrorContext_Type_File = "ParserErrorContext_Type_File";

		// Token: 0x0400187D RID: 6269
		internal const string ParserErrorContext_Type_File_Line = "ParserErrorContext_Type_File_Line";

		// Token: 0x0400187E RID: 6270
		internal const string ParserErrorContext_Type_Line = "ParserErrorContext_Type_Line";

		// Token: 0x0400187F RID: 6271
		internal const string ParserErrorCreatingInstance = "ParserErrorCreatingInstance";

		// Token: 0x04001880 RID: 6272
		internal const string ParserErrorParsingAttrib = "ParserErrorParsingAttrib";

		// Token: 0x04001881 RID: 6273
		internal const string ParserErrorParsingAttribType = "ParserErrorParsingAttribType";

		// Token: 0x04001882 RID: 6274
		internal const string ParserEventDelegateTypeNotAccessible = "ParserEventDelegateTypeNotAccessible";

		// Token: 0x04001883 RID: 6275
		internal const string ParserFailedEndInit = "ParserFailedEndInit";

		// Token: 0x04001884 RID: 6276
		internal const string ParserFailedToCreateFromConstructor = "ParserFailedToCreateFromConstructor";

		// Token: 0x04001885 RID: 6277
		internal const string ParserFailFindType = "ParserFailFindType";

		// Token: 0x04001886 RID: 6278
		internal const string ParserFilterXmlReaderNoDefinitionPrefixChangeAllowed = "ParserFilterXmlReaderNoDefinitionPrefixChangeAllowed";

		// Token: 0x04001887 RID: 6279
		internal const string ParserFilterXmlReaderNoIndexAttributeAccess = "ParserFilterXmlReaderNoIndexAttributeAccess";

		// Token: 0x04001888 RID: 6280
		internal const string ParserIAddChildText = "ParserIAddChildText";

		// Token: 0x04001889 RID: 6281
		internal const string ParserIEnumerableIAddChild = "ParserIEnumerableIAddChild";

		// Token: 0x0400188A RID: 6282
		internal const string ParserInvalidContentPropertyAttribute = "ParserInvalidContentPropertyAttribute";

		// Token: 0x0400188B RID: 6283
		internal const string ParserInvalidIdentifierName = "ParserInvalidIdentifierName";

		// Token: 0x0400188C RID: 6284
		internal const string ParserInvalidStaticMember = "ParserInvalidStaticMember";

		// Token: 0x0400188D RID: 6285
		internal const string ParserKeyOnExplicitDictionary = "ParserKeyOnExplicitDictionary";

		// Token: 0x0400188E RID: 6286
		internal const string ParserKeysAreStrings = "ParserKeysAreStrings";

		// Token: 0x0400188F RID: 6287
		internal const string ParserLineAndOffset = "ParserLineAndOffset";

		// Token: 0x04001890 RID: 6288
		internal const string ParserMapPIMissingAssembly = "ParserMapPIMissingAssembly";

		// Token: 0x04001891 RID: 6289
		internal const string ParserMapPIMissingKey = "ParserMapPIMissingKey";

		// Token: 0x04001892 RID: 6290
		internal const string ParserMappingUriInvalid = "ParserMappingUriInvalid";

		// Token: 0x04001893 RID: 6291
		internal const string ParserMarkupExtensionBadConstructorParam = "ParserMarkupExtensionBadConstructorParam";

		// Token: 0x04001894 RID: 6292
		internal const string ParserMarkupExtensionBadDelimiter = "ParserMarkupExtensionBadDelimiter";

		// Token: 0x04001895 RID: 6293
		internal const string ParserMarkupExtensionDelimiterBeforeFirstAttribute = "ParserMarkupExtensionDelimiterBeforeFirstAttribute";

		// Token: 0x04001896 RID: 6294
		internal const string ParserMarkupExtensionInvalidClosingBracketCharacers = "ParserMarkupExtensionInvalidClosingBracketCharacers";

		// Token: 0x04001897 RID: 6295
		internal const string ParserMarkupExtensionMalformedBracketCharacers = "ParserMarkupExtensionMalformedBracketCharacers";

		// Token: 0x04001898 RID: 6296
		internal const string ParserMarkupExtensionNoEndCurlie = "ParserMarkupExtensionNoEndCurlie";

		// Token: 0x04001899 RID: 6297
		internal const string ParserMarkupExtensionNoNameValue = "ParserMarkupExtensionNoNameValue";

		// Token: 0x0400189A RID: 6298
		internal const string ParserMarkupExtensionNoQuotesInName = "ParserMarkupExtensionNoQuotesInName";

		// Token: 0x0400189B RID: 6299
		internal const string ParserMarkupExtensionTrailingGarbage = "ParserMarkupExtensionTrailingGarbage";

		// Token: 0x0400189C RID: 6300
		internal const string ParserMarkupExtensionUnknownAttr = "ParserMarkupExtensionUnknownAttr";

		// Token: 0x0400189D RID: 6301
		internal const string ParserMetroUnknownAttribute = "ParserMetroUnknownAttribute";

		// Token: 0x0400189E RID: 6302
		internal const string ParserMultiBamls = "ParserMultiBamls";

		// Token: 0x0400189F RID: 6303
		internal const string ParserMultiRoot = "ParserMultiRoot";

		// Token: 0x040018A0 RID: 6304
		internal const string ParserNestedComplexProp = "ParserNestedComplexProp";

		// Token: 0x040018A1 RID: 6305
		internal const string ParserNoAttrArray = "ParserNoAttrArray";

		// Token: 0x040018A2 RID: 6306
		internal const string ParserNoChildrenTag = "ParserNoChildrenTag";

		// Token: 0x040018A3 RID: 6307
		internal const string ParserNoComplexMulti = "ParserNoComplexMulti";

		// Token: 0x040018A4 RID: 6308
		internal const string ParserNoDefaultConstructor = "ParserNoDefaultConstructor";

		// Token: 0x040018A5 RID: 6309
		internal const string ParserNoDefaultPropConstructor = "ParserNoDefaultPropConstructor";

		// Token: 0x040018A6 RID: 6310
		internal const string ParserNoDictionaryKey = "ParserNoDictionaryKey";

		// Token: 0x040018A7 RID: 6311
		internal const string ParserNoDictionaryName = "ParserNoDictionaryName";

		// Token: 0x040018A8 RID: 6312
		internal const string ParserNoDigitEnums = "ParserNoDigitEnums";

		// Token: 0x040018A9 RID: 6313
		internal const string ParserNoDPOnOwner = "ParserNoDPOnOwner";

		// Token: 0x040018AA RID: 6314
		internal const string ParserNoElementCreate2 = "ParserNoElementCreate2";

		// Token: 0x040018AB RID: 6315
		internal const string ParserNoEvents = "ParserNoEvents";

		// Token: 0x040018AC RID: 6316
		internal const string ParserNoEventTag = "ParserNoEventTag";

		// Token: 0x040018AD RID: 6317
		internal const string ParserNoMatchingArray = "ParserNoMatchingArray";

		// Token: 0x040018AE RID: 6318
		internal const string ParserNoMatchingIDictionary = "ParserNoMatchingIDictionary";

		// Token: 0x040018AF RID: 6319
		internal const string ParserNoMatchingIList = "ParserNoMatchingIList";

		// Token: 0x040018B0 RID: 6320
		internal const string ParserNoNameOnType = "ParserNoNameOnType";

		// Token: 0x040018B1 RID: 6321
		internal const string ParserNoNamespace = "ParserNoNamespace";

		// Token: 0x040018B2 RID: 6322
		internal const string ParserNoNameUnderDefinitionScopeType = "ParserNoNameUnderDefinitionScopeType";

		// Token: 0x040018B3 RID: 6323
		internal const string ParserNoNestedXmlDataIslands = "ParserNoNestedXmlDataIslands";

		// Token: 0x040018B4 RID: 6324
		internal const string ParserNoPropOnComplexProp = "ParserNoPropOnComplexProp";

		// Token: 0x040018B5 RID: 6325
		internal const string ParserNoPropType = "ParserNoPropType";

		// Token: 0x040018B6 RID: 6326
		internal const string ParserNoResource = "ParserNoResource";

		// Token: 0x040018B7 RID: 6327
		internal const string ParserNoSerializer = "ParserNoSerializer";

		// Token: 0x040018B8 RID: 6328
		internal const string ParserNoSetterChild = "ParserNoSetterChild";

		// Token: 0x040018B9 RID: 6329
		internal const string ParserNotAllowedInternalType = "ParserNotAllowedInternalType";

		// Token: 0x040018BA RID: 6330
		internal const string ParserNotMarkedPublic = "ParserNotMarkedPublic";

		// Token: 0x040018BB RID: 6331
		internal const string ParserNotMarkupExtension = "ParserNotMarkupExtension";

		// Token: 0x040018BC RID: 6332
		internal const string ParserNoType = "ParserNoType";

		// Token: 0x040018BD RID: 6333
		internal const string ParserNoTypeConv = "ParserNoTypeConv";

		// Token: 0x040018BE RID: 6334
		internal const string ParserNullPropertyCollection = "ParserNullPropertyCollection";

		// Token: 0x040018BF RID: 6335
		internal const string ParserNullReturned = "ParserNullReturned";

		// Token: 0x040018C0 RID: 6336
		internal const string ParserOwnerEventMustBePublic = "ParserOwnerEventMustBePublic";

		// Token: 0x040018C1 RID: 6337
		internal const string ParserParentDO = "ParserParentDO";

		// Token: 0x040018C2 RID: 6338
		internal const string ParserPrefixNSElement = "ParserPrefixNSElement";

		// Token: 0x040018C3 RID: 6339
		internal const string ParserPrefixNSProperty = "ParserPrefixNSProperty";

		// Token: 0x040018C4 RID: 6340
		internal const string ParserPropertyCollectionClosed = "ParserPropertyCollectionClosed";

		// Token: 0x040018C5 RID: 6341
		internal const string ParserPropNoValue = "ParserPropNoValue";

		// Token: 0x040018C6 RID: 6342
		internal const string ParserProvideValueCantSetUri = "ParserProvideValueCantSetUri";

		// Token: 0x040018C7 RID: 6343
		internal const string ParserPublicType = "ParserPublicType";

		// Token: 0x040018C8 RID: 6344
		internal const string ParserReadOnlyNullProperty = "ParserReadOnlyNullProperty";

		// Token: 0x040018C9 RID: 6345
		internal const string ParserReadOnlyProp = "ParserReadOnlyProp";

		// Token: 0x040018CA RID: 6346
		internal const string ParserResourceKeyType = "ParserResourceKeyType";

		// Token: 0x040018CB RID: 6347
		internal const string ParserSyncOnRoot = "ParserSyncOnRoot";

		// Token: 0x040018CC RID: 6348
		internal const string ParserTextInComplexProp = "ParserTextInComplexProp";

		// Token: 0x040018CD RID: 6349
		internal const string ParserTextInvalidInArrayOrDictionary = "ParserTextInvalidInArrayOrDictionary";

		// Token: 0x040018CE RID: 6350
		internal const string ParserTooManyAssemblies = "ParserTooManyAssemblies";

		// Token: 0x040018CF RID: 6351
		internal const string ParserTypeConverterTextNeedsEndElement = "ParserTypeConverterTextNeedsEndElement";

		// Token: 0x040018D0 RID: 6352
		internal const string ParserTypeConverterTextUnusable = "ParserTypeConverterTextUnusable";

		// Token: 0x040018D1 RID: 6353
		internal const string ParserUndeclaredNS = "ParserUndeclaredNS";

		// Token: 0x040018D2 RID: 6354
		internal const string ParserUnexpectedEndEle = "ParserUnexpectedEndEle";

		// Token: 0x040018D3 RID: 6355
		internal const string ParserUnexpInBAML = "ParserUnexpInBAML";

		// Token: 0x040018D4 RID: 6356
		internal const string ParserUnknownAttribute = "ParserUnknownAttribute";

		// Token: 0x040018D5 RID: 6357
		internal const string ParserUnknownBaml = "ParserUnknownBaml";

		// Token: 0x040018D6 RID: 6358
		internal const string ParserUnknownDefAttribute = "ParserUnknownDefAttribute";

		// Token: 0x040018D7 RID: 6359
		internal const string ParserUnknownDefAttributeCompiler = "ParserUnknownDefAttributeCompiler";

		// Token: 0x040018D8 RID: 6360
		internal const string ParserUnknownPresentationOptionsAttribute = "ParserUnknownPresentationOptionsAttribute";

		// Token: 0x040018D9 RID: 6361
		internal const string ParserUnknownTag = "ParserUnknownTag";

		// Token: 0x040018DA RID: 6362
		internal const string ParserUnknownXmlType = "ParserUnknownXmlType";

		// Token: 0x040018DB RID: 6363
		internal const string ParserWriterNoSeekEnd = "ParserWriterNoSeekEnd";

		// Token: 0x040018DC RID: 6364
		internal const string ParserWriterUnknownOrigin = "ParserWriterUnknownOrigin";

		// Token: 0x040018DD RID: 6365
		internal const string ParserXmlIslandMissing = "ParserXmlIslandMissing";

		// Token: 0x040018DE RID: 6366
		internal const string ParserXmlIslandUnexpected = "ParserXmlIslandUnexpected";

		// Token: 0x040018DF RID: 6367
		internal const string ParserXmlLangPropertyValueInvalid = "ParserXmlLangPropertyValueInvalid";

		// Token: 0x040018E0 RID: 6368
		internal const string ParserXmlReaderNoLineInfo = "ParserXmlReaderNoLineInfo";

		// Token: 0x040018E1 RID: 6369
		internal const string PartialTrustPrintDialogMustBeInvoked = "PartialTrustPrintDialogMustBeInvoked";

		// Token: 0x040018E2 RID: 6370
		internal const string PasswordBoxInvalidTextContainer = "PasswordBoxInvalidTextContainer";

		// Token: 0x040018E3 RID: 6371
		internal const string PathParameterIsNull = "PathParameterIsNull";

		// Token: 0x040018E4 RID: 6372
		internal const string PathParametersIndexOutOfRange = "PathParametersIndexOutOfRange";

		// Token: 0x040018E5 RID: 6373
		internal const string PathSyntax = "PathSyntax";

		// Token: 0x040018E6 RID: 6374
		internal const string PlatformRequirementTitle = "PlatformRequirementTitle";

		// Token: 0x040018E7 RID: 6375
		internal const string PopupReopeningNotAllowed = "PopupReopeningNotAllowed";

		// Token: 0x040018E8 RID: 6376
		internal const string PositionalArgumentsWrongLength = "PositionalArgumentsWrongLength";

		// Token: 0x040018E9 RID: 6377
		internal const string PrevoiusPartialPageContentOutstanding = "PrevoiusPartialPageContentOutstanding";

		// Token: 0x040018EA RID: 6378
		internal const string PrevoiusUninitializedDocumentReferenceOutstanding = "PrevoiusUninitializedDocumentReferenceOutstanding";

		// Token: 0x040018EB RID: 6379
		internal const string PrintDialogInstallPrintSupportCaption = "PrintDialogInstallPrintSupportCaption";

		// Token: 0x040018EC RID: 6380
		internal const string PrintDialogInstallPrintSupportMessageBox = "PrintDialogInstallPrintSupportMessageBox";

		// Token: 0x040018ED RID: 6381
		internal const string PrintDialogInvalidPageRange = "PrintDialogInvalidPageRange";

		// Token: 0x040018EE RID: 6382
		internal const string PrintDialogPageRange = "PrintDialogPageRange";

		// Token: 0x040018EF RID: 6383
		internal const string PrintDialogZeroNotAllowed = "PrintDialogZeroNotAllowed";

		// Token: 0x040018F0 RID: 6384
		internal const string PrintJobDescription = "PrintJobDescription";

		// Token: 0x040018F1 RID: 6385
		internal const string ProgressBarReadOnly = "ProgressBarReadOnly";

		// Token: 0x040018F2 RID: 6386
		internal const string PropertyFoundOutsideStartElement = "PropertyFoundOutsideStartElement";

		// Token: 0x040018F3 RID: 6387
		internal const string PropertyIdOutOfSequence = "PropertyIdOutOfSequence";

		// Token: 0x040018F4 RID: 6388
		internal const string PropertyIsImmutable = "PropertyIsImmutable";

		// Token: 0x040018F5 RID: 6389
		internal const string PropertyIsInitializeOnly = "PropertyIsInitializeOnly";

		// Token: 0x040018F6 RID: 6390
		internal const string PropertyMustHaveValue = "PropertyMustHaveValue";

		// Token: 0x040018F7 RID: 6391
		internal const string PropertyNotBindable = "PropertyNotBindable";

		// Token: 0x040018F8 RID: 6392
		internal const string PropertyNotFound = "PropertyNotFound";

		// Token: 0x040018F9 RID: 6393
		internal const string PropertyNotSupported = "PropertyNotSupported";

		// Token: 0x040018FA RID: 6394
		internal const string PropertyOutOfOrder = "PropertyOutOfOrder";

		// Token: 0x040018FB RID: 6395
		internal const string PropertyPathIndexWrongType = "PropertyPathIndexWrongType";

		// Token: 0x040018FC RID: 6396
		internal const string PropertyPathInvalidAccessor = "PropertyPathInvalidAccessor";

		// Token: 0x040018FD RID: 6397
		internal const string PropertyPathNoOwnerType = "PropertyPathNoOwnerType";

		// Token: 0x040018FE RID: 6398
		internal const string PropertyPathNoProperty = "PropertyPathNoProperty";

		// Token: 0x040018FF RID: 6399
		internal const string PropertyPathSyntaxError = "PropertyPathSyntaxError";

		// Token: 0x04001900 RID: 6400
		internal const string PropertyToSortByNotFoundOnType = "PropertyToSortByNotFoundOnType";

		// Token: 0x04001901 RID: 6401
		internal const string PropertyTriggerCycleDetected = "PropertyTriggerCycleDetected";

		// Token: 0x04001902 RID: 6402
		internal const string PropertyTriggerLayerLimitExceeded = "PropertyTriggerLayerLimitExceeded";

		// Token: 0x04001903 RID: 6403
		internal const string PTSError = "PTSError";

		// Token: 0x04001904 RID: 6404
		internal const string PTSInvalidHandle = "PTSInvalidHandle";

		// Token: 0x04001905 RID: 6405
		internal const string RangeActionsNotSupported = "RangeActionsNotSupported";

		// Token: 0x04001906 RID: 6406
		internal const string ReadCountNegative = "ReadCountNegative";

		// Token: 0x04001907 RID: 6407
		internal const string ReadNotSupported = "ReadNotSupported";

		// Token: 0x04001908 RID: 6408
		internal const string ReadOnlyPropertyNotAllowed = "ReadOnlyPropertyNotAllowed";

		// Token: 0x04001909 RID: 6409
		internal const string RecordOutOfOrder = "RecordOutOfOrder";

		// Token: 0x0400190A RID: 6410
		internal const string Rect_WidthAndHeightCannotBeNegative = "Rect_WidthAndHeightCannotBeNegative";

		// Token: 0x0400190B RID: 6411
		internal const string RelativeSourceInvalidAncestorLevel = "RelativeSourceInvalidAncestorLevel";

		// Token: 0x0400190C RID: 6412
		internal const string RelativeSourceModeInvalid = "RelativeSourceModeInvalid";

		// Token: 0x0400190D RID: 6413
		internal const string RelativeSourceModeIsImmutable = "RelativeSourceModeIsImmutable";

		// Token: 0x0400190E RID: 6414
		internal const string RelativeSourceNeedsAncestorType = "RelativeSourceNeedsAncestorType";

		// Token: 0x0400190F RID: 6415
		internal const string RelativeSourceNeedsMode = "RelativeSourceNeedsMode";

		// Token: 0x04001910 RID: 6416
		internal const string RelativeSourceNotInFindAncestorMode = "RelativeSourceNotInFindAncestorMode";

		// Token: 0x04001911 RID: 6417
		internal const string RemovedItemNotFound = "RemovedItemNotFound";

		// Token: 0x04001912 RID: 6418
		internal const string RemoveRequiresOffsetZero = "RemoveRequiresOffsetZero";

		// Token: 0x04001913 RID: 6419
		internal const string RemoveRequiresPositiveCount = "RemoveRequiresPositiveCount";

		// Token: 0x04001914 RID: 6420
		internal const string RemovingPlaceholder = "RemovingPlaceholder";

		// Token: 0x04001915 RID: 6421
		internal const string ReparentModelChildIllegal = "ReparentModelChildIllegal";

		// Token: 0x04001916 RID: 6422
		internal const string RequestNavigateEventMustHaveRoutedEvent = "RequestNavigateEventMustHaveRoutedEvent";

		// Token: 0x04001917 RID: 6423
		internal const string RequiredAttributeMissing = "RequiredAttributeMissing";

		// Token: 0x04001918 RID: 6424
		internal const string RequiresExplicitCulture = "RequiresExplicitCulture";

		// Token: 0x04001919 RID: 6425
		internal const string RequiresXmlNamespaceMapping = "RequiresXmlNamespaceMapping";

		// Token: 0x0400191A RID: 6426
		internal const string RequiresXmlNamespaceMappingUri = "RequiresXmlNamespaceMappingUri";

		// Token: 0x0400191B RID: 6427
		internal const string ReshowNotAllowed = "ReshowNotAllowed";

		// Token: 0x0400191C RID: 6428
		internal const string ResourceDictionaryDeferredContentFailure = "ResourceDictionaryDeferredContentFailure";

		// Token: 0x0400191D RID: 6429
		internal const string ResourceDictionaryDuplicateDeferredContent = "ResourceDictionaryDuplicateDeferredContent";

		// Token: 0x0400191E RID: 6430
		internal const string ResourceDictionaryInvalidMergedDictionary = "ResourceDictionaryInvalidMergedDictionary";

		// Token: 0x0400191F RID: 6431
		internal const string ResourceDictionaryIsReadOnly = "ResourceDictionaryIsReadOnly";

		// Token: 0x04001920 RID: 6432
		internal const string ResourceDictionaryLoadFromFailure = "ResourceDictionaryLoadFromFailure";

		// Token: 0x04001921 RID: 6433
		internal const string ReturnEventHandlerMustBeOnParentPage = "ReturnEventHandlerMustBeOnParentPage";

		// Token: 0x04001922 RID: 6434
		internal const string RichTextBox_CantSetDocumentInsideChangeBlock = "RichTextBox_CantSetDocumentInsideChangeBlock";

		// Token: 0x04001923 RID: 6435
		internal const string RichTextBox_DocumentBelongsToAnotherRichTextBoxAlready = "RichTextBox_DocumentBelongsToAnotherRichTextBoxAlready";

		// Token: 0x04001924 RID: 6436
		internal const string RichTextBox_PointerNotInSameDocument = "RichTextBox_PointerNotInSameDocument";

		// Token: 0x04001925 RID: 6437
		internal const string RowCacheCannotModifyNonExistentLayout = "RowCacheCannotModifyNonExistentLayout";

		// Token: 0x04001926 RID: 6438
		internal const string RowCachePageNotFound = "RowCachePageNotFound";

		// Token: 0x04001927 RID: 6439
		internal const string RowCacheRecalcWithNoPageCache = "RowCacheRecalcWithNoPageCache";

		// Token: 0x04001928 RID: 6440
		internal const string RuntimeTypeRequired = "RuntimeTypeRequired";

		// Token: 0x04001929 RID: 6441
		internal const string ScrollBar_ContextMenu_Bottom = "ScrollBar_ContextMenu_Bottom";

		// Token: 0x0400192A RID: 6442
		internal const string ScrollBar_ContextMenu_LeftEdge = "ScrollBar_ContextMenu_LeftEdge";

		// Token: 0x0400192B RID: 6443
		internal const string ScrollBar_ContextMenu_PageDown = "ScrollBar_ContextMenu_PageDown";

		// Token: 0x0400192C RID: 6444
		internal const string ScrollBar_ContextMenu_PageLeft = "ScrollBar_ContextMenu_PageLeft";

		// Token: 0x0400192D RID: 6445
		internal const string ScrollBar_ContextMenu_PageRight = "ScrollBar_ContextMenu_PageRight";

		// Token: 0x0400192E RID: 6446
		internal const string ScrollBar_ContextMenu_PageUp = "ScrollBar_ContextMenu_PageUp";

		// Token: 0x0400192F RID: 6447
		internal const string ScrollBar_ContextMenu_RightEdge = "ScrollBar_ContextMenu_RightEdge";

		// Token: 0x04001930 RID: 6448
		internal const string ScrollBar_ContextMenu_ScrollDown = "ScrollBar_ContextMenu_ScrollDown";

		// Token: 0x04001931 RID: 6449
		internal const string ScrollBar_ContextMenu_ScrollHere = "ScrollBar_ContextMenu_ScrollHere";

		// Token: 0x04001932 RID: 6450
		internal const string ScrollBar_ContextMenu_ScrollLeft = "ScrollBar_ContextMenu_ScrollLeft";

		// Token: 0x04001933 RID: 6451
		internal const string ScrollBar_ContextMenu_ScrollRight = "ScrollBar_ContextMenu_ScrollRight";

		// Token: 0x04001934 RID: 6452
		internal const string ScrollBar_ContextMenu_ScrollUp = "ScrollBar_ContextMenu_ScrollUp";

		// Token: 0x04001935 RID: 6453
		internal const string ScrollBar_ContextMenu_Top = "ScrollBar_ContextMenu_Top";

		// Token: 0x04001936 RID: 6454
		internal const string ScrollViewer_CannotBeNaN = "ScrollViewer_CannotBeNaN";

		// Token: 0x04001937 RID: 6455
		internal const string ScrollViewer_OutOfRange = "ScrollViewer_OutOfRange";

		// Token: 0x04001938 RID: 6456
		internal const string SeekFailed = "SeekFailed";

		// Token: 0x04001939 RID: 6457
		internal const string SeekNegative = "SeekNegative";

		// Token: 0x0400193A RID: 6458
		internal const string SeekNotSupported = "SeekNotSupported";

		// Token: 0x0400193B RID: 6459
		internal const string SelectedCellsCollection_DuplicateItem = "SelectedCellsCollection_DuplicateItem";

		// Token: 0x0400193C RID: 6460
		internal const string SelectedCellsCollection_InvalidItem = "SelectedCellsCollection_InvalidItem";

		// Token: 0x0400193D RID: 6461
		internal const string SelectionChangeActive = "SelectionChangeActive";

		// Token: 0x0400193E RID: 6462
		internal const string SelectionChangeNotActive = "SelectionChangeNotActive";

		// Token: 0x0400193F RID: 6463
		internal const string SelectionDoesNotResolveToAPage = "SelectionDoesNotResolveToAPage";

		// Token: 0x04001940 RID: 6464
		internal const string SelectionMustBeServiceProvider = "SelectionMustBeServiceProvider";

		// Token: 0x04001941 RID: 6465
		internal const string SerializerProviderAlreadyRegistered = "SerializerProviderAlreadyRegistered";

		// Token: 0x04001942 RID: 6466
		internal const string SerializerProviderCannotLoad = "SerializerProviderCannotLoad";

		// Token: 0x04001943 RID: 6467
		internal const string SerializerProviderDefaultFileExtensionNull = "SerializerProviderDefaultFileExtensionNull";

		// Token: 0x04001944 RID: 6468
		internal const string SerializerProviderDisplayNameNull = "SerializerProviderDisplayNameNull";

		// Token: 0x04001945 RID: 6469
		internal const string SerializerProviderManufacturerNameNull = "SerializerProviderManufacturerNameNull";

		// Token: 0x04001946 RID: 6470
		internal const string SerializerProviderManufacturerWebsiteNull = "SerializerProviderManufacturerWebsiteNull";

		// Token: 0x04001947 RID: 6471
		internal const string SerializerProviderNotRegistered = "SerializerProviderNotRegistered";

		// Token: 0x04001948 RID: 6472
		internal const string SerializerProviderUnknownSerializer = "SerializerProviderUnknownSerializer";

		// Token: 0x04001949 RID: 6473
		internal const string SerializerProviderWrongVersion = "SerializerProviderWrongVersion";

		// Token: 0x0400194A RID: 6474
		internal const string SetFocusFailed = "SetFocusFailed";

		// Token: 0x0400194B RID: 6475
		internal const string SetInDeferSelectionActive = "SetInDeferSelectionActive";

		// Token: 0x0400194C RID: 6476
		internal const string SetLengthNotSupported = "SetLengthNotSupported";

		// Token: 0x0400194D RID: 6477
		internal const string SetPositionNotSupported = "SetPositionNotSupported";

		// Token: 0x0400194E RID: 6478
		internal const string SetterOnStyleNotAllowedToHaveTarget = "SetterOnStyleNotAllowedToHaveTarget";

		// Token: 0x0400194F RID: 6479
		internal const string SetterValueCannotBeUnset = "SetterValueCannotBeUnset";

		// Token: 0x04001950 RID: 6480
		internal const string SetterValueOfMarkupExtensionNotSupported = "SetterValueOfMarkupExtensionNotSupported";

		// Token: 0x04001951 RID: 6481
		internal const string SharedAttributeInLooseXaml = "SharedAttributeInLooseXaml";

		// Token: 0x04001952 RID: 6482
		internal const string ShowDialogOnModal = "ShowDialogOnModal";

		// Token: 0x04001953 RID: 6483
		internal const string ShowDialogOnVisible = "ShowDialogOnVisible";

		// Token: 0x04001954 RID: 6484
		internal const string ShowNonActivatedAndMaximized = "ShowNonActivatedAndMaximized";

		// Token: 0x04001955 RID: 6485
		internal const string ShutdownModeWhenAppShutdown = "ShutdownModeWhenAppShutdown";

		// Token: 0x04001956 RID: 6486
		internal const string SourceNameNotSupportedForDataTriggers = "SourceNameNotSupportedForDataTriggers";

		// Token: 0x04001957 RID: 6487
		internal const string SourceNameNotSupportedForStyleTriggers = "SourceNameNotSupportedForStyleTriggers";

		// Token: 0x04001958 RID: 6488
		internal const string Stack_VisualInDifferentSubTree = "Stack_VisualInDifferentSubTree";

		// Token: 0x04001959 RID: 6489
		internal const string StartIndexExceedsBufferSize = "StartIndexExceedsBufferSize";

		// Token: 0x0400195A RID: 6490
		internal const string StartNodeMustBeDocumentPageViewOrFixedPage = "StartNodeMustBeDocumentPageViewOrFixedPage";

		// Token: 0x0400195B RID: 6491
		internal const string StartNodeMustBeFixedPageProxy = "StartNodeMustBeFixedPageProxy";

		// Token: 0x0400195C RID: 6492
		internal const string StaticResourceInXamlOnly = "StaticResourceInXamlOnly";

		// Token: 0x0400195D RID: 6493
		internal const string Storyboard_AnimationMismatch = "Storyboard_AnimationMismatch";

		// Token: 0x0400195E RID: 6494
		internal const string Storyboard_BeginStoryboardNameNotFound = "Storyboard_BeginStoryboardNameNotFound";

		// Token: 0x0400195F RID: 6495
		internal const string Storyboard_BeginStoryboardNameRequired = "Storyboard_BeginStoryboardNameRequired";

		// Token: 0x04001960 RID: 6496
		internal const string Storyboard_BeginStoryboardNoStoryboard = "Storyboard_BeginStoryboardNoStoryboard";

		// Token: 0x04001961 RID: 6497
		internal const string Storyboard_ComplexPathNotSupported = "Storyboard_ComplexPathNotSupported";

		// Token: 0x04001962 RID: 6498
		internal const string Storyboard_ImmutableTargetNotSupported = "Storyboard_ImmutableTargetNotSupported";

		// Token: 0x04001963 RID: 6499
		internal const string Storyboard_MediaElementNotFound = "Storyboard_MediaElementNotFound";

		// Token: 0x04001964 RID: 6500
		internal const string Storyboard_MediaElementRequired = "Storyboard_MediaElementRequired";

		// Token: 0x04001965 RID: 6501
		internal const string Storyboard_NameNotFound = "Storyboard_NameNotFound";

		// Token: 0x04001966 RID: 6502
		internal const string Storyboard_NeverApplied = "Storyboard_NeverApplied";

		// Token: 0x04001967 RID: 6503
		internal const string Storyboard_NoNameScope = "Storyboard_NoNameScope";

		// Token: 0x04001968 RID: 6504
		internal const string Storyboard_NoTarget = "Storyboard_NoTarget";

		// Token: 0x04001969 RID: 6505
		internal const string Storyboard_PropertyPathEmpty = "Storyboard_PropertyPathEmpty";

		// Token: 0x0400196A RID: 6506
		internal const string Storyboard_PropertyPathFrozenCheckFailed = "Storyboard_PropertyPathFrozenCheckFailed";

		// Token: 0x0400196B RID: 6507
		internal const string Storyboard_PropertyPathIncludesNonAnimatableProperty = "Storyboard_PropertyPathIncludesNonAnimatableProperty";

		// Token: 0x0400196C RID: 6508
		internal const string Storyboard_PropertyPathMustPointToDependencyObject = "Storyboard_PropertyPathMustPointToDependencyObject";

		// Token: 0x0400196D RID: 6509
		internal const string Storyboard_PropertyPathMustPointToDependencyProperty = "Storyboard_PropertyPathMustPointToDependencyProperty";

		// Token: 0x0400196E RID: 6510
		internal const string Storyboard_PropertyPathObjectNotFound = "Storyboard_PropertyPathObjectNotFound";

		// Token: 0x0400196F RID: 6511
		internal const string Storyboard_PropertyPathPropertyNotFound = "Storyboard_PropertyPathPropertyNotFound";

		// Token: 0x04001970 RID: 6512
		internal const string Storyboard_PropertyPathSealedCheckFailed = "Storyboard_PropertyPathSealedCheckFailed";

		// Token: 0x04001971 RID: 6513
		internal const string Storyboard_PropertyPathUnresolved = "Storyboard_PropertyPathUnresolved";

		// Token: 0x04001972 RID: 6514
		internal const string Storyboard_StoryboardReferenceRequired = "Storyboard_StoryboardReferenceRequired";

		// Token: 0x04001973 RID: 6515
		internal const string Storyboard_TargetNameNotAllowedInStyle = "Storyboard_TargetNameNotAllowedInStyle";

		// Token: 0x04001974 RID: 6516
		internal const string Storyboard_TargetNameNotDependencyObject = "Storyboard_TargetNameNotDependencyObject";

		// Token: 0x04001975 RID: 6517
		internal const string Storyboard_TargetPropertyRequired = "Storyboard_TargetPropertyRequired";

		// Token: 0x04001976 RID: 6518
		internal const string Storyboard_UnableToFreeze = "Storyboard_UnableToFreeze";

		// Token: 0x04001977 RID: 6519
		internal const string Storyboard_UnrecognizedHandoffBehavior = "Storyboard_UnrecognizedHandoffBehavior";

		// Token: 0x04001978 RID: 6520
		internal const string Storyboard_UnrecognizedTimeSeekOrigin = "Storyboard_UnrecognizedTimeSeekOrigin";

		// Token: 0x04001979 RID: 6521
		internal const string StreamCannotBeWritten = "StreamCannotBeWritten";

		// Token: 0x0400197A RID: 6522
		internal const string StreamDoesNotSupportSeek = "StreamDoesNotSupportSeek";

		// Token: 0x0400197B RID: 6523
		internal const string StreamDoesNotSupportWrite = "StreamDoesNotSupportWrite";

		// Token: 0x0400197C RID: 6524
		internal const string StreamNotSet = "StreamNotSet";

		// Token: 0x0400197D RID: 6525
		internal const string StreamObjectDisposed = "StreamObjectDisposed";

		// Token: 0x0400197E RID: 6526
		internal const string StringIdOutOfSequence = "StringIdOutOfSequence";

		// Token: 0x0400197F RID: 6527
		internal const string StyleBasedOnHasLoop = "StyleBasedOnHasLoop";

		// Token: 0x04001980 RID: 6528
		internal const string StyleCannotBeBasedOnSelf = "StyleCannotBeBasedOnSelf";

		// Token: 0x04001981 RID: 6529
		internal const string StyleDataTriggerBindingHasBadValue = "StyleDataTriggerBindingHasBadValue";

		// Token: 0x04001982 RID: 6530
		internal const string StyleDataTriggerBindingMissing = "StyleDataTriggerBindingMissing";

		// Token: 0x04001983 RID: 6531
		internal const string StyleForWrongType = "StyleForWrongType";

		// Token: 0x04001984 RID: 6532
		internal const string StyleHasTooManyElements = "StyleHasTooManyElements";

		// Token: 0x04001985 RID: 6533
		internal const string StyleImpliedAndComplexChildren = "StyleImpliedAndComplexChildren";

		// Token: 0x04001986 RID: 6534
		internal const string StyleInvalidElementTag = "StyleInvalidElementTag";

		// Token: 0x04001987 RID: 6535
		internal const string StyleKnownTagWrongLocation = "StyleKnownTagWrongLocation";

		// Token: 0x04001988 RID: 6536
		internal const string StyleNoClrEvent = "StyleNoClrEvent";

		// Token: 0x04001989 RID: 6537
		internal const string StyleNoDef = "StyleNoDef";

		// Token: 0x0400198A RID: 6538
		internal const string StyleNoDictionaryKey = "StyleNoDictionaryKey";

		// Token: 0x0400198B RID: 6539
		internal const string StyleNoEventSetters = "StyleNoEventSetters";

		// Token: 0x0400198C RID: 6540
		internal const string StyleNoPropOrEvent = "StyleNoPropOrEvent";

		// Token: 0x0400198D RID: 6541
		internal const string StyleNoSetterResource = "StyleNoSetterResource";

		// Token: 0x0400198E RID: 6542
		internal const string StyleNoTarget = "StyleNoTarget";

		// Token: 0x0400198F RID: 6543
		internal const string StyleNoTemplateBindInSetters = "StyleNoTemplateBindInSetters";

		// Token: 0x04001990 RID: 6544
		internal const string StyleNoTemplateBindInVisualTrigger = "StyleNoTemplateBindInVisualTrigger";

		// Token: 0x04001991 RID: 6545
		internal const string StyleNoTopLevelElement = "StyleNoTopLevelElement";

		// Token: 0x04001992 RID: 6546
		internal const string StylePropertyCustom = "StylePropertyCustom";

		// Token: 0x04001993 RID: 6547
		internal const string StylePropertyInStyleNotAllowed = "StylePropertyInStyleNotAllowed";

		// Token: 0x04001994 RID: 6548
		internal const string StylePropertySetterMinAttrs = "StylePropertySetterMinAttrs";

		// Token: 0x04001995 RID: 6549
		internal const string StylePropTriggerPropMissing = "StylePropTriggerPropMissing";

		// Token: 0x04001996 RID: 6550
		internal const string StyleSetterUnknownProp = "StyleSetterUnknownProp";

		// Token: 0x04001997 RID: 6551
		internal const string StyleTagNotSupported = "StyleTagNotSupported";

		// Token: 0x04001998 RID: 6552
		internal const string StyleTargetTypeMismatchWithElement = "StyleTargetTypeMismatchWithElement";

		// Token: 0x04001999 RID: 6553
		internal const string StyleTextNotSupported = "StyleTextNotSupported";

		// Token: 0x0400199A RID: 6554
		internal const string StyleTriggersCannotTargetTheTemplate = "StyleTriggersCannotTargetTheTemplate";

		// Token: 0x0400199B RID: 6555
		internal const string StyleUnknownProp = "StyleUnknownProp";

		// Token: 0x0400199C RID: 6556
		internal const string StyleUnknownTrigger = "StyleUnknownTrigger";

		// Token: 0x0400199D RID: 6557
		internal const string StyleValueOfExpressionNotSupported = "StyleValueOfExpressionNotSupported";

		// Token: 0x0400199E RID: 6558
		internal const string SystemResourceForTypeIsNotStyle = "SystemResourceForTypeIsNotStyle";

		// Token: 0x0400199F RID: 6559
		internal const string TableCollectionCountNeedNonNegNum = "TableCollectionCountNeedNonNegNum";

		// Token: 0x040019A0 RID: 6560
		internal const string TableCollectionElementTypeExpected = "TableCollectionElementTypeExpected";

		// Token: 0x040019A1 RID: 6561
		internal const string TableCollectionInOtherCollection = "TableCollectionInOtherCollection";

		// Token: 0x040019A2 RID: 6562
		internal const string TableCollectionInvalidOffLen = "TableCollectionInvalidOffLen";

		// Token: 0x040019A3 RID: 6563
		internal const string TableCollectionNotEnoughCapacity = "TableCollectionNotEnoughCapacity";

		// Token: 0x040019A4 RID: 6564
		internal const string TableCollectionOutOfRange = "TableCollectionOutOfRange";

		// Token: 0x040019A5 RID: 6565
		internal const string TableCollectionOutOfRangeNeedNonNegNum = "TableCollectionOutOfRangeNeedNonNegNum";

		// Token: 0x040019A6 RID: 6566
		internal const string TableCollectionRangeOutOfRange = "TableCollectionRangeOutOfRange";

		// Token: 0x040019A7 RID: 6567
		internal const string TableCollectionRankMultiDimNotSupported = "TableCollectionRankMultiDimNotSupported";

		// Token: 0x040019A8 RID: 6568
		internal const string TableCollectionWrongProxyParent = "TableCollectionWrongProxyParent";

		// Token: 0x040019A9 RID: 6569
		internal const string TableInvalidParentNodeType = "TableInvalidParentNodeType";

		// Token: 0x040019AA RID: 6570
		internal const string TargetNameNotFound = "TargetNameNotFound";

		// Token: 0x040019AB RID: 6571
		internal const string TargetNameNotSupportedForStyleSetters = "TargetNameNotSupportedForStyleSetters";

		// Token: 0x040019AC RID: 6572
		internal const string Template3DValueOnly = "Template3DValueOnly";

		// Token: 0x040019AD RID: 6573
		internal const string TemplateBadDictionaryKey = "TemplateBadDictionaryKey";

		// Token: 0x040019AE RID: 6574
		internal const string TemplateCannotHaveNestedContentPresenterAndGridViewRowPresenter = "TemplateCannotHaveNestedContentPresenterAndGridViewRowPresenter";

		// Token: 0x040019AF RID: 6575
		internal const string TemplateChildIndexOutOfRange = "TemplateChildIndexOutOfRange";

		// Token: 0x040019B0 RID: 6576
		internal const string TemplateCircularReferenceFound = "TemplateCircularReferenceFound";

		// Token: 0x040019B1 RID: 6577
		internal const string TemplateContentSetTwice = "TemplateContentSetTwice";

		// Token: 0x040019B2 RID: 6578
		internal const string TemplateDupName = "TemplateDupName";

		// Token: 0x040019B3 RID: 6579
		internal const string TemplateFindNameInInvalidElement = "TemplateFindNameInInvalidElement";

		// Token: 0x040019B4 RID: 6580
		internal const string TemplateHasNestedNameScope = "TemplateHasNestedNameScope";

		// Token: 0x040019B5 RID: 6581
		internal const string TemplateInvalidBamlRecord = "TemplateInvalidBamlRecord";

		// Token: 0x040019B6 RID: 6582
		internal const string TemplateInvalidRootElementTag = "TemplateInvalidRootElementTag";

		// Token: 0x040019B7 RID: 6583
		internal const string TemplateKnownTagWrongLocation = "TemplateKnownTagWrongLocation";

		// Token: 0x040019B8 RID: 6584
		internal const string TemplateMustBeFE = "TemplateMustBeFE";

		// Token: 0x040019B9 RID: 6585
		internal const string TemplateNoMultipleRoots = "TemplateNoMultipleRoots";

		// Token: 0x040019BA RID: 6586
		internal const string TemplateNoProp = "TemplateNoProp";

		// Token: 0x040019BB RID: 6587
		internal const string TemplateNoTarget = "TemplateNoTarget";

		// Token: 0x040019BC RID: 6588
		internal const string TemplateNoTemplateBindInVisualTrigger = "TemplateNoTemplateBindInVisualTrigger";

		// Token: 0x040019BD RID: 6589
		internal const string TemplateNoTriggerTarget = "TemplateNoTriggerTarget";

		// Token: 0x040019BE RID: 6590
		internal const string TemplateNotTargetType = "TemplateNotTargetType";

		// Token: 0x040019BF RID: 6591
		internal const string TemplateTagNotSupported = "TemplateTagNotSupported";

		// Token: 0x040019C0 RID: 6592
		internal const string TemplateTargetTypeMismatch = "TemplateTargetTypeMismatch";

		// Token: 0x040019C1 RID: 6593
		internal const string TemplateTextNotSupported = "TemplateTextNotSupported";

		// Token: 0x040019C2 RID: 6594
		internal const string TemplateUnknownProp = "TemplateUnknownProp";

		// Token: 0x040019C3 RID: 6595
		internal const string TextBoxBase_CantSetIsUndoEnabledInsideChangeBlock = "TextBoxBase_CantSetIsUndoEnabledInsideChangeBlock";

		// Token: 0x040019C4 RID: 6596
		internal const string TextBoxBase_UnmatchedEndChange = "TextBoxBase_UnmatchedEndChange";

		// Token: 0x040019C5 RID: 6597
		internal const string TextBoxDecoratorMarkedAsTextBoxContentMustHaveNoContent = "TextBoxDecoratorMarkedAsTextBoxContentMustHaveNoContent";

		// Token: 0x040019C6 RID: 6598
		internal const string TextBoxInvalidChild = "TextBoxInvalidChild";

		// Token: 0x040019C7 RID: 6599
		internal const string TextBoxInvalidTextContainer = "TextBoxInvalidTextContainer";

		// Token: 0x040019C8 RID: 6600
		internal const string TextBoxMinMaxLinesMismatch = "TextBoxMinMaxLinesMismatch";

		// Token: 0x040019C9 RID: 6601
		internal const string TextBoxScrollViewerMarkedAsTextBoxContentMustHaveNoContent = "TextBoxScrollViewerMarkedAsTextBoxContentMustHaveNoContent";

		// Token: 0x040019CA RID: 6602
		internal const string TextBox_ContextMenu_Copy = "TextBox_ContextMenu_Copy";

		// Token: 0x040019CB RID: 6603
		internal const string TextBox_ContextMenu_Cut = "TextBox_ContextMenu_Cut";

		// Token: 0x040019CC RID: 6604
		internal const string TextBox_ContextMenu_Description_DBCSSpace = "TextBox_ContextMenu_Description_DBCSSpace";

		// Token: 0x040019CD RID: 6605
		internal const string TextBox_ContextMenu_Description_SBCSSpace = "TextBox_ContextMenu_Description_SBCSSpace";

		// Token: 0x040019CE RID: 6606
		internal const string TextBox_ContextMenu_IgnoreAll = "TextBox_ContextMenu_IgnoreAll";

		// Token: 0x040019CF RID: 6607
		internal const string TextBox_ContextMenu_More = "TextBox_ContextMenu_More";

		// Token: 0x040019D0 RID: 6608
		internal const string TextBox_ContextMenu_NoSpellingSuggestions = "TextBox_ContextMenu_NoSpellingSuggestions";

		// Token: 0x040019D1 RID: 6609
		internal const string TextBox_ContextMenu_Paste = "TextBox_ContextMenu_Paste";

		// Token: 0x040019D2 RID: 6610
		internal const string TextContainerChangingReentrancyInvalid = "TextContainerChangingReentrancyInvalid";

		// Token: 0x040019D3 RID: 6611
		internal const string TextContainerDoesNotContainElement = "TextContainerDoesNotContainElement";

		// Token: 0x040019D4 RID: 6612
		internal const string TextContainer_UndoManagerCreatedMoreThanOnce = "TextContainer_UndoManagerCreatedMoreThanOnce";

		// Token: 0x040019D5 RID: 6613
		internal const string TextEditorCanNotRegisterCommandHandler = "TextEditorCanNotRegisterCommandHandler";

		// Token: 0x040019D6 RID: 6614
		internal const string TextEditorCopyPaste_EntryPartIsMissingInXamlPackage = "TextEditorCopyPaste_EntryPartIsMissingInXamlPackage";

		// Token: 0x040019D7 RID: 6615
		internal const string TextEditorPropertyIsNotApplicableForTextFormatting = "TextEditorPropertyIsNotApplicableForTextFormatting";

		// Token: 0x040019D8 RID: 6616
		internal const string TextEditorSpellerInteropHasBeenDisposed = "TextEditorSpellerInteropHasBeenDisposed";

		// Token: 0x040019D9 RID: 6617
		internal const string TextEditorTypeOfParameterIsNotAppropriateForFormattingProperty = "TextEditorTypeOfParameterIsNotAppropriateForFormattingProperty";

		// Token: 0x040019DA RID: 6618
		internal const string TextElementCollection_CannotCopyToArrayNotSufficientMemory = "TextElementCollection_CannotCopyToArrayNotSufficientMemory";

		// Token: 0x040019DB RID: 6619
		internal const string TextElementCollection_IndexOutOfRange = "TextElementCollection_IndexOutOfRange";

		// Token: 0x040019DC RID: 6620
		internal const string TextElementCollection_ItemHasUnexpectedType = "TextElementCollection_ItemHasUnexpectedType";

		// Token: 0x040019DD RID: 6621
		internal const string TextElementCollection_NextSiblingDoesNotBelongToThisCollection = "TextElementCollection_NextSiblingDoesNotBelongToThisCollection";

		// Token: 0x040019DE RID: 6622
		internal const string TextElementCollection_NoEnumerator = "TextElementCollection_NoEnumerator";

		// Token: 0x040019DF RID: 6623
		internal const string TextElementCollection_PreviousSiblingDoesNotBelongToThisCollection = "TextElementCollection_PreviousSiblingDoesNotBelongToThisCollection";

		// Token: 0x040019E0 RID: 6624
		internal const string TextElementCollection_TextElementTypeExpected = "TextElementCollection_TextElementTypeExpected";

		// Token: 0x040019E1 RID: 6625
		internal const string TextElement_UnmatchedEndPointer = "TextElement_UnmatchedEndPointer";

		// Token: 0x040019E2 RID: 6626
		internal const string TextPanelIllegalParaTypeForIAddChild = "TextPanelIllegalParaTypeForIAddChild";

		// Token: 0x040019E3 RID: 6627
		internal const string TextPointer_CannotInsertTextElementBecauseItBelongsToAnotherTree = "TextPointer_CannotInsertTextElementBecauseItBelongsToAnotherTree";

		// Token: 0x040019E4 RID: 6628
		internal const string TextPositionIsFrozen = "TextPositionIsFrozen";

		// Token: 0x040019E5 RID: 6629
		internal const string TextProvider_InvalidChildElement = "TextProvider_InvalidChildElement";

		// Token: 0x040019E6 RID: 6630
		internal const string TextProvider_InvalidPoint = "TextProvider_InvalidPoint";

		// Token: 0x040019E7 RID: 6631
		internal const string TextProvider_TextSelectionNotSupported = "TextProvider_TextSelectionNotSupported";

		// Token: 0x040019E8 RID: 6632
		internal const string TextRangeEdit_InvalidStructuralPropertyApply = "TextRangeEdit_InvalidStructuralPropertyApply";

		// Token: 0x040019E9 RID: 6633
		internal const string TextRangeProvider_EmptyStringParameter = "TextRangeProvider_EmptyStringParameter";

		// Token: 0x040019EA RID: 6634
		internal const string TextRangeProvider_InvalidParameterValue = "TextRangeProvider_InvalidParameterValue";

		// Token: 0x040019EB RID: 6635
		internal const string TextRangeProvider_WrongTextRange = "TextRangeProvider_WrongTextRange";

		// Token: 0x040019EC RID: 6636
		internal const string TextRange_InvalidParameterValue = "TextRange_InvalidParameterValue";

		// Token: 0x040019ED RID: 6637
		internal const string TextRange_PropertyCannotBeIncrementedOrDecremented = "TextRange_PropertyCannotBeIncrementedOrDecremented";

		// Token: 0x040019EE RID: 6638
		internal const string TextRange_UnrecognizedStructureInDataFormat = "TextRange_UnrecognizedStructureInDataFormat";

		// Token: 0x040019EF RID: 6639
		internal const string TextRange_UnsupportedDataFormat = "TextRange_UnsupportedDataFormat";

		// Token: 0x040019F0 RID: 6640
		internal const string TextSchema_CannotInsertContentInThisPosition = "TextSchema_CannotInsertContentInThisPosition";

		// Token: 0x040019F1 RID: 6641
		internal const string TextSchema_CannotSplitElement = "TextSchema_CannotSplitElement";

		// Token: 0x040019F2 RID: 6642
		internal const string TextSchema_ChildTypeIsInvalid = "TextSchema_ChildTypeIsInvalid";

		// Token: 0x040019F3 RID: 6643
		internal const string TextSchema_IllegalElement = "TextSchema_IllegalElement";

		// Token: 0x040019F4 RID: 6644
		internal const string TextSchema_IllegalHyperlinkChild = "TextSchema_IllegalHyperlinkChild";

		// Token: 0x040019F5 RID: 6645
		internal const string TextSchema_TextIsNotAllowed = "TextSchema_TextIsNotAllowed";

		// Token: 0x040019F6 RID: 6646
		internal const string TextSchema_TextIsNotAllowedInThisContext = "TextSchema_TextIsNotAllowedInThisContext";

		// Token: 0x040019F7 RID: 6647
		internal const string TextSchema_TheChildElementBelongsToAnotherTreeAlready = "TextSchema_TheChildElementBelongsToAnotherTreeAlready";

		// Token: 0x040019F8 RID: 6648
		internal const string TextSchema_ThisBlockUIContainerHasAChildUIElementAlready = "TextSchema_ThisBlockUIContainerHasAChildUIElementAlready";

		// Token: 0x040019F9 RID: 6649
		internal const string TextSchema_ThisInlineUIContainerHasAChildUIElementAlready = "TextSchema_ThisInlineUIContainerHasAChildUIElementAlready";

		// Token: 0x040019FA RID: 6650
		internal const string TextSchema_UIElementNotAllowedInThisPosition = "TextSchema_UIElementNotAllowedInThisPosition";

		// Token: 0x040019FB RID: 6651
		internal const string TextSegmentsMustNotOverlap = "TextSegmentsMustNotOverlap";

		// Token: 0x040019FC RID: 6652
		internal const string TextStore_BadIMECharOffset = "TextStore_BadIMECharOffset";

		// Token: 0x040019FD RID: 6653
		internal const string TextStore_BadLockFlags = "TextStore_BadLockFlags";

		// Token: 0x040019FE RID: 6654
		internal const string TextStore_CompositionRejected = "TextStore_CompositionRejected";

		// Token: 0x040019FF RID: 6655
		internal const string TextStore_CONNECT_E_CANNOTCONNECT = "TextStore_CONNECT_E_CANNOTCONNECT";

		// Token: 0x04001A00 RID: 6656
		internal const string TextStore_CONNECT_E_NOCONNECTION = "TextStore_CONNECT_E_NOCONNECTION";

		// Token: 0x04001A01 RID: 6657
		internal const string TextStore_E_NOINTERFACE = "TextStore_E_NOINTERFACE";

		// Token: 0x04001A02 RID: 6658
		internal const string TextStore_E_NOTIMPL = "TextStore_E_NOTIMPL";

		// Token: 0x04001A03 RID: 6659
		internal const string TextStore_NoSink = "TextStore_NoSink";

		// Token: 0x04001A04 RID: 6660
		internal const string TextStore_ReentrantRequestLock = "TextStore_ReentrantRequestLock";

		// Token: 0x04001A05 RID: 6661
		internal const string TextStore_TS_E_FORMAT = "TextStore_TS_E_FORMAT";

		// Token: 0x04001A06 RID: 6662
		internal const string TextStore_TS_E_INVALIDPOINT = "TextStore_TS_E_INVALIDPOINT";

		// Token: 0x04001A07 RID: 6663
		internal const string TextStore_TS_E_NOLAYOUT = "TextStore_TS_E_NOLAYOUT";

		// Token: 0x04001A08 RID: 6664
		internal const string TextStore_TS_E_READONLY = "TextStore_TS_E_READONLY";

		// Token: 0x04001A09 RID: 6665
		internal const string TextViewInvalidLayout = "TextViewInvalidLayout";

		// Token: 0x04001A0A RID: 6666
		internal const string ThemeDictionaryExtension_Name = "ThemeDictionaryExtension_Name";

		// Token: 0x04001A0B RID: 6667
		internal const string ThemeDictionaryExtension_Source = "ThemeDictionaryExtension_Source";

		// Token: 0x04001A0C RID: 6668
		internal const string ToolBar_InvalidStyle_ToolBarOverflowPanel = "ToolBar_InvalidStyle_ToolBarOverflowPanel";

		// Token: 0x04001A0D RID: 6669
		internal const string ToolBar_InvalidStyle_ToolBarPanel = "ToolBar_InvalidStyle_ToolBarPanel";

		// Token: 0x04001A0E RID: 6670
		internal const string ToolTipStaysOpenFalseNotAllowed = "ToolTipStaysOpenFalseNotAllowed";

		// Token: 0x04001A0F RID: 6671
		internal const string ToStringFormatString_Control = "ToStringFormatString_Control";

		// Token: 0x04001A10 RID: 6672
		internal const string ToStringFormatString_GridView = "ToStringFormatString_GridView";

		// Token: 0x04001A11 RID: 6673
		internal const string ToStringFormatString_GridViewColumn = "ToStringFormatString_GridViewColumn";

		// Token: 0x04001A12 RID: 6674
		internal const string ToStringFormatString_GridViewRowPresenter = "ToStringFormatString_GridViewRowPresenter";

		// Token: 0x04001A13 RID: 6675
		internal const string ToStringFormatString_GridViewRowPresenterBase = "ToStringFormatString_GridViewRowPresenterBase";

		// Token: 0x04001A14 RID: 6676
		internal const string ToStringFormatString_HeaderedContentControl = "ToStringFormatString_HeaderedContentControl";

		// Token: 0x04001A15 RID: 6677
		internal const string ToStringFormatString_HeaderedItemsControl = "ToStringFormatString_HeaderedItemsControl";

		// Token: 0x04001A16 RID: 6678
		internal const string ToStringFormatString_ItemsControl = "ToStringFormatString_ItemsControl";

		// Token: 0x04001A17 RID: 6679
		internal const string ToStringFormatString_RangeBase = "ToStringFormatString_RangeBase";

		// Token: 0x04001A18 RID: 6680
		internal const string ToStringFormatString_ToggleButton = "ToStringFormatString_ToggleButton";

		// Token: 0x04001A19 RID: 6681
		internal const string Track_SameButtons = "Track_SameButtons";

		// Token: 0x04001A1A RID: 6682
		internal const string TransformNotSupported = "TransformNotSupported";

		// Token: 0x04001A1B RID: 6683
		internal const string TriggerActionAlreadySealed = "TriggerActionAlreadySealed";

		// Token: 0x04001A1C RID: 6684
		internal const string TriggerActionMustBelongToASingleTrigger = "TriggerActionMustBelongToASingleTrigger";

		// Token: 0x04001A1D RID: 6685
		internal const string TriggerOnStyleNotAllowedToHaveSource = "TriggerOnStyleNotAllowedToHaveSource";

		// Token: 0x04001A1E RID: 6686
		internal const string TriggersSupportsEventTriggersOnly = "TriggersSupportsEventTriggersOnly";

		// Token: 0x04001A1F RID: 6687
		internal const string TrustNotGrantedText = "TrustNotGrantedText";

		// Token: 0x04001A20 RID: 6688
		internal const string TrustNotGrantedTitle = "TrustNotGrantedTitle";

		// Token: 0x04001A21 RID: 6689
		internal const string TwoWayBindingNeedsPath = "TwoWayBindingNeedsPath";

		// Token: 0x04001A22 RID: 6690
		internal const string TypeIdOutOfSequence = "TypeIdOutOfSequence";

		// Token: 0x04001A23 RID: 6691
		internal const string TypeMustImplementIAddChild = "TypeMustImplementIAddChild";

		// Token: 0x04001A24 RID: 6692
		internal const string TypeNameMustBeSpecified = "TypeNameMustBeSpecified";

		// Token: 0x04001A25 RID: 6693
		internal const string TypeValueSerializerUnavailable = "TypeValueSerializerUnavailable";

		// Token: 0x04001A26 RID: 6694
		internal const string UIA_OperationCannotBePerformed = "UIA_OperationCannotBePerformed";

		// Token: 0x04001A27 RID: 6695
		internal const string UiLessPageFunctionNotCallingOnReturn = "UiLessPageFunctionNotCallingOnReturn";

		// Token: 0x04001A28 RID: 6696
		internal const string UnableToConvertInt32 = "UnableToConvertInt32";

		// Token: 0x04001A29 RID: 6697
		internal const string UnableToLocateResource = "UnableToLocateResource";

		// Token: 0x04001A2A RID: 6698
		internal const string UndefinedHighlightAnchor = "UndefinedHighlightAnchor";

		// Token: 0x04001A2B RID: 6699
		internal const string UndoContainerTypeMismatch = "UndoContainerTypeMismatch";

		// Token: 0x04001A2C RID: 6700
		internal const string UndoManagerAlreadyAttached = "UndoManagerAlreadyAttached";

		// Token: 0x04001A2D RID: 6701
		internal const string UndoNoOpenParentUnit = "UndoNoOpenParentUnit";

		// Token: 0x04001A2E RID: 6702
		internal const string UndoNoOpenUnit = "UndoNoOpenUnit";

		// Token: 0x04001A2F RID: 6703
		internal const string UndoNotInNormalState = "UndoNotInNormalState";

		// Token: 0x04001A30 RID: 6704
		internal const string UndoServiceDisabled = "UndoServiceDisabled";

		// Token: 0x04001A31 RID: 6705
		internal const string UndoUnitAlreadyOpen = "UndoUnitAlreadyOpen";

		// Token: 0x04001A32 RID: 6706
		internal const string UndoUnitCantBeAddedTwice = "UndoUnitCantBeAddedTwice";

		// Token: 0x04001A33 RID: 6707
		internal const string UndoUnitCantBeOpenedTwice = "UndoUnitCantBeOpenedTwice";

		// Token: 0x04001A34 RID: 6708
		internal const string UndoUnitLocked = "UndoUnitLocked";

		// Token: 0x04001A35 RID: 6709
		internal const string UndoUnitNotFound = "UndoUnitNotFound";

		// Token: 0x04001A36 RID: 6710
		internal const string UndoUnitNotOnTopOfStack = "UndoUnitNotOnTopOfStack";

		// Token: 0x04001A37 RID: 6711
		internal const string UndoUnitOpen = "UndoUnitOpen";

		// Token: 0x04001A38 RID: 6712
		internal const string UnexpectedAttribute = "UnexpectedAttribute";

		// Token: 0x04001A39 RID: 6713
		internal const string UnexpectedCollectionChangeAction = "UnexpectedCollectionChangeAction";

		// Token: 0x04001A3A RID: 6714
		internal const string UnexpectedProperty = "UnexpectedProperty";

		// Token: 0x04001A3B RID: 6715
		internal const string UnexpectedType = "UnexpectedType";

		// Token: 0x04001A3C RID: 6716
		internal const string UnexpectedValueTypeForCondition = "UnexpectedValueTypeForCondition";

		// Token: 0x04001A3D RID: 6717
		internal const string UnexpectedValueTypeForDataTrigger = "UnexpectedValueTypeForDataTrigger";

		// Token: 0x04001A3E RID: 6718
		internal const string UnexpectedXmlNodeInXmlFixedPageInfoConstructor = "UnexpectedXmlNodeInXmlFixedPageInfoConstructor";

		// Token: 0x04001A3F RID: 6719
		internal const string UnknownBamlRecord = "UnknownBamlRecord";

		// Token: 0x04001A40 RID: 6720
		internal const string UnknownContainerFormat = "UnknownContainerFormat";

		// Token: 0x04001A41 RID: 6721
		internal const string UnknownErrorText = "UnknownErrorText";

		// Token: 0x04001A42 RID: 6722
		internal const string UnknownErrorTitle = "UnknownErrorTitle";

		// Token: 0x04001A43 RID: 6723
		internal const string UnknownIndexType = "UnknownIndexType";

		// Token: 0x04001A44 RID: 6724
		internal const string UnmatchedBracket = "UnmatchedBracket";

		// Token: 0x04001A45 RID: 6725
		internal const string UnmatchedLocComment = "UnmatchedLocComment";

		// Token: 0x04001A46 RID: 6726
		internal const string UnmatchedParen = "UnmatchedParen";

		// Token: 0x04001A47 RID: 6727
		internal const string UnRecognizedBamlNodeType = "UnRecognizedBamlNodeType";

		// Token: 0x04001A48 RID: 6728
		internal const string UnserializableKeyValue = "UnserializableKeyValue";

		// Token: 0x04001A49 RID: 6729
		internal const string UnsupportedTriggerInStyle = "UnsupportedTriggerInStyle";

		// Token: 0x04001A4A RID: 6730
		internal const string UnsupportedTriggerInTemplate = "UnsupportedTriggerInTemplate";

		// Token: 0x04001A4B RID: 6731
		internal const string Untitled = "Untitled";

		// Token: 0x04001A4C RID: 6732
		internal const string UntitledPrintJobDescription = "UntitledPrintJobDescription";

		// Token: 0x04001A4D RID: 6733
		internal const string UriNotMatchWithRootType = "UriNotMatchWithRootType";

		// Token: 0x04001A4E RID: 6734
		internal const string ValidationRule_UnexpectedValue = "ValidationRule_UnexpectedValue";

		// Token: 0x04001A4F RID: 6735
		internal const string ValidationRule_UnknownStep = "ValidationRule_UnknownStep";

		// Token: 0x04001A50 RID: 6736
		internal const string Validation_ConversionFailed = "Validation_ConversionFailed";

		// Token: 0x04001A51 RID: 6737
		internal const string ValueMustBeXamlReader = "ValueMustBeXamlReader";

		// Token: 0x04001A52 RID: 6738
		internal const string ValueNotBetweenInt32MinMax = "ValueNotBetweenInt32MinMax";

		// Token: 0x04001A53 RID: 6739
		internal const string ValueSerializerContextUnavailable = "ValueSerializerContextUnavailable";

		// Token: 0x04001A54 RID: 6740
		internal const string VirtualizedCellInfoCollection_DoesNotSupportIndexChanges = "VirtualizedCellInfoCollection_DoesNotSupportIndexChanges";

		// Token: 0x04001A55 RID: 6741
		internal const string VirtualizedCellInfoCollection_IsReadOnly = "VirtualizedCellInfoCollection_IsReadOnly";

		// Token: 0x04001A56 RID: 6742
		internal const string VirtualizedElement = "VirtualizedElement";

		// Token: 0x04001A57 RID: 6743
		internal const string VisualTreeRootIsFrameworkElement = "VisualTreeRootIsFrameworkElement";

		// Token: 0x04001A58 RID: 6744
		internal const string VisualTriggerSettersIncludeUnsupportedSetterType = "VisualTriggerSettersIncludeUnsupportedSetterType";

		// Token: 0x04001A59 RID: 6745
		internal const string WebBrowserNoCastToIWebBrowser2 = "WebBrowserNoCastToIWebBrowser2";

		// Token: 0x04001A5A RID: 6746
		internal const string WebBrowserOverlap = "WebBrowserOverlap";

		// Token: 0x04001A5B RID: 6747
		internal const string WebRequestCreationFailed = "WebRequestCreationFailed";

		// Token: 0x04001A5C RID: 6748
		internal const string WindowAlreadyClosed = "WindowAlreadyClosed";

		// Token: 0x04001A5D RID: 6749
		internal const string WindowMustBeRoot = "WindowMustBeRoot";

		// Token: 0x04001A5E RID: 6750
		internal const string WindowPassedShouldBeOnApplicationThread = "WindowPassedShouldBeOnApplicationThread";

		// Token: 0x04001A5F RID: 6751
		internal const string WpfPayload_InvalidImageSource = "WpfPayload_InvalidImageSource";

		// Token: 0x04001A60 RID: 6752
		internal const string WriteNotSupported = "WriteNotSupported";

		// Token: 0x04001A61 RID: 6753
		internal const string WrongNavigateRootElement = "WrongNavigateRootElement";

		// Token: 0x04001A62 RID: 6754
		internal const string WrongSelectionType = "WrongSelectionType";

		// Token: 0x04001A63 RID: 6755
		internal const string XamlFilterNestedFixedPage = "XamlFilterNestedFixedPage";

		// Token: 0x04001A64 RID: 6756
		internal const string XmlGlyphRunInfoIsNonGraphic = "XmlGlyphRunInfoIsNonGraphic";

		// Token: 0x04001A65 RID: 6757
		internal const string XmlNodeAlreadyOwned = "XmlNodeAlreadyOwned";

		// Token: 0x04001A66 RID: 6758
		internal const string XpsValidatingLoaderDiscardControlHasIncorrectType = "XpsValidatingLoaderDiscardControlHasIncorrectType";

		// Token: 0x04001A67 RID: 6759
		internal const string XpsValidatingLoaderDuplicateReference = "XpsValidatingLoaderDuplicateReference";

		// Token: 0x04001A68 RID: 6760
		internal const string XpsValidatingLoaderMoreThanOneDiscardControlInPackage = "XpsValidatingLoaderMoreThanOneDiscardControlInPackage";

		// Token: 0x04001A69 RID: 6761
		internal const string XpsValidatingLoaderMoreThanOnePrintTicketPart = "XpsValidatingLoaderMoreThanOnePrintTicketPart";

		// Token: 0x04001A6A RID: 6762
		internal const string XpsValidatingLoaderMoreThanOneThumbnailInPackage = "XpsValidatingLoaderMoreThanOneThumbnailInPackage";

		// Token: 0x04001A6B RID: 6763
		internal const string XpsValidatingLoaderMoreThanOneThumbnailPart = "XpsValidatingLoaderMoreThanOneThumbnailPart";

		// Token: 0x04001A6C RID: 6764
		internal const string XpsValidatingLoaderPrintTicketHasIncorrectType = "XpsValidatingLoaderPrintTicketHasIncorrectType";

		// Token: 0x04001A6D RID: 6765
		internal const string XpsValidatingLoaderRestrictedFontHasIncorrectType = "XpsValidatingLoaderRestrictedFontHasIncorrectType";

		// Token: 0x04001A6E RID: 6766
		internal const string XpsValidatingLoaderThumbnailHasIncorrectType = "XpsValidatingLoaderThumbnailHasIncorrectType";

		// Token: 0x04001A6F RID: 6767
		internal const string XpsValidatingLoaderUnlistedResource = "XpsValidatingLoaderUnlistedResource";

		// Token: 0x04001A70 RID: 6768
		internal const string XpsValidatingLoaderUnsupportedEncoding = "XpsValidatingLoaderUnsupportedEncoding";

		// Token: 0x04001A71 RID: 6769
		internal const string XpsValidatingLoaderUnsupportedMimeType = "XpsValidatingLoaderUnsupportedMimeType";

		// Token: 0x04001A72 RID: 6770
		internal const string XpsValidatingLoaderUnsupportedRootNamespaceUri = "XpsValidatingLoaderUnsupportedRootNamespaceUri";

		// Token: 0x04001A73 RID: 6771
		internal const string XpsValidatingLoaderUriNotInSamePackage = "XpsValidatingLoaderUriNotInSamePackage";

		// Token: 0x04001A74 RID: 6772
		internal const string Animation_ChildMustBeKeyFrame = "Animation_ChildMustBeKeyFrame";

		// Token: 0x04001A75 RID: 6773
		internal const string Animation_InvalidAnimationUsingKeyFramesDuration = "Animation_InvalidAnimationUsingKeyFramesDuration";

		// Token: 0x04001A76 RID: 6774
		internal const string Animation_InvalidBaseValue = "Animation_InvalidBaseValue";

		// Token: 0x04001A77 RID: 6775
		internal const string Animation_InvalidResolvedKeyTimes = "Animation_InvalidResolvedKeyTimes";

		// Token: 0x04001A78 RID: 6776
		internal const string Animation_InvalidTimeKeyTime = "Animation_InvalidTimeKeyTime";

		// Token: 0x04001A79 RID: 6777
		internal const string Animation_Invalid_DefaultValue = "Animation_Invalid_DefaultValue";

		// Token: 0x04001A7A RID: 6778
		internal const string Animation_NoTextChildren = "Animation_NoTextChildren";

		// Token: 0x04001A7B RID: 6779
		internal const string BrowserHostingNotSupported = "BrowserHostingNotSupported";

		// Token: 0x04001A7C RID: 6780
		internal const string CannotConvertStringToType = "CannotConvertStringToType";

		// Token: 0x04001A7D RID: 6781
		internal const string CannotConvertType = "CannotConvertType";

		// Token: 0x04001A7E RID: 6782
		internal const string CannotModifyReadOnlyContainer = "CannotModifyReadOnlyContainer";

		// Token: 0x04001A7F RID: 6783
		internal const string CannotRetrievePartsOfWriteOnlyContainer = "CannotRetrievePartsOfWriteOnlyContainer";

		// Token: 0x04001A80 RID: 6784
		internal const string CollectionNumberOfElementsMustBeLessOrEqualTo = "CollectionNumberOfElementsMustBeLessOrEqualTo";

		// Token: 0x04001A81 RID: 6785
		internal const string Collection_BadType = "Collection_BadType";

		// Token: 0x04001A82 RID: 6786
		internal const string Collection_CopyTo_ArrayCannotBeMultidimensional = "Collection_CopyTo_ArrayCannotBeMultidimensional";

		// Token: 0x04001A83 RID: 6787
		internal const string Collection_CopyTo_IndexGreaterThanOrEqualToArrayLength = "Collection_CopyTo_IndexGreaterThanOrEqualToArrayLength";

		// Token: 0x04001A84 RID: 6788
		internal const string Collection_CopyTo_NumberOfElementsExceedsArrayLength = "Collection_CopyTo_NumberOfElementsExceedsArrayLength";

		// Token: 0x04001A85 RID: 6789
		internal const string Enumerator_VerifyContext = "Enumerator_VerifyContext";

		// Token: 0x04001A86 RID: 6790
		internal const string Enum_Invalid = "Enum_Invalid";

		// Token: 0x04001A87 RID: 6791
		internal const string FileFormatException = "FileFormatException";

		// Token: 0x04001A88 RID: 6792
		internal const string FileFormatExceptionWithFileName = "FileFormatExceptionWithFileName";

		// Token: 0x04001A89 RID: 6793
		internal const string Freezable_CantBeFrozen = "Freezable_CantBeFrozen";

		// Token: 0x04001A8A RID: 6794
		internal const string InvalidPermissionStateValue = "InvalidPermissionStateValue";

		// Token: 0x04001A8B RID: 6795
		internal const string InvalidPermissionType = "InvalidPermissionType";

		// Token: 0x04001A8C RID: 6796
		internal const string ParameterCannotBeNegative = "ParameterCannotBeNegative";

		// Token: 0x04001A8D RID: 6797
		internal const string SecurityExceptionForSettingSandboxExternalToTrue = "SecurityExceptionForSettingSandboxExternalToTrue";

		// Token: 0x04001A8E RID: 6798
		internal const string StringEmpty = "StringEmpty";

		// Token: 0x04001A8F RID: 6799
		internal const string TokenizerHelperEmptyToken = "TokenizerHelperEmptyToken";

		// Token: 0x04001A90 RID: 6800
		internal const string TokenizerHelperExtraDataEncountered = "TokenizerHelperExtraDataEncountered";

		// Token: 0x04001A91 RID: 6801
		internal const string TokenizerHelperMissingEndQuote = "TokenizerHelperMissingEndQuote";

		// Token: 0x04001A92 RID: 6802
		internal const string TokenizerHelperPrematureStringTermination = "TokenizerHelperPrematureStringTermination";

		// Token: 0x04001A93 RID: 6803
		internal const string TypeMetadataCannotChangeAfterUse = "TypeMetadataCannotChangeAfterUse";

		// Token: 0x04001A94 RID: 6804
		internal const string UnexpectedParameterType = "UnexpectedParameterType";

		// Token: 0x04001A95 RID: 6805
		internal const string Visual_ArgumentOutOfRange = "Visual_ArgumentOutOfRange";
	}
}
