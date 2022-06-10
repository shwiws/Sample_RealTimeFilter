using Microsoft.VisualBasic;
using System.Linq;
namespace RealTimeFilter
{
    /// <summary>
    /// 上場企業クラス
    /// </summary>
    public class ListedCompany
    {
        public string 銘柄名 { get; set; }
        public string 証券コード { get; set; }
        public string 上場区分 { get; set; }
        public string 読み { get; set; }
        private readonly string 検索用キーワード;
        public ListedCompany(string 銘柄名, string 証券コード, string 上場区分, string 読み)
        {
            this.銘柄名 = 銘柄名;
            this.証券コード = 証券コード;
            this.上場区分 = 上場区分;
            this.読み = 読み;
            this.検索用キーワード = string.Join(
                "|",
                new string[] { 銘柄名, 証券コード, 上場区分, 読み }
                    .Select(x => Strings.StrConv(x, VbStrConv.Hiragana | VbStrConv.Wide | VbStrConv.Uppercase)));
        }

        /// <summary>
        /// キーワードにマッチするか
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public bool IsMatch(string keyword)
        {
            return this.検索用キーワード.Contains(keyword);
        }
    }
}
