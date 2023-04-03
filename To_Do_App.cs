using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Windows.Forms;

namespace To_Do_App_Final_Project
{
    public partial class To_Do_App : Form
    {
        List<Task> tasks = new List<Task>();
        public To_Do_App()
        {
            InitializeComponent();
        }

        private void Addtask(string title, string description, DateTime dueDate)
        {
            Task task = new Task(title, description, dueDate);
            tasks.Add(task);
            dataGridView1.Rows.Add(title,description,dueDate);
            dataGridView1.Columns[2].DefaultCellStyle.Format = "dddd, dd MMMM yyyy";
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (textBoxTitle.Text != "" && textBoxDes.Text != "") 
            {
                string title = textBoxTitle.Text;
                string description = textBoxDes.Text;
                DateTime dateTime = dateTimePickerDate.Value;
                Addtask(title, description, dateTime);

                textBoxTitle.Text = "";
                textBoxDes.Text = "";
            }
            else
            {
                MessageBox.Show("กรุณาใส่รายการให้ครบถ้วน");
            }
            
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "CSV|*.csv|TEXT|*.txt";
            saveFile.ShowDialog();
            if (saveFile.FileName != "")
            {
                using(StreamWriter writer = new StreamWriter(saveFile.FileName))
                {
                    foreach(Task item in tasks)
                    {
                        writer.WriteLine(String.Format("{0},{1},{2}",
                            item.Title,
                            item.Description,
                            item.DueDate.ToString()
                            ));
                    }
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV|*.csv|TEXT|*.txt";
            openFileDialog.ShowDialog();
            using (StreamReader reader = new StreamReader(openFileDialog.FileName))
            {
                while (!reader.EndOfStream)
                {
                    string[] line = reader.ReadLine().Split(',');
                    if (line.Length >= 3)
                    {
                        Task listTask = new Task(
                            line[0],
                            line[1],
                            DateTime.Parse(line[2])
                            );
                        tasks.Add(listTask);
                    }
                }
            }
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = tasks;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Edit")
            {
                DataGridViewRow dr = dataGridView1.Rows[e.RowIndex];
                textBoxTitle.Text = dr.Cells[0].Value.ToString();
                textBoxDes.Text = dr.Cells[1].Value.ToString();

            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "Update")
            {
                DataGridViewRow update = dataGridView1.Rows[e.RowIndex];
                update.Cells[0].Value = textBoxTitle.Text;
                update.Cells[1].Value = textBoxDes.Text;
                update.Cells[2].Value = dateTimePickerDate.Value;
                textBoxTitle.Text = "";
                textBoxDes.Text = "";

            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "Delete")
            {

                DataGridViewRow delete = dataGridView1.Rows[e.RowIndex];
                dataGridView1.Rows.RemoveAt(delete.Index);
            }
        }
    }
}