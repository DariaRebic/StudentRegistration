using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace StudentRegistration

{
    //Lines 13-22: Declares a partial class named Form1 that extends the Form class. 
    //This class represents the main form of the application. 
    //The constructor (public Form1()) initializes the form and calls the Load() method.
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Load();
        }

        //Declare class-level variables, including a SqlConnection (con) for connecting to the database
        SqlConnection con = new SqlConnection("Data Source=daria\\MSSQLSERVER02; Initial Catalog=ASPIRA; User Id=sa; Password=keko");
        //SqlCommand (cmd) for executing SQL commands
        SqlCommand cmd;
        //SqlDataReader (read) for reading data from the database
        SqlDataReader read;
        SqlDataAdapter adapter;
        string id;
        //Mode is a boolean variable used to determine whether the application is in "Add" mode or "Edit" mode.
        bool Mode = true;
        //sql is a string variable used to store SQL queries
        string sql;

        public void Load()
        //Load method retrieves data from the "Student" table and populates a DataGridView (dataGridView1) with the results.
        {
            try
            {
                sql = "select * from Student";
                cmd = new SqlCommand(sql, con);
                con.Open();
                read = cmd.ExecuteReader();
                dataGridView1.Rows.Clear();

                while (read.Read())
                {
                    dataGridView1.Rows.Add(read[0], read[1], read[2], read[3]);
                }
                con.Close();
            }
            catch (Exception ex)
            //Any exceptions that occur during this process are caught and displayed in a message box
                {
                     MessageBox.Show(ex.Message);
                }
            }

        public void getID(string id)
        //getID method retrieves data for a specific student ID and populates text boxes (txtName, txtCourse, txtFee) with the retrieved information.
        {
            sql = "select * from Student where ID = '" + id + "' ";
            cmd = new SqlCommand(sql, con);
            con.Open();
            read = cmd.ExecuteReader();

            while (read.Read())
            {
                txtName.Text = read[1].ToString();
                txtCourse.Text = read[2].ToString();
                txtFee.Text = read[3].ToString();
            }
            con.Close();
        }




        private void button1_Click(object sender, EventArgs e)
        //Defines the event handler for the button click (button1_Click). 
        //This method is responsible for adding a new student record or updating an existing one based on the mode (Mode). 
        //It utilizes parameterized SQL queries to prevent SQL injection
        {
            string name = txtName.Text;
            string course = txtCourse.Text;
            string fee = txtFee.Text;

            if (Mode == true)
            {
                sql = "insert into Student(studentName,course,fee) values(@studentName,@course,@fee)";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@studentName", name);
                cmd.Parameters.AddWithValue("@course", course);
                cmd.Parameters.AddWithValue("@fee", fee);
                MessageBox.Show("Record added!");
                cmd.ExecuteNonQuery();

                txtName.Clear();
                txtCourse.Clear();
                txtFee.Clear();
                txtName.Focus();
            }
            else
            {
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql = "update Student set studentName = @studentName, course = @course, fee = @fee where id = @ID ";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@studentName", name);
                cmd.Parameters.AddWithValue("@course", course);
                cmd.Parameters.AddWithValue("@fee", fee);
                cmd.Parameters.AddWithValue("@ID", id);
                MessageBox.Show("Record updated!");
                cmd.ExecuteNonQuery();

                txtName.Clear();
                txtCourse.Clear();
                txtFee.Clear();
                txtName.Focus();
                button1.Text = "Save";
                Mode = true;
            }
            con.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //This method handles the "Edit" and "Delete" operations based on the clicked cell in the data grid
        {
            if (e.ColumnIndex == dataGridView1.Columns["Edit"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                getID(id);
                button1.Text = "Edit";
            }
            else if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql = "delete from Student where ID = @ID";
                con.Open();
                cmd = new SqlCommand(sql,con);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record deleted!");
                con.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        //: Defines event handler for the "Refresh" (button3_Click) button
        //The "Refresh" button reloads the data
        {
            Load();
        }

        private void button2_Click(object sender, EventArgs e)
        //Defines event handler for the "Clear" (button2_Click) button
        // The "Clear" button clears the text boxes and sets the mode to "Save"
        {
            txtName.Clear();
            txtCourse.Clear();
            txtFee.Clear();
            txtName.Focus();
            button1.Text = "Save";
            Mode = true;
        }
    }
}
