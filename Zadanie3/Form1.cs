using MySql.Data.MySqlClient;

namespace Zadanie3
{

    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            string connStr = "server=pma.sdlik.ru; port=62002; user=st_5; databes=is_5_EKZ; password=123456789;";
            MySqlConnection conn;
            conn = new MySqlConnection(connStr);
            conn.Open();
            string querySql = $"SELECT COUNT(*) FROM ST_5_T WHERE loginUsers='{login}' and passUsers='{password}' and enabledUsers=1";
            MySqlCommand AuthCom = new MySqlCommand(querySql, conn);
            string result = AuthCom.ExecuteScalar().ToString();

            if (Convert.ToInt32(result) > 0)
            {
                string queryGetDataUser = $"SELECT " +
                   $"User_esti.idUsers, " +
                   $"User_esti.loginUsers, " +
                   $"User_esti.passUsers, " +
                   $"User_esti.fioUsers, 	" +
                   $"User_esti.enabledUsers, 	" +
                   $"User_Role.titleRole, 	" +
                   $"User_esti.roleUsers " +
                   $"FROM	User_Role	" +
                   $"INNER JOIN 	User_esti	ON 		User_Role.idRole = User_esti.roleUsers " +
                   $"WHERE User_esti.loginUsers='{login}' and User_esti.passUsers='{password}' and User_esti.enabledUsers=1";

                MySqlCommand commandGetDataUser = new MySqlCommand(queryGetDataUser, conn);
                MySqlDataReader reader = commandGetDataUser.ExecuteReader();
                while (reader.Read())
                {
                    authClass.auth_id = Convert.ToInt32(reader[0].ToString());
                    authClass.auth_fio = reader[3].ToString();
                    authClass.auth_role = Convert.ToInt32(reader[6].ToString());
                    authClass.auth_role_title = reader[5].ToString();

                }
                reader.Close();

                MessageBox.Show($"Доступ {login} разрешён ");
                authClass.InAuthenticated = true;
                this.Close();
            }
            else
            {
                MessageBox.Show($"Нет никого дома {login}");
                Application.Exit();
            }
            conn.Close();


        }

    }
}
