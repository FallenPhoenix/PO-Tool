/*
 * Created with SharpDevelop 3.
 * User: F. Phoenix
 * Date: 08.01.2012
 * Time: 10:29
 * 
 */
using System;
using System.Windows.Forms;

namespace PO_Tool
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
		
	}
}
