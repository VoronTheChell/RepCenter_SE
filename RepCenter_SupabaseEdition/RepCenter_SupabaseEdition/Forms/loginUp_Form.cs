using RepCenter_SupabaseEdition.Forms;


namespace RepCenter_SupabaseEdition
{
    public partial class loginUp_Form : Form
    {
        public loginUp_Form()
        {
            InitializeComponent();

            login_textBox.MaxLength = 20;
            pass_textBox.MaxLength = 20;

            MaximizeBox = false;
            pass_textBox.UseSystemPasswordChar = true;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;

            login_textBox.KeyDown += TextBox_KeyDown;
            pass_textBox.KeyDown += TextBox_KeyDown;
        }

        private void PictureBoxTogglePassword_Click(object sender, EventArgs e)
        {
            if (pass_textBox.UseSystemPasswordChar)
            {
                pass_textBox.UseSystemPasswordChar = false;
                PictureBoxTogglePassword.Image = Properties.Resources.open;
            }
            else
            {
                pass_textBox.UseSystemPasswordChar = true;
                PictureBoxTogglePassword.Image = Properties.Resources.close;
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.SelectNextControl((Control)sender, true, true, true, true);
            }

        }

        private async void enter_button_Click(object sender, EventArgs e)
        {
            var loginUser = login_textBox.Text;
            var passUser = pass_textBox.Text;

            Inicialization.Icon = SystemIcons.Information;
            Inicialization.Visible = true;
            Inicialization.BalloonTipTitle = "Пожалуйста подождите!";
            Inicialization.BalloonTipText = "Выполняеться инициализация пользователя...";
            Inicialization.BalloonTipIcon = ToolTipIcon.Info;
            Inicialization.ShowBalloonTip(350);

            if (string.IsNullOrWhiteSpace(loginUser) || string.IsNullOrWhiteSpace(passUser))
            {
                MessageBox.Show("Введите логин и пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var service = new SupabaseService();
            await service.InitAsync();

            var query = await service.LoginAsync(loginUser, service.Getsupabase());

            if (query != null)
            {
                string hashedPassword = query.PasswordUser;
                string status = query.StatusUser.Trim();

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(passUser, hashedPassword);

                if (isPasswordValid)
                {
                    MessageBox.Show("Вы успешно вошли!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    Inicialization.Visible = false;
                    
                    login_textBox.Text = "";
                    pass_textBox.Text = "";

                    this.Hide();

                    switch (status)
                    {
                        case "admin":
                            var adminForm = new Admin_Form();
                            adminForm.FormClosed += (s, args) => this.Close();
                            adminForm.Show();
                            this.Show();
                            break;

                        case "учащийся":
                            var scollerForm = new FormScoller();
                            scollerForm.FormClosed += (s, args) => this.Close();
                            scollerForm.Show();
                            this.Show();
                            break;

                        case "учитель":
                            var teacherForm = new FormTeacher();
                            teacherForm.FormClosed += (s, args) => this.Close();
                            teacherForm.Show();
                            this.Show();
                            break;

                        default:
                            MessageBox.Show($"Неизвестный статус пользователя: '{status}'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
                else
                {
                    Inicialization.Icon = SystemIcons.Error;
                    Inicialization.Visible = true;
                    Inicialization.BalloonTipTitle = "Ошибка входа!";
                    Inicialization.BalloonTipText = "Был указан неверный пароль!";
                    Inicialization.BalloonTipIcon = ToolTipIcon.Error;
                    Inicialization.ShowBalloonTip(3000);
                }
            }
            else
            {
                Inicialization.Icon = SystemIcons.Error;
                Inicialization.Visible = true;
                Inicialization.BalloonTipTitle = "Ошибка входа!";
                Inicialization.BalloonTipText = "Такой пользователь не найден!";
                Inicialization.BalloonTipIcon = ToolTipIcon.Error;
                Inicialization.ShowBalloonTip(3000);
            }
        }

        private void Reg_link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            singUp_Form singUp = new singUp_Form();

            this.Hide();
            singUp.ShowDialog();
            this.Show();
        }
    }
}
