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
    public partial class Add_Tovar : Form
    {

        DataBase DataBase = new DataBase();

        public Add_Tovar()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;//открытие новой формы по центру экрана

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataBase.openConnection();
            var name = textBox1.Text;
            var Count_of = textBox2.Text;
            int Price;
            var Dostavka = textBox4.Text;
            var NomerRyada = textBox5.Text;
            var NomerPolki = textBox6.Text;
            var Grupa = textBox7.Text;

            if (int.TryParse(textBox3.Text, out Price))  //проверка TextBox3  на целое число
            {
                var addQuery = $"INSERT INTO products(Name,Count_of,Price,Dostavka,Number_Ryada,Number_Polki,Grupa) " +
                $"VALUES ('{name}', '{Count_of}','{Price}', '{Dostavka}', '{NomerRyada}', '{NomerPolki}', '{Grupa}')";

                var command = new SqlCommand(addQuery, DataBase.GetConnection());
                command.ExecuteNonQuery();

                MessageBox.Show("Товар прибыл на склад ", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("ОШИБКА\n Цена должна иметь числовой формат", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            DataBase.closeConnection();
        }
    }
}
