using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

using std_ez;
using OldW;

namespace OldW
{
    namespace Modeling
    {
        public partial class FormSetWarning : Form
        {
            private Dictionary<String, WarningValue> listWarning = new Dictionary<String, WarningValue>();

            public FormSetWarning()
            {
                InitializeComponent();
                //cbRate.SelectedItem = cbRate.Items[0];
            }

            private void button1_Click(object sender, EventArgs e)
            {
                //得到界面的值
                WarningValue warning = getFormValve();
                String warningId = warning.getId();

                //重复命名即为修改,没有重复即添加
                if (listWarning.ContainsKey(warningId) == true)
                {
                    listWarning[warningId] = warning;
                }
                else
                {
                    listWarning.Add(warningId, warning);
                }
                
                try
                {
                    //保存
                    FileStream dataFile = new FileStream(Path.Combine(GlobalSettings.Path_data, "WarningValue.dat"), FileMode.Create);
                    StreamWriter sw = new StreamWriter(dataFile);
                    foreach(var ele in listWarning)
                    {
                        String warningEleEncode = BinarySerializer.Encode64(ele.Value);
                        sw.WriteLine(warningEleEncode);
                    }
                    sw.Close();
                    //当前的选择
                    FileStream dataFileUsing = new FileStream(Path.Combine(GlobalSettings.Path_data, "WarningValueUsing.dat"), FileMode.Create);
                    StreamWriter swUsing = new StreamWriter(dataFileUsing);
                    String warningUsingEncode = BinarySerializer.Encode64(warning);
                    swUsing.WriteLine(warningUsingEncode);
                    swUsing.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    this.Close();
                }
                updatecbInput();
                this.Close();
            }

            /// <summary>
            /// 读取界面上的值，创建WarningValue对象
            /// </summary>
            /// <returns></returns>
            private WarningValue getFormValve()
            {
                String name = tbName.Text;
                String rate = cbRate.Text;

                double incloSum;
                double.TryParse(tbIncliSum.Text, out incloSum);
                double incloVelo;
                double.TryParse(tbIncliVelo.Text, out incloVelo);
                WarningIncli incli = new WarningIncli(incloSum, incloVelo);

                double setlSum;
                double.TryParse(tbGSetlSum.Text, out setlSum);
                double setlVelo;
                double.TryParse(tbGSetlVelo.Text, out setlVelo);
                WarningGSetle setle = new WarningGSetle(setlSum, setlVelo);

                double forceRate;
                double.TryParse(tbForceRatio.Text, out forceRate);
                WarningForce force = new WarningForce(forceRate);

                //创建警戒值类
                WarningValue warning = new WarningValue(name, rate, incli, setle, force);
                return warning;
            }

            private void btnFalse_Click(object sender, EventArgs e)
            {
                this.Close();
            }

            private void FormSetWarning_Load(object sender, EventArgs e)
            {
                //combobox等级
                cbRate.SelectedItem = cbRate.Items[0];

                //combobox input
                updatecbInput();

                
            }

            /// <summary>
            /// 更新cbInput
            /// </summary>
            private void updatecbInput()
            {
                String strLine;
                listWarning.Clear();
                cbInput.Items.Clear();
                try
                {
                    FileStream dataFile = new FileStream(Path.Combine(GlobalSettings.Path_data, "WarningValue.dat"), FileMode.Open);
                    StreamReader sr = new StreamReader(dataFile);
                    strLine = sr.ReadLine();
                    while (strLine != null)
                    {
                        WarningValue warning = BinarySerializer.Decode64(strLine) as WarningValue;
                        String warningId = warning.getId();
                        this.cbInput.Items.Add(warningId);
                        listWarning.Add(warningId, warning);
                        strLine = sr.ReadLine();
                    }
                    sr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            private void tbIncliSum_KeyPress(object sender, KeyPressEventArgs e)
            {     
                checkNum(ref this.tbIncliSum,ref e);
            }

            /// <summary>
            /// 判断输入是否合理
            /// </summary>
            /// <param name="tb"></param>
            /// <param name="e"></param>
            private void checkNum(ref TextBox tb, ref KeyPressEventArgs e)
            {
                if (e.KeyChar == '.' && tb.Text.IndexOf(".") != -1)
                {
                    e.Handled = true;
                }

                if (!((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == '.' || e.KeyChar == 8))
                {
                    e.Handled = true;
                }  
            }

            private void tbIncliVelo_KeyPress(object sender, KeyPressEventArgs e)
            {
                checkNum(ref this.tbIncliVelo, ref e);
            }

            private void tbGSetlSum_KeyPress(object sender, KeyPressEventArgs e)
            {
                checkNum(ref this.tbGSetlSum, ref e);
            }

            private void tbGSetlVelo_KeyPress(object sender, KeyPressEventArgs e)
            {
                checkNum(ref this.tbGSetlVelo, ref e);
            }

            private void tbForceRatio_KeyPress(object sender, KeyPressEventArgs e)
            {
                checkNum(ref this.tbForceRatio, ref e);
            }

            private void cbInput_SelectedIndexChanged(object sender, EventArgs e)
            {
                WarningValue warning = listWarning[cbInput.Text];
                tbName.Text = warning.name;
                cbRate.SelectedItem = warning.rate;
                tbIncliSum.Text = warning.warningIncli.sum.ToString();
                tbIncliVelo.Text = warning.warningIncli.velo.ToString();
                tbGSetlSum.Text = warning.warningGSetle.sum.ToString();
                tbGSetlVelo.Text = warning.warningGSetle.velo.ToString();
                tbForceRatio.Text = warning.warningForc.ratio.ToString();
            }

            private void btnDele_Click(object sender, EventArgs e)
            {
                //得到界面的值
                WarningValue warning = getFormValve();
                String warningId = warning.getId();
                listWarning.Remove(warningId);

                //保存
                FileStream dataFile = new FileStream(Path.Combine(GlobalSettings.Path_data, "WarningValue.dat"), FileMode.Create);
                StreamWriter sw = new StreamWriter(dataFile);
                foreach (var ele in listWarning)
                {
                    String warningEleEncode = BinarySerializer.Encode64(ele.Value);
                    sw.WriteLine(warningEleEncode);
                }
                sw.Close();

                //更新
                updatecbInput();
                return;
            }
        }
    }
}
