namespace AugerMixer.Forms
{
    partial class InventorControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventorControl));
            this.label1 = new System.Windows.Forms.Label();
            this.inventorVersions = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.InventorLaunch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ЗДЕСЬ ЕСТЬ ТЕКСТ";
            // 
            // InventorVersions
            // 
            this.inventorVersions.FormattingEnabled = true;
            this.inventorVersions.Location = new System.Drawing.Point(64, 32);
            this.inventorVersions.Name = "InventorVersions";
            this.inventorVersions.Size = new System.Drawing.Size(55, 21);
            this.inventorVersions.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Inventor";
            // 
            // InventorLaunch
            // 
            this.InventorLaunch.Location = new System.Drawing.Point(125, 32);
            this.InventorLaunch.Name = "InventorLaunch";
            this.InventorLaunch.Size = new System.Drawing.Size(69, 21);
            this.InventorLaunch.TabIndex = 3;
            this.InventorLaunch.Text = "Запустить";
            this.InventorLaunch.UseVisualStyleBackColor = true;
            this.InventorLaunch.Click += new System.EventHandler(this.InventorLaunch_Click);
            // 
            // InventorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(200, 57);
            this.Controls.Add(this.InventorLaunch);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.inventorVersions);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InventorControl";
            this.Text = "Запуск Inventor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox inventorVersions;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button InventorLaunch;
    }
}