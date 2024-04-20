using System;

namespace Steamworks
{
	// Token: 0x02000158 RID: 344
	public enum EControllerActionOrigin
	{
		// Token: 0x04000627 RID: 1575
		k_EControllerActionOrigin_None,
		// Token: 0x04000628 RID: 1576
		k_EControllerActionOrigin_A,
		// Token: 0x04000629 RID: 1577
		k_EControllerActionOrigin_B,
		// Token: 0x0400062A RID: 1578
		k_EControllerActionOrigin_X,
		// Token: 0x0400062B RID: 1579
		k_EControllerActionOrigin_Y,
		// Token: 0x0400062C RID: 1580
		k_EControllerActionOrigin_LeftBumper,
		// Token: 0x0400062D RID: 1581
		k_EControllerActionOrigin_RightBumper,
		// Token: 0x0400062E RID: 1582
		k_EControllerActionOrigin_LeftGrip,
		// Token: 0x0400062F RID: 1583
		k_EControllerActionOrigin_RightGrip,
		// Token: 0x04000630 RID: 1584
		k_EControllerActionOrigin_Start,
		// Token: 0x04000631 RID: 1585
		k_EControllerActionOrigin_Back,
		// Token: 0x04000632 RID: 1586
		k_EControllerActionOrigin_LeftPad_Touch,
		// Token: 0x04000633 RID: 1587
		k_EControllerActionOrigin_LeftPad_Swipe,
		// Token: 0x04000634 RID: 1588
		k_EControllerActionOrigin_LeftPad_Click,
		// Token: 0x04000635 RID: 1589
		k_EControllerActionOrigin_LeftPad_DPadNorth,
		// Token: 0x04000636 RID: 1590
		k_EControllerActionOrigin_LeftPad_DPadSouth,
		// Token: 0x04000637 RID: 1591
		k_EControllerActionOrigin_LeftPad_DPadWest,
		// Token: 0x04000638 RID: 1592
		k_EControllerActionOrigin_LeftPad_DPadEast,
		// Token: 0x04000639 RID: 1593
		k_EControllerActionOrigin_RightPad_Touch,
		// Token: 0x0400063A RID: 1594
		k_EControllerActionOrigin_RightPad_Swipe,
		// Token: 0x0400063B RID: 1595
		k_EControllerActionOrigin_RightPad_Click,
		// Token: 0x0400063C RID: 1596
		k_EControllerActionOrigin_RightPad_DPadNorth,
		// Token: 0x0400063D RID: 1597
		k_EControllerActionOrigin_RightPad_DPadSouth,
		// Token: 0x0400063E RID: 1598
		k_EControllerActionOrigin_RightPad_DPadWest,
		// Token: 0x0400063F RID: 1599
		k_EControllerActionOrigin_RightPad_DPadEast,
		// Token: 0x04000640 RID: 1600
		k_EControllerActionOrigin_LeftTrigger_Pull,
		// Token: 0x04000641 RID: 1601
		k_EControllerActionOrigin_LeftTrigger_Click,
		// Token: 0x04000642 RID: 1602
		k_EControllerActionOrigin_RightTrigger_Pull,
		// Token: 0x04000643 RID: 1603
		k_EControllerActionOrigin_RightTrigger_Click,
		// Token: 0x04000644 RID: 1604
		k_EControllerActionOrigin_LeftStick_Move,
		// Token: 0x04000645 RID: 1605
		k_EControllerActionOrigin_LeftStick_Click,
		// Token: 0x04000646 RID: 1606
		k_EControllerActionOrigin_LeftStick_DPadNorth,
		// Token: 0x04000647 RID: 1607
		k_EControllerActionOrigin_LeftStick_DPadSouth,
		// Token: 0x04000648 RID: 1608
		k_EControllerActionOrigin_LeftStick_DPadWest,
		// Token: 0x04000649 RID: 1609
		k_EControllerActionOrigin_LeftStick_DPadEast,
		// Token: 0x0400064A RID: 1610
		k_EControllerActionOrigin_Gyro_Move,
		// Token: 0x0400064B RID: 1611
		k_EControllerActionOrigin_Gyro_Pitch,
		// Token: 0x0400064C RID: 1612
		k_EControllerActionOrigin_Gyro_Yaw,
		// Token: 0x0400064D RID: 1613
		k_EControllerActionOrigin_Gyro_Roll,
		// Token: 0x0400064E RID: 1614
		k_EControllerActionOrigin_PS4_X,
		// Token: 0x0400064F RID: 1615
		k_EControllerActionOrigin_PS4_Circle,
		// Token: 0x04000650 RID: 1616
		k_EControllerActionOrigin_PS4_Triangle,
		// Token: 0x04000651 RID: 1617
		k_EControllerActionOrigin_PS4_Square,
		// Token: 0x04000652 RID: 1618
		k_EControllerActionOrigin_PS4_LeftBumper,
		// Token: 0x04000653 RID: 1619
		k_EControllerActionOrigin_PS4_RightBumper,
		// Token: 0x04000654 RID: 1620
		k_EControllerActionOrigin_PS4_Options,
		// Token: 0x04000655 RID: 1621
		k_EControllerActionOrigin_PS4_Share,
		// Token: 0x04000656 RID: 1622
		k_EControllerActionOrigin_PS4_LeftPad_Touch,
		// Token: 0x04000657 RID: 1623
		k_EControllerActionOrigin_PS4_LeftPad_Swipe,
		// Token: 0x04000658 RID: 1624
		k_EControllerActionOrigin_PS4_LeftPad_Click,
		// Token: 0x04000659 RID: 1625
		k_EControllerActionOrigin_PS4_LeftPad_DPadNorth,
		// Token: 0x0400065A RID: 1626
		k_EControllerActionOrigin_PS4_LeftPad_DPadSouth,
		// Token: 0x0400065B RID: 1627
		k_EControllerActionOrigin_PS4_LeftPad_DPadWest,
		// Token: 0x0400065C RID: 1628
		k_EControllerActionOrigin_PS4_LeftPad_DPadEast,
		// Token: 0x0400065D RID: 1629
		k_EControllerActionOrigin_PS4_RightPad_Touch,
		// Token: 0x0400065E RID: 1630
		k_EControllerActionOrigin_PS4_RightPad_Swipe,
		// Token: 0x0400065F RID: 1631
		k_EControllerActionOrigin_PS4_RightPad_Click,
		// Token: 0x04000660 RID: 1632
		k_EControllerActionOrigin_PS4_RightPad_DPadNorth,
		// Token: 0x04000661 RID: 1633
		k_EControllerActionOrigin_PS4_RightPad_DPadSouth,
		// Token: 0x04000662 RID: 1634
		k_EControllerActionOrigin_PS4_RightPad_DPadWest,
		// Token: 0x04000663 RID: 1635
		k_EControllerActionOrigin_PS4_RightPad_DPadEast,
		// Token: 0x04000664 RID: 1636
		k_EControllerActionOrigin_PS4_CenterPad_Touch,
		// Token: 0x04000665 RID: 1637
		k_EControllerActionOrigin_PS4_CenterPad_Swipe,
		// Token: 0x04000666 RID: 1638
		k_EControllerActionOrigin_PS4_CenterPad_Click,
		// Token: 0x04000667 RID: 1639
		k_EControllerActionOrigin_PS4_CenterPad_DPadNorth,
		// Token: 0x04000668 RID: 1640
		k_EControllerActionOrigin_PS4_CenterPad_DPadSouth,
		// Token: 0x04000669 RID: 1641
		k_EControllerActionOrigin_PS4_CenterPad_DPadWest,
		// Token: 0x0400066A RID: 1642
		k_EControllerActionOrigin_PS4_CenterPad_DPadEast,
		// Token: 0x0400066B RID: 1643
		k_EControllerActionOrigin_PS4_LeftTrigger_Pull,
		// Token: 0x0400066C RID: 1644
		k_EControllerActionOrigin_PS4_LeftTrigger_Click,
		// Token: 0x0400066D RID: 1645
		k_EControllerActionOrigin_PS4_RightTrigger_Pull,
		// Token: 0x0400066E RID: 1646
		k_EControllerActionOrigin_PS4_RightTrigger_Click,
		// Token: 0x0400066F RID: 1647
		k_EControllerActionOrigin_PS4_LeftStick_Move,
		// Token: 0x04000670 RID: 1648
		k_EControllerActionOrigin_PS4_LeftStick_Click,
		// Token: 0x04000671 RID: 1649
		k_EControllerActionOrigin_PS4_LeftStick_DPadNorth,
		// Token: 0x04000672 RID: 1650
		k_EControllerActionOrigin_PS4_LeftStick_DPadSouth,
		// Token: 0x04000673 RID: 1651
		k_EControllerActionOrigin_PS4_LeftStick_DPadWest,
		// Token: 0x04000674 RID: 1652
		k_EControllerActionOrigin_PS4_LeftStick_DPadEast,
		// Token: 0x04000675 RID: 1653
		k_EControllerActionOrigin_PS4_RightStick_Move,
		// Token: 0x04000676 RID: 1654
		k_EControllerActionOrigin_PS4_RightStick_Click,
		// Token: 0x04000677 RID: 1655
		k_EControllerActionOrigin_PS4_RightStick_DPadNorth,
		// Token: 0x04000678 RID: 1656
		k_EControllerActionOrigin_PS4_RightStick_DPadSouth,
		// Token: 0x04000679 RID: 1657
		k_EControllerActionOrigin_PS4_RightStick_DPadWest,
		// Token: 0x0400067A RID: 1658
		k_EControllerActionOrigin_PS4_RightStick_DPadEast,
		// Token: 0x0400067B RID: 1659
		k_EControllerActionOrigin_PS4_DPad_North,
		// Token: 0x0400067C RID: 1660
		k_EControllerActionOrigin_PS4_DPad_South,
		// Token: 0x0400067D RID: 1661
		k_EControllerActionOrigin_PS4_DPad_West,
		// Token: 0x0400067E RID: 1662
		k_EControllerActionOrigin_PS4_DPad_East,
		// Token: 0x0400067F RID: 1663
		k_EControllerActionOrigin_PS4_Gyro_Move,
		// Token: 0x04000680 RID: 1664
		k_EControllerActionOrigin_PS4_Gyro_Pitch,
		// Token: 0x04000681 RID: 1665
		k_EControllerActionOrigin_PS4_Gyro_Yaw,
		// Token: 0x04000682 RID: 1666
		k_EControllerActionOrigin_PS4_Gyro_Roll,
		// Token: 0x04000683 RID: 1667
		k_EControllerActionOrigin_XBoxOne_A,
		// Token: 0x04000684 RID: 1668
		k_EControllerActionOrigin_XBoxOne_B,
		// Token: 0x04000685 RID: 1669
		k_EControllerActionOrigin_XBoxOne_X,
		// Token: 0x04000686 RID: 1670
		k_EControllerActionOrigin_XBoxOne_Y,
		// Token: 0x04000687 RID: 1671
		k_EControllerActionOrigin_XBoxOne_LeftBumper,
		// Token: 0x04000688 RID: 1672
		k_EControllerActionOrigin_XBoxOne_RightBumper,
		// Token: 0x04000689 RID: 1673
		k_EControllerActionOrigin_XBoxOne_Menu,
		// Token: 0x0400068A RID: 1674
		k_EControllerActionOrigin_XBoxOne_View,
		// Token: 0x0400068B RID: 1675
		k_EControllerActionOrigin_XBoxOne_LeftTrigger_Pull,
		// Token: 0x0400068C RID: 1676
		k_EControllerActionOrigin_XBoxOne_LeftTrigger_Click,
		// Token: 0x0400068D RID: 1677
		k_EControllerActionOrigin_XBoxOne_RightTrigger_Pull,
		// Token: 0x0400068E RID: 1678
		k_EControllerActionOrigin_XBoxOne_RightTrigger_Click,
		// Token: 0x0400068F RID: 1679
		k_EControllerActionOrigin_XBoxOne_LeftStick_Move,
		// Token: 0x04000690 RID: 1680
		k_EControllerActionOrigin_XBoxOne_LeftStick_Click,
		// Token: 0x04000691 RID: 1681
		k_EControllerActionOrigin_XBoxOne_LeftStick_DPadNorth,
		// Token: 0x04000692 RID: 1682
		k_EControllerActionOrigin_XBoxOne_LeftStick_DPadSouth,
		// Token: 0x04000693 RID: 1683
		k_EControllerActionOrigin_XBoxOne_LeftStick_DPadWest,
		// Token: 0x04000694 RID: 1684
		k_EControllerActionOrigin_XBoxOne_LeftStick_DPadEast,
		// Token: 0x04000695 RID: 1685
		k_EControllerActionOrigin_XBoxOne_RightStick_Move,
		// Token: 0x04000696 RID: 1686
		k_EControllerActionOrigin_XBoxOne_RightStick_Click,
		// Token: 0x04000697 RID: 1687
		k_EControllerActionOrigin_XBoxOne_RightStick_DPadNorth,
		// Token: 0x04000698 RID: 1688
		k_EControllerActionOrigin_XBoxOne_RightStick_DPadSouth,
		// Token: 0x04000699 RID: 1689
		k_EControllerActionOrigin_XBoxOne_RightStick_DPadWest,
		// Token: 0x0400069A RID: 1690
		k_EControllerActionOrigin_XBoxOne_RightStick_DPadEast,
		// Token: 0x0400069B RID: 1691
		k_EControllerActionOrigin_XBoxOne_DPad_North,
		// Token: 0x0400069C RID: 1692
		k_EControllerActionOrigin_XBoxOne_DPad_South,
		// Token: 0x0400069D RID: 1693
		k_EControllerActionOrigin_XBoxOne_DPad_West,
		// Token: 0x0400069E RID: 1694
		k_EControllerActionOrigin_XBoxOne_DPad_East,
		// Token: 0x0400069F RID: 1695
		k_EControllerActionOrigin_XBox360_A,
		// Token: 0x040006A0 RID: 1696
		k_EControllerActionOrigin_XBox360_B,
		// Token: 0x040006A1 RID: 1697
		k_EControllerActionOrigin_XBox360_X,
		// Token: 0x040006A2 RID: 1698
		k_EControllerActionOrigin_XBox360_Y,
		// Token: 0x040006A3 RID: 1699
		k_EControllerActionOrigin_XBox360_LeftBumper,
		// Token: 0x040006A4 RID: 1700
		k_EControllerActionOrigin_XBox360_RightBumper,
		// Token: 0x040006A5 RID: 1701
		k_EControllerActionOrigin_XBox360_Start,
		// Token: 0x040006A6 RID: 1702
		k_EControllerActionOrigin_XBox360_Back,
		// Token: 0x040006A7 RID: 1703
		k_EControllerActionOrigin_XBox360_LeftTrigger_Pull,
		// Token: 0x040006A8 RID: 1704
		k_EControllerActionOrigin_XBox360_LeftTrigger_Click,
		// Token: 0x040006A9 RID: 1705
		k_EControllerActionOrigin_XBox360_RightTrigger_Pull,
		// Token: 0x040006AA RID: 1706
		k_EControllerActionOrigin_XBox360_RightTrigger_Click,
		// Token: 0x040006AB RID: 1707
		k_EControllerActionOrigin_XBox360_LeftStick_Move,
		// Token: 0x040006AC RID: 1708
		k_EControllerActionOrigin_XBox360_LeftStick_Click,
		// Token: 0x040006AD RID: 1709
		k_EControllerActionOrigin_XBox360_LeftStick_DPadNorth,
		// Token: 0x040006AE RID: 1710
		k_EControllerActionOrigin_XBox360_LeftStick_DPadSouth,
		// Token: 0x040006AF RID: 1711
		k_EControllerActionOrigin_XBox360_LeftStick_DPadWest,
		// Token: 0x040006B0 RID: 1712
		k_EControllerActionOrigin_XBox360_LeftStick_DPadEast,
		// Token: 0x040006B1 RID: 1713
		k_EControllerActionOrigin_XBox360_RightStick_Move,
		// Token: 0x040006B2 RID: 1714
		k_EControllerActionOrigin_XBox360_RightStick_Click,
		// Token: 0x040006B3 RID: 1715
		k_EControllerActionOrigin_XBox360_RightStick_DPadNorth,
		// Token: 0x040006B4 RID: 1716
		k_EControllerActionOrigin_XBox360_RightStick_DPadSouth,
		// Token: 0x040006B5 RID: 1717
		k_EControllerActionOrigin_XBox360_RightStick_DPadWest,
		// Token: 0x040006B6 RID: 1718
		k_EControllerActionOrigin_XBox360_RightStick_DPadEast,
		// Token: 0x040006B7 RID: 1719
		k_EControllerActionOrigin_XBox360_DPad_North,
		// Token: 0x040006B8 RID: 1720
		k_EControllerActionOrigin_XBox360_DPad_South,
		// Token: 0x040006B9 RID: 1721
		k_EControllerActionOrigin_XBox360_DPad_West,
		// Token: 0x040006BA RID: 1722
		k_EControllerActionOrigin_XBox360_DPad_East,
		// Token: 0x040006BB RID: 1723
		k_EControllerActionOrigin_SteamV2_A,
		// Token: 0x040006BC RID: 1724
		k_EControllerActionOrigin_SteamV2_B,
		// Token: 0x040006BD RID: 1725
		k_EControllerActionOrigin_SteamV2_X,
		// Token: 0x040006BE RID: 1726
		k_EControllerActionOrigin_SteamV2_Y,
		// Token: 0x040006BF RID: 1727
		k_EControllerActionOrigin_SteamV2_LeftBumper,
		// Token: 0x040006C0 RID: 1728
		k_EControllerActionOrigin_SteamV2_RightBumper,
		// Token: 0x040006C1 RID: 1729
		k_EControllerActionOrigin_SteamV2_LeftGrip,
		// Token: 0x040006C2 RID: 1730
		k_EControllerActionOrigin_SteamV2_RightGrip,
		// Token: 0x040006C3 RID: 1731
		k_EControllerActionOrigin_SteamV2_LeftGrip_Upper,
		// Token: 0x040006C4 RID: 1732
		k_EControllerActionOrigin_SteamV2_RightGrip_Upper,
		// Token: 0x040006C5 RID: 1733
		k_EControllerActionOrigin_SteamV2_LeftBumper_Pressure,
		// Token: 0x040006C6 RID: 1734
		k_EControllerActionOrigin_SteamV2_RightBumper_Pressure,
		// Token: 0x040006C7 RID: 1735
		k_EControllerActionOrigin_SteamV2_LeftGrip_Pressure,
		// Token: 0x040006C8 RID: 1736
		k_EControllerActionOrigin_SteamV2_RightGrip_Pressure,
		// Token: 0x040006C9 RID: 1737
		k_EControllerActionOrigin_SteamV2_LeftGrip_Upper_Pressure,
		// Token: 0x040006CA RID: 1738
		k_EControllerActionOrigin_SteamV2_RightGrip_Upper_Pressure,
		// Token: 0x040006CB RID: 1739
		k_EControllerActionOrigin_SteamV2_Start,
		// Token: 0x040006CC RID: 1740
		k_EControllerActionOrigin_SteamV2_Back,
		// Token: 0x040006CD RID: 1741
		k_EControllerActionOrigin_SteamV2_LeftPad_Touch,
		// Token: 0x040006CE RID: 1742
		k_EControllerActionOrigin_SteamV2_LeftPad_Swipe,
		// Token: 0x040006CF RID: 1743
		k_EControllerActionOrigin_SteamV2_LeftPad_Click,
		// Token: 0x040006D0 RID: 1744
		k_EControllerActionOrigin_SteamV2_LeftPad_Pressure,
		// Token: 0x040006D1 RID: 1745
		k_EControllerActionOrigin_SteamV2_LeftPad_DPadNorth,
		// Token: 0x040006D2 RID: 1746
		k_EControllerActionOrigin_SteamV2_LeftPad_DPadSouth,
		// Token: 0x040006D3 RID: 1747
		k_EControllerActionOrigin_SteamV2_LeftPad_DPadWest,
		// Token: 0x040006D4 RID: 1748
		k_EControllerActionOrigin_SteamV2_LeftPad_DPadEast,
		// Token: 0x040006D5 RID: 1749
		k_EControllerActionOrigin_SteamV2_RightPad_Touch,
		// Token: 0x040006D6 RID: 1750
		k_EControllerActionOrigin_SteamV2_RightPad_Swipe,
		// Token: 0x040006D7 RID: 1751
		k_EControllerActionOrigin_SteamV2_RightPad_Click,
		// Token: 0x040006D8 RID: 1752
		k_EControllerActionOrigin_SteamV2_RightPad_Pressure,
		// Token: 0x040006D9 RID: 1753
		k_EControllerActionOrigin_SteamV2_RightPad_DPadNorth,
		// Token: 0x040006DA RID: 1754
		k_EControllerActionOrigin_SteamV2_RightPad_DPadSouth,
		// Token: 0x040006DB RID: 1755
		k_EControllerActionOrigin_SteamV2_RightPad_DPadWest,
		// Token: 0x040006DC RID: 1756
		k_EControllerActionOrigin_SteamV2_RightPad_DPadEast,
		// Token: 0x040006DD RID: 1757
		k_EControllerActionOrigin_SteamV2_LeftTrigger_Pull,
		// Token: 0x040006DE RID: 1758
		k_EControllerActionOrigin_SteamV2_LeftTrigger_Click,
		// Token: 0x040006DF RID: 1759
		k_EControllerActionOrigin_SteamV2_RightTrigger_Pull,
		// Token: 0x040006E0 RID: 1760
		k_EControllerActionOrigin_SteamV2_RightTrigger_Click,
		// Token: 0x040006E1 RID: 1761
		k_EControllerActionOrigin_SteamV2_LeftStick_Move,
		// Token: 0x040006E2 RID: 1762
		k_EControllerActionOrigin_SteamV2_LeftStick_Click,
		// Token: 0x040006E3 RID: 1763
		k_EControllerActionOrigin_SteamV2_LeftStick_DPadNorth,
		// Token: 0x040006E4 RID: 1764
		k_EControllerActionOrigin_SteamV2_LeftStick_DPadSouth,
		// Token: 0x040006E5 RID: 1765
		k_EControllerActionOrigin_SteamV2_LeftStick_DPadWest,
		// Token: 0x040006E6 RID: 1766
		k_EControllerActionOrigin_SteamV2_LeftStick_DPadEast,
		// Token: 0x040006E7 RID: 1767
		k_EControllerActionOrigin_SteamV2_Gyro_Move,
		// Token: 0x040006E8 RID: 1768
		k_EControllerActionOrigin_SteamV2_Gyro_Pitch,
		// Token: 0x040006E9 RID: 1769
		k_EControllerActionOrigin_SteamV2_Gyro_Yaw,
		// Token: 0x040006EA RID: 1770
		k_EControllerActionOrigin_SteamV2_Gyro_Roll,
		// Token: 0x040006EB RID: 1771
		k_EControllerActionOrigin_Count
	}
}
