using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using ControlzEx.Theming;

namespace TaskDialogInterop
{
	public partial class TaskDialog
	{
		private const string HtmlHyperlinkPattern = "<a href=\".+\">.+</a>";
		private const string HtmlHyperlinkCapturePattern = "<a href=\"(?<link>.+)\">(?<text>.+)</a>";

		private static readonly Regex _hyperlinkRegex = new(HtmlHyperlinkPattern);
		private static readonly Regex _hyperlinkCaptureRegex = new(HtmlHyperlinkCapturePattern);

		internal const int CommandButtonIDOffset = 2000;
		internal const int RadioButtonIDOffset = 1000;
		internal const int CustomButtonIDOffset = 500;

		/// <summary>
		/// Forces the WPF-based TaskDialog window instead of using native calls.
		/// </summary>
		public static bool ForceEmulationMode { get; set; }

		/// <summary>
		/// Occurs when a task dialog is about to show.
		/// </summary>
		/// <remarks>
		/// Use this event for both notification and modification of all task
		/// dialog showings. Changes made to the configuration options will be
		/// persisted.
		/// </remarks>
		public static event TaskDialogShowingEventHandler Showing;
		/// <summary>
		/// Occurs when a task dialog has been closed.
		/// </summary>
		public static new event TaskDialogClosedEventHandler Closed;

		/// <summary>
		/// Displays a task dialog with the given configuration options.
		/// </summary>
		/// <param name="options">
		/// A <see cref="T:TaskDialogInterop.TaskDialogOptions"/> that specifies the
		/// configuration options for the dialog.
		/// </param>
		/// <returns>
		/// A <see cref="T:TaskDialogInterop.TaskDialogResult"/> value that specifies
		/// which button is clicked by the user.
		/// </returns>
		public static TaskDialogResult Show(TaskDialogOptions options)
		{

            // Make a copy since we'll let Showing event possibly modify them
            TaskDialogOptions configOptions = options;

            onShowing(new TaskDialogShowingEventArgs(ref configOptions));

            TaskDialogResult result;
            //if (VistaTaskDialog.IsAvailableOnThisOS && !ForceEmulationMode)
            
            result = showEmulatedTaskDialog(configOptions);
            

            onClosed(new TaskDialogClosedEventArgs(result));

			return result;
		}

