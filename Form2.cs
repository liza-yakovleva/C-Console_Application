using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace L2_1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Form2 is closing"); // Перевірка
            this.Close();

           
        }
        private void AutoSizeColumns(DataGridView dgv)
        {
            for (int i = 0; i < dgv.Columns.Count; i++)
                dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }
        public static DataTable GetSQLTable(string Query)
        {
            string DB = "Data Source=(LocalDB)\\MSSQLLocalDB; Initial Catalog='Power Plants';Integrated Security=True";
            using (SqlConnection Conn = new SqlConnection(DB))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                Conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(Query, DB);
                da.Fill(ds);
                dt = ds.Tables[0];
                return dt;
            }
        }


        public static void MODIFY(string Query)
        {
            string DB = "Data Source=(LocalDB)\\MSSQLLocalDB; Initial Catalog='Power Plants';Integrated Security=True";
            using (SqlConnection Conn = new SqlConnection(DB))
            {
                Conn.Open();
                SqlCommand Comm = new SqlCommand(Query, Conn);
                MessageBox.Show(Comm.ExecuteNonQuery().ToString());
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
           
            string ID = textBox5.Text;
            string Name = textBox4.Text;
            string Description = textBox3.Text;

            if (radioButton8.Checked) // Отримання всіх напрямків діяльності
            {
                DataTable T = GetSQLTable("SELECT * FROM Fuel");
                DGV1.DataSource = T;
                AutoSizeColumns(DGV1);
            }
            else if (radioButton7.Checked) // Додавання нового напрямку діяльності
            {
                if (!IsValidName(Name))
                {
                    MessageBox.Show("Назва напрямку повинна містити лише букви та бути не більше 50 символів.", "Помилка формату");
                    return;
                }

                if (string.IsNullOrWhiteSpace(Description))
                {
                    MessageBox.Show("Опис напрямку не може бути порожнім.", "Помилка формату");
                    return;
                }
                int affectedRows = ExecuteNonQuery($"INSERT INTO Fuel (FuelName, FuelDescription) VALUES (N'{Name}', N'{Description}')");
                MessageBox.Show($"{affectedRows} напрямків діяльності успішно додано!");
            }
            else if (radioButton6.Checked) // Оновлення існуючого напрямку діяльності
            {
                if (!PassFormatID(ID))
                {
                    MessageBox.Show("Невірний формат ID!", "Помилка формату");
                    return;
                }

                if (!IsValidName(Name))
                {
                    MessageBox.Show("Назва напрямку повинна містити лише букви та бути не більше 50 символів.", "Помилка формату");
                    return;
                }

                if (string.IsNullOrWhiteSpace(Description))
                {
                    MessageBox.Show("Опис напрямку не може бути порожнім.", "Помилка формату");
                    return;
                }

                int affectedRows = ExecuteNonQuery($"UPDATE Fuel SET FuelName=N'{Name}', FuelDescription=N'{Description}' WHERE ID={ID}");
                MessageBox.Show($"{affectedRows} напрямків діяльності успішно оновлено!");
            }
            else if (radioButton5.Checked) // Видалення напрямку діяльності
            {
                if (!PassFormatID(ID))
                {
                    MessageBox.Show("Невірний формат ID!", "Помилка формату");
                    return;
                }

                int affectedRows = ExecuteNonQuery($"DELETE FROM Fuel WHERE ID={ID}");
                MessageBox.Show($"{affectedRows} напрямків діяльності успішно вилучено!");
            }
        }
        private int ExecuteNonQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB; Initial Catalog='Power Plants'; Integrated Security=True"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    return command.ExecuteNonQuery();
                }
            }
        }
        public bool IsValidName(string name)
        {
            Regex regex = new Regex("^[A-Za-zА-Яа-яЁёІіЇїЄє]{1,50}$");
            return regex.IsMatch(name);
        }
        public bool PassFormatID(string id)
        {
            // Перевірка, чи ID є дійсним числом
            Regex regex = new Regex("^[0-9]+$");
            return regex.IsMatch(id);
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}

