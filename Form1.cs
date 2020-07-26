using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.Drawing;
using System.Linq;

namespace Document_arrangement
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		string folderPath_1, folderPath_2;
		List<string> targetList = new List<string>();
		List<string> targetList_path = new List<string>();
		public void down(string str)  //listBox自动到底部
		{
			bool scroll = false;
			if (this.listBox1.TopIndex == this.listBox1.Items.Count - (int)(this.listBox1.Height / this.listBox1.ItemHeight))
				scroll = true;
			this.listBox1.Items.Add(str);
			if (scroll)
				this.listBox1.TopIndex = this.listBox1.Items.Count - (int)(this.listBox1.Height / this.listBox1.ItemHeight);
		}
		private void button1_Click(object sender, EventArgs e)  //选择源文件夹按钮
		{
			FolderBrowserDialog folder = new FolderBrowserDialog();
			folder.Description = "选择文件夹";
			if (folder.ShowDialog() == DialogResult.OK)
			{
				folderPath_1 = folder.SelectedPath;
				textBox1.Text = folderPath_1;
				down("源文件夹路径 " + folderPath_1);
			}
		}

		private void button2_Click(object sender, EventArgs e)  //选择目标文件夹按钮
		{
			FolderBrowserDialog folder = new FolderBrowserDialog();
			folder.Description = "选择文件夹";
			if (folder.ShowDialog() == DialogResult.OK)
			{
				folderPath_2 = folder.SelectedPath;
				textBox2.Text = folderPath_2;
				down("目标文件夹路径 " + folderPath_2);
			}
		}

		private void button3_Click(object sender, EventArgs e)  //开始整理按钮
		{
			if (folderPath_1 == null || folderPath_2 == null)
			{
				MessageBox.Show("文件夹路径不能为空");
			}
			else
			{
				if (textBox3.Text == "")
				{
					MessageBox.Show("请输入文件后缀");
				}
				else
				{
					for (int z = 0; z < targetList.Count; z++)
					{
						var name = targetList[z];
						var name_path = targetList_path[z];
						string[] result = null;
						if (radioButton1.Checked)
							result = Directory.GetFiles(@folderPath_1, "*" + name + "*.doc?", SearchOption.TopDirectoryOnly);
						if (radioButton2.Checked)
							result = (string[])Directory.GetFiles(@folderPath_1, "*" + name + "*.*", SearchOption.TopDirectoryOnly);
						if (result.Length == 0)
						{
							down("***未找到" + name + "的作业***");
						}
						else
						{
							if (radioButton1.Checked)
							{
								foreach (var file in result)
								{
									var a = Path.GetFileNameWithoutExtension(file);
									var b = Path.GetExtension(file);
									var c = Path.GetFileName(file);
									File.Move(@file, @name_path + "\\" + a + textBox3.Text + b);
									down("文件" + c + "移动成功");
								}
							}
							if (radioButton2.Checked)
							{
								foreach (var file in result)
								{
									var a = Path.GetFileNameWithoutExtension(file);
									var b = Path.GetExtension(file);
									var c = Path.GetFileName(file);
									Directory.Move(@file, @name_path + "\\" + a + textBox3.Text + b);
									down("文件" + a + "移动成功");
								}
							}
						}
					}
					down("+++++++++文件整理结束+++++++++");
				}
			}

		}

		public void ScanFile()  //读取目标文件夹下的所有文件夹名字
		{
			var directories = Directory.GetDirectories(folderPath_2);
			down("---------------------------------------");
			down("目标文件夹列表：");
			foreach (var name in directories)
			{
				targetList_path.Add(name);
				var a0 = Path.GetFileName(name);
				string a = System.Text.RegularExpressions.Regex.Replace(a0, @"[^0-9]+", "");
				targetList.Add(a);
				down(a);
			}
			down("++++++共有" + directories.Length + "个学生文件夹++++++");
			down("---------------------------------------");
		}

		private void listBox1_DrawItem(object sender, DrawItemEventArgs e)  //listBox显示红色字体
		{
			e.DrawBackground();
			if (listBox1.Items[e.Index].ToString().Substring(0, 1) == "*")  //如果首字带*号，则红字显示
			{
				e.Graphics.DrawString(((ListBox)sender).Items[e.Index].ToString(), e.Font, new SolidBrush(Color.Red), e.Bounds);
			}
			else if (listBox1.Items[e.Index].ToString().Substring(0, 1) == "+")
			{
				e.Graphics.DrawString(((ListBox)sender).Items[e.Index].ToString(), e.Font, new SolidBrush(Color.Blue), e.Bounds);
			}
			else
			{
				e.Graphics.DrawString(((ListBox)sender).Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);
			}
			e.DrawFocusRectangle();
		}

		private void button4_Click(object sender, EventArgs e)  //获取源文件夹下的所有文件名
		{
			if (folderPath_1 == null || folderPath_2 == null)  //判断路径是否为空
			{
				MessageBox.Show("文件夹路径不能为空");
			}
			else
			{
				//读取源文件夹下的所有文件
				if (radioButton1.Checked)  //文件类型
				{
					var fileName = Directory.GetFiles(@folderPath_1, "*.doc?", SearchOption.TopDirectoryOnly);
					down("源文件夹列表：");
					foreach (var name in fileName)
					{
						down(Path.GetFileName(name));
					}
					down("++++++文件：" + fileName.Length + "个++++++");
					ScanFile();
				}
				else if (radioButton2.Checked)
				{
					var fileName = Directory.GetFiles(@folderPath_1, "*.*", SearchOption.TopDirectoryOnly);
					down("源文件夹列表：");
					foreach (var name in fileName)
					{
						down(Path.GetFileName(name));
					}
					down("++++++文件：" + fileName.Length + "个++++++");
					ScanFile();
				}
				else
				{
					MessageBox.Show("你没有选择类型");
				}
			}
		}
	}
}
