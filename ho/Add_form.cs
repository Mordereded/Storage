using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace ho
{
    public partial class Add_form : Form
    {
        private NpgsqlConnection connect;
        private NpgsqlCommand sqlCommand;
        private string login = "";
        private string password = "";
        private string connectionstring = "";
        private string sql = "";
        // private DataTable dt;
        private NpgsqlDataReader dataReader;
        private string table_name;
        private List<string> Attribute_name;
        private List<string> exist_attribute = new List<string>();
        //private List<string> Attribute_values;
        private int ID;
        public int AddedItemId { get; private set; }
        public Add_form()
        {
            InitializeComponent();
        }
        public Add_form(string login, string password)
        {
            InitializeComponent();
            this.login = login;
            this.password = password;
            connectionstring = "Server = localhost;" + "Port = 5432;" + "Database = Storage;" + "User Id = '" + login + "';" + "Password = '" + password + "';";
            this.Load += MyForm_Load;
        }
        bool Issomething = false;
        public Add_form(string login, string password, string table_name,int index = 0)
        {
            this.Load += MyForm_Load;
            InitializeComponent();
            this.ID = index;
            this.Resize += Form_Resize;
            //this.SetBounds(this.Left, this.Top, 400, 400);
            button1.Location = new Point((this.Width - button1.Width)/2, (this.Height - button1.Height) - 50);
            this.login = login;
            this.password = password;
            connectionstring = "Server = localhost;" + "Port = 5432;" + "Database = Storage;" + "User Id = '" + login + "';" + "Password = '" + password + "';";
            this.table_name = table_name;
            if (connect != null && connect.State != ConnectionState.Closed)
            {
                connect.Close();
            }
            if (table_name == "Customer")
            {
                this.Size = new Size(100, 200);
                label5.Visible = false;
                comboBox1.Visible = false;
                comboBox2.Visible = false;
                TextBox textBox4 = new TextBox();
                textBox4.Name = "textBox4";
                textBox3.Visible = true; // Убираем не нужные поля и надписи
                label1.Text = "Имя";
                label1.Location = new Point(65, 9);
                label2.Text = "Фамилия";
                label2.Location = new Point(65, 54);
                label3.Text = "Отчество (Не обязательно для ввода)";
                label4.Location = new Point(60, 135);
                label4.Text = "Номер телефона";
                textBox4.Location = new Point(29, 160);
                textBox4.Text = "+";
                textBox4.Size = (Size)new Point(200, 300);
                this.Controls.Add(textBox4);


            }
            else if (table_name == "Item")
            {
                TextBox textBoxDescription = new TextBox();
                textBoxDescription.Name = "textBoxDescription";
                textBoxDescription.Multiline = true; // Разрешить многострочный ввод
                textBoxDescription.ScrollBars = ScrollBars.Vertical; // Включить вертикальную полосу прокрутки
                textBoxDescription.Width = 200; // Установить ширину текстового поля
                textBoxDescription.Height = 100; // Установить высоту текстового поля
                textBoxDescription.Location = new Point(29, 190); // Установить положение текстового поля на форме
                this.Controls.Add(textBoxDescription);
                this.Size = new Size(100, 200);
                
                label5.Visible = false;
                textBox3.Visible = false; // Убираем не нужные поля и надписи
                label1.Text = "Название предмета";
                label1.Location = new Point(50, 9);
                label2.Text = "Цена товара";
                label2.Location = new Point(65, 54);
                label3.Text = "Категория";
                label4.Text = "id_Поставщика";
                label4.Location = new Point(60, 135);
                comboBox1.Location = new Point(29, 110);
                comboBox2.Location = new Point(29, 150);
                comboBox1.Items.Clear(); // Комбобокс для категорий
                comboBox2.Items.Clear(); // Комбобокс для поставщиков
                sql = "SELECT category_name FROM \"Category\"";
                connect = new NpgsqlConnection(connectionstring);
                connect.Open();
                using (sqlCommand = new NpgsqlCommand(sql, connect))
                {
                    using (dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            string categoryName = dataReader.GetString(0);
                            comboBox1.Items.Add(categoryName);
                        }
                    }
                }
                sql = "SELECT id_provider FROM \"Provider\"";
                using (sqlCommand = new NpgsqlCommand(sql, connect))
                {
                    using (dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            int id_provider = dataReader.GetInt32(0);
                            comboBox2.Items.Add(id_provider);
                        }
                    }
                }
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
                connect.Close();
                comboBox1.SelectedIndexChanged += comboBox_ForItem_Category_Change;
            }
            ///Доделать добавление не сущетсвующих атрибутов 
            else if (table_name == "Item.Category.Attribute.Attribute_value")
            {
                this.Size = new Size(300, 200);
                label5.Visible = false;
                label4.Visible = false;
                label3.Visible = false;
                label2.Visible = false;
                label1.Visible = false;
                textBox1.Visible = false;
                textBox2.Visible = false;
                textBox3.Visible = false; // Убираем не нужные поля и надписи
                comboBox2.Visible = false;
                comboBox1.Visible = false;
                Attribute_name = new List<string>();
                sql = "SELECT \"attribute_name\" FROM \"Attribute\" WHERE \"id_category\" = (SELECT \"id_category\" FROM \"Category\" WHERE \"id_category\" = (SELECT \"id_category\" FROM \"Item\" WHERE \"id_item\" = @id_item))";
                try
                {
                    using (NpgsqlConnection conn = new NpgsqlConnection(connectionstring))
                    {
                        conn.Open();
                        using (NpgsqlCommand Command = new NpgsqlCommand(sql, conn))
                        {
                            Command.Parameters.AddWithValue("@id_item", ID);
                            using (NpgsqlDataReader read = Command.ExecuteReader())
                            {
                                while (read.Read())
                                {
                                    if (!read.IsDBNull(0))
                                    {
                                        Attribute_name.Add(read.GetString(0));
                                    }
                                }
                            }
                        }
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                sql = $"SELECT a.\"attribute_name\" " +
                          $"FROM \"Item\" i " +
                          $"JOIN \"Category\" c ON i.\"id_category\" = c.\"id_category\" " +
                          $"LEFT JOIN \"Attribute_value\" av ON i.\"id_item\" = av.\"id_item\" " +
                          $"LEFT JOIN \"Attribute\" a ON av.\"id_attribute\" = a.\"id_attribute\" " +
                          $"WHERE i.\"id_item\" = @id_item ";
                try
                {
                    using (NpgsqlConnection conn = new NpgsqlConnection(connectionstring))
                    {
                        conn.Open();
                        using (NpgsqlCommand Command = new NpgsqlCommand(sql, conn))
                        {
                            Command.Parameters.AddWithValue("@id_item", ID);
                            using (NpgsqlDataReader read = Command.ExecuteReader())
                            {
                                if (read != null)
                                {
                                    while (read.Read())
                                    {
                                        if (!read.IsDBNull(0))
                                        {
                                            exist_attribute.Add(read.GetString(0));
                                        }
                                    }
                                }
                            }
                        }
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if(exist_attribute != null)
                    Attribute_name.RemoveAll(attribute => exist_attribute.Contains(attribute));
                int labelTop = 9;
                int textBoxTop = 25;
                int counter = 0;
                int right_shift = 30;
                
                foreach (var attribute in Attribute_name)
                {
                    Issomething = true;
                    // Создание и настройка Label
                    Label label = new Label();
                    label.Text = attribute;
                    label.AutoSize = true;
                    label.Name = attribute + "Lable";
                    label.Top = labelTop;
                    label.Location = new Point(right_shift, label.Top);
                    // Создание и настройка TextBox
                    TextBox textBox = new TextBox();
                    textBox.Top = textBoxTop;
                    textBox.Name = attribute + "Text";
                    //textBox.Text = attribute;
                    textBox.Location = new Point(right_shift, textBox.Top);

                    // Добавление Label и TextBox на форму
                    this.Controls.Add(label);
                    this.Controls.Add(textBox);
                    dynamicControls.Add(label);
                    dynamicControls.Add(textBox);
                    // Обновление вертикальной позиции для следующего элемента
                    labelTop += 45;
                    textBoxTop += 45;
                    counter++;
                    if (counter % 4 == 0) // если 10 записей то мы смещаем в следующий ряд
                    {
                        right_shift += 140;
                        labelTop = 9;
                        textBoxTop = 25;
                        this.Width += 250;
                    }
                    
                }
                if (!Issomething)
                {
                    MessageBox.Show("Отсутствуют атрибуты у категории", "Предупреждение");
                }

            }
            else if (table_name == "Provider")
            {

                try
                {
                    sql = SQL_SELECT("Company");
                    connect = new NpgsqlConnection(connectionstring);
                    connect.Open();
                    sqlCommand = new NpgsqlCommand(sql, connect);
                    dataReader = sqlCommand.ExecuteReader();
                    comboBox1.Items.Clear();
                    while (dataReader.Read())
                    {
                        string value = dataReader.GetString(1); // Индекс столбца, который содержит значения
                        comboBox1.Items.Add(value);
                    }
                    sql = SQL_SELECT("Country");
                    connect = new NpgsqlConnection(connectionstring);
                    connect.Open();
                    sqlCommand = new NpgsqlCommand(sql, connect);
                    dataReader = sqlCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        string value = dataReader.GetString(1); // Индекс столбца, который содержит значения
                        comboBox2.Items.Add(value);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Закрытие соединения и освобождение ресурсов
                    sqlCommand.Dispose();
                    connect?.Close();
                    connect?.Dispose();
                }
            }
            else if (table_name == "Category.Attribute")
            {
                this.Size = new Size(300, 200);
                label5.Visible = false;
                label4.Visible = false;
                label3.Visible = false;
                textBox1.Visible = false;
                textBox3.Visible = false; // Убираем не нужные поля и надписи
                comboBox2.Visible = false;
                label1.Text = "Категория";
                label1.Location = new Point(80, 9);
                label2.Text = "Название атрибута";
                label2.Location = new Point(50, 54);
                comboBox1.Location = textBox1.Location;
                textBox1.Location = new Point(29, 110);
                comboBox1.Items.Clear(); // Комбобокс для клиента

                sql = "SELECT \"category_name\" FROM \"Category\"";
                connect = new NpgsqlConnection(connectionstring);
                connect.Open();
                using (sqlCommand = new NpgsqlCommand(sql, connect))
                {
                    using (dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            string category_name = dataReader.GetString(0);
                            comboBox1.Items.Add(category_name);
                        }
                    }
                }
                comboBox1.SelectedIndex = 0;
                connect.Close();
            }
            else if (table_name == "Contract")
            {
                TextBox textBoxDescription = new TextBox();
                textBoxDescription.Name = "textBoxDescription";
                textBoxDescription.Multiline = true; // Разрешить многострочный ввод
                textBoxDescription.ScrollBars = ScrollBars.Vertical; // Включить вертикальную полосу прокрутки
                textBoxDescription.Width = 200; // Установить ширину текстового поля
                textBoxDescription.Height = 100; // Установить высоту текстового поля
                textBoxDescription.Location = new Point(29, 170); // Установить положение текстового поля на форме
                Button Show_table_button = new Button();
                Show_table_button.Name = "Show_table_button";
                Show_table_button.Text = "Товары";
                Show_table_button.Size = new Size(100, 30); // Установка размера кнопки (ширина, высота)
                Show_table_button.Location = new Point(170, 100); // Установка расположения кнопки (координата X, координата Y)
                // Привязка функции к событию Click кнопки
                Show_table_button.Click += Show_Table_Click;
                this.Controls.Add(Show_table_button);
                this.Controls.Add(textBoxDescription);
                this.Size = new Size(100, 200);
                label5.Visible = false;
                label4.Visible = false;
                textBox3.Visible = false; // Убираем не нужные поля и надписи
                comboBox2.Visible = false;
                label1.Text = "id_клиента";
                label1.Location = new Point(50, 9);
                label2.Text = "Стоимость контракта";
                label2.Location = new Point(50, 54);
                label3.Text = "id_товара";
                comboBox1.Location = textBox1.Location;
                textBox1.Location = new Point(29, 110);
                comboBox1.Items.Clear(); // Комбобокс для клиента
                
                sql = "SELECT id_customer FROM \"Customer\"";
                connect = new NpgsqlConnection(connectionstring);
                connect.Open();
                using (sqlCommand = new NpgsqlCommand(sql, connect))
                {
                    using (dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            int id_customer = dataReader.GetInt32(0);
                            comboBox1.Items.Add(id_customer);
                        }
                    }
                }
                comboBox1.SelectedIndex = 0;
                connect.Close();
            }
        }

     // Подписываемся на событие Load

        private void MyForm_Load(object sender, EventArgs e)
        {
            if(!Issomething && table_name == "Item.Category.Attribute.Attribute_value")
            {
                this.Close();
            }
        }
        List<Control> dynamicControls = new List<Control>();
        private void comboBox_ForItem_Category_Change(object sender, EventArgs e)
        {
            ClearDynamicControls(); // Удаляем старые контролеры
            this.Size = new Size(500, 500);
            Attribute_name = new List<string>();
            sql = $"SELECT \"attribute_name\" FROM \"Attribute\" WHERE \"id_category\" = (SELECT \"id_category\" FROM \"Category\" WHERE \"category_name\" = \'{comboBox1.SelectedItem.ToString()}\');";

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionstring))
                {
                    conn.Open();
                    using (NpgsqlCommand Command = new NpgsqlCommand(sql, conn))
                    {
                        using (NpgsqlDataReader read = Command.ExecuteReader())
                        {
                            while (read.Read())
                            {
                                Attribute_name.Add(read.GetString(0));
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            int labelTop = 9;
            int textBoxTop = 25;
            int counter = 0;
            int right_shift = 180;
            foreach (var attribute in Attribute_name)
            {
                // Создание и настройка Label
                Label label = new Label();
                label.Text = attribute;
                label.AutoSize = true;
                label.Name = attribute + "Lable";
                label.Top = labelTop;
                label.Location = new Point(right_shift, label.Top);
                // Создание и настройка TextBox
                TextBox textBox = new TextBox();
                textBox.Top = textBoxTop;
                textBox.Name = attribute + "Text";
                textBox.Location = new Point(right_shift, textBox.Top);

                // Добавление Label и TextBox на форму
                this.Controls.Add(label);
                this.Controls.Add(textBox);
                dynamicControls.Add(label);
                dynamicControls.Add(textBox);
                // Обновление вертикальной позиции для следующего элемента
                labelTop += 45;
                textBoxTop += 45;
                counter++;
                if(counter % 4 == 0) // если 10 записей то мы смещаем в следующий ряд
                {
                    right_shift += 140;
                    labelTop = 9;
                    textBoxTop = 25;
                    this.Width += 250;
                }
            }
        }
        private void ClearDynamicControls()
        {
            foreach (var control in dynamicControls)
            {
                this.Controls.Remove(control);
                control.Dispose();
            }

            dynamicControls.Clear();
        }
        private DataTable GetDataFromDatabase(int offset,string tablename = "Item")
        {
            sql = $"SELECT * FROM \"{tablename}\" ORDER BY id_{tablename.ToLower()} OFFSET {offset} ROWS LIMIT 30";

            using (connect = new NpgsqlConnection(connectionstring))
            {
                using (sqlCommand = new NpgsqlCommand(sql, connect))
                {

                    connect.Open();
                    dataReader = sqlCommand.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(dataReader);
                    return dataTable;
                }
            }
        }
        int currentOffset = 0;
        private void Show_Table_Click(object sender, EventArgs e)
        {
            string table_to_show = "Item";
            using (Form dialogForm = new Form())
            {
                DataGridView dataGridView = new DataGridView();
                dataGridView.Dock = DockStyle.Top;
                dataGridView.ReadOnly = true;
                // Загрузка данных из базы данных с указанным сдвигом
                DataTable dataTable = GetDataFromDatabase(currentOffset, table_to_show);
                dataGridView.DataSource = dataTable;
                dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
                dataGridView.Size = new Size(30, 300);
                dialogForm.Controls.Add(dataGridView);
                this.Controls.Remove(dataGridView);
                // Создание кнопок для управления сдвигом таблицы
                Button previousButton = new Button();
                previousButton.Text = "Прошлая";
                previousButton.Location = new Point(10, 350);
                previousButton.BringToFront();
                previousButton.Click += (send, el) =>
                {
                    if (currentOffset > 0)
                    {
                        currentOffset -= 40;
                        DataTable previousData = GetDataFromDatabase(currentOffset, table_to_show);
                        dataGridView.DataSource = previousData;
                    }
                };
                dialogForm.Controls.Add(previousButton);

                Button nextButton = new Button();
                nextButton.BringToFront();
                nextButton.Text = "Следуюущая";
                nextButton.Width = 80;
                nextButton.Location = new Point(90, 350);
                nextButton.Click += (send, el) =>
                {
                    currentOffset += 40;
                    DataTable nextData = GetDataFromDatabase(currentOffset, table_to_show);
                    dataGridView.DataSource = nextData;
                };
                dialogForm.Controls.Add(nextButton);
                dataGridView.BringToFront();
                dialogForm.ClientSize = new Size(700, 400);
                dialogForm.StartPosition = FormStartPosition.CenterParent;
                dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogForm.Text = "Таблица";

                dialogForm.ShowDialog();
            }
        }
        public string SQL_SELECT(string table)
        {

            string sql = "";
            switch (table)
            {
                case "Company":
                    sql = $"SELECT * FROM \"Company\";";
                    break;
                case "Country":
                    sql = $"SELECT * FROM \"Country\";";
                    break;
                case "Category":
                    sql = $"SELECT * FROM \"Category\";";
                    break;
                case "Provider":
                    sql = $"SELECT * FROM \"Provider\";";
                    break;
            }
            return sql;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (table_name == "Provider")
            {
                try
                {
                    connect = new NpgsqlConnection(connectionstring);
                    connect.Open();
                    
                    sql = $"SELECT id_country FROM \"Country\" WHERE country_name = \'{comboBox2.Text}\'";
                    sqlCommand = new NpgsqlCommand(sql, connect);
                    dataReader = sqlCommand.ExecuteReader();
                    int id_country = 0;
                    while (dataReader.Read())
                    {
                        id_country = dataReader.GetInt32(0);
                    }
                    connect.Close();
                    connect.Open();
                    sql = $"SELECT id_company FROM \"Company\" WHERE company_name = \'{comboBox1.Text}\'";
                    sqlCommand = new NpgsqlCommand(sql, connect);
                    dataReader = sqlCommand.ExecuteReader();
                    int id_company = 0;
                    while (dataReader.Read())
                    {
                        id_company = dataReader.GetInt32(0);
                    }
                    connect.Close();
                    connect.Open();
                    if (!string.IsNullOrEmpty(textBox3.Text))
                    {
                        sql = $"INSERT INTO \"{table_name}\"(first_name, second_name,last_name,id_country,id_company) VALUES( @first_name, @second_name, @last_name,@id_country, @id_company) RETURNING id_provider; ";
                    }
                    else
                    {
                        sql = $"INSERT INTO \"{table_name}\"(first_name, second_name,id_country,id_company) VALUES( @first_name, @second_name,@id_country, @id_company) RETURNING id_provider; ";
                    }
                    //INSERT INTO "Provider"(first_name, second_name,last_name,id_country,id_company) VALUES ('Андрей', 'Осипов','Викторович',158, 58);
                    /*DialogResult dialogResult = MessageBox.Show("Вы уверен, что хотите добавить значение в таблицу?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.No) return;*/
                   
                    sqlCommand = new NpgsqlCommand(sql, connect);
                    sqlCommand.Parameters.AddWithValue($"@first_name", textBox1.Text);
                    sqlCommand.Parameters.AddWithValue($"@second_name", textBox2.Text);
                    if (!string.IsNullOrEmpty(textBox3.Text)) sqlCommand.Parameters.AddWithValue($"@last_name", textBox3.Text);
                    sqlCommand.Parameters.AddWithValue($"@id_country", Convert.ToInt32(id_country));
                    sqlCommand.Parameters.AddWithValue($"@id_company", Convert.ToInt32(id_company));
                    AddedItemId = (int)sqlCommand.ExecuteScalar();
                    MessageBox.Show($"Вставленный элемент имеет номер - {AddedItemId}", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Закрытие соединения и освобождение ресурсов
                    sqlCommand?.Dispose();
                    connect?.Close();
                    connect?.Dispose();
                    this.Close();
                }
            }
            else if (table_name == "Contract")
            {
                try
                {

                    connect = new NpgsqlConnection(connectionstring);
                    connect.Open();
                    sql = $"INSERT INTO \"Contract\"(id_customer,contract_cost,discription,id_item) VALUES(@id_customer,@cost,@discript,@id_item) RETURNING id_contract;";
                    sqlCommand = new NpgsqlCommand(sql, connect);
                    sqlCommand.Parameters.AddWithValue($"@id_customer", Convert.ToInt32(comboBox1.SelectedItem.ToString()));
                    sqlCommand.Parameters.AddWithValue($"@cost", Convert.ToDecimal(textBox2.Text));
                    sqlCommand.Parameters.AddWithValue($"@id_item", Convert.ToInt32(textBox1.Text));
                    TextBox textBoxDescription = this.Controls["textBoxDescription"] as TextBox;
                    if (!string.IsNullOrEmpty(textBoxDescription.Text))
                    {
                        sqlCommand.Parameters.AddWithValue("@discript", textBoxDescription.Text);
                    }
                    else
                    {
                        sqlCommand.Parameters.AddWithValue("@discript", DBNull.Value);
                    }
                    AddedItemId = (int)sqlCommand.ExecuteScalar();
                    MessageBox.Show($"Вставленный элемент имеет номер - {AddedItemId}", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Закрытие соединения и освобождение ресурсов
                    sqlCommand?.Dispose();
                    connect?.Close();
                    connect?.Dispose();
                    this.Close();
                }
            }
            else if (table_name == "Customer")
            {
                TextBox txtPhoneNumber = this.Controls.Find("textBox4", true).FirstOrDefault() as TextBox;
                if(!string.IsNullOrEmpty(textBox3.Text))
                {
                    sql = $"INSERT INTO \"Customer\"(first_name,second_name,last_name,phone_number) VALUES(@first_name,@second_name,@last_name,@phone_number) RETURNING id_customer;";
                }
                else
                {
                    sql = $"INSERT INTO \"Customer\"(first_name,second_name,phone_number) VALUES(@first_name,@second_name,@phone_number) RETURNING id_customer;";
                }
                try
                {
                    connect = new NpgsqlConnection(connectionstring);
                    connect.Open();
                    //sql = $"INSERT INTO \"Customer\"(first_name,second_name,{last_name}phone_number) VALUES(@first_name,@second_name,@{last_name}phone_number) RETURNING id_customer;";
                    sqlCommand = new NpgsqlCommand(sql, connect);
                    sqlCommand.Parameters.AddWithValue($"@first_name", textBox1.Text);
                    sqlCommand.Parameters.AddWithValue($"@second_name",textBox2.Text);
                    if (!string.IsNullOrEmpty(textBox3.Text)) { sqlCommand.Parameters.AddWithValue($"@last_name", textBox3.Text); }
                    sqlCommand.Parameters.AddWithValue($"@phone_number", txtPhoneNumber.Text);
                    AddedItemId = (int)sqlCommand.ExecuteScalar();
                    MessageBox.Show($"Вставленный элемент имеет номер - {AddedItemId}", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Закрытие соединения и освобождение ресурсов
                    sqlCommand?.Dispose();
                    connect?.Close();
                    connect?.Dispose();
                    this.Close();
                }
            }
            else if (table_name == "Category.Attribute")
            {
                try
                {
                    connect = new NpgsqlConnection(connectionstring);
                    connect.Open();
                    sql = "INSERT INTO \"Attribute\" (id_category, attribute_name) " +
                           "VALUES ((SELECT \"id_category\" FROM \"Category\" WHERE \"Category\".\"category_name\" = @id_category), @category_name) " +
                           "RETURNING id_category;";
                    sqlCommand = new NpgsqlCommand(sql, connect);
                    sqlCommand.Parameters.AddWithValue($"@id_category", comboBox1.SelectedItem.ToString());
                    sqlCommand.Parameters.AddWithValue($"@category_name", textBox2.Text);
                    AddedItemId = (int)sqlCommand.ExecuteScalar();
                    MessageBox.Show($"Вставленный элемент имеет номер - {AddedItemId}", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Закрытие соединения и освобождение ресурсов
                    sqlCommand?.Dispose();
                    connect?.Close();
                    connect?.Dispose();
                    this.Close();
                }
            }
            // Ввод предмета нужно доделать
            else if (table_name == "Item")
            {
                connect = new NpgsqlConnection(connectionstring);
                connect.Open();


                try
                {
                    string values = string.Empty;

                    for (int i = 0; i < dynamicControls.Count; i += 2)
                    {
                        if (dynamicControls[i] is Label label && dynamicControls[i + 1] is TextBox textBox)
                        {
                            string attributeName = label.Text;
                            string attributeValue = textBox.Text;

                            // Добавляем название атрибута и его значение в строку values
                            if (!string.IsNullOrEmpty(attributeValue))
                            {
                                // Добавляем название атрибута и его значение в строку values
                                values += $@"('{attributeName}', '{attributeValue}'),";
                            }
                        }
                    }


                    // Удаляем последнюю запятую из строки values
                    values = values.TrimEnd(',');
                    //MessageBox.Show($"{values}", "");
                    sql = $"INSERT INTO \"Item\"(item_name,item_cost,id_category,id_provider,item_discription) VALUES(@name,@cost,(SELECT id_category FROM \"Category\" WHERE category_name = \'{comboBox1.SelectedItem.ToString()}\'),@provider,@discript) RETURNING id_item;";
                    sqlCommand = new NpgsqlCommand(sql, connect);
                    sqlCommand.Parameters.AddWithValue($"@name", textBox1.Text);
                    sqlCommand.Parameters.AddWithValue($"@cost", Convert.ToDecimal(textBox2.Text));
                    sqlCommand.Parameters.AddWithValue($"@provider", Convert.ToInt32(comboBox2.SelectedItem));
                    TextBox textBoxDescription = this.Controls["textBoxDescription"] as TextBox;
                    if (!string.IsNullOrEmpty(textBoxDescription.Text))
                    {
                        sqlCommand.Parameters.AddWithValue("@discript", textBoxDescription.Text);
                    }
                    else
                    {
                        sqlCommand.Parameters.AddWithValue("@discript", DBNull.Value);
                    }
                    AddedItemId = (int)sqlCommand.ExecuteScalar();
                    connect.Close();
                    if (!string.IsNullOrEmpty(values))
                    {
                        connect = new NpgsqlConnection(connectionstring);
                        connect.Open();
                        sql = $@"INSERT INTO ""Attribute_value"" (""id_item"", ""id_attribute"", ""attribute_value"")
                       SELECT {AddedItemId}, a.""id_attribute"", v.""attribute_value""
                       FROM ""Attribute"" a
                       JOIN ""Category"" c ON a.""id_category"" = c.""id_category""
                       JOIN (VALUES {values}
                       ) AS v (""attribute_name"", ""attribute_value"")
                       ON a.""attribute_name"" = v.""attribute_name""
                       WHERE c.""category_name"" = (SELECT category_name FROM ""Category"" WHERE id_category = (SELECT id_category FROM ""Item"" WHERE id_item = {AddedItemId}))
                       AND a.""id_category"" = (SELECT ""id_category"" FROM ""Item"" WHERE ""id_item"" = {AddedItemId});";
                        sqlCommand = new NpgsqlCommand(sql, connect);
                        sqlCommand.ExecuteScalar();
                    }
                    MessageBox.Show($"Вставленный элемент имеет номер - {AddedItemId}", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                }
                finally
                {
                    sqlCommand?.Dispose();
                    connect?.Close();
                    connect?.Dispose();
                    this.Close();
                }
            }
            else if (table_name == "Item.Category.Attribute.Attribute_value")
            {
                string values = string.Empty;

                for (int i = 0; i < dynamicControls.Count; i += 2)
                {
                    if (dynamicControls[i] is Label label && dynamicControls[i + 1] is TextBox textBox)
                    {
                        string attributeName = label.Text;
                        string attributeValue = textBox.Text;

                        
                        if (!string.IsNullOrEmpty(attributeValue))
                        {
                            values += $@"('{attributeName}', '{attributeValue}'),";
                        }
                    }
                }
                values = values.TrimEnd(',');
                try
                {
                    connect = new NpgsqlConnection(connectionstring);
                    connect.Open();
                    sql = $@"INSERT INTO ""Attribute_value"" (""id_item"", ""id_attribute"", ""attribute_value"")
                       SELECT {ID}, a.""id_attribute"", v.""attribute_value""
                       FROM ""Attribute"" a
                       JOIN ""Category"" c ON a.""id_category"" = c.""id_category""
                       JOIN (VALUES {values}
                       ) AS v (""attribute_name"", ""attribute_value"")
                       ON a.""attribute_name"" = v.""attribute_name""
                       WHERE c.""category_name"" = (SELECT category_name FROM ""Category"" WHERE id_category = (SELECT id_category FROM ""Item"" WHERE id_item = {ID}))
                       AND a.""id_category"" = (SELECT ""id_category"" FROM ""Item"" WHERE ""id_item"" = {ID});";
                    sqlCommand = new NpgsqlCommand(sql, connect);
                    sqlCommand.ExecuteScalar();
                    MessageBox.Show($"Вставленно в элемент - {ID}", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Закрытие соединения и освобождение ресурсов
                    sqlCommand?.Dispose();
                    connect?.Close();
                    connect?.Dispose();
                    this.Close();
                }
            }
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            button1.Location = new Point((this.Width - button1.Width) / 2, (this.Height - button1.Height) - 50);
            
        }



        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
