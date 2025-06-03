namespace RepCenter_SupabaseEdition.Forms
{
    partial class about_form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(about_form));
            pictureBox1 = new PictureBox();
            label1 = new Label();
            button1 = new Button();
            listBox1 = new ListBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.ikonka;
            pictureBox1.Location = new Point(12, 89);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(121, 120);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.Location = new Point(198, 22);
            label1.Name = "label1";
            label1.Size = new Size(224, 40);
            label1.TabIndex = 1;
            label1.Text = "RepCenter APP";
            // 
            // button1
            // 
            button1.Location = new Point(509, 231);
            button1.Name = "button1";
            button1.Size = new Size(86, 26);
            button1.TabIndex = 2;
            button1.Text = "OK";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // listBox1
            // 
            listBox1.AllowDrop = true;
            listBox1.BackColor = SystemColors.Menu;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 17;
            listBox1.Items.AddRange(new object[] { "RepCenter — это лёгкое и удобное настольное приложение ", "для Windows, предназначенное для автоматизации процессов ", "в репетиторском центре. ", "Программа позволяет эффективно управлять базой студентов ", "и преподавателей, составлять расписание занятий, ", "отслеживать посещаемость и успеваемость, вести учёт платежей ", "и формировать отчётность.", "", "Приложение работает на базе Supabase — облачной платформы ", "с поддержкой REST API что позволяет безопасно хранить ", "и обрабатывать данные без необходимости ", "развёртывания собственного сервера." });
            listBox1.Location = new Point(142, 80);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(452, 140);
            listBox1.TabIndex = 3;
            // 
            // about_form
            // 
            AutoScaleDimensions = new SizeF(8F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(608, 271);
            Controls.Add(listBox1);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 204);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "about_form";
            ShowInTaskbar = false;
            Text = "О приложении";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label1;
        private Button button1;
        private ListBox listBox1;
    }
}