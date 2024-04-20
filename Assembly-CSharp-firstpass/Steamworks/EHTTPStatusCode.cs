using System;

namespace Steamworks
{
	// Token: 0x0200019E RID: 414
	public enum EHTTPStatusCode
	{
		// Token: 0x040009A8 RID: 2472
		k_EHTTPStatusCodeInvalid,
		// Token: 0x040009A9 RID: 2473
		k_EHTTPStatusCode100Continue = 100,
		// Token: 0x040009AA RID: 2474
		k_EHTTPStatusCode101SwitchingProtocols,
		// Token: 0x040009AB RID: 2475
		k_EHTTPStatusCode200OK = 200,
		// Token: 0x040009AC RID: 2476
		k_EHTTPStatusCode201Created,
		// Token: 0x040009AD RID: 2477
		k_EHTTPStatusCode202Accepted,
		// Token: 0x040009AE RID: 2478
		k_EHTTPStatusCode203NonAuthoritative,
		// Token: 0x040009AF RID: 2479
		k_EHTTPStatusCode204NoContent,
		// Token: 0x040009B0 RID: 2480
		k_EHTTPStatusCode205ResetContent,
		// Token: 0x040009B1 RID: 2481
		k_EHTTPStatusCode206PartialContent,
		// Token: 0x040009B2 RID: 2482
		k_EHTTPStatusCode300MultipleChoices = 300,
		// Token: 0x040009B3 RID: 2483
		k_EHTTPStatusCode301MovedPermanently,
		// Token: 0x040009B4 RID: 2484
		k_EHTTPStatusCode302Found,
		// Token: 0x040009B5 RID: 2485
		k_EHTTPStatusCode303SeeOther,
		// Token: 0x040009B6 RID: 2486
		k_EHTTPStatusCode304NotModified,
		// Token: 0x040009B7 RID: 2487
		k_EHTTPStatusCode305UseProxy,
		// Token: 0x040009B8 RID: 2488
		k_EHTTPStatusCode307TemporaryRedirect = 307,
		// Token: 0x040009B9 RID: 2489
		k_EHTTPStatusCode400BadRequest = 400,
		// Token: 0x040009BA RID: 2490
		k_EHTTPStatusCode401Unauthorized,
		// Token: 0x040009BB RID: 2491
		k_EHTTPStatusCode402PaymentRequired,
		// Token: 0x040009BC RID: 2492
		k_EHTTPStatusCode403Forbidden,
		// Token: 0x040009BD RID: 2493
		k_EHTTPStatusCode404NotFound,
		// Token: 0x040009BE RID: 2494
		k_EHTTPStatusCode405MethodNotAllowed,
		// Token: 0x040009BF RID: 2495
		k_EHTTPStatusCode406NotAcceptable,
		// Token: 0x040009C0 RID: 2496
		k_EHTTPStatusCode407ProxyAuthRequired,
		// Token: 0x040009C1 RID: 2497
		k_EHTTPStatusCode408RequestTimeout,
		// Token: 0x040009C2 RID: 2498
		k_EHTTPStatusCode409Conflict,
		// Token: 0x040009C3 RID: 2499
		k_EHTTPStatusCode410Gone,
		// Token: 0x040009C4 RID: 2500
		k_EHTTPStatusCode411LengthRequired,
		// Token: 0x040009C5 RID: 2501
		k_EHTTPStatusCode412PreconditionFailed,
		// Token: 0x040009C6 RID: 2502
		k_EHTTPStatusCode413RequestEntityTooLarge,
		// Token: 0x040009C7 RID: 2503
		k_EHTTPStatusCode414RequestURITooLong,
		// Token: 0x040009C8 RID: 2504
		k_EHTTPStatusCode415UnsupportedMediaType,
		// Token: 0x040009C9 RID: 2505
		k_EHTTPStatusCode416RequestedRangeNotSatisfiable,
		// Token: 0x040009CA RID: 2506
		k_EHTTPStatusCode417ExpectationFailed,
		// Token: 0x040009CB RID: 2507
		k_EHTTPStatusCode4xxUnknown,
		// Token: 0x040009CC RID: 2508
		k_EHTTPStatusCode429TooManyRequests = 429,
		// Token: 0x040009CD RID: 2509
		k_EHTTPStatusCode500InternalServerError = 500,
		// Token: 0x040009CE RID: 2510
		k_EHTTPStatusCode501NotImplemented,
		// Token: 0x040009CF RID: 2511
		k_EHTTPStatusCode502BadGateway,
		// Token: 0x040009D0 RID: 2512
		k_EHTTPStatusCode503ServiceUnavailable,
		// Token: 0x040009D1 RID: 2513
		k_EHTTPStatusCode504GatewayTimeout,
		// Token: 0x040009D2 RID: 2514
		k_EHTTPStatusCode505HTTPVersionNotSupported,
		// Token: 0x040009D3 RID: 2515
		k_EHTTPStatusCode5xxUnknown = 599
	}
}
