// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the ARXDEMO_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// ARXDEMO_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef ARXDEMO_EXPORTS
#define ARXDEMO_API __declspec(dllexport)
#else
#define ARXDEMO_API __declspec(dllimport)
#endif

// This class is exported from the ARXDemo.dll
class ARXDEMO_API CARXDemo {
public:
	CARXDemo(void);
	// TODO: add your methods here.
};

extern ARXDEMO_API int nARXDemo;
ARXDEMO_API int fnARXDemo(void);
