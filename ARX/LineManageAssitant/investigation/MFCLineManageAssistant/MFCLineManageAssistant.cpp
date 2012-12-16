// MFCLineManageAssistant.cpp : Defines the initialization routines for the DLL.
//

#include "MenuManager.h"

#include "stdafx.h"
#include "MFCLineManageAssistant.h"

#include "AcExtensionModule.h"

#include "CutCommandMgr.h"

#include "rxregsvc.h"
#include "acutads.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//
//TODO: If this DLL is dynamically linked against the MFC DLLs,
//		any functions exported from this DLL which call into
//		MFC must have the AFX_MANAGE_STATE macro added at the
//		very beginning of the function.
//
//		For example:
//
//		extern "C" BOOL PASCAL EXPORT ExportedFunction()
//		{
//			AFX_MANAGE_STATE(AfxGetStaticModuleState());
//			// normal function body here
//		}
//
//		It is very important that this macro appear in each
//		function, prior to any calls into MFC.  This means that
//		it must appear as the first statement within the 
//		function, even before any object variable declarations
//		as their constructors may generate calls into the MFC
//		DLL.
//
//		Please see MFC Technical Notes 33 and 58 for additional
//		details.
//

// CMFCLineManageAssistantApp

BEGIN_MESSAGE_MAP(CMFCLineManageAssistantApp, CWinApp)
END_MESSAGE_MAP()

extern "C" HWND adsw_acadMainWnd();

/////////////////////////////////////////////////////////////////////////////
// Define the sole extension module object.

AC_IMPLEMENT_EXTENSION_MODULE(theArxDLL);

//////////////////////////////////////////////////////////////
//
// Entry points
//
//////////////////////////////////////////////////////////////

extern "C" int APIENTRY
DllMain(HINSTANCE hInstance, DWORD dwReason, LPVOID lpReserved)
{
    // Remove this if you use lpReserved
    UNREFERENCED_PARAMETER(lpReserved);
    
    if (dwReason == DLL_PROCESS_ATTACH)
    {
        theArxDLL.AttachInstance(hInstance);
    }
    else if (dwReason == DLL_PROCESS_DETACH)
    {
        theArxDLL.DetachInstance();  
    }
    return 1;   // ok
}


// CMFCLineManageAssistantApp construction

CMFCLineManageAssistantApp::CMFCLineManageAssistantApp()
{
	// TODO: add construction code here,
	// Place all significant initialization in InitInstance
}


// The one and only CMFCLineManageAssistantApp object

CMFCLineManageAssistantApp theApp;


// CMFCLineManageAssistantApp initialization

BOOL CMFCLineManageAssistantApp::InitInstance()
{
	CWinApp::InitInstance();

	return TRUE;
}

AcRx::AppRetCode acrxEntryPoint(AcRx::AppMsgCode msg, void* appId)
{
	switch(msg)
	{
	case AcRx::kInitAppMsg:
		acrxUnlockApplication(appId);
		acrxRegisterAppMDIAware(appId);

		//注册命令
		CutCommandMgr::instance()->registerCommands();

		//注册菜单
		MenuManager::CreateMenu(appId);

		acutPrintf(L"管线辅助系统加载成功\n");
		break;

	case AcRx::kUnloadAppMsg:

		MenuManager::unRegister();

		acutPrintf(L"管线辅助系统卸载成功\n");
		break;
	}

	return AcRx::kRetOK;
}