		/// <summary>
		/// Displays a task dialog that has a message and that returns a result.
		/// </summary>
		/// <param name="owner">
		/// The <see cref="T:System.Windows.Window"/> that owns this dialog.
		/// </param>
		/// <param name="messageText">
		/// A <see cref="T:System.String"/> that specifies the text to display.
		/// </param>
		/// <returns>
		/// A <see cref="T:TaskDialogInterop.TaskDialogSimpleResult"/> value that
		/// specifies which button is clicked by the user.
		/// </returns>
		public static TaskDialogSimpleResult ShowMessage(Window owner, string messageText)
		{
			TaskDialogOptions options = TaskDialogOptions.Default;

			options.Owner = owner;
			options.Content = messageText;
			options.CommonButtons = TaskDialogCommonButtons.Close;

			return Show(options).Result;
		}
		/// <summary>
		/// Displays a task dialog that has a message and that returns a result.
		/// </summary>
		/// <param name="owner">
		/// The <see cref="T:System.Windows.Window"/> that owns this dialog.
		/// </param>
		/// <param name="messageText">
		/// A <see cref="T:System.String"/> that specifies the text to display.
		/// </param>
		/// <param name="caption">
		/// A <see cref="T:System.String"/> that specifies the title bar
		/// caption to display.
		/// </param>
		/// <returns>
		/// A <see cref="T:TaskDialogInterop.TaskDialogSimpleResult"/> value that
		/// specifies which button is clicked by the user.
		/// </returns>
		public static TaskDialogSimpleResult ShowMessage(Window owner, string messageText, string caption)
		{
			return ShowMessage(owner, messageText, caption, TaskDialogCommonButtons.Close);
		}
		/// <summary>
		/// Displays a task dialog that has a message and that returns a result.
		/// </summary>
		/// <param name="owner">
		/// The <see cref="T:System.Windows.Window"/> that owns this dialog.
		/// </param>
		/// <param name="messageText">
		/// A <see cref="T:System.String"/> that specifies the text to display.
		/// </param>
		/// <param name="caption">
		/// A <see cref="T:System.String"/> that specifies the title bar
		/// caption to display.
		/// </param>
		/// <param name="buttons">
		/// A <see cref="T:TaskDialogInterop.TaskDialogCommonButtons"/> value that
		/// specifies which button or buttons to display.
		/// </param>
		/// <returns>
		/// A <see cref="T:TaskDialogInterop.TaskDialogSimpleResult"/> value that
		/// specifies which button is clicked by the user.
		/// </returns>
		public static TaskDialogSimpleResult ShowMessage(Window owner, string messageText, string caption, TaskDialogCommonButtons buttons)
		{
			return ShowMessage(owner, messageText, caption, buttons, TaskDialogIcon.None);
		}
		/// <summary>
		/// Displays a task dialog that has a message and that returns a result.
		/// </summary>
		/// <param name="owner">
		/// The <see cref="T:System.Windows.Window"/> that owns this dialog.
		/// </param>
		/// <param name="messageText">
		/// A <see cref="T:System.String"/> that specifies the text to display.
		/// </param>
		/// <param name="caption">
		/// A <see cref="T:System.String"/> that specifies the title bar
		/// caption to display.
		/// </param>
		/// <param name="buttons">
		/// A <see cref="T:TaskDialogInterop.TaskDialogCommonButtons"/> value that
		/// specifies which button or buttons to display.
		/// </param>
		/// <param name="icon">
		/// A <see cref="T:TaskDialogInterop.VistaTaskDialogIcon"/> that specifies the
		/// icon to display.
		/// </param>
		/// <returns>
		/// A <see cref="T:TaskDialogInterop.TaskDialogSimpleResult"/> value that
		/// specifies which button is clicked by the user.
		/// </returns>
		public static TaskDialogSimpleResult ShowMessage(Window owner, string messageText, string caption, TaskDialogCommonButtons buttons, TaskDialogIcon icon)
		{
			TaskDialogOptions options = TaskDialogOptions.Default;

			options.Owner = owner;
			options.Title = caption;
			options.Content = messageText;
			options.CommonButtons = buttons;
			options.MainIcon = icon;

			return Show(options).Result;
		}
		/// <summary>
		/// Displays a task dialog that has a message and that returns a result.
		/// </summary>
		/// <param name="owner">
		/// The <see cref="T:System.Windows.Window"/> that owns this dialog.
		/// </param>
		/// <param name="title">
		/// A <see cref="T:System.String"/> that specifies the title bar
		/// caption to display.
		/// </param>
		/// <param name="mainInstruction">
		/// A <see cref="T:System.String"/> that specifies the main text to display.
		/// </param>
		/// <param name="content">
		/// A <see cref="T:System.String"/> that specifies the body text to display.
		/// </param>
		/// <param name="expandedInfo">
		/// A <see cref="T:System.String"/> that specifies the expanded text to display when toggled.
		/// </param>
		/// <param name="verificationText">
		/// A <see cref="T:System.String"/> that specifies the text to display next to a checkbox.
		/// </param>
		/// <param name="footerText">
		/// A <see cref="T:System.String"/> that specifies the footer text to display.
		/// </param>
		/// <param name="buttons">
		/// A <see cref="T:TaskDialogInterop.TaskDialogCommonButtons"/> value that
		/// specifies which button or buttons to display.
		/// </param>
		/// <param name="mainIcon">
		/// A <see cref="T:TaskDialogInterop.VistaTaskDialogIcon"/> that specifies the
		/// main icon to display.
		/// </param>
		/// <param name="footerIcon">
		/// A <see cref="T:TaskDialogInterop.VistaTaskDialogIcon"/> that specifies the
		/// footer icon to display.
		/// </param>
		/// <returns></returns>
		public static TaskDialogSimpleResult ShowMessage(Window owner, string title, string mainInstruction, string content, string expandedInfo, string verificationText, string footerText, TaskDialogCommonButtons buttons, TaskDialogIcon mainIcon, TaskDialogIcon footerIcon)
		{
			TaskDialogOptions options = TaskDialogOptions.Default;

			if (owner != null)
				options.Owner = owner;
			if (!String.IsNullOrEmpty(title))
				options.Title = title;
			if (!String.IsNullOrEmpty(mainInstruction))
				options.MainInstruction = mainInstruction;
			if (!String.IsNullOrEmpty(content))
				options.Content = content;
			if (!String.IsNullOrEmpty(expandedInfo))
				options.ExpandedInfo = expandedInfo;
			if (!String.IsNullOrEmpty(verificationText))
				options.VerificationText = verificationText;
			if (!String.IsNullOrEmpty(footerText))
				options.FooterText = footerText;
			options.CommonButtons = buttons;
			options.MainIcon = mainIcon;
			options.FooterIcon = footerIcon;

			return Show(options).Result;
		}

