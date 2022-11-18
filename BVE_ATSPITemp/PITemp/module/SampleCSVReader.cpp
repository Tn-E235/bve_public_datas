/* ----------------------------------------------------------------------------
    CSVファイル読み込みサンプルプログラム

    書いた人 Tn(twitter;@Tn_E235)
    日付：2022/11/12
----------------------------------------------------------------------------- */
#include <windows.h>
#include <filesystem>
#include <fstream>
#include <sstream>
#include <string>
#include "SampleCSVReader.h"

using namespace std;

/* ----------------------------------------------------------------------------
    CSVファイル読み込みメソッド
----------------------------------------------------------------------------- */
int SampleCSVReader(
        HINSTANCE hModule,              // [IN ]
        string fileName,                // [IN ]ファイル名
        vector<vector<string>> data     // [OUT]CSVファイル取り込みデータ
) {
    TCHAR dllpath[_MAX_PATH];

    GetModuleFileName(
        hModule, dllpath, sizeof(dllpath)
    );

    filesystem::path folderPath(dllpath);
    folderPath.remove_filename();
    int ret;
    ret = SampleCSVReader_ReadCSV(folderPath, fileName, data);
    if (ret != 0) {
        return ERR;
    }

    return DONE;
}

/* ----------------------------------------------------------------------------
    CSVファイル読み込みメソッド
    ※SampleCSVReader以外から呼び出し不要
----------------------------------------------------------------------------- */
int SampleCSVReader_ReadCSV(
        filesystem::path cd,            // [IN ]ファイルパス
        string fileName,                // [IN ]ファイル名
        vector<vector<string>> data     // [OUT]CSVファイル取り込みデータ
) {
    filesystem::path filePath = cd;
    filePath.append(fileName);
    ifstream csvFile(filePath);

    // ファイルの有無を確認
    if (!csvFile) {
        // ファイルがないとき
        MessageBox(NULL, TEXT("ファイルが見つかりませんでした。"),
            TEXT("SampleCSVReader"), MB_OK | MB_ICONINFORMATION);
        return ERR;
    }

    string line = "";
    string str;
    
    // 1行つ読み込み
    while (getline(csvFile, line)) {
        istringstream getData(line);
        vector<string> items;
        // カンマごとに1データを取得
        while (getline(getData, str, ',')) {
            // 1データをレコードに追加
            items.push_back(str);
        }
        // 1レコードとしてぶち込む
        data.push_back(items);
    }
    return DONE;
}