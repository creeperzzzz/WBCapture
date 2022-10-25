using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WBCapture
{
	public partial class Form1 : Form
	{
		Timer m_timer;
		Rectangle m_rect;
		Bitmap m_bitmap;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			m_timer = new Timer();
			m_timer.Tick += timer_Tick;

			if (Properties.Settings.Default.WindowPosition != new Point(7575, 7575))
			{
				Location = Properties.Settings.Default.WindowPosition;
				Size = Properties.Settings.Default.WindowSize;
			}
			m_rect = new Rectangle(
				Properties.Settings.Default.CapturePosition,
				Properties.Settings.Default.CaptureSize);
			m_timer.Interval = Properties.Settings.Default.Interval;
			chkTop.Checked = Properties.Settings.Default.TopMost;

			m_timer.Start();

			updateRect();
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			using (Graphics g = Graphics.FromImage(m_bitmap))
			{
				g.CopyFromScreen(m_rect.Location, new Point(0, 0), m_rect.Size);
			}
			//Invalidate();
			pictureBox1.Invalidate();
		}

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawImage(m_bitmap, Point.Empty);
		}

		private void chkTop_CheckedChanged(object sender, EventArgs e)
		{
			TopMost = chkTop.Checked;
		}

		private void updateRect()
		{
			m_bitmap = new Bitmap(m_rect.Width, m_rect.Height);
			pictureBox1.Image = m_bitmap;

			label2.Text = string.Format("{0}, {1} / {2}, {3}", m_rect.Left, m_rect.Top, m_rect.Width, m_rect.Height);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			m_rect = pictureBox1.RectangleToScreen(new Rectangle(Point.Empty, pictureBox1.Size));
			updateRect();
		}

		private void textInterval_TextChanged(object sender, EventArgs e)
		{
			try
			{
				m_timer.Interval = Convert.ToInt32(textInterval.Text);
			}
			catch
			{
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			Properties.Settings.Default.Interval = m_timer.Interval;
			Properties.Settings.Default.WindowPosition = this.Location;
			Properties.Settings.Default.WindowSize = this.Size;
			Properties.Settings.Default.CapturePosition = m_rect.Location;
			Properties.Settings.Default.CaptureSize = m_rect.Size;
			Properties.Settings.Default.TopMost = this.TopMost;
			Properties.Settings.Default.Save();
		}
	}
}
