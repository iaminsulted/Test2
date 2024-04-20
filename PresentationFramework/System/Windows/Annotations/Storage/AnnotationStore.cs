using System;
using System.Collections.Generic;

namespace System.Windows.Annotations.Storage
{
	// Token: 0x02000874 RID: 2164
	public abstract class AnnotationStore : IDisposable
	{
		// Token: 0x06007FBD RID: 32701 RVA: 0x003204E4 File Offset: 0x0031F4E4
		~AnnotationStore()
		{
			this.Dispose(false);
		}

		// Token: 0x06007FBE RID: 32702
		public abstract void AddAnnotation(Annotation newAnnotation);

		// Token: 0x06007FBF RID: 32703
		public abstract Annotation DeleteAnnotation(Guid annotationId);

		// Token: 0x06007FC0 RID: 32704
		public abstract IList<Annotation> GetAnnotations(ContentLocator anchorLocator);

		// Token: 0x06007FC1 RID: 32705
		public abstract IList<Annotation> GetAnnotations();

		// Token: 0x06007FC2 RID: 32706
		public abstract Annotation GetAnnotation(Guid annotationId);

		// Token: 0x06007FC3 RID: 32707
		public abstract void Flush();

		// Token: 0x06007FC4 RID: 32708 RVA: 0x00320514 File Offset: 0x0031F514
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x17001D70 RID: 7536
		// (get) Token: 0x06007FC5 RID: 32709
		// (set) Token: 0x06007FC6 RID: 32710
		public abstract bool AutoFlush { get; set; }

		// Token: 0x1400016D RID: 365
		// (add) Token: 0x06007FC7 RID: 32711 RVA: 0x00320524 File Offset: 0x0031F524
		// (remove) Token: 0x06007FC8 RID: 32712 RVA: 0x0032055C File Offset: 0x0031F55C
		public event StoreContentChangedEventHandler StoreContentChanged;

		// Token: 0x1400016E RID: 366
		// (add) Token: 0x06007FC9 RID: 32713 RVA: 0x00320594 File Offset: 0x0031F594
		// (remove) Token: 0x06007FCA RID: 32714 RVA: 0x003205CC File Offset: 0x0031F5CC
		public event AnnotationAuthorChangedEventHandler AuthorChanged;

		// Token: 0x1400016F RID: 367
		// (add) Token: 0x06007FCB RID: 32715 RVA: 0x00320604 File Offset: 0x0031F604
		// (remove) Token: 0x06007FCC RID: 32716 RVA: 0x0032063C File Offset: 0x0031F63C
		public event AnnotationResourceChangedEventHandler AnchorChanged;

		// Token: 0x14000170 RID: 368
		// (add) Token: 0x06007FCD RID: 32717 RVA: 0x00320674 File Offset: 0x0031F674
		// (remove) Token: 0x06007FCE RID: 32718 RVA: 0x003206AC File Offset: 0x0031F6AC
		public event AnnotationResourceChangedEventHandler CargoChanged;

		// Token: 0x06007FCF RID: 32719 RVA: 0x003206E4 File Offset: 0x0031F6E4
		protected virtual void Dispose(bool disposing)
		{
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				if (!this._disposed)
				{
					this._disposed = true;
				}
			}
		}

		// Token: 0x06007FD0 RID: 32720 RVA: 0x00320730 File Offset: 0x0031F730
		protected virtual void OnAuthorChanged(AnnotationAuthorChangedEventArgs args)
		{
			AnnotationAuthorChangedEventHandler annotationAuthorChangedEventHandler = null;
			if (args.Author == null)
			{
				return;
			}
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				annotationAuthorChangedEventHandler = this.AuthorChanged;
			}
			if (this.AutoFlush)
			{
				this.Flush();
			}
			if (annotationAuthorChangedEventHandler != null)
			{
				annotationAuthorChangedEventHandler(this, args);
			}
		}

		// Token: 0x06007FD1 RID: 32721 RVA: 0x00320798 File Offset: 0x0031F798
		protected virtual void OnAnchorChanged(AnnotationResourceChangedEventArgs args)
		{
			AnnotationResourceChangedEventHandler annotationResourceChangedEventHandler = null;
			if (args.Resource == null)
			{
				return;
			}
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				annotationResourceChangedEventHandler = this.AnchorChanged;
			}
			if (this.AutoFlush)
			{
				this.Flush();
			}
			if (annotationResourceChangedEventHandler != null)
			{
				annotationResourceChangedEventHandler(this, args);
			}
		}

		// Token: 0x06007FD2 RID: 32722 RVA: 0x00320800 File Offset: 0x0031F800
		protected virtual void OnCargoChanged(AnnotationResourceChangedEventArgs args)
		{
			AnnotationResourceChangedEventHandler annotationResourceChangedEventHandler = null;
			if (args.Resource == null)
			{
				return;
			}
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				annotationResourceChangedEventHandler = this.CargoChanged;
			}
			if (this.AutoFlush)
			{
				this.Flush();
			}
			if (annotationResourceChangedEventHandler != null)
			{
				annotationResourceChangedEventHandler(this, args);
			}
		}

		// Token: 0x06007FD3 RID: 32723 RVA: 0x00320868 File Offset: 0x0031F868
		protected virtual void OnStoreContentChanged(StoreContentChangedEventArgs e)
		{
			StoreContentChangedEventHandler storeContentChangedEventHandler = null;
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				storeContentChangedEventHandler = this.StoreContentChanged;
			}
			if (this.AutoFlush)
			{
				this.Flush();
			}
			if (storeContentChangedEventHandler != null)
			{
				storeContentChangedEventHandler(this, e);
			}
		}

		// Token: 0x17001D71 RID: 7537
		// (get) Token: 0x06007FD4 RID: 32724 RVA: 0x003208C4 File Offset: 0x0031F8C4
		protected object SyncRoot
		{
			get
			{
				return this.lockObject;
			}
		}

		// Token: 0x17001D72 RID: 7538
		// (get) Token: 0x06007FD5 RID: 32725 RVA: 0x003208CC File Offset: 0x0031F8CC
		protected bool IsDisposed
		{
			get
			{
				return this._disposed;
			}
		}

		// Token: 0x04003B97 RID: 15255
		private bool _disposed;

		// Token: 0x04003B98 RID: 15256
		private object lockObject = new object();
	}
}
