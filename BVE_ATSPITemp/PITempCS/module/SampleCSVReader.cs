/* ----------------------------------------------------------------------------
    CSVファイル読み込みサンプルプログラム

    書いた人 Tn(twitter;@Tn_E235)
    日付：2022/11/12
----------------------------------------------------------------------------- */
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace PITempCS.module {

    public class SampleCSVReader {

        const int DONE = 0; // 返値(成功)
        const int ERR = 1;  // 返値(失敗)

        string filePath;    // ファイルパス
        bool loaded;        // 読み込み可能フラグ

        /* --------------------------------------------------------------------
            インスタンス生成
        -------------------------------------------------------------------- */
        public SampleCSVReader() {
            filePath = "";
            loaded = false;
        }

        /* --------------------------------------------------------------------
            初期化メソッド
            (クラス外からの呼び出しは不要です)
        -------------------------------------------------------------------- */
        private int init(
                string fileName // [IN ]読み込みたいファイルの名前(パスは含まない)
        ){
            // AtsPluginの自DLLパス
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            // ファイルパスの文字数
            int pathLength = path.Length;
            // ファイルパスの後ろから'\'を検索
            int index = path.LastIndexOf('\\');
            // '\'が見つからなかったとき
            if (index < 0) { return ERR; }
            // 実行DLL名を削除
            path = path.Remove(index + 1, pathLength - index - 1);

            try {
                // ファイルを開く
                FileStream file = File.Open(path + fileName, FileMode.Open);
                file.Close();
            }
            catch (FileNotFoundException fnfex) {
                // ファイルが見つからないとき
                MessageBox.Show(fnfex.Message, "[SampleCSVReader]CSVファイルが見つかりません。");
                return ERR;
            }

            // ファイル読み込み可能フラグを立てる
            loaded = true;
            // ファイルパスを保存
            filePath = path + fileName;
            return DONE;
        }

        /* --------------------------------------------------------------------
            ファイル読み込みメソッド
            (クラス外からの呼び出しは不要です)
            
            初期化メソッドにより設定されたファイルが
            読み込み可能であると判断された場合、
            CSVファイルを読み込みます
        -------------------------------------------------------------------- */
        private int readFile(
            ref List<List<string>> list // [OUT]CSVファイル取り込みデータ
        ) {
            // 初期化処理が呼ばれていない/ファイルが読み込めないとき
            if (loaded == false) { return ERR; }
            // リスト初期化
            list.Clear();

            try {
                // ファイルを開く
                StreamReader file = new StreamReader(filePath);
                // 末尾まで繰り返す
                while (!file.EndOfStream) {
                    // 一行を読み込む
                    string line = file.ReadLine();
                    // カンマ分けして配列にいれる
                    string[] values = line.Split(',');
                    // 配列からリストに変換
                    List<string> lists = new List<string>();
                    lists.AddRange(values);
                    // リストに1レコードとしてぶち込む
                    list.Add(lists);
                }
                file.Close();
            }
            catch (FileNotFoundException fnfex) {
                // ファイルが見つからないとき
                MessageBox.Show(fnfex.Message, "[SampleCSVReader]CSVファイルが見つかりません。");
                return ERR;
            }

            return DONE;
        }

        /* --------------------------------------------------------------------
            CSVファイル読み込みメソッド
            
            インスタンス生成後に実行します
        -------------------------------------------------------------------- */
        public int readFile(
                string fileName,            // [IN ]ファイル名
                ref List<List<string>> list // [OUT]CSVデータ取り込み用変数
        ) {
            // 返値
            int ret;
            // 初期化
            ret = init(fileName);
            if (ret != 0) {
                // ファイルがないとき
                return ERR;
            }
            // ファイル読み込み
            ret = readFile(ref list);
            if (ret != 0) {
                // ファイルが読み込めないとき
                return ERR;
            }
            return DONE;
        }
    }
}
