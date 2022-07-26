﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ControlzEx.Theming;
using MahApps.Metro.Controls;

namespace TaskDialogInterop
{
	/// <summary>
	/// Displays a task dialog.
	/// </summary>
	public partial class TaskDialog : MetroWindow
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskDialog"/> class.
        /// </summary>
        public TaskDialog()
		{
			InitializeComponent();

			this.Loaded += new RoutedEventHandler(taskDialog_Loaded);
			this.SourceInitialized += new EventHandler(taskDialog_SourceInitialized);
			this.KeyDown += new KeyEventHandler(taskDialog_KeyDown);
			base.ContentRendered += new EventHandler(taskDialog_ContentRendered);
			this.Closing += new System.ComponentModel.CancelEventHandler(taskDialog_Closing);
			base.Closed += new EventHandler(taskDialog_Closed);
		}

		private TaskDialogViewModel ViewModel
		{
			get { return this.DataContext as TaskDialogViewModel; }
		}

		private void taskDialog_Loaded(object sender, RoutedEventArgs e)
		{
			if (ViewModel != null)
			{
				ViewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(viewModel_PropertyChanged);
				ViewModel.RequestClose +=new EventHandler(viewModel_RequestClose);

				convertToHyperlinkedText(ContentText, ViewModel.Content);
				convertToHyperlinkedText(ContentExpandedInfo, ViewModel.ContentExpandedInfo);
				convertToHyperlinkedText(FooterExpandedInfo, ViewModel.FooterExpandedInfo);
				convertToHyperlinkedText(FooterText, ViewModel.FooterText);

				this.WindowStartupLocation = ViewModel.StartPosition;

				if (ViewModel.NormalButtons.Count == 0)
				{
					this.MaxWidth = 462;
				}
				
				// Footer only shows the secondary white top border when the buttons section is visible
				FooterInner.BorderThickness = new Thickness(
					FooterInner.BorderThickness.Left,
					((ButtonsArea.Visibility == System.Windows.Visibility.Visible) ? 1 : 0),
					FooterInner.BorderThickness.Right,
					FooterInner.BorderThickness.Bottom);

				// Hide the special button areas if they are empty
				if (ViewModel.CommandLinks.Count == 0)
					CommandLinks.Visibility = System.Windows.Visibility.Collapsed;
				if (ViewModel.RadioButtons.Count == 0)
					RadioButtons.Visibility = System.Windows.Visibility.Collapsed;

				// Play the appropriate sound
				switch (ViewModel.MainIconType)
				{
					default:
					case TaskDialogIcon.None:
					case TaskDialogIcon.Shield:
						// No sound
						break;
					case TaskDialogIcon.Warning:
						System.Media.SystemSounds.Exclamation.Play();
						break;
					case TaskDialogIcon.Error:
						System.Media.SystemSounds.Hand.Play();
						break;
					case TaskDialogIcon.Information:
						System.Media.SystemSounds.Asterisk.Play();
						break;
				}
			}
		}
		private void taskDialog_SourceInitialized(object sender, EventArgs e)
		{
			if (ViewModel != null)
			{
				ViewModel.NotifyConstructed();
				ViewModel.NotifyCreated();

				if (ViewModel.AllowDialogCancellation)
				{
					SafeNativeMethods.SetWindowIconVisibility(this, false);
				}
				else
				{
					// This also hides the icon, too
					SafeNativeMethods.SetWindowCloseButtonVisibility(this, false);
				}
			}
		}
        
		private void taskDialog_KeyDown(object sender, KeyEventArgs e)
		{
			if (ViewModel != null)
			{
				// Block Alt-F4 if it isn't allowed
				if (!ViewModel.AllowDialogCancellation
					&& e.Key == Key.System && e.SystemKey == Key.F4)
					e.Handled = true;

				// Handel Esc manually if the override has been set
				if (ViewModel.AllowDialogCancellation
					&& e.Key == Key.Escape)
				{
					e.Handled = true;
					this.DialogResult = false;
					Close();
				}
			}
		}
		private void taskDialog_ContentRendered(object sender, EventArgs e)
		{
			ViewModel.NotifyShown();
		}
		private void taskDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = ViewModel.ShouldCancelClosing();
		}
		private void taskDialog_Closed(object sender, EventArgs e)
		{
			ViewModel.NotifyClosed();
		}
		private void viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Content")
				convertToHyperlinkedText(ContentText, ViewModel.Content);
			if (e.PropertyName == "ContentExpandedInfo")
				convertToHyperlinkedText(ContentExpandedInfo, ViewModel.ContentExpandedInfo);
			if (e.PropertyName == "FooterExpandedInfo")
				convertToHyperlinkedText(FooterExpandedInfo, ViewModel.FooterExpandedInfo);
			if (e.PropertyName == "FooterText")
				convertToHyperlinkedText(FooterText, ViewModel.FooterText);
		}
		private void viewModel_RequestClose(object sender, EventArgs e)
		{
			this.Close();
		}
		private void normalButton_Click(object sender, RoutedEventArgs e)
		{
		}
		private void commandLink_Click(object sender, RoutedEventArgs e)
		{
		}
		private void hyperlink_Click(object sender, EventArgs e)
		{
			if (sender is Hyperlink)
			{
				string uri = (sender as Hyperlink).Tag.ToString();

				if (ViewModel.HyperlinkCommand.CanExecute(uri))
					ViewModel.HyperlinkCommand.Execute(uri);
			}
		}

		private void convertToHyperlinkedText(TextBlock textBlock, string text)
		{
			foreach (Inline inline in textBlock.Inlines)
			{
				if (inline is Hyperlink)
				{
					(inline as Hyperlink).Click -= hyperlink_Click;
				}
			}

			textBlock.Inlines.Clear();

			if (String.IsNullOrEmpty(text))
				return;

			List<Hyperlink> hyperlinks = new();

			foreach (Match match in _hyperlinkCaptureRegex.Matches(text))
			{
				var hyperlink = new Hyperlink();

				hyperlink.Inlines.Add(match.Groups["text"].Value);
				hyperlink.Tag = match.Groups["link"].Value;
				hyperlink.Click += hyperlink_Click;

				hyperlinks.Add(hyperlink);
			}

			string[] substrings = _hyperlinkRegex.Split(text);

			for (int i = 0; i < substrings.Length; i++)
			{
				textBlock.Inlines.Add(substrings[i]);

				if (i < hyperlinks.Count)
					textBlock.Inlines.Add(hyperlinks[i]);
			}
		}
	}
}
