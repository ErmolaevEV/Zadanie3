using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zadanie3
{
    public partial class Form3 : Form
    {
        MySqlConnection conn;
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        private BindingSource bSource = new BindingSource();
        private DataSet ds = new DataSet();
        private DataTable table = new DataTable();
        string id_selected_rows = "0";

        public Form3()
        {
            InitializeComponent();
        }

        public void reload_list()
        {
            
            table.Clear();
            GetListUsers();
        }

        public void GetListUsers()
        {
            //Объявление запроса
            string sqlQueryLoadUsers = "SELECT " +
                "User_esti.idUsers, " +
                "User_esti.loginUsers, " +
                "User_esti.passUsers, " +
                "User_esti.fioUsers, 	" +
                "User_esti.enabledUsers, 	" +
                "User_Role.titleRole, 	" +
                "FROM " +
                "User_esti 	" +
                "INNER JOIN 	" +
                "T_Role 	" +
                "ON 		" +
                "T_Users.roleUsers = T_Role.idRole";

            //Открываем соединение
            conn.Open();
            //Объявляем команду, которая выполнить запрос в соединении conn
            MyDA.SelectCommand = new MySqlCommand(sqlQueryLoadUsers, conn);
            MyDA.Fill(table);
            bSource.DataSource = table; 
            dataGridView1.DataSource = bSource;
            conn.Close();

            int count_rows = dataGridView1.RowCount;
            toolStripLabel2.Text = (count_rows).ToString();
        }

        public void DeleteUser()
        {
            //Формируем строку запроса на добавление строк
            string sql_delete_user = "DELETE FROM T_Users WHERE idUsers='" + id_selected_rows + "'";
            //Посылаем запрос на обновление данных
            MySqlCommand delete_user = new MySqlCommand(sql_delete_user, conn);
            try
            {
                conn.Open();
                delete_user.ExecuteNonQuery();
                MessageBox.Show("Удаление прошло успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления строки \n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            finally
            {
                conn.Close();
                //Вызов метода обновления ДатаГрида
                reload_list();
            }

        }

        private void Component_UserMgmt_Load(object sender, EventArgs e)
        {
            string connStr = "server=10.80.1.7;port=3306;user=st_60;database=is_60_EKZ;password=123456789;";
            conn = new MySqlConnection(connStr);

            GetListUsers();

            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[1].Visible = true;
            dataGridView1.Columns[2].Visible = true;
            dataGridView1.Columns[3].Visible = true;
            dataGridView1.Columns[4].Visible = true;

            dataGridView1.Columns[0].FillWeight = 15;
            dataGridView1.Columns[1].FillWeight = 40;
            dataGridView1.Columns[2].FillWeight = 15;
            dataGridView1.Columns[3].FillWeight = 15;
            dataGridView1.Columns[4].FillWeight = 15;

            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[4].ReadOnly = true;

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = true;
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteUser();
        }

        public void GetSelectedIDString()
        {
            string index_selected_rows;
            index_selected_rows = dataGridView1.SelectedCells[0].RowIndex.ToString();
            id_selected_rows = dataGridView1.Rows[Convert.ToInt32(index_selected_rows)].Cells[0].Value.ToString();
            toolStripLabel4.Text = id_selected_rows;
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];
            dataGridView1.CurrentRow.Selected = true;
            GetSelectedIDString();
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!e.RowIndex.Equals(-1) && !e.ColumnIndex.Equals(-1) && e.Button.Equals(MouseButtons.Right))
            {
                dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];
                dataGridView1.CurrentCell.Selected = true;
                GetSelectedIDString();
            }
        }

        public void ChangeStatusEmploy(string new_state)
        {
            string redact_id = id_selected_rows;
            string query2 = $"UPDATE T_Users SET enabledUsers='{new_state}' WHERE (idUsers='{id_selected_rows}')";
            MySqlCommand command = new MySqlCommand(query2, conn);


            try
            {
                conn.Open();
                command.ExecuteNonQuery();
                MessageBox.Show("Изменение статуса прошло успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка изменения строки \n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            finally
            {
                conn.Close();
                reload_list();
            }
        }

        private void активенToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeStatusEmploy("1");
        }

        private void неактивенToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeStatusEmploy("0");
        }
    }
}