		internal static VistaTaskDialogCommonButtons ConvertCommonButtons(TaskDialogCommonButtons commonButtons)
		{
            var vtdCommonButtons = commonButtons switch
            {
                TaskDialogCommonButtons.Close => VistaTaskDialogCommonButtons.Close,
                TaskDialogCommonButtons.OKCancel => VistaTaskDialogCommonButtons.OK | VistaTaskDialogCommonButtons.Cancel,
                TaskDialogCommonButtons.RetryCancel => VistaTaskDialogCommonButtons.Retry | VistaTaskDialogCommonButtons.Cancel,
                TaskDialogCommonButtons.YesNo => VistaTaskDialogCommonButtons.Yes | VistaTaskDialogCommonButtons.No,
                TaskDialogCommonButtons.YesNoCancel => VistaTaskDialogCommonButtons.Yes | VistaTaskDialogCommonButtons.No | VistaTaskDialogCommonButtons.Cancel,
                _ => VistaTaskDialogCommonButtons.None,
            };
            return vtdCommonButtons;
		}
		internal static TaskDialogButtonData ConvertCommonButton(VistaTaskDialogCommonButtons commonButton, System.Windows.Input.ICommand command = null, bool isDefault = false, bool isCancel = false)
		{
            var id = commonButton switch
            {
                VistaTaskDialogCommonButtons.OK => (int)TaskDialogSimpleResult.Ok,
                VistaTaskDialogCommonButtons.Yes => (int)TaskDialogSimpleResult.Yes,
                VistaTaskDialogCommonButtons.No => (int)TaskDialogSimpleResult.No,
                VistaTaskDialogCommonButtons.Cancel => (int)TaskDialogSimpleResult.Cancel,
                VistaTaskDialogCommonButtons.Retry => (int)TaskDialogSimpleResult.Retry,
                VistaTaskDialogCommonButtons.Close => (int)TaskDialogSimpleResult.Close,
                _ => (int)TaskDialogSimpleResult.None,
            };
            return new TaskDialogButtonData(id, "_" + commonButton.ToString(), command, isDefault, isCancel);
		}
		internal static int GetButtonIdForCommonButton(TaskDialogCommonButtons commonButtons, int index)
		{
            int buttonId;
            switch (commonButtons)
			{
				default:
				case TaskDialogCommonButtons.None:
				case TaskDialogCommonButtons.Close:
					// We'll set to 0 even for Close, as it doesn't matter that we
					//get the value right since there is only one button anyway
					buttonId = 0;
					break;
				case TaskDialogCommonButtons.OKCancel:
					if (index == 0)
						buttonId = (int)VistaTaskDialogCommonButtons.OK;
					else if (index == 1)
						buttonId = (int)VistaTaskDialogCommonButtons.Cancel;
					else
						buttonId = 0;
					break;
				case TaskDialogCommonButtons.RetryCancel:
					if (index == 0)
						buttonId = (int)VistaTaskDialogCommonButtons.Retry;
					else if (index == 1)
						buttonId = (int)VistaTaskDialogCommonButtons.Cancel;
					else
						buttonId = 0;
					break;
				case TaskDialogCommonButtons.YesNo:
					if (index == 0)
						buttonId = (int)VistaTaskDialogCommonButtons.Yes;
					else if (index == 1)
						buttonId = (int)VistaTaskDialogCommonButtons.No;
					else
						buttonId = 0;
					break;
				case TaskDialogCommonButtons.YesNoCancel:
					if (index == 0)
						buttonId = (int)VistaTaskDialogCommonButtons.Yes;
					else if (index == 1)
						buttonId = (int)VistaTaskDialogCommonButtons.No;
					else if (index == 2)
						buttonId = (int)VistaTaskDialogCommonButtons.Cancel;
					else
						buttonId = 0;
					break;
			}

			return buttonId;
		}

