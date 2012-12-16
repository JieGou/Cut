// ARXDemo.cpp : Defines the exported functions for the DLL application.
//

#include "LineManageAssistant.h"

#include "rxregsvc.h"
#include "acutads.h"

#include "CutCommandMgr.h"

// This is an example of an exported variable
ARXDEMO_API int nARXDemo=0;

// This is an example of an exported function.
ARXDEMO_API int fnARXDemo(void)
{
	return 42;
}

// This is the constructor of a class that has been exported.
// see ARXDemo.h for the class definition
CARXDemo::CARXDemo()
{
	return;
}

AcRx::AppRetCode acrxEntryPoint(AcRx::AppMsgCode msg, void* appId)
{
	switch(msg)
	{
	case AcRx::kInitAppMsg:
		acrxUnlockApplication(appId);
		acrxRegisterAppMDIAware(appId);

		CutCommandMgr::instance()->registerCommands();

		acutPrintf(L"Example Application Loaded\n");
		break;

	case AcRx::kUnloadAppMsg:
		acutPrintf(L"Example Application Unloaded\n");
		break;
	}

	return AcRx::kRetOK;
}
