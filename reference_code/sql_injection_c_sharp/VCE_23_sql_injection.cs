/*
 * Created by SharpDevelop.
 * User: gohmi
 * Date: 28/05/2019
 * Time: 5:08 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace YADBS
{
	/// <summary>
	/// Description of ConnectForm.
	/// </summary>
	public partial class ConnectForm : Form
	{
		public ConnectForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void Button2Click(object sender, EventArgs e)
		{
			try {
			this.Close();
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
			}
		}
		
		public string connectionName = "";
		public string server = "";
		public string database = "";
		public string port = "";
		public string user = "";
		public string passwords = "";
		
		void Button1Click(object sender, EventArgs e)
		{
			try {
			server = textBox1.Text;
			database = textBox2.Text;
			user = textBox3.Text;
			
			port = textBox5.Text;
			connectionName = textBox6.Text;
			
			List<string> res = new List<string>();
			StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "/connection.ini");
			string line = "";
			
			while((line = sr.ReadLine()) != null) {
				res.Add(line);
			}
			
			sr.Close();
			
			
			if(res.Contains(connectionName)) 
				MessageBox.Show("Connection name");
			
			else {
			
			StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "\\connection.ini", true);
			sw.WriteLine("[ConnectionName]" + connectionName);
			sw.WriteLine("[Server]" + server);
			sw.WriteLine("[Database]" + database);
			sw.WriteLine("[port]" + port);
			sw.WriteLine("[User]" + user);
			sw.Close();
			
			}
			
			this.Close();
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
			}
		}
	}
}