using System;
using System.Drawing;
using System.Windows.Forms;

namespace YADBS
{
	/// <summary>
	/// Description of SelectForm.
	/// </summary>
	public partial class SelectForm : Form
	{
		public SelectForm()
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
			if(comboBox2.Text == "Or") {
				textBox3.Text += "or";
			}
			
			if(comboBox2.Text == "And") {
				textBox3.Text += "and";
			}
			
			if(comboBox2.Text == "Not") {
				textBox3.Text += "not";
			}
			
			if(comboBox2.Text == "Like") {
				textBox3.Text += "like";
			}
		}
		void Button1Click(object sender, EventArgs e)
		{
			textBox1.Text += comboBox1.Text + "()";
		}
		public string query;
		void Button3Click(object sender, EventArgs e)
		{
			if(checkBox1.Checked)
				query = "select distinct " + textBox1.Text;
			else query = "select " + textBox1.Text;
			
			
			query += " from " + textBox2.Text;
			query += " where " + textBox3.Text;
			if(checkBox2.Checked) {
				query += " group by " + textBox4.Text;
			}
			
			if(checkBox3.Checked) {
				query += " limit " + textBox5.Text;
			}
			
			query += ";";
			
			this.Close();
		}
	}
}