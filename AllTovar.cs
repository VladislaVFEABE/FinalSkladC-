using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Final
{
    enum RowState
    {
        Existed,
        New,
        Modified,
        ModifiedNew,
        Deleted
    }


    public partial class AllTovar : Form
    {
        private readonly checkUser _user;

        DataBase DataBase = new DataBase();

        int selectedRow;

        public AllTovar(checkUser user)
        {

            _user = user;
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;//открытие новой формы по центру экрана

        }

        private void isAdmin()
        {

            toolStripMenuItem1.Enabled = _user.IsAdmin;
            newTovar.Enabled = _user.IsAdmin;
            button2.Enabled = _user.IsAdmin;
            button3.Enabled = _user.IsAdmin;
            button4.Enabled = _user.IsAdmin;
        }


        private void CreateColumns()
        {
            dataGridView1.Columns.Add("ID", "ID");
            dataGridView1.Columns.Add("Name", "Наименование товара");
            dataGridView1.Columns.Add("Count_of", "Количество");
            dataGridView1.Columns.Add("Price", "Цена");
            dataGridView1.Columns.Add("Dostavka", "Поставщик");
            dataGridView1.Columns.Add("Number_Ryada", "Номер Ряда");
            dataGridView1.Columns.Add("Number_Polki", "Номер Полки");
            dataGridView1.Columns.Add("Grupa", "Группа");
            dataGridView1.Columns.Add("IsNew", String.Empty);
        }

        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetInt32(2), record.GetInt32(3), record.GetString(4),
                record.GetInt32(5), record.GetInt32(6), record.GetString(7), RowState.ModifiedNew);
        }

        private void RefreshData(DataGridView dgw) //Метод выводящий данные в таблицу из БД
        {
            Clear();
            dgw.Rows.Clear();//очистка строк

            string Query = $"SELECT * FROM products";
            SqlCommand command = new SqlCommand(Query, DataBase.GetConnection());
            DataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();


        }

        //private void toolStripButton1_Click(object sender, EventArgs e)
        //{
        //    SqlConnection sql = new SqlConnection(@"Data Source=DESKTOP-081C48G;Initial Catalog=Cursovaya;Integrated Security=True");
        //    sql.Close();
        //}

        private void AllTovar_Load(object sender, EventArgs e)
        {
            toolStripTextBox1.Text = $"{_user.Login}, Role:{_user.Status}";
            isAdmin();
            CreateColumns();
            RefreshData(dataGridView1);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if(e.RowIndex >= 0)//Если rowIndex > 0 тогда передаем данные
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow]; //выносим данные из полей datagridview в форму
                textBox4.Text = row.Cells[0].Value.ToString(); 
                textBox5.Text = row.Cells[1].Value.ToString();
                textBox6.Text = row.Cells[2].Value.ToString();
                textBox7.Text = row.Cells[3].Value.ToString();
                textBox8.Text = row.Cells[4].Value.ToString();
                textBox9.Text = row.Cells[5].Value.ToString();
                textBox10.Text = row.Cells[6].Value.ToString();
                textBox11.Text = row.Cells[7].Value.ToString();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            RefreshData(dataGridView1);
            
        }

        private void newTovar_Click(object sender, EventArgs e) //метод добавление товара
        {
            Add_Tovar add_Tovar = new Add_Tovar();
            add_Tovar.Show();
        }

        private void textSearch_TextChanged(object sender, EventArgs e) //метод поиска по ряду
        {
            
        }

        private void textSearch2_TextChanged(object sender, EventArgs e) //метод поиска по полочке
        {
            
        }

        private void textSearch3_TextChanged(object sender, EventArgs e) //метод поиска по группе
        {
            
        }

        private void Search(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string SearchQuery = $"SELECT * from products WHERE concat(ID,Name,Count_of,Price,Dostavka,Number_Ryada,Number_Polki,Grupa) like '%" +textBox11+ "%'";

            SqlCommand command = new SqlCommand(SearchQuery, DataBase.GetConnection());

            DataBase.openConnection();
            SqlDataReader SqlRead = command.ExecuteReader();

            while (SqlRead.Read())
            {
                ReadSingleRow(dgw, SqlRead);
            }
            SqlRead.Close();

        }
        private void DeleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex; //Передаем индекс строки в которой находимся
            dataGridView1.Rows[index].Visible = false;
            if(dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView1.Rows[index].Cells[8].Value = RowState.Deleted;
                return;
            }
            dataGridView1.Rows[index].Cells[8].Value = RowState.Deleted;
        }

        private void Update()
        {
            DataBase.openConnection();
            for(int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var RowState = (RowState)dataGridView1.Rows[index].Cells[8].Value;

                if (RowState == RowState.Existed)
                {
                    continue;
                }
                    
                if(RowState == RowState.Deleted)
                {
                    var ID = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var Delete = $"DELETE FROM products WHERE ID = {ID}";

                    var command = new SqlCommand(Delete, DataBase.GetConnection());
                    command.ExecuteNonQuery();
                }

                if (RowState == RowState.Modified)
                {
                    var ID = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var Name = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var Count_of = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    var Price = dataGridView1.Rows[index].Cells[3].Value.ToString();
                    var Dostavka = dataGridView1.Rows[index].Cells[4].Value.ToString();
                    var Number_Ryada = dataGridView1.Rows[index].Cells[5].Value.ToString();
                    var Number_Polki = dataGridView1.Rows[index].Cells[6].Value.ToString();
                    var Grupa = dataGridView1.Rows[index].Cells[7].Value.ToString();

                    var ChangeQuery = $"UPDATE products SET Name = '{Name}', Count_of = '{Count_of}', Price = '{Price}', Dostavka = '{Dostavka}' , Number_Ryada = '{Number_Ryada}, Number_Polki = '{Number_Polki}', Grupa = '{Grupa}' WHERE ID = '{ID}'";

                    var command = new SqlCommand(ChangeQuery, DataBase.GetConnection());
                    command.ExecuteNonQuery();
                }

            }
          
            DataBase.closeConnection();
            
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Clear();
            DeleteRow();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            Update();
        }

        private void Change()
        {
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
            var id = textBox4.Text;
            var Name = textBox5.Text;
            var Count_of =  textBox6.Text;
            var Dostavka = textBox8.Text;
            var Number_Ryada = textBox9.Text;
            var Number_Polki = textBox10.Text;
            var Grupa = textBox11.Text;
            int price;
            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                if (int.TryParse(textBox7.Text, out price))
                {
                    dataGridView1.Rows[selectedRowIndex].SetValues(id, Name, Count_of, Dostavka,Number_Ryada,Number_Polki,Grupa,price);
                    dataGridView1.Rows[selectedRowIndex].Cells[8].Value = RowState.Modified;
                }
                else
                {
                    MessageBox.Show("Цена должна иметь числовой формат", "Уведомление");
                }

            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            Change();
            Clear();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void Clear()
        {
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Admin_Panel admin = new Admin_Panel();
            this.Hide();
            admin.ShowDialog();
            this.Show();
        }
    }
}
