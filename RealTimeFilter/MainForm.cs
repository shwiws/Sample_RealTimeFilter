using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace RealTimeFilter
{
    public partial class MainForm : Form
    {
        private readonly DataTable dataTable = new DataTable("上場企業一覧");
        public MainForm()
        {
            this.InitializeComponent();
            this.dataTable.Columns.Add(nameof(ListedCompany.銘柄名));
            this.dataTable.Columns.Add(nameof(ListedCompany.証券コード));
            this.dataTable.Columns.Add(nameof(ListedCompany.上場区分));
            this.dataTable.Columns.Add(nameof(ListedCompany.読み));
            ListedCompanies.Data.ForEach(c =>
            {
                this.dataTable.Rows.Add(c.銘柄名, c.証券コード,c.上場区分,c.読み);
            });
            this.dataGridView.DataSource = this.dataTable;
        }

        /// <summary>
        /// キー押下イベント。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var context = Imm.ImmGetContext((sender as TextBox).Handle);
            // 取得できない場合は終了
            if (context == IntPtr.Zero)
            {
                return;
            }
            try
            {
                // IME入力値の取得
                var buf = new byte[1024];
                var length = Imm.ImmGetCompositionStringW(context, Imm.GCS_COMPSTR, buf, buf.Length);
                if (length == 0)
                {
                    // 入力中でなかった場合は何もしない
                }
                else if (length >= 0)
                {
                    var composition = Encoding.Unicode.GetString(buf, 0, length);
                    // 現在の入力内容
                    var searchExpression =
                        this.uiSearchWordTextBox.Text.Substring(0, this.uiSearchWordTextBox.SelectionStart)
                        + composition
                        + this.uiSearchWordTextBox.Text.Substring(this.uiSearchWordTextBox.SelectionStart + this.uiSearchWordTextBox.SelectionLength);

                    // 行の表示・非表示
                    this.FilterBySearchWord(searchExpression);
                }
                else
                {
                    // 0未満はエラー
                }
            }
            finally
            {
                Imm.ImmReleaseContext(this.uiSearchWordTextBox.Handle, context);
            }
        }

        /// <summary>
        /// 検索ボックスの文字列確定後のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            this.FilterBySearchWord(this.uiSearchWordTextBox.Text);
        }

        /// <summary>
        /// データのフィルタリング
        /// </summary>
        /// <param name="searchWord">検索ワード</param>
        private void FilterBySearchWord(string searchWord)
        {
            // 検索ワードにマッチする企業の証券コードを取得する。
            var upperWideSearchWord = Strings.StrConv(searchWord, VbStrConv.Hiragana | VbStrConv.Wide | VbStrConv.Uppercase);
            var codes = ListedCompanies.Data.Where(company => company.IsMatch(upperWideSearchWord)).Select(company => $"'{company.証券コード}'");
            this.dataTable.DefaultView.RowFilter = codes.Any() ? $"[証券コード] IN ({string.Join(",", codes)}) " : null;
            Debug.WriteLine(this.dataTable.DefaultView.RowFilter);
        }
    }
}
