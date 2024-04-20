using System;

[Serializable]
public class InitTxn
{
	public long orderid;

	public long steamid;

	public int appid;

	public int itemcount;

	public int userid;

	public string language;

	public string currency;

	public string usersession;

	public string ipaddress;

	public int[] itemid;

	public int[] qty;

	public double[] amount;

	private double[] _usdAmount;

	public string[] description;

	public string[] category;

	public string httpcontent => "";
}
