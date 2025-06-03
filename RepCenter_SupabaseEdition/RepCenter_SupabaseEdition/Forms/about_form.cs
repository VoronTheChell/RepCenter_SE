using NAudio.Wave;
using System.Media; // для работы с .wav
using System.Reflection;
using System.Windows.Media;

namespace RepCenter_SupabaseEdition.Forms
{
    public partial class about_form : Form
    {
        public about_form()
        {
            InitializeComponent();

            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }
    }
}
