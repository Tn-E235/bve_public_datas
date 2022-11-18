/* ----------------------------------------------------------------------------
    CSV�t�@�C���ǂݍ��݃T���v���v���O����

    �������l Tn(twitter;@Tn_E235)
    ���t�F2022/11/12
----------------------------------------------------------------------------- */
#include <windows.h>
#include <filesystem>
#include <fstream>
#include <sstream>
#include <string>
#include "SampleCSVReader.h"

using namespace std;

/* ----------------------------------------------------------------------------
    CSV�t�@�C���ǂݍ��݃��\�b�h
----------------------------------------------------------------------------- */
int SampleCSVReader(
        HINSTANCE hModule,              // [IN ]
        string fileName,                // [IN ]�t�@�C����
        vector<vector<string>> data     // [OUT]CSV�t�@�C����荞�݃f�[�^
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
    CSV�t�@�C���ǂݍ��݃��\�b�h
    ��SampleCSVReader�ȊO����Ăяo���s�v
----------------------------------------------------------------------------- */
int SampleCSVReader_ReadCSV(
        filesystem::path cd,            // [IN ]�t�@�C���p�X
        string fileName,                // [IN ]�t�@�C����
        vector<vector<string>> data     // [OUT]CSV�t�@�C����荞�݃f�[�^
) {
    filesystem::path filePath = cd;
    filePath.append(fileName);
    ifstream csvFile(filePath);

    // �t�@�C���̗L�����m�F
    if (!csvFile) {
        // �t�@�C�����Ȃ��Ƃ�
        MessageBox(NULL, TEXT("�t�@�C����������܂���ł����B"),
            TEXT("SampleCSVReader"), MB_OK | MB_ICONINFORMATION);
        return ERR;
    }

    string line = "";
    string str;
    
    // 1�s�ǂݍ���
    while (getline(csvFile, line)) {
        istringstream getData(line);
        vector<string> items;
        // �J���}���Ƃ�1�f�[�^���擾
        while (getline(getData, str, ',')) {
            // 1�f�[�^�����R�[�h�ɒǉ�
            items.push_back(str);
        }
        // 1���R�[�h�Ƃ��ĂԂ�����
        data.push_back(items);
    }
    return DONE;
}