		/// <summary>
		/// Raises the <see cref="E:Showing"/> event.
		/// </summary>
		/// <param name="e">The <see cref="TaskDialogInterop.TaskDialogShowingEventArgs"/> instance containing the event data.</param>
		private static void onShowing(TaskDialogShowingEventArgs e)
		{
            Showing?.Invoke(null, e);
        }
		/// <summary>
		/// Raises the <see cref="E:Closed"/> event.
		/// </summary>
		/// <param name="e">The <see cref="TaskDialogInterop.TaskDialogClosedEventArgs"/> instance containing the event data.</param>
		private static void onClosed(TaskDialogClosedEventArgs e)
		{
            Closed?.Invoke(null, e);
        }
        
		private static TaskDialogResult showEmulatedTaskDialog(TaskDialogOptions options)
		{
			TaskDialog td = new();
			TaskDialogViewModel tdvm = new(options);

			td.DataContext = tdvm;

			if (options.Owner != null)
			{
				td.Owner = options.Owner;
			}

            Theme theme = ThemeManager.Current.DetectTheme(td);
            if (!string.IsNullOrEmpty(options.Theme))
				ThemeManager.Current.ChangeTheme(td, options.Theme);

			td.ShowDialog();
            ThemeManager.Current.ChangeTheme(td, theme);
            int? commandButtonResult = null;
			int? customButtonResult = null;

            int diagResult = tdvm.DialogResult;
            int radioButtonResult = tdvm.RadioResult - RadioButtonIDOffset;
            bool verificationChecked = tdvm.VerificationChecked;

            TaskDialogSimpleResult simpResult;
            if (diagResult >= CommandButtonIDOffset)
            {
                simpResult = TaskDialogSimpleResult.Command;
                commandButtonResult = diagResult - CommandButtonIDOffset;
            }
            //else if (diagResult >= RadioButtonIDOffset)
            //{
            //    simpResult = (TaskDialogSimpleResult)diagResult;
            //    radioButtonResult = diagResult - RadioButtonIDOffset;
            //}
            else if (diagResult >= CustomButtonIDOffset)
            {
                simpResult = TaskDialogSimpleResult.Custom;
                customButtonResult = diagResult - CustomButtonIDOffset;
            }
            // This occurs usually when the red X button is clicked
            else if (diagResult == -1)
            {
                simpResult = TaskDialogSimpleResult.Cancel;
            }
            else
            {
                simpResult = (TaskDialogSimpleResult)diagResult;
            }

            TaskDialogResult result = new(
				simpResult,
				string.IsNullOrEmpty(options.VerificationText) ? null : verificationChecked,
				options.RadioButtons == null || options.RadioButtons.Length == 0 ? null : radioButtonResult,
				options.CommandButtons == null || options.CommandButtons.Length == 0 ? null : commandButtonResult,
				options.CustomButtons == null || options.CustomButtons.Length == 0 ? null : customButtonResult);

            return result;
		}
		private static bool detectHyperlinks(string content, string expandedInfo, string footerText)
		{
			return detectHyperlinks(content) || detectHyperlinks(expandedInfo) || detectHyperlinks(footerText);
		}
		private static bool detectHyperlinks(string text)
		{
			if (String.IsNullOrEmpty(text))
				return false;
			return _hyperlinkRegex.IsMatch(text);
		}
	}
}
