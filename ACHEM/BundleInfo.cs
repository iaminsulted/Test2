public class BundleInfo
{
	public string FileName;

	public short Version;

	public long CRCWeb;

	public long CRCAndroid;

	public long CRCIOS;

	public long CRCPC;

	public long CRCOSX;

	public int DependencyID;

	public long CRC => 0L;

	public BundleInfo(string filename, short version, long crcWeb, long crcAndroid, long crcIOS, long crcPC = 0L, long crcOSX = 0L, int dependency = 0)
	{
		FileName = filename;
		Version = version;
		CRCWeb = crcWeb;
		CRCAndroid = crcAndroid;
		CRCIOS = crcIOS;
		CRCPC = crcPC;
		CRCOSX = crcOSX;
		DependencyID = dependency;
	}
}
