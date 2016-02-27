namespace OldW
{
    namespace Modeling
    {
        partial class FormSetWarning 
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnTrue = new System.Windows.Forms.Button();
            this.btnFalse = new System.Windows.Forms.Button();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbInput = new System.Windows.Forms.ComboBox();
            this.cbRate = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbForceRatio = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbGSetlVelo = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbGSetlSum = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbIncliVelo = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbIncliSum = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDele = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "导入";
            // 
            // btnTrue
            // 
            this.btnTrue.Location = new System.Drawing.Point(67, 421);
            this.btnTrue.Name = "btnTrue";
            this.btnTrue.Size = new System.Drawing.Size(75, 23);
            this.btnTrue.TabIndex = 1;
            this.btnTrue.Text = "确定";
            this.btnTrue.UseVisualStyleBackColor = true;
            this.btnTrue.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnFalse
            // 
            this.btnFalse.Location = new System.Drawing.Point(232, 421);
            this.btnFalse.Name = "btnFalse";
            this.btnFalse.Size = new System.Drawing.Size(75, 23);
            this.btnFalse.TabIndex = 2;
            this.btnFalse.Text = "取消";
            this.btnFalse.UseVisualStyleBackColor = true;
            this.btnFalse.Click += new System.EventHandler(this.btnFalse_Click);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(67, 87);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(100, 21);
            this.tbName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "名称";
            // 
            // cbInput
            // 
            this.cbInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInput.FormattingEnabled = true;
            this.cbInput.Location = new System.Drawing.Point(67, 36);
            this.cbInput.Name = "cbInput";
            this.cbInput.Size = new System.Drawing.Size(100, 20);
            this.cbInput.TabIndex = 5;
            this.cbInput.SelectedIndexChanged += new System.EventHandler(this.cbInput_SelectedIndexChanged);
            // 
            // cbRate
            // 
            this.cbRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRate.FormattingEnabled = true;
            this.cbRate.Items.AddRange(new object[] {
            "一级",
            "二级",
            "三级"});
            this.cbRate.Location = new System.Drawing.Point(232, 87);
            this.cbRate.Name = "cbRate";
            this.cbRate.Size = new System.Drawing.Size(100, 20);
            this.cbRate.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(185, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "等级";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.tbForceRatio);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.tbGSetlVelo);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.tbGSetlSum);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.tbIncliVelo);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.tbIncliSum);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(12, 124);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(349, 281);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设定";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(44, 207);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(29, 12);
            this.label12.TabIndex = 19;
            this.label12.Text = "系数";
            // 
            // tbForceRatio
            // 
            this.tbForceRatio.Location = new System.Drawing.Point(79, 204);
            this.tbForceRatio.Name = "tbForceRatio";
            this.tbForceRatio.Size = new System.Drawing.Size(76, 21);
            this.tbForceRatio.TabIndex = 18;
            this.tbForceRatio.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbForceRatio_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(173, 142);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 12);
            this.label9.TabIndex = 17;
            this.label9.Text = "变化速率(mm/d)";
            // 
            // tbGSetlVelo
            // 
            this.tbGSetlVelo.Location = new System.Drawing.Point(268, 139);
            this.tbGSetlVelo.Name = "tbGSetlVelo";
            this.tbGSetlVelo.Size = new System.Drawing.Size(76, 21);
            this.tbGSetlVelo.TabIndex = 16;
            this.tbGSetlVelo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbGSetlVelo_KeyPress);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 142);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 15;
            this.label10.Text = "累计值(mm)";
            // 
            // tbGSetlSum
            // 
            this.tbGSetlSum.Location = new System.Drawing.Point(79, 139);
            this.tbGSetlSum.Name = "tbGSetlSum";
            this.tbGSetlSum.Size = new System.Drawing.Size(76, 21);
            this.tbGSetlSum.TabIndex = 14;
            this.tbGSetlSum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbGSetlSum_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(173, 70);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 12);
            this.label8.TabIndex = 13;
            this.label8.Text = "变化速率(mm/d)";
            // 
            // tbIncliVelo
            // 
            this.tbIncliVelo.Location = new System.Drawing.Point(268, 67);
            this.tbIncliVelo.Name = "tbIncliVelo";
            this.tbIncliVelo.Size = new System.Drawing.Size(76, 21);
            this.tbIncliVelo.TabIndex = 12;
            this.tbIncliVelo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbIncliVelo_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 70);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "累计值(mm)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 174);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "轴力";
            // 
            // tbIncliSum
            // 
            this.tbIncliSum.Location = new System.Drawing.Point(79, 67);
            this.tbIncliSum.Name = "tbIncliSum";
            this.tbIncliSum.Size = new System.Drawing.Size(76, 21);
            this.tbIncliSum.TabIndex = 9;
            this.tbIncliSum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbIncliSum_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "地表沉降";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "侧斜";
            // 
            // btnDele
            // 
            this.btnDele.Location = new System.Drawing.Point(232, 34);
            this.btnDele.Name = "btnDele";
            this.btnDele.Size = new System.Drawing.Size(75, 23);
            this.btnDele.TabIndex = 9;
            this.btnDele.Text = "删除";
            this.btnDele.UseVisualStyleBackColor = true;
            this.btnDele.Click += new System.EventHandler(this.btnDele_Click);
            // 
            // FormSetWarning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 456);
            this.Controls.Add(this.btnDele);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbRate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.btnFalse);
            this.Controls.Add(this.btnTrue);
            this.Controls.Add(this.label1);
            this.Name = "FormSetWarning";
            this.Text = "设置警戒值";
            this.Load += new System.EventHandler(this.FormSetWarning_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

            }

            #endregion

            private System.Windows.Forms.Label label1;
            private System.Windows.Forms.Button btnTrue;
            private System.Windows.Forms.Button btnFalse;
            private System.Windows.Forms.TextBox tbName;
            private System.Windows.Forms.Label label2;
            private System.Windows.Forms.ComboBox cbInput;
            private System.Windows.Forms.ComboBox cbRate;
            private System.Windows.Forms.Label label3;
            private System.Windows.Forms.GroupBox groupBox1;
            private System.Windows.Forms.Label label12;
            private System.Windows.Forms.TextBox tbForceRatio;
            private System.Windows.Forms.Label label9;
            private System.Windows.Forms.TextBox tbGSetlVelo;
            private System.Windows.Forms.Label label10;
            private System.Windows.Forms.TextBox tbGSetlSum;
            private System.Windows.Forms.Label label8;
            private System.Windows.Forms.TextBox tbIncliVelo;
            private System.Windows.Forms.Label label7;
            private System.Windows.Forms.Label label6;
            private System.Windows.Forms.TextBox tbIncliSum;
            private System.Windows.Forms.Label label5;
            private System.Windows.Forms.Label label4;
            private System.Windows.Forms.Button btnDele;
        }
    }
}