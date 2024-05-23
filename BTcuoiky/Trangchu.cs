using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTcuoiky
{
    public partial class Trangchu : Form
    {
        private Button currentButton;

        private void HighlightButton(Button btn)
        {
            if (currentButton != null)
            {
                ResetButtonColor();
            }

            currentButton = btn;
            currentButton.BackColor = Color.FromArgb(72, 209, 204);
            // Các hành động khác
        }

        private void ResetButtonColor()
        {
            if (currentButton != null)
            {
                currentButton.BackColor = Color.FromArgb(255, 0, 139, 139);
            }
        }
    

        public Trangchu()
        {
            InitializeComponent();
            customizeDesing();
        }
        private void customizeDesing()
        {
            panel_danhmuc.Visible = false;
            panel_quanli.Visible = false;
            panel_hethong.Visible = false;
            panel_thongke.Visible = false;

        }
        private void hideSubmenu()
        {
            if (panel_danhmuc.Visible == true)
                panel_danhmuc.Visible = false;
            if (panel_quanli.Visible == true)
                panel_quanli.Visible = false;
            if (panel_hethong.Visible == true)
                panel_hethong.Visible = false;
            if (panel_thongke.Visible == true)
                panel_thongke.Visible = false;

        }
        private void showSubMenu(Panel subMeunu)
        {
            if (subMeunu.Visible == false)
            {
                hideSubmenu();
                subMeunu.Visible = true;
            }
            else
                subMeunu.Visible = false;

        }
        private Form activeForm = null;
        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panel_body.Controls.Add(childForm);
            panel_body.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void button_Danhmuc_Click(object sender, EventArgs e)
        {
            showSubMenu(panel_danhmuc);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                HighlightButton(clickedButton);
            }
            openChildForm(new Nhanvien());
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                HighlightButton(clickedButton);
            }
            openChildForm(new Benhnhan());
            ;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                HighlightButton(clickedButton);
            }
            openChildForm(new Thuoc());
           
        }

        private void button_quanli_Click(object sender, EventArgs e)
        {
            showSubMenu(panel_quanli);

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                HighlightButton(clickedButton);
            }
            openChildForm(new Phieukhambenh());
           
           
        }

        private void button_hethong_Click(object sender, EventArgs e)
        {
            showSubMenu(panel_hethong);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                HighlightButton(clickedButton);
            }
            openChildForm(new DoiMK());
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                HighlightButton(clickedButton);
            }
            openChildForm(new Thongtinnhanvien());
            
        }

        private void button_thongke_Click(object sender, EventArgs e)
        {
            showSubMenu(panel_thongke);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                HighlightButton(clickedButton);
            }
            openChildForm( new Thongkethuochethan());
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                HighlightButton(clickedButton);
            }
            openChildForm(new Doanhthu());
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                HighlightButton(clickedButton);
            }
            openChildForm(new Hoadon());
           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                HighlightButton(clickedButton);
            }
            openChildForm(new Donthuoc());
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                HighlightButton(clickedButton);
            }
            openChildForm(new Dichvu());
           
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                HighlightButton(clickedButton);
            }
            openChildForm(new Phongkham());
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                HighlightButton(clickedButton);
            }
            openChildForm(new Dangkytaikhoan());
           
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
