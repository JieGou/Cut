// MFCLineManageAssistant.h : main header file for the MFCLineManageAssistant DLL
//

#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"		// main symbols


// CMFCLineManageAssistantApp
// See MFCLineManageAssistant.cpp for the implementation of this class
//

class CMFCLineManageAssistantApp : public CWinApp
{
public:
	CMFCLineManageAssistantApp();

// Overrides
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
