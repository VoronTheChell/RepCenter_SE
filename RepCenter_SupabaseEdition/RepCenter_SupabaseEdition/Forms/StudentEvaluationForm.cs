using System.Drawing.Printing;
using System.Windows.Forms;
namespace RepCenter_SupabaseEdition.Forms
{
    public partial class StudentEvaluationForm : Form
    {
        public StudentEvaluationForm()
        {
            InitializeComponent();

            Task LSTCBA = LoadScoolersToComboBoxAsync();
            Task LSuTCBA = LoadSubjectsToComboBoxAsync();

            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;

            SelectScooler_CB.DropDownStyle = ComboBoxStyle.DropDownList;
            PridmetSelect_CB.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private async Task LoadScoolersToComboBoxAsync()
        {
            SelectScooler_CB.Items.Clear();
            var service = new SupabaseService();
            await service.InitAsync();

            var response = await service.supabase.From<Student>().Get();
            foreach (var student in response.Models)
                SelectScooler_CB.Items.Add(student.FIO);
        }

        private async Task LoadSubjectsToComboBoxAsync()
        {
            PridmetSelect_CB.Items.Clear();
            var service = new SupabaseService();
            await service.InitAsync();

            var response = await service.supabase.From<Pridments>().Get();
            foreach (var subject in response.Models)
                PridmetSelect_CB.Items.Add(subject.NamePridment);
        }

        private async Task<int> GetIDByNameStudentAsync(string name)
        {
            var service = new SupabaseService();
            await service.InitAsync();

            var response = await service.supabase
                .From<Student>()
                .Filter("fio", Supabase.Postgrest.Constants.Operator.Equals, name)
                .Get();

            return response.Models.FirstOrDefault()?.StudentId ?? -1;
        }

        private void SaveGradeToFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Fileшs (*.pdf)|*.pdf",
                Title = "Сохранить оценку"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                if (filePath.EndsWith(".pdf"))
                {
                    SaveToPdf(filePath);
                }

                MessageBox.Show("Файл успешно сохранен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SaveToPdf(string filePath)
        {
            // Создаем документ PDF
            var pdfDocument = new Aspose.Pdf.Document();
            var page = pdfDocument.Pages.Add();

            // Настраиваем шрифт и форматирование текста
            var textFragment = new Aspose.Pdf.Text.TextFragment("Оценка учащегося")
            {
                TextState =
                {
                    FontSize = 16,
                    Font = Aspose.Pdf.Text.FontRepository.FindFont("Arial"),
                    FontStyle = Aspose.Pdf.Text.FontStyles.Bold
                }
            };
            page.Paragraphs.Add(textFragment);

            // Добавляем данные об оценке
            var studentInfo = new Aspose.Pdf.Text.TextFragment(
                $"Имя: {SelectScooler_CB.Text}\n" +
                $"Предмет: {PridmetSelect_CB.Text}\n" +
                $"Оценка: {valuesSucsses_TB.Value}/50 баллов.\n" +
                $"Комментарий: {plusDesc_TB.Text}"
            )
            {
                TextState =
                {
                    FontSize = 12,
                    Font = Aspose.Pdf.Text.FontRepository.FindFont("Arial")
                }
            };
            page.Paragraphs.Add(studentInfo);

            // Сохраняем PDF в файл
            pdfDocument.Save(filePath);

            MessageBox.Show($"Оценка успешно сохранена в PDF:\n{filePath}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void PrintGrade()
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += (sender, e) =>
            {
                string gradeDetails = $"Результат обучение ученика:" +
                                      $"Имя: {SelectScooler_CB.Text}\n" +
                                      $"Предмет: {PridmetSelect_CB.Text}\n" +
                                      $"Оценка: {valuesSucsses_TB.Value}/50 баллов.\n" +
                                      $"Комментарий: {plusDesc_TB.Text}";
                e.Graphics.DrawString(gradeDetails, new System.Drawing.Font("Arial", 12), Brushes.Black, new PointF(100, 100));
            };

            PrintDialog printDialog = new PrintDialog
            {
                Document = printDocument
            };

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();

                notifyIcon1.Visible = true;
                notifyIcon1.BalloonTipTitle = "Успех!";
                notifyIcon1.BalloonTipText = "Документ успешно отправлен на печать!";
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon1.ShowBalloonTip(350);
            }
        }

        private void AskToExportGrade()
        {
            DialogResult result = MessageBox.Show(
                "Вы хотите распечатать или сохранить оценку учащегося?\n\n" +
                "Нажмите \"Да\" для сохранения в PDF, \"Нет\" для печати.",
                "Сохранение или печать оценки",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            switch (result)
            {
                case DialogResult.Yes:
                    SaveGradeToFile();
                    break;
                case DialogResult.No:
                    PrintGrade();
                    break;
                case DialogResult.Cancel:
                    MessageBox.Show("Действие отменено.", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        private async void buttonPublic_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SelectScooler_CB.Text) ||
                string.IsNullOrWhiteSpace(PridmetSelect_CB.Text) ||
                string.IsNullOrWhiteSpace(plusDesc_TB.Text))
            {
                MessageBox.Show("Пустые значения, пожалуйста, введите!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int studentId = await GetIDByNameStudentAsync(SelectScooler_CB.Text);
            string subject = PridmetSelect_CB.Text;
            decimal grade = valuesSucsses_TB.Value;
            string comments = plusDesc_TB.Text;

            var service = new SupabaseService();
            await service.InitAsync();

            var newGrade = new StudentGrades
            {
                StudentId = studentId,
                Subject = subject,
                Grade = grade,
                Comments = comments
            };

            var response = await service.supabase.From<StudentGrades>().Insert(newGrade);

            if (response.Models.Count > 0)
            {
                MessageBox.Show("Оценка успешно сохранена!");
                AskToExportGrade();
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка при сохранении оценки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void valuesSucsses_TB_Scroll(object sender, EventArgs e)
        {
            var values = valuesSucsses_TB.Value;
            ScoreLable.Text = values.ToString();

            if (values <= 4)
            {
                ScoreLable.ForeColor = Color.Red;
            }

            else if (values >= 5 && values <= 7)
            {
                ScoreLable.ForeColor = Color.Yellow;
            }

            else if (values >= 8)
            {
                ScoreLable.ForeColor = Color.Green;
            }
        }
    }
}
