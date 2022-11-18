#ifndef __SAMPLECSVREADER__
#define __SAMPLECSVREADER__

#include <filesystem>

#define DONE 0
#define ERR 1

extern int SampleCSVReader(HINSTANCE hModule, std::string, std::vector<std::vector<std::string>>);
extern int SampleCSVReader_ReadCSV(std::filesystem::path cd, std::string, std::vector<std::vector<std::string>> data);

#endif /* __SAMPLECSVREADER__ */