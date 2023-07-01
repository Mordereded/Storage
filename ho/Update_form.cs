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
    public partial class Update_form : Form
    {
        private NpgsqlConnection connect;
        private NpgsqlCommand sqlCommand;
        private string login = "";
        private string password = "";
        private string connectionstring = "";
        private string sql = "";
        private DataTable dt;
        private NpgsqlDataReader dataReader;
        private string table_name;
        private int id;
        
        private Dictionary<string, string> originalFieldValues =  new Dictionary<string, string>();
        public Update_form()
        {
            InitializeComponent();
        }
        public Update_form(string login, string password,string table_name, int id )
        {
            InitializeComponent();
            this.login = login;
            this.password = password;
            this.table_name = table_name;
            this.id = id;
            connectionstring = $"Server = localhost;" + "Port = 5432;" + "Database = Storage;" + "User Id = '" + login + "';" + "Password = '" + password + "';";
            this.Load += MyForm_Load;
        }
        Label name__ = new Label();
        List<string> prevatribute = new List<string>();
        List<string> attributeName = new List<string>();
        List<string> existitemattribute = new List<string>();
        bool isSomething = true;
        private void CreateControls(string table)
        {  
            if (table_name == "Category.Attribute")
            {
                connect = new NpgsqlConnection(connectionstring);
                connect.Open();
                sql = $"SELECT category_name FROM \"Category\" WHERE \"id_category\" = '{id}'";
                sqlCommand = new NpgsqlCommand(sql, connect);
                dataReader = sqlCommand.ExecuteReader();
                while(dataReader.Read())
                {
                    name__.Text = "Название категории: " + dataReader.GetString(0);
                }
                
                connect.Close();
                connect.Open();
                sql = "SELECT \"Attribute\".\"attribute_name\"";
                sql += " FROM \"Category\"";
                sql += $" JOIN \"Attribute\" ON \"Category\".\"id_category\" = \"Attribute\".\"id_category\" WHERE \"Category\".\"id_category\" = {id}";
                sqlCommand = new NpgsqlCommand(sql, connect);
                dataReader = sqlCommand.ExecuteReader();
                while(dataReader.Read())
                {
                    prevatribute.Add(dataReader.GetString(0));

                }
                name__.Width = 300;
                name__.Top = 10;
                Controls.Add(name__);
                int top = 40;
                int labelWidth = 160;
                int textboxWidth = 150;
                int numberofattrib = 1;
                foreach (var element in prevatribute)
                {
                    Label label = new Label();
                    label.Name = element;
                    label.Text = "Название " + numberofattrib + " - Атрибута";
                    label.Width = labelWidth;
                    label.Top = top;
                    TextBox textbox = new TextBox();
                    textbox.Width = textboxWidth;
                    textbox.Top = top;
                    textbox.Left = labelWidth + 40;
                    textbox.Name = element + "TextBox"; // Установка имени элемента управления
                    textbox.Text = element;
                    Controls.Add(textbox);
                    // Добавление надписи и текстового поля в коллекцию элементов управления формы
                    Controls.Add(label);
                    numberofattrib++;
                    top += 30;
                }
                button1.Top = top;
            }
            else if (table_name == "Item.Category.Attribute.Attribute_value")
            {
                connect = new NpgsqlConnection(connectionstring);
                connect.Open();
                sql = $"SELECT \"category_name\"FROM \"Category\" WHERE \"id_category\" = (SELECT \"id_category\" FROM \"Item\" WHERE id_item = @id_item)";
                sqlCommand = new NpgsqlCommand(sql, connect);
                sqlCommand.Parameters.AddWithValue("@id_item", id);
                dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    name__.Text = "Название категории: " + dataReader.GetString(0) + "; Id_Товара: " + id;
                }
                name__.Width = 400;
                name__.Top = 10;
                Controls.Add(name__);
                connect.Close();
                connect.Open();
                sql = "SELECT \"Attribute\".\"attribute_name\"";
                sql += " FROM \"Category\"";
                sql += $" JOIN \"Attribute\" ON \"Category\".\"id_category\" = \"Attribute\".\"id_category\" WHERE \"Category\".\"id_category\" = (SELECT \"id_category\" FROM \"Item\" WHERE id_item = @id_item)";
                sqlCommand = new NpgsqlCommand(sql, connect);
                sqlCommand.Parameters.AddWithValue("@id_item", id);
                dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    attributeName.Add(dataReader.GetString(0));
                }
                connect.Close();
                connect.Open();
                sql = $@"
                        SELECT a.""attribute_name"", av.""attribute_value""
                        FROM ""Item"" i
                        JOIN ""Category"" c ON i.""id_category"" = c.""id_category""
                        LEFT JOIN ""Attribute_value"" av ON i.""id_item"" = av.""id_item""
                        LEFT JOIN ""Attribute"" a ON av.""id_attribute"" = a.""id_attribute""
                        WHERE i.""id_item"" = @id_item;";
                sqlCommand = new NpgsqlCommand(sql, connect);
                sqlCommand.Parameters.AddWithValue("@id_item", id);
                dataReader = sqlCommand.ExecuteReader();
                if (dataReader.HasRows == false) return;
                while (dataReader.Read())
                {
                    if (!dataReader.IsDBNull(0))
                    {
                        existitemattribute.Add(dataReader.GetString(0));
                    }

                    if (!dataReader.IsDBNull(1))
                    {
                        prevatribute.Add(dataReader.GetString(1));
                    }
                }
                if (existitemattribute.Count == 0) { isSomething = false; }
                if (!isSomething)
                {
                    MessageBox.Show("Отсутствуют атрибуты у предмета", "Предупреждение");
                    return;
                }
                int top = 40;
                int labelWidth = 160;
                int textboxWidth = 150;
                int numberofattrib = 0;
                foreach (var element in existitemattribute)
                {
                    Label label = new Label();
                    label.Name = element ;
                    label.Text = element;
                    label.Width = labelWidth;
                    label.Top = top;
                    TextBox textbox = new TextBox();
                    textbox.Width = textboxWidth;
                    textbox.Top = top;
                    textbox.Left = labelWidth + 40;
                    textbox.Name = element ; // Установка имени элемента управления
                    Controls.Add(textbox);
                    top += 30;
                    // Добавление надписи и текстового поля в коллекцию элементов управления формы
                    Controls.Add(label);
                    top += 30;
                }
                if(prevatribute.Count != 0)
                {
                    foreach (Control control in Controls)
                    {
                        if (control is TextBox textBox && existitemattribute.Contains(textBox.Name))
                        {
                            int index = existitemattribute.IndexOf(textBox.Name);
                            if (index < prevatribute.Count)
                            {
                                textBox.Text = prevatribute[index];
                            }
                        }
                    }
                }
                button1.Top = top;
                this.Size = new Size(400, 400);
                

            }
        }
        private void Update_form_Load(object sender, EventArgs e)
        {
            if(table_name == "Category.Attribute" || table_name == "Item.Category.Attribute.Attribute_value")
            {
                CreateControls(table_name);
                return;
            }
            // Получение информации о полях таблицы
            DataTable tableSchema = GetTableSchema(table_name);

            // Создание элементов управления на основе информации о полях
            CreateControls(tableSchema);

            // Помещение созданных элементов управления на форму
            LoadRecordData();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }
        private DataTable GetTableSchema(string tableName)
        {
            using (var connection = new NpgsqlConnection(connectionstring))
            {
                connection.Open();
                //sqlCommand = new NpgsqlCommand($"SELECT * FROM get_table_columns(\'{table_name}\')", connection);
                DataTable schema = connection.GetSchema("Columns", new string[] { null, null, tableName });
                return schema;
            }
        }

        /*private List<string> GetTableDictionary(string tablename)
        {
            List<string> dict = new List<string>();
            if (tablename == "Company" || tablename == "Country")
            {
                connect = new NpgsqlConnection(connectionstring);
                connect.Open();
                string sql = sql = $"SELECT {tablename.ToLower()}_name FROM \"{tablename}\";";
                sqlCommand = new NpgsqlCommand(sql, connect);
                dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    string value = dataReader.GetString(0); // Предполагается, что результатом запроса является строковое значение в столбце с индексом 0
                    dict.Add(value);
                }
            }

            return dict;
        }*/
        private void MyForm_Load(object sender, EventArgs e)
        {
            if (!isSomething && table_name == "Item.Category.Attribute.Attribute_value")
            {
                this.Close();
            }
        }
        private void CreateControls(DataTable tableSchema)
        {
            int top = 20;
            int labelWidth = 160;
            int textboxWidth = 150;
            //int comboboxWidth = 150;
            int numberofattrib = 1;
            foreach (DataRow row in tableSchema.Rows)
            {
                string columnName = row["COLUMN_NAME"].ToString();
                string dataType = row["DATA_TYPE"].ToString();
                if (columnName == $"id_{table_name.ToLower()}")
                    continue;
                // Создание надписи с названием поля
                Label label = new Label();
                label.Text = numberofattrib +" Атрибут(" + columnName +")";
                label.Width = labelWidth;
                label.Top = top;
                TextBox textbox = new TextBox();
                textbox.Width = textboxWidth;
                textbox.Top = top;
                textbox.Left = labelWidth + 40;
                textbox.Name = columnName + "TextBox"; // Установка имени элемента управления
                Controls.Add(textbox);
                // Добавление надписи и текстового поля в коллекцию элементов управления формы
                Controls.Add(label);
                

                // Добавление текстового поля в словарь с оригинальными значениями
                originalFieldValues.Add(columnName, "");
                numberofattrib++;
                // Загрузка оригинального значения поля из базы данных
                /*connect = new NpgsqlConnection(connectionstring);
                connect.Open();
                string query = "SELECT " + $"\"{columnName}\"" + " FROM "+$"\"{table_name}\"" + $" WHERE \"id_{table_name.ToLower()}\" = @Id";
                using (var command = new NpgsqlCommand(query, connect))
                {
                    // Параметр Id для фильтрации по идентификатору записи
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            object originalValue = reader[columnName];
                            originalFieldValues[columnName] = originalValue != null ? originalValue.ToString() : "";
                        }
                    }
                }*/

                // Увеличение вертикальной позиции для следующего элемента
                top += 30;
                //connect.Close();
            }
        }
        private async void LoadRecordData(string table)
        {
            using (var connection = new NpgsqlConnection(connectionstring))
            {
                await connection.OpenAsync();

                // Получение данных записи из базы данных
                string query = $"SELECT * FROM \"{table}\" WHERE id_{table.ToLower()} = @Id";
                try
                {
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        // Параметр Id для фильтрации по идентификатору записи
                        command.Parameters.AddWithValue("@Id", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                // Загрузка значений полей записи в текстовые поля формы
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetName(i);
                                    string columnValue = reader.IsDBNull(i) ? string.Empty : reader.GetValue(i).ToString();

                                    string controlKey = columnName.Replace(" ", "_"); // Преобразование имени столбца в допустимый ключ
                                    Control control = Controls[columnName + "TextBox"];
                                    if (control is TextBox)
                                    {
                                        TextBox textbox = (TextBox)control;
                                        textbox.Text = columnValue;
                                        originalFieldValues[columnName] = columnValue; // Сохранение оригинального значения поля
                                    }
                                    //Работа с справочникаим если будет время добавить
                                    /*                                    else
                                                                        {
                                                                            control = Controls[columnName + "ComboBox"];
                                                                            if (control is ComboBox)
                                                                            {
                                                                                ComboBox comboBox = (ComboBox)control;
                                                                                comboBox.SelectedItem = ;
                                                                                originalFieldValues[columnName] = columnValue; // Сохранение оригинального значения поля
                                                                            }
                                                                        }*/
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }
        private async void LoadRecordData()
        {
            using (var connection = new NpgsqlConnection(connectionstring))
            {
                await connection.OpenAsync();

                // Получение данных записи из базы данных
                string query = $"SELECT * FROM \"{table_name}\" WHERE id_{table_name.ToLower()} = @Id";
                try
                {
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        // Параметр Id для фильтрации по идентификатору записи
                        command.Parameters.AddWithValue("@Id", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                // Загрузка значений полей записи в текстовые поля формы
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetName(i);
                                    string columnValue = reader.IsDBNull(i) ? string.Empty : reader.GetValue(i).ToString();

                                    string controlKey = columnName.Replace(" ", "_"); // Преобразование имени столбца в допустимый ключ
                                    Control control = Controls[columnName + "TextBox"];
                                    if (control is TextBox)
                                    {
                                        TextBox textbox = (TextBox)control;
                                        textbox.Text = columnValue;
                                        originalFieldValues[columnName] = columnValue; // Сохранение оригинального значения поля
                                    }
                                    //Работа с справочникаим если будет время добавить
/*                                    else
                                    {
                                        control = Controls[columnName + "ComboBox"];
                                        if (control is ComboBox)
                                        {
                                            ComboBox comboBox = (ComboBox)control;
                                            comboBox.SelectedItem = ;
                                            originalFieldValues[columnName] = columnValue; // Сохранение оригинального значения поля
                                        }
                                    }*/
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool hasSetClause = false;
            if (table_name == "Category.Attribute")
            {
                try
                {
                    connect = new NpgsqlConnection(connectionstring);
                    connect.Open();
                    sql = $"UPDATE \"Attribute\" SET \"attribute_name\" = @attributeValue WHERE \"id_category\" = @id AND \"attribute_name\" = @attributeName";
                    int i = 0;
                    foreach (var control in Controls)
                    {
                        sql = $"UPDATE \"Attribute\" SET \"attribute_name\" = @attributeValue WHERE \"id_category\" = @id AND \"attribute_name\" = @attributeName";
                        if (control is TextBox txt)
                        {
                            if (txt.Text != prevatribute[i] && !(string.IsNullOrEmpty(txt.Text)))
                            {
                                sqlCommand = new NpgsqlCommand(sql, connect);
                                sqlCommand.Parameters.AddWithValue("@attributeValue", txt.Text);
                                sqlCommand.Parameters.AddWithValue("@attributeName", prevatribute[i]);
                                sqlCommand.Parameters.AddWithValue("@id", id);
                                sqlCommand.ExecuteScalar();
                                hasSetClause = true;
                                i++;
                            }
                            else if(txt.Text == prevatribute[i] || string.IsNullOrEmpty(txt.Text))
                            {
                                i++;
                                continue;
                            }
                            
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if(hasSetClause)
                    {
                        MessageBox.Show("Изменения сохранены");
                    }
                    else
                    {
                        MessageBox.Show("Нет изменений для сохранения");
                    }
                    connect.Close();
                    this.Close();
                }
                

            }
            else if(table_name == "Item.Category.Attribute.Attribute_value")
            {
                try
                {
                    connect = new NpgsqlConnection(connectionstring);
                    int index = 0;
                    connect.Open();
                    foreach (var control in Controls)
                    {

                      
                        if (control is TextBox txt)
                        {
                            sql = $@"UPDATE ""Attribute_value"" SET ""attribute_value"" = '{txt.Text}' WHERE ""id_item"" = {id} AND ""id_attribute"" = (SELECT a.""id_attribute"" FROM ""Attribute"" a JOIN ""Category"" c ON a.""id_category"" = c.""id_category"" WHERE c.""category_name"" = (SELECT category_name FROM ""Category"" WHERE id_category = (SELECT id_category FROM ""Item"" WHERE id_item = {id})) AND a.""attribute_name"" = '{attributeName[index]}' AND a.""id_category"" = (SELECT ""id_category"" FROM ""Item"" WHERE ""id_item"" = {id}));";
                            if (txt.Text != prevatribute[index] && !(string.IsNullOrEmpty(txt.Text)))
                            {
                                sqlCommand = new NpgsqlCommand(sql, connect);
                                /*sqlCommand.Parameters.AddWithValue("@Value", txt.Text);
                                sqlCommand.Parameters.AddWithValue("@atribut_name", prevatribute[index]);
                                sqlCommand.Parameters.AddWithValue("@ID", id);*/
                                sqlCommand.ExecuteNonQuery();
                                hasSetClause = true;
                                index++;
                            }
                            else if (txt.Text == prevatribute[index] || string.IsNullOrEmpty(txt.Text))
                            {
                                index++;
                                continue;
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (hasSetClause)
                    {
                        MessageBox.Show("Изменения сохранены");
                    }
                    else
                    {
                        MessageBox.Show("Нет изменений для сохранения");
                    }
                    connect.Close();
                    this.Close();
                }
                
            }
            else
            {
                string typeQuery = $"SELECT \"column_name\", data_type FROM information_schema.columns WHERE table_name = '{table_name}';";

                Dictionary<string, string> fieldTypes = new Dictionary<string, string>();

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionstring))
                {
                    connection.Open();

                    using (NpgsqlCommand typeCommand = new NpgsqlCommand(typeQuery, connection))
                    {
                        using (NpgsqlDataReader typeReader = typeCommand.ExecuteReader())
                        {
                            while (typeReader.Read())
                            {
                                string columnName = typeReader.GetString(0);
                                string dataType = typeReader.GetString(1);

                                fieldTypes[columnName] = dataType;
                            }
                        }
                    }

                    // Параметры запроса обновления записи
                    string query = $"UPDATE \"{table_name}\" SET ";


                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connection;

                        foreach (Control control in Controls)
                        {
                            if (control is TextBox)
                            {
                                TextBox textbox = (TextBox)control;
                                string columnName = textbox.Name.Replace("TextBox", "");

                                // Проверка наличия значения в текстовом поле
                                if (!string.IsNullOrEmpty(textbox.Text))
                                {
                                    // Проверка наличия изменений в значении поля
                                    if (textbox.Text != originalFieldValues[columnName])
                                    {
                                        // Получение типа поля из словаря fieldTypes
                                        string dataType = fieldTypes[columnName];

                                        // Проверка наличия изменений в значении поля в зависимости от типа
                                        if (IsValueModified(textbox.Text, originalFieldValues[columnName], dataType))
                                        {

                                            // Добавление параметра для обновления поля
                                            query += hasSetClause ? $", {columnName} = @{columnName}" : $"{columnName} = @{columnName}";
                                            if (dataType == "integer")
                                            {
                                                var data = Convert.ToInt32(textbox.Text);
                                                command.Parameters.AddWithValue($"@{columnName}", data);
                                            }
                                            else if (dataType == "character varying")
                                            {
                                                string data = textbox.Text;
                                                command.Parameters.AddWithValue($"@{columnName}", data);
                                            }
                                            else if (dataType == "money")
                                            {
                                                decimal data = Convert.ToDecimal(textbox.Text);
                                                command.Parameters.AddWithValue($"@{columnName}", data);
                                            }
                                            else if (dataType == "text")
                                            {
                                                string data = textbox.Text;
                                                command.Parameters.AddWithValue($"@{columnName}", data);
                                            }

                                            hasSetClause = true;
                                        }
                                    }
                                }
                                else
                                {
                                    // Если поле пустое, используем оригинальное значение
                                    textbox.Text = originalFieldValues[columnName];
                                }
                            }

                        }
                        try
                        {
                            // Выполнять обновление только если есть изменения
                            if (hasSetClause)
                            {
                                // Добавление условия фильтрации по идентификатору записи
                                query += $" WHERE id_{table_name.ToLower()} = @Id";
                                command.Parameters.AddWithValue("@Id", id);
                                command.CommandText = query;

                                command.ExecuteNonQuery();

                                MessageBox.Show("Изменения сохранены");
                            }
                            else
                            {
                                MessageBox.Show("Нет изменений для сохранения");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            this.Close();
                        }
                    }
                }
            }
            
        }

        private bool IsValueModified(string newValue, string originalValue, string dataType)
        {
            // Метод для проверки изменения значения поля в зависимости от его типа
            switch (dataType)
            {
                case "integer":
                    int newIntValue;
                    int originalIntValue;
                    if (int.TryParse(newValue, out newIntValue) && int.TryParse(originalValue, out originalIntValue))
                    {
                        return newIntValue != originalIntValue;
                    }
                    break;
                case "money":
                    decimal newDecimalValue;
                    decimal originalDecimalValue;
                    if (decimal.TryParse(newValue, out newDecimalValue) && decimal.TryParse(originalValue, out originalDecimalValue))
                    {
                        return newDecimalValue != originalDecimalValue;
                    }
                    break;
                case "character varying":
                    return newValue != originalValue;
                case "text":
                    return newValue != originalValue;
                // Добавьте другие типы данных, которые используются в вашей таблице
                default:
                    return newValue != originalValue;
            }

            return false;
        }
    }
}
