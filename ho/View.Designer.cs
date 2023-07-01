
namespace ho
{
    partial class View
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(View));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1_add = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.label_page_count = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.iTEMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.компанииToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.страныToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.клиентыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.поставщикиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.товарыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.категорииToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.категорииИИхСвойстваToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.товарыToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.товарыСКатегориямиИСвойстваимиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.контрактыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button_change_data = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.Find_button = new System.Windows.Forms.Button();
            this.запросыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_double_click_on_cell);
            // 
            // button1_add
            // 
            this.button1_add.BackColor = System.Drawing.Color.LavenderBlush;
            resources.ApplyResources(this.button1_add, "button1_add");
            this.button1_add.Name = "button1_add";
            this.button1_add.UseVisualStyleBackColor = false;
            this.button1_add.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.LavenderBlush;
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.LavenderBlush;
            resources.ApplyResources(this.button4, "button4");
            this.button4.Name = "button4";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.LavenderBlush;
            resources.ApplyResources(this.button5, "button5");
            this.button5.Name = "button5";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button_Shift_page_left_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.LavenderBlush;
            resources.ApplyResources(this.button6, "button6");
            this.button6.Name = "button6";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button_Shift_page_right_Click);
            // 
            // label_page_count
            // 
            resources.ApplyResources(this.label_page_count, "label_page_count");
            this.label_page_count.Name = "label_page_count";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Tan;
            this.label3.Name = "label3";
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.LavenderBlush;
            resources.ApplyResources(this.button7, "button7");
            this.button7.Name = "button7";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button_Show_Full_Table_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iTEMToolStripMenuItem,
            this.поставщикиToolStripMenuItem,
            this.товарыToolStripMenuItem,
            this.контрактыToolStripMenuItem,
            this.запросыToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // iTEMToolStripMenuItem
            // 
            this.iTEMToolStripMenuItem.BackColor = System.Drawing.Color.Tan;
            this.iTEMToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.компанииToolStripMenuItem,
            this.страныToolStripMenuItem,
            this.клиентыToolStripMenuItem});
            this.iTEMToolStripMenuItem.Name = "iTEMToolStripMenuItem";
            resources.ApplyResources(this.iTEMToolStripMenuItem, "iTEMToolStripMenuItem");
            // 
            // компанииToolStripMenuItem
            // 
            this.компанииToolStripMenuItem.Name = "компанииToolStripMenuItem";
            resources.ApplyResources(this.компанииToolStripMenuItem, "компанииToolStripMenuItem");
            this.компанииToolStripMenuItem.Click += new System.EventHandler(this.компанииToolStripMenuItem_Click);
            // 
            // страныToolStripMenuItem
            // 
            this.страныToolStripMenuItem.Name = "страныToolStripMenuItem";
            resources.ApplyResources(this.страныToolStripMenuItem, "страныToolStripMenuItem");
            this.страныToolStripMenuItem.Click += new System.EventHandler(this.страныToolStripMenuItem_Click);
            // 
            // клиентыToolStripMenuItem
            // 
            this.клиентыToolStripMenuItem.Name = "клиентыToolStripMenuItem";
            resources.ApplyResources(this.клиентыToolStripMenuItem, "клиентыToolStripMenuItem");
            this.клиентыToolStripMenuItem.Click += new System.EventHandler(this.клиентыToolStripMenuItem_Click);
            // 
            // поставщикиToolStripMenuItem
            // 
            this.поставщикиToolStripMenuItem.BackColor = System.Drawing.Color.Tan;
            this.поставщикиToolStripMenuItem.Name = "поставщикиToolStripMenuItem";
            resources.ApplyResources(this.поставщикиToolStripMenuItem, "поставщикиToolStripMenuItem");
            this.поставщикиToolStripMenuItem.Click += new System.EventHandler(this.поставщикиToolStripMenuItem_Click);
            // 
            // товарыToolStripMenuItem
            // 
            this.товарыToolStripMenuItem.BackColor = System.Drawing.Color.Tan;
            this.товарыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.категорииToolStripMenuItem1,
            this.товарыToolStripMenuItem1,
            this.товарыСКатегориямиИСвойстваимиToolStripMenuItem});
            this.товарыToolStripMenuItem.Name = "товарыToolStripMenuItem";
            resources.ApplyResources(this.товарыToolStripMenuItem, "товарыToolStripMenuItem");
            // 
            // категорииToolStripMenuItem1
            // 
            this.категорииToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.категорииИИхСвойстваToolStripMenuItem});
            this.категорииToolStripMenuItem1.Name = "категорииToolStripMenuItem1";
            resources.ApplyResources(this.категорииToolStripMenuItem1, "категорииToolStripMenuItem1");
            this.категорииToolStripMenuItem1.Click += new System.EventHandler(this.категорииToolStripMenuItem1_Click);
            // 
            // категорииИИхСвойстваToolStripMenuItem
            // 
            this.категорииИИхСвойстваToolStripMenuItem.Name = "категорииИИхСвойстваToolStripMenuItem";
            resources.ApplyResources(this.категорииИИхСвойстваToolStripMenuItem, "категорииИИхСвойстваToolStripMenuItem");
            this.категорииИИхСвойстваToolStripMenuItem.Click += new System.EventHandler(this.категорииИИхСвойстваToolStripMenuItem_Click);
            // 
            // товарыToolStripMenuItem1
            // 
            this.товарыToolStripMenuItem1.Name = "товарыToolStripMenuItem1";
            resources.ApplyResources(this.товарыToolStripMenuItem1, "товарыToolStripMenuItem1");
            this.товарыToolStripMenuItem1.Click += new System.EventHandler(this.товарыToolStripMenuItem1_Click);
            // 
            // товарыСКатегориямиИСвойстваимиToolStripMenuItem
            // 
            this.товарыСКатегориямиИСвойстваимиToolStripMenuItem.Name = "товарыСКатегориямиИСвойстваимиToolStripMenuItem";
            resources.ApplyResources(this.товарыСКатегориямиИСвойстваимиToolStripMenuItem, "товарыСКатегориямиИСвойстваимиToolStripMenuItem");
            this.товарыСКатегориямиИСвойстваимиToolStripMenuItem.Click += new System.EventHandler(this.товарыСКатегориямиИСвойстваимиToolStripMenuItem_Click);
            // 
            // контрактыToolStripMenuItem
            // 
            this.контрактыToolStripMenuItem.BackColor = System.Drawing.Color.Tan;
            this.контрактыToolStripMenuItem.Name = "контрактыToolStripMenuItem";
            resources.ApplyResources(this.контрактыToolStripMenuItem, "контрактыToolStripMenuItem");
            this.контрактыToolStripMenuItem.Click += new System.EventHandler(this.контрактыToolStripMenuItem_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Tan;
            this.label2.Name = "label2";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Tan;
            this.label4.Name = "label4";
            // 
            // button_change_data
            // 
            this.button_change_data.BackColor = System.Drawing.Color.LavenderBlush;
            resources.ApplyResources(this.button_change_data, "button_change_data");
            this.button_change_data.Name = "button_change_data";
            this.button_change_data.UseVisualStyleBackColor = false;
            this.button_change_data.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Tan;
            this.label5.Name = "label5";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            resources.GetString("comboBox1.Items"),
            resources.GetString("comboBox1.Items1")});
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Tan;
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.Tan;
            this.label7.Name = "label7";
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.LavenderBlush;
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.DeteleAll);
            // 
            // Find_button
            // 
            resources.ApplyResources(this.Find_button, "Find_button");
            this.Find_button.Name = "Find_button";
            this.Find_button.UseVisualStyleBackColor = true;
            this.Find_button.Click += new System.EventHandler(this.Find_button_click);
            // 
            // запросыToolStripMenuItem
            // 
            this.запросыToolStripMenuItem.BackColor = System.Drawing.Color.Tan;
            this.запросыToolStripMenuItem.Name = "запросыToolStripMenuItem";
            resources.ApplyResources(this.запросыToolStripMenuItem, "запросыToolStripMenuItem");
            // 
            // View
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.Find_button);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button_change_data);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label_page_count);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1_add);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "View";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1_add;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label_page_count;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem iTEMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem компанииToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem страныToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem клиентыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem поставщикиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem товарыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem контрактыToolStripMenuItem;
        private System.Windows.Forms.Button button_change_data;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem категорииToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem товарыToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem товарыСКатегориямиИСвойстваимиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem категорииИИхСвойстваToolStripMenuItem;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button Find_button;
        private System.Windows.Forms.ToolStripMenuItem запросыToolStripMenuItem;
    }
}