/*
 * Created by SharpDevelop.
 * User: gohmi
 * Date: 01/06/2019
 * Time: 12:31 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace YADBS
{
	/// <summary>
	/// Description of DeleteForm.
	/// </summary>
	public partial class DeleteForm : Form
	{
		public DeleteForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		public string query;
		void Button2Click(object sender, EventArgs e)
		{
			query = "DELETE FROM " + textBox1.Text + " WHERE " + textBox2.Text + ";";
			this.Close();
		}
		void Button1Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}