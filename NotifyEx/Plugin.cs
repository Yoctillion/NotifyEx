using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Grabacr07.KanColleViewer.Composition;
using Livet;
using NotifyEx.Models;
using NotifyEx.Properties;
using NotifyEx.ViewModels;
using NotifyEx.Views;

namespace NotifyEx
{
	[Export(typeof(IPlugin))]
	[Export(typeof(ISettings))]
	[Export(typeof(IRequestNotify))]
	[ExportMetadata("Guid", "3190E362-3833-4953-87C3-B2C22C058EE8")]
	[ExportMetadata("Title", "NotifyEx")]
	[ExportMetadata("Description", "通知内容扩展")]
	[ExportMetadata("Version", "0.8.1")]
	[ExportMetadata("Author", "@Yoctillion")]
	public class Plugin : IPlugin, ISettings, IRequestNotify
	{
		private ToolViewModel _viewModel;

		public void Initialize()
		{
            NotifyHost.TryInitialize(this);

			Settings.Default.Reload();
			_viewModel = new ToolViewModel();
		}

		public string Name => "NotifyEx";

		public object View => new ToolView { DataContext = _viewModel };

		public event EventHandler<NotifyEventArgs> NotifyRequested;

		internal void Notify(string type, string header, string body)
		{
			NotifyRequested?.Invoke(this, new NotifyEventArgs(type, header, body)
			{
				Activated = () =>
				{
					DispatcherHelper.UIDispatcher.Invoke(() =>
					{
						var window = Application.Current.MainWindow;
						if (window.WindowState == WindowState.Minimized)
							window.WindowState = WindowState.Normal;
						window.Activate();
					});
				},
			});
		}
	}
}
