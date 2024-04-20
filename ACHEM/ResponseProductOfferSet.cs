using System;

public class ResponseProductOfferSet : Response
{
	public ProductID ProductID;

	public DateTime ExpireUTC;

	public ResponseProductOfferSet()
	{
		type = 36;
		cmd = 1;
	}

	public ResponseProductOfferSet(ProductID productID, DateTime expireUTC)
	{
		type = 36;
		cmd = 1;
		ProductID = productID;
		ExpireUTC = expireUTC;
	}
}
