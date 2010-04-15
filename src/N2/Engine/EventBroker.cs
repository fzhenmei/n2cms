﻿using System;
using System.Web;
using System.Threading;
using System.Diagnostics;

namespace N2.Engine
{
	/// <summary>
	/// A broker for events from the http application. The purpose of this 
	/// class is to reduce the risk of temporary errors during initialization
	/// causing the site to be crippled.
	/// </summary>
	public class EventBroker
	{
		static EventBroker()
		{
			Instance = new EventBroker();
		}

		/// <summary>Accesses the event broker singleton instance.</summary>
		public static EventBroker Instance
		{
			get { return Singleton<EventBroker>.Instance; }
			protected set { Singleton<EventBroker>.Instance = value; }
		}

		/// <summary>Attaches to events from the application instance.</summary>
		public virtual void Attach(HttpApplication application)
		{
			Trace.WriteLine("EventBroker: Attaching to " + application);

			application.BeginRequest += Application_BeginRequest;
			//application.PostAuthorizeRequest += application_PostAuthorizeRequest;
			application.AuthorizeRequest += Application_AuthorizeRequest;
			//application.PostMapRequestHandler += Application_PostMapRequestHandler;
			application.AcquireRequestState += Application_AcquireRequestState;
			application.Error += Application_Error;
			application.EndRequest += Application_EndRequest;

			application.Disposed += Application_Disposed;
		}

		/// <summary>Detaches events from the application instance.</summary>
		void Application_Disposed(object sender, EventArgs e)
		{
			Trace.WriteLine("EventBroker: Disposing " + sender);
		}

		public EventHandler<EventArgs> BeginRequest;
		public EventHandler<EventArgs> AuthorizeRequest;
		//public EventHandler<EventArgs> PostAuthorizeRequest;
		public EventHandler<EventArgs> AcquireRequestState;
		//public EventHandler<EventArgs> PostMapRequestHandler;
		public EventHandler<EventArgs> Error;
		public EventHandler<EventArgs> EndRequest;

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			if (BeginRequest != null && !IsStaticResource(sender))
			{
				Debug.WriteLine("Application_BeginRequest");
				BeginRequest(sender, e);
			}
		}

		protected void Application_AuthorizeRequest(object sender, EventArgs e)
		{
			if (AuthorizeRequest != null && !IsStaticResource(sender))
			{
				Debug.WriteLine("Application_AuthorizeRequest");
				AuthorizeRequest(sender, e);
			}
		}

		//void application_PostAuthorizeRequest(object sender, EventArgs e)
		//{
		//    Debug.WriteLine("application_PostAuthorizeRequest");
		//    if (PostAuthorizeRequest != null && !IsStaticResource(sender))
		//        PostAuthorizeRequest(sender, e);
		//}

		//void Application_PostMapRequestHandler(object sender, EventArgs e)
		//{
		//    Debug.WriteLine("Application_PostMapRequestHandler");
		//    if (PostMapRequestHandler != null && !IsStaticResource(sender))
		//        PostMapRequestHandler(sender, e);
		//}

		protected void Application_AcquireRequestState(object sender, EventArgs e)
		{
			if (AcquireRequestState != null && !IsStaticResource(sender))
			{
				Debug.WriteLine("Application_AcquireRequestState");
				AcquireRequestState(sender, e);
			}
		}

		protected void Application_Error(object sender, EventArgs e)
		{
			if (Error != null && !IsStaticResource(sender))
				Error(sender, e);
		}

		protected void Application_EndRequest(object sender, EventArgs e)
		{
			if (EndRequest != null && !IsStaticResource(sender))
				EndRequest(sender, e);
		}

		/// <summary>Returns true if the requested resource is one of the typical resources that needn't be processed by the cms engine.</summary>
		/// <param name="sender">The event sender, probably a http application.</param>
		/// <returns>True if the request targets a static resource file.</returns>
		/// <remarks>
		/// These are the file extensions considered to be static resources:
		/// .css
		///	.gif
		/// .png 
		/// .jpg
		/// .jpeg
		/// .js
		/// .axd
		/// .ashx
		/// </remarks>
		protected static bool IsStaticResource(object sender)
		{
			HttpApplication application = sender as HttpApplication;
			if(application != null)
			{
				string path = application.Request.Path;
				string extension = VirtualPathUtility.GetExtension(path);
				
				if(extension == null) return false;

				switch (extension.ToLower())
				{
					case ".css":
					case ".gif":
					case ".png":
					case ".jpg":
					case ".jpeg":
					case ".js":
					case ".axd":
					case ".ashx":
						return true;
				}
			}
			return false;
		}
	}
